using System;
using System.Threading;
using DogSE.Library.Log;
using DogSE.Server.Core.Timer;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DogSE.Server.Core.UnitTest.Timer
{
    /// <summary>
    /// 时间片测试
    /// </summary>
    [TestClass]
    public class TimerSliceTest
    {
        private WorldBase w = null;
        /// <summary>
        /// 初始化
        /// </summary>
        [TestInitialize]
        public void StartWorld()
        {
            //Logs.AddAppender(new ConsoleAppender());
            //Logs.SetMessageLevel<ConsoleAppender>(LogMessageType.MSG_DEBUG);

            //Logs.AddAppender(new DebugAppender());
            //Logs.SetMessageLevel<DebugAppender>(LogMessageType.MSG_DEBUG);

            w = new WorldBase();
            w.StartWorld();
        }

        /// <summary>
        /// 
        /// </summary>
        [TestMethod]
        public void Test1()
        {
            //  因为任务队列和时间调度和测试程序不在一个线程里
            //  所以，在这里可以直接用Thead.Sleep()进行睡眠
            bool isCall = false;
            TimeSlice.StartTimeSlice(TimeSpan.FromMilliseconds(10), () =>
            {
                isCall = true;
            });

            //  这里比任务队列多延迟那么一点点
            Thread.Sleep(20);
            Assert.IsTrue(isCall);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <remarks>
        /// 10次调用，调用后就结束
        /// 每次间隔10ms
        /// </remarks>
        [TestMethod]
        public void Test2()
        {
            int count = 0;
            bool isStopEvent = false;
            var timeSlice = TimeSlice.StartTimeSlice(TimeSpan.Zero, TimeSpan.FromMilliseconds(10), 10, () => { count++; });
            timeSlice.StopTimeSlice += (o, e) => { isStopEvent = true; };

            Thread.Sleep(10*10 + 10);
            Assert.AreEqual(10, count);
            Assert.IsTrue(isStopEvent);
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <remarks>
        /// 定时器时间间隔10ms
        /// 执行100ms
        /// </remarks>
        [TestMethod]
        public void Test3()
        {
            int count = 0;
            bool isStopEvent = false;
            var timeSlice = TimeSlice.StartTimeSlice(TimeSpan.Zero, TimeSpan.FromMilliseconds(10), TimeSpan.FromMilliseconds(10 * 10), () => { count++; });
            timeSlice.StopTimeSlice += (o, e) => { isStopEvent = true; };

            Thread.Sleep(10 * 10 + 50);

            //  这里是11次，是因为在调用定时器时，没有起始时间间隔
            //  所以，一开始会执行一次
            //  在100ms时间范围内被执行10次，所以，总次数为11次
            //  如果今后要解决类似问题
            //  要么是增加起始时间，要么，减少总时间。
            //  推荐，增加起始起始时间
            Assert.AreEqual(11, count);
            Assert.IsTrue(isStopEvent);
        }
    }
}
