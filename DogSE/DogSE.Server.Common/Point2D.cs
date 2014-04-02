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

namespace DogSE.Common
{
    /// <summary>
    /// 
    /// </summary>
    public struct Point2D : IPoint2D, IComparable, IComparable<Point2D>, IEquatable<Point2D>
    {
        #region zh-CHS 类常量 | en Class Constants
        /// <summary>
        /// 
        /// </summary>
        public static readonly Point2D Zero = new Point2D( 0, 0 );
        #endregion

        #region zh-CHS 构造和初始化和清理 | en Constructors and Initializers and Dispose
        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public Point2D( float x, float y )
        {
            m_X = x;
            m_Y = y;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="point2D"></param>
        public Point2D( IPoint2D point2D )
            : this( point2D.X, point2D.Y )
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
        #endregion

        #region zh-CHS 方法 | en Method
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Point2D Parse( string value )
        {
            int iStart = value.IndexOf( '(' );
            int iEnd = value.IndexOf( ',', iStart + 1 );

            string strParam1 = value.Substring( iStart + 1, iEnd - ( iStart + 1 ) ).Trim();

            iStart = iEnd;
            iEnd = value.IndexOf( ')', iStart + 1 );

            string strParam2 = value.Substring( iStart + 1, iEnd - ( iStart + 1 ) ).Trim();

            return new Point2D( Convert.ToSingle( strParam1 ), Convert.ToSingle( strParam2 ) );
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="xCompare"></param>
        /// <param name="yCompare"></param>
        /// <returns></returns>
        public static bool operator ==( Point2D xCompare, Point2D yCompare )
        {
            return xCompare.m_X == yCompare.X && xCompare.Y == yCompare.Y;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="xCompare"></param>
        /// <param name="yCompare"></param>
        /// <returns></returns>
        public static bool operator !=( Point2D xCompare, Point2D yCompare )
        {
            return xCompare.m_X != yCompare.X || xCompare.Y != yCompare.Y;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="xCompare"></param>
        /// <param name="yCompare"></param>
        /// <returns></returns>
        public static bool operator ==( Point2D xCompare, IPoint2D yCompare )
        {
            if ( object.ReferenceEquals( yCompare, null ) )
                return false;

            return xCompare.m_X == yCompare.X && xCompare.Y == yCompare.Y;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="xCompare"></param>
        /// <param name="yCompare"></param>
        /// <returns></returns>
        public static bool operator !=( Point2D xCompare, IPoint2D yCompare )
        {
            if ( object.ReferenceEquals( yCompare, null ) )
                return false;

            return xCompare.m_X != yCompare.X || xCompare.Y != yCompare.Y;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="xCompare"></param>
        /// <param name="yCompare"></param>
        /// <returns></returns>
        public static bool operator >( Point2D xCompare, Point2D yCompare )
        {
            return xCompare.m_X > yCompare.X && xCompare.Y > yCompare.Y;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="xCompare"></param>
        /// <param name="yCompare"></param>
        /// <returns></returns>
        public static bool operator >( Point2D xCompare, IPoint2D yCompare )
        {
            if ( object.ReferenceEquals( yCompare, null ) )
                return false;

            return xCompare.m_X > yCompare.X && xCompare.Y > yCompare.Y;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="xCompare"></param>
        /// <param name="yCompare"></param>
        /// <returns></returns>
        public static bool operator <( Point2D xCompare, Point2D yCompare )
        {
            return xCompare.m_X < yCompare.X && xCompare.Y < yCompare.Y;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="xCompare"></param>
        /// <param name="yCompare"></param>
        /// <returns></returns>
        public static bool operator <( Point2D xCompare, IPoint2D yCompare )
        {
            if ( object.ReferenceEquals( yCompare, null ) )
                return false;

            return xCompare.m_X < yCompare.X && xCompare.Y < yCompare.Y;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="xCompare"></param>
        /// <param name="yCompare"></param>
        /// <returns></returns>
        public static bool operator >=( Point2D xCompare, Point2D yCompare )
        {
            return xCompare.m_X >= yCompare.X && xCompare.Y >= yCompare.Y;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="xCompare"></param>
        /// <param name="yCompare"></param>
        /// <returns></returns>
        public static bool operator >=( Point2D xCompare, IPoint2D yCompare )
        {
            if ( object.ReferenceEquals( yCompare, null ) )
                return false;

            return xCompare.m_X >= yCompare.X && xCompare.Y >= yCompare.Y;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="xCompare"></param>
        /// <param name="yCompare"></param>
        /// <returns></returns>
        public static bool operator <=( Point2D xCompare, Point2D yCompare )
        {
            return xCompare.m_X <= yCompare.X && xCompare.Y <= yCompare.Y;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="xCompare"></param>
        /// <param name="yCompare"></param>
        /// <returns></returns>
        public static bool operator <=( Point2D xCompare, IPoint2D yCompare )
        {
            if ( object.ReferenceEquals( yCompare, null ) )
                return false;

            return xCompare.m_X <= yCompare.X && xCompare.Y <= yCompare.Y;
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

            IPoint2D point2D = xObject as IPoint2D;
            if ( point2D == null )
                return false;

            return m_X == point2D.X && m_Y == point2D.Y;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return (int)m_X ^ (int)m_Y;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return String.Format( "({0}, {1})", m_X, m_Y );
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="other"></param>
        public bool Equals( Point2D other )
        {
            return ( ( m_X == other.m_X ) && ( m_Y == other.m_Y ) );
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public int CompareTo( Point2D other )
        {
            int iCompare = ( m_X.CompareTo( other.X ) );

            if ( iCompare == 0 )
                iCompare = ( m_Y.CompareTo( other.Y ) );

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

            if ( other is Point2D == false )
                return 1;

            return CompareTo( (Point2D)other );
        }
        #endregion
    }
}
#endregion

