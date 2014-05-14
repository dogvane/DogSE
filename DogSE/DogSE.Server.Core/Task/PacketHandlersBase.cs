
//     NOTES
// ---------------
//
// This file is a part of the MMOSE(Massively Multiplayer Online Server Engine) for .NET.
//
//                              2006-2010 DemoSoft Team
//
//
// First Version : by H.Q.Cai - mailto:caihuanqing@hotmail.com
// Update Version: by Dogvane - mailto:dogvane@gmail.com

/***************************************************************************
 *
 *   This program is free software; you can redistribute it and/or modify
 *   it under the terms of the GNU Lesser General Public License as published
 *   by the Free Software Foundation; either version 2.1 of the License, or
 *   (at your option) any later version.
 *
 ***************************************************************************/

#region zh-CHS 包含名字空间 | en Include namespace

using DogSE.Library.Log;
using DogSE.Server.Core.Net;

#endregion

namespace DogSE.Server.Core.Task
{
    /// <summary>
    /// 指定 NetState 的发送优先级。
    /// </summary>
    public enum PacketPriority
    {
        /// <summary>
        /// 可以将 NetState 安排在具有任何其他优先级的线程之后。
        /// </summary>
        Lowest = 0,
        /// <summary>
        /// 可以将 System.Threading.Thread 安排在具有 Normal 优先级的线程之后，在具有 Lowest 优先级的线程之前。
        /// </summary>
        BelowNormal = 1,
        /// <summary>
        /// 可以将 NetState 安排在具有 AboveNormal 优先级的线程之后，在具有 BelowNormal 优先级的线程之前。默认情况下，线程具有 Normal 优先级。
        /// </summary>
        Normal = 2,
        /// <summary>
        /// 可以将 System.Threading.Thread 安排在具有 Highest 优先级的线程之后，在具有 Normal 优先级的线程之前。
        /// </summary>
        AboveNormal = 3,
        /// <summary>
        /// 可以将 NetState 安排在具有任何其他优先级的线程之前。
        /// </summary>
        Highest = 4,
    }

    /// <summary>
    /// 消息调用委托
    /// </summary>
    /// <param name="netState"></param>
    /// <param name="reader"></param>
    public delegate void PacketReceiveCallback(NetState netState, PacketReader reader);

    /// <summary>
    /// 管理全部数据包的调用者
    /// </summary>
    public class PacketHandlersBase
    {

        #region zh-CHS 公开方法 | en Public Method
        #region zh-CHS 私有成员变量 | en Private Member Variables

        /// <summary>
        /// 
        /// </summary>
        private readonly PacketHandler[] m_Handlers = new PacketHandler[ushort.MaxValue];

        /// <summary>
        /// 仅用于测试
        /// </summary>
        public PacketHandler[] Handlers
        {
            get
            {
                return m_Handlers;
            }
        }
        #endregion
        /// <summary>
        /// 注册数据包的处理调用者
        /// 注意，如果存在相同的消息id，会进行调用方法的替换操作
        /// </summary>
        /// <param name="iPacketID"></param>
        /// <param name="onPacketReceive"></param>
        public void Register(ushort iPacketID, PacketReceiveCallback onPacketReceive)
        {
            if (m_Handlers[iPacketID] != null)
            {
                //  如果有注册相同的消息id，这里只是进行记录，并不干预运行
                Logs.Warn("Msgid {0} is replace.", iPacketID);
            }

            m_Handlers[iPacketID] = new PacketHandler(iPacketID, PacketPriority.Normal, onPacketReceive);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="iPacketID"></param>
        /// <param name="priority"></param>
        /// <param name="onPacketReceive"></param>
        public void Register(ushort iPacketID, PacketPriority priority, PacketReceiveCallback onPacketReceive)
        {
            if (m_Handlers[iPacketID] != null)
            {
                Logs.Warn("Msgid {0} is replace.", iPacketID);
            }

            m_Handlers[iPacketID] = new PacketHandler(iPacketID, priority, onPacketReceive);
        }


        /// <summary>
        /// 获取数据包的处理调用者
        /// </summary>
        /// <param name="iPacketID"></param>
        /// <returns>
        /// 如果消息id对应的消息不存在，则会返回null
        /// </returns>
        public PacketHandler GetHandler(ushort iPacketID)
        {
            return m_Handlers[iPacketID];
        }

        /// <summary>
        /// 移去数据包的处理调用者
        /// </summary>
        /// <param name="iPacketID"></param>
        internal void RemoveHandler(ushort iPacketID)
        {
            m_Handlers[iPacketID] = null;
        }

        /// <summary>
        /// 清理所有的消息处理句柄
        /// </summary>
        public void Clean()
        {
            for (int i = 0; i < m_Handlers.Length; i++)
            {
                m_Handlers[i] = null;
            }
        }

        #endregion
    }
}

