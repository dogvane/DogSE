using System;
using System.Collections.Generic;
using DogSE.Library.Log;
using DogSE.Server.Core.Net;

namespace DogSE.Server.Core.Common
{
    /// <summary>
    /// GM 命令行管理类
    /// </summary>
    public static class GMCommand
    {
        /// <summary>
        /// 命令字典
        /// </summary>
        private static readonly Dictionary<string, Func<NetState, string[], bool>> map =
            new Dictionary<string, Func<NetState, string[], bool>>();

        /// <summary>
        /// 添加一个网络触发的gm命令
        /// 不用考虑注销，如果重新调用，会替换之前的引用
        /// </summary>
        /// <param name="name"></param>
        /// <param name="fun"></param>
        static public void AddNetCommand(string name, Func<NetState, string[], bool> fun)
        {
            map[name.ToLower()] = fun;
        }

        /// <summary>
        /// 获得一个gm指令对应的方法
        /// </summary>
        /// <param name="commandName"></param>
        /// <returns></returns>
        public static Func<NetState, string[], bool> GetCommandFun(string commandName)
        {
            Func<NetState, string[], bool> ret;
            if (map.TryGetValue(commandName.ToLower(), out ret))
            {
                return ret;
            }

            Logs.Error("not find gm command {0}", commandName);
            return null;
        }

        /// <summary>
        /// 注册一个控制台的GM指令
        /// 方法同 GameServerService.RegisterConsoleCommand 一致
        /// 只不过多放一个地方，方便调用而已
        /// </summary>
        /// <param name="commandName"></param>
        /// <param name="fun"></param>
        public static void RegisterConsoleCommand(string commandName, GameServerService.CommandCallbackDelegate fun)
        {
            GameServerService.RegisterConsoleCommand(commandName, fun);
        }
    }
}
