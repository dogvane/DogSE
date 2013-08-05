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
using System.Threading;
using Demo.Mmose.Core.Common;
#endregion

namespace DogSE.Server.Net
{
    /// <summary>
    /// 
    /// </summary>
    public struct PacketBuffer
    {
        #region zh-CHS 共有常量 | en Public Constants
        /// <summary>
        /// 
        /// </summary>
        public readonly static PacketBuffer NullPacketBuffer = new PacketBuffer( null, 0 );
        #endregion

        #region zh-CHS 构造和初始化和清理 | en Constructors and Initializers and Dispose
        /// <summary>
        /// 
        /// </summary>
        /// <param name="byteBuffer"></param>
        /// <param name="lLength"></param>
        internal PacketBuffer( byte[] byteBuffer, long lLength )
        {
            m_Buffer = byteBuffer;
            m_lLength = lLength;
        }
        #endregion

        #region zh-CHS 共有属性 | en Public Properties
        #region zh-CHS 私有成员变量 | en Private Member Variables
        /// <summary>
        /// 
        /// </summary>
        private long m_lLength;
        #endregion
        /// <summary>
        /// 
        /// </summary>
        public long Length
        {
            get { return m_lLength; }
        }

        #region zh-CHS 私有成员变量 | en Private Member Variables
        /// <summary>
        /// 
        /// </summary>
        private byte[] m_Buffer;
        #endregion
        /// <summary>
        /// 
        /// </summary>
        public byte[] Buffer
        {
            get { return m_Buffer; }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool IsNULL
        {
            get { return m_Buffer == null || m_lLength <= 0; }
        }
        #endregion
    }

    /// <summary>
    /// 需发送的数据包的基类
    /// </summary>
    [MultiThreadedNoSupport( "zh-CHS", "当前的类所有成员没有锁定(只在局部创建需发送的数据包),不支持多线程操作" )]
    public abstract class Packet
    {
        #region zh-CHS 私有成员变量 | en Private Member Variables
        /// <summary>
        /// 输出包的长度
        /// </summary>
        private long m_PacketLength = 0;
        #endregion

