
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

using System;
using DogSE.Library.Log;
using DogSE.Server.Core.Net;
using DogSE.Server.Core.Protocol;
using DogSE.Server.Core.Task;

#endregion

namespace DogSE.Server.Core.TaskT
{
    /// <summary>
    /// 管理全部数据包的调用者
    /// </summary>
    public class PacketHandlersBaseT<T>
    {

        #region zh-CHS 公开方法 | en Public Method
        #region zh-CHS 私有成员变量 | en Private Member Variables

        /// <summary>
        /// 
        /// </summary>
        private readonly PacketHandlerT<T>[] m_Handlers = new PacketHandlerT<T>[ushort.MaxValue];

        /// <summary>
        /// 仅用于测试
        /// </summary>
        public PacketHandlerT<T>[] Handlers
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
        public void Register(ushort iPacketID, Action<T, PacketReader> onPacketReceive)
        {
            if (m_Handlers[iPacketID] != null)
            {
                //  如果有注册相同的消息id，这里只是进行记录，并不干预运行
                Logs.Warn("Msgid {0} is replace.", iPacketID);
            }
            
            //  这里在初始化的时候就把对应的性能监视的对象给创建好
            NetTaskProfile.GetNetTaskProfile(iPacketID);

            m_Handlers[iPacketID] = new PacketHandlerT<T>(iPacketID, PacketPriority.Normal, onPacketReceive);
        }

        /// <summary>
        /// 任务类型
        /// </summary>
        /// <param name="iPacketID"></param>
        /// <param name="taskType"></param>
        /// <param name="onPacketReceive"></param>
        public void Register(ushort iPacketID, TaskType taskType, Action<T, PacketReader> onPacketReceive)
        {
            if (m_Handlers[iPacketID] != null)
            {
                //  如果有注册相同的消息id，这里只是进行记录，并不干预运行
                Logs.Warn("Msgid {0} is replace.", iPacketID);
            }

            //  这里在初始化的时候就把对应的性能监视的对象给创建好
            NetTaskProfile.GetNetTaskProfile(iPacketID);

            m_Handlers[iPacketID] = new PacketHandlerT<T>(iPacketID, PacketPriority.Normal, taskType, onPacketReceive);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="iPacketID"></param>
        /// <param name="priority"></param>
        /// <param name="onPacketReceive"></param>
        public void Register(ushort iPacketID, PacketPriority priority, Action<T, PacketReader> onPacketReceive)
        {
            if (m_Handlers[iPacketID] != null)
            {
                Logs.Warn("Msgid {0} is replace.", iPacketID);
            }

            NetTaskProfile.GetNetTaskProfile(iPacketID);

            m_Handlers[iPacketID] = new PacketHandlerT<T>(iPacketID, priority, onPacketReceive);
        }


        /// <summary>
        /// 获取数据包的处理调用者
        /// </summary>
        /// <param name="iPacketID"></param>
        /// <returns>
        /// 如果消息id对应的消息不存在，则会返回null
        /// </returns>
        public PacketHandlerT<T> GetHandler(ushort iPacketID)
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

    /// <summary>
    /// 数据包的主处理者
    /// </summary>
    public class PacketHandlerT<T>
    {
        #region zh-CHS 构造和初始化和清理 | en Constructors and Initializers and Dispose
        /// <summary>
        /// 
        /// </summary>
        /// <param name="iPacketID"></param>
        /// <param name="priority"></param>
        /// <param name="onPacketReceive"></param>
        internal PacketHandlerT(ushort iPacketID, PacketPriority priority, Action<T, PacketReader> onPacketReceive)
        {
            m_PacketID = iPacketID;
            m_PacketPriority = priority;
            m_OnReceive = onPacketReceive;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="iPacketID"></param>
        /// <param name="priority"></param>
        /// <param name="taskType"></param>
        /// <param name="onPacketReceive"></param>
        internal PacketHandlerT(ushort iPacketID, PacketPriority priority, TaskType taskType,
            Action<T, PacketReader> onPacketReceive)
        {
            m_PacketID = iPacketID;
            m_PacketPriority = priority;
            m_OnReceive = onPacketReceive;
            m_TaskType = taskType;
        }

        #endregion

        #region zh-CHS 属性 | en Properties
        #region zh-CHS 私有成员变量 | en Private Member Variables
        /// <summary>
        /// 数据包的ID
        /// </summary>
        private readonly ushort m_PacketID;
        #endregion
        /// <summary>
        /// 数据包的ID
        /// </summary>
        public ushort PacketID
        {
            get { return m_PacketID; }
        }

        #region zh-CHS 私有成员变量 | en Private Member Variables
        /// <summary>
        /// 数据包的优先级
        /// </summary>
        private readonly PacketPriority m_PacketPriority = PacketPriority.Normal;
        #endregion
        /// <summary>
        /// 数据包的优先级
        /// </summary>
        public PacketPriority PacketPriority
        {
            get { return m_PacketPriority; }
        }

        #endregion

        #region zh-CHS 私有成员变量 | en Private Member Variables


        #region zh-CHS 私有成员变量 | en Private Member Variables

        /// <summary>
        /// 数据实际处理的回调
        /// </summary>
        private readonly Action<T, PacketReader> m_OnReceive;

        #endregion

        /// <summary>
        /// 数据实际处理的回调
        /// </summary>
        public Action<T, PacketReader> OnReceive
        {
            get { return m_OnReceive; }
        }


        #endregion

        #region zh-CHS 私有成员变量 | en Private Member Variables

        /// <summary>
        /// 数据包的优先级
        /// </summary>
        private readonly TaskType m_TaskType = TaskType.Main;

        /// <summary>
        /// 数据包的优先级
        /// </summary>
        public TaskType TaskType
        {
            get { return m_TaskType; }
        }

        #endregion
    }
}

