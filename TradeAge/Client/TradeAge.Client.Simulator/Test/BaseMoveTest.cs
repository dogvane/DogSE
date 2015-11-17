using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using DogSE.Library.Time;
using TradeAge.Client.Entity.Common;

namespace TradeAge.Client.Simulator.Test
{
    /// <summary>
    /// 简单的移动测试
    /// </summary>
    class BaseMoveTest
    {

        private SimuPlayer first, second;

        public void Start()
        {
            first = new SimuPlayer();
            first.Start2(Guid.NewGuid().ToString().Substring(0, 3), "111");

            second = new SimuPlayer();
            second.Start2(Guid.NewGuid().ToString().Substring(0, 3), "111");


            while(!(first.IsLoginSuccess && second.IsLoginSuccess))
            {
                Thread.Sleep(10);
            }

            first.Controller.Scene.Move(OneServer.NowTime, new Vector2(10, 10), new Vector2(2, 1));
        }
    }
}