        #region zh-CHS 构造和初始化和清理 | en Constructors and Initializers and Dispose
        /// <summary>
        /// 通过调用EnsureCapacity(...)来产生m_Stream
        /// </summary>
        /// <param name="iPacketID"></param>
        public Packet( long iPacketID )
        {
            m_PacketID  = iPacketID;
            m_PacketLength  = 0;
            m_Stream    = PacketWriter.Instance();  // 通过InternalCompile(...)的PacketWriter.Release(...)返回进入空闲的集合

            PacketProfile packetProfile = PacketProfile.GetOutgoingProfile( iPacketID );
            if ( packetProfile != null )
                packetProfile.RegConstruct();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="iPacketID"></param>
        /// <param name="iLength"></param>
        public Packet( long iPacketID, long iPacketLength )
        {
            m_PacketID  = iPacketID;
            m_PacketLength  = iPacketLength;
            m_Stream    = PacketWriter.Instance( iPacketLength );  // 通过InternalCompile(...)的PacketWriter.Release(...)返回进入空闲的集合

            PacketProfile packetProfile = PacketProfile.GetOutgoingProfile( iPacketID );
            if ( packetProfile != null )
                packetProfile.RegConstruct();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="iPacketID"></param>
        /// <param name="iLength"></param>
        public Packet( long iPacketID, long iPacketLength, long iCapacity )
        {
            m_PacketID = iPacketID;
            m_PacketLength = iPacketLength;
            m_Stream = PacketWriter.Instance( iCapacity );  // 通过InternalCompile(...)的PacketWriter.Release(...)返回进入空闲的集合

            PacketProfile packetProfile = PacketProfile.GetOutgoingProfile( iPacketID );
            if ( packetProfile != null )
                packetProfile.RegConstruct();
        }

        #endregion

        #region zh-CHS 属性 | en Properties
        #region zh-CHS 私有成员变量 | en Private Member Variables
        /// <summary>
        /// 输出包的ID
        /// </summary>
        private long m_PacketID;
        #endregion
        /// <summary>
        /// 
        /// </summary>
        public long PacketID
        {
            get { return m_PacketID; }
        }

        #region zh-CHS 私有成员变量 | en Private Member Variables
        /// <summary>
        /// 输出包的输出流
        /// </summary>
        private PacketWriter m_Stream;
        #endregion
        /// <summary>
        /// 输出包的输出流
        /// </summary>
        public PacketWriter WriterStream
        {
            get { return m_Stream; }
        }

        #region zh-CHS 私有成员变量 | en Private Member Variables
        /// <summary>
        /// 数据包的优先级
        /// </summary>
        private PacketPriority m_PacketPriority = PacketPriority.Normal;
        #endregion
        /// <summary>
        /// 数据包的优先级
        /// </summary>
        public PacketPriority PacketPriority
        {
            get { return m_PacketPriority; }
        }

        /// <summary>
        /// 
        /// </summary>
        public Endian Endian
        {
            get { return m_Stream.Endian; }
            set { m_Stream.Endian = value; }
        }
        #endregion

        #region zh-CHS 方法 | en Method
        /// <summary>
        /// 加密输出信息包的数据
        /// </summary>
        /// <param name="iOutLength">返回压缩或没有压缩后的数据长度</param>
        /// <returns>返回压缩或没有压缩数据</returns>
        public PacketBuffer AcquireBuffer()
        {
            if ( m_Stream == null )
                throw new ArgumentNullException( "m_Stream", "Packet.AcquireCompile(...) - m_Stream == null error!" );

            if ( m_PacketLength <= 0 )
                m_PacketLength = WriterStream.Length;
            else if ( WriterStream.Length != m_PacketLength )
            {
                long iDiff = WriterStream.Length - m_PacketLength;

                throw new Exception( string.Format( "Packet.AcquireCompile(...) - WriterStream.Length != m_Length | 0x{0:X2} ('{1}') (length={2} LengthDiff={3}{4})| error! 信息包的长度与需要发送的预先定义的信息包长度不相符!", m_PacketID, GetType().Name, m_PacketLength, iDiff >= 0 ? "+" : "-", iDiff ) );
            }

            byte[] returnBuffer = m_Stream.Stream.GetBuffer();
            if ( returnBuffer == null )
                throw new ArgumentNullException( "returnBuffer", string.Format( "Packet.AcquireCompile(...) - returnBuffer == null | 0x{0:X2} ('{1}') (length={2})| error!", m_PacketID, GetType().Name, m_PacketLength ) );

            return new PacketBuffer( returnBuffer, m_Stream.Length );
        }

        #region zh-CHS 私有成员变量 | en Private Member Variables
        /// <summary>
        /// 
        /// </summary>
        private int m_bIsRelease;
        #endregion
        /// <summary>
        /// 表示数据已发送完成,释放请求过的加解压缩的内存
        /// </summary>
        public void Release()
        {
            // 检测有没有调用过Release(...)函数
            if ( Interlocked.Exchange( ref m_bIsRelease, 1 ) == 1 )
                return;

            if ( m_Stream != null )
            {
                m_Stream.Release(); // 返回进入空闲的集合
                m_Stream = null;
            }
        }
        #endregion
    }


    /// <summary>
    /// 
    /// </summary>
    public class BufferPacket : Packet
    {
        #region zh-CHS 构造和初始化和清理 | en Constructors and Initializers and Dispose
        /// <summary>
        /// 
        /// </summary>
        /// <param name="iPacketID"></param>
        public BufferPacket(byte[] byteBuffer, int iOffset, int iSize)
            : base(0)
        {
            //////////////////////////////////////////////////////////////////////////

            WriterStream.Write(byteBuffer, iOffset, iSize);

            //////////////////////////////////////////////////////////////////////////
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="iPacketID"></param>
        public BufferPacket(Packet packet)
            : base(0)
        {
            //////////////////////////////////////////////////////////////////////////

            WriterStream.Write(packet.WriterStream.Stream.GetBuffer(), 0, (int)packet.WriterStream.Stream.Length);

            //////////////////////////////////////////////////////////////////////////
        }
        #endregion
    }

}
#endregion

