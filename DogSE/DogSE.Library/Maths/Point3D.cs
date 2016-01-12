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
#endregion

namespace DogSE.Library.Maths
{
    /// <summary>
    /// 
    /// </summary>
    public struct Point3D : IPoint3D, IComparable, IComparable<Point3D>, IEquatable<Point3D>
    {
        #region zh-CHS 类常量 | en Class Constants
        /// <summary>
        /// 
        /// </summary>
        public static readonly Point3D Zero = new Point3D( 0, 0, 0 );
        #endregion

        #region zh-CHS 构造和初始化和清理 | en Constructors and Initializers and Dispose
        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        public Point3D( float x, float y, float z )
        {
            m_X = x;
            m_Y = y;
            m_Z = z;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="point3D"></param>
        public Point3D( IPoint3D point3D )
            : this( point3D.X, point3D.Y, point3D.Z )
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="point2D"></param>
        /// <param name="z"></param>
        public Point3D( IPoint2D point2D, float z )
            : this( point2D.X, point2D.Y, z )
        {
        }
        #endregion

        #region zh-CHS 属性 | en Properties
        #region zh-CHS 私有成员变量 | en Private Member Variables
        /// <summary>
        /// 
        /// </summary>
        private float m_X;
        #endregion
        /// <summary>
        /// 
        /// </summary>
        public float X
        {
            get { return m_X; }
            set { m_X = value; }
        }

        #region zh-CHS 私有成员变量 | en Private Member Variables
        /// <summary>
        /// 
        /// </summary>
        private float m_Y;
        #endregion
        /// <summary>
        /// 
        /// </summary>
        public float Y
        {
            get { return m_Y; }
            set { m_Y = value; }
        }

        #region zh-CHS 私有成员变量 | en Private Member Variables
        /// <summary>
        /// 
        /// </summary>
        private float m_Z;
        #endregion
        /// <summary>
        /// 
        /// </summary>
        public float Z
        {
            get { return m_Z; }
            set { m_Z = value; }
        }
        #endregion

        #region zh-CHS 方法 | en Method
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Point3D Parse( string value )
        {
            int iStart = value.IndexOf( '(' );
            int iEnd = value.IndexOf( ',', iStart + 1 );

            string strParam1 = value.Substring( iStart + 1, iEnd - ( iStart + 1 ) ).Trim();

            iStart = iEnd;
            iEnd = value.IndexOf( ',', iStart + 1 );

            string strParam2 = value.Substring( iStart + 1, iEnd - ( iStart + 1 ) ).Trim();

            iStart = iEnd;
            iEnd = value.IndexOf( ')', iStart + 1 );

            string strParam3 = value.Substring( iStart + 1, iEnd - ( iStart + 1 ) ).Trim();

            return new Point3D( Convert.ToSingle( strParam1 ), Convert.ToSingle( strParam2 ), Convert.ToSingle( strParam3 ) );
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="xCompare"></param>
        /// <param name="yCompare"></param>
        /// <returns></returns>
        public static bool operator ==( Point3D xCompare, Point3D yCompare )
        {
            return xCompare.m_X == yCompare.m_X && xCompare.m_Y == yCompare.m_Y && xCompare.m_Z == yCompare.m_Z;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="xCompare"></param>
        /// <param name="yCompare"></param>
        /// <returns></returns>
        public static bool operator ==( Point3D xCompare, IPoint3D yCompare )
        {
            if ( object.ReferenceEquals( yCompare, null ) )
                return false;

            return xCompare.m_X == yCompare.X && xCompare.m_Y == yCompare.Y && xCompare.m_Z == yCompare.Z;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="xCompare"></param>
        /// <param name="yCompare"></param>
        /// <returns></returns>
        public static bool operator !=( Point3D xCompare, Point3D yCompare )
        {
            return xCompare.m_X != yCompare.m_X || xCompare.m_Y != yCompare.m_Y || xCompare.m_Z != yCompare.m_Z;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="xCompare"></param>
        /// <param name="yCompare"></param>
        /// <returns></returns>
        public static bool operator !=( Point3D xCompare, IPoint3D yCompare )
        {
            if ( object.ReferenceEquals( yCompare, null ) )
                return false;

            return xCompare.m_X != yCompare.X || xCompare.m_Y != yCompare.Y || xCompare.m_Z != yCompare.Z;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="point3D"></param>
        /// <returns></returns>
        public static explicit operator Point2D( Point3D point3D )
        {
            return new Point2D( point3D );
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="xObject"></param>
        /// <returns></returns>
        public override bool Equals( object xObject )
        {
            if ( xObject == null )
                return false;

            IPoint3D point3D = xObject as IPoint3D;
            if ( point3D == null )
                return false;

            return m_X == point3D.X && m_Y == point3D.Y && m_Z == point3D.Z;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return (int)m_X ^ (int)m_Y ^ (int)m_Z;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return String.Format( "({0}, {1}, {2})", m_X, m_Y, m_Z );
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="other"></param>
        public bool Equals( Point3D other )
        {
            return ( ( m_X == other.m_X ) && ( m_Y == other.m_Y ) && ( m_Z == other.m_Z ) );
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public int CompareTo( Point3D other )
        {
            int iCompare = ( m_X.CompareTo( other.m_X ) );

            if ( iCompare == 0 )
            {
                iCompare = ( m_Y.CompareTo( other.m_Y ) );

                if ( iCompare == 0 )
                    iCompare = ( m_Z.CompareTo( other.m_Z ) );
            }

            return iCompare;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public int CompareTo( object other )
        {
            if ( other == null )
                return 1;

            if ( other is Point3D == false )
                return 1;

            return CompareTo( (Point3D)other );
        }
        #endregion
    }
}
#endregion

