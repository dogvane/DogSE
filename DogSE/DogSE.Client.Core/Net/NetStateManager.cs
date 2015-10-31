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

using DogSE.Common;
//using System.Collections.Concurrent;
using System.Linq;
using TradeAge.Client.Core.Net;

namespace DogSE.Client.Core.Net
{
    /// <summary>
    /// 
    /// </summary>
    public class NetStateManager
    {
        #region zh-CHS 私有常量 | en Private Constants
        /// <summary>
        /// 
        /// </summary>
        private readonly static int HANDLER_CAPACITY_SIZE = 1024;
        #endregion

        #region zh-CHS 共有属性 | en Public Properties
        /// <summary>
        /// 
        /// </summary>
        public int Count
        {
            get { return m_NetStates.Count; }
        }
        #endregion

        #region zh-CHS 内部属性 | en Internal Properties
        #region zh-CHS 私有成员变量 | en Private Member Variables
        /// <summary>
        /// 
        /// </summary>
        private ExclusiveSerial m_ExclusiveSerial = new ExclusiveSerial();

        #endregion
        /// <summary>
        /// 
        /// </summary>
        public ExclusiveSerial ExclusiveSerial
        {
            get { return m_ExclusiveSerial; }
        }
        #endregion
        
        #region zh-CHS 共有静态属性 | en Public Static Properties

        #region zh-CHS 私有静态成员变量 | en Private Static Member Variables
        /// <summary>
        /// 
        /// </summary>
        private static long s_SendMaxSize1Sec = 64 * 1024; // 64K
        #endregion
        /// <summary>
        /// 每秒最大的传输率
        /// </summary>
        public static long SendMaxSize1Sec
        {
            get { return s_SendMaxSize1Sec; }
            set { s_SendMaxSize1Sec = value; }
        }

        #region zh-CHS 私有静态成员变量 | en Private Static Member Variables
        /// <summary>
        /// 
        /// </summary>
        private static long s_SendCachedMaxSize = 5 * s_SendMaxSize1Sec; // 320K
        #endregion
        /// <summary>
        /// 允许最大的传输率堆积率
        /// </summary>
        public static long SendCachedMaxSize
        {
            get { return s_SendCachedMaxSize; }
            set { s_SendCachedMaxSize = value; }
        }


        #region zh-CHS 私有静态成员变量 | en Private Static Member Variables
        /// <summary>
        /// 
        /// </summary>
        private static long s_ReceiveCachedMaxSize = 320 * 1024; // 320K
        #endregion
        /// <summary>
        /// 允许最大的接收堆积率
        /// </summary>
        public static long ReceiveCachedMaxSize
        {
            get { return s_ReceiveCachedMaxSize; }
            set { s_ReceiveCachedMaxSize = value; }
        }
        #endregion

        #region zh-CHS NetState 方法 | en BaseItem Method

        #region zh-CHS 内部方法 | en Internal Methods
        #region zh-CHS 私有成员变量 | en Private Member Variables
        /// <summary>
        /// 网络客户端的集合
        /// </summary>
        private readonly ConcurrentDictionary<long, NetState> m_NetStates = new ConcurrentDictionary<long, NetState>(2, HANDLER_CAPACITY_SIZE);
        #endregion
        /// <summary>
        /// 添加客户端到集合
        /// </summary>
        internal void InternalAddNetState( long netStateId, NetState netState )
        {
            m_NetStates.GetOrAdd( netStateId, netState );
        }

        /// <summary>
        /// 在客户端的集合内删除某客户端
        /// </summary>
        /// <param name="netStateId"></param>
        internal void InternalRemoveNetState( long netStateId )
        {
            NetState netstate;
            m_NetStates.TryRemove(netStateId, out netstate);
        }
        #endregion

        #region zh-CHS 共有方法 | en Public Methods
        /// <summary>
        /// 在客户端的集合内删除某客户端
        /// </summary>
        /// <param name="netStateId"></param>
        public NetState GetNetState( long netStateId )
        {
            NetState netState;

            m_NetStates.TryGetValue( netStateId, out netState );

            return netState;
        }

        /// <summary>
        /// 全部网络客户端的数组
        /// </summary>
        /// <returns></returns>
        public NetState[] ToArray()
        {
            return m_NetStates.Values.ToArray();
        }
        #endregion

        #endregion
    }
}