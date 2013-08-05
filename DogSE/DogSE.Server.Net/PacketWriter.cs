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
using System.Text;
using Demo.Mmose.Core.Common;
using Demo.Mmose.Core.Util;
using DogSE.Library.Util;

#endregion

namespace DogSE.Server.Net
{
    /// <summary>
    /// Provides functionality for writing primitive binary data.
    /// </summary>
    [MultiThreadedNoSupport( "zh-CHS", "当前的类所有成员没有锁定(只在局部创建在需发送的数据包(Packet)内),不支持多线程操作" )]
    public class PacketWriter
    {
        #region zh-CHS 类常量 | en Class Constants
        /// <summary>
        /// 
        /// </summary>
        private const int BUFFER_SIZE = 4096;
        /// <summary>
        /// 缓冲区容器的大小
        /// </summary>
        private const long CAPACITY_SIZE = 1024;
        #endregion

        #region zh-CHS 私有成员变量 | en Private Member Variables
        /// <summary>
        /// 输出流的数据容量
        /// </summary>
        private long m_Capacity = 0;
        /// <summary>
        /// Internal format buffer.
        /// 本来认为在多线程里面会出错,其实不需要,因调用总是在一个线程里面处理的
        /// 不能为静态
        /// </summary>
        private byte[] m_Buffer = new byte[8];
        /// <summary>
        /// 同上
        /// </summary>
        private CONVERT_FLOAT_INT_UINT m_ConvertFloat = new CONVERT_FLOAT_INT_UINT();
        /// <summary>
        /// 同上
        /// </summary>
        private CONVERT_DOUBLE_LONG_ULONG m_ConvertDouble = new CONVERT_DOUBLE_LONG_ULONG();
        #endregion

        #region zh-CHS 构造和初始化和清理 | en Constructors and Initializers and Dispose
        /// <summary>
        /// Instantiates a new PacketWriter instance with the default capacity of 4 bytes.
        /// </summary>
        public PacketWriter() : this( BUFFER_SIZE )
        {
        }

        /// <summary>
        /// Instantiates a new PacketWriter instance with a given capacity.
        /// </summary>
        /// <param name="iCapacity">Initial capacity for the internal stream.</param>
        public PacketWriter( long iCapacity )
        {
            m_Capacity = iCapacity;
            m_Stream = new System.IO.MemoryStream( m_Capacity > BUFFER_SIZE ? (int)m_Capacity : BUFFER_SIZE );
        }
        #endregion

        #region zh-CHS 属性 | en Properties
        /// <summary>
        /// Gets the total stream length.
        /// </summary>
        public long Length
        {
            get { return m_Stream.Length; }
        }

        #region zh-CHS Length方法 | en Length Methods
        /// <summary>
        /// 
        /// </summary>
        /// <param name="iLength"></param>
        public void SetLength(long iLength)
        {
            m_Stream.SetLength( iLength );
        }
        #endregion

