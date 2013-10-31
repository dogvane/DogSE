using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using DogSE.Server.Core.Config;

namespace TradeAge.Server.Database
{
    [StaticXmlConfigRoot(@"..\Server.Config")]
    public static class DatabaseConfig
    {
        /// <summary>
        /// 游戏服务器连接字符串
        /// </summary>
        public static string GameDbConnectString { get; set; }
    }
}
