using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DogSE.Server.Core.Config;

namespace DogSE.Server.Database.MangoDB
{
    /// <summary>
    /// MangoDB的客户端配置文件
    /// </summary>
    [StaticXmlConfigRoot(@"..\Server.Config", RootName = "MongodbLog")]
    public static class MongodbLogConfig
    {
        /// <summary>
        /// 数据库地址
        /// </summary>
        public static string Host { get; set; }

        /// <summary>
        /// 数据库名称
        /// </summary>
        public static string Database { get; set; }

        /// <summary>
        /// 日志级别
        /// </summary>
        public static string LogLevel { get; set; }
    }
}
