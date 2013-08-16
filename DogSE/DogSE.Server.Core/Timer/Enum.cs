#region zh-CHS 2010 - 2010 DemoSoft 团队 | en 2010-2010 DemoSoft Team

//     NOTES
// ---------------
//
// This file is a part of the MMOCE(Massively Multiplayer Online Client Engine) for .NET.
//
//                              2006-2010 DemoSoft Team
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

namespace DogSE.Server.Core.Timer
{
    #region zh-CHS 枚举 | en Enum
    /// <summary>
    /// 时间片优先级的枚举定义
    /// </summary>
    public enum TimerFrequency
    {
        /// <summary>
        /// 实时的时间片
        /// </summary>
        EveryTick = 0x00,
        /// <summary>
        /// 25毫秒的时间片
        /// </summary>
        TwentyFiveMS = 0x01,
        /// <summary>
        /// 100毫秒的时间片
        /// </summary>
        HundredMS = 0x02,
        /// <summary>
        /// 500毫秒的时间片
        /// </summary>
        FiveHundredMS = 0x03,
        /// <summary>
        /// 1秒的时间片
        /// </summary>
        OneSecond = 0x04,
        /// <summary>
        /// 5秒的时间片
        /// </summary>
        FiveSeconds = 0x05,
        /// <summary>
        /// 20秒的时间片
        /// </summary>
        TwentySeconds = 0x06,
        /// <summary>
        /// 1分钟的时间片
        /// </summary>
        OneMinute = 0x07
    }

    /// <summary>
    /// 指定 TimeSlice 的调度优先级。
    /// </summary>
    public enum TimerPriority
    {
        /// <summary>
        /// 可以将 TimeSlice 安排在具有任何其他优先级的线程之后。
        /// </summary>
        Lowest = 0,
        /// <summary>
        /// 可以将 TimeSlice 安排在具有 AboveNormal 优先级的线程之后，在具有 BelowNormal 优先级的线程之前。默认情况下，线程具有 Normal 优先级。
        /// </summary>
        Normal = 2,
        /// <summary>
        /// 可以将 TimeSlice 安排在具有任何其他优先级的线程之前。
        /// </summary>
        Highest = 4,
    }
    #endregion
}
#endregion