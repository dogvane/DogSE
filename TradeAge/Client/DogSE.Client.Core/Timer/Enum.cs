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

namespace DogSE.Client.Core.Timer
{
    /// <summary>
    /// 时间片优先级的枚举定义
    /// </summary>
    public enum TimerFrequency
    {
        /// <summary>
        /// 实时的时间片
        /// </summary>
        EveryTick = 0,

        /// <summary>
        /// 秒级别
        /// 在秒级队列，100ms检查一次
        /// </summary>
        Second = 1,

        /// <summary>
        /// 分钟级别
        /// 检查按照1s一次，检查秒级别的数据
        /// </summary>
        Minute = 2,

        /// <summary>
        /// 大于分钟级别的
        /// 先放入等待队列，1分钟检查一次，将他移入秒级别等待队列
        /// </summary>
        LongTime = 3,
    }

    /// <summary>
    /// 时间片间隔执行的类型
    /// </summary>
    public enum TimeSliceRunType
    {
        /// <summary>
        /// 内部线程
        /// </summary>
        None = 0,

        /// <summary>
        /// 业务逻辑线程执行
        /// </summary>
        LogicTask = 1,
    }
}
#endregion