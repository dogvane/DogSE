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

using System;
using System.Collections.Generic;
using DogSE.Library.Time;

namespace DogSE.Client.Core.Timer
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
        internal TimeSlice( TimeSpan delayTimeSpan )
            : this(delayTimeSpan, TimeSpan.Zero, 1, TimeSpan.MaxValue )
        {
        }


        /// <summary>
        /// 初始化时间片
        /// </summary>
        /// <param name="delayTimeSpan">延迟的时间</param>
        /// <param name="intervalTimeSpan">间隔的时间</param>
        internal TimeSlice(TimeSpan delayTimeSpan, TimeSpan intervalTimeSpan)
            : this(delayTimeSpan, intervalTimeSpan,  long.MaxValue, TimeSpan.MaxValue )
        {
        }

        /// <summary>
        /// 初始化时间片
        /// </summary>
        /// <param name="delayTimeSpan">延迟的时间</param>
        /// <param name="intervalTimeSpan">间隔的时间</param>
        /// <param name="iTimes">调用的次数</param>
        internal TimeSlice(TimeSpan delayTimeSpan, TimeSpan intervalTimeSpan, long iTimes)
            : this(delayTimeSpan, intervalTimeSpan, iTimes, TimeSpan.MaxValue )
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="delayTimeSpan"></param>
        /// <param name="intervalTimeSpan"></param>
        /// <param name="timeLeft">剩余时间</param>
        internal TimeSlice(TimeSpan delayTimeSpan, TimeSpan intervalTimeSpan, TimeSpan timeLeft)
            : this(delayTimeSpan, intervalTimeSpan, long.MaxValue, timeLeft )
        {
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="delayTimeSpan">第一次启动间隔</param>
        /// <param name="intervalTimeSpan">每次调用间隔</param>
        /// <param name="iTimes">累计调用次数</param>
        /// <param name="timeLeft">剩余时间</param>
        internal TimeSlice(TimeSpan delayTimeSpan, TimeSpan intervalTimeSpan, long iTimes, TimeSpan timeLeft)
        {
            m_Times = iTimes;
            m_TimeLeft = timeLeft;
            m_DelayTime = delayTimeSpan;
            m_IntervalTime = intervalTimeSpan;

            if (m_TimeLeft == TimeSpan.MaxValue)
                m_StopTime = DateTime.MaxValue;
            else
                m_StopTime = OneServer.NowTime + m_TimeLeft;

            if (iTimes == 1)
                m_RunFrequency = TimeSliceUtil.ComputePriority(delayTimeSpan);
            else
                m_RunFrequency = TimeSliceUtil.ComputePriority(intervalTimeSpan);

            // 添加某种时间片类型创建的数量
            TimerProfile timerProfile = TimerProfile.GetProfile(TimeSliceName);
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

        /// <summary>
        /// 时间片在哪里执行
        /// </summary>
        public TimeSliceRunType RunType { get; set; }

        #region zh-CHS 私有成员变量 | en Private Member Variables

        /// <summary>
        /// 调用的总次数
        /// </summary>
        private long m_Times;
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
        private TimeSpan m_TimeLeft;

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
        private DateTime m_StopTime;

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
        private TimerFrequency m_RunFrequency;

        #endregion
        /// <summary>
        /// 时间片的优先级
        /// </summary>
        public TimerFrequency Frequency
        {
            get { return m_RunFrequency; }
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
        private bool m_Running;

        #endregion
        /// <summary>
        /// 调用是否运行
        /// </summary>
        public bool IsRunning
        {
            get { return m_Running; }
        }


        #endregion

        #region zh-CHS 内部属性 | en Internal Properties

        /// <summary>
        /// TimerThread.m_Timers当前某种时间片的引用
        /// </summary>
        internal HashSet<TimeSlice> TimeSliceHashSet { get; set; }

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

                TimerProfile timerProfile = TimerProfile.GetProfile(TimeSliceName);
                timerProfile.RegStart();
            }
        }

        /// <summary>
        /// 停止时间片的处理
        /// </summary>
        public void Stop()
        {
            if (m_Running)
            {
                TimerThread.RemoveTimer(this);
                m_Running = false;

                TimerProfile timerProfile = TimerProfile.GetProfile(TimeSliceName);
                timerProfile.RegStopped();

                // 时间片已经停止的回调事件
                OnStopTimeSlice();
            }
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

        #region zh-CHS 共有事件 | en Public Event


        /// <summary>
        /// 
        /// </summary>
        public event EventHandler<StopTimeSliceEventArgs> StopTimeSlice;

        internal void OnStopTimeSlice()
        {
            EventHandler<StopTimeSliceEventArgs> handler = StopTimeSlice;
            if (handler != null) 
                handler(this, new StopTimeSliceEventArgs(this));
        }

        #endregion

        /// <summary>
        /// 时间回调方法的名称
        /// </summary>
        public virtual string TimeSliceName {get;set;}

        internal static string FormatDelegate(Delegate delegateCallback)
        {
            if (delegateCallback == null)
                return "null";

            string methodName;
            if (delegateCallback.Method.DeclaringType != null)
                methodName = delegateCallback.Method.DeclaringType.FullName;
            else
                methodName = delegateCallback.Method.ReflectedType.FullName;

            return String.Format("{0}.{1}", methodName, delegateCallback.Method.Name);
        }
    }
}

