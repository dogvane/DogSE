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
using System.Threading;

#endregion

namespace DogSE.Client.Core.Net
{
    /// <summary>
    /// 需发送的数据包的基类
    /// </summary>
    public abstract class Packet
    {
        #region zh-CHS 构造和初始化和清理 | en Constructors and Initializers and Dispose
        /// <summary>
        /// 通过调用EnsureCapacity(...)来产生m_Stream
        /// </summary>
        /// <param name="iPacketID"></param>
        public Packet( ushort iPacketID )
        {
            m_PacketID  = iPacketID;
            m_Stream = new PacketWriter(iPacketID);

            PacketProfile packetProfile = PacketProfile.GetOutgoingProfile( iPacketID );
            if ( packetProfile != null )
                packetProfile.RegConstruct();
        }

        /// <summary>
        /// 通过调用EnsureCapacity(...)来产生m_Stream
        /// </summary>
        /// <param name="iPacketID"></param>
        /// <param name="len"></param>
        public Packet(ushort iPacketID, int len = 0)
        {
            m_PacketID = iPacketID;
            m_Stream = new PacketWriter(iPacketID);

            PacketProfile packetProfile = PacketProfile.GetOutgoingProfile(iPacketID);
            if (packetProfile != null)
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
                PacketWriter.ReleaseContent(m_Stream);
                //m_Stream.Dispose();
                m_Stream = null;
            }
        }

        #endregion
    }

}

