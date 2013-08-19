#region zh-CHS 2010 - 2010 DemoSoft 团队 | en 2010-2010 DemoSoft Team

//     NOTES
// ---------------
//
// This file is a part of the MMOCE(Massively Multiplayer Online Client Engine) for .NET.
//
//                              2010-2010 DemoSoft Team
//
//
// First Version : by H.Q.Cai - mailto:caihuanqing@hotmail.com

/***************************************************************************
 *
 *   This program is free software; you can redistribute it and/or modify
 *   it under the terms of the GNU Lesser General Public License as published
 *   by the Free Software Foundation; either version 2.1 of the License, or
 *   (at your option) any later version.
 *
 ***************************************************************************/

#region zh-CHS 包含名字空间 | en Include namespace
using System;
using System.Collections.Generic;
using System.Threading;
using DogSE.Library.Time;

#endregion

namespace DogSE.Server.Core.Timer
{
    /// <summary>
    /// 时间片的处理(具有均衡负载的时间片处理)
    /// </summary>
    public class TimeSlice
    {
        #region zh-CHS 构造和初始化和清理 | en Constructors and Initializers and Dispose
        /// <summary>
        /// 初始化时间片
        /// </summary>
        /// <param name="delayTimeSpan">延迟的时间</param>
        public TimeSlice( TimeSpan delayTimeSpan )
            : this(delayTimeSpan, TimeSpan.Zero, (long) 1, TimeSpan.MaxValue )
        {
        }


        /// <summary>
        /// 初始化时间片
        /// </summary>
        /// <param name="delayTimeSpan">延迟的时间</param>
        /// <param name="intervalTimeSpan">间隔的时间</param>
        public TimeSlice( TimeSpan delayTimeSpan, TimeSpan intervalTimeSpan )
            : this(delayTimeSpan, intervalTimeSpan, (long) long.MaxValue, TimeSpan.MaxValue )
        {
        }

