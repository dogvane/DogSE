
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
using DogSE.Library.Log;
using DogSE.Library.Time;
using DogSE.Client.Core.Task;

#endregion

namespace DogSE.Client.Core.Timer
{
    /// <summary>
    /// 
    /// </summary>
    internal class TimerThread
    {
        /// <summary>
        /// 任务管理器
        /// </summary>
        static internal Unity3DTaskManager TaskManager {private get; set; }


        #region zh-CHS 私有静态成员变量 | en Private Static Member Variables
        /// <summary>
        /// 下一次调用的时间片
        /// </summary>
        private static readonly DateTime[] s_NextPriorities =
            {
                OneServer.NowTime,
                OneServer.NowTime,
                OneServer.NowTime,
                OneServer.NowTime
            };

        /// <summary>
        /// 延迟调用的时间片
        /// </summary>
        private static readonly TimeSpan[] s_PriorityDelays =
			{
				TimeSpan.Zero,
				TimeSpan.FromMilliseconds( 100 ),
				TimeSpan.FromMilliseconds( 1000 ),
				TimeSpan.FromMinutes( 1.0 )
			};

        /// <summary>
        /// 4种时间片的列表
        /// </summary>
        private static readonly HashSet<TimeSlice>[] s_Timers =
			{
				new HashSet<TimeSlice>(),
				new HashSet<TimeSlice>(),
				new HashSet<TimeSlice>(),
				new HashSet<TimeSlice>()
			};

        #endregion

        #region zh-CHS 构造和初始化和清理 | en Constructors and Initializers and Dispose
        #region zh-CHS 私有静态成员变量 | en Private Static Member Variables
        /// <summary>
        /// 
        /// </summary>
        private static Thread s_TimerThread;
        #endregion
        /// <summary>
        /// 如果有实例TimeSlice的时候就开始初始化线程
        /// </summary>
        public static void StartTimerThread()
        {
            // 检测是否已经启动 如果启动就不需要 再次启动了
            if ( s_TimerThread != null )
                return;

            s_TimerThread = new Thread( RunTimerThread )
            {
                Name = "主时间片 线程",
                IsBackground = true
            };

            s_TimerThread.Start();
        }
        #endregion

        #region zh-CHS 静态方法 | en Static Method
        /// <summary>
        /// 添加或修改或移去时间片
        /// </summary>
        /// <param name="tTimer"></param>
        public static void AddTimer( TimeSlice tTimer )
        {
            Change( tTimer, (long)tTimer.Frequency, true );
        }

        /// <summary>
        /// 移去时间片
        /// </summary>
        /// <param name="tTimer"></param>
        public static void RemoveTimer( TimeSlice tTimer )
        {
            Change( tTimer, -1, false );
        }
        #endregion

        #region zh-CHS 私有静态方法 | en Private Static Method
        #region zh-CHS 私有静态成员变量 | en Private Static Member Variables
        /// <summary>
        /// 当有新的时间片改动或添加或移去的时候事件发生
        /// </summary>
        private static readonly AutoResetEvent s_Signal = new AutoResetEvent( true );

        //private static ThreadQueueEntity s_threadQueue;

