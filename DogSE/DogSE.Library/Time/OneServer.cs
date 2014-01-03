using System;
using System.Diagnostics;

namespace DogSE.Library.Time
{
    /// <summary>
    /// 暂时用这个来获得全部时间，稍后会对它的获取方法做调整
    /// </summary>
    public static class OneServer
    {
        /// 服务器当前的时间
        private static DateTime s_NowTime = DateTime.Now;

        // 用于计算经过的时间（因为Stopwatch的计算速度比DateTime.Now快近3倍）
        private static Stopwatch s_UpdateTime = Stopwatch.StartNew();


        /// <summary>
        /// 服务器当前的时间
        /// </summary>
        public static DateTime NowTime { get { return s_NowTime + s_UpdateTime.Elapsed; } }

        /// <summary>
        /// 服务器是否处于关闭状态
        /// </summary>
        public static bool Closing { get; set; }
    }
}
