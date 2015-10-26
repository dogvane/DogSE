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
using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using DogSE.Library.Common;
using DogSE.Library.Util;

#endregion

namespace DogSE.Client.Core.Net
{
    /// <summary>
    /// 计算机如何存储大数值的体系结构
    /// </summary>
    public enum Endian
    {
        /// <summary>
        ///  Intel x86，AMD64，DEC VAX
        /// </summary>
        LITTLE_ENDIAN = 0,
        /// <summary>
        /// Sun SPARC, Motorola 68000，Java Virtual Machine
        /// </summary>
        BIG_ENDIAN = 1,
    }

    /// <summary>
    /// 数据包的数据读取
    /// </summary>
    public class PacketReader
    {
        #region zh-CHS 构造和初始化和清理 | en Constructors and Initializers and Dispose

        /// <summary>
        /// 
        /// </summary>
        /// <param name="buffer"></param>
        public void SetBuffer(DogBuffer buffer)
        {
            m_Data  = buffer.Bytes;
            m_Size  = buffer.Length;
            m_Index = ReceiveQueue.PacketLengthSize + 2;    // 包头的长度和消息码长度
            m_buffer = buffer;
        }


        /// <summary>
        /// 写入留包
        /// </summary>
        public PacketReader()
        {

        }

        private static ObjectPool<PacketReader> s_pool = new ObjectPool<PacketReader>(32);

        /// <summary>
        /// 从缓冲池里分配一个消息包
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns></returns>
        public static PacketReader AcquireContent(DogBuffer buffer)
        {
            var ret = s_pool.AcquireContent();
            ret.SetBuffer(buffer);
            return ret;
        }

        /// <summary>
        /// 回收一个写留包
        /// </summary>
        /// <param name="packat"></param>
        public static void ReleaseContent(PacketReader packat)
        {
            packat.Dispose();
            s_pool.ReleaseContent(packat);
        }


        private DogBuffer m_buffer;

        #endregion

        #region zh-CHS 属性 | en Properties
        #region zh-CHS 私有成员变量 | en Private Member Variables
        /// <summary>
        /// 数据缓存
        /// </summary>
        private byte[] m_Data;
        #endregion
        /// <summary>
        /// 数据缓存
        /// </summary>
        public byte[] Buffer
        {
            get { return m_Data; }
        }

        #region zh-CHS 私有成员变量 | en Private Member Variables
        /// <summary>
        /// 数据缓存内的实际数据的大小
        /// </summary>
        private long m_Size;
        #endregion
        /// <summary>
        /// 数据缓存内的实际数据的大小
        /// </summary>
        public long Size
        {
            get { return m_Size; }
        }

        #region zh-CHS 私有成员变量 | en Private Member Variables
        /// <summary>
        /// 数据缓存的实际数据的索引
        /// </summary>
        private long m_Index;
        #endregion
        /// <summary>
        /// 数据缓存的实际数据的索引
        /// </summary>
        public long Position
        {
            get { return m_Index; }
        }

        #region zh-CHS 私有成员变量 | en Private Member Variables
        /// <summary>
        /// 
        /// </summary>
        private Endian m_Endian = Endian.BIG_ENDIAN;
        #endregion
        /// <summary>
        /// 
        /// </summary>
        public Endian Endian
        {
            get { return m_Endian; }
            set { m_Endian = value; }
        }
        #endregion

        #region zh-CHS 方法 | en Method


