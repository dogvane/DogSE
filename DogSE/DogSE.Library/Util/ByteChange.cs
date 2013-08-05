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

namespace DogSE.Library.Util
{
    /// <summary>
    /// 
    /// </summary>
    public static class ByteChange
    {
        #region Convert FLOAT UINT INT
        /// <summary>
        /// 
        /// </summary>
        /// <param name="fValue"></param>
        /// <returns></returns>
        public static uint FloatToUint( float fValue )
        {
            return new CONVERT_FLOAT_INT_UINT { fFloat = fValue }.uiUint;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fValue"></param>
        /// <returns></returns>
        public static uint ChangeToUint( this float fValue )
        {
            return FloatToUint( fValue );
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fValue"></param>
        /// <returns></returns>
        public static int FloatToInt( float fValue )
        {
            return new CONVERT_FLOAT_INT_UINT { fFloat = fValue }.iInt;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fValue"></param>
        /// <returns></returns>
        public static int ChangeToInt( this float fValue )
        {
            return FloatToInt( fValue );
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="uiValue"></param>
        /// <returns></returns>
        public static float UintToFloat( uint uiValue )
        {
            return new CONVERT_FLOAT_INT_UINT { uiUint = uiValue }.fFloat;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="uiValue"></param>
        /// <returns></returns>
        public static float ChangeToFloat( this uint uiValue )
        {
            return UintToFloat( uiValue );
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="iValue"></param>
        /// <returns></returns>
        public static float IntToFloat( int iValue )
        {
            return new CONVERT_FLOAT_INT_UINT { iInt = iValue }.fFloat;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="iValue"></param>
        /// <returns></returns>
        public static float ChangeToFloat( this int iValue )
        {
            return IntToFloat( iValue );
        }
        #endregion

        #region Convert DOUBLE LONG ULONG

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dValue"></param>
        /// <returns></returns>
        public static long DoubleToLong( double dValue )
        {
            return new CONVERT_DOUBLE_LONG_ULONG { dDouble = dValue }.lLong;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="dValue"></param>
        /// <returns></returns>
        public static long ChangeToLong( this double dValue )
        {
            return DoubleToLong( dValue );
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dValue"></param>
        /// <returns></returns>
        public static ulong DoubleToUlong( double dValue )
        {
            return new CONVERT_DOUBLE_LONG_ULONG { dDouble = dValue }.ulUlong;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dValue"></param>
        /// <returns></returns>
        public static ulong ChangeToUlong( this double dValue )
        {
            return DoubleToUlong( dValue );
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="lValue"></param>
        /// <returns></returns>
        public static double LongToDouble( long lValue )
        {
            return new CONVERT_DOUBLE_LONG_ULONG { lLong = lValue }.dDouble;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="lValue"></param>
        /// <returns></returns>
        public static double ChangeToDouble( this long lValue )
        {
            return LongToDouble( lValue );
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ulValue"></param>
        /// <returns></returns>
        public static double UlongToDouble( ulong ulValue )
        {
            return new CONVERT_DOUBLE_LONG_ULONG { ulUlong = ulValue }.dDouble;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ulValue"></param>
        /// <returns></returns>
        public static double ChangeToDouble( this ulong ulValue )
        {
            return UlongToDouble( ulValue );
        }
        #endregion
    }
}