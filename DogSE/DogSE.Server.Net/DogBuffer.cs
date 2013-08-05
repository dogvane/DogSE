using System;
using DogSE.Library.Common;

namespace DogSE.Server.Net
{
    /// <summary>
    /// Dog引擎专属缓冲区
    /// </summary>
    public class DogBuffer32K:DogBuffer
    {
        /// <summary>
        /// Dog引擎专属缓冲区
        /// </summary>
        public DogBuffer32K()
        {
            m_buffer = new byte[1024 * 32];
            BuffSizeType = DogBufferType._32K;
        }
    }

    /// <summary>
    /// 缓冲区的长度尺寸
    /// </summary>
    internal enum DogBufferType
    {
        _4K,

        _32K,
    }

    /// <summary>
    /// Dog引擎专属缓冲区
    /// </summary>
    public class DogBuffer
    {
        /// <summary>
        /// 
        /// </summary>
        public DogBuffer()
        {
            m_buffer = new byte[1024*4];
            BuffSizeType = DogBufferType._4K;
        }

        internal DogBufferType BuffSizeType { get; set; }

        /// <summary>
        /// 字节的数组
        /// </summary>
        protected byte[] m_buffer;


        /// <summary>
        /// 缓冲区对应的字节数组长度
        /// </summary>
        public Byte[] Bytes
        {
            get { return m_buffer; }
        }

        /// <summary>
        /// 当前使用的数据长度
        /// </summary>
        public int Length { get; set; }

        /// <summary>
        /// 扩大缓冲数据的大小(注意，扩大后，Byte返回的数组的引用将不同)
        /// </summary>
        public void UpdateCapacity()
        {
            var newBuffer = new byte[m_buffer.Length * 2];

            Buffer.BlockCopy(m_buffer, 0, newBuffer, 0, m_buffer.Length);

            m_buffer = newBuffer;
        }

        private int referenceCounter;

        /// <summary>
        /// 标记使用
        /// 如果对象是通过 DogBufferPool 获得对象，则不用调用该方法
        /// 如果是参数传入，并且需要使用它的byte数组，则需要先Use，再Release
        /// </summary>
        public void Use()
        {
            referenceCounter++;
        }

        /// <summary>
        /// 当不再使用缓冲区时，需要手动释放，对象会自动返回对象池里
        /// </summary>
        public void Release()
        {
            referenceCounter--;
            if (referenceCounter < 0)
            {
                referenceCounter = 0;
                return;
            }

            if (referenceCounter == 0)
            {
                ReleaseToPool(this);
            }

        }


        /// <summary>
        /// 对象池
        /// </summary>
        static readonly ObjectPool<DogBuffer> s_pools = new ObjectPool<DogBuffer>(1024 * 2);

        /// <summary>
        /// 对象池
        /// </summary>
        static readonly ObjectPool<DogBuffer32K> s_pools32K = new ObjectPool<DogBuffer32K>(1024 * 2);

        /// <summary>
        /// 从缓冲池里获得数据
        /// </summary>
        /// <returns></returns>
        public static DogBuffer GetFromPool4K()
        {
            var ret = s_pools.AcquireContent();
            ret.Use();
            ret.Length = 0;
            return ret;
        }


        internal static void ReleaseToPool(DogBuffer bufff)
        {
            bufff.Length = 0;
            if (bufff.BuffSizeType == DogBufferType._4K)
                s_pools.ReleaseContent(bufff);
            else if (bufff.BuffSizeType == DogBufferType._32K)
                s_pools32K.ReleaseContent(bufff as DogBuffer32K);
        }

        /// <summary>
        /// 从缓冲池里获得数据
        /// </summary>
        /// <returns></returns>
        public static DogBuffer32K GetFromPool32K()
        {
            var ret = s_pools32K.AcquireContent();
            ret.Use();
            ret.Length = 0;
            return ret;
        }
    }
}
