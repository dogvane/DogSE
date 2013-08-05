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
using System.Collections.Generic;
using System.Threading;
using Demo.Mmose.Core.Common;
using Demo.Mmose.Core.Util;
using Demo.Mmose.Core.Common.Log;
using DogSE.Library.Log;

#endregion

namespace DogSE.Server.Net
{
    /// <summary>
    /// 
    /// </summary>
    internal struct SendBuffer
    {
        #region zh-CHS 共有常量 | en Public Constants
        /// <summary>
        /// 
        /// </summary>
        public readonly static SendBuffer NullBuffer = new SendBuffer(null);
        #endregion

        #region zh-CHS 构造和初始化和清理 | en Constructors and Initializers and Dispose
        /// <summary>
        /// 私有的构造类
        /// </summary>
        private SendBuffer(byte[] buffer)
        {
            m_Buffer = buffer;
            m_Length = 0;
        }
        #endregion

        #region zh-CHS 属性 | en Properties
        #region zh-CHS 私有成员变量 | en Private Member Variables
        /// <summary>
        /// 缓冲区的字节
        /// </summary>
        private byte[] m_Buffer;
        #endregion
        /// <summary>
        /// 缓冲区的字节
        /// </summary>
        public byte[] Buffer
        {
            get { return m_Buffer; }
        }

        #region zh-CHS 私有成员变量 | en Private Member Variables
        /// <summary>
        /// 缓冲区的长度
        /// </summary>
        private long m_Length;
        #endregion
        /// <summary>
        /// 缓冲区已写入的长度
        /// </summary>
        public long Length
        {
            get { return m_Length; }
        }

        /// <summary>
        /// 缓冲区的剩余的有效空间
        /// </summary>
        public long SpareSpace
        {
            get { return (m_Buffer.Length - m_Length); }
        }

        /// <summary>
        /// 缓冲区是否已经满了
        /// </summary>
        public bool IsFull
        {
            get { return (m_Length == m_Buffer.Length); }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool IsNull
        {
            get { return m_Buffer == null; }
        }
        #endregion

        #region zh-CHS 方法 | en Method
        /// <summary>
        /// 
        /// </summary>
        /// <param name="byteBuffer"></param>
        /// <param name="iOffset"></param>
        /// <param name="iLength"></param>
        /// <returns></returns>
        public long Write(byte[] byteBuffer, long lOffset, long lLength)
        {
            // 获取可以写当前缓冲区的字节数
            long iWrite = Math.Min(lLength, SpareSpace);

            // 写入数据
            System.Buffer.BlockCopy(byteBuffer, (int)lOffset, m_Buffer, (int)m_Length, (int)iWrite);

            // 跟新缓冲区的长度
            m_Length += iWrite;

            return iWrite;
        }

        /// <summary>
        /// 
        /// </summary>
        public void Release()
        {
            if (m_Buffer != null)
            {
                // 把数据返回进内存池
                SendQueue.ReleaseBuffer(m_Buffer);

                m_Buffer = null;
                m_Length = 0;
            }
        }
        #endregion

        #region zh-CHS 静态方法 | en Static Method
        /// <summary>
        /// 请求Gram类
        /// </summary>
        /// <returns></returns>
        public static SendBuffer Instance()
        {
            return new SendBuffer(SendQueue.AcquireBuffer());
        }
        #endregion
    }

    /// <summary>
    /// 数据输出包的缓冲区,如果数据过长就等处理缓存发送时发送数据( 小于 MTU 时 )
    /// 之所以小于 MTU 是因为TCP-IP发送数据时总是发送至客户端彼此确认完毕后才通知返回的,如果过大网络延迟时会严重影响网络的通讯.
    /// </summary>
    internal class SendQueue
    {
        #region zh-CHS 私有成员变量 | en Private Member Variables
        #region zh-CHS 私有常量 | en Private Constants
        /// <summary>
        /// 
        /// </summary>
        private readonly static int QUEUE_CAPACITY_SIZE = 1024;
        #endregion
        /// <summary>
        /// 当Flush发出的数据(最后发出的数据块)
        /// </summary>
        private SendBuffer m_FlushBuffer = SendBuffer.NullBuffer;
        /// <summary>
        /// 等待需要发出的数据
        /// </summary>
        private Queue<SendBuffer> m_PendingBuffer = new Queue<SendBuffer>(QUEUE_CAPACITY_SIZE);
        /// <summary>
        /// 等待需要发出的数据锁
        /// </summary>
        private SpinLock m_LockFlushAndPending = new SpinLock();
        #endregion

