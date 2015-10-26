using System;

namespace DogSE.Client.Core.Timer
{
    /// <summary>
    /// 时间间隔辅助类
    /// </summary>
    public class TimeSliceUtil
    {

        #region zh-CHS 私有静态成员变量 | en Private Static Member Variables

        /// <summary>
        /// 超过3分钟等待的放入长时间等待队列
        /// </summary>
        private static readonly TimeSpan s_LongTimeCheck = TimeSpan.FromMinutes(3);

        /// <summary>
        /// 1分钟以内的100ms检查一次
        /// </summary>
        private static readonly TimeSpan s_MinuteCheck = TimeSpan.FromSeconds(60);

        /// <summary>
        /// 1s以内的，即时检查
        /// </summary>
        private static readonly TimeSpan s_SecondCheck = TimeSpan.FromSeconds(1);

        #endregion
        /// <summary>
        /// 获取时间片的优先级
        /// </summary>
        /// <param name="timeSpan"></param>
        /// <returns></returns>
        public static TimerFrequency ComputePriority(TimeSpan timeSpan)
        {
            if (timeSpan >= s_LongTimeCheck)
                return TimerFrequency.LongTime;

            if (timeSpan >= s_MinuteCheck)
                return TimerFrequency.Minute;

            if (timeSpan >= s_SecondCheck)
                return TimerFrequency.Second;

            return TimerFrequency.EveryTick;
        }

        /// <summary>
        /// 在 delayTimeSpan 时间结束后调用一次回调函数
        /// </summary>
        /// <param name="delayTimeSpan"></param>
        /// <param name="timerCallback"></param>
        /// <returns></returns>
        public static TimeSlice StartTimeSlice(TimeSpan delayTimeSpan, TimeSliceCallback timerCallback)
        {
            return StartTimeSlice(delayTimeSpan, TimeSpan.Zero, 1, TimeSpan.MaxValue, timerCallback);
        }


        /// <summary>
        /// 从 delayTimeSpan 后开始，每隔 intervalTimeSpan 调用一次回调函数，直到游戏退出
        /// </summary>
        /// <param name="delayTimeSpan"></param>
        /// <param name="intervalTimeSpan"></param>
        /// <param name="timerCallback"></param>
        /// <returns></returns>
        public static TimeSlice StartTimeSlice(TimeSpan delayTimeSpan, TimeSpan intervalTimeSpan, TimeSliceCallback timerCallback)
        {
            return StartTimeSlice(delayTimeSpan, intervalTimeSpan, long.MaxValue, TimeSpan.MaxValue, timerCallback);
        }


        /// <summary>
        /// 从 delayTimeSpan 后开始，每隔 intervalTimeSpan 调用一次回调函数，直到 iTimes 次回调后结束。
        /// </summary>
        /// <remarks>
        /// 如果 iTimes == 1则表示调用一次后就结束
        /// </remarks>
        /// <param name="delayTimeSpan"></param>
        /// <param name="intervalTimeSpan"></param>
        /// <param name="iTimes">调用的次数</param>
        /// <param name="timerCallback">回调方法</param>
        /// <param name="timeLeft">剩余时间</param>
        /// <returns></returns>
        public static TimeSlice StartTimeSlice(TimeSpan delayTimeSpan, TimeSpan intervalTimeSpan, long iTimes, TimeSpan timeLeft, TimeSliceCallback timerCallback)
        {
            TimeSlice timeSlice = new DelayCallTimer(delayTimeSpan, intervalTimeSpan, iTimes, timeLeft, timerCallback);

            timeSlice.Start();

            return timeSlice;
        }


        /// <summary>
        /// 从 delayTimeSpan 后开始，每隔 intervalTimeSpan 调用一次回调函数，直到 iTimes 次回调后结束。
        /// </summary>
        /// <param name="delayTimeSpan"></param>
        /// <param name="intervalTimeSpan"></param>
        /// <param name="iTimes"></param>
        /// <param name="timerStateCallback"></param>
        /// <returns></returns>
        public static TimeSlice StartTimeSlice(TimeSpan delayTimeSpan, TimeSpan intervalTimeSpan, long iTimes, TimeSliceCallback timerStateCallback)
        {
            return StartTimeSlice(delayTimeSpan, intervalTimeSpan, iTimes, TimeSpan.MaxValue, timerStateCallback);
        }