        /// <summary>
        /// 初始化时间片
        /// </summary>
        /// <param name="delayTimeSpan">延迟的时间</param>
        /// <param name="intervalTimeSpan">间隔的时间</param>
        /// <param name="iTimes">调用的次数</param>
        public TimeSlice( TimeSpan delayTimeSpan, TimeSpan intervalTimeSpan, long iTimes )
            : this(delayTimeSpan, intervalTimeSpan, iTimes, TimeSpan.MaxValue )
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="delayTimeSpan"></param>
        /// <param name="intervalTimeSpan"></param>
        /// <param name="timeLeft">剩余时间</param>
        public TimeSlice( TimeSpan delayTimeSpan, TimeSpan intervalTimeSpan, TimeSpan timeLeft )
            : this(delayTimeSpan, intervalTimeSpan, (long) long.MaxValue, timeLeft )
        {
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="delayTimeSpan"></param>
        /// <param name="intervalTimeSpan"></param>
        /// <param name="iTimes"></param>
        /// <param name="timeLeft">剩余时间</param>
        public TimeSlice(TimeSpan delayTimeSpan, TimeSpan intervalTimeSpan, long iTimes, TimeSpan timeLeft)
        {
            m_Times = iTimes;
            m_TimeLeft = timeLeft;
            m_DelayTime = delayTimeSpan;
            m_IntervalTime = intervalTimeSpan;

            if ( m_TimeLeft == TimeSpan.MaxValue )
                m_StopTime = DateTime.MaxValue;
            else
                m_StopTime = OneServer.NowTime + m_TimeLeft;

            if ( iTimes == 1 )
                m_RunFrequency = ComputePriority( delayTimeSpan );
            else
                m_RunFrequency = ComputePriority( intervalTimeSpan );

            // 添加某种时间片类型创建的数量
            TimerProfile timerProfile = GetProfile();
            if ( timerProfile != null )
                timerProfile.RegCreation(); // 添加某种时间片类型的创建的数量
        }
        #endregion

        #region zh-CHS 属性 | en Properties

        #region zh-CHS 私有成员变量 | en Private Member Variables

        /// <summary>
        /// 调用次数的累计数
        /// </summary>
        internal long m_NumberOfTimes;

        #endregion
        /// <summary>
        /// 调用次数的累计数
        /// </summary>
        public long NumberOfTimes
        {
            get { return m_NumberOfTimes; }
        }

        #region zh-CHS 私有成员变量 | en Private Member Variables
        /// <summary>
        /// 调用的总次数
        /// </summary>
        private long m_Times = long.MaxValue;
        #endregion
        /// <summary>
        /// 调用的总次数
        /// </summary>
        public long Times
        {
            get { return m_Times; }
            set
            {
                // 每次设置都产生新的调用次数

                // 先置空调用的次数
                m_NumberOfTimes = 0;

                // 后设置需要检测的总次数
                m_Times = value;
            }
        }

        #region zh-CHS 私有成员变量 | en Private Member Variables
        /// <summary>
        /// 剩余时间
        /// </summary>
        private TimeSpan m_TimeLeft = TimeSpan.MaxValue;
        #endregion
        /// <summary>
        /// 剩余时间
        /// </summary>
        public TimeSpan TimeLeft
        {
            get { return m_TimeLeft; }
            set
            {
                // 每次设置都产生新的停止时间

                // 先设置停止时间
                m_StopTime = OneServer.NowTime + value;
                // 后设置剩余时间
                m_TimeLeft = value;
            }
        }

        #region zh-CHS 私有成员变量 | en Private Member Variables
        /// <summary>
        /// 剩余时间
        /// </summary>
        private DateTime m_StopTime = DateTime.MaxValue;
        #endregion
        /// <summary>
        /// 停止时间
        /// </summary>
        public DateTime StopTime
        {
            get { return m_StopTime; }
        }

        #region zh-CHS 私有成员变量 | en Private Member Variables
        /// <summary>
        /// 时间片的优先级
        /// </summary>
        private TimerFrequency m_RunFrequency = TimerFrequency.EveryTick;
        #endregion
        /// <summary>
        /// 时间片的优先级
        /// </summary>
        public TimerFrequency Frequency
        {
            get { return m_RunFrequency; }
            set
            {
                if ( m_RunFrequency != value )
                {
                    m_RunFrequency = value;

                    // 如果是在运行则调用改变当前时间片的优先级, 如果没有运行则在Start()调用TimerThread.AddTimer(...)
                    if ( IsRunning )
                        TimerThread.PriorityChange( this, m_RunFrequency );
                }
            }
        }

        #region zh-CHS 私有成员变量 | en Private Member Variables
        /// <summary>
        /// 下一次调用的时间
        /// </summary>
        private DateTime m_NextTime = OneServer.NowTime;
        #endregion
        /// <summary>
        /// 下一次的调用时间
        /// </summary>
        public DateTime NextTime
        {
            get { return m_NextTime; }
            internal set { m_NextTime = value; }
        }

        #region zh-CHS 私有成员变量 | en Private Member Variables
        /// <summary>
        /// 延迟调用的时间(只在加入集合时计算一次)
        /// </summary>
        private TimeSpan m_DelayTime = TimeSpan.Zero;
        #endregion
        /// <summary>
        /// 延迟调用的时间间隔(只在加入集合时计算一次)
        /// </summary>
        public TimeSpan DelayTime
        {
            get { return m_DelayTime; }
            set { m_DelayTime = value; }
        }

        #region zh-CHS 私有成员变量 | en Private Member Variables
        /// <summary>
        /// 间隔调用的时间
        /// </summary>
        private TimeSpan m_IntervalTime = TimeSpan.Zero;
        #endregion
        /// <summary>
        /// 间隔调用的时间间隔
        /// </summary>
        public TimeSpan IntervalTime
        {
            get { return m_IntervalTime; }
            set { m_IntervalTime = value; }
        }

        #region zh-CHS 私有成员变量 | en Private Member Variables

        /// <summary>
        /// 调用是否在运行(volatile 用于多线程)
        /// </summary>
        private bool m_Running = false;

        #endregion
        /// <summary>
        /// 调用是否运行
        /// </summary>
        public bool IsRunning
        {
            get { return m_Running; }
        }

        #region zh-CHS BaseWorld属性 | en BaseWorld Properties

        /// <summary>
        /// 管理当前的世界服务
        /// </summary>
        public WorldBase BaseWorld { get; internal set; }

        #endregion

        #endregion

        #region zh-CHS 内部属性 | en Internal Properties

        /// <summary>
        /// TimerThread.m_Timers当前某种时间片的引用
        /// </summary>
        internal HashSet<TimeSlice> TimeSliceHashSet { get; set; }

        #region zh-CHS 私有成员变量 | en Private Member Variables
        /// <summary>
        /// 表示当前的时间片已否已经加入了TimeSlice.s_TimeSliceQueue的先入先出集合中
        /// </summary>
        private volatile bool m_InQueued;
        #endregion
        /// <summary>
        /// 表示当前的时间片已否已经加入了TimeSlice.s_TimeSliceQueue的先入先出集合中
        /// </summary>
        internal bool InQueued
        {
            get { return m_InQueued; }
        }

        #endregion

        #region zh-CHS 共有静态成员变量 | en Public Static Member Variables
        #region zh-CHS 私有成员变量 | en Private Member Variables
        /// <summary>
        /// 当时间片的累计数量到该数量时将中断后面的调用
        /// </summary>
        private static long s_BreakSliceAtNumber = 200; // 默认值(在多线程中不因该太多)
        #endregion
        /// <summary>
        /// 当时间片的累计数量到该数量时将中断后面的调用
        /// </summary>
        public static long BreakSliceAtNumber
        {
            get { return s_BreakSliceAtNumber; }
            set { s_BreakSliceAtNumber = value; }
        }
        #endregion

        #region zh-CHS 内部静态属性 | en Internal Static Properties
        #region zh-CHS 私有成员变量 | en Private Member Variables
        /// <summary>
        /// TimerProfile处理信息定义,以类型名为关键字共有8种
        /// </summary>
        private static Dictionary<string, TimerProfile> s_Profiles = new Dictionary<string, TimerProfile>();
        #endregion
        /// <summary>
        /// 时间片的处理信息定义,以类型名为关键字共有8种
        /// </summary>
        internal static Dictionary<string, TimerProfile> Profiles
        {
            get { return s_Profiles; }
        }
        #endregion

        #region zh-CHS 共有方法 | en Public Methods
        /// <summary>
        /// 开始时间片的处理
        /// </summary>
        public void Start()
        {
            if (!m_Running)
            {
                TimerThread.AddTimer(this);

                m_Running = true;

                TimerProfile timerProfile = GetProfile();
                if (timerProfile != null)
                    timerProfile.RegStart();
            }
        }

        /// <summary>
        /// 停止时间片的处理
        /// </summary>
        public void Stop()
        {
            if ( m_Running )
            {
                TimerThread.RemoveTimer( this );
                m_Running = false;

                TimerProfile timerProfile = GetProfile();
                if ( timerProfile != null )
                    timerProfile.RegStopped();

                // 时间片已经停止的回调事件
                EventHandler<StopTimeSliceEventArgs> tempEventArgs = m_EventStopTimeSlice;
                if ( tempEventArgs != null )
                {
                    var eventArgs = new StopTimeSliceEventArgs( this );

                    tempEventArgs( this, eventArgs );
                }
            }
        }

        /// <summary>
        /// 给出某种时间片的处理信息
        /// </summary>
        /// <returns></returns>
        public TimerProfile GetProfile()
        {
            string strName = ToString();
            if ( string.IsNullOrEmpty( strName ) )
                strName = "null";

            TimerProfile timerProfile;
            s_Profiles.TryGetValue(strName, out timerProfile);
            if ( timerProfile == null )
            {
                timerProfile = new TimerProfile();
                s_Profiles.Add( strName, timerProfile ); 
            }

            return timerProfile;
        }

        /// <summary>
        /// 给出时间片的定义字符串
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return GetType().FullName;
        }
        #endregion

