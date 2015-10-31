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

#endregion

namespace DogSE.Client.Core.Timer
{
    /// <summary>
    /// 时间片的属性改变
    /// </summary>
    internal struct TimerChangeEntry
    {
        #region zh-CHS 构造和初始化和清理 | en Constructors and Initializers and Dispose
        /// <summary>
        /// 
        /// </summary>
        /// <param name="tTimer"></param>
        /// <param name="newIndex"></param>
        /// <param name="isAdd"></param>
        public TimerChangeEntry( TimeSlice tTimer, long newIndex, bool isAdd )
        {
            m_TimerSlice = tTimer;
            m_NewPriority = newIndex;
            m_IsAddTimerSlice = isAdd;
        }
        #endregion

        #region zh-CHS 属性 | en Properties
        #region zh-CHS 私有成员变量 | en Private Member Variables
        /// <summary>
        /// 添加或修改或移去的时间片
        /// </summary>
        private TimeSlice m_TimerSlice;
        #endregion
        /// <summary>
        /// 
        /// </summary>
        public TimeSlice TimerSlice
        {
            get { return m_TimerSlice; }
        }

        #region zh-CHS 私有成员变量 | en Private Member Variables
        /// <summary>
        /// 新时间片的属性
        /// </summary>
        private long m_NewPriority;
        #endregion
        /// <summary>
        /// 
        /// </summary>
        public long TimerPriority
        {
            get { return m_NewPriority; }
        }

        #region zh-CHS 私有成员变量 | en Private Member Variables
        /// <summary>
        /// 是否是添加还是移去时间片
        /// </summary>
        private bool m_IsAddTimerSlice;
        #endregion
        /// <summary>
        /// 
        /// </summary>
        public bool AddTimerSlice
        {
            get { return m_IsAddTimerSlice; }
        }
        #endregion
    }
}
#endregion

