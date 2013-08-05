using DogSE.Library.Log;
using DogSE.Server.Core.LogicModule;
using DogSE.Server.Core.Task;

namespace DogSE.Server.Core.Protocol
{
    /// <summary>
    /// 注册网络方法到游戏系统里
    /// </summary>
    internal class RegisterNetMethod
    {
        private readonly PacketHandlersBase packetHandlerManager;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="w"></param>
        public RegisterNetMethod(PacketHandlersBase w)
        {
            packetHandlerManager = w;
        }

        /// <summary>
        /// 将方法注册到消息系统里
        /// </summary>
        /// <param name="module"></param>
        public void Register(ILogicModule[] module)
        {
            foreach(var m in module)
            {
                Register(m);
            }
        }

        /// <summary>
        /// 将方法注册到消息系统里
        /// </summary>
        /// <param name="module"></param>
        void Register(ILogicModule module)
        {
            var type = module.GetType();

            var createCode = new CreateReadCode(type);
            var proxy = createCode.CreateCodeAndBuilder();
            if (proxy == null)
            {
                Logs.Error("模块 {0} 无法进行消息代理生成。", type.Name);
                return;
            }

            proxy.PacketHandlerManager = packetHandlerManager;
            proxy.SetModule(module);
            proxy.Init();
        }

    }
}
