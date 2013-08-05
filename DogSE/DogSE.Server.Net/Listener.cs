using System;
using System.Collections.Concurrent;
using System.Net;
using System.Net.Sockets;
using DogSE.Library.Common;
using DogSE.Library.Log;

namespace DogSE.Server.Net
{
    /// <summary>
    /// 重写的监听器
    /// </summary>
    public class Listener<T>
    {
        private TcpListener serverSocket;

        /// <summary>
        /// 客户端连接上的Session组合
        /// </summary>
        private readonly ConcurrentBag<ClientSession<T>> connectSessions = new ConcurrentBag<ClientSession<T>>();


        /// <summary>
        /// 启动服务器
        /// </summary>
        /// <param name="port"></param>
        public void StartServer(int port)
        {
            StartServer("127.0.0.1", port);
        }

        /// <summary>
        /// 启动服务器
        /// </summary>
        /// <param name="host"></param>
        /// <param name="port"></param>
        public void StartServer(string host, int port)
        {
            if (serverSocket != null)
            {
                return;
            }

            serverSocket = new TcpListener(IPAddress.Parse(host),  port);
            serverSocket.Start(50);
            serverSocket.Server.UseOnlyOverlappedIO = true;
            serverSocket.BeginAcceptSocket(OnSocketAccept, null);
        }

        /// <summary>
        /// 触发socket的连接
        /// </summary>
        /// <param name="result"></param>
        void OnSocketAccept(IAsyncResult result)
        {
            var acceptSocket = serverSocket.EndAcceptSocket(result);
            //  这里在触发socket开始后，又重新抛一个异步连接，
            //  目的是如果下面的操作就算超时，也不会影响socket的正常连接
            serverSocket.BeginAcceptSocket(OnSocketAccept, null);

            if (acceptSocket != null)
            {
                var session = new ClientSession<T>(acceptSocket);
                var ev = SocketConnect;
                if (ev != null)
                {
                    var arg = new SocketConnectEventArgs<T>(session);
                    try
                    {
                        ev(this, arg);
                    }
                    catch (Exception ex)
                    {
                        Logs.Error("On accept socket event error.", ex);
                        arg.AllowConnection = false;
                    }

                    if (!arg.AllowConnection)
                        session.CloseSocket();
                    else
                    {
                        connectSessions.Add(session);
                        session.RecvBuffer = DogBuffer.GetFromPool32K();

                        session.Socket.UseOnlyOverlappedIO = true;
                        session.Socket.BeginReceive(session.RecvBuffer.Bytes, 0, session.RecvBuffer.Bytes.Length,
                                                    SocketFlags.Partial, OnClientRecv, session);
                    }
                }
                else
                {
                    //  如果没有响应连接事件，目前是直接把客户端关闭
                    Logs.Error("Linster SocketConnect event not invoke.");
                    session.CloseSocket();
                }
            }
        }

        /// <summary>
        /// 消息接收处理
        /// </summary>
        /// <param name="ar"></param>
        private void OnClientRecv(IAsyncResult ar)
        {
            var session = ar.AsyncState as ClientSession<T>;
            if (session == null)
                return;

            SocketError error = SocketError.SocketError;
            var len = 0;
            
            if (session.Socket.Connected)
                len = session.Socket.EndReceive(ar, out error);

            if (error == SocketError.Success && len > 0)
            {
                //  正常收到数据
                var buff = session.RecvBuffer;
                buff.Length = len;  //  设置buff的有效长度

                //  在处理逻辑前，先重新抛一个接收的请求到系统，这样就可以及时的收到消息
                //  不必等系统逻辑完成操作后，才能继续接收消息。
                session.RecvBuffer = DogBuffer.GetFromPool32K();    //因为之前的接收缓冲区还有作用，因此这里需要重新申请
                session.Socket.BeginReceive(session.RecvBuffer.Bytes, 0, session.RecvBuffer.Bytes.Length,
                            SocketFlags.Partial, OnClientRecv, session);

                var e = SocketRecv;
                if (e != null)
                {
                    var ev = m_recvEventArgsPool.AcquireContent();
                    ev.Buffer = buff;
                    ev.Session = session;
                    try
                    {
                        e(this, ev);
                    }
                    catch (Exception ex)
                    {
                        Logs.Error("OnSocketRecv event error.", ex);
                    }

                    m_recvEventArgsPool.ReleaseContent(ev);
                }

                buff.Release();
            }
            else
            {
                //  发生意外情况,socket将需要被关闭
                if (session.Socket.Connected)
                    session.Socket.Close();

                if (connectSessions.TryTake(out session))
                {
                    //  触发关闭连接事件
                    var e = SocketDisconnect;
                    if (e != null)
                    {
                        try
                        {
                            e(this, new SocketDisconnectEventArgs<T>(session));
                        }
                        catch (Exception ex)
                        {
                            Logs.Error("On socket close event error.", ex);
                        }
                    }
                }
                
            }
        }

        private readonly ObjectPool<SocketRecvEventArgs<T>> m_recvEventArgsPool = new ObjectPool<SocketRecvEventArgs<T>>(1024 * 8);

        /// <summary>
        /// 关闭所有的客户端连接
        /// </summary>
        public void DisconnectAll()
        {
            var sessions = connectSessions.ToArray();
            foreach (var s in sessions)
                s.CloseSocket();
        }

        /// <summary>
        /// 关闭并停止服务器的socket操作
        /// </summary>
        public void Close()
        {
            DisconnectAll();
            serverSocket.Stop();
        }


        /// <summary>
        /// 当有客户端socket连接上服务器时，触发当前事件
        /// </summary>
        public event EventHandler<SocketConnectEventArgs<T>> SocketConnect;


        /// <summary>
        /// socket发生关闭连接事件
        /// </summary>
        /// <remarks>
        /// 不管是客户端主动关闭，还是客户端关闭，事件都是会触发到的
        /// </remarks>
        public event EventHandler<SocketDisconnectEventArgs<T>> SocketDisconnect;

        /// <summary>
        /// socket有数据送达
        /// </summary>
        public event EventHandler<SocketRecvEventArgs<T>> SocketRecv;

    }

}
