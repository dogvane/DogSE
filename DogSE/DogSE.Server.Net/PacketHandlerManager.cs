#region zh-CHS 2006 - 2010 DemoSoft 团队 | en 2006-2010 DemoSoft Team

//     NOTES
// ---------------
//
// This file is a part of the MMOSE(Massively Multiplayer Online Server Engine) for .NET.
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
using System.Collections.Generic;
using System.Linq;
using Demo.Mmose.Core.Collections;
using Demo.Mmose.Core.Common;
using Demo.Mmose.Core.Common.Atom;
using System.Diagnostics;
#endregion

namespace DogSE.Server.Net
{
    /// <summary>
    /// 数据包执行信息
    /// </summary>
    public class PacketExecuteInfo
    {
        /// <summary>
        /// 
        /// </summary>
        public PacketHandler PacketHandler { get; internal set; }

        /// <summary>
        /// 
        /// </summary>
        public PacketReader PacketReader { get; internal set; }


        /// <summary>
        /// 
        /// </summary>
        public void Execute(NetState netState)
        {
            var sw = Stopwatch.StartNew();

            // 只有在客户端未断开连接时才执行业务回调
            if (netState.Running)
                PacketHandler.OnReceive(netState, PacketReader);

            sw.Stop();

            PacketHandler.CallTimes++;
            PacketHandler.ElapsedTicks += sw.Elapsed.Ticks;
        }

    }

    /// <summary>
    /// 
    /// </summary>
    [MultiThreadedNoSupport("zh-CHS", "当前的类所有成员没有锁定(管理仅在主世界服务开始的时候创建的数据包处理调用者),不支持多线程操作")]
    public class PacketHandlerManager
    {
        #region zh-CHS 私有成员变量 | en Private Member Variables
        /// <summary>
        /// 
        /// </summary>
        private MultiDictionary<PacketPriority, PacketExecuteInfo> m_PacketHandlers = new MultiDictionary<PacketPriority, PacketExecuteInfo>(false);
        #endregion

        #region zh-CHS 共有方法 | en Public Methods

        /// <summary>
        /// 获取优先级最高的数据包
        /// </summary>
        /// <returns></returns>
        public PacketExecuteInfo DequeueFirstPriority()
        {
            IEnumerable<PacketExecuteInfo> outValues = null;

            // 获取优先级最高的
            m_PacketHandlers.TryEnumerateValuesForKey(PacketPriority.Highest, out outValues);
            if (outValues != null)
            {
                PacketExecuteInfo packetExecuteInfo = outValues.FirstOrDefault<PacketExecuteInfo>();
                if (packetExecuteInfo != null)
                {
                    m_PacketHandlers.Remove(PacketPriority.Highest, packetExecuteInfo);

                    return packetExecuteInfo;
                }
            }

            // 获取优先级通常之上的
            outValues = null;
            m_PacketHandlers.TryEnumerateValuesForKey(PacketPriority.BelowNormal, out outValues);
            if (outValues != null)
            {
                PacketExecuteInfo packetExecuteInfo = outValues.FirstOrDefault<PacketExecuteInfo>();
                if (packetExecuteInfo != null)
                {
                    m_PacketHandlers.Remove(PacketPriority.BelowNormal, packetExecuteInfo);

                    return packetExecuteInfo;
                }
            }

            // 获取优先级通常的
            outValues = null;
            m_PacketHandlers.TryEnumerateValuesForKey(PacketPriority.Normal, out outValues);
            if (outValues != null)
            {
                PacketExecuteInfo packetExecuteInfo = outValues.FirstOrDefault<PacketExecuteInfo>();
                if (packetExecuteInfo != null)
                {
                    m_PacketHandlers.Remove(PacketPriority.Normal, packetExecuteInfo);

                    return packetExecuteInfo;
                }
            }

            // 获取优先级通常之下的
            outValues = null;
            m_PacketHandlers.TryEnumerateValuesForKey(PacketPriority.AboveNormal, out outValues);
            if (outValues != null)
            {
                PacketExecuteInfo packetExecuteInfo = outValues.FirstOrDefault<PacketExecuteInfo>();
                if (packetExecuteInfo != null)
                {
                    m_PacketHandlers.Remove(PacketPriority.AboveNormal, packetExecuteInfo);

                    return packetExecuteInfo;
                }
            }

            // 获取优先级最低的
            outValues = null;
            m_PacketHandlers.TryEnumerateValuesForKey(PacketPriority.Lowest, out outValues);
            if (outValues != null)
            {
                PacketExecuteInfo packetExecuteInfo = outValues.FirstOrDefault<PacketExecuteInfo>();
                if (packetExecuteInfo != null)
                {
                    m_PacketHandlers.Remove(PacketPriority.Lowest, packetExecuteInfo);

                    return packetExecuteInfo;
                }
            }

            return null;
        }

        /// <summary>
        /// 压入数据包至优先级处理列表
        /// </summary>
        public void Enqueue(PacketHandler packetHandler, PacketReader packetReader)
        {
            PacketExecuteInfo packetExecuteInfo = new PacketExecuteInfo { PacketHandler = packetHandler, PacketReader = packetReader };

            m_PacketHandlers.Add(packetHandler.PacketPriority, packetExecuteInfo);
        }

        #endregion

    }
}
#endregion
