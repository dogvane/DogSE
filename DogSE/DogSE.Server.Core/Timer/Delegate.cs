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


namespace DogSE.Server.Core.Timer
{
    #region zh-CHS 委托 | en Delegate
    /// <summary>
    /// 时间片的委托
    /// </summary>
    public delegate void TimeSliceCallback();

    /// <summary>
    /// 包含指定泛行对象的时间片的委托
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="tState"></param>
    public delegate void TimeSliceStateCallback<T>( T tState );


    /// <summary>
    /// Aura的事件数据类
    /// </summary>
    public class StopTimeSliceEventArgs : EventArgs
    {
        #region zh-CHS 构造和初始化和清理 | en Constructors and Initializers and Dispose
        /// <summary>
        /// 初始化构造
        /// </summary>
        /// <param name="timeSlice"></param>
        public StopTimeSliceEventArgs( TimeSlice timeSlice )
        {
            m_TimeSlice = timeSlice;
        }
        #endregion

        #region zh-CHS 共有属性 | en Public Properties

        #region zh-CHS 私有成员变量 | en Private Member Variables
        /// <summary>
        /// 
        /// </summary>
        private TimeSlice m_TimeSlice;
        #endregion
        /// <summary>
        /// 
        /// </summary>
        public TimeSlice TimeSlice
        {
            get { return m_TimeSlice; }
        }

        #endregion
    }
    #endregion
}
#endregion