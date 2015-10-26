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

namespace DogSE.Client.Core.Net
{
    /// <summary>
    /// 数据包的详细信息
    /// </summary>
    public class PacketProfile
    {
        #region zh-CHS 私有成员变量 | en Private Member Variables
        /// <summary>
        /// 计算了的次数
        /// </summary>
        private long m_Count;
        /// <summary>
        /// 构造的次数
        /// </summary>
        private long m_Constructed;
        /// <summary>
        /// 总共的处理字节
        /// </summary>
        private long m_TotalByteLength;
        /// <summary>
        /// 是Outgoing还是Incoming
        /// </summary>
        private bool m_Outgoing;
        /// <summary>
        /// 总共的处理时间
        /// </summary>
        private TimeSpan m_TotalProcTime;
        /// <summary>
        /// 最高的处理时间
        /// </summary>
        private TimeSpan m_PeakProcTime;
        #endregion

        #region zh-CHS 构造和初始化和清理 | en Constructors and Initializers and Dispose
        /// <summary>
        /// 包的信息
        /// </summary>
        /// <param name="bOutgoing">是输入包还是输出包</param>
        private PacketProfile( bool bOutgoing )
        {
            m_Outgoing = bOutgoing;
        }
        #endregion

        #region zh-CHS 属性 | en Properties
        /// <summary>
        /// 输入还是输出包
        /// </summary>
        public bool Outgoing
        {
            get { return m_Outgoing; }
        }

        /// <summary>
        /// 构造的次数
        /// </summary>
        public long Constructed
        {
            get { return m_Constructed; }
        }

        /// <summary>
        /// 总共的处理字节
        /// </summary>
        public long TotalByteLength
        {
            get { return m_TotalByteLength; }
        }

        /// <summary>
        /// 总共的处理时间
        /// </summary>
        public TimeSpan TotalProcTime
        {
            get { return m_TotalProcTime; }
        }

        /// <summary>
        /// 最高的处理时间
        /// </summary>
        public TimeSpan PeakProcTime
        {
            get { return m_PeakProcTime; }
        }

        /// <summary>
        /// 计算了的次数
        /// </summary>
        public long Count
        {
            get { return m_Count; }
        }

        /// <summary>
        /// 平均的处理字节
        /// </summary>
        public double AverageByteLength
        {
            get
            {
                if ( m_Count == 0 )
                    return 0;

                return Math.Round( (double)m_TotalByteLength / m_Count, 2 );
            }
        }

        /// <summary>
        /// 平均的处理时间
        /// </summary>
        public TimeSpan AverageProcTime
        {
            get
            {
                if ( m_Count == 0 )
                    return TimeSpan.Zero;

                return TimeSpan.FromTicks( m_TotalProcTime.Ticks / m_Count );
            }
        }
        #endregion

        #region zh-CHS 方法 | en Method
        /// <summary>
        /// 计算
        /// </summary>
        /// <param name="iByteLength"></param>
        /// <param name="processTime"></param>
        public void Record( long iByteLength, TimeSpan processTime )
        {
            ++m_Count;
            m_TotalByteLength += iByteLength;
            m_TotalProcTime += processTime;

            if ( processTime > m_PeakProcTime )
                m_PeakProcTime = processTime;
        }

        /// <summary>
        /// 该类的构造了的次数
        /// </summary>
        public void RegConstruct()
        {
            ++m_Constructed;
        }
        #endregion

        #region zh-CHS 静态方法 | en Static Method
        #region zh-CHS 私有静态成员变量 | en Private Static Member Variables
        /// <summary>
        /// 
        /// </summary>
        private static Dictionary<long, PacketProfile> s_OutgoingProfiles = new Dictionary<long, PacketProfile>();
         #endregion
        /// <summary>
        /// 给出当前输出包ID的包属性
        /// </summary>
        /// <param name="iPacketID">数据包的ID</param>
        /// <returns>返回当前包ID的包属性</returns>
        public static PacketProfile GetOutgoingProfile( long iPacketID )
        {

            PacketProfile packetProfile;
            s_OutgoingProfiles.TryGetValue( iPacketID, out packetProfile );

            if ( packetProfile == null )
                s_OutgoingProfiles[iPacketID] = packetProfile = new PacketProfile( true );

            return packetProfile;
        }

        #region zh-CHS 私有静态成员变量 | en Private Static Member Variables
        /// <summary>
        /// 
        /// </summary>
        private static Dictionary<long, PacketProfile> s_IncomingProfiles = new Dictionary<long, PacketProfile>();
        #endregion
        /// <summary>
        /// 给出当前输入包ID的包属性
        /// </summary>
        /// <param name="iPacketID">数据包的ID</param>
        /// <returns>返回当前包ID的包属性</returns>
        public static PacketProfile GetIncomingProfile( long iPacketID )
        {
            PacketProfile packetProfile;
            s_IncomingProfiles.TryGetValue( iPacketID, out packetProfile );

            if ( packetProfile == null )
                s_IncomingProfiles[iPacketID] = packetProfile = new PacketProfile( false );

            return packetProfile;
        }
        #endregion
    }
}
#endregion

