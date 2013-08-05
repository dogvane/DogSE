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
using System.IO;
using System.Text;
using Demo.Mmose.Core.Common;
using Demo.Mmose.Core.Server.Language;
using Demo.Mmose.Core.Util;
#endregion

namespace DogSE.Server.Net
{
    /// <summary>
    /// 数据包的数据读取
    /// </summary>
    [MultiThreadedNoSupport( "zh-CHS", "当前的类所有成员没有锁定(只在局部创建数据包的读取),不支持多线程操作" )]
    public class PacketReader
    {
        #region zh-CHS 构造和初始化和清理 | en Constructors and Initializers and Dispose
        /// <summary>
        /// 
        /// </summary>
        /// <param name="byteData"></param>
        /// <param name="iSize"></param>
        /// <param name="iOffset"></param>
        public PacketReader( byte[] byteData, long iSize, long iOffset )
        {
            m_Data  = byteData;
            m_Size  = iSize;
            m_Index = iOffset;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="byteData"></param>
        /// <param name="iSize"></param>
        /// <param name="iOffset"></param>
        public PacketReader( byte[] byteData, long iSize, long iOffset, EventHandler<PacketLengthInfoEventArgs> eventPacketLength )
        {
            m_Data  = byteData;
            m_Size  = iSize;
            m_Index = iOffset;

            EventPacketLength += eventPacketLength;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="byteData"></param>
        /// <param name="iSize"></param>
        /// <param name="iOffset"></param>
        public PacketReader( byte[] byteData, long iSize, long iOffset, EventHandler<PacketIdInfoEventArgs> eventPacketID )
        {
            m_Data  = byteData;
            m_Size  = iSize;
            m_Index = iOffset;

            EventPacketID += eventPacketID;
        }
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
        private Endian m_Endian = Endian.LITTLE_ENDIAN;
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
        /// <param name="netState"></param>
        public void Trace( NetState netState )
        {
            using ( StreamWriter streamWriter = new StreamWriter( "Unknown_Packets.log", true ) )
            {
                byte[] byteBuffer = m_Data;

                if ( byteBuffer.Length > 0 )
                {
                    streamWriter.WriteLine( LanguageString.SingletonInstance.PacketReaderString001, netState, GetPacketLength(), GetPacketID() );
                    streamWriter.WriteLine( LanguageString.SingletonInstance.PacketReaderString002 );
                }

                using ( MemoryStream memoryStream = new MemoryStream( byteBuffer ) )
                    Utility.FormatBuffer( streamWriter, memoryStream, GetPacketLength() );

                streamWriter.WriteLine();
                streamWriter.WriteLine();
            }
        }

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
                return (long)m_Data[m_Index++] | ( (long)m_Data[m_Index++] << 8 ) | ( (long)m_Data[m_Index++] << 16 ) | ( (long)m_Data[m_Index++] << 24 ) | ( (long)m_Data[m_Index++] << 32 ) | ( (long)m_Data[m_Index++] << 40 ) | ( (long)m_Data[m_Index++] << 48 ) | ( (long)m_Data[m_Index++] << 56 );
            else
                return ( (long)m_Data[m_Index++] << 56 ) | ( (long)m_Data[m_Index++] << 48 ) | ( (long)m_Data[m_Index++] << 40 ) | ( (long)m_Data[m_Index++] << 32 ) | ( (uint)( m_Data[m_Index++] << 24 ) ) | ( (uint)m_Data[m_Index++] << 16 ) | ( (uint)m_Data[m_Index++] << 8 ) | (uint)m_Data[m_Index++];
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
                return m_Data[m_Index++] | ( m_Data[m_Index++] << 8 ) | ( m_Data[m_Index++] << 16 ) | ( m_Data[m_Index++] << 24 );
            else
                return ( m_Data[m_Index++] << 24 ) | ( m_Data[m_Index++] << 16 ) | ( m_Data[m_Index++] << 8 ) | m_Data[m_Index++];
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
                return (short)( m_Data[m_Index++] | ( m_Data[m_Index++] << 8 ) );
            else
                return (short)( m_Data[m_Index++] << 8 | ( m_Data[m_Index++] ) );
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
                return (ulong)( (long)m_Data[m_Index++] | ( (long)m_Data[m_Index++] << 8 ) | ( (long)m_Data[m_Index++] << 16 ) | ( (long)m_Data[m_Index++] << 24 ) | ( (long)m_Data[m_Index++] << 32 ) | ( (long)m_Data[m_Index++] << 40 ) | ( (long)m_Data[m_Index++] << 48 ) | ( (long)m_Data[m_Index++] << 56 ) );
            else
                return (ulong)( ( (long)m_Data[m_Index++] << 56 ) | ( (long)m_Data[m_Index++] << 48 ) | ( (long)m_Data[m_Index++] << 40 ) | ( (long)m_Data[m_Index++] << 32 ) | ( (uint)m_Data[m_Index++] << 24 ) | ( (uint)m_Data[m_Index++] << 16 ) | ( (uint)m_Data[m_Index++] << 8 ) | (uint)m_Data[m_Index++] );
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
                return (uint)( m_Data[m_Index++] | ( m_Data[m_Index++] << 8 ) | ( m_Data[m_Index++] << 16 ) | ( m_Data[m_Index++] << 24 ) );
            else
                return (uint)( ( m_Data[m_Index++] << 24 ) | ( m_Data[m_Index++] << 16 ) | ( m_Data[m_Index++] << 8 ) | m_Data[m_Index++] );
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
                return (ushort)( m_Data[m_Index++] | ( m_Data[m_Index++] << 8 ) );
            else
                return (ushort)( ( m_Data[m_Index++] << 8 ) | m_Data[m_Index++] );
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
        private CONVERT_FLOAT_INT_UINT m_ConvertFloat = new CONVERT_FLOAT_INT_UINT();
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
        /// <param name="iFixedLength">字符串的长度</param>
        /// <returns></returns>
        public string ReadUnicodeStringLE( long iFixedLength )
        {
            long iBound = m_Index + ( iFixedLength << 1 );
            long iEnd = iBound;

            if ( iBound > m_Size )
                iBound = m_Size;

            StringBuilder stringBuilder = new StringBuilder( (int)iFixedLength );

            long cChar;
            while ( ( m_Index + 1 ) < iBound && ( cChar = ( m_Data[m_Index++] | ( m_Data[m_Index++] << 8 ) ) ) != 0 )
                stringBuilder.Append( (char)cChar );

            m_Index = iEnd;

            return stringBuilder.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="iFixedLength">字符串的长度</param>
        /// <returns></returns>
        public string ReadUnicodeStringLESafe( long iFixedLength )
        {
            long iBound = m_Index + ( iFixedLength << 1 );
            long iEnd = iBound;

            if ( iBound > m_Size )
                iBound = m_Size;

            StringBuilder stringBuilder = new StringBuilder( (int)iFixedLength );

            long cChar;
            while ( ( m_Index + 1 ) < iBound && ( cChar = ( m_Data[m_Index++] | ( m_Data[m_Index++] << 8 ) ) ) != 0 )
            {
                if ( IsSafeChar( cChar ) )
                    stringBuilder.Append( (char)cChar );
            }

            m_Index = iEnd;

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
        /// <param name="fixedLength"></param>
        /// <returns></returns>
        public string ReadUnicodeString( long iFixedLength )
        {
            long iBound = m_Index + ( iFixedLength << 1 );
            long iEnd = iBound;

            if ( iBound > m_Size )
                iBound = m_Size;

            StringBuilder stringBuilder = new StringBuilder( (int)iFixedLength );

            long cChar;
            while ( ( m_Index + 1 ) < iBound && ( cChar = ( ( m_Data[m_Index++] << 8 ) | m_Data[m_Index++] ) ) != 0 )
                stringBuilder.Append( (char)cChar );

            m_Index = iEnd;

            return stringBuilder.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fixedLength"></param>
        /// <returns></returns>
        public string ReadUnicodeStringSafe( long iFixedLength )
        {
            long iBound = m_Index + ( iFixedLength << 1 );
            long iEnd = iBound;

            if ( iBound > m_Size )
                iBound = m_Size;

            StringBuilder stringBuilder = new StringBuilder( (int)iFixedLength );

            long cChar;
            while ( ( m_Index + 1 ) < iBound && ( cChar = ( ( m_Data[m_Index++] << 8 ) | m_Data[m_Index++] ) ) != 0 )
            {
                if ( IsSafeChar( cChar ) )
                    stringBuilder.Append( (char)cChar );
            }

            m_Index = iEnd;

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
        public string ReadUTF8StringSafe()
        {
            if ( m_Index >= m_Size )
                return String.Empty;

            long iCount = 0;
            long iIndex = m_Index;

            while ( m_Index < m_Size && m_Data[m_Index++] != 0 )
                ++iCount;

            string strString = ConvertString.UTF8.GetString( m_Data, (int)iIndex, (int)iCount );

            bool isSafe = true;
            foreach ( char safeChar in strString )
            {
                if ( ( isSafe = IsSafeChar( safeChar ) ) == false )
                    break;
            }

            if ( isSafe == true )
                return strString;

            StringBuilder stringBuilder = new StringBuilder( strString.Length );

            foreach ( char safeChar in strString )
            {
                if ( IsSafeChar( (long)safeChar ) )
                    stringBuilder.Append( safeChar );
            }

            return stringBuilder.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="iFixedLength">字符串的字节长度</param>
        /// <returns></returns>
        public string ReadUTF8String( long iFixedLength )
        {
            if ( m_Index >= m_Size )
            {
                m_Index += iFixedLength;
                return String.Empty;
            }

            long iBound = m_Index + iFixedLength;
            long iEnd = iBound;
            if ( iBound > m_Size )
                iBound = m_Size;

            long iCount = 0;
            long iIndex = m_Index;
            while ( m_Index < iBound && m_Data[m_Index++] != 0 )
                ++iCount;

            m_Index = iEnd;

            return ConvertString.UTF8.GetString( m_Data, (int)iIndex, (int)iCount );
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="iFixedLength">字符串的字节长度</param>
        /// <returns></returns>
        public string ReadUTF8StringSafe( long iFixedLength )
        {
            if ( m_Index >= m_Size )
            {
                m_Index += iFixedLength;
                return String.Empty;
            }

            long iBound = m_Index + iFixedLength;
            long iEnd = iBound;
            if ( iBound > m_Size )
                iBound = m_Size;

            long iCount = 0;
            long iIndex = m_Index;
            while ( m_Index < iBound && m_Data[m_Index++] != 0 )
                ++iCount;

            m_Index = iEnd;

            string strString = ConvertString.UTF8.GetString( m_Data, (int)iIndex, (int)iCount );

            bool isSafe = true;
            foreach ( char safeChar in strString )
            {
                if ( ( isSafe = IsSafeChar( safeChar ) ) == false )
                    break;
            }

            if ( isSafe == true )
                return strString;

            StringBuilder stringBuilder = new StringBuilder( strString.Length );

            foreach ( char safeChar in strString )
            {
                if ( IsSafeChar( (long)safeChar ) )
                    stringBuilder.Append( safeChar );
            }

            return stringBuilder.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string ReadString()
        {
            StringBuilder stringBuilder = new StringBuilder();

            int cChar;
            while ( m_Index < m_Size && ( cChar = m_Data[m_Index++] ) != 0 )
                stringBuilder.Append( (char)cChar );

            return stringBuilder.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string ReadStringSafe()
        {
            StringBuilder stringBuilder = new StringBuilder();

            int cChar;
            while ( m_Index < m_Size && ( cChar = m_Data[m_Index++] ) != 0 )
            {
                if ( IsSafeChar( cChar ) )
                    stringBuilder.Append( (char)cChar );
            }

            return stringBuilder.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fixedLength"></param>
        /// <returns></returns>
        public string ReadString( long iFixedLength )
        {
            long iBound = m_Index + iFixedLength;
            long iEnd = iBound;

            if ( iBound > m_Size )
                iBound = m_Size;

            StringBuilder stringBuilder = new StringBuilder( (int)iFixedLength );

            long cChar;
            while ( m_Index < iBound && ( cChar = m_Data[m_Index++] ) != 0 )
                stringBuilder.Append( (char)cChar );

            m_Index = iEnd;

            return stringBuilder.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fixedLength"></param>
        /// <returns></returns>
        public string ReadStringSafe( long iFixedLength )
        {
            long iBound = m_Index + iFixedLength;
            long iEnd = iBound;

            if ( iBound > m_Size )
                iBound = m_Size;

            StringBuilder stringBuilder = new StringBuilder( (int)iFixedLength );

            int cChar;
            while ( m_Index < iBound && ( cChar = m_Data[m_Index++] ) != 0 )
            {
                if ( IsSafeChar( cChar ) )
                    stringBuilder.Append( (char)cChar );
            }

            m_Index = iEnd;

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
        /// 
        /// </summary>
        /// <returns></returns>
        public long GetPacketLength()
        {
            long iReturn = 0;

            if ( EventPacketLength != null )
            {
                PacketLengthInfoEventArgs packetLengthInfoEventArgs = new PacketLengthInfoEventArgs( m_Data, m_Size, 0 );
                EventPacketLength( this, packetLengthInfoEventArgs );
                iReturn = packetLengthInfoEventArgs.PacketLength;
            }

            return iReturn;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ushort GetPacketID()
        {
            ushort iReturn = 0;

            if ( EventPacketID != null )
            {
                PacketIdInfoEventArgs packetIdInfoEventArgs = new PacketIdInfoEventArgs( m_Data, m_Size, 0 );
                EventPacketID( this, packetIdInfoEventArgs );
                iReturn = packetIdInfoEventArgs.PacketId;
            }

            return iReturn;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public PacketHeadT GetPacketHead<PacketHeadT>( EventHandler<PacketHeadInfoEventArgs<PacketHeadT>> eventHandler )
        {
            if ( eventHandler == null )
                throw new ArgumentNullException( "eventHandler", "PacketReader.GetPacketHead<.>(...) - eventHandler == null error!" );
            
            PacketHeadT returnT = default( PacketHeadT );

            PacketHeadInfoEventArgs<PacketHeadT> eventArgs = new PacketHeadInfoEventArgs<PacketHeadT>( m_Data, m_Size, 0 );
            eventHandler( this, eventArgs );
            returnT = eventArgs.PacketHead;

            return returnT;
        }
        #endregion

        #region zh-CHS 共有事件 | en Public Event
        /// <summary>
        /// 获取接收到的数据包长度
        /// </summary>
        public event EventHandler<PacketLengthInfoEventArgs> EventPacketLength;
        /// <summary>
        /// 获取接收到的数据包ID
        /// </summary>
        public event EventHandler<PacketIdInfoEventArgs> EventPacketID;
         #endregion
    }
}
#endregion

