using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using DogSE.Library.Log;
using DogSE.Library.Time;


namespace DogSE.Client.Core.Net
{
    /// <summary>
    /// 客户端的Session
    /// </summary>
    public class ClientSession<T>
    {

        /// <summary>
        /// 发送数据上下文对象
        /// </summary>
        internal SocketAsyncEventArgs SendEventArgs { get; private set; }
        /// <summary>
        /// 接收数据上下文对象
        /// </summary>
        internal SocketAsyncEventArgs ReceiveEventArgs { get; private set; }

        /// <summary>
        /// 接收数据上下文对象
        /// </summary>
        internal SocketAsyncEventArgs ConnectEventArgs { get; private set; }


        /// <summary>
        /// 和Session关联的对象
        /// </summary>
        public T Data { get; set; }

        /// <summary>
        /// 客户端对应的Socket
        /// </summary>
        internal Socket Socket { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public ClientSession()
        {
            Init();
        }

#if DEBUG
        /// <summary>
        /// 
        /// </summary>
        ~ ClientSession()
        {
            Logs.Info("ClientSession release");
        }
#endif

        void Init()
        {
            SendEventArgs = new SocketAsyncEventArgs();
            SendEventArgs.Completed += OnSendCompleted;

            ReceiveEventArgs = new SocketAsyncEventArgs();
            ReceiveEventArgs.Completed += OnRecvCompleted;

            ConnectEventArgs = new SocketAsyncEventArgs();
            ConnectEventArgs.Completed += OnConnectCompleted;
        }

        /// <summary>
        /// 连接上的时间
        /// </summary>
        public DateTime ConnectTime { get; private set; }

        private readonly Stopwatch m_upTime = new Stopwatch();

        /// <summary>
        /// 在线时间
        /// </summary>
        public TimeSpan OnlineTime { get { return m_upTime.Elapsed; }}

        /// <summary>
        /// 接收的缓冲区数据，仅用于同底层数据交换时用
        /// </summary>
        internal DogBuffer32K RecvBuffer { get; private set; }

        /// <summary>
        /// 重新异步开始接受数据
        /// </summary>
        internal void SyncRecvData()
        {
            if (isClose || Socket == null || !Socket.Connected)
                return;

#if !UNITY_IPHONE
            lock (Socket)
#endif
            {
                var buff = DogBuffer.GetFromPool32K();
                RecvBuffer = buff;
                ReceiveEventArgs.SetBuffer(RecvBuffer.Bytes, 0, RecvBuffer.Bytes.Length);
                Socket.ReceiveAsync(ReceiveEventArgs);
            }
        }

        private bool isClose = false;

        /// <summary>
        /// 只关闭socket
        /// </summary>
        public void CloseSocket()
        {
            isClose = true;
            if (Socket != null)
            {
#if !UNITY_IPHONE
                lock (Socket)
#endif
                {
                    if (Socket.Connected)
                        Socket.Close();
                }
                Socket = null;
            }
        }

        /// <summary>
        /// 关闭连接
        /// </summary>
        public void Close()
        {
            SendEventArgs.Completed -= OnSendCompleted;
            ReceiveEventArgs.Completed -= OnRecvCompleted;
            ConnectEventArgs.Completed -= OnConnectCompleted;

            CloseSocket();

            if (RecvBuffer != null)
                RecvBuffer.Release();

            SendEventArgs = null;
            ReceiveEventArgs = null;
            ConnectEventArgs = null;

            if (m_PendingBuffer.Count > 0)
            {
                foreach (var buff in m_PendingBuffer)
                {
                    buff.Release();
                }

                m_PendingBuffer.Clear();
            }
        }


        /// <summary>
        /// 等待需要发出的数据
        /// </summary>
        private readonly Queue<DogBuffer> m_PendingBuffer = new Queue<DogBuffer>(32);

        private bool isSending;

        /// <summary>
        /// 向客户端发送数据
        /// </summary>
        /// <param name="buff"></param>
        public void SendPackage(DogBuffer buff)
        {
            if (Socket == null || !Socket.Connected)
                return;

            buff.Use();

#if !UNITY_IPHONE
            lock (m_PendingBuffer)
#endif
            {
                m_PendingBuffer.Enqueue(buff);
            }

            PeekSend();
        }

        /// <summary>
        /// 检查队列里是否有要发送的数据，如果有则进行发送处理
        /// </summary>
        private void PeekSend()
        {
            if (isClose)
                return;

#if !UNITY_IPHONE
            lock (m_PendingBuffer)
#endif
            {
                if (isSending || m_PendingBuffer.Count == 0)
                    return;

                //  TODO 这里要不要考虑进行并报发送处理
                isSending = true;

                DogBuffer buff;
                buff = m_PendingBuffer.Dequeue();
                SendEventArgs.UserToken = buff;
                SendEventArgs.SetBuffer(buff.Bytes, 0, buff.Length);
                Socket.SendAsync(SendEventArgs);
            }
        }

        void OnSendCompleted(object sender, SocketAsyncEventArgs e)
        {
            var buff = e.UserToken as DogBuffer;
            if (buff != null)
            {
                NetProfile.Instatnce.SendCount++;
                NetProfile.Instatnce.SendLength += buff.Length;

                buff.Release();
            }

            if (e.BytesTransferred == 0)
            {
                //  发送传输为0，目标方应该断开连接了，这里进入断开环节。
                isSending = false;
                CloseSocket();
                return;
            }

            //  发送完成后，再检查一下还有没有没发送的接着发送
            isSending = false;
            PeekSend();
        }

        /// <summary>
        /// 远程的地址
        /// </summary>
        public string RemoteOnlyIP
        {
            get { return Socket.RemoteEndPoint.ToString(); }
        }

        /// <summary>
        /// 远程关联的端口
        /// </summary>
        public int RemotePort
        {
            get { return 0; }
        }

        void OnRecvCompleted(object sender, SocketAsyncEventArgs e)
        {
            if (e.BytesTransferred == 0)
            {
                //  传输为0，表示客户端已经被关闭,触发断开事件
                NetProfile.Instatnce.DisconnectCount++;

                //  触发关闭连接事件
                var disconnectTemp = SocketDisconnect;
                if (disconnectTemp != null)
                {
                    var arg = new SocketDisconnectEventArgs<T>();
                    arg.Session = this;

                    try
                    {
                        disconnectTemp(this, arg);
                    }
                    catch (Exception ex)
                    {
                        Logs.Error("On socket close event error.", ex);
                    }

                    //  清理工作由内部的发送缓冲检查出错误后内部进行处理
                }
            }
            else
            {
                //  正常收到数据
                var buff = RecvBuffer;
                buff.Length = e.BytesTransferred;  //  设置buff的有效长度


                NetProfile.Instatnce.RecvCount++;
                NetProfile.Instatnce.RecvLength += buff.Length;

                var recvTemp = SocketRecv;
                if (recvTemp != null)
                {
                    var ev = new SocketRecvEventArgs<T>();

                    ev.Buffer = buff;
                    ev.Session = this;
                    try
                    {
                        recvTemp(this, ev);
                    }
                    catch (Exception ex)
                    {
                        Logs.Error("OnSocketRecv event error.", ex);
                    }
                }

                buff.Release();

                //  在处理完消息包后，再进行投递
                //  因为在这之前是先投递再执行数据操作
                //  这样会存在一定的多线程并发下的错误
                //  因为数据处理的逻辑是解包头，然后把包数据压入消息处理队列
                //  因此先处理包消息，再投递，不会影响包接收的性能
                SyncRecvData();
            }
        }


        void OnConnectCompleted(object sender, SocketAsyncEventArgs e)
        {
            isBeginConnect = false;

            if (e.SocketError != SocketError.Success)
            {
                if (e.AcceptSocket != Socket)
                {
                    Logs.Error("Socket 重复连接错误");
                    Socket = e.AcceptSocket;
                }

                //  连接失败
                if (SocketConnect != null)
                {
                    try
                    {
                        SocketConnect(this, new SocketConnectEventArgs<T>
                        {
                            IsConnected = false,
                            SocketError = e.SocketError,
                            Session = this
                        });
                    }
                    catch (Exception ex)
                    {
                        Logs.Error("OnConnectCompleted event error.", ex);
                    }
                }

                isClose = false;

                return;
            }

            ConnectTime = OneServer.NowTime;
            //  连接成功，发起一次接收数据请求
            SyncRecvData();

            if (SocketConnect != null)
            {
                try
                {
                    SocketConnect(this, new SocketConnectEventArgs<T>
                    {
                        IsConnected = true,
                        Session = this
                    });
                }
                catch (Exception ex)
                {
                    Logs.Error("OnConnectCompleted event error.", ex);
                }
            }
        }

        private bool isBeginConnect = false;

        /// <summary>
        /// 先服务器发起一个连接
        /// 这是一个异步方法
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="port"></param>
        public void BeginConnect(string ip, int port)
        {
            if (isBeginConnect && Socket != null)
            {
                Logs.Error("replace connecting...");
                return;
            }

            isBeginConnect = true;

            Socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPAddress address;
            if (IPAddress.TryParse(ip, out address))
            {
                if (ConnectEventArgs == null)   //  如果之前连接被关闭了，这里会重新对连接的数据做初始化
                    Init();

                ConnectEventArgs.RemoteEndPoint = new IPEndPoint(address, port);
                Socket.ConnectAsync(ConnectEventArgs);
            }
            else
            {
                Logs.Error("ip is error", ip);
            }
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
