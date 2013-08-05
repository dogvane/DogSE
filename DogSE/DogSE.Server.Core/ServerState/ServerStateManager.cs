using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace DogSE.Server.Core.ServerState
{
    /// <summary>
    /// 服务器状态管理器
    /// </summary>
    public static class ServerStateManager
    {
        /// <summary>
        /// 当前系统加载的逻辑模块
        /// </summary>
        private static readonly List<IServerState> s_modules = new List<IServerState>();

        /// <summary>
        /// 注册一个服务器状态监控模块
        /// </summary>
        public static void Register(IServerState module)
        {
            var oldModule = s_modules.FirstOrDefault(o => o.ModuleId == module.ModuleId);
            if (oldModule != null)
                s_modules.Remove(oldModule);

            s_modules.Add(module);
        }

        /// <summary>
        /// 写数据
        /// </summary>
        /// <returns></returns>
        public static string WriterSimpleStates()
        {
            var buff = new StringBuilder(s_modules.Count*32*1024);
            using (var memStream = new MemoryStream(s_modules.Count * 32 * 1024))
            {
                var writer = new StreamWriter(memStream);
                foreach (var module in s_modules.ToArray())
                {
                    buff.AppendFormat("[{0}]", module.ModuleId);
                    buff.AppendLine();
                    module.AppendSimpleState(writer);
                    buff.AppendLine();
                }

                memStream.Position = 0;
                return new StreamReader(memStream).ReadToEnd();
            }
        }

        /// <summary>
        /// 写数据
        /// </summary>
        /// <returns></returns>
        public static string WriterFullStates()
        {
            var buff = new StringBuilder(s_modules.Count * 32 * 1024);
            using (var memStream = new MemoryStream(s_modules.Count * 32 * 1024))
            {
                var writer = new StreamWriter(memStream);
                foreach (var module in s_modules.ToArray())
                {
                    buff.AppendFormat("[{0}]", module.ModuleId);
                    buff.AppendLine();
                    module.AppendFullState(writer);
                    buff.AppendLine();
                }

                memStream.Position = 0;
                return new StreamReader(memStream).ReadToEnd();
            }
        }
    }
}
