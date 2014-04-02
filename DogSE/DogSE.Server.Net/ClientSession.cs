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
        ///  唯一id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 发送数据上下文对象
        /// </summary>
        internal SocketAsyncEventArgs SendEventArgs { get; private set; }
        /// <summary>
        /// 接收数据上下文对象
        /// </summary>
        internal SocketAsyncEventArgs ReceiveEventArgs { get; private set; }

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
            SendEventArgs = new SocketAsyncEventArgs();
            SendEventArgs.Completed += OnSendCompleted;

            ReceiveEventArgs = new SocketAsyncEventArgs();

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
        internal DogBuffer32K RecvBuffer { get; private set; }

        /// <summary>
        /// 重新异步开始接受数据
        /// </summary>
        internal void SyncRecvData()
        {
            var buff = DogBuffer.GetFromPool32K();
            RecvBuffer = buff;
            ReceiveEventArgs.SetBuffer(buff.Bytes, 0, buff.Bytes.Length);
            Socket.ReceiveAsync(ReceiveEventArgs);
        }

        /// <summary>
        /// 关闭连接
        /// </summary>
        public void CloseSocket()
        {
            if (Socket != null)
            {
                if (Socket.Connected)
                    Socket.Close();

                Socket = null;
            }

            if (RecvBuffer != null)
                RecvBuffer.Release();

            SendEventArgs = null;
            ReceiveEventArgs = null;
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
            buff.Use();

            lock (m_PendingBuffer)
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
            lock (m_PendingBuffer)
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
    }
}
