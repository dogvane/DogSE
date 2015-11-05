using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using DogSE.Client.Core;
using DogSE.Client.Core.Timer;
using DogSE.Library.Log;
using TradeAge.Client.Controller;
using TradeAge.Client.Simulator.Test;

namespace TradeAge.Client.Simulator
{
    public class Program
    {
        private static void Main(string[] args)
        {
            Logs.AddConsoleAppender();

            GameServerService.StartTaskThread();

            var c = new BaseLoginTest();

            var userName = Guid.NewGuid().ToString().Substring(0, 4);
            c.Start(userName, "111");

            while (true)
            {
                if (Console.ReadKey(false).Key == ConsoleKey.Escape)
                    break;
                Thread.Sleep(100);
            }

            GameServerService.RunType = ServerStateType.Closing;
        }
    }

}
