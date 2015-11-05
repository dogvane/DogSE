using System;
using System.Collections.Generic;
using System.Windows.Forms;
using DogSE.Library.Log;
using DogSE.Server.Core;
using DogSE.Server.Core.Config;
using DogSE.Server.Core.Protocol.AutoCode;
using TradeAge.Server.Logic;

namespace TradeAge.Server.Game
{
    /// <summary>
    /// 游戏的启动项目，同时也是一个服务器状态的监视窗口
    /// </summary>
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            //Application.EnableVisualStyles();
            //Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new FrmMain());


            ServerConfig.Tcp = new[]
            {
                new TcpConfig
                {
                    Host = "127.0.0.1",
                    Port = 4530,
                }
            };

            Logs.ConfigLogFile("tradeage.log");
            Logs.AddAppender(new ConsoleAppender());

            GameServerService.IsConsoleRun = true;
            var world = new WorldBase();
            world.IsAutoRegisterMessage = false;
            world.NetStateDisconnect += world_NetStateDisconnect;
            DB.Init();

            GameServerService.AfterModuleInit = () =>
            {
                ServerLogicProtoclRegister.Register(world.GetModules(), world.PacketHandlers);
                ClientProxyRegister.Register();
                return true;
            };
            LogicModule.Prints();
            GameServerService.StartGame(world);
        }

        static void world_NetStateDisconnect(object sender, NetStateDisconnectEventArgs e)
        {
            //  网络连接断开
            if (e.NetState != null)
            {
                Logs.Info("{0} close socket.", e.NetState.Serial);
            }
        }
    }
}
