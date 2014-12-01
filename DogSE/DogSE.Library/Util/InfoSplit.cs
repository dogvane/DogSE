using System;
using System.Collections.Generic;

namespace DogSE.Library.Util
{
    /// <summary>
    /// 信息切割工具类
    /// </summary>
    public class InfoSplit
    {
        /// <summary>
        /// 信息切割类，用来切割
        /// xx:u1;yy:u2
        /// 这样的字符串
        /// </summary>
        /// <param name="context"></param>
        /// <param name="split1"></param>
        /// <param name="split2"></param>
        public InfoSplit(string context, char split1 = ';', char split2 = ':')
        {
            foreach (var data in context.Split(new[] {split1}, StringSplitOptions.RemoveEmptyEntries))
            {
                var d2 = data.Split(split2);
                if (d2.Length > 1)
                    map[d2[0]] = d2[1];
            }
        }

        private readonly Dictionary<string, string> map = new Dictionary<string, string>();

        /// <summary>
        /// 获得某个字符串
        /// </summary>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public string GetString(string key, string defaultValue = "")
        {
            string ret;
            if (map.TryGetValue(key, out ret))
                return ret;

            return defaultValue;
        }

        /// <summary>
        /// 获得某个int值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public int GetInteger(string key, int defaultValue = 0)
        {
            string ret;
            if (map.TryGetValue(key, out ret))
            {
                int intRet;
                if (int.TryParse(ret, out intRet))
                    return intRet;
            }

            return defaultValue;
        }
    }
}
