using System;

namespace DogSE.Library.Time
{
    /// <summary>
    /// 暂时用这个来获得全部时间，稍后会对它的获取方法做调整
    /// </summary>
    public static class OneServer
    {
        private static TimeSpan span = TimeSpan.Zero;


        /// <summary>
        /// 服务器当前的时间
        /// </summary>
        public static DateTime NowTime { get { return DateTime.Now.Add(span); } }

        /// <summary>
        /// 获得服务器时间
        /// </summary>
        /// <returns></returns>
        public static string GetServerTime()
        {
            return NowTime.ToString("HH:mm:ss " + span.TotalSeconds);
        }

        /// <summary>
        /// 设置服务器时间
        /// </summary>
        /// <param name="serverTime"></param>
        public static void SetServerTime(DateTime serverTime)
        {
            span = serverTime - DateTime.Now;
        }
    }
}
