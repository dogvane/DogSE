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
#endregion

namespace DogSE.Library.Util
{
    /// <summary>
    /// 
    /// </summary>
    public static class RandomEx
    {
        #region zh-CHS 私有静态成员变量 | en Private Static Member Variables
        /// <summary>
        /// 
        /// </summary>
        private static Random s_Random = new Random();
        #endregion

        #region zh-CHS 共有静态方法 | en Public Static Methods
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static double RandomDouble()
        {
            return s_Random.NextDouble();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static bool RandomBool()
        {
            return ( s_Random.Next( 2 ) == 0 );
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="iMaxValue"></param>
        /// <returns></returns>
        public static int Random( int iMaxValue )
        {
            return s_Random.Next( iMaxValue );
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="arrayT"></param>
        /// <returns></returns>
        public static T RandomArray<T>( T[] arrayT )
        {
            if ( arrayT.Length > 0 )
                return arrayT[s_Random.Next( arrayT.Length )];
            else
                return default( T );
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="listT"></param>
        /// <returns></returns>
        public static T RandomList<T>( List<T> listT )
        {
            if ( listT.Count > 0 )
                return listT[s_Random.Next( listT.Count )];
            else
                return default( T );
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bufferRandom"></param>
        /// <returns></returns>
        public static void RandomInBytes( ref byte[] bufferRandom )
        {
            s_Random.NextBytes( bufferRandom );
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="iMinValue"></param>
        /// <param name="iMaxValue"></param>
        /// <returns></returns>
        public static int RandomMinMax( int iMinValue, int iMaxValue )
        {
            if ( iMinValue > iMaxValue )
            {
                int iCopy = iMinValue;
                iMinValue = iMaxValue;
                iMaxValue = iCopy;
            }
            else if ( iMinValue == iMaxValue )
                return iMinValue;

            return s_Random.Next( iMinValue, iMaxValue );
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="iBaseValue"></param>
        /// <param name="iAppendMaxValue"></param>
        /// <returns></returns>
        public static int Random( int iBaseValue, int iAppendMaxValue )
        {
            if ( iAppendMaxValue == 0 )
                return iBaseValue;
            else if ( iAppendMaxValue > 0 )
                return iBaseValue + s_Random.Next( iAppendMaxValue );
            else
                return iBaseValue - s_Random.Next( -iAppendMaxValue );
        }

        /// <summary>
        /// 千分比的触发几率
        /// </summary>
        /// <returns></returns>
        public static bool IsTriggerProbability1000( int iPermille )
        {
            if ( iPermille >= 1000 )
                return true;
            else if ( iPermille <= 0 )
                return false;

            int iDigit100 = iPermille / 100;
            int iDigit10 = ( iPermille / 10 ) % 10;
            int iDigit = iPermille % 10;

            if ( s_Random.Next( 10 ) < iDigit100 )
                return true;

            if ( s_Random.Next( 10 ) < iDigit10 )
                return true;

            if ( s_Random.Next( 10 ) < iDigit )
                return true;

            return false;
        }

        /// <summary>
        /// 百分比的触发几率
        /// </summary>
        /// <returns></returns>
        public static bool IsTriggerProbability100( int iPercent )
        {
            if ( iPercent >= 100 )
                return true;
            else if ( iPercent <= 0 )
                return false;

            int iDigit10 = iPercent / 10;
            int iDigit = iPercent % 10;

            if ( s_Random.Next( 10 ) < iDigit10 )
                return true;

            if ( s_Random.Next( 10 ) < iDigit )
                return true;

            return false;
        }

        /// <summary>
        /// 十分比的触发几率
        /// </summary>
        /// <returns></returns>
        public static bool IsTriggerProbability10( int iProbability )
        {
            if ( iProbability >= 10 )
                return true;
            else if ( iProbability <= 0 )
                return false;

            if ( s_Random.Next( 10 ) < iProbability )
                return true;

            return false;
        }

        /// <summary>
        /// 随机数
        /// </summary>
        /// <returns></returns>
        public static uint RandomBitInUint( byte iBitNumber, byte iRandomCount )
        {
            if ( iBitNumber > 32 )
                throw new Exception( "RandomEx.RandomBitInUint(...) - iBitNumber > 32 error!" );

            if ( iRandomCount > iBitNumber )
                throw new Exception( "RandomEx.RandomBitInUint(...) - iRandomCount > iBitNumber error!" );

            uint randomUint = 0;

            for ( int i = 0; i < iRandomCount; i++ )
            {
                do
                {
                    int randomBit = Random( iBitNumber );

                    uint bitValue = (uint)1 << randomBit;

                    if ( ( randomUint & bitValue ) == bitValue )
                        continue;
                    else
                    {
                        randomUint |= bitValue;
                        break;
                    }

                } while ( true );
            }

            return randomUint;
        }

        /// <summary>
        /// 随机数
        /// </summary>
        /// <returns></returns>
        public static ushort RandomBitInUshort( byte iBitNumber, byte iRandomCount )
        {
            if ( iBitNumber > 16 )
                throw new Exception( "RandomEx.RandomBitInUint(...) - iBitNumber > 16 error!" );

            if ( iRandomCount > iBitNumber )
                throw new Exception( "RandomEx.RandomBitInUint(...) - iRandomCount > iBitNumber error!" );

            ushort randomUshort = 0;

            for ( int i = 0; i < iRandomCount; i++ )
            {
                do
                {
                    int randomBit = Random( iBitNumber );

                    ushort bitValue = (ushort)( 1 << randomBit );

                    if ( ( randomUshort & bitValue ) == bitValue )
                        continue;
                    else
                    {
                        randomUshort |= bitValue;
                        break;
                    }

                } while ( true );
            }

            return randomUshort;
        }

        /// <summary>
        /// 随机数
        /// </summary>
        /// <returns></returns>
        public static byte RandomBitInByte( byte iBitNumber, byte iRandomCount )
        {
            if ( iBitNumber > 8 )
                throw new Exception( "RandomEx.RandomBitInUint(...) - iBitNumber > 8 error!" );

            if ( iRandomCount > iBitNumber )
                throw new Exception( "RandomEx.RandomBitInUint(...) - iRandomCount > iBitNumber error!" );

            byte randomByte = 0;

            for ( int i = 0; i < iRandomCount; i++ )
            {
                do
                {
                    int randomBit = Random( iBitNumber );

                    byte bitValue = (byte)( 1 << randomBit );

                    if ( ( randomByte & bitValue ) == bitValue )
                        continue;
                    else
                    {
                        randomByte |= bitValue;
                        break;
                    }

                } while ( true );
            }

            return randomByte;
        }

        #endregion
    }
}
#endregion