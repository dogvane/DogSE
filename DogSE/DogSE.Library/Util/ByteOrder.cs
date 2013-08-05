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

#endregion

namespace DogSE.Library.Util
{
    /// <summary>
    /// 
    /// </summary>
    public static class ByteOrder
    {
        #region zh-CHS 共有静态方法 | en Public Static Methods
        /// <summary>
        /// 
        /// </summary>
        /// <param name="iNetUShort"></param>
        /// <returns></returns>
        public static ushort NetToHost( ushort iNetUShort )
        {
            return HostToNet( iNetUShort );
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="iNetUShort"></param>
        /// <returns></returns>
        public static ushort OrderToHost( this ushort iNetUShort )
        {
            return HostToNet( iNetUShort );
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="iHostUShort"></param>
        /// <returns></returns>
        public static ushort HostToNet( ushort iHostUShort )
        {
            ushort iUShortA = iHostUShort;
            ushort iUShortB = iHostUShort;

            return (ushort)( ( ( iUShortA >> 8 ) & 0x00FF ) | ( ( iUShortB << 8 ) & 0xFF00 ) );
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="iHostUShort"></param>
        /// <returns></returns>
        public static ushort OrderToNet( this ushort iHostUShort )
        {
            return HostToNet( iHostUShort );
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="iNetULong"></param>
        /// <returns></returns>
        public static ulong NetToHost( ulong iNetULong )
        {
            return HostToNet( iNetULong );
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="iNetULong"></param>
        /// <returns></returns>
        public static ulong OrderToHost( ulong iNetULong )
        {
            return HostToNet( iNetULong );
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="iHostULong"></param>
        /// <returns></returns>
        public static ulong HostToNet( ulong iHostULong )
        {
            ulong iULongA = iHostULong;
            ulong iULongB = iHostULong;
            ulong iULongC = iHostULong;
            ulong iULongD = iHostULong;

            return ( ( iULongA << 24 ) & 0xFF000000 )
                   | ( ( iULongB << 8 ) & 0x00FF0000 )
                   | ( ( iULongC >> 8 ) & 0x0000FF00 )
                   | ( ( iULongD >> 24 ) & 0x000000FF );
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="iHostULong"></param>
        /// <returns></returns>
        public static ulong OrderToNet( ulong iHostULong )
        {
            return HostToNet( iHostULong );
        }
        #endregion
    }
}
#endregion