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
using System.IO;
using System.Text;
using System;
#endregion

namespace DogSE.Library.Util
{
    /// <summary>
    /// 
    /// </summary>
    public static class Utility
    {
        #region zh-CHS Text Format方法 | en Public Static Methods
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Output"></param>
        /// <param name="streamInput"></param>
        /// <param name="iLength"></param>
        public static void FormatBuffer( TextWriter Output, System.IO.Stream streamInput, long iLength )
        {
            Output.WriteLine( "     | -- -- -- -- -- -- -- --  -- -- -- -- -- -- -- -- | ---------------- |" );
            Output.WriteLine( "     | 00 01 02 03 04 05 06 07  08 09 0A 0B 0C 0D 0E 0F | 0123456789ABCDEF |" );
            Output.WriteLine( "     | -- -- -- -- -- -- -- --  -- -- -- -- -- -- -- -- | ---------------- |" );

            long iByteIndex = 0;
            long iWhole = iLength >> 4;
            long iRem = iLength & 0xF;

            for ( long iIndex = 0; iIndex < iWhole; ++iIndex, iByteIndex += 16 )
            {
                StringBuilder strBytes = new StringBuilder( 49 );
                StringBuilder strChars = new StringBuilder( 16 );

                for ( int iIndex2 = 0; iIndex2 < 16; ++iIndex2 )
                {
                    int iByte = streamInput.ReadByte();

                    strBytes.Append( iByte.ToString( "X2" ) );

                    if ( iIndex2 != 7 )
                        strBytes.Append( ' ' );
                    else
                        strBytes.Append( "  " );

                    if ( iByte >= 0x20 && iByte < 0x80 )
                        strChars.Append( (char)iByte );
                    else
                        strChars.Append( '.' );
                }

                Output.Write( iByteIndex.ToString( "X4" ) );
                Output.Write( "   " );
                Output.Write( strBytes.ToString() );
                Output.Write( "  " );
                Output.WriteLine( strChars.ToString() );
            }

            if ( iRem != 0 )
            {
                StringBuilder strBytes = new StringBuilder( 49 );
                StringBuilder strChars = new StringBuilder( (int)iRem );

                for ( long iIndex2 = 0; iIndex2 < 16; ++iIndex2 )
                {
                    if ( iIndex2 < iRem )
                    {
                        long iByte = streamInput.ReadByte();

                        strBytes.Append( iByte.ToString( "X2" ) );

                        if ( iIndex2 != 7 )
                            strBytes.Append( ' ' );
                        else
                            strBytes.Append( "  " );

                        if ( iByte >= 0x20 && iByte < 0x80 )
                            strChars.Append( (char)iByte );
                        else
                            strChars.Append( '.' );
                    }
                    else
                        strBytes.Append( "   " );
                }

                if ( iRem <= 7 )
                    strBytes.Append( ' ' );

                Output.Write( iByteIndex.ToString( "X4" ) );
                Output.Write( "   " );
                Output.Write( strBytes.ToString() );
                Output.Write( "  " );
                Output.WriteLine( strChars.ToString() );
            }
        }
        #endregion

        #region zh-CHS 获取枚举的最大最小值 方法 | en Public Static Methods
        /// <summary>
        /// 获取枚举的最大值
        /// </summary>
        /// <typeparam name="EnumTypeT"></typeparam>
        /// <param name="enumType"></param>
        /// <returns></returns>
        public static EnumTypeT GetEmunMaxValues<EnumTypeT>( Type enumType ) where EnumTypeT : IComparable<EnumTypeT>
        {
            EnumTypeT maxValue = default( EnumTypeT );

            Array enumArray = Enum.GetValues( enumType );
            foreach ( var item in enumArray )
            {
                if ( maxValue.CompareTo( (EnumTypeT)item ) < 0 )
                    maxValue = (EnumTypeT)item;
            }

            return maxValue;
        }


        /// <summary>
        /// 获取枚举的最小值
        /// </summary>
        /// <typeparam name="EnumTypeT"></typeparam>
        /// <param name="enumType"></param>
        /// <returns></returns>
        public static EnumTypeT GetEmunMinValues<EnumTypeT>( Type enumType ) where EnumTypeT : IComparable<EnumTypeT>
        {
            EnumTypeT minValue = default( EnumTypeT );

            Array enumArray = Enum.GetValues( enumType );
            foreach ( var item in enumArray )
            {
                if ( minValue.CompareTo( (EnumTypeT)item ) > 0 )
                    minValue = (EnumTypeT)item;
            }

            return minValue;
        }
        #endregion
    }
}
#endregion

