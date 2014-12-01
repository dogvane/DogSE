using DogSE.Library.Log;

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


        static ServerConfig()
        {
            CheckOfflinePlayerTimeSpan = 60;
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

        /// <summary>
        /// 间隔多久清理一次不在线的玩家
        /// </summary>
        public static int CheckOfflinePlayerTimeSpan { get;set; }

        private static int _playerClearTime = 60;

        /// <summary>
        /// 清理玩家离线的间隔
        /// 单位：秒
        /// 最小 60, 标示玩家离线后，60s数据才有可能被移除Cache
        /// </summary>
        public static int PlayerClearTime
        {
            get { return _playerClearTime; }
            set
            {
                if (value < 60)
                {
                    Logs.Error("PlayerClearTime must big then 60,now = {0}", value);
                    return;
                }
                _playerClearTime = value;
            }
        }
    }

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
}
