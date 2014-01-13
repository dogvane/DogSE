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
                    var arg = m_connectArgsPool.AcquireContent();
                    arg.Session = session;
                    arg.AllowConnection = true;

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
                    {
                        session.CloseSocket();  //  如果业务逻辑不允许连接，则自己关闭
                    }
                    else
                    {
                        connectSessions.Add(session);
                        session.Socket.UseOnlyOverlappedIO = true;

                        session.ReceiveEventArgs.UserToken = session;
                        session.ReceiveEventArgs.Completed += OnRecvCompleted;

                        session.SyncRecvData();
                    }

                    m_connectArgsPool.ReleaseContent(arg);  //  事件完成后就进行回收
                }
                else
                {
                    //  如果没有响应连接事件，目前是直接把客户端关闭
                    Logs.Error("Linster SocketConnect event not invoke.");
                    session.CloseSocket();
                }
            }
        }

        void OnRecvCompleted(object sender, SocketAsyncEventArgs e)
        {
            var session = e.UserToken as ClientSession<T>;
            if (session == null)
            {
                Logs.Error("OnRecvCompleted UserToken is not " + typeof(ClientSession<T>).Name);
                return;
            }

            if (e.BytesTransferred == 0)
            {
                //  传输为0，表示客户端已经被关闭

                if (!connectSessions.TryTake(out session))
                {
                    Logs.Error("connectSessions.TryTake(out session) fail.");
                    return;
                }

                //  触发关闭连接事件
                var disconnectTemp = SocketDisconnect;
                if (disconnectTemp != null)
                {
                    var arg = m_disconnectArgsPool.AcquireContent();
                    arg.Session = session;

                    try
                    {
                        disconnectTemp(this, arg);
                    }
                    catch (Exception ex)
                    {
                        Logs.Error("On socket close event error.", ex);
                    }

                    m_disconnectArgsPool.ReleaseContent(arg);

                    //  清理工作由内部的发送缓冲检查出错误后内部进行处理
                }
            }
            else
            {
                //  正常收到数据
                var buff = session.RecvBuffer;
                buff.Length = e.BytesTransferred;  //  设置buff的有效长度

                //  在处理逻辑前，先重新抛一个接收的请求到系统，这样就可以及时的收到消息
                //  不必等系统逻辑完成操作后，才能继续接收消息。
                session.SyncRecvData();

                var recvTemp = SocketRecv;
                if (recvTemp != null)
                {
                    var ev = m_recvEventArgsPool.AcquireContent();
                    ev.Buffer = buff;
                    ev.Session = session;
                    try
                    {
                        recvTemp(this, ev);
                    }
                    catch (Exception ex)
                    {
                        Logs.Error("OnSocketRecv event error.", ex);
                    }

                    m_recvEventArgsPool.ReleaseContent(ev);
                }

                buff.Release();
            }
        }

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

        private readonly ObjectPool<SocketConnectEventArgs<T>> m_connectArgsPool = new ObjectPool<SocketConnectEventArgs<T>>();

        /// <summary>
        /// socket发生关闭连接事件
        /// </summary>
        /// <remarks>
        /// 不管是客户端主动关闭，还是客户端关闭，事件都是会触发到的
        /// </remarks>
        public event EventHandler<SocketDisconnectEventArgs<T>> SocketDisconnect;


        private readonly ObjectPool<SocketDisconnectEventArgs<T>> m_disconnectArgsPool = new ObjectPool<SocketDisconnectEventArgs<T>>();


        /// <summary>
        /// socket有数据送达
        /// </summary>
        public event EventHandler<SocketRecvEventArgs<T>> SocketRecv;


        private readonly ObjectPool<SocketRecvEventArgs<T>> m_recvEventArgsPool = new ObjectPool<SocketRecvEventArgs<T>>(1024 * 8);


    }

}