        /// <summary>
        /// Gets or sets the current stream position.
        /// </summary>
        public long Position
        {
            get { return m_Stream.Position; }
            set { m_Stream.Position = value; }
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

        #region zh-CHS 内部属性 | en Internal Properties
        #region zh-CHS 私有成员变量 | en Private Member Variables
        /// <summary>
        /// Internal stream which holds the entire packet.
        /// </summary>
        private System.IO.MemoryStream m_Stream;
        #endregion
        /// <summary>
        /// The internal stream used by this PacketWriter instance.
        /// </summary>
        internal System.IO.MemoryStream Stream
        {
            get { return m_Stream; }
        }
        #endregion

        #region zh-CHS 方法 | en Method
        /// <summary>
        /// Writes a 1-byte boolean value to the underlying stream. False is represented by 0, true by 1.
        /// </summary>
        public void Write( bool bValue )
        {
            m_Stream.WriteByte( (byte)( bValue == true ? 1 : 0 ) );
        }

        /// <summary>
        /// Writes a 1-byte unsigned integer value to the underlying stream.
        /// </summary>
        public void Write( byte byteValue )
        {
            m_Stream.WriteByte( byteValue );
        }

        /// <summary>
        /// Writes a 1-byte signed integer value to the underlying stream.
        /// </summary>
        public void Write( sbyte sbyteValue )
        {
            m_Stream.WriteByte( (byte)sbyteValue );
        }

        /// <summary>
        /// Writes a 2-byte signed integer value to the underlying stream.
        /// </summary>
        public void Write( short shortValue )
        {
            if ( m_Endian == Endian.LITTLE_ENDIAN )
            {
                m_Buffer[0] = (byte)shortValue;
                m_Buffer[1] = (byte)( shortValue >> 8 );
            }
            else
            {
                m_Buffer[0] = (byte)( shortValue >> 8 );
                m_Buffer[1] = (byte)shortValue;
            }

            m_Stream.Write( m_Buffer, 0, 2 );
        }

        /// <summary>
        /// Writes a 2-byte unsigned integer value to the underlying stream.
        /// </summary>
        public void Write( ushort ushortValue )
        {
            if ( m_Endian == Endian.LITTLE_ENDIAN )
            {
                m_Buffer[0] = (byte)ushortValue;
                m_Buffer[1] = (byte)( ushortValue >> 8 );
            }
            else
            {
                m_Buffer[0] = (byte)( ushortValue >> 8 );
                m_Buffer[1] = (byte)ushortValue;
            }

            m_Stream.Write( m_Buffer, 0, 2 );
        }

        /// <summary>
        /// Writes a 4-byte signed integer value to the underlying stream.
        /// </summary>
        public void Write( int intValue )
        {
            if ( m_Endian == Endian.LITTLE_ENDIAN )
            {
                m_Buffer[0] = (byte)intValue;
                m_Buffer[1] = (byte)( intValue >> 8 );
                m_Buffer[2] = (byte)( intValue >> 16 );
                m_Buffer[3] = (byte)( intValue >> 24 );
            }
            else
            {
                m_Buffer[0] = (byte)( intValue >> 24 );
                m_Buffer[1] = (byte)( intValue >> 16 );
                m_Buffer[2] = (byte)( intValue >> 8 );
                m_Buffer[3] = (byte)intValue;
            }

            m_Stream.Write( m_Buffer, 0, 4 );
        }

        /// <summary>
        /// Writes a 4-byte unsigned integer value to the underlying stream.
        /// </summary>
        public void Write( uint uintValue )
        {
            if ( m_Endian == Endian.LITTLE_ENDIAN )
            {
                m_Buffer[0] = (byte)uintValue;
                m_Buffer[1] = (byte)( uintValue >> 8 );
                m_Buffer[2] = (byte)( uintValue >> 16 );
                m_Buffer[3] = (byte)( uintValue >> 24 );
            }
            else
            {
                m_Buffer[0] = (byte)( uintValue >> 24 );
                m_Buffer[1] = (byte)( uintValue >> 16 );
                m_Buffer[2] = (byte)( uintValue >> 8 );
                m_Buffer[3] = (byte)uintValue;
            }

            m_Stream.Write( m_Buffer, 0, 4 );
        }

        /// <summary>
        /// Writes a 4-byte float value to the underlying stream.
        /// </summary>
        public void Write( float floatValue )
        {
            m_ConvertFloat.fFloat = floatValue;
            Write( m_ConvertFloat.uiUint );
        }

        /// <summary>
        /// Writes a 8-byte long value to the underlying stream.
        /// </summary>
        public void Write( long longValue )
        {
            if ( m_Endian == Endian.LITTLE_ENDIAN )
            {
                m_Buffer[0] = (byte)longValue;
                m_Buffer[1] = (byte)( longValue >> 8 );
                m_Buffer[2] = (byte)( longValue >> 16 );
                m_Buffer[3] = (byte)( longValue >> 24 );
                m_Buffer[4] = (byte)( longValue >> 32 );
                m_Buffer[5] = (byte)( longValue >> 40 );
                m_Buffer[6] = (byte)( longValue >> 48 );
                m_Buffer[7] = (byte)( longValue >> 56 );
            }
            else
            {
                m_Buffer[0] = (byte)( longValue >> 56 );
                m_Buffer[1] = (byte)( longValue >> 48 );
                m_Buffer[2] = (byte)( longValue >> 40 );
                m_Buffer[3] = (byte)( longValue >> 32 );
                m_Buffer[4] = (byte)( longValue >> 24 );
                m_Buffer[5] = (byte)( longValue >> 16 );
                m_Buffer[6] = (byte)( longValue >> 8 );
                m_Buffer[7] = (byte)longValue;
            }

            m_Stream.Write( m_Buffer, 0, 8 );
        }

        /// <summary>
        /// Writes a 8-byte unsigned long value to the underlying stream.
        /// </summary>
        public void Write( ulong ulongValue )
        {
            if ( m_Endian == Endian.LITTLE_ENDIAN )
            {
                m_Buffer[0] = (byte)ulongValue;
                m_Buffer[1] = (byte)( ulongValue >> 8 );
                m_Buffer[2] = (byte)( ulongValue >> 16 );
                m_Buffer[3] = (byte)( ulongValue >> 24 );
                m_Buffer[4] = (byte)( ulongValue >> 32 );
                m_Buffer[5] = (byte)( ulongValue >> 40 );
                m_Buffer[6] = (byte)( ulongValue >> 48 );
                m_Buffer[7] = (byte)( ulongValue >> 56 );
            }
            else
            {
                m_Buffer[0] = (byte)( ulongValue >> 56 );
                m_Buffer[1] = (byte)( ulongValue >> 48 );
                m_Buffer[2] = (byte)( ulongValue >> 40 );
                m_Buffer[3] = (byte)( ulongValue >> 32 );
                m_Buffer[4] = (byte)( ulongValue >> 24 );
                m_Buffer[5] = (byte)( ulongValue >> 16 );
                m_Buffer[6] = (byte)( ulongValue >> 8 );
                m_Buffer[7] = (byte)ulongValue;
            }

            m_Stream.Write( m_Buffer, 0, 8 );
        }

        /// <summary>
        /// Writes a 8-byte double value to the underlying stream.
        /// </summary>
        public void Write( double doubleValue )
        {
            m_ConvertDouble.dDouble = doubleValue;
            Write( m_ConvertDouble.ulUlong );
        }

        /// <summary>
        /// Writes a sequence of bytes to the underlying stream
        /// </summary>
        public void Write( byte[] byteBuffer, int iOffset, int iSize )
        {
            m_Stream.Write( byteBuffer, iOffset, iSize );
        }

        /// <summary>
        /// Writes a fixed-length ASCII-encoded string value to the underlying stream. To fit (size), the string content is either truncated or padded with null characters.
        /// </summary>
        /// <param name="iValue"></param>
        /// <param name="iSize"></param>
        public void WriteAsciiFixed( string strValue, int iSize )
        {
            if ( strValue == null )
                strValue = string.Empty;

            byte[] buffer = Encoding.ASCII.GetBytes( strValue );

            if ( buffer.Length >= iSize )
            {
                m_Stream.Write( buffer, 0, iSize );
            }
            else
            {
                m_Stream.Write( buffer, 0, buffer.Length );
                Fill( 0x0, iSize - buffer.Length );
            }
        }

        /// <summary>
        /// Writes a dynamic-length ASCII-encoded string value to the underlying stream, followed by a 1-byte null character.
        /// </summary>
        /// <param name="strValue"></param>
        public void WriteAsciiNull( string strValue )
        {
            if ( strValue == null )
                strValue = string.Empty;

            byte[] buffer = Encoding.ASCII.GetBytes( strValue );

            m_Stream.Write( buffer, 0, buffer.Length );
            m_Stream.WriteByte( 0 );
        }

        /// <summary>
        /// Writes a dynamic-length little-endian unicode string value to the underlying stream, followed by a 2-byte null character.
        /// </summary>
        /// <param name="strValue"></param>
        public void WriteLittleUniNull( string strValue )
        {
            if ( strValue == null )
                strValue = string.Empty;

            byte[] buffer = Encoding.Unicode.GetBytes( strValue );

            m_Stream.Write( buffer, 0, buffer.Length );

            m_Buffer[0] = 0;
            m_Buffer[1] = 0;
            m_Stream.Write( m_Buffer, 0, 2 );
        }

        /// <summary>
        /// Writes a fixed-length little-endian unicode string value to the underlying stream. To fit (size), the string content is either truncated or padded with null characters.
        /// </summary>
        /// <param name="strValue"></param>
        /// <param name="iSize"></param>
        public void WriteLittleUniFixed( string strValue, int iSize )
        {
            if ( strValue == null )
                strValue = string.Empty;

            byte[] buffer = Encoding.Unicode.GetBytes( strValue );

            iSize *= 2;
            if ( buffer.Length >= iSize )
            {
                m_Stream.Write( buffer, 0, iSize );
            }
            else
            {
                m_Stream.Write( buffer, 0, buffer.Length );
                Fill( 0x0, iSize - buffer.Length );
            }
        }

        /// <summary>
        /// Writes a dynamic-length big-endian unicode string value to the underlying stream, followed by a 2-byte null character.
        /// </summary>
        /// <param name="strValue"></param>
        public void WriteBigUniNull( string strValue )
        {
            if ( strValue == null )
                strValue = string.Empty;

            byte[] buffer = Encoding.BigEndianUnicode.GetBytes( strValue );

            m_Stream.Write( buffer, 0, buffer.Length );

            m_Buffer[0] = 0;
            m_Buffer[1] = 0;
            m_Stream.Write( m_Buffer, 0, 2 );
        }

        /// <summary>
        /// Writes a fixed-length big-endian unicode string value to the underlying stream. To fit (size), the string content is either truncated or padded with null characters.
        /// </summary>
        /// <param name="strValue"></param>
        /// <param name="iSize"></param>
        public void WriteBigUniFixed( string strValue, int iSize )
        {
            if ( strValue == null )
                strValue = string.Empty;

            byte[] buffer = Encoding.BigEndianUnicode.GetBytes( strValue );

            iSize *= 2;
            if ( buffer.Length >= iSize )
            {
                m_Stream.Write( buffer, 0, iSize );
            }
            else
            {
                m_Stream.Write( buffer, 0, buffer.Length );
                Fill( 0x0, iSize - buffer.Length );
            }
        }

        /// <summary>
        /// Writes a dynamic-length UTF8 string value to the underlying stream, followed by a 2-byte null character.
        /// </summary>
        /// <param name="strValue"></param>
        public void WriteUTF8Null( string strValue )
        {
            if (strValue == null)
                strValue = string.Empty;

            byte[] buffer = Encoding.UTF8.GetBytes( strValue );

            m_Stream.Write( buffer, 0, buffer.Length );
            m_Stream.WriteByte( 0 );
        }

        /// <summary>
        /// Writes a fixed-length UTF8 string value to the underlying stream. To fit (iLength), the string content is either truncated or padded with null characters.
        /// </summary>
        /// <param name="strValue"></param>
        /// <param name="iLength"></param>
        public void WriteUTF8Fixed( string strValue, int iLength )
        {
            if ( strValue == null )
                strValue = string.Empty;

            byte[] buffer = Encoding.UTF8.GetBytes( strValue );

            if ( buffer.Length >= iLength )
                m_Stream.Write( buffer, 0, iLength );
            else
            {
                m_Stream.Write( buffer, 0, buffer.Length );
                Fill( 0x0, iLength - buffer.Length );
            }
        }

        /// <summary>
        /// Writes a number of 0x00 byte values to the underlying stream.
        /// </summary>
        /// <param name="iLength"></param>
        public void Fill( byte fillValue, long iLength )
        {
            for ( int iIndex = 0; iIndex < iLength; iIndex++ )
                m_Stream.WriteByte( fillValue );
        }

        /// <summary>
        /// Offsets the current position from an origin.
        /// </summary>
        /// <param name="iOffset"></param>
        /// <param name="seekOrigin"></param>
        /// <returns></returns>
        public long Seek( long iOffset, SeekOrigin seekOrigin )
        {
            return m_Stream.Seek( iOffset, (System.IO.SeekOrigin)seekOrigin );
        }

        /// <summary>
        /// Gets the entire stream content as a byte array.
        /// </summary>
        /// <returns></returns>
        public byte[] ToArray()
        {
            return m_Stream.ToArray();
        }
        #endregion

        #region zh-CHS 内部静态方法 | en Internal Static Method
        /// <summary>
        /// 
        /// </summary>
        internal void Release()
        {
            // 内部调用, 在Packet.Release() 已经检测过是否已经释放
            s_PacketWriterPool.ReleaseContent( this );
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        internal static PacketWriter Instance()
        {
            return Instance( BUFFER_SIZE );
        }

        #region zh-CHS 私有静态成员变量 | en Private Static Member Variables
        /// <summary>
        /// 
        /// </summary>
        private static MemoryPool<PacketWriter> s_PacketWriterPool = new MemoryPool<PacketWriter>( CAPACITY_SIZE );
        #endregion
        /// <summary>
        /// 
        /// </summary>
        /// <param name="iCapacity"></param>
        /// <returns></returns>
        internal static PacketWriter Instance( long iCapacity )
        {
            PacketWriter packetWriter = s_PacketWriterPool.AcquireContent();
            if ( packetWriter == null )
                throw new ArgumentNullException( "packetWriter", "PacketWriter.Instance(...) - packetWriter == null error!" );

            packetWriter.m_Capacity = iCapacity;
            packetWriter.m_Stream.SetLength( 0 );

            if ( packetWriter.m_Stream.Capacity < packetWriter.m_Capacity )
                packetWriter.m_Stream.Capacity = (int)packetWriter.m_Capacity;

            return packetWriter;
        }
        #endregion
    }
}
#endregion

