using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
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
        /// 
        /// </summary>
        ~ClientSession()
        {
            //Console.WriteLine("ClientSession dispone()");
        }


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

            RecvBuffer = new DogBuffer();
            ReceiveEventArgs = new SocketAsyncEventArgs();
            ReceiveEventArgs.SetBuffer(RecvBuffer.Bytes, 0, RecvBuffer.Bytes.Length);

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
        internal DogBuffer RecvBuffer { get; private set; }

        /// <summary>
        /// 重新异步开始接受数据
        /// </summary>
        internal void SyncRecvData()
        {
            if (Socket == null || !Socket.Connected)
                return;

            ReceiveEventArgs.SetBuffer(0, RecvBuffer.Bytes.Length);

            //  好吧，会有一定概率，在通过第一行的验证后，socket被关闭，然后Socket被设置为空
            if (Socket != null)
                Socket.ReceiveAsync(ReceiveEventArgs);            
        }

        /// <summary>
        /// 只关闭Socket
        /// </summary>
        public void CloseSocket()
        {
            if (Socket != null)
            {
                if (Socket.Connected)
                    Socket.Close();

                Socket = null;
            }
        }

        /// <summary>
        /// 关闭连接
        /// </summary>
        public void Close()
        {
            if (Socket != null)
            {
                if (Socket.Connected)
                    Socket.Close();

                Socket.Dispose();
                Socket = null;
            }

            if (SendEventArgs != null)
            {
                SendEventArgs.Completed -= OnSendCompleted;
                SendEventArgs.UserToken = null;
                SendEventArgs.Dispose();
            }

            SendEventArgs = null;

            if (ReceiveEventArgs != null)
            {
                ReceiveEventArgs.UserToken = null;
                ReceiveEventArgs.Dispose();
            }

            ReceiveEventArgs = null;

            //  清理发送缓冲区
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
        /// <param name="isSendNow">是否立即发送</param>
        public void SendPackage(DogBuffer buff, bool isSendNow = true)
        {
            if (Socket == null || !Socket.Connected)
                return;

            if (buff.Length == 0)
                throw new Exception("buff lenght is zero.");

            buff.Use();

            lock (m_PendingBuffer)
            {
                m_PendingBuffer.Enqueue(buff);
            }

            if (isSendNow)
                PeekSend();
        }

        /// <summary>
        /// 检查队列里是否有要发送的数据，如果有则进行发送处理
        /// </summary>
        public void PeekSend()
        {
            lock (m_PendingBuffer)
            {
                if (isSending || m_PendingBuffer.Count == 0)
                    return;

                //  TODO 这里要不要考虑进行并报发送处理
                isSending = true;

                if (m_PendingBuffer.Count > 1)
                {
                    //   2 个包以上，进行拼包后再发送
                    var buffs = m_PendingBuffer.ToArray();
                    m_PendingBuffer.Clear();

                    int offSet = 0;
                    foreach (var b in buffs)
                        offSet += b.Length;

                    DogBuffer sendBuff;

                    if (offSet < 4000)
                        sendBuff = DogBuffer.GetFromPool4K();
                    else
                        sendBuff = DogBuffer.GetFromPool32K();

                    if (offSet >= sendBuff.Bytes.Length)
                        sendBuff.UpdateCapacity(offSet);

                    foreach (var buff in buffs)
                    {
                        Buffer.BlockCopy(buff.Bytes, 0, sendBuff.Bytes, sendBuff.Length, buff.Length);
                        sendBuff.Length += buff.Length;

                        buff.Release();
                    }

                    SendEventArgs.UserToken = sendBuff;
                    SendEventArgs.SetBuffer(sendBuff.Bytes, 0, sendBuff.Length);
                    Socket.SendAsync(SendEventArgs);
                }
                else
                {
                    DogBuffer buff;
                    buff = m_PendingBuffer.Dequeue();
                    SendEventArgs.UserToken = buff;
                    SendEventArgs.SetBuffer(buff.Bytes, 0, buff.Length);
                    Socket.SendAsync(SendEventArgs);
                }
            }
        }

        void OnSendCompleted(object sender, SocketAsyncEventArgs e)
        {
            try
            {
                var buff = e.UserToken as DogBuffer;
                if (buff != null)
                {
                    NetProfile.Instatnce.SendCount++;
                    NetProfile.Instatnce.SendLength += buff.Length;

                    buff.Release();
                }
#if DEBUG
                else
                {
                    Logs.Error("OnSendCompleted 的 userToken 竟然不是 DogBuffer 类型");
                }
#endif
                if (e.BytesTransferred == 0 || e.SocketError != SocketError.Success)
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
            catch (Exception ex)
            {
                Logs.Error(ex.ToString());
            }

        }

        /// <summary>
        /// 远程的地址
        /// </summary>
        public string RemoteOnlyIP
        {
            get
            {

                if (Socket != null)
                {
                    var ipPoint = Socket.RemoteEndPoint as IPEndPoint;
                    if (ipPoint != null)
                    {
                        return ipPoint.Address.ToString();
                    }
                }
                return string.Empty;
            }
        }

        /// <summary>
        /// 远程关联的端口
        /// </summary>
        public int RemotePort
        {
            get
            {
                if (Socket != null)
                {
                    var ipPoint = Socket.RemoteEndPoint as IPEndPoint;
                    if (ipPoint != null)
                    {
                        return ipPoint.Port;
                    }
                }
                return -1;
            }
        }
    }
}