        #endregion
        /// <summary>
        /// Timer的时间检测函数，这里是独立线程
        /// 但只负责检查时间片，不负责任务执行
        /// </summary>
        private static void RunTimerThread()
        {
            //s_threadQueue = ThreadQueue.GetThreadQueue("TimeSlice");

            Logs.Info( "Time slice: Time slice thread start!" );

            bool bSkipWait = false; // 是否跳过等待
            const int longTimeIndex = 3;
            List<TimeSlice> remove = new List<TimeSlice>();

            while (GameServerService.RunType != ServerStateType.Closing)
            {
                if (bSkipWait)
                    bSkipWait = false; // 恢复原始设置
                else
                    s_Signal.WaitOne(1); // 等待让其它的CPU有机会处理

                // 先处理改变了优先级的时间片集合
                ProcessChangeQueue();

                // 这里只先处理前3种时间片数据
                long iIndex;
                DateTime nowDateTime = OneServer.NowTime;
                for (iIndex = 0; iIndex < longTimeIndex; iIndex++)
                {
                    #region 执行普通的时间片的检查
                    
                    // 如果小于下一次处理的时间片就跳出
                    if (nowDateTime < s_NextPriorities[iIndex])
                        break;

                    // 设置下一次处理的时间片
                    s_NextPriorities[iIndex] = nowDateTime + s_PriorityDelays[iIndex];

                    foreach (TimeSlice timeSlice in s_Timers[iIndex])
                    {
                        // 如果当前时间片已经处理过,已不在先入先出的集合中,并且当前的时间大于下一次调用的时间
                        if (nowDateTime >= timeSlice.NextTime)
                        {
                            // 将定时任务压入业务逻辑处理线程里
                            //if (timeSlice.RunType == TimeSliceRunType.LogicTask)
                                TaskManager.AppendTask(timeSlice.OnTick);
                            //else
                            //    s_threadQueue.Append(timeSlice.OnTick);

                            // 调用次数累加 1
                            timeSlice.m_NumberOfTimes++;

                            // 如果运行次数大于等于当前的时间片的运行数量话就停止(如果只运行一次的话就马上调用停止,下次运行将从列表中移去,因为已经加入了TimeSlice.s_TimeSliceQueue集合所以会调用一次的)
                            bool needStop = false;
                            if (timeSlice.Times <= 0) // 检测可调用的次数
                                needStop = true;
                            else if (timeSlice.NumberOfTimes >= timeSlice.Times) // 检测调用的次数是否大于或等于最大的调用次数
                                needStop = true;
                            else if (nowDateTime >= timeSlice.StopTime) //  检测当前时间是否大于或等于最大的停止时间
                                needStop = true;
                            else
                                timeSlice.NextTime = nowDateTime + timeSlice.IntervalTime; // 计算下次调用的时间

                            if (needStop)
                            {
                                //if (timeSlice.RunType == TimeSliceRunType.LogicTask)
                                    TaskManager.AppendTask(timeSlice.Stop);
                                //else
                                //    s_threadQueue.Append(timeSlice.Stop);

                                RemoveTimer(timeSlice);
                            }
                            if (timeSlice.Frequency == TimerFrequency.LongTime)
                                remove.Add(timeSlice);
                        }
                    }

                    if (remove.Count > 0)
                    {
                        //  内部移除，就不必要走线程处理了
                        remove.ForEach(o => s_Timers[iIndex].Remove(o));
                    }
                    #endregion
                }

                //  长时间操作队列把对象放入1s间隔的检查队列里
                if (nowDateTime > s_NextPriorities[longTimeIndex])
                {
                    s_NextPriorities[longTimeIndex] = nowDateTime + s_PriorityDelays[longTimeIndex];
                    var checkTime = nowDateTime.AddMinutes(1);
                    foreach (TimeSlice timeSlice in s_Timers[longTimeIndex])
                    {
                        if (timeSlice.NextTime < checkTime)
                        {
                            s_Timers[longTimeIndex - 1].Add(timeSlice);
                        }
                    }
                }

                // 检查是否有马上需要执行的时间片
                nowDateTime = OneServer.NowTime;
                if (s_Timers[(int) TimerFrequency.EveryTick].Count > 0)
                    bSkipWait = true;
                else if (nowDateTime >= s_NextPriorities[1]) // 检查100毫秒的时间片
                    bSkipWait = true;
            }

            Logs.Info("Time slice: Time slice thread stop!");
        }

        #region zh-CHS 私有静态成员变量 | en Private Static Member Variables
        /// <summary>
        /// 改变或添加或移去的时间片
        /// </summary>
        private static readonly Queue<TimerChangeEntry> s_TimerChangeEntryChangeQueue = new Queue<TimerChangeEntry>();
        /// <summary>
        /// 锁
        /// </summary>
        private static readonly object s_LockTimerChangeEntryChangeQueue = new object();
        #endregion
        /// <summary>
        /// 添加或修改或移去的时间片
        /// </summary>
        /// <param name="tTimer"></param>
        /// <param name="newIndex"></param>
        /// <param name="isAdd"></param>
        private static void Change( TimeSlice tTimer, long newIndex, bool isAdd )
        {
            lock( s_LockTimerChangeEntryChangeQueue )
            {
                // 在ProcessChangeQueue(...)中释放入不使用的列表中
                s_TimerChangeEntryChangeQueue.Enqueue( new TimerChangeEntry( tTimer, newIndex, isAdd ) );
            }

            // 发生事件
            s_Signal.Set();
        }

        /// <summary>
        /// 处理添加或修改或移去的时间片
        /// </summary>
        private static void ProcessChangeQueue()
        {
            TimerChangeEntry[] change = null;

            lock(s_LockTimerChangeEntryChangeQueue )
            {
                if ( s_TimerChangeEntryChangeQueue.Count > 0 )
                {
                    change = s_TimerChangeEntryChangeQueue.ToArray();
                    s_TimerChangeEntryChangeQueue.Clear();
                }
            }

            if ( change == null )
                return;

            for ( int i = 0; i < change.Length; i++ )
            {
                TimerChangeEntry timerChangeEntry = change[i];

                TimeSlice nonceTimer = timerChangeEntry.TimerSlice;
                long newIndex = timerChangeEntry.TimerPriority;

                // 先从当前的优先级时间片列表中移去
                HashSet<TimeSlice> timeSliceHashSet = nonceTimer.TimeSliceHashSet;
                if ( timeSliceHashSet != null )
                    timeSliceHashSet.Remove( nonceTimer );

                // 如果是添加的话,初始化时间片数据
                if ( timerChangeEntry.AddTimerSlice )
                {
                    nonceTimer.NextTime = OneServer.NowTime + nonceTimer.DelayTime; // 计算下次调用的延迟的时间(m_Delay只计算使用一次)
                    nonceTimer.m_NumberOfTimes = 0;
                }

                // 如果优先级大于或等于零则添加到新的时间片列表中去,否则置空
                if (newIndex >= 0 && newIndex < s_Timers.Length) // 8种时间片, -1 将不添加进列表,删除掉
                {
                    nonceTimer.TimeSliceHashSet = s_Timers[newIndex];
                    nonceTimer.TimeSliceHashSet.Add( nonceTimer );
                }
                else
                    nonceTimer.TimeSliceHashSet = null;
            }
        }
        #endregion
    }
}
#endregion