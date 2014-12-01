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
#if NET40
using System.Collections.Concurrent;
#endif

using System.Text;
using System.Threading;
using DogSE.Library.Log;

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
        /// 内存池的初始容量
        /// </summary>
        private readonly long m_InitialCapacity;

        /// <summary>
        /// 内存池
        /// </summary>
        private ConcurrentStack<T> m_FreePool = new ConcurrentStack<T>();

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
                m_FreePool.Push(new T());

            ObjectPoolStateInfo.Add(this);
        }

        /// <summary>
        /// 
        /// </summary>
        ~ObjectPool()
        {            
            StringBuilder ret = new StringBuilder(512);
            ret.AppendFormat("{0}\r\n", Name);
            ret.AppendFormat("FreeCount:{0}\r\n", m_FreePool.Count);
            ret.AppendFormat("InitialCapacity:{0}\r\n", m_InitialCapacity);
            ret.AppendFormat("NewCount:{0}\r\n", newCount);
            ret.AppendFormat("AcquireCount:{0}\r\n", acquireCount);
            ret.AppendFormat("ReleaseCount:{0}\r\n", releaseCount);
            Logs.Info(ret.ToString());
            Console.WriteLine(ret.ToString());
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

        private int acquireCount;

        private int releaseCount;

        private int newCount;

        #region zh-CHS 共有方法 | en Public Methods

        /// <summary>
        /// 内存池请求数据
        /// </summary>
        /// <returns></returns>
        public T AcquireContent()
        {
            T returnT;

            bool bIsCompareExchange = false;
            bool bCompareExchangeResult = false;
            int iTryAcquireCount = 0;
            long iMisses = m_Misses;
            do
            {
                if (m_FreePool.TryPop(out returnT))                
                    break;

                ++iTryAcquireCount;

                if (bIsCompareExchange == false)
                {
                    if (Interlocked.CompareExchange(ref m_Misses, iMisses + 1, iMisses) == iMisses)
                        bCompareExchangeResult = true;

                    bIsCompareExchange = true;
                }

                long iMaxTryAcquireCount = m_InitialCapacity / 2;

                // 申请缓存
                if (iTryAcquireCount >= iMaxTryAcquireCount || bCompareExchangeResult)
                {
                    for (int iIndex = 0; iIndex < m_InitialCapacity; ++iIndex)
                    {
                        newCount++;
                        m_FreePool.Push(new T());
                    }
                }
                else
                    continue;

                if (m_FreePool.TryPop(out returnT))
                    break;

                // 重置数据
                bIsCompareExchange = false;
                bCompareExchangeResult = false;
                iTryAcquireCount = 0;
                iMisses = m_Misses;
            } while (true);

            acquireCount++;

            return returnT;
        }

        /// <summary>
        /// 内存池释放数据
        /// </summary>
        /// <param name="content"></param>
        public void ReleaseContent(T content)
        {
            if (content == null)
                throw new ArgumentNullException("content",
                                                "MemoryPool.ReleasePoolContent(...) - contentT == null error!");
            releaseCount++;
            m_FreePool.Push(content);
        }

        /// <summary>
        /// 释放内存池内全部的数据
        /// </summary>
        public void Free()
        {
            m_FreePool = new ConcurrentStack<T>();
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
                           CurrentCapacity = m_InitialCapacity + newCount,
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
