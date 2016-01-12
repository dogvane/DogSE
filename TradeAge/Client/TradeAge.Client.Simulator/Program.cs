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

            //test1();
            //test2();
            TestMove();
            
            while (true)
            {
                if (Console.ReadKey(false).Key == ConsoleKey.Escape)
                    break;
                Thread.Sleep(100);
            }

            GameServerService.RunType = ServerStateType.Closing;
        }

        static void test1()
        {

            var c = new BaseLoginTest();


            var userName = Guid.NewGuid().ToString().Substring(0, 4);
            c.Start(userName, "111");

        }

        static void test2()
        {
            var c = new BaseMoveTest();
            c.Start();

        }

        static void TestMove()
        {
            var bm = new BaseMoveTest();
            bm.Start();
        }
    }

}
