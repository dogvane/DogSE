using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TradeAge.Server.Database.XmlFile;

namespace TradeAge.Server.Logic
{
    /// <summary>
    /// 数据库访问
    /// </summary>
    public static class DB
    {
        /// <summary>
        /// 游戏数据库，目前用xml文件存储
        /// </summary>
        public static XmlFileService GameDB { get; private set; }

        /// <summary>
        /// 初始化,理论上需要在游戏服务端配置文件加载完成后才能调用
        /// </summary>

        public static void Init()
        {
            GameDB = new XmlFileService();
        }
    }
}