        #region zh-CHS 保护方法 | en Protected Methods
        /// <summary>
        /// 时间片的处理函数
        /// </summary>
        public virtual void OnTick()
        {
        }
        #endregion

        #region zh-CHS 共有静态方法 | en Public Static Methods

        #region zh-CHS 私有静态成员变量 | en Private Static Member Variables
        /// <summary>
        /// 判断的时间倍数
        /// </summary>
        /// <remarks>
        /// 10 约等于在这个时间间隔 10 会触发10次判断，数值越大越精确。
        /// 
        /// 20 相当于 5%
        /// 10 相当于 10%
        /// 
        /// </remarks>
        private const int TimeMultiple = 20;

        /// <summary>
        /// 1分钟的时间片
        /// </summary>
        private static readonly TimeSpan s_OneMinute = TimeSpan.FromMinutes(1.0 * TimeMultiple);

        /// <summary>
        /// 20秒的时间片
        /// </summary>
        private static readonly TimeSpan s_TwentySeconds = TimeSpan.FromSeconds(20.0 * TimeMultiple);

        /// <summary>
        /// 5秒的时间片
        /// </summary>
        private static readonly TimeSpan s_FiveSeconds = TimeSpan.FromSeconds(5.0 * TimeMultiple);

        /// <summary>
        /// 1秒的时间片
        /// </summary>
        private static readonly TimeSpan s_OneSecond = TimeSpan.FromSeconds(1.0 * TimeMultiple);

