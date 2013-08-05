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
using System.Runtime.InteropServices;
#endregion

namespace DogSE.Library.Util
{
    #region Convert FLOAT INT UINT
    /// <summary>
    /// 
    /// </summary>
    [StructLayout( LayoutKind.Explicit, Size = 4 )]
    public struct CONVERT_FLOAT_INT_UINT
    {
        /// <summary>
        /// 
        /// </summary>
        [FieldOffset( 0 )]
        public uint uiUint;

        /// <summary>
        /// 
        /// </summary>
        [FieldOffset( 0 )]
        public int iInt;

        /// <summary>
        /// 
        /// </summary>
        [FieldOffset( 0 )]
        public float fFloat;
    }
    #endregion

    #region Convert DOUBLE LONG ULONG
    /// <summary>
    /// 
    /// </summary>
    [StructLayout( LayoutKind.Explicit, Size = 8 )]
    public struct CONVERT_DOUBLE_LONG_ULONG
    {
        /// <summary>
        /// 
        /// </summary>
        [FieldOffset( 0 )]
        public ulong ulUlong;

        /// <summary>
        /// 
        /// </summary>
        [FieldOffset( 0 )]
        public long lLong;

        /// <summary>
        /// 
        /// </summary>
        [FieldOffset( 0 )]
        public double dDouble;
    }
    #endregion
}
#endregion