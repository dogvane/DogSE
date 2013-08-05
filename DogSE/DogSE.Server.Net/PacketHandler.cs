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
using Demo.Mmose.Core.Common;
using Demo.Mmose.Core.Common.Atom;
#endregion

namespace DogSE.Server.Net
{
    /// <summary>
    /// 数据包的主处理者
    /// </summary>
    [MultiThreadedNoSupport("zh-CHS", "当前的类所有成员没有锁定(仅在主世界服务开始的时候创建数据包处理的调用者,所有成员只有返回值,无法设置),不支持多线程操作")]
    public class PacketHandler
    {
        #region zh-CHS 构造和初始化和清理 | en Constructors and Initializers and Dispose
        /// <summary>
        /// 
        /// </summary>
        /// <param name="iPacketID"></param>
        /// <param name="iLength"></param>
        /// <param name="bInGame"></param>
        /// <param name="onPacketReceive"></param>
        internal PacketHandler(long iPacketID, long iMinLength, PacketPriority priority, PacketReceiveCallback onPacketReceive)
        {
            m_PacketID = iPacketID;
            m_MinLength = iMinLength;
            m_PacketPriority = priority;
            m_OnReceive = onPacketReceive;

            CallTimes = 0;
            ElapsedTicks = 0;
        }
        #endregion

        #region zh-CHS 属性 | en Properties
        #region zh-CHS 私有成员变量 | en Private Member Variables
        /// <summary>
        /// 数据包的ID
        /// </summary>
        private long m_PacketID;
        #endregion
        /// <summary>
        /// 数据包的ID
        /// </summary>
        public long PacketID
        {
            get { return m_PacketID; }
        }

        #region zh-CHS 私有成员变量 | en Private Member Variables
        /// <summary>
        /// 数据包的优先级
        /// </summary>
        private PacketPriority m_PacketPriority = PacketPriority.Normal;
        #endregion
        /// <summary>
        /// 数据包的优先级
        /// </summary>
        public PacketPriority PacketPriority
        {
            get { return m_PacketPriority; }
        }

        #region zh-CHS 私有成员变量 | en Private Member Variables
        /// <summary>
        /// 数据包最小的长度
        /// </summary>
        private long m_MinLength;
        #endregion
        /// <summary>
        /// 数据包最小的长度
        /// </summary>
        public long MinLength
        {
            get { return m_MinLength; }
        }

        #region zh-CHS 私有成员变量 | en Private Member Variables
        /// <summary>
        /// 数据实际处理的回调
        /// </summary>
        private PacketReceiveCallback m_OnReceive;
        #endregion
        /// <summary>
        /// 数据实际处理的回调
        /// </summary>
        public PacketReceiveCallback OnReceive
        {
            get { return m_OnReceive; }
        }


        /// <summary>
        /// 调用次数
        /// </summary>
        public AtomicLong CallTimes { get; set; }
        /// <summary>
        /// 执行消耗Ticks
        /// </summary>
        public AtomicLong ElapsedTicks { get; set; }

        #endregion
    }
}
#endregion