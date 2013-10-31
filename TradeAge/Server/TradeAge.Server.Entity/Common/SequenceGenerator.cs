using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TradeAge.Server.Entity.Common
{
    /// <summary>
    /// int序列生成器
    /// </summary>
    public class IntSequenceGenerator
    {
        private int m_min;
        private int m_gen;

        private readonly object m_genLock = new object();

        /// <summary>
        /// 初始化
        /// </summary>
        public IntSequenceGenerator(int minSeq)
        {
            m_min = minSeq;
        }

        /// <summary>
        /// 获得下一个序列
        /// </summary>
        /// <returns></returns>
        public int NextSeq()
        {
            lock (m_genLock)
            {
                m_gen++;
            }

            return m_min + m_gen;
        }
    }

    /// <summary>
    /// 长整形的序列生成器
    /// </summary>
    public class LongSequenceGenerator
    {
        private long m_min;
        private long m_gen;

        private readonly object m_genLock = new object();

        /// <summary>
        /// 初始化
        /// </summary>
        public LongSequenceGenerator(long minSeq)
        {
            m_min = minSeq;
        }

        /// <summary>
        /// 获得下一个序列
        /// </summary>
        /// <returns></returns>
        public long NextSeq()
        {
            lock (m_genLock)
            {
                m_gen++;
            }

            return m_min + m_gen;
        }
    }


}
