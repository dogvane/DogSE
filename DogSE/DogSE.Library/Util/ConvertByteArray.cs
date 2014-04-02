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
#endregion

namespace DogSE.Library.Util
{
    /// <summary>
    /// 
    /// </summary>
    public static class ConvertByteArray
    {
        #region zh-CHS ToByteArray | en ToByteArray
        /// <summary>
        /// 
        /// </summary>
        /// <param name="iInt"></param>
        /// <returns></returns>
        public static byte[] ToByteArray( int iInt )
        {
            return BitConverter.GetBytes( iInt );
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="iInt"></param>
        /// <param name="byteBuffer"></param>
        /// <param name="iBufferIndex"></param>
        public static void ToByteArray( int iInt, ref byte[] byteBuffer, int iBufferIndex )
        {
            if ( byteBuffer == null )
                throw new ArgumentNullException( "byteBuffer" );

            if ( ( iBufferIndex + 4 ) > byteBuffer.Length )
                throw new ArgumentException( "ByteArray.ToByteArray(...) - ( iBufferIndex + 4 ) > byteBuffer.Length error!" );

            unsafe
            {
                fixed ( byte* numRef = byteBuffer )
                    *( (int*)numRef + iBufferIndex ) = iInt;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="iInt"></param>
        /// <returns></returns>
        public static byte[] ToArrayInByte( this int iInt )
        {
            return BitConverter.GetBytes( iInt );
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="iInt"></param>
        /// <param name="byteBuffer"></param>
        /// <param name="iBufferIndex"></param>
        public static void ToArrayInByte( this int iInt, ref byte[] byteBuffer, int iBufferIndex )
        {
            ToByteArray( iInt, ref byteBuffer, iBufferIndex );
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="iUInt"></param>
        /// <returns></returns>
        public static byte[] ToByteArray( uint iUInt )
        {
            return BitConverter.GetBytes( iUInt );
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="iUInt"></param>
        /// <param name="byteBuffer"></param>
        /// <param name="iBufferIndex"></param>
        public static void ToByteArray( uint iUInt, ref byte[] byteBuffer, int iBufferIndex )
        {
            if ( byteBuffer == null )
                throw new ArgumentNullException( "byteBuffer" );

            if ( ( iBufferIndex + 4 ) > byteBuffer.Length )
                throw new ArgumentException( "ByteArray.ToByteArray(...) - ( iBufferIndex + 4 ) > byteBuffer.Length error!" );

            unsafe
            {
                fixed ( byte* numRef = byteBuffer )
                    *( (int*)numRef + iBufferIndex ) = (int)iUInt;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="iUInt"></param>
        /// <returns></returns>
        public static byte[] ToArrayInByte( this uint iUInt )
        {
            return BitConverter.GetBytes( iUInt );
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="iUInt"></param>
        /// <param name="byteBuffer"></param>
        /// <param name="iBufferIndex"></param>
        public static void ToArrayInByte( this uint iUInt, ref byte[] byteBuffer, int iBufferIndex )
        {
            ToByteArray( iUInt, ref byteBuffer, iBufferIndex );
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="fFloat"></param>
        /// <returns></returns>
        public static byte[] ToByteArray( float fFloat )
        {
            return BitConverter.GetBytes( fFloat );
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="fFloat"></param>
        /// <param name="byteBuffer"></param>
        /// <param name="iBufferIndex"></param>
        public static void ToByteArray( float fFloat, ref byte[] byteBuffer, int iBufferIndex )
        {
            if ( byteBuffer == null )
                throw new ArgumentNullException( "byteBuffer" );

            if ( ( iBufferIndex + 4 ) > byteBuffer.Length )
                throw new ArgumentException( "ByteArray.ToByteArray(...) - ( iBufferIndex + 4 ) > byteBuffer.Length error!" );

            unsafe
            {
                fixed ( byte* numRef = byteBuffer )
                    *( (int*)numRef + iBufferIndex ) = ( *( (int*)&fFloat ) );
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="fFloat"></param>
        /// <returns></returns>
        public static byte[] ToArrayInByte( this float fFloat )
        {
            return BitConverter.GetBytes( fFloat );
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="fFloat"></param>
        /// <param name="byteBuffer"></param>
        /// <param name="iBufferIndex"></param>
        public static void ToArrayInByte( this float fFloat, ref byte[] byteBuffer, int iBufferIndex )
        {
            ToByteArray( fFloat, ref byteBuffer, iBufferIndex );
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="iLong"></param>
        /// <returns></returns>
        public static byte[] ToByteArray( long iLong )
        {
            return BitConverter.GetBytes( iLong );
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="iLong"></param>
        /// <param name="byteBuffer"></param>
        /// <param name="iBufferIndex"></param>
        public static void ToByteArray( long iLong, ref byte[] byteBuffer, int iBufferIndex )
        {
            if ( byteBuffer == null )
                throw new ArgumentNullException( "byteBuffer" );

            if ( ( iBufferIndex + 8 ) > byteBuffer.Length )
                throw new ArgumentException( "ByteArray.ToByteArray(...) - ( iBufferIndex + 8 ) > byteBuffer.Length error!" );

            unsafe
            {
                fixed ( byte* numRef = byteBuffer )
                    *( (long*)numRef + iBufferIndex ) = iLong;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="iLong"></param>
        /// <returns></returns>
        public static byte[] ToArrayInByte( this long iLong )
        {
            return BitConverter.GetBytes( iLong );
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="iLong"></param>
        /// <param name="byteBuffer"></param>
        /// <param name="iBufferIndex"></param>
        public static void ToArrayInByte( this long iLong, ref byte[] byteBuffer, int iBufferIndex )
        {
            ToByteArray( iLong, ref byteBuffer, iBufferIndex );
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="iULong"></param>
        /// <returns></returns>
        public static byte[] ToByteArray( ulong iULong )
        {
            return BitConverter.GetBytes( iULong );
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="iULong"></param>
        /// <param name="byteBuffer"></param>
        /// <param name="iBufferIndex"></param>
        public static void ToByteArray( ulong iULong, ref byte[] byteBuffer, int iBufferIndex )
        {
            if ( byteBuffer == null )
                throw new ArgumentNullException( "byteBuffer" );

            if ( ( iBufferIndex + 8 ) > byteBuffer.Length )
                throw new ArgumentException( "ByteArray.ToByteArray(...) - ( iBufferIndex + 8 ) > byteBuffer.Length error!" );

            unsafe
            {
                fixed ( byte* numRef = byteBuffer )
                    *( (long*)numRef + iBufferIndex ) = (long)iULong;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="iULong"></param>
        /// <returns></returns>
        public static byte[] ToArrayInByte( this ulong iULong )
        {
            return BitConverter.GetBytes( iULong );
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="iULong"></param>
        /// <param name="byteBuffer"></param>
        /// <param name="iBufferIndex"></param>
        public static void ToArrayInByte( this ulong iULong, ref byte[] byteBuffer, int iBufferIndex )
        {
            ToByteArray( iULong, ref byteBuffer, iBufferIndex );
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="dDouble"></param>
        /// <returns></returns>
        public static byte[] ToByteArray( double dDouble )
        {
            return BitConverter.GetBytes( dDouble );
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="dDouble"></param>
        /// <param name="byteBuffer"></param>
        /// <param name="iBufferIndex"></param>
        public static void ToByteArray( double dDouble, ref byte[] byteBuffer, int iBufferIndex )
        {
            if ( byteBuffer == null )
                throw new ArgumentNullException( "byteBuffer" );

            if ( ( iBufferIndex + 8 ) > byteBuffer.Length )
                throw new ArgumentException( "ByteArray.ToByteArray(...) - ( iBufferIndex + 8 ) > byteBuffer.Length error!" );

            unsafe
            {
                fixed ( byte* numRef = byteBuffer )
                    *( (long*)numRef + iBufferIndex ) = ( *( (long*)&dDouble ) );
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dDouble"></param>
        /// <returns></returns>
        public static byte[] ToArrayInByte( this double dDouble )
        {
            return BitConverter.GetBytes( dDouble );
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="dDouble"></param>
        /// <param name="byteBuffer"></param>
        /// <param name="iBufferIndex"></param>
        public static void ToArrayInByte( this double dDouble, ref byte[] byteBuffer, int iBufferIndex )
        {
            ToByteArray( dDouble, ref byteBuffer, iBufferIndex );
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="iShort"></param>
        /// <returns></returns>
        public static byte[] ToByteArray( short iShort )
        {
            return BitConverter.GetBytes( iShort );
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="iShort"></param>
        /// <param name="byteBuffer"></param>
        /// <param name="iBufferIndex"></param>
        public static void ToByteArray( short iShort, ref byte[] byteBuffer, int iBufferIndex )
        {
            if ( byteBuffer == null )
                throw new ArgumentNullException( "byteBuffer" );

            if ( ( iBufferIndex + 2 ) > byteBuffer.Length )
                throw new ArgumentException( "ByteArray.ToByteArray(...) - ( iBufferIndex + 2 ) > byteBuffer.Length error!" );

            unsafe
            {
                fixed ( byte* numRef = byteBuffer )
                    *( (short*)numRef + iBufferIndex ) = iShort;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="iShort"></param>
        /// <returns></returns>
        public static byte[] ToArrayInByte( this short iShort )
        {
            return BitConverter.GetBytes( iShort );
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="iShort"></param>
        /// <param name="byteBuffer"></param>
        /// <param name="iBufferIndex"></param>
        public static void ToArrayInByte( this short iShort, ref byte[] byteBuffer, int iBufferIndex )
        {
            ToByteArray( iShort, ref byteBuffer, iBufferIndex );
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="iUShort"></param>
        /// <returns></returns>
        public static byte[] ToByteArray( ushort iUShort )
        {
            return BitConverter.GetBytes( iUShort );
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="iUShort"></param>
        /// <param name="byteBuffer"></param>
        /// <param name="iBufferIndex"></param>
        public static void ToByteArray( ushort iUShort, ref byte[] byteBuffer, int iBufferIndex )
        {
            if ( byteBuffer == null )
                throw new ArgumentNullException( "byteBuffer" );

            if ( ( iBufferIndex + 2 ) > byteBuffer.Length )
                throw new ArgumentException( "ByteArray.ToByteArray(...) - ( iBufferIndex + 2 ) > byteBuffer.Length error!" );

            unsafe
            {
                fixed ( byte* numRef = byteBuffer )
                    *( (short*)numRef + iBufferIndex ) = (short)iUShort;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="iUShort"></param>
        /// <returns></returns>
        public static byte[] ToArrayInByte( this ushort iUShort )
        {
            return BitConverter.GetBytes( iUShort );
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="iUShort"></param>
        /// <param name="byteBuffer"></param>
        /// <param name="iBufferIndex"></param>
        public static void ToArrayInByte( this ushort iUShort, ref byte[] byteBuffer, int iBufferIndex )
        {
            ToByteArray( iUShort, ref byteBuffer, iBufferIndex );
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cChar"></param>
        /// <returns></returns>
        public static byte[] ToByteArray( char cChar )
        {
            return BitConverter.GetBytes( cChar );
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cChar"></param>
        /// <param name="byteBuffer"></param>
        /// <param name="iBufferIndex"></param>
        public static void ToByteArray( char cChar, ref byte[] byteBuffer, int iBufferIndex )
        {
            if ( byteBuffer == null )
                throw new ArgumentNullException( "byteBuffer" );

            if ( ( iBufferIndex + 2 ) > byteBuffer.Length )
                throw new ArgumentException( "ByteArray.ToByteArray(...) - ( iBufferIndex + 2 ) > byteBuffer.Length error!" );

            unsafe
            {
                fixed ( byte* numRef = byteBuffer )
                    *( (short*)numRef + iBufferIndex ) = (short)cChar;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cChar"></param>
        /// <returns></returns>
        public static byte[] ToArrayInByte( this char cChar )
        {
            return BitConverter.GetBytes( cChar );
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="cChar"></param>
        /// <param name="byteBuffer"></param>
        /// <param name="iBufferIndex"></param>
        public static void ToArrayInByte( this char cChar, ref byte[] byteBuffer, int iBufferIndex )
        {
            ToByteArray( cChar, ref byteBuffer, iBufferIndex );
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="iByte"></param>
        /// <returns></returns>
        public static byte[] ToByteArray( byte iByte )
        {
            return new[] { iByte };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="iByte"></param>
        /// <returns></returns>
        public static byte[] ToArrayInByte( this byte iByte )
        {
            return new[] { iByte };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="iSByte"></param>
        /// <returns></returns>
        public static byte[] ToByteArray( sbyte iSByte )
        {
            return new[] { (byte)iSByte };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="iSByte"></param>
        /// <returns></returns>
        public static byte[] ToArrayInByte( this sbyte iSByte )
        {
            return new[] { (byte)iSByte };
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="bBool"></param>
        /// <returns></returns>
        public static byte[] ToByteArray( bool bBool )
        {
            return new[] { bBool ? (byte)1 : (byte)0 };
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="bBool"></param>
        /// <param name="byteBuffer"></param>
        /// <param name="iBufferIndex"></param>
        public static void ToByteArray( bool bBool, ref byte[] byteBuffer, int iBufferIndex )
        {
            if ( byteBuffer == null )
                throw new ArgumentNullException( "byteBuffer" );

            if ( iBufferIndex > byteBuffer.Length )
                throw new ArgumentException( "ByteArray.ToByteArray(...) - iBufferIndex > byteBuffer.Length error!" );
            
            byteBuffer[iBufferIndex] = bBool ? (byte)1 : (byte)0;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="bBool"></param>
        /// <returns></returns>
        public static byte[] ToArrayInByte( this bool bBool )
        {
            return new[] { bBool ? (byte)1 : (byte)0 };
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="bBool"></param>
        /// <param name="byteBuffer"></param>
        /// <param name="iBufferIndex"></param>
        public static void ToArrayInByte( this bool bBool, ref byte[] byteBuffer, int iBufferIndex = 0 )
        {
            ToByteArray( bBool, ref byteBuffer, iBufferIndex );
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="strString"></param>
        /// <returns></returns>
        public static byte[] ToByteArray( string strString )
        {
            return ConvertString.UTF8.GetBytes( strString );
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="strString"></param>
        /// <param name="byteBuffer"></param>
        /// <param name="iBufferIndex"></param>
        /// <returns>剩余多少字节没有拷贝过去</returns>
        public static int ToByteArray( string strString, ref byte[] byteBuffer, int iBufferIndex  = 0)
        {
            return ToByteArray(strString, Encoding.UTF8, ref byteBuffer, iBufferIndex);            
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="strString"></param>
        /// <returns></returns>
        public static byte[] ToArrayInByte( this string strString )
        {
            return ConvertString.UTF8.GetBytes( strString );
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="strString"></param>
        /// <param name="byteBuffer"></param>
        /// <param name="iBufferIndex"></param>
        /// <returns></returns>
        public static int ToArrayInByte( this string strString, ref byte[] byteBuffer, int iBufferIndex )
        {
            return ToByteArray( strString, ref byteBuffer, iBufferIndex );
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="strString"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public static byte[] ToByteArray( string strString, Encoding encoding )
        {
            return encoding.GetBytes( strString );
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="strString"></param>
        /// <param name="encoding"></param>
        /// <param name="byteBuffer"></param>
        /// <param name="iBufferIndex"></param>
        /// <returns></returns>
        public static int ToByteArray( string strString, Encoding encoding, ref byte[] byteBuffer, int iBufferIndex = 0 )
        {
            if ( byteBuffer == null )
                throw new ArgumentNullException( "byteBuffer" );

            if ( iBufferIndex > byteBuffer.Length )
                throw new ArgumentException( "ByteArray.ToByteArray(...) - iBufferIndex > byteBuffer.Length error!" );

            byte[] byteString = encoding.GetBytes( strString );

            int iResidualSize = byteBuffer.Length - iBufferIndex;

            if ( iResidualSize >= byteBuffer.Length )
                Buffer.BlockCopy( byteString, 0, byteBuffer, iBufferIndex, byteBuffer.Length );
            else
                Buffer.BlockCopy( byteString, 0, byteBuffer, iBufferIndex, iResidualSize );

            return iResidualSize >= byteBuffer.Length ? 0 : byteBuffer.Length - iResidualSize;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="strString"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public static byte[] ToArrayInByte( this string strString, Encoding encoding )
        {
            return encoding.GetBytes( strString );
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="strString"></param>
        /// <param name="encoding"></param>
        /// <param name="byteBuffer"></param>
        /// <param name="iBufferIndex"></param>
        /// <returns></returns>
        public static int ToArrayInByte( this string strString, Encoding encoding, ref byte[] byteBuffer, int iBufferIndex )
        {
            return ToByteArray( strString, encoding, ref byteBuffer, iBufferIndex );
        }
        #endregion

        #region zh-CHS To... | en To...
        /// <summary>
        /// 
        /// </summary>
        /// <param name="intBuffer"></param>
        /// <returns></returns>
        public static int ToInt32( byte[] intBuffer )
        {
            return BitConverter.ToInt32( intBuffer, 0 );
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="intBuffer"></param>
        /// <param name="iBufferIndex"></param>
        /// <returns></returns>
        public static int ToInt32( byte[] intBuffer, int iBufferIndex )
        {
            return BitConverter.ToInt32( intBuffer, iBufferIndex );
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="intBuffer"></param>
        /// <returns></returns>
        public static int ConvertToInt32( this byte[] intBuffer )
        {
            return BitConverter.ToInt32( intBuffer, 0 );
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="intBuffer"></param>
        /// <param name="iBufferIndex"></param>
        /// <returns></returns>
        public static int ConvertToInt32( this byte[] intBuffer, int iBufferIndex )
        {
            return BitConverter.ToInt32( intBuffer, iBufferIndex );
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="uintBuffer"></param>
        /// <returns></returns>
        public static uint ToUInt32( byte[] uintBuffer )
        {
            return BitConverter.ToUInt32( uintBuffer, 0 );
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="uintBuffer"></param>
        /// <param name="iBufferIndex"></param>
        /// <returns></returns>
        public static uint ToUInt32( byte[] uintBuffer, int iBufferIndex )
        {
            return BitConverter.ToUInt32( uintBuffer, iBufferIndex );
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="uintBuffer"></param>
        /// <returns></returns>
        public static uint ConvertToUInt32( this byte[] uintBuffer )
        {
            return BitConverter.ToUInt32( uintBuffer, 0 );
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="uintBuffer"></param>
        /// <param name="iBufferIndex"></param>
        /// <returns></returns>
        public static uint ConvertToUInt32( this byte[] uintBuffer, int iBufferIndex )
        {
            return BitConverter.ToUInt32( uintBuffer, iBufferIndex );
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="uintBuffer"></param>
        /// <returns></returns>
        public static float ToSingle( byte[] uintBuffer )
        {
            return BitConverter.ToSingle( uintBuffer, 0 );
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="uintBuffer"></param>
        /// <param name="iBufferIndex"></param>
        /// <returns></returns>
        public static float ToSingle( byte[] uintBuffer, int iBufferIndex )
        {
            return BitConverter.ToSingle( uintBuffer, iBufferIndex );
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="uintBuffer"></param>
        /// <returns></returns>
        public static float ConvertToSingle( this byte[] uintBuffer )
        {
            return BitConverter.ToSingle( uintBuffer, 0 );
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="uintBuffer"></param>
        /// <param name="iBufferIndex"></param>
        /// <returns></returns>
        public static float ConvertToSingle( this byte[] uintBuffer, int iBufferIndex )
        {
            return BitConverter.ToSingle( uintBuffer, iBufferIndex );
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="longBuffer"></param>
        /// <returns></returns>
        public static long ToInt64( byte[] longBuffer )
        {
            return BitConverter.ToInt64( longBuffer, 0 );
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="longBuffer"></param>
        /// <param name="iBufferIndex"></param>
        /// <returns></returns>
        public static long ToInt64( byte[] longBuffer, int iBufferIndex )
        {
            return BitConverter.ToInt64( longBuffer, iBufferIndex );
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="longBuffer"></param>
        /// <returns></returns>
        public static long ConvertToInt64( this byte[] longBuffer )
        {
            return BitConverter.ToInt64( longBuffer, 0 );
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="longBuffer"></param>
        /// <param name="iBufferIndex"></param>
        /// <returns></returns>
        public static long ConvertToInt64( this byte[] longBuffer, int iBufferIndex )
        {
            return BitConverter.ToInt64( longBuffer, iBufferIndex );
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ulongBuffer"></param>
        /// <returns></returns>
        public static ulong ToUInt64( byte[] ulongBuffer )
        {
            return BitConverter.ToUInt64( ulongBuffer, 0 );
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="ulongBuffer"></param>
        /// <param name="iBufferIndex"></param>
        /// <returns></returns>
        public static ulong ToUInt64( byte[] ulongBuffer, int iBufferIndex )
        {
            return BitConverter.ToUInt64( ulongBuffer, iBufferIndex );
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ulongBuffer"></param>
        /// <returns></returns>
        public static ulong ConvertToUInt64( this byte[] ulongBuffer )
        {
            return BitConverter.ToUInt64( ulongBuffer, 0 );
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="ulongBuffer"></param>
        /// <param name="iBufferIndex"></param>
        /// <returns></returns>
        public static ulong ConvertToUInt64( this byte[] ulongBuffer, int iBufferIndex )
        {
            return BitConverter.ToUInt64( ulongBuffer, iBufferIndex );
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ulongBuffer"></param>
        /// <returns></returns>
        public static double ToDouble( byte[] ulongBuffer )
        {
            return BitConverter.ToDouble( ulongBuffer, 0 );
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="ulongBuffer"></param>
        /// <param name="iBufferIndex"></param>
        /// <returns></returns>
        public static double ToDouble( byte[] ulongBuffer, int iBufferIndex )
        {
            return BitConverter.ToDouble( ulongBuffer, iBufferIndex );
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ulongBuffer"></param>
        /// <returns></returns>
        public static double ConvertToDouble( this byte[] ulongBuffer )
        {
            return BitConverter.ToDouble( ulongBuffer, 0 );
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="ulongBuffer"></param>
        /// <param name="iBufferIndex"></param>
        /// <returns></returns>
        public static double ConvertToDouble( this byte[] ulongBuffer, int iBufferIndex )
        {
            return BitConverter.ToDouble( ulongBuffer, iBufferIndex );
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="shortBuffer"></param>
        /// <returns></returns>
        public static short ToInt16( byte[] shortBuffer )
        {
            return BitConverter.ToInt16( shortBuffer, 0 );
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="shortBuffer"></param>
        /// <param name="iBufferIndex"></param>
        /// <returns></returns>
        public static short ToInt16( byte[] shortBuffer, int iBufferIndex )
        {
            return BitConverter.ToInt16( shortBuffer, iBufferIndex );
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="shortBuffer"></param>
        /// <returns></returns>
        public static short ConvertToInt16( this byte[] shortBuffer )
        {
            return BitConverter.ToInt16( shortBuffer, 0 );
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="shortBuffer"></param>
        /// <param name="iBufferIndex"></param>
        /// <returns></returns>
        public static short ConvertToInt16( this byte[] shortBuffer, int iBufferIndex )
        {
            return BitConverter.ToInt16( shortBuffer, iBufferIndex );
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ushortBuffer"></param>
        /// <returns></returns>
        public static ushort ToUInt16( byte[] ushortBuffer )
        {
            return BitConverter.ToUInt16( ushortBuffer, 0 );
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="ushortBuffer"></param>
        /// <param name="iBufferIndex"></param>
        /// <returns></returns>
        public static ushort ToUInt16( byte[] ushortBuffer, int iBufferIndex )
        {
            return BitConverter.ToUInt16( ushortBuffer, iBufferIndex );
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ushortBuffer"></param>
        /// <returns></returns>
        public static ushort ConvertToUInt16( this byte[] ushortBuffer )
        {
            return BitConverter.ToUInt16( ushortBuffer, 0 );
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="ushortBuffer"></param>
        /// <param name="iBufferIndex"></param>
        /// <returns></returns>
        public static ushort ConvertToUInt16( this byte[] ushortBuffer, int iBufferIndex )
        {
            return BitConverter.ToUInt16( ushortBuffer, iBufferIndex );
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="charBuffer"></param>
        /// <returns></returns>
        public static char ToChar( byte[] charBuffer )
        {
            return BitConverter.ToChar( charBuffer, 0 );
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="charBuffer"></param>
        /// <param name="iBufferIndex"></param>
        /// <returns></returns>
        public static char ToChar( byte[] charBuffer, int iBufferIndex )
        {
            return BitConverter.ToChar( charBuffer, iBufferIndex );
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="charBuffer"></param>
        /// <returns></returns>
        public static char ConvertToChar( this byte[] charBuffer )
        {
            return BitConverter.ToChar( charBuffer, 0 );
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="charBuffer"></param>
        /// <param name="iBufferIndex"></param>
        /// <returns></returns>
        public static char ConvertToChar( this byte[] charBuffer, int iBufferIndex )
        {
            return BitConverter.ToChar( charBuffer, iBufferIndex );
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="charBuffer"></param>
        /// <returns></returns>
        public static bool ToBoolean( byte[] charBuffer )
        {
            return BitConverter.ToBoolean( charBuffer, 0 );
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="charBuffer"></param>
        /// <param name="iBufferIndex"></param>
        /// <returns></returns>
        public static bool ToBoolean( byte[] charBuffer, int iBufferIndex )
        {
            return BitConverter.ToBoolean( charBuffer, iBufferIndex );
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="charBuffer"></param>
        /// <returns></returns>
        public static bool ConvertToBoolean( this byte[] charBuffer )
        {
            return BitConverter.ToBoolean( charBuffer, 0 );
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="charBuffer"></param>
        /// <param name="iBufferIndex"></param>
        /// <returns></returns>
        public static bool ConvertToBoolean( this byte[] charBuffer, int iBufferIndex )
        {
            return BitConverter.ToBoolean( charBuffer, iBufferIndex );
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="byteString"></param>
        /// <returns></returns>
        public static string ToString( byte[] byteString )
        {
            return ConvertString.UTF8.GetString( byteString );
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="byteString"></param>
        /// <returns></returns>
        public static string ConvertToString( this byte[] byteString )
        {
            return ConvertString.UTF8.GetString( byteString );
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="byteString"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public static string ToString( byte[] byteString, Encoding encoding )
        {
            return encoding.GetString( byteString );
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="byteString"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public static string ConvertToString( this byte[] byteString, Encoding encoding )
        {
            return encoding.GetString( byteString );
        }

        /// <summary>
        /// 字节数组转换成字符串
        /// </summary>
        /// <param name="byteString"></param>
        /// <param name="iOffset"></param>
        /// <param name="iSize"></param>
        /// <returns></returns>
        public static string ToString( byte[] byteString, long iOffset, long iSize )
        {
            return ConvertString.UTF8.GetString( byteString, (int)iOffset, (int)iSize );
        }


        /// <summary>
        /// 字节数组转换成字符串
        /// </summary>
        /// <param name="byteString"></param>
        /// <param name="iOffset"></param>
        /// <param name="iSize"></param>
        /// <returns></returns>
        public static string ConvertToString( this byte[] byteString, long iOffset, long iSize )
        {
            return ConvertString.UTF8.GetString( byteString, (int)iOffset, (int)iSize );
        }


        /// <summary>
        /// 字节数组转换成字符串
        /// </summary>
        /// <param name="byteString"></param>
        /// <param name="iOffset"></param>
        /// <param name="iSize"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public static string ToString( byte[] byteString, long iOffset, long iSize, Encoding encoding )
        {
            return encoding.GetString( byteString, (int)iOffset, (int)iSize );
        }

        /// <summary>
        /// 字节数组转换成字符串
        /// </summary>
        /// <param name="byteString"></param>
        /// <param name="iOffset"></param>
        /// <param name="iSize"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public static string ConvertToString( this byte[] byteString, long iOffset, long iSize, Encoding encoding )
        {
            return encoding.GetString( byteString, (int)iOffset, (int)iSize );
        }
        #endregion

        #region zh-CHS Concat | en Concat
        /// <summary>
        /// 顺序连接两个数组
        /// </summary>
        /// <param name="bufferA"></param>
        /// <param name="bufferB"></param>
        /// <returns>返回连接后的数组</returns>
        public static byte[] Concat( byte[] bufferA, byte[] bufferB )
        {
            if ( bufferA == null )
                throw new ArgumentNullException( "bufferA" );

            if ( bufferB == null )
                throw new ArgumentNullException( "bufferB" );

            var byteBuffer = new byte[bufferA.Length + bufferB.Length];

            Buffer.BlockCopy( bufferA, 0, byteBuffer, 0, bufferA.Length );
            Buffer.BlockCopy( bufferB, 0, byteBuffer, bufferA.Length, bufferB.Length );

            return byteBuffer;
        }


        /// <summary>
        /// 顺序连接两个数组
        /// </summary>
        /// <param name="thisBuffer"></param>
        /// <param name="concatBuffer"></param>
        /// <returns>返回连接后的数组</returns>
        public static byte[] Coalition( this byte[] thisBuffer, byte[] concatBuffer )
        {
            if ( thisBuffer == null )
                throw new ArgumentNullException( "thisBuffer" );

            if ( concatBuffer == null )
                throw new ArgumentNullException( "concatBuffer" );

            var byteBuffer = new byte[thisBuffer.Length + concatBuffer.Length];

            Buffer.BlockCopy( thisBuffer, 0, byteBuffer, 0, thisBuffer.Length );
            Buffer.BlockCopy( concatBuffer, 0, byteBuffer, thisBuffer.Length, concatBuffer.Length );

            return byteBuffer;
        }


        /// <summary>
        /// 顺序连接两个数组
        /// </summary>
        /// <param name="bufferA"></param>
        /// <param name="iOffsetA"></param>
        /// <param name="iSizeA"></param>
        /// <param name="bufferB"></param>
        /// <param name="iOffsetB"></param>
        /// <param name="iSizeB"></param>
        /// <returns></returns>
        public static byte[] Concat( byte[] bufferA, long iOffsetA, long iSizeA, byte[] bufferB, long iOffsetB, long iSizeB )
        {
            if ( bufferA == null )
                throw new ArgumentNullException( "bufferA" );

            if ( bufferB == null )
                throw new ArgumentNullException( "bufferB" );
            
            if ( ( iOffsetA + iSizeA ) > bufferA.Length )
                throw new ArgumentException( "ByteArray.Concat(...) - ( iOffsetA + iSizeA ) > bufferA.Length error!" );

            if ( ( iOffsetB + iSizeB ) > bufferB.Length )
                throw new ArgumentException( "ByteArray.Concat(...) - ( iOffsetB + iSizeB ) > bufferB.Length error!" );

            var byteBuffer = new byte[iSizeA + iSizeB];

            Buffer.BlockCopy( bufferA, (int)iOffsetA, byteBuffer, 0, (int)iSizeA );
            Buffer.BlockCopy( bufferB, (int)iOffsetB, byteBuffer, (int)iSizeA, (int)iSizeB );

            return byteBuffer;
        }


        /// <summary>
        /// 顺序连接两个数组
        /// </summary>
        /// <param name="thisBuffer"></param>
        /// <param name="iThisOffset"></param>
        /// <param name="iThisSize"></param>
        /// <param name="bufferB"></param>
        /// <param name="iOffsetB"></param>
        /// <param name="iSizeB"></param>
        /// <returns></returns>
        public static byte[] Coalition( this byte[] thisBuffer, long iThisOffset, long iThisSize, byte[] bufferB, long iOffsetB, long iSizeB )
        {
            if ( thisBuffer == null )
                throw new ArgumentNullException( "thisBuffer" );

            if ( bufferB == null )
                throw new ArgumentNullException( "bufferB" );

            if ( ( iThisOffset + iThisSize ) > thisBuffer.Length )
                throw new ArgumentException( "ByteArray.Coalition(...) - ( iThisOffset + iThisSize ) > thisBuffer.Length error!" );

            if ( ( iOffsetB + iSizeB ) > bufferB.Length )
                throw new ArgumentException( "ByteArray.Coalition(...) - ( iOffsetB + iSizeB ) > bufferB.Length error!" );

            var byteBuffer = new byte[iThisSize + iSizeB];

            Buffer.BlockCopy( thisBuffer, (int)iThisOffset, byteBuffer, 0, (int)iThisSize );
            Buffer.BlockCopy( bufferB, (int)iOffsetB, byteBuffer, (int)iThisSize, (int)iSizeB );

            return byteBuffer;
        }
        #endregion
    }
}
#endregion