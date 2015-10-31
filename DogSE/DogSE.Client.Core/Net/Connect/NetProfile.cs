using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DogSE.Client.Core.Net
{
    /// <summary>
    /// 网络层的性能
    /// </summary>
    public class NetProfile
    {
        /// <summary>
        /// 发送的次数
        /// </summary>
        public long SendCount { get; set; }

        /// <summary>
        /// 发送包的长度
        /// </summary>
        public long SendLength { get; set; }

        /// <summary>
        /// 接收的次数
        /// </summary>
        public long RecvCount { get; set; }

        /// <summary>
        /// 接收的长度
        /// </summary>
        public long RecvLength { get; set; }

        /// <summary>
        /// 连接上来的数量
        /// </summary>
        public long AcceptCount { get; set; }

        /// <summary>
        /// 断开的数量
        /// </summary>
        public long DisconnectCount { get; set; }

        static private NetProfile s_instance = new NetProfile();

        /// <summary>
        /// 单例
        /// </summary>
        public static NetProfile Instatnce
        {
            get
            {
                return s_instance;
            }
        }
    }
}
