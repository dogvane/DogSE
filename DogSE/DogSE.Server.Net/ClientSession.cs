using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Sockets;
using DogSE.Library.Log;
using DogSE.Library.Time;


namespace DogSE.Server.Net
{
    /// <summary>
    /// 客户端的Session
    /// </summary>
    public class ClientSession<T>
    {
        /// <summary>
        /// 和Session关联的对象
        /// </summary>
        public T Data { get; set; }

        /// <summary>
        /// 客户端对应的Socket
        /// </summary>
        internal Socket Socket { get; private set; }

        /// <summary>
        /// 客户端的session的初始化，必须有一个对应的网络连接
        /// </summary>
        /// <param name="socket"></param>
        internal ClientSession(Socket socket)
        {
            if (socket == null)
                throw new ArgumentNullException("socket", "ClientSession create socket don't null.");

            Socket = socket;
            ConnectTime = OneServer.NowTime;
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
        internal DogBuffer32K RecvBuffer { get; set; }

        /// <summary>
        /// 关闭连接
        /// </summary>
        public void CloseSocket()
        {
            if (Socket != null)
            {
                if (Socket.Connected)
                    Socket.Close();
                    //Socket.Disconnect(false);
                    //Socket.BeginDisconnect(true, ir =>
                    //{
                    //    (ir.AsyncState as Socket).EndDisconnect(ir);
                    //}, Socket);
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
            buff.Use();

            m_PendingBuffer.Enqueue(buff);

            PeekSend();
        }

        /// <summary>
        /// 检查队列里是否有要发送的数据，如果有则进行发送处理
        /// </summary>
        void PeekSend()
        {
            if (m_PendingBuffer.Count > 0 && !isSending)
            {
                //  TODO 这里要不要考虑进行并报发送处理
                isSending = true;
                var buff = m_PendingBuffer.Dequeue();
                
                Socket.BeginSend(buff.Bytes, 0, buff.Length, SocketFlags.None, OnSendReturn, buff);
            }
        }

        void OnSendReturn(IAsyncResult result)
        {
            SocketError error;
            var ret = Socket.EndSend(result, out error);
            if (error == SocketError.Success)
            {
                //  发送成功
                var buff = result.AsyncState as DogBuffer;

                if (buff != null)
                {
                    if (ret == buff.Length)
                        buff.Release();
                    else
                        Logs.Error("Async send length not buff len.");
                }

                isSending = false;
                PeekSend();
            }
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
    }
}
