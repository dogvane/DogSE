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

namespace DogSE.Server.Net
{
    #region zh-CHS 接口 | en Interface
    /// <summary>
    /// 加解密数据包的接口
    /// </summary>
    public interface IPacketEncoder
    {
        #region zh-CHS 接口 | en Interface
        /// <summary>
        /// 加密数据包
        /// </summary>
        /// <param name="netStateTo"></param>
        /// <param name="byteBuffer"></param>
        /// <param name="iLength"></param>
        void EncodeOutgoingPacket( NetState netStateTo, ref byte[] byteBuffer, ref long iLength );
        /// <summary>
        /// 解密数据包
        /// </summary>
        /// <param name="netStateFrom"></param>
        /// <param name="byteBuffer"></param>
        /// <param name="iLength"></param>
        void DecodeIncomingPacket( NetState netStateFrom, ref byte[] byteBuffer, ref long iLength );
        #endregion
    }

    /// <summary>
    /// 可填充得扩展数据接口
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IExtendData<T>
    {
        T Data { get; set; }
    }
    #endregion
}
#endregion

