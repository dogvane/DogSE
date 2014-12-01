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
using System.Threading;
#endregion

namespace DogSE.Common
{
    /// <summary>
    /// 
    /// </summary>
    public struct Serial : IComparable, IComparable<Serial>, IEquatable<Serial>
    {
        #region zh-CHS 类常量 | en Class Constants
        /// <summary>
        /// 
        /// </summary>
        public static readonly Serial MinusOne = new Serial( -1 );
        /// <summary>
        /// 
        /// </summary>
        public static readonly Serial Zero = new Serial( 0 );
        #endregion

        #region zh-CHS 构造和初始化和清理 | en Constructors and Initializers and Dispose

        /// <summary>
        /// 
        /// </summary>
        /// <param name="lSerial"></param>
        public Serial( long lSerial )
        {
            m_Serial = lSerial;
        }
        #endregion

        #region zh-CHS 属性 | en Properties
        #region zh-CHS 私有成员变量 | en Private Member Variables
        /// <summary>
        /// 
        /// </summary>
        private readonly long m_Serial;
        #endregion
        /// <summary>
        /// 
        /// </summary>
        public long Value
        {
            get { return m_Serial; }
        }
        #endregion

        #region zh-CHS 方法 | en Method
        /// <summary>
        /// 
        /// </summary>
        /// <param name="serialA"></param>
        /// <param name="serialB"></param>
        /// <returns></returns>
        public static bool operator ==( Serial serialA, Serial serialB )
        {
            return serialA.m_Serial == serialB.m_Serial;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="serialA"></param>
        /// <param name="serialB"></param>
        /// <returns></returns>
        public static bool operator !=( Serial serialA, Serial serialB )
        {
            return serialA.m_Serial != serialB.m_Serial;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="serialA"></param>
        /// <param name="serialB"></param>
        /// <returns></returns>
        public static bool operator >( Serial serialA, Serial serialB )
        {
            return serialA.m_Serial > serialB.m_Serial;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="serialA"></param>
        /// <param name="serialB"></param>
        /// <returns></returns>
        public static bool operator <( Serial serialA, Serial serialB )
        {
            return serialA.m_Serial < serialB.m_Serial;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="serialA"></param>
        /// <param name="serialB"></param>
        /// <returns></returns>
        public static bool operator >=( Serial serialA, Serial serialB )
        {
            return serialA.m_Serial >= serialB.m_Serial;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="serialA"></param>
        /// <param name="serialB"></param>
        /// <returns></returns>
        public static bool operator <=( Serial serialA, Serial serialB )
        {
            return serialA.m_Serial <= serialB.m_Serial;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="serial"></param>
        /// <returns></returns>
        public static implicit operator long( Serial serial )
        {
            return serial.m_Serial;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="serial"></param>
        /// <returns></returns>
        public static explicit operator ulong( Serial serial )
        {
            return (ulong)serial.m_Serial;
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="serial"></param>
        /// <returns></returns>
        public static explicit operator int( Serial serial )
        {
            return (int)serial.m_Serial;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="serial"></param>
        /// <returns></returns>
        public static explicit operator uint( Serial serial )
        {
            return (uint)serial.m_Serial;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="serial"></param>
        /// <returns></returns>
        public static explicit operator short( Serial serial )
        {
            return (short)serial.m_Serial;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="serial"></param>
        /// <returns></returns>
        public static explicit operator ushort( Serial serial )
        {
            return (ushort)serial.m_Serial;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="serial"></param>
        /// <returns></returns>
        public static explicit operator byte( Serial serial )
        {
            return (byte)serial.m_Serial;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="serial"></param>
        /// <returns></returns>
        public static explicit operator sbyte( Serial serial )
        {
            return (sbyte)serial.m_Serial;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="lSerial"></param>
        /// <returns></returns>
        public static implicit operator Serial( long lSerial )
        {
            return new Serial( lSerial );
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="ulSerial"></param>
        /// <returns></returns>
        public static explicit operator Serial( ulong ulSerial )
        {
            return new Serial( (long)ulSerial );
        }
        #endregion

        #region zh-CHS 方法覆盖 | en Method Overrides

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return String.Format( "0x{0:X8}", m_Serial );
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return m_Serial.GetHashCode();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="xObject"></param>
        /// <returns></returns>
        public override bool Equals( object xObject )
        {
            if (!(xObject is Serial))
                return false;

            return ( (Serial)xObject ).m_Serial == m_Serial;
        }

        #endregion

        #region zh-CHS 接口实现 | en Interface Implementation

        /// <summary>
        /// 
        /// </summary>
        /// <param name="other"></param>
        public bool Equals( Serial other )
        {
            return m_Serial == other.m_Serial;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="otherSerial"></param>
        /// <returns></returns>
        public int CompareTo( Serial otherSerial )
        {
            return m_Serial.CompareTo( otherSerial.m_Serial );
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="otherObject"></param>
        /// <returns></returns>
        public int CompareTo( object otherObject )
        {
            if ( otherObject == null )
                return 1;
            if ( otherObject is Serial )
                return CompareTo( (Serial)otherObject );

            return -1;
        }

        #endregion
    }

    /// <summary>
    /// 给出唯一的Serial
    /// </summary>
    public class ExclusiveSerial
    {
        #region zh-CHS 构造和初始化和清理 | en Constructors and Initializers and Dispose
        /// <summary>
        /// 
        /// </summary>
        public ExclusiveSerial()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="lMinSerial"></param>
        public ExclusiveSerial( long lMinSerial )
        {
            m_MinSerial = lMinSerial;
            m_MaxSerial = long.MaxValue;

            m_ExclusiveSerialIndex = m_MinSerial;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="lMaxSerial"></param>
        /// <param name="lMinSerial"></param>
        public ExclusiveSerial( long lMinSerial, long lMaxSerial )
        {
            if ( lMinSerial > lMaxSerial )
                throw new ArgumentException( "ExclusiveSerial.ExclusiveSerial(...) - lMinSerial > lMaxSerial error!", "lMinSerial" );

            m_MinSerial = lMinSerial;
            m_MaxSerial = lMaxSerial;

            m_ExclusiveSerialIndex = m_MinSerial;
        }
        #endregion

        #region zh-CHS 共有属性 | en Public Properties
        #region zh-CHS 私有成员变量 | en Private Member Variables
        /// <summary>
        /// 
        /// </summary>
        private long m_MinSerial;
        #endregion
        /// <summary>
        /// 
        /// </summary>
        public long MinSerial
        {
            get { return m_MinSerial; }
        }

        #region zh-CHS 私有成员变量 | en Private Member Variables
        /// <summary>
        /// 
        /// </summary>
        private long m_MaxSerial = long.MaxValue;
        #endregion
        /// <summary>
        /// 
        /// </summary>
        public long MaxSerial
        {
            get { return m_MaxSerial; }
        }
        #endregion

        #region zh-CHS 私有成员变量 | en Private Member Variables
        /// <summary>
        /// 
        /// </summary>
        private readonly Queue<long> m_ExclusiveSerial = new Queue<long>( 20 );
        /// <summary>
        /// 
        /// </summary>
        private readonly object m_LockExclusiveSerial = new object();
        /// <summary>
        /// 
        /// </summary>
        private long m_ExclusiveSerialIndex;
        #endregion

        #region zh-CHS 共有方法 | en Public Methods
        /// <summary>
        /// 
        /// </summary>
        public Serial NextExclusiveSerial()
        {
            Serial serial;

            lock( m_LockExclusiveSerial )
            {
                if ( m_ExclusiveSerial.Count > 0 )
                    serial = m_ExclusiveSerial.Dequeue();
                else
                    serial = Interlocked.Increment( ref m_ExclusiveSerialIndex );
            }

            if ( serial < m_MinSerial )
                serial = m_MinSerial;
            else if ( serial > m_MaxSerial )
                serial = m_MaxSerial;

            return serial;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="serial"></param>
        public void ReleaseSerial( Serial serial )
        {
            lock(m_LockExclusiveSerial )
            {
                m_ExclusiveSerial.Enqueue( serial );
            }
        }
        #endregion
    }
}
#endregion