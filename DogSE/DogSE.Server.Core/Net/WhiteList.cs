using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using DogSE.Library.Log;

namespace DogSE.Server.Core.Net
{
    /// <summary>
    /// 白名单
    /// </summary>
    public static class WhiteList
    {
        static WhiteList()
        {
            IsEnable = false;
        }

        /// <summary>
        /// 是否开启白名单
        /// 默认为flase
        /// </summary>
        public static bool IsEnable { get; set; }

        private static readonly HashSet<string> whiltes = new HashSet<string>();

        /// <summary>
        /// 判断ip是否在白名单里
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        public static bool Contains(string ip)
        {
            return whiltes.Contains(ip);
        }

        /// <summary>
        /// 追加一批白名单
        /// </summary>
        /// <param name="ips"></param>
        public static void Append(params string[] ips)
        {
            foreach (var ip in ips)
                whiltes.Add(ip);
        }

        /// <summary>
        /// 清理白名单里的数据
        /// </summary>
        public static void Clear()
        {
            whiltes.Clear();
        }

        /// <summary>
        /// 加载白名单的配置文件
        /// </summary>
        /// <remarks>
        /// 文件为文本文件
        /// 字符串格式的ip地址 192.168.1.1
        /// 每行一个ip
        /// </remarks>
        public static void LoadWhiteListFile(string fileName = "ipwhitelist.txt")
        {
            if (!File.Exists(fileName))
            {
                Logs.Error("not find white list file:{0}", fileName);
                return;
            }

            var ips = File.ReadAllLines(fileName);
            whiltes.Clear();

            foreach (var ip in ips)
            {
                if (!string.IsNullOrEmpty(ip))
                    whiltes.Add(ip);
            }

            Logs.Info("white list load count:{0}", whiltes.Count);
        }
    }
}
