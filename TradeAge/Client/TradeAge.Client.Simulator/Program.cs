using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using DogSE.Client.Core;
using DogSE.Client.Core.Timer;
using DogSE.Library.Log;
using TradeAge.Client.Entity.Character;
using TradeAge.Client.Entity.Login;
using TradeAge.Client.Logic.Controller;
using TradeAge.Client.Logic.Controller.Login;

namespace TradeAge.Client.Simulator
{
    class Program
    {
        private static GameController controller;

        private static List<SimulatorMove> pink = new List<SimulatorMove>();
        static void Main(string[] args)
        {
            Logs.ConfigLogFile("TradeAge.log");
            Logs.AddAppender(new ConsoleAppender());

            controller = new GameController();
            controller.Net.NetStateConnect += Net_NetStateConnect;
            controller.Net.ConnectServer("127.0.0.1", 4530);
            pink.Add(new SimulatorMove(controller));

            GameServerService.StartTaskThread();
            
            while (true)
            {
                if (Console.KeyAvailable)
                {
                    var key = Console.ReadKey(false);
                    if (key.Key == ConsoleKey.Escape)
                        break;
                }

                Thread.Sleep(100);
                pink.ForEach(o => o.RandMove());
            }

            NetController.CloseThread();
        }

        private static string playerName = Guid.NewGuid().ToString().Substring(0, 5);
        static void Net_NetStateConnect(object sender, NetStateConnectEventArgs e)
        {
            Console.WriteLine("socket connected:" + e.IsConnected);
            controller.Login.LoginServerRet += Login_LoginServerRet;
            controller.Login.LoginServer(playerName, "123456", 0);


        }

        static void Login_LoginServerRet(object sender, LoginController.LoginServerResultEventArgs e)
        {
            Console.WriteLine("login server result: {0}", e.Result);

            if (e.Result == LoginServerResult.Success && e.IsCreatePlayered == false)
            {
                //  角色没创建过
                controller.Login.CreatePlayerRet += Login_CreatePlayerRet;
                controller.Login.CreatePlayer(playerName, Sex.Male);
            }
        }

        static void Login_CreatePlayerRet(object sender, LoginController.CreatePlayerResultEventArgs e)
        {
            Console.WriteLine("Create player result: {0}", e.Result);
        }
    }
}
