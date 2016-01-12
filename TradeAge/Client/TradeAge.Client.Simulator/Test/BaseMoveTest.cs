using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using DogSE.Library.Time;

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

            t = new Thread(() =>
            {
                DateTime lastTime = OneServer.NowTime;

                while (true)
                {
                    var updateTime = (float)(OneServer.NowTime - lastTime).TotalSeconds;

                    var nearSprite = first.FindNearSprite();
                    if (nearSprite != null)
                    {
                        
                    }

                    //  30fps间隔
                    Thread.Sleep(33);
                }
            });


            //first.Controller.Scene.Move(OneServer.NowTime, new Vector2(10, 10), new Vector2(2, 1));
        }

        //  缓存线程T，免得被gc回收
        private Thread t;
    }
}
