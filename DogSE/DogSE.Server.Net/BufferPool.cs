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

using System;
using System.Collections.Concurrent;
using System.Threading;

#endregion

namespace DogSE.Server.Net
{
    /// <summary>
    /// Byte的内存池
    /// </summary>
    public class BufferPool
    {
        #region zh-CHS 共有的结构 | en Public Struct

        /// <summary>
        /// 
        /// </summary>
        public struct PoolInfo
        {
            #region zh-CHS 共有属性 | en Public Properties

            /// <summary>
            /// 
            /// </summary>
            public long BufferSize { get; internal set; }

            /// <summary>
            /// 
            /// </summary>
            public long FreeCount { get; internal set; }

            /// <summary>
            /// 
            /// </summary>
            public long InitialCapacity { get; internal set; }

            /// <summary>
            /// 
            /// </summary>
            public long CurrentCapacity { get; internal set; }

            /// <summary>
            /// 
            /// </summary>
            public long Misses { get; internal set; }

            #endregion
        }

        #endregion

        #region zh-CHS 私有成员变量 | en Private Member Variables

        /// <summary>
        /// 数据的大小
        /// </summary>
        private readonly long m_BufferSize;

        /// <summary>
        /// 内存池
        /// </summary>
        private readonly ConcurrentQueue<byte[]> m_FreeBuffers = new ConcurrentQueue<byte[]>();

        /// <summary>
        /// 初始的空间大小
        /// </summary>
        private readonly long m_InitialCapacity;

        /// <summary>
        /// 数据获取的失败次数
        /// </summary>
        private long m_Misses;

        #endregion

        #region zh-CHS 构造和初始化和清理 | en Constructors and Initializers and Dispose

        /// <summary>
        /// 初始化构造
        /// </summary>
        /// <param name="iInitialCapacity"></param>
        /// <param name="iBufferSize"></param>
        public BufferPool(long iInitialCapacity, long iBufferSize)
        {
            m_BufferSize = iBufferSize;
            m_InitialCapacity = iInitialCapacity;

            for (int iIndex = 0; iIndex < iInitialCapacity; ++iIndex)
                m_FreeBuffers.Enqueue(new byte[iBufferSize]);
        }

        #endregion

        #region zh-CHS 方法 | en Method

        /// <summary>
        /// 内存池请求数据
        /// </summary>
        /// <returns></returns>
        public byte[] AcquireBuffer()
        {
            byte[] returnByteArray;

            long iMaxTryAcquireCount = m_InitialCapacity/2;

            bool bIsCompareExchange = false;
            bool bCompareExchangeResult = false;
            int iTryAcquireCount = 0;
            long iMisses = m_Misses;
            do
            {
                if (m_FreeBuffers.TryDequeue(out returnByteArray))
                    break;

                ++iTryAcquireCount;

                if (bIsCompareExchange == false)
                {
                    if (Interlocked.CompareExchange(ref m_Misses, iMisses + 1, iMisses) == iMisses)
                        bCompareExchangeResult = true;

                    bIsCompareExchange = true;
                }

                // 申请缓存
                if (iTryAcquireCount >= iMaxTryAcquireCount || bCompareExchangeResult)
                {
                    for (int iIndex = 0; iIndex < m_InitialCapacity; ++iIndex)
                        m_FreeBuffers.Enqueue(new byte[m_BufferSize]);
                }
                else
                    continue;

                if (m_FreeBuffers.TryDequeue(out returnByteArray))
                    break;

                // 重置数据
                bIsCompareExchange = false;
                bCompareExchangeResult = false;
                iTryAcquireCount = 0;
                iMisses = m_Misses;
            } while (true);

            return returnByteArray;
        }

        /// <summary>
        /// 内存池请释放
        /// </summary>
        /// <param name="byteBuffer"></param>
        public void ReleaseBuffer(byte[] byteBuffer)
        {
            if (byteBuffer == null)
                throw new ArgumentNullException("byteBuffer",
                                                "BufferPool.ReleaseBuffer(...) - byteBuffer == null error!");

            m_FreeBuffers.Enqueue(byteBuffer);
        }


        /// <summary>
        /// 获取内存池的详细信息
        /// </summary>
        /// <returns></returns>
        public PoolInfo GetPoolInfo()
        {
            // 不用锁定的,因只是读取数据而已
            return new PoolInfo
                       {
                           Misses = m_Misses,
                           BufferSize = m_BufferSize,
                           FreeCount = m_FreeBuffers.Count,
                           InitialCapacity = m_InitialCapacity,
                           CurrentCapacity = m_InitialCapacity*(m_Misses + 1)
                       };
        }

        #endregion
    }
}

#endregion