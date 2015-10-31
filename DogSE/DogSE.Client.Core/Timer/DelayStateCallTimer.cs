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
#endregion

namespace DogSE.Client.Core.Timer
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal class DelayStateCallTimer<T> : TimeSlice
    {
        #region zh-CHS 私有成员变量 | en Private Member Variables
        /// <summary>
        /// 
        /// </summary>
        private TimeSliceStateCallback<T> m_Callback;

        /// <summary>
        /// 
        /// </summary>
        private T m_State;
        #endregion

        #region zh-CHS 构造和初始化和清理 | en Constructors and Initializers and Dispose

        /// <summary>
        /// 延迟调用的时间有状态类
        /// </summary>
        /// <param name="iTimes">调用的次数</param>
        /// <param name="delayTimeSpan">延迟的时间</param>
        /// <param name="intervalTimeSpan">间隔的时间</param>
        /// <param name="timerStateCallback">委托</param>
        /// <param name="timeLeft">剩余时间</param>
        /// <param name="tState">回调的状态类</param>
        public DelayStateCallTimer(TimeSpan delayTimeSpan, TimeSpan intervalTimeSpan, long iTimes, TimeSpan timeLeft, TimeSliceStateCallback<T> timerStateCallback, T tState )
            : base(delayTimeSpan, intervalTimeSpan, iTimes, timeLeft )
        {
            m_Callback = timerStateCallback;
            m_State = tState;
        }
        #endregion

        #region zh-CHS 属性 | en Properties
        /// <summary>
        /// 
        /// </summary>
        public TimeSliceStateCallback<T> Callback
        {
            get { return m_Callback; }
        }
        #endregion

        #region zh-CHS 方法 | en Method
        /// <summary>
        /// 
        /// </summary>
        public override void OnTick()
        {
            if ( m_Callback != null )
                m_Callback( m_State );
        }

        private string m_timeSliceName;

        /// <summary>
        /// 时间回调名字
        /// </summary>
        public override string TimeSliceName
        {
            get
            {
                if (m_timeSliceName == null)
                    m_timeSliceName = FormatDelegate(m_Callback);
                return m_timeSliceName;
            }
            set
            {
                m_timeSliceName = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return TimeSliceName;
        }

        #endregion
    }
}
#endregion

