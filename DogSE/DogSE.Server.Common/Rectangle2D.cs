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
    public struct Rectangle2D
    {
        #region zh-CHS 共有常量 | en Public Constants
        /// <summary>
        /// 
        /// </summary>
        public static readonly Rectangle2D ZeroRectangle2D = new Rectangle2D( 0, 0, 0, 0 );
        #endregion

        #region zh-CHS 私有成员变量 | en Private Member Variables
        /// <summary>
        /// 
        /// </summary>
        private Point2D m_Start;
        /// <summary>
        /// 
        /// </summary>
        private Point2D m_End;
        #endregion

        #region zh-CHS 构造和初始化和清理 | en Constructors and Initializers and Dispose
        /// <summary>
        /// 
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        public Rectangle2D( IPoint2D start, IPoint2D end )
        {
            m_Start = new Point2D( start );
            m_End = new Point2D( end );
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public Rectangle2D( float x, float y, float width, float height )
        {
            m_Start = new Point2D( x, y );
            m_End = new Point2D( x + width, y + height );
        }
        #endregion

        #region zh-CHS 属性 | en Properties
        /// <summary>
        /// 
        /// </summary>
        public Point2D Start
        {
            get { return m_Start; }
            set { m_Start = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public Point2D End
        {
            get { return m_End; }
            set { m_End = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public float X
        {
            get { return m_Start.X; }
            set { m_Start.X = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public float Y
        {
            get { return m_Start.Y; }
            set { m_Start.Y = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public float Width
        {
            get { return m_End.X - m_Start.X; }
            set { m_End.X = m_Start.X + value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public float Height
        {
            get { return m_End.Y - m_Start.Y; }
            set { m_End.Y = m_Start.Y + value; }
        }
        #endregion

        #region zh-CHS 方法 | en Method
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Rectangle2D Parse( string value )
        {
            int iStart = value.IndexOf( '(' );
            int iEnd = value.IndexOf( ',', iStart + 1 );

            string strParam1 = value.Substring( iStart + 1, iEnd - ( iStart + 1 ) ).Trim();

            iStart = iEnd;
            iEnd = value.IndexOf( ',', iStart + 1 );

            string strParam2 = value.Substring( iStart + 1, iEnd - ( iStart + 1 ) ).Trim();

            iStart = value.IndexOf( '(' , iEnd );
            iEnd = value.IndexOf( ',', iStart + 1 );

            string strParam3 = value.Substring( iStart + 1, iEnd - ( iStart + 1 ) ).Trim();

            iStart = iEnd;
            iEnd = value.IndexOf( ')', iStart + 1 );

            string strParam4 = value.Substring( iStart + 1, iEnd - ( iStart + 1 ) ).Trim();

            return new Rectangle2D( Convert.ToSingle( strParam1 ), Convert.ToSingle( strParam2 ), Convert.ToSingle( strParam3 ), Convert.ToSingle( strParam4 ) );
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public void SetRectangle( float x, float y, float width, float height )
        {
            m_Start = new Point2D( x, y );
            m_End = new Point2D( x + width, y + height );
        }

        /// <summary>
        /// 与rectangle2D相交
        /// </summary>
        /// <param name="rectangle2D"></param>
        public void MakeHold( Rectangle2D rectangle2D )
        {
            if ( rectangle2D.m_Start.X < m_Start.X )
                m_Start.X = rectangle2D.m_Start.X;

            if ( rectangle2D.m_Start.Y < m_Start.Y )
                m_Start.Y = rectangle2D.m_Start.Y;

            if ( rectangle2D.m_End.X > m_End.X )
                m_End.X = rectangle2D.m_End.X;

            if ( rectangle2D.m_End.Y > m_End.Y )
                m_End.Y = rectangle2D.m_End.Y;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="point2D"></param>
        /// <returns></returns>
        public bool Contains( Point2D point2D )
        {
            return ( m_Start.X <= point2D.X && m_Start.Y <= point2D.Y && m_End.X > point2D.X && m_End.Y > point2D.Y );
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="point2D"></param>
        /// <returns></returns>
        public bool Contains( IPoint2D point2D )
        {
            return ( m_Start <= point2D && m_End > point2D );
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return String.Format( "({0}, {1})+({2}, {3})", X, Y, Width, Height );
        }
        #endregion
    }
}
#endregion

