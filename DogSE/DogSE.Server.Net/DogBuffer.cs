using System;
using System.Collections.Generic;
using System.Threading;
using DogSE.Library.Common;
using DogSE.Library.Log;

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
        ~DogBuffer()
        {

        }

#if DEBUG
        private static int s_id = 0;

        private int m_id = 0;
#endif

        /// <summary>
        /// 
        /// </summary>
        public DogBuffer()
        {
            m_buffer = new byte[1024*4];
            BuffSizeType = DogBufferType._4K;
#if DEBUG
            m_id = s_id++;
#endif
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
        /// <param name="minSize">扩大的最小尺寸</param>
        public void UpdateCapacity(int minSize = 0)
        {
            int newSize;
            if (minSize == 0)
                newSize = m_buffer.Length * 2;
            else
            {
                newSize = FixSize(minSize);
                Logs.Info("UpdateCapacity size={0} newsize={1}", minSize, newSize);
            }

            var newBuffer = new byte[newSize];

            Buffer.BlockCopy(m_buffer, 0, newBuffer, 0, m_buffer.Length);

            m_buffer = newBuffer;
        }

        /// <summary>
        /// 按照4K对齐
        /// </summary>
        /// <param name="minSize"></param>
        /// <returns></returns>
        private int FixSize(int minSize)
        {
            return (minSize/4096 + 1)*4096;
        }

        private int referenceCounter;

        /// <summary>
        /// 标记使用
        /// 如果对象是通过 DogBufferPool 获得对象，则不用调用该方法
        /// 如果是参数传入，并且需要使用它的byte数组，则需要先Use，再Release
        /// </summary>
        public void Use()
        {
            //if (referenceCounter == 0)
            //    Logs.Error("buff use() referenceCounter is zero.");

            //Interlocked.Increment(ref referenceCounter);
            referenceCounter++;

            //var stack = new System.Diagnostics.StackTrace(0);
            //var name = stack.GetFrame(1).GetMethod().Name;
            //if (name == "GetFromPool32K" || name == "GetFromPool4K")
            //    name = stack.GetFrame(2).GetMethod().Name;
            //Logs.Debug("use buffer id = {0} counter={1}  strace = {2}", m_id, referenceCounter, name);
        }

        /// <summary>
        /// 当不再使用缓冲区时，需要手动释放，对象会自动返回对象池里
        /// </summary>
        public void Release()
        {
            lock (lockOjb)
            {
                //Interlocked.Decrement(ref referenceCounter);
                referenceCounter--;

                //var stack = new System.Diagnostics.StackTrace(0);
                //Logs.Info("release buffer id = {0} counter={1}  strace = {2}", m_id, referenceCounter, stack.GetFrame(1).GetMethod().Name);

                if (referenceCounter < 0)
                {
                    Logs.Error("重复释放。");
                    referenceCounter = 0;

#if DEBUG
                    var stack = new System.Diagnostics.StackTrace(0);
                    Logs.Info("release buffer id = {0} counter={1}  strace = {2}", m_id, referenceCounter,
                        stack.GetFrame(1).GetMethod().Name);
#endif
                    return;
                }

                if (referenceCounter == 0)
                {
                    ReleaseToPool(this);
                }
            }
        }

        private static readonly object lockOjb = new object();

        /// <summary>
        /// 对象池
        /// </summary>
        static readonly ObjectPool<DogBuffer> s_pools = new ObjectPool<DogBuffer>(256, 1024);

        /// <summary>
        /// 对象池
        /// </summary>
        static readonly ObjectPool<DogBuffer32K> s_pools32K = new ObjectPool<DogBuffer32K>(256, 2048);

        /// <summary>
        /// 从缓冲池里获得数据
        /// </summary>
        /// <returns></returns>
        public static DogBuffer GetFromPool4K()
        {
            lock (lockOjb)
            {
                start:
                var ret = s_pools.AcquireContent();
                if (ret.referenceCounter != 0)
                {
                    Logs.Error("dog buffer4k is used. counter = {0}", ret.referenceCounter);
#if DEBUG
                    var stack = new System.Diagnostics.StackTrace(0);
                    Logs.Info("dog buffer is used. strace = {0}", stack.ToString());
#endif
                    goto start;
                }

                ret.Use();
                //ret.referenceCounter++;
                ret.Length = 0;
                return ret;
            }
        }


        internal static void ReleaseToPool(DogBuffer bufff)
        {
            //lock (lockOjb)
            {
                bufff.Length = 0;
                if (bufff.BuffSizeType == DogBufferType._4K)
                    s_pools.ReleaseContent(bufff);
                else if (bufff.BuffSizeType == DogBufferType._32K)
                {
                    var _32buf = bufff as DogBuffer32K;
                    s_pools32K.ReleaseContent(_32buf);
                }
            }
        }

        internal static void ReleaseToPool(DogBuffer32K bufff)
        {
            lock (lockOjb)
            {
                bufff.Length = 0;
                s_pools32K.ReleaseContent(bufff);
            }
        }

        /// <summary>
        /// 从缓冲池里获得数据
        /// </summary>
        /// <returns></returns>
        public static DogBuffer32K GetFromPool32K()
        {
            lock (lockOjb)
            {
                start1:
                var ret = s_pools32K.AcquireContent();

                if (ret.referenceCounter != 0)
                {
                    Logs.Error("dog buffer32 is used.");
                    //Logs.Error(string.Format("buff is exists {0} exitstCount={1}", exitsList.Contains(ret).ToString(), exitsList.Count));
#if DEBUG
                    var stack = new System.Diagnostics.StackTrace(0);
                    Logs.Error("dog buffer is used. strace = {0}", stack.ToString());
#endif
                    goto start1;
                }
                //exitsList.Add(ret);
                //ret.referenceCounter++;
                ret.Use();
                ret.Length = 0;
                return ret;
            }
        }
    }


}
