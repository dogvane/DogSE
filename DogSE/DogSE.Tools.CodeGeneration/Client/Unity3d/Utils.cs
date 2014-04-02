using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DogSE.Tools.CodeGeneration.Client.Unity3d
{
    class Utils
    {

        /// <summary>
        /// 给方法名之前加一个on
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string GetFixBeCallProxyName(string name)
        {
            string ret = name;
            if (name.Substring(2).ToLower() != "on")
            {
                ret = "On" + name;
            }

            //if (name.ToLower().EndsWith("result"))
            //{
            //    ret = ret.Substring(0, ret.Length - "result".Length);
            //}

            return ret;
        }

        /// <summary>
        /// 取消方法名前面的on
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string GetFixCallProxyName(string name)
        {
            if (name.Substring(0, 2).ToLower() == "on")
                return name.Substring(2, name.Length - 2);
            return name;
        }

        /// <summary>
        /// 取消接口前面的 i
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string GetFixInterfaceName(string name)
        {
            if (name[0].ToString().ToLower() == "i")
                return name.Substring(1, name.Length - 1);
            return name;
        }

        /// <summary>
        /// 将 .Server. 替换为 .Client.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string GetFixFullTypeName(string name)
        {
            return name.Replace(".Server.", ".Client.");
        }

    }
}