        /// <summary>
        /// 
        /// </summary>
        /// <param name="iOffset"></param>
        /// <param name="seekOrigin"></param>
        /// <returns></returns>
        public long Seek( long iOffset, SeekOrigin seekOrigin )
        {
            switch ( seekOrigin )
            {
                case SeekOrigin.Begin:
                    m_Index = iOffset;
                    break;
                case SeekOrigin.Current:
                    m_Index += iOffset;
                    break;
                case SeekOrigin.End:
                    m_Index = m_Size - iOffset;
                    break;
            }

            return m_Index;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public long ReadLong64()
        {
            if ( ( m_Index + 8 ) > m_Size )
                return 0;

            if ( m_Endian == Endian.LITTLE_ENDIAN )
                return ( (long)m_Data[m_Index++] << 56 ) | ( (long)m_Data[m_Index++] << 48 ) | ( (long)m_Data[m_Index++] << 40 ) | ( (long)m_Data[m_Index++] << 32 ) | ( (uint)( m_Data[m_Index++] << 24 ) ) | ( (uint)m_Data[m_Index++] << 16 ) | ( (uint)m_Data[m_Index++] << 8 ) | m_Data[m_Index++];
            else
                return m_Data[m_Index++] | ((long)m_Data[m_Index++] << 8) | ((long)m_Data[m_Index++] << 16) | ((long)m_Data[m_Index++] << 24) | ((long)m_Data[m_Index++] << 32) | ((long)m_Data[m_Index++] << 40) | ((long)m_Data[m_Index++] << 48) | ((long)m_Data[m_Index++] << 56);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public int ReadInt32()
        {
            if ( ( m_Index + 4 ) > m_Size )
                return 0;

            if ( m_Endian == Endian.LITTLE_ENDIAN )
                return ( m_Data[m_Index++] << 24 ) | ( m_Data[m_Index++] << 16 ) | ( m_Data[m_Index++] << 8 ) | m_Data[m_Index++];
            else
                return m_Data[m_Index++] | (m_Data[m_Index++] << 8) | (m_Data[m_Index++] << 16) | (m_Data[m_Index++] << 24);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public short ReadInt16()
        {
            if ( ( m_Index + 2 ) > m_Size )
                return 0;

            if ( m_Endian == Endian.LITTLE_ENDIAN ) 
                return (short)( m_Data[m_Index++] << 8 | ( m_Data[m_Index++] ) );
            else
                return (short)(m_Data[m_Index++] | (m_Data[m_Index++] << 8));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public byte ReadByte()
        {
            if ( ( m_Index + 1 ) > m_Size )
                return 0;

            return m_Data[m_Index++];
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ulong ReadULong64 ()
        {
            if ( ( m_Index + 8 ) > m_Size )
                return 0;

            if ( m_Endian == Endian.LITTLE_ENDIAN )
                return (ulong)( ( (long)m_Data[m_Index++] << 56 ) | ( (long)m_Data[m_Index++] << 48 ) | ( (long)m_Data[m_Index++] << 40 ) | ( (long)m_Data[m_Index++] << 32 ) | ( (uint)m_Data[m_Index++] << 24 ) | ( (uint)m_Data[m_Index++] << 16 ) | ( (uint)m_Data[m_Index++] << 8 ) | m_Data[m_Index++] );
            else
                return (ulong)(m_Data[m_Index++] | ((long)m_Data[m_Index++] << 8) | ((long)m_Data[m_Index++] << 16) | ((long)m_Data[m_Index++] << 24) | ((long)m_Data[m_Index++] << 32) | ((long)m_Data[m_Index++] << 40) | ((long)m_Data[m_Index++] << 48) | ((long)m_Data[m_Index++] << 56));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public uint ReadUInt32()
        {
            if ( ( m_Index + 4 ) > m_Size )
                return 0;

            if ( m_Endian == Endian.LITTLE_ENDIAN ) 
                return (uint)( ( m_Data[m_Index++] << 24 ) | ( m_Data[m_Index++] << 16 ) | ( m_Data[m_Index++] << 8 ) | m_Data[m_Index++] );
            else
                return (uint)(m_Data[m_Index++] | (m_Data[m_Index++] << 8) | (m_Data[m_Index++] << 16) | (m_Data[m_Index++] << 24));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ushort ReadUInt16()
        {
            if ( ( m_Index + 2 ) > m_Size )
                return 0;

            if ( m_Endian == Endian.LITTLE_ENDIAN ) 
                return (ushort)( ( m_Data[m_Index++] << 8 ) | m_Data[m_Index++] );
            else
                return (ushort)(m_Data[m_Index++] | (m_Data[m_Index++] << 8));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public sbyte ReadSByte()
        {
            if ( ( m_Index + 1 ) > m_Size )
                return 0;

            return (sbyte)m_Data[m_Index++];
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool ReadBoolean()
        {
            if ( ( m_Index + 1 ) > m_Size )
                return false;

            return ( m_Data[m_Index++] != 0 );
        }

        #region zh-CHS 私有成员变量 | en Private Member Variables
        /// <summary>
        /// 同上
        /// </summary>
        private CONVERT_FLOAT_INT_UINT m_ConvertFloat;
        #endregion
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public float ReadFloat()
        {
            m_ConvertFloat.uiUint = ReadUInt32();
            return m_ConvertFloat.fFloat;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool ReadBuffer(ref byte[] byteBuffer, long ibyteBufferOffset, long iReadSize )
        {
            if ( ( m_Index + iReadSize ) > m_Size )
                return false;

            if ( ( ibyteBufferOffset + iReadSize ) > byteBuffer.Length )
                return false;

            System.Buffer.BlockCopy( m_Data, (int)m_Index, byteBuffer, (int)ibyteBufferOffset, (int)iReadSize );

            m_Index += iReadSize;

            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string ReadUnicodeStringLE()
        {
            StringBuilder stringBuilder = new StringBuilder();

            long cChar;
            while ( ( m_Index + 1 ) < m_Size && ( cChar = ( m_Data[m_Index++] | ( m_Data[m_Index++] << 8 ) ) ) != 0 )
                stringBuilder.Append( (char)cChar );

            return stringBuilder.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string ReadUnicodeStringLESafe()
        {
            StringBuilder stringBuilder = new StringBuilder();

            long cChar;
            while ( ( m_Index + 1 ) < m_Size && ( cChar = ( m_Data[m_Index++] | ( m_Data[m_Index++] << 8 ) ) ) != 0 )
            {
                if ( IsSafeChar( cChar ) )
                    stringBuilder.Append( (char)cChar );
            }

            return stringBuilder.ToString();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string ReadUnicodeString()
        {
            StringBuilder stringBuilder = new StringBuilder();

            long cChar;
            while ( ( m_Index + 1 ) < m_Size && ( cChar = ( ( m_Data[m_Index++] << 8 ) | m_Data[m_Index++] ) ) != 0 )
                stringBuilder.Append( (char)cChar );

            return stringBuilder.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string ReadUnicodeStringSafe()
        {
            StringBuilder stringBuilder = new StringBuilder();

            long cChar;
            while ( ( m_Index + 1 ) < m_Size && ( cChar = ( ( m_Data[m_Index++] << 8 ) | m_Data[m_Index++] ) ) != 0 )
            {
                if ( IsSafeChar( cChar ) )
                    stringBuilder.Append( (char)cChar );
            }

            return stringBuilder.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string ReadUTF8String()
        {
            if ( m_Index >= m_Size )
                return String.Empty;

            long iCount = 0;
            long iIndex = m_Index;

            while ( m_Index < m_Size && m_Data[m_Index++] != 0 )
                ++iCount;

            if ( iCount == 0 )
                return string.Empty;
            else
                return ConvertString.UTF8.GetString( m_Data, (int)iIndex, (int)iCount );
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string ReadString()
        {
            var stringBuilder = new StringBuilder();

            int cChar;
            while ( m_Index < m_Size && ( cChar = m_Data[m_Index++] ) != 0 )
                stringBuilder.Append( (char)cChar );

            return stringBuilder.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cChar"></param>
        /// <returns></returns>
        public static bool IsSafeChar( long cChar )
        {
            return ( cChar >= 0x20 && cChar < 0xFFFE );
        }

        /// <summary>
        /// 获得消息包id
        /// </summary>
        /// <returns></returns>
        public ushort GetPacketID()
        {
            var index = ReceiveQueue.PacketLengthSize;

            if ((index + 2) > m_Size)
                return 0;

            if (m_Endian == Endian.LITTLE_ENDIAN)
                return (ushort)((m_Data[index++] << 8) | m_Data[index]);
            else
                return (ushort)(m_Data[index++] | (m_Data[index] << 8));
        }

        #endregion


        #region IDisposable 成员

        /// <summary>
        /// 释放资源
        /// </summary>
        void Dispose()
        {
            if (m_buffer != null)
            {
                var t = m_buffer;
                m_buffer = null;
                t.Release();
            }
        }

        #endregion
    }
}
