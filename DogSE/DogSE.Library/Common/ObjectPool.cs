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


using System;
using System.Threading;

namespace DogSE.Library.Common
{
    /// <summary>
    /// 对象池
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ObjectPool<T> : IPoolInfo where T : new()
    {

        #region zh-CHS 私有成员变量 | en Private Member Variables

        /// <summary>
        /// 内存池的容量
        /// </summary>
        private readonly long m_InitialCapacity;

        /// <summary>
        /// 内存池
        /// </summary>
        private ConcurrentQueue<T> m_FreePool = new ConcurrentQueue<T>();

        /// <summary>
        /// 内存池的容量不足时再次请求数据的次数
        /// </summary>
        private long m_Misses;

        #endregion

        #region zh-CHS 构造和初始化和清理 | en Constructors and Initializers and Dispose

        /// <summary>
        /// 初始化内存池
        /// </summary>
        /// <param name="iInitialCapacity">初始化内存池对象的数量</param>
        public ObjectPool(long iInitialCapacity = 1024)
        {
            m_InitialCapacity = iInitialCapacity;

            for (int iIndex = 0; iIndex < iInitialCapacity; ++iIndex)
                m_FreePool.Enqueue(new T());

            ObjectPoolStateInfo.Add(this);
        }

        /// <summary>
        /// 初始化内存池
        /// </summary>
        /// <param name="name">对象池的名字</param>
        /// <param name="iInitialCapacity">初始化内存池对象的数量</param>
        public ObjectPool(string name, long iInitialCapacity = 1024)
            :this(iInitialCapacity)
        {
            m_Name = name;
        }

        #endregion

        #region zh-CHS 共有方法 | en Public Methods

        /// <summary>
        /// 内存池请求数据
        /// </summary>
        /// <returns></returns>
        public T AcquireContent()
        {
            T returnT;

            long iMaxTryAcquireCount = m_InitialCapacity/2;

            bool bIsCompareExchange = false;
            bool bCompareExchangeResult = false;
            int iTryAcquireCount = 0;
            long iMisses = m_Misses;
            do
            {
                if (m_FreePool.TryDequeue(out returnT))
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
                        m_FreePool.Enqueue(new T());
                }
                else
                    continue;

                if (m_FreePool.TryDequeue(out returnT))
                    break;

                // 重置数据
                bIsCompareExchange = false;
                bCompareExchangeResult = false;
                iTryAcquireCount = 0;
                iMisses = m_Misses;
            } while (true);

            return returnT;
        }

        /// <summary>
        /// 内存池释放数据
        /// </summary>
        /// <param name="contentT"></param>
        public void ReleaseContent(T contentT)
        {
            if (contentT == null)
                throw new ArgumentNullException("contentT",
                                                "MemoryPool.ReleasePoolContent(...) - contentT == null error!");

            m_FreePool.Enqueue(contentT);
        }

        /// <summary>
        /// 释放内存池内全部的数据
        /// </summary>
        public void Free()
        {
            m_FreePool = new ConcurrentQueue<T>();
        }


        /// <summary>
        /// 给出内存池的详细信息
        /// </summary>
        /// <returns></returns>
        public PoolInfo GetPoolInfo()
        {
            // 不需要锁定的，因为只是给出没有修改数据
            return new PoolInfo
                       {
                           FreeCount = m_FreePool.Count,
                           InitialCapacity = m_InitialCapacity,
                           CurrentCapacity = m_InitialCapacity*(1 + m_Misses) /* m_Misses是从零开始计算的因此需加1*/,
                           Misses = m_Misses
                       };
        }

        #endregion

        #region IPoolInfo 成员

        private string m_Name;

        /// <summary>
        /// 对象池的名字
        /// </summary>
        public string Name
        {
            get
            {
                if (m_Name == null)
                    m_Name = typeof (T).Name;
                return m_Name;
            }
        }

        #endregion
    }
}
