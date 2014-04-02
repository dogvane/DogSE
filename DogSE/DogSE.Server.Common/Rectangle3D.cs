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
    public struct Rectangle3D
    {
        #region zh-CHS 私有成员变量 | en Private Member Variables
        /// <summary>
        /// 
        /// </summary>
        private Point3D m_Start;
        /// <summary>
        /// 
        /// </summary>
        private Point3D m_End;
        #endregion

        #region zh-CHS 构造和初始化和清理 | en Constructors and Initializers and Dispose
        /// <summary>
        /// 
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        public Rectangle3D( Point3D start, Point3D end )
        {
            m_Start = start;
            m_End = end;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="depth"></param>
        public Rectangle3D( float x, float y, float z, float width, float height, float depth )
        {
            m_Start = new Point3D( x, y, z );
            m_End = new Point3D( x + width, y + height, z + depth );
        }
        #endregion

        #region zh-CHS 属性 | en Properties
        /// <summary>
        /// 
        /// </summary>
        public Point3D Start
        {
            get { return m_Start; }
            set { m_Start = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public Point3D End
        {
            get { return m_End; }
            set { m_End = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public float Width
        {
            get { return m_End.X - m_Start.X; }
        }

        /// <summary>
        /// 
        /// </summary>
        public float Height
        {
            get { return m_End.Y - m_Start.Y; }
        }

        /// <summary>
        /// 
        /// </summary>
        public float Depth
        {
            get { return m_End.Z - m_Start.Z; }
        }
        #endregion

        #region zh-CHS 方法 | en Method
        /// <summary>
        /// 没测试过
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Rectangle3D Parse( string value )
        {
            int iStart = value.IndexOf( '(' );
            int iEnd = value.IndexOf( ',', iStart + 1 );

            string strParam1 = value.Substring( iStart + 1, iEnd - ( iStart + 1 ) ).Trim();

            iStart = iEnd;
            iEnd = value.IndexOf( ',', iStart + 1 );

            string strParam2 = value.Substring( iStart + 1, iEnd - ( iStart + 1 ) ).Trim();

            iStart = iEnd;
            iEnd = value.IndexOf( ',', iStart + 1 );

            string strParam3 = value.Substring( iStart + 1, iEnd - ( iStart + 1 ) ).Trim();

            iStart = value.IndexOf( '(', iEnd );
            iEnd = value.IndexOf( ',', iStart + 1 );

            string strParam4 = value.Substring( iStart + 1, iEnd - ( iStart + 1 ) ).Trim();

            iStart = iEnd;
            iEnd = value.IndexOf( ',', iStart + 1 );

            string strParam5 = value.Substring( iStart + 1, iEnd - ( iStart + 1 ) ).Trim();

            iStart = iEnd;
            iEnd = value.IndexOf( ')', iStart + 1 );

            string strParam6 = value.Substring( iStart + 1, iEnd - ( iStart + 1 ) ).Trim();

            return new Rectangle3D( Convert.ToSingle( strParam1 ), Convert.ToSingle( strParam2 ), Convert.ToSingle( strParam3 ), Convert.ToSingle( strParam4 ), Convert.ToSingle( strParam5 ), Convert.ToSingle( strParam6 ) );
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="point3D"></param>
        /// <returns></returns>
        public bool Contains( Point3D point3D )
        {
            return ( point3D.X >= m_Start.X )   &&
                ( point3D.X < m_End.X )         &&
                ( point3D.Y >= m_Start.Y )      &&
                ( point3D.Y < m_End.Y )         &&
                ( point3D.Z >= m_Start.Z )      &&
                ( point3D.Z < m_End.Z );
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="point3D"></param>
        /// <returns></returns>
        public bool Contains( IPoint3D point3D )
        {
            return ( point3D.X >= m_Start.X ) &&
                ( point3D.X < m_End.X )       &&
                ( point3D.Y >= m_Start.Y )    &&
                ( point3D.Y < m_End.Y )       &&
                ( point3D.Z >= m_Start.Z )    &&
                ( point3D.Z < m_End.Z );
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return String.Format( "({0}, {1}, {2})+({3}, {4}, {5})", Start.X, Start.Y, Start.Z, Width, Height, Depth );
        }
        #endregion
    }
}
#endregion

