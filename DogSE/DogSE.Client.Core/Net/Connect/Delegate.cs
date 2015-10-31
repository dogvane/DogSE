using System;
using System.Net.Sockets;

namespace DogSE.Client.Core.Net
{
    /// <summary>
    /// sokcet连接事件
    /// </summary>
    public class SocketConnectEventArgs<T> : EventArgs
    {
        /// <summary>
        /// 
        /// </summary>
        public SocketConnectEventArgs()
        {
        }

        /// <summary>
        /// 客户端连接的Session
        /// </summary>
        public ClientSession<T> Session { get; internal set; }

        /// <summary>
        /// 是否连接成功
        /// </summary>
        public bool IsConnected { get;internal set; }

        /// <summary>
        /// 如果出错，出错的原因
        /// </summary>
        public SocketError SocketError { get; internal set; }
    }

    /// <summary>
    /// 客户端发生关闭连接事件
    /// </summary>
    public class SocketDisconnectEventArgs<T> : EventArgs
    {
        /// <summary>
        /// 客户端连接的Session
        /// </summary>
        public ClientSession<T> Session { get; internal set; }
    }

    /// <summary>
    /// 数据受到事件
    /// </summary>
    public class SocketRecvEventArgs<T> : EventArgs
    {
        /// <summary>
        /// 客户端Session
        /// </summary>
        public ClientSession<T> Session { get; internal set; }

        /// <summary>
        /// 缓冲区数据
        /// </summary>
        public DogBuffer Buffer { get; internal set; }
    }
}