        /// <summary>
        /// 500毫秒的时间片
        /// </summary>
        private static readonly TimeSpan s_FiveHundredMS = TimeSpan.FromMilliseconds(500.0 * TimeMultiple);

        /// <summary>
        /// 100毫秒的时间片
        /// </summary>
        private static readonly TimeSpan s_HundredMS = TimeSpan.FromMilliseconds(100.0 * TimeMultiple);

        /// <summary>
        /// 25毫秒的时间片
        /// </summary>
        private static readonly TimeSpan s_TwentyFiveMS = TimeSpan.FromMilliseconds(25.0 * TimeMultiple);

        #endregion
        /// <summary>
        /// 获取时间片的优先级
        /// </summary>
        /// <param name="timeSpan"></param>
        /// <returns></returns>
        public static TimerFrequency ComputePriority( TimeSpan timeSpan )
        {
            if ( timeSpan >= s_OneMinute )
                return TimerFrequency.OneMinute;

            if ( timeSpan >= s_TwentySeconds )
                return TimerFrequency.TwentySeconds;

            if ( timeSpan >= s_FiveSeconds )
                return TimerFrequency.FiveSeconds;

            if ( timeSpan >= s_OneSecond )
                return TimerFrequency.OneSecond;

            if ( timeSpan >= s_FiveHundredMS )
                return TimerFrequency.FiveHundredMS;

            if ( timeSpan >= s_HundredMS )
                return TimerFrequency.HundredMS;

            if ( timeSpan >= s_TwentyFiveMS )
                return TimerFrequency.TwentyFiveMS;

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
        /// 在 delayTimeSpan 时间结束后调用一次回调函数
        /// </summary>
        /// <param name="delayTimeSpan"></param>
        /// <param name="timerStateCallback"></param>
        /// <param name="tState"></param>
        /// <returns></returns>
        public static TimeSlice StartTimeSlice(TimeSpan delayTimeSpan, TimeSliceStateCallback timerStateCallback, object tState)
        {
            return StartTimeSlice(delayTimeSpan, TimeSpan.Zero, 1, TimeSpan.MaxValue, timerStateCallback, tState);
        }

        /// <summary>
        /// 从 delayTimeSpan 后开始，每隔 intervalTimeSpan 调用一次回调函数，直到游戏退出
        /// </summary>
        /// <param name="delayTimeSpan"></param>
        /// <param name="intervalTimeSpan"></param>
        /// <param name="timerStateCallback"></param>
        /// <param name="tState"></param>
        /// <returns></returns>
        public static TimeSlice StartTimeSlice(TimeSpan delayTimeSpan, TimeSpan intervalTimeSpan, TimeSliceStateCallback timerStateCallback, object tState)
        {
            return StartTimeSlice(delayTimeSpan, intervalTimeSpan, long.MaxValue, TimeSpan.MaxValue, timerStateCallback, tState);
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
        /// 从 delayTimeSpan 后开始，每隔 intervalTimeSpan 调用一次回调函数，直到 iTimes 次回调后结束。
        /// </summary>
        /// <param name="delayTimeSpan"></param>
        /// <param name="intervalTimeSpan"></param>
        /// <param name="iTimes"></param>
        /// <param name="timerStateCallback"></param>
        /// <param name="tState"></param>
        /// <returns></returns>
        public static TimeSlice StartTimeSlice(TimeSpan delayTimeSpan, TimeSpan intervalTimeSpan, long iTimes, TimeSliceStateCallback timerStateCallback, object tState)
        {
            return StartTimeSlice(delayTimeSpan, intervalTimeSpan, iTimes, TimeSpan.MaxValue, timerStateCallback, tState);
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
        /// 从 delayTimeSpan 后开始，每隔 intervalTimeSpan 调用一次回调函数，直到超过 timeLeft 的时间。
        /// </summary>
        /// <param name="delayTimeSpan"></param>
        /// <param name="intervalTimeSpan"></param>
        /// <param name="timeLeft"></param>
        /// <param name="timerStateCallback"></param>
        /// <param name="tState"></param>
        /// <returns></returns>
        public static TimeSlice StartTimeSlice(TimeSpan delayTimeSpan, TimeSpan intervalTimeSpan, TimeSpan timeLeft, TimeSliceStateCallback timerStateCallback, object tState)
        {
            return StartTimeSlice(delayTimeSpan, intervalTimeSpan, long.MaxValue, timeLeft, timerStateCallback, tState);
        }


        /// <summary>
        /// 带优先级从 delayTimeSpan 后开始，每隔 intervalTimeSpan 调用一次回调函数，直到超过 timeLeft 的时间或者回调次数达到 iTimes。
        /// </summary>
        /// <param name="delayTimeSpan"></param>
        /// <param name="intervalTimeSpan"></param>
        /// <param name="iTimes"></param>
        /// <param name="timeLeft"></param>
        /// <param name="timerStateCallback"></param>
        /// <param name="tState"></param>
        /// <returns></returns>
        public static TimeSlice StartTimeSlice(TimeSpan delayTimeSpan, TimeSpan intervalTimeSpan, long iTimes, TimeSpan timeLeft, TimeSliceStateCallback timerStateCallback, object tState)
        {
            TimeSlice timeSlice = new DelayStateCallTimer(delayTimeSpan, intervalTimeSpan, iTimes, timeLeft, timerStateCallback, tState);

            timeSlice.Start();

            return timeSlice;
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


        #endregion

        #region zh-CHS 内部静态方法 | en Internal Static Methods
        #region zh-CHS 私有静态成员变量 | en Private Static Member Variables

        /// <summary>
        /// 需要处理的时间片的先进先出列队集合(线程安全)(Normal 调度优先级)
        /// </summary>
        private static Queue<TimeSlice> s_NormalTimeSliceQueue = new Queue<TimeSlice>();
        /// <summary>
        /// (Normal 调度优先级)集合锁
        /// </summary>
        private static object s_LockNormalTimeSliceQueue = new object();

        #endregion
        /// <summary>
        /// 放入处理列表中
        /// </summary>
        internal static void JoinProcessQueue(TimeSlice timeSlice)
        {
            // 现修改 防止多线程(还没改成true就已经调用完成被设置成false了)
            timeSlice.m_InQueued = true;

            Monitor.Enter(s_LockNormalTimeSliceQueue);
            try
            {
                // 线程安全, 放入处理列表中
                s_NormalTimeSliceQueue.Enqueue(timeSlice);
            }
            finally
            {
                Monitor.Exit(s_LockNormalTimeSliceQueue);
            }
        }

        /// <summary>
        /// 获取时间片定义的字符串信息
        /// </summary>
        /// <param name="delegateCallback"></param>
        /// <returns></returns>
        internal static string FormatDelegate( Delegate delegateCallback )
        {
            if ( delegateCallback == null )
                return "null";

            return String.Format( "{0}.{1}", delegateCallback.Method.DeclaringType.FullName, delegateCallback.Method.Name );
        }
        #endregion

        #region zh-CHS 共有事件 | en Public Event

        #region zh-CHS StopTimeSliceEventArgs事件 | en Public Event

        #region zh-CHS 私有成员变量 | en Private Member Variables
        /// <summary>
        /// 
        /// </summary>
        private EventHandler<StopTimeSliceEventArgs> m_EventStopTimeSlice;
        #endregion
        /// <summary>
        /// 
        /// </summary>
        public event EventHandler<StopTimeSliceEventArgs> StopTimeSlice
        {
            add
            {
                Monitor.Enter( this );
                try
                {
                    m_EventStopTimeSlice += value;
                }
                finally
                {
                    Monitor.Exit( this );
                }
            }
            remove
            {
                Monitor.Enter( this ); 
                try
                {
                    if (m_EventStopTimeSlice != null)
                        m_EventStopTimeSlice -= value;
                }
                finally
                {
                    Monitor.Exit( this );
                }
            }
        }

        #endregion

        #endregion
    }
}
#endregion