        #region zh-CHS 属性 | en Properties
        /// <summary>
        /// 当前需发送的数据是否空的
        /// </summary>
        public bool IsEmpty
        {
            get { return (m_PendingBuffer.Count <= 0 && m_FlushBuffer.IsNull == true); }
        }

        #region zh-CHS 私有成员变量 | en Private Member Variables
        /// <summary>
        /// 
        /// </summary>
        private long m_WaitSendSize = 0;
        #endregion
        /// <summary>
        /// 缓冲区内还有多少没有发送的数据
        /// </summary>
        public long WaitSendSize
        {
            get { return m_WaitSendSize; }
        }
        #endregion

        #region zh-CHS 静态属性 | en Static Properties
        #region zh-CHS 私有成员变量 | en Private Member Variables
        /// <summary>
        /// 缓冲的数据大小
        /// 常见的以太网的MTU为: 1500 
        /// PPPoE（ADSL）的MTU 为：1492 
        /// Dial-up（MODEM）的MTU：576 
        /// 一个tcp包最大可传输1432 byte
        /// </summary>
        private static int s_BufferSizeMTU = 1400; // 根据 MTU 来设置
        #endregion
        /// <summary>
        /// 合并缓冲的数据大小
        /// </summary>
        public static int MTU_BufferSize
        {
            get { return s_BufferSizeMTU; }
            set
            {
                if (s_BufferSizeMTU == value)
                    return;

                if (s_BufferSizeMTU <= 0)
                    return;

                s_BufferSizeMTU = value;
                s_UnusedBuffers = new BufferPool(INITIAL_CAPACITY, s_BufferSizeMTU);
            }
        }
        #endregion

        #region zh-CHS 方法 | en Method
        #region zh-CHS 私有成员变量 | en Private Member Variables
        // 可能是windows操作系统的最大可发送的字节数
        //private const int PENDING_MAX_BUFFER = 96 * 1024;
        #endregion
        /// <summary>
        /// 如果数据满了,且缓冲区内的数据是空的则返回需要发送的数据
        /// 调用Enqueue(...)后调用Dequeue(...),不能直接调用Dequeue(...)
        /// </summary>
        /// <param name="byteBuffer"></param>
        /// <param name="iOffset"></param>
        /// <param name="iLength"></param>
        /// <returns></returns>
        public void Enqueue(byte[] byteBuffer, long iOffset, long iLength)
        {
            if (byteBuffer == null)
                throw new ArgumentNullException("byteBuffer", "SendQueue.Enqueue(...) - byteBuffer == null error!");

            if (iOffset < 0 || iOffset >= byteBuffer.Length)
                throw new Exception("SendQueue.Enqueue(...) - iOffset < 0 || iOffset >= byteBuffer.Length error!");

            if (iLength < 0 || iLength > byteBuffer.Length) // 如果iLength == 0就返回空,如果iLength == 0就跳过
                throw new Exception("SendQueue.Enqueue(...) - iLength < 0 || iLength > byteBuffer.Length error!");

            if ((byteBuffer.Length - iOffset) < iLength)
                throw new Exception("SendQueue.Enqueue(...) - ( byteBuffer.Length - iOffset ) < iLength error!");

            SpinLockEx.ReliableEnter(ref m_LockFlushAndPending);
            try
            {
                do
                {
                    if (m_FlushBuffer.IsNull == true)
                    {
                        // nothing yet buffered
                        m_FlushBuffer = SendBuffer.Instance();

                        if (m_FlushBuffer.IsNull == true)
                            throw new Exception("SendQueue.Enqueue(...) - m_FlushBuffer.IsNull == true error!");
                    }

                    // 当前已经写入的字节
                    long iBytesWritten = m_FlushBuffer.Write(byteBuffer, iOffset, iLength);

                    iOffset += iBytesWritten;
                    iLength -= iBytesWritten;

                    // 写入需要发送的数据的大小
                    m_WaitSendSize += iBytesWritten;

                    // 如果数据没有满,且数据写入完毕则退出,返回空,不添加到集合内
                    if (m_FlushBuffer.IsFull == true)
                    {
                        // 如果满了添加到集合内的尾处
                        m_PendingBuffer.Enqueue(m_FlushBuffer);

                        m_FlushBuffer = SendBuffer.NullBuffer;　// 置空再次请求缓存
                    }
                } while (iLength > 0);
            }
            catch (Exception e)
            {
                Logs.Error("SendQueue->Enqueue: Occurred error when enqueuing the data {0} ", e);
            }
            finally
            {
                m_LockFlushAndPending.Exit();
            }
        }

