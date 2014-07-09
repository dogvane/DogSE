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
    [StaticXmlConfigRootAttribute(@"..\Server.Config")]
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

        /// <summary>
        /// 服务器id
        /// </summary>
        public static int ServerId { get; set; }

        /// <summary>
        /// 日志等级
        /// </summary>
        /// <remarks>
        /// 注意，这里只是负责从配置文件里读取
        /// 真正配置日志的时候，需要把这个字符串转为对应的枚举值
        /// </remarks>
        public static string LogLevel { get; set; }
    }
}
