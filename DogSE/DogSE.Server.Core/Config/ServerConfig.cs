namespace DogSE.Server.Core.Config
{
    /// <summary>
    /// 服务器的配置文件项
    /// </summary>
    /// <remarks>
    /// 如果定义了root节点，则节点下的静态属性才会进行反射
    /// 如果属性也是一个对象，而非数值，
    /// 则会创建该对象，对象也将以实例方式存在。
    /// </remarks>
    [XmlConfigRootAttribute(@"..\Server.Config")]
    public static class ServerConfig
    {
        /// <summary>
        /// 服务器的tcp配置
        /// </summary>
        public class TcpConfig
        {
            /// <summary>
            /// 主机对外的地址
            /// </summary>
            public string Host { get; set; }

            /// <summary>
            /// 开放的端口号
            /// </summary>
            public int Port { get; set; }
        }

        /// <summary>
        /// 
        /// </summary>
        static public TcpConfig[] Tcp { get; set; }
    }
}