        /// <summary>
        /// 从 delayTimeSpan 后开始，每隔 intervalTimeSpan 调用一次回调函数，直到超过 timeLeft 的时间。
        /// </summary>
        /// <param name="delayTimeSpan"></param>
        /// <param name="intervalTimeSpan"></param>
        /// <param name="timeLeft"></param>
        /// <param name="timerStateCallback"></param>
        /// <returns></returns>
        public static TimeSlice StartTimeSlice(TimeSpan delayTimeSpan, TimeSpan intervalTimeSpan, TimeSpan timeLeft, TimeSliceCallback timerStateCallback)
        {
            return StartTimeSlice(delayTimeSpan, intervalTimeSpan, long.MaxValue, timeLeft, timerStateCallback);
        }

        /// <summary>
        /// 在 delayTimeSpan 时间结束后调用一次回调函数
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="delayTimeSpan"></param>
        /// <param name="timerStateCallback"></param>
        /// <param name="tState"></param>
        /// <returns></returns>
        public static TimeSlice StartTimeSlice<T>(TimeSpan delayTimeSpan, TimeSliceStateCallback<T> timerStateCallback, T tState)
        {
            return StartTimeSlice(delayTimeSpan, TimeSpan.Zero, 1, TimeSpan.MaxValue, timerStateCallback, tState);
        }


        /// <summary>
        /// 从 delayTimeSpan 后开始，每隔 intervalTimeSpan 调用一次回调函数，直到游戏退出
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="delayTimeSpan"></param>
        /// <param name="intervalTimeSpan"></param>
        /// <param name="timerStateCallback"></param>
        /// <param name="tState"></param>
        /// <returns></returns>
        public static TimeSlice StartTimeSlice<T>(TimeSpan delayTimeSpan, TimeSpan intervalTimeSpan, TimeSliceStateCallback<T> timerStateCallback, T tState)
        {
            return StartTimeSlice(delayTimeSpan, intervalTimeSpan, long.MaxValue, TimeSpan.MaxValue, timerStateCallback, tState);
        }


        /// <summary>
        /// 从 delayTimeSpan 后开始，每隔 intervalTimeSpan 调用一次回调函数，直到 iTimes 次回调后结束。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="delayTimeSpan"></param>
        /// <param name="intervalTimeSpan"></param>
        /// <param name="iTimes"></param>
        /// <param name="timerStateCallback"></param>
        /// <param name="tState"></param>
        /// <returns></returns>
        public static TimeSlice StartTimeSlice<T>(TimeSpan delayTimeSpan, TimeSpan intervalTimeSpan, long iTimes, TimeSliceStateCallback<T> timerStateCallback, T tState)
        {
            return StartTimeSlice(delayTimeSpan, intervalTimeSpan, iTimes, TimeSpan.MaxValue, timerStateCallback, tState);
        }



        /// <summary>
        /// 从 delayTimeSpan 后开始，每隔 intervalTimeSpan 调用一次回调函数，直到超过 timeLeft 的时间。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="delayTimeSpan"></param>
        /// <param name="intervalTimeSpan"></param>
        /// <param name="timeLeft"></param>
        /// <param name="timerStateCallback"></param>
        /// <param name="tState"></param>
        /// <returns></returns>
        public static TimeSlice StartTimeSlice<T>(TimeSpan delayTimeSpan, TimeSpan intervalTimeSpan, TimeSpan timeLeft, TimeSliceStateCallback<T> timerStateCallback, T tState)
        {
            return StartTimeSlice(delayTimeSpan, intervalTimeSpan, long.MaxValue, timeLeft, timerStateCallback, tState);
        }


        /// <summary>
        /// 带优先级从 delayTimeSpan 后开始，每隔 intervalTimeSpan 调用一次回调函数，直到超过 timeLeft 的时间或者回调次数达到 iTimes。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="delayTimeSpan"></param>
        /// <param name="intervalTimeSpan"></param>
        /// <param name="iTimes"></param>
        /// <param name="timeLeft"></param>
        /// <param name="timerStateCallback"></param>
        /// <param name="tState"></param>
        /// <returns></returns>
        public static TimeSlice StartTimeSlice<T>(TimeSpan delayTimeSpan, TimeSpan intervalTimeSpan, long iTimes, TimeSpan timeLeft, TimeSliceStateCallback<T> timerStateCallback, T tState)
        {
            TimeSlice timeSlice = new DelayStateCallTimer<T>(delayTimeSpan, intervalTimeSpan, iTimes, timeLeft, timerStateCallback, tState);

            timeSlice.Start();

            return timeSlice;
        }

    }
}