        /// <summary>
        /// 获取当前的数据
        /// </summary>
        /// <returns></returns>
        public SendBuffer Dequeue()
        {
            SendBuffer sendGram = SendBuffer.NullBuffer;

            SpinLockEx.ReliableEnter(ref m_LockFlushAndPending);
            try
            {
                if (m_PendingBuffer.Count > 0)
                {
                    sendGram = m_PendingBuffer.Dequeue();   // 再给出数据
                }
                else if (m_FlushBuffer.IsNull == false)
                {
                    sendGram = m_FlushBuffer; // 再给出数据
                    m_FlushBuffer = SendBuffer.NullBuffer;
                }

                // 移去已发送的数据的大小
                m_WaitSendSize -= sendGram.Length;
            }
            catch (Exception e) { Logs.FatalError("SendQueue->Dequeue: Occurred error when dequeuing the data {0} ", e.Message); }
            finally
            {
                m_LockFlushAndPending.Exit();
            }

            return sendGram;
        }

        /// <summary>
        /// 清除数据
        /// </summary>
        public void Clear()
        {
            SpinLockEx.ReliableEnter(ref m_LockFlushAndPending);
            try
            {
                while (m_PendingBuffer.Count > 0)
                    m_PendingBuffer.Dequeue().Release();

                if (m_FlushBuffer.IsNull == false)
                {
                    m_FlushBuffer.Release();
                    m_FlushBuffer = SendBuffer.NullBuffer;
                }

                // 清空
                m_WaitSendSize = 0;
            }
            finally
            {
                m_LockFlushAndPending.Exit();
            }
        }
        #endregion

        #region zh-CHS 私有静态方法 | en Private Static Method
        #region zh-CHS 私有静态成员变量 | en Private Static Member Variables
        #region zh-CHS 私有常量 | en Private Constants
        /// <summary>
        /// 
        /// </summary>
        private readonly static int INITIAL_CAPACITY = 4096;
        #endregion
        /// <summary>
        /// 
        /// </summary>
        private static BufferPool s_UnusedBuffers = new BufferPool(INITIAL_CAPACITY, s_BufferSizeMTU);
        #endregion
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        internal static byte[] AcquireBuffer()
        {
            return s_UnusedBuffers.AcquireBuffer();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="byteBuffer"></param>
        internal static void ReleaseBuffer(byte[] byteBuffer)
        {
            if (byteBuffer == null)
                throw new ArgumentNullException("byteBuffer", "SendQueue.ReleaseBuffer(...) - byteBuffer == null error!");

            if (byteBuffer.Length >= s_BufferSizeMTU) // 可能修改过m_CoalesceBufferSize如果不同就抛弃它
                s_UnusedBuffers.ReleaseBuffer(byteBuffer);
        }
        #endregion
    }
}
#endregion