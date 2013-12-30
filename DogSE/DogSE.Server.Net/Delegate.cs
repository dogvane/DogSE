using System;

namespace DogSE.Server.Net
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
            AllowConnection = true;
        }

        /// <summary>
        /// 客户端连接的Session
        /// </summary>
        public ClientSession<T> Session { get; internal set; }

        /// <summary>
        /// 是否允许连接
        /// </summary>
        public bool AllowConnection { get; set; }
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