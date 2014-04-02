using System.Runtime.InteropServices;

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
using System.Globalization;
#endregion



namespace DogSE.Common
{
    /// <summary>
    /// Defines a vector with three components.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct Vector3 : IEquatable<Vector3>
    {

        #region zh-CHS 共有成员变量 | en Public Member Variables

        /// <summary>
        /// Gets or sets the x-component of the vector.
        /// </summary>
        public float X;
        /// <summary>
        /// Gets or sets the y-component of the vector.
        /// </summary>
        public float Y;
        /// <summary>
        /// Gets or sets the z-component of the vector.
        /// </summary>
        public float Z;

        #endregion

        #region zh-CHS 构造和初始化和清理 | en Constructors and Initializers and Dispose

        /// <summary>
        /// Initializes a new instance of Vector3.
        /// </summary>
        /// <param name="x">Initial value for the x-component of the vector.</param>
        /// <param name="y">Initial value for the y-component of the vector.</param>
        /// <param name="z">Initial value for the z-component of the vector.</param>
        public Vector3( float x, float y, float z )
        {
            X = x;
            Y = y;
            Z = z;
        }

        /// <summary>
        /// Creates a new instance of Vector3.
        /// </summary>
        /// <param name="value">Value to initialize each component to.</param>
        public Vector3( float value )
        {
            X = Y = Z = value;
        }

        /// <summary>
        /// Initializes a new instance of Vector3.
        /// </summary>
        /// <param name="value">A vector containing the values to initialize x and y components with.</param>
        /// <param name="z">Initial value for the z-component of the vector.</param>
        public Vector3( Vector2 value, float z )
        {
            X = value.X;
            Y = value.Y;
            Z = z;
        }

        #endregion

        #region zh-CHS 共有静态属性 | en Public Static Properties

        #region zh-CHS 私有静态成员变量 | en Private Static Member Variables
        /// <summary>
        /// 
        /// </summary>
        private static Vector3 m_Zero = new Vector3();
        #endregion
        /// <summary>
        /// Returns a Vector3 with all of its components set to zero.
        /// </summary>
        public static Vector3 Zero
        {
            get { return m_Zero; }
        }

        #region zh-CHS 私有静态成员变量 | en Private Static Member Variables
        /// <summary>
        /// 
        /// </summary>
        private static Vector3 m_One = new Vector3( 1f, 1f, 1f );
        #endregion
        /// <summary>
        /// Returns a Vector3 with ones in all of its components.
        /// </summary>
        public static Vector3 One
        {
            get { return m_One; }
        }

        #region zh-CHS 私有静态成员变量 | en Private Static Member Variables
        /// <summary>
        /// 
        /// </summary>
        private static Vector3 m_UnitX = new Vector3( 1f, 0f, 0f );
        #endregion
        /// <summary>
        /// Returns the x unit Vector3 (1, 0, 0).
        /// </summary>
        public static Vector3 UnitX
        {
            get { return m_UnitX; }
        }

        #region zh-CHS 私有静态成员变量 | en Private Static Member Variables
        /// <summary>
        /// 
        /// </summary>
        private static Vector3 m_UnitY = new Vector3( 0f, 1f, 0f );
        #endregion
        /// <summary>
        /// Returns the y unit Vector3 (0, 1, 0).
        /// </summary>
        public static Vector3 UnitY
        {
            get { return m_UnitY; }
        }

        #region zh-CHS 私有静态成员变量 | en Private Static Member Variables
        /// <summary>
        /// 
        /// </summary>
        private static Vector3 m_UnitZ = new Vector3( 0f, 0f, 1f );
        #endregion
        /// <summary>
        /// Returns the z unit Vector3 (0, 0, 1).
        /// </summary>
        public static Vector3 UnitZ
        {
            get { return m_UnitZ; }
        }

        #region zh-CHS 私有静态成员变量 | en Private Static Member Variables
        /// <summary>
        /// 
        /// </summary>
        private static Vector3 m_Up = new Vector3( 0f, 1f, 0f );
        #endregion
        /// <summary>
        /// Returns a unit vector designating up (0, 1, 0).
        /// </summary>
        public static Vector3 Up
        {
            get { return m_Up; }
        }

        #region zh-CHS 私有静态成员变量 | en Private Static Member Variables
        /// <summary>
        /// 
        /// </summary>
        private static Vector3 m_Down = new Vector3( 0f, -1f, 0f );
        #endregion
        /// <summary>
        /// Returns a unit Vector3 designating down (0, −1, 0).
        /// </summary>
        public static Vector3 Down
        {
            get { return m_Down; }
        }

        #region zh-CHS 私有静态成员变量 | en Private Static Member Variables
        /// <summary>
        /// 
        /// </summary>
        private static Vector3 m_Right = new Vector3( 1f, 0f, 0f );
        #endregion
        /// <summary>
        /// Returns a unit Vector3 pointing to the right (1, 0, 0).
        /// </summary>
        public static Vector3 Right
        {
            get { return m_Right; }
        }

        #region zh-CHS 私有静态成员变量 | en Private Static Member Variables
        /// <summary>
        /// 
        /// </summary>
        private static Vector3 m_Left = new Vector3( -1f, 0f, 0f );
        #endregion
        /// <summary>
        /// Returns a unit Vector3 designating left (−1, 0, 0).
        /// </summary>
        public static Vector3 Left
        {
            get { return m_Left; }
        }

        #region zh-CHS 私有静态成员变量 | en Private Static Member Variables
        /// <summary>
        /// 
        /// </summary>
        private static Vector3 m_Forward = new Vector3( 0f, 0f, -1f );
        #endregion
        /// <summary>
        /// Returns a unit Vector3 designating forward in a right-handed coordinate system(0, 0, −1).
        /// </summary>
        public static Vector3 Forward
        {
            get { return m_Forward; }
        }

        #region zh-CHS 私有静态成员变量 | en Private Static Member Variables
        /// <summary>
        /// 
        /// </summary>
        private static Vector3 m_Backward = new Vector3( 0f, 0f, 1f );
        #endregion
        /// <summary>
        /// Returns a unit Vector3 designating backward in a right-handed coordinate system (0, 0, 1).
        /// </summary>
        public static Vector3 Backward
        {
            get { return m_Backward; }
        }

        #endregion

        #region zh-CHS 共有方法 | en Public Methods

        /// <summary>
        /// Calculates the length of the vector.
        /// </summary>
        public float Length()
        {
            return (float)Math.Sqrt( (double)( ( X * X ) + ( Y * Y ) ) + ( Z * Z ) );
        }

        /// <summary>
        /// Calculates the length of the vector squared.
        /// </summary>
        public float LengthSquared()
        {
            return ( ( ( X * X ) + ( Y * Y ) ) + ( Z * Z ) );
        }

        /// <summary>
        /// Turns the current vector into a unit vector. The result is a vector one unit in length pointing in the same direction as the original vector.
        /// </summary>
        public void Normalize()
        {
            float num = 1f / ( (float)Math.Sqrt( (double)( ( X * X ) + ( Y * Y ) ) + ( Z * Z ) ) );

            X *= num;
            Y *= num;
            Z *= num;
        }

        #endregion

        #region zh-CHS 共有静态方法 | en Public Static Methods

        /// <summary>
        /// Calculates the distance between two vectors.
        /// </summary>
        /// <param name="value1">Source vector.</param>
        /// <param name="value2">Source vector.</param>
        public static float Distance( Vector3 value1, Vector3 value2 )
        {
            float numX = value1.X - value2.X;
            float numY = value1.Y - value2.Y;
            float numZ = value1.Z - value2.Z;

            return (float)Math.Sqrt( (double)( ( numX * numX ) + ( numY * numY ) ) + ( numZ * numZ ) );
        }

        /// <summary>
        /// Calculates the distance between two vectors.
        /// </summary>
        /// <param name="value1">Source vector.</param>
        /// <param name="value2">Source vector.</param>
        /// <param name="result">[OutAttribute] The distance between the vectors.</param>
        public static void Distance( ref Vector3 value1, ref Vector3 value2, out float result )
        {
            float numX = value1.X - value2.X;
            float numY = value1.Y - value2.Y;
            float numZ = value1.Z - value2.Z;

            result = (float)Math.Sqrt( (double)( ( numX * numX ) + ( numY * numY ) ) + ( numZ * numZ ) );
        }

        /// <summary>
        /// Calculates the distance between two vectors squared.
        /// </summary>
        /// <param name="value1">Source vector.</param>
        /// <param name="value2">Source vector.</param>
        public static float DistanceSquared( Vector3 value1, Vector3 value2 )
        {
            float numX = value1.X - value2.X;
            float numY = value1.Y - value2.Y;
            float numZ = value1.Z - value2.Z;

            return ( ( ( numX * numX ) + ( numY * numY ) ) + ( numZ * numZ ) );
        }

        /// <summary>
        /// Calculates the distance between two vectors squared.
        /// </summary>
        /// <param name="value1">Source vector.</param>
        /// <param name="value2">Source vector.</param>
        /// <param name="result">[OutAttribute] The distance between the two vectors squared.</param>
        public static void DistanceSquared( ref Vector3 value1, ref Vector3 value2, out float result )
        {
            float numX = value1.X - value2.X;
            float numY = value1.Y - value2.Y;
            float numZ = value1.Z - value2.Z;

            result = ( ( numX * numX ) + ( numY * numY ) ) + ( numZ * numZ );
        }

        /// <summary>
        /// Calculates the dot product of two vectors. If the two vectors are unit vectors, the dot product returns a floating point value between -1 and 1 that can be used to determine some properties of the angle between two vectors. For example, it can show whether the vectors are orthogonal, parallel, or have an acute or obtuse angle between them.
        /// </summary>
        /// <param name="vector1">Source vector.</param>
        /// <param name="vector2">Source vector.</param>
        public static float Dot( Vector3 vector1, Vector3 vector2 )
        {
            return ( ( ( vector1.X * vector2.X ) + ( vector1.Y * vector2.Y ) ) + ( vector1.Z * vector2.Z ) );
        }

        /// <summary>
        /// Calculates the dot product of two vectors and writes the result to a user-specified variable. If the two vectors are unit vectors, the dot product returns a floating point value between -1 and 1 that can be used to determine some properties of the angle between two vectors. For example, it can show whether the vectors are orthogonal, parallel, or have an acute or obtuse angle between them.
        /// </summary>
        /// <param name="vector1">Source vector.</param>
        /// <param name="vector2">Source vector.</param>
        /// <param name="result">[OutAttribute] The dot product of the two vectors.</param>
        public static void Dot( ref Vector3 vector1, ref Vector3 vector2, out float result )
        {
            result = ( ( vector1.X * vector2.X ) + ( vector1.Y * vector2.Y ) ) + ( vector1.Z * vector2.Z );
        }

        /// <summary>
        /// Creates a unit vector from the specified vector. The result is a vector one unit in length pointing in the same direction as the original vector.
        /// </summary>
        /// <param name="value">The source Vector3.</param>
        public static Vector3 Normalize( Vector3 value )
        {
            float num = 1f / ( (float)Math.Sqrt( (double)( ( value.X * value.X ) + ( value.Y * value.Y ) ) + ( value.Z * value.Z ) ) );

            return new Vector3 { X = value.X * num, Y = value.Y * num, Z = value.Z * num };
        }

        /// <summary>
        /// Creates a unit vector from the specified vector, writing the result to a user-specified variable. The result is a vector one unit in length pointing in the same direction as the original vector.
        /// </summary>
        /// <param name="value">Source vector.</param>
        /// <param name="result">[OutAttribute] The normalized vector.</param>
        public static void Normalize( ref Vector3 value, out Vector3 result )
        {
            float num = 1f / ( (float)Math.Sqrt( (double)( ( value.X * value.X ) + ( value.Y * value.Y ) ) + ( value.Z * value.Z ) ) );

            result.X = value.X * num;
            result.Y = value.Y * num;
            result.Z = value.Z * num;
        }

        /// <summary>
        /// Calculates the cross product of two vectors.
        /// </summary>
        /// <param name="vector1">Source vector.</param>
        /// <param name="vector2">Source vector.</param>
        public static Vector3 Cross( Vector3 vector1, Vector3 vector2 )
        {
            return new Vector3 { X = ( vector1.Y * vector2.Z ) - ( vector1.Z * vector2.Y ), Y = ( vector1.Z * vector2.X ) - ( vector1.X * vector2.Z ), Z = ( vector1.X * vector2.Y ) - ( vector1.Y * vector2.X ) };
        }

        /// <summary>
        /// Calculates the cross product of two vectors.
        /// </summary>
        /// <param name="vector1">Source vector.</param>
        /// <param name="vector2">Source vector.</param>
        /// <param name="result">[OutAttribute] The cross product of the vectors.</param>
        public static void Cross( ref Vector3 vector1, ref Vector3 vector2, out Vector3 result )
        {
            result.X = ( vector1.Y * vector2.Z ) - ( vector1.Z * vector2.Y );
            result.Y = ( vector1.Z * vector2.X ) - ( vector1.X * vector2.Z );
            result.Z = ( vector1.X * vector2.Y ) - ( vector1.Y * vector2.X );
        }

        /// <summary>
        /// Returns the reflection of a vector off a surface that has the specified normal.  Reference page contains code sample.
        /// </summary>
        /// <param name="vector">Source vector.</param>
        /// <param name="normal">Normal of the surface.</param>
        public static Vector3 Reflect( Vector3 vector, Vector3 normal )
        {
            float num = ( ( vector.X * normal.X ) + ( vector.Y * normal.Y ) ) + ( vector.Z * normal.Z );

            return new Vector3 { X = vector.X - ( ( 2f * num ) * normal.X ), Y = vector.Y - ( ( 2f * num ) * normal.Y ), Z = vector.Z - ( ( 2f * num ) * normal.Z ) };
        }

        /// <summary>
        /// Returns the reflection of a vector off a surface that has the specified normal.  Reference page contains code sample.
        /// </summary>
        /// <param name="vector">Source vector.</param>
        /// <param name="normal">Normal of the surface.</param>
        /// <param name="result">[OutAttribute] The reflected vector.</param>
        public static void Reflect( ref Vector3 vector, ref Vector3 normal, out Vector3 result )
        {
            float num = ( ( vector.X * normal.X ) + ( vector.Y * normal.Y ) ) + ( vector.Z * normal.Z );

            result.X = vector.X - ( ( 2f * num ) * normal.X );
            result.Y = vector.Y - ( ( 2f * num ) * normal.Y );
            result.Z = vector.Z - ( ( 2f * num ) * normal.Z );
        }

        /// <summary>
        /// Returns a vector that contains the lowest value from each matching pair of components.
        /// </summary>
        /// <param name="value1">Source vector.</param>
        /// <param name="value2">Source vector.</param>
        public static Vector3 Min( Vector3 value1, Vector3 value2 )
        {
            return new Vector3 { X = ( value1.X < value2.X ) ? value1.X : value2.X, Y = ( value1.Y < value2.Y ) ? value1.Y : value2.Y, Z = ( value1.Z < value2.Z ) ? value1.Z : value2.Z };
        }

        /// <summary>
        /// Returns a vector that contains the lowest value from each matching pair of components.
        /// </summary>
        /// <param name="value1">Source vector.</param>
        /// <param name="value2">Source vector.</param>
        /// <param name="result">[OutAttribute] The minimized vector.</param>
        public static void Min( ref Vector3 value1, ref Vector3 value2, out Vector3 result )
        {
            result.X = ( value1.X < value2.X ) ? value1.X : value2.X;
            result.Y = ( value1.Y < value2.Y ) ? value1.Y : value2.Y;
            result.Z = ( value1.Z < value2.Z ) ? value1.Z : value2.Z;
        }

        /// <summary>
        /// Returns a vector that contains the highest value from each matching pair of components.
        /// </summary>
        /// <param name="value1">Source vector.</param>
        /// <param name="value2">Source vector.</param>
        public static Vector3 Max( Vector3 value1, Vector3 value2 )
        {
            return new Vector3 { X = ( value1.X > value2.X ) ? value1.X : value2.X, Y = ( value1.Y > value2.Y ) ? value1.Y : value2.Y, Z = ( value1.Z > value2.Z ) ? value1.Z : value2.Z };
        }

        /// <summary>
        /// Returns a vector that contains the highest value from each matching pair of components.
        /// </summary>
        /// <param name="value1">Source vector.</param>
        /// <param name="value2">Source vector.</param>
        /// <param name="result">[OutAttribute] The maximized vector.</param>
        public static void Max( ref Vector3 value1, ref Vector3 value2, out Vector3 result )
        {
            result.X = ( value1.X > value2.X ) ? value1.X : value2.X;
            result.Y = ( value1.Y > value2.Y ) ? value1.Y : value2.Y;
            result.Z = ( value1.Z > value2.Z ) ? value1.Z : value2.Z;
        }

        /// <summary>
        /// Restricts a value to be within a specified range.
        /// </summary>
        /// <param name="value1">The value to clamp.</param>
        /// <param name="min">The minimum value.</param>
        /// <param name="max">The maximum value.</param>
        public static Vector3 Clamp( Vector3 value1, Vector3 min, Vector3 max )
        {
            float x = value1.X;
            x = ( x > max.X ) ? max.X : x;
            x = ( x < min.X ) ? min.X : x;

            float y = value1.Y;
            y = ( y > max.Y ) ? max.Y : y;
            y = ( y < min.Y ) ? min.Y : y;

            float z = value1.Z;
            z = ( z > max.Z ) ? max.Z : z;
            z = ( z < min.Z ) ? min.Z : z;

            return new Vector3 { X = x, Y = y, Z = z };
        }

        /// <summary>
        /// Restricts a value to be within a specified range.
        /// </summary>
        /// <param name="value1">The value to clamp.</param>
        /// <param name="min">The minimum value.</param>
        /// <param name="max">The maximum value.</param>
        /// <param name="result">[OutAttribute] The clamped value.</param>
        public static void Clamp( ref Vector3 value1, ref Vector3 min, ref Vector3 max, out Vector3 result )
        {
            float x = value1.X;
            x = ( x > max.X ) ? max.X : x;
            x = ( x < min.X ) ? min.X : x;

            float y = value1.Y;
            y = ( y > max.Y ) ? max.Y : y;
            y = ( y < min.Y ) ? min.Y : y;

            float z = value1.Z;
            z = ( z > max.Z ) ? max.Z : z;
            z = ( z < min.Z ) ? min.Z : z;

            result.X = x;
            result.Y = y;
            result.Z = z;
        }

        /// <summary>
        /// Performs a linear interpolation between two vectors.
        /// </summary>
        /// <param name="value1">Source vector.</param>
        /// <param name="value2">Source vector.</param>
        /// <param name="amount">Value between 0 and 1 indicating the weight of value2.</param>
        public static Vector3 Lerp( Vector3 value1, Vector3 value2, float amount )
        {
            return new Vector3 { X = value1.X + ( ( value2.X - value1.X ) * amount ), Y = value1.Y + ( ( value2.Y - value1.Y ) * amount ), Z = value1.Z + ( ( value2.Z - value1.Z ) * amount ) };
        }

        /// <summary>
        /// Performs a linear interpolation between two vectors.
        /// </summary>
        /// <param name="value1">Source vector.</param>
        /// <param name="value2">Source vector.</param>
        /// <param name="amount">Value between 0 and 1 indicating the weight of value2.</param>
        /// <param name="result">[OutAttribute] The result of the interpolation.</param>
        public static void Lerp( ref Vector3 value1, ref Vector3 value2, float amount, out Vector3 result )
        {
            result.X = value1.X + ( ( value2.X - value1.X ) * amount );
            result.Y = value1.Y + ( ( value2.Y - value1.Y ) * amount );
            result.Z = value1.Z + ( ( value2.Z - value1.Z ) * amount );
        }

        /// <summary>
        /// Returns a Vector3 containing the 3D Cartesian coordinates of a point specified in Barycentric coordinates relative to a 3D triangle.
        /// </summary>
        /// <param name="value1">A Vector3 containing the 3D Cartesian coordinates of vertex 1 of the triangle.</param>
        /// <param name="value2">A Vector3 containing the 3D Cartesian coordinates of vertex 2 of the triangle.</param>
        /// <param name="value3">A Vector3 containing the 3D Cartesian coordinates of vertex 3 of the triangle.</param>
        /// <param name="amount1">Barycentric coordinate b2, which expresses the weighting factor toward vertex 2 (specified in value2).</param>
        /// <param name="amount2">Barycentric coordinate b3, which expresses the weighting factor toward vertex 3 (specified in value3).</param>
        public static Vector3 Barycentric( Vector3 value1, Vector3 value2, Vector3 value3, float amount1, float amount2 )
        {
            return new Vector3
            {
                X = ( value1.X + ( amount1 * ( value2.X - value1.X ) ) ) + ( amount2 * ( value3.X - value1.X ) ),
                Y = ( value1.Y + ( amount1 * ( value2.Y - value1.Y ) ) ) + ( amount2 * ( value3.Y - value1.Y ) ),
                Z = ( value1.Z + ( amount1 * ( value2.Z - value1.Z ) ) ) + ( amount2 * ( value3.Z - value1.Z ) )
            };
        }

        /// <summary>
        /// Returns a Vector3 containing the 3D Cartesian coordinates of a point specified in barycentric (areal) coordinates relative to a 3D triangle.
        /// </summary>
        /// <param name="value1">A Vector3 containing the 3D Cartesian coordinates of vertex 1 of the triangle.</param>
        /// <param name="value2">A Vector3 containing the 3D Cartesian coordinates of vertex 2 of the triangle.</param>
        /// <param name="value3">A Vector3 containing the 3D Cartesian coordinates of vertex 3 of the triangle.</param>
        /// <param name="amount1">Barycentric coordinate b2, which expresses the weighting factor toward vertex 2 (specified in value2).</param>
        /// <param name="amount2">Barycentric coordinate b3, which expresses the weighting factor toward vertex 3 (specified in value3).</param>
        /// <param name="result">[OutAttribute] The 3D Cartesian coordinates of the specified point are placed in this Vector3 on exit.</param>
        public static void Barycentric( ref Vector3 value1, ref Vector3 value2, ref Vector3 value3, float amount1, float amount2, out Vector3 result )
        {
            result.X = ( value1.X + ( amount1 * ( value2.X - value1.X ) ) ) + ( amount2 * ( value3.X - value1.X ) );
            result.Y = ( value1.Y + ( amount1 * ( value2.Y - value1.Y ) ) ) + ( amount2 * ( value3.Y - value1.Y ) );
            result.Z = ( value1.Z + ( amount1 * ( value2.Z - value1.Z ) ) ) + ( amount2 * ( value3.Z - value1.Z ) );
        }

        /// <summary>
        /// Interpolates between two values using a cubic equation.
        /// </summary>
        /// <param name="value1">Source value.</param>
        /// <param name="value2">Source value.</param>
        /// <param name="amount">Weighting value.</param>
        public static Vector3 SmoothStep( Vector3 value1, Vector3 value2, float amount )
        {
            amount = ( amount > 1f ) ? 1f : ( ( amount < 0f ) ? 0f : amount );
            amount = ( amount * amount ) * ( 3f - ( 2f * amount ) );

            return new Vector3 { X = value1.X + ( ( value2.X - value1.X ) * amount ), Y = value1.Y + ( ( value2.Y - value1.Y ) * amount ), Z = value1.Z + ( ( value2.Z - value1.Z ) * amount ) };
        }

        /// <summary>
        /// Interpolates between two values using a cubic equation.
        /// </summary>
        /// <param name="value1">Source vector.</param>
        /// <param name="value2">Source vector.</param>
        /// <param name="amount">Weighting value.</param>
        /// <param name="result">[OutAttribute] The interpolated value.</param>
        public static void SmoothStep( ref Vector3 value1, ref Vector3 value2, float amount, out Vector3 result )
        {
            amount = ( amount > 1f ) ? 1f : ( ( amount < 0f ) ? 0f : amount );
            amount = ( amount * amount ) * ( 3f - ( 2f * amount ) );

            result.X = value1.X + ( ( value2.X - value1.X ) * amount );
            result.Y = value1.Y + ( ( value2.Y - value1.Y ) * amount );
            result.Z = value1.Z + ( ( value2.Z - value1.Z ) * amount );
        }

        /// <summary>
        /// Performs a Catmull-Rom interpolation using the specified positions.
        /// </summary>
        /// <param name="value1">The first position in the interpolation.</param>
        /// <param name="value2">The second position in the interpolation.</param>
        /// <param name="value3">The third position in the interpolation.</param>
        /// <param name="value4">The fourth position in the interpolation.</param>
        /// <param name="amount">Weighting factor.</param>
        public static Vector3 CatmullRom( Vector3 value1, Vector3 value2, Vector3 value3, Vector3 value4, float amount )
        {
            float amountDouble = amount * amount;
            float amountTriple = amount * amountDouble;

            return new Vector3
            {
                X = 0.5f * ( ( ( ( 2f * value2.X ) + ( ( -value1.X + value3.X ) * amount ) ) + ( ( ( ( ( 2f * value1.X ) - ( 5f * value2.X ) ) + ( 4f * value3.X ) ) - value4.X ) * amountDouble ) ) + ( ( ( ( -value1.X + ( 3f * value2.X ) ) - ( 3f * value3.X ) ) + value4.X ) * amountTriple ) ),
                Y = 0.5f * ( ( ( ( 2f * value2.Y ) + ( ( -value1.Y + value3.Y ) * amount ) ) + ( ( ( ( ( 2f * value1.Y ) - ( 5f * value2.Y ) ) + ( 4f * value3.Y ) ) - value4.Y ) * amountDouble ) ) + ( ( ( ( -value1.Y + ( 3f * value2.Y ) ) - ( 3f * value3.Y ) ) + value4.Y ) * amountTriple ) ),
                Z = 0.5f * ( ( ( ( 2f * value2.Z ) + ( ( -value1.Z + value3.Z ) * amount ) ) + ( ( ( ( ( 2f * value1.Z ) - ( 5f * value2.Z ) ) + ( 4f * value3.Z ) ) - value4.Z ) * amountDouble ) ) + ( ( ( ( -value1.Z + ( 3f * value2.Z ) ) - ( 3f * value3.Z ) ) + value4.Z ) * amountTriple ) )
            };
        }

        /// <summary>
        /// Performs a Catmull-Rom interpolation using the specified positions.
        /// </summary>
        /// <param name="value1">The first position in the interpolation.</param>
        /// <param name="value2">The second position in the interpolation.</param>
        /// <param name="value3">The third position in the interpolation.</param>
        /// <param name="value4">The fourth position in the interpolation.</param>
        /// <param name="amount">Weighting factor.</param>
        /// <param name="result">[OutAttribute] A vector that is the result of the Catmull-Rom interpolation.</param>
        public static void CatmullRom( ref Vector3 value1, ref Vector3 value2, ref Vector3 value3, ref Vector3 value4, float amount, out Vector3 result )
        {
            float amountDouble = amount * amount;
            float amountTriple = amount * amountDouble;

            result.X = 0.5f * ( ( ( ( 2f * value2.X ) + ( ( -value1.X + value3.X ) * amount ) ) + ( ( ( ( ( 2f * value1.X ) - ( 5f * value2.X ) ) + ( 4f * value3.X ) ) - value4.X ) * amountDouble ) ) + ( ( ( ( -value1.X + ( 3f * value2.X ) ) - ( 3f * value3.X ) ) + value4.X ) * amountTriple ) );
            result.Y = 0.5f * ( ( ( ( 2f * value2.Y ) + ( ( -value1.Y + value3.Y ) * amount ) ) + ( ( ( ( ( 2f * value1.Y ) - ( 5f * value2.Y ) ) + ( 4f * value3.Y ) ) - value4.Y ) * amountDouble ) ) + ( ( ( ( -value1.Y + ( 3f * value2.Y ) ) - ( 3f * value3.Y ) ) + value4.Y ) * amountTriple ) );
            result.Z = 0.5f * ( ( ( ( 2f * value2.Z ) + ( ( -value1.Z + value3.Z ) * amount ) ) + ( ( ( ( ( 2f * value1.Z ) - ( 5f * value2.Z ) ) + ( 4f * value3.Z ) ) - value4.Z ) * amountDouble ) ) + ( ( ( ( -value1.Z + ( 3f * value2.Z ) ) - ( 3f * value3.Z ) ) + value4.Z ) * amountTriple ) );
        }

        /// <summary>
        /// Performs a Hermite spline interpolation.
        /// </summary>
        /// <param name="value1">Source position vector.</param>
        /// <param name="tangent1">Source tangent vector.</param>
        /// <param name="value2">Source position vector.</param>
        /// <param name="tangent2">Source tangent vector.</param>
        /// <param name="amount">Weighting factor.</param>
        public static Vector3 Hermite( Vector3 value1, Vector3 tangent1, Vector3 value2, Vector3 tangent2, float amount )
        {
            float amountDouble = amount * amount;
            float amountTriple = amount * amountDouble;

            float num1 = amountTriple - amountDouble;
            float num2 = ( amountTriple - ( 2f * amountDouble ) ) + amount;
            float num3 = ( -2f * amountTriple ) + ( 3f * amountDouble );
            float num4 = ( ( 2f * amountTriple ) - ( 3f * amountDouble ) ) + 1f;

            return new Vector3
            {
                X = ( ( ( value1.X * num4 ) + ( value2.X * num3 ) ) + ( tangent1.X * num2 ) ) + ( tangent2.X * num1 ),
                Y = ( ( ( value1.Y * num4 ) + ( value2.Y * num3 ) ) + ( tangent1.Y * num2 ) ) + ( tangent2.Y * num1 ),
                Z = ( ( ( value1.Z * num4 ) + ( value2.Z * num3 ) ) + ( tangent1.Z * num2 ) ) + ( tangent2.Z * num1 )
            };
        }

        /// <summary>
        /// Performs a Hermite spline interpolation.
        /// </summary>
        /// <param name="value1">Source position vector.</param>
        /// <param name="tangent1">Source tangent vector.</param>
        /// <param name="value2">Source position vector.</param>
        /// <param name="tangent2">Source tangent vector.</param>
        /// <param name="amount">Weighting factor.</param>
        /// <param name="result">[OutAttribute] The result of the Hermite spline interpolation.</param>
        public static void Hermite( ref Vector3 value1, ref Vector3 tangent1, ref Vector3 value2, ref Vector3 tangent2, float amount, out Vector3 result )
        {
            float amountDouble = amount * amount;
            float amountTriple = amount * amountDouble;

            float num1 = amountTriple - amountDouble;
            float num2 = ( amountTriple - ( 2f * amountDouble ) ) + amount;
            float num3 = ( -2f * amountTriple ) + ( 3f * amountDouble );
            float num4 = ( ( 2f * amountTriple ) - ( 3f * amountDouble ) ) + 1f;

            result.X = ( ( ( value1.X * num4 ) + ( value2.X * num3 ) ) + ( tangent1.X * num2 ) ) + ( tangent2.X * num1 );
            result.Y = ( ( ( value1.Y * num4 ) + ( value2.Y * num3 ) ) + ( tangent1.Y * num2 ) ) + ( tangent2.Y * num1 );
            result.Z = ( ( ( value1.Z * num4 ) + ( value2.Z * num3 ) ) + ( tangent1.Z * num2 ) ) + ( tangent2.Z * num1 );
        }

        /// <summary>
        /// Transforms a 3D vector by the given matrix.
        /// </summary>
        /// <param name="position">The source vector.</param>
        /// <param name="matrix">The transformation matrix.</param>
        public static Vector3 Transform( Vector3 position, Matrix matrix )
        {
            return new Vector3
            {
                X = ( ( ( position.X * matrix.M11 ) + ( position.Y * matrix.M21 ) ) + ( position.Z * matrix.M31 ) ) + matrix.M41,
                Y = ( ( ( position.X * matrix.M12 ) + ( position.Y * matrix.M22 ) ) + ( position.Z * matrix.M32 ) ) + matrix.M42,
                Z = ( ( ( position.X * matrix.M13 ) + ( position.Y * matrix.M23 ) ) + ( position.Z * matrix.M33 ) ) + matrix.M43
            };
        }

        /// <summary>
        /// Transforms a Vector3 by the given Matrix.
        /// </summary>
        /// <param name="position">The source Vector3.</param>
        /// <param name="matrix">The transformation Matrix.</param>
        /// <param name="result">[OutAttribute] The transformed vector.</param>
        public static void Transform( ref Vector3 position, ref Matrix matrix, out Vector3 result )
        {
            result.X = ( ( ( position.X * matrix.M11 ) + ( position.Y * matrix.M21 ) ) + ( position.Z * matrix.M31 ) ) + matrix.M41;
            result.Y = ( ( ( position.X * matrix.M12 ) + ( position.Y * matrix.M22 ) ) + ( position.Z * matrix.M32 ) ) + matrix.M42;
            result.Z = ( ( ( position.X * matrix.M13 ) + ( position.Y * matrix.M23 ) ) + ( position.Z * matrix.M33 ) ) + matrix.M43;
        }

        /// <summary>
        /// Transforms a 3D vector normal by a matrix.
        /// </summary>
        /// <param name="normal">The source vector.</param>
        /// <param name="matrix">The transformation matrix.</param>
        public static Vector3 TransformNormal( Vector3 normal, Matrix matrix )
        {
            return new Vector3
            {
                X = ( ( normal.X * matrix.M11 ) + ( normal.Y * matrix.M21 ) ) + ( normal.Z * matrix.M31 ),
                Y = ( ( normal.X * matrix.M12 ) + ( normal.Y * matrix.M22 ) ) + ( normal.Z * matrix.M32 ),
                Z = ( ( normal.X * matrix.M13 ) + ( normal.Y * matrix.M23 ) ) + ( normal.Z * matrix.M33 )
            };
        }

        /// <summary>
        /// Transforms a vector normal by a matrix.
        /// </summary>
        /// <param name="normal">The source vector.</param>
        /// <param name="matrix">The transformation Matrix.</param>
        /// <param name="result">[OutAttribute] The Vector3 resulting from the transformation.</param>
        public static void TransformNormal( ref Vector3 normal, ref Matrix matrix, out Vector3 result )
        {
            result.X = ( ( normal.X * matrix.M11 ) + ( normal.Y * matrix.M21 ) ) + ( normal.Z * matrix.M31 );
            result.Y = ( ( normal.X * matrix.M12 ) + ( normal.Y * matrix.M22 ) ) + ( normal.Z * matrix.M32 );
            result.Z = ( ( normal.X * matrix.M13 ) + ( normal.Y * matrix.M23 ) ) + ( normal.Z * matrix.M33 );
        }

        /// <summary>
        /// Transforms a Vector3 by a specified Quaternion rotation.
        /// </summary>
        /// <param name="value">The Vector3 to rotate.</param>
        /// <param name="rotation">The Quaternion rotation to apply.</param>
        public static Vector3 Transform( Vector3 value, Quaternion rotation )
        {
            float rotationX = rotation.X + rotation.X;
            float rotationY = rotation.Y + rotation.Y;
            float rotationZ = rotation.Z + rotation.Z;

            float num1 = rotation.W * rotationX;
            float num2 = rotation.W * rotationY;
            float num3 = rotation.W * rotationZ;

            float num4 = rotation.X * rotationX;
            float num5 = rotation.X * rotationY;
            float num6 = rotation.X * rotationZ;

            float num7 = rotation.Y * rotationY;
            float num8 = rotation.Y * rotationZ;
            float num9 = rotation.Z * rotationZ;

            return new Vector3
            {
                X = ( ( value.X * ( ( 1f - num7 ) - num9 ) ) + ( value.Y * ( num5 - num3 ) ) ) + ( value.Z * ( num6 + num2 ) ),
                Y = ( ( value.X * ( num5 + num3 ) ) + ( value.Y * ( ( 1f - num4 ) - num9 ) ) ) + ( value.Z * ( num8 - num1 ) ),
                Z = ( ( value.X * ( num6 - num2 ) ) + ( value.Y * ( num8 + num1 ) ) ) + ( value.Z * ( ( 1f - num4 ) - num7 ) )
            };
        }

        /// <summary>
        /// Transforms a Vector3 by a specified Quaternion rotation.
        /// </summary>
        /// <param name="value">The Vector3 to rotate.</param>
        /// <param name="rotation">The Quaternion rotation to apply.</param>
        /// <param name="result">[OutAttribute] An existing Vector3 filled in with the results of the rotation.</param>
        public static void Transform( ref Vector3 value, ref Quaternion rotation, out Vector3 result )
        {
            float rotationX = rotation.X + rotation.X;
            float rotationY = rotation.Y + rotation.Y;
            float rotationZ = rotation.Z + rotation.Z;

            float num1 = rotation.W * rotationX;
            float num2 = rotation.W * rotationY;
            float num3 = rotation.W * rotationZ;

            float num4 = rotation.X * rotationX;
            float num5 = rotation.X * rotationY;
            float num6 = rotation.X * rotationZ;

            float num7 = rotation.Y * rotationY;
            float num8 = rotation.Y * rotationZ;
            float num9 = rotation.Z * rotationZ;

            result.X = ( ( value.X * ( ( 1f - num7 ) - num9 ) ) + ( value.Y * ( num5 - num3 ) ) ) + ( value.Z * ( num6 + num2 ) );
            result.Y = ( ( value.X * ( num5 + num3 ) ) + ( value.Y * ( ( 1f - num4 ) - num9 ) ) ) + ( value.Z * ( num8 - num1 ) );
            result.Z = ( ( value.X * ( num6 - num2 ) ) + ( value.Y * ( num8 + num1 ) ) ) + ( value.Z * ( ( 1f - num4 ) - num7 ) );
        }

        /// <summary>
        /// Transforms a source array of Vector3s by a specified Matrix and writes the results to an existing destination array.
        /// </summary>
        /// <param name="sourceArray">The source array.</param>
        /// <param name="matrix">The transform Matrix to apply.</param>
        /// <param name="destinationArray">An existing destination array into which the transformed Vector3s are written.</param>
        public static void Transform( Vector3[] sourceArray, ref Matrix matrix, Vector3[] destinationArray )
        {
            if ( sourceArray == null )
                throw new ArgumentNullException( "sourceArray" );

            if ( destinationArray == null )
                throw new ArgumentNullException( "destinationArray" );

            if ( destinationArray.Length < sourceArray.Length )
                throw new ArgumentException( "FrameworkResources.NotEnoughTargetSize" );

            for ( int i = 0; i < sourceArray.Length; i++ )
            {
                float x = sourceArray[i].X;
                float y = sourceArray[i].Y;
                float z = sourceArray[i].Z;

                destinationArray[i].X = ( ( ( x * matrix.M11 ) + ( y * matrix.M21 ) ) + ( z * matrix.M31 ) ) + matrix.M41;
                destinationArray[i].Y = ( ( ( x * matrix.M12 ) + ( y * matrix.M22 ) ) + ( z * matrix.M32 ) ) + matrix.M42;
                destinationArray[i].Z = ( ( ( x * matrix.M13 ) + ( y * matrix.M23 ) ) + ( z * matrix.M33 ) ) + matrix.M43;
            }
        }

        /// <summary>
        /// Applies a specified transform Matrix to a specified range of an array of Vector3s and writes the results into a specified range of a destination array.
        /// </summary>
        /// <param name="sourceArray">The source array.</param>
        /// <param name="sourceIndex">The index in the source array at which to start.</param>
        /// <param name="matrix">The transform Matrix to apply.</param>
        /// <param name="destinationArray">The existing destination array.</param>
        /// <param name="destinationIndex">The index in the destination array at which to start.</param>
        /// <param name="length">The number of Vector3s to transform.</param>
        public static void Transform( Vector3[] sourceArray, int sourceIndex, ref Matrix matrix, Vector3[] destinationArray, int destinationIndex, int length )
        {
            if ( sourceArray == null )
                throw new ArgumentNullException( "sourceArray" );

            if ( destinationArray == null )
                throw new ArgumentNullException( "destinationArray" );

            if ( sourceArray.Length < ( sourceIndex + length ) )
                throw new ArgumentException( "FrameworkResources.NotEnoughSourceSize" );

            if ( destinationArray.Length < ( destinationIndex + length ) )
                throw new ArgumentException( "FrameworkResources.NotEnoughTargetSize" );

            while ( length > 0 )
            {
                float x = sourceArray[sourceIndex].X;
                float y = sourceArray[sourceIndex].Y;
                float z = sourceArray[sourceIndex].Z;

                destinationArray[destinationIndex].X = ( ( ( x * matrix.M11 ) + ( y * matrix.M21 ) ) + ( z * matrix.M31 ) ) + matrix.M41;
                destinationArray[destinationIndex].Y = ( ( ( x * matrix.M12 ) + ( y * matrix.M22 ) ) + ( z * matrix.M32 ) ) + matrix.M42;
                destinationArray[destinationIndex].Z = ( ( ( x * matrix.M13 ) + ( y * matrix.M23 ) ) + ( z * matrix.M33 ) ) + matrix.M43;

                sourceIndex++;
                destinationIndex++;
                length--;
            }
        }

        /// <summary>
        /// Transforms an array of 3D vector normals by a specified Matrix.
        /// </summary>
        /// <param name="sourceArray">The array of Vector3 normals to transform.</param>
        /// <param name="matrix">The transform matrix to apply.</param>
        /// <param name="destinationArray">An existing Vector3 array into which the results of the transforms are written.</param>
        public static void TransformNormal( Vector3[] sourceArray, ref Matrix matrix, Vector3[] destinationArray )
        {
            if ( sourceArray == null )
                throw new ArgumentNullException( "sourceArray" );

            if ( destinationArray == null )
                throw new ArgumentNullException( "destinationArray" );

            if ( destinationArray.Length < sourceArray.Length )
                throw new ArgumentException( "FrameworkResources.NotEnoughTargetSize" );

            for ( int i = 0; i < sourceArray.Length; i++ )
            {
                float x = sourceArray[i].X;
                float y = sourceArray[i].Y;
                float z = sourceArray[i].Z;

                destinationArray[i].X = ( ( x * matrix.M11 ) + ( y * matrix.M21 ) ) + ( z * matrix.M31 );
                destinationArray[i].Y = ( ( x * matrix.M12 ) + ( y * matrix.M22 ) ) + ( z * matrix.M32 );
                destinationArray[i].Z = ( ( x * matrix.M13 ) + ( y * matrix.M23 ) ) + ( z * matrix.M33 );
            }
        }

        /// <summary>
        /// Transforms a specified range in an array of 3D vector normals by a specified Matrix and writes the results to a specified range in a destination array.
        /// </summary>
        /// <param name="sourceArray">The source array of Vector3 normals.</param>
        /// <param name="sourceIndex">The starting index in the source array.</param>
        /// <param name="matrix">The transform Matrix to apply.</param>
        /// <param name="destinationArray">The destination Vector3 array.</param>
        /// <param name="destinationIndex">The starting index in the destination array.</param>
        /// <param name="length">The number of vectors to transform.</param>
        public static void TransformNormal( Vector3[] sourceArray, int sourceIndex, ref Matrix matrix, Vector3[] destinationArray, int destinationIndex, int length )
        {
            if ( sourceArray == null )
                throw new ArgumentNullException( "sourceArray" );

            if ( destinationArray == null )
                throw new ArgumentNullException( "destinationArray" );

            if ( sourceArray.Length < ( sourceIndex + length ) )
                throw new ArgumentException( "FrameworkResources.NotEnoughSourceSize" );

            if ( destinationArray.Length < ( destinationIndex + length ) )
                throw new ArgumentException( "FrameworkResources.NotEnoughTargetSize" );

            while ( length > 0 )
            {
                float x = sourceArray[sourceIndex].X;
                float y = sourceArray[sourceIndex].Y;
                float z = sourceArray[sourceIndex].Z;

                destinationArray[destinationIndex].X = ( ( x * matrix.M11 ) + ( y * matrix.M21 ) ) + ( z * matrix.M31 );
                destinationArray[destinationIndex].Y = ( ( x * matrix.M12 ) + ( y * matrix.M22 ) ) + ( z * matrix.M32 );
                destinationArray[destinationIndex].Z = ( ( x * matrix.M13 ) + ( y * matrix.M23 ) ) + ( z * matrix.M33 );

                sourceIndex++;
                destinationIndex++;
                length--;
            }
        }

        /// <summary>
        /// Transforms a source array of Vector3s by a specified Quaternion rotation and writes the results to an existing destination array.
        /// </summary>
        /// <param name="sourceArray">The source array.</param>
        /// <param name="rotation">The Quaternion rotation to apply.</param>
        /// <param name="destinationArray">An existing destination array into which the transformed Vector3s are written.</param>
        public static void Transform( Vector3[] sourceArray, ref Quaternion rotation, Vector3[] destinationArray )
        {
            if ( sourceArray == null )
                throw new ArgumentNullException( "sourceArray" );

            if ( destinationArray == null )
                throw new ArgumentNullException( "destinationArray" );

            if ( destinationArray.Length < sourceArray.Length )
                throw new ArgumentException( "FrameworkResources.NotEnoughTargetSize" );

            float rotationX = rotation.X + rotation.X;
            float rotationY = rotation.Y + rotation.Y;
            float rotationZ = rotation.Z + rotation.Z;

            float num1 = rotation.W * rotationX;
            float num2 = rotation.W * rotationY;
            float num3 = rotation.W * rotationZ;

            float num4 = rotation.X * rotationX;
            float num5 = rotation.X * rotationY;
            float num6 = rotation.X * rotationZ;

            float num7 = rotation.Y * rotationY;
            float num8 = rotation.Y * rotationZ;
            float num9 = rotation.Z * rotationZ;

            float result1 = ( 1f - num7 ) - num9;
            float result2 = num5 - num3;
            float result3 = num6 + num2;
            float result4 = num5 + num3;
            float result5 = ( 1f - num4 ) - num9;
            float result6 = num8 - num1;
            float result7 = num6 - num2;
            float result8 = num8 + num1;
            float result9 = ( 1f - num4 ) - num7;

            for ( int i = 0; i < sourceArray.Length; i++ )
            {
                float x = sourceArray[i].X;
                float y = sourceArray[i].Y;
                float z = sourceArray[i].Z;

                destinationArray[i].X = ( ( x * result1 ) + ( y * result2 ) ) + ( z * result3 );
                destinationArray[i].Y = ( ( x * result4 ) + ( y * result5 ) ) + ( z * result6 );
                destinationArray[i].Z = ( ( x * result7 ) + ( y * result8 ) ) + ( z * result9 );
            }
        }

        /// <summary>
        /// Applies a specified Quaternion rotation to a specified range of an array of Vector3s and writes the results into a specified range of a destination array.
        /// </summary>
        /// <param name="sourceArray">The source array.</param>
        /// <param name="sourceIndex">The index in the source array at which to start.</param>
        /// <param name="rotation">The Quaternion rotation to apply.</param>
        /// <param name="destinationArray">The existing destination array.</param>
        /// <param name="destinationIndex">The index in the destination array at which to start.</param>
        /// <param name="length">The number of Vector3s to transform.</param>
        public static void Transform( Vector3[] sourceArray, int sourceIndex, ref Quaternion rotation, Vector3[] destinationArray, int destinationIndex, int length )
        {
            if ( sourceArray == null )
                throw new ArgumentNullException( "sourceArray" );

            if ( destinationArray == null )
                throw new ArgumentNullException( "destinationArray" );

            if ( sourceArray.Length < ( sourceIndex + length ) )
                throw new ArgumentException( "FrameworkResources.NotEnoughSourceSize" );

            if ( destinationArray.Length < ( destinationIndex + length ) )
                throw new ArgumentException( "FrameworkResources.NotEnoughTargetSize" );

            float rotationX = rotation.X + rotation.X;
            float rotationY = rotation.Y + rotation.Y;
            float rotationZ = rotation.Z + rotation.Z;

            float num1 = rotation.W * rotationX;
            float num2 = rotation.W * rotationY;
            float num3 = rotation.W * rotationZ;

            float num4 = rotation.X * rotationX;
            float num5 = rotation.X * rotationY;
            float num6 = rotation.X * rotationZ;

            float num7 = rotation.Y * rotationY;
            float num8 = rotation.Y * rotationZ;
            float num9 = rotation.Z * rotationZ;

            float result1 = ( 1f - num7 ) - num9;
            float result2 = num5 - num3;
            float result3 = num6 + num2;
            float result4 = num5 + num3;
            float result5 = ( 1f - num4 ) - num9;
            float result6 = num8 - num1;
            float result7 = num6 - num2;
            float result8 = num8 + num1;
            float result9 = ( 1f - num4 ) - num7;

            while ( length > 0 )
            {
                float x = sourceArray[sourceIndex].X;
                float y = sourceArray[sourceIndex].Y;
                float z = sourceArray[sourceIndex].Z;

                destinationArray[destinationIndex].X = ( ( x * result1 ) + ( y * result2 ) ) + ( z * result3 );
                destinationArray[destinationIndex].Y = ( ( x * result4 ) + ( y * result5 ) ) + ( z * result6 );
                destinationArray[destinationIndex].Z = ( ( x * result7 ) + ( y * result8 ) ) + ( z * result9 );

                sourceIndex++;
                destinationIndex++;
                length--;
            }
        }

        /// <summary>
        /// Returns a vector pointing in the opposite direction.
        /// </summary>
        /// <param name="value">Source vector.</param>
        public static Vector3 Negate( Vector3 value )
        {
            return new Vector3 { X = -value.X, Y = -value.Y, Z = -value.Z };
        }

        /// <summary>
        /// Returns a vector pointing in the opposite direction.
        /// </summary>
        /// <param name="value">Source vector.</param>
        /// <param name="result">[OutAttribute] Vector pointing in the opposite direction.</param>
        public static void Negate( ref Vector3 value, out Vector3 result )
        {
            result.X = -value.X;
            result.Y = -value.Y;
            result.Z = -value.Z;
        }

        /// <summary>
        /// Adds two vectors.
        /// </summary>
        /// <param name="value1">Source vector.</param>
        /// <param name="value2">Source vector.</param>
        public static Vector3 Add( Vector3 value1, Vector3 value2 )
        {
            return new Vector3 { X = value1.X + value2.X, Y = value1.Y + value2.Y, Z = value1.Z + value2.Z };
        }

        /// <summary>
        /// Adds two vectors.
        /// </summary>
        /// <param name="value1">Source vector.</param>
        /// <param name="value2">Source vector.</param>
        /// <param name="result">[OutAttribute] Sum of the source vectors.</param>
        public static void Add( ref Vector3 value1, ref Vector3 value2, out Vector3 result )
        {
            result.X = value1.X + value2.X;
            result.Y = value1.Y + value2.Y;
            result.Z = value1.Z + value2.Z;
        }

        /// <summary>
        /// Subtracts a vector from a vector.
        /// </summary>
        /// <param name="value1">Source vector.</param>
        /// <param name="value2">Source vector.</param>
        public static Vector3 Subtract( Vector3 value1, Vector3 value2 )
        {
            return new Vector3 { X = value1.X - value2.X, Y = value1.Y - value2.Y, Z = value1.Z - value2.Z };
        }

        /// <summary>
        /// Subtracts a vector from a vector.
        /// </summary>
        /// <param name="value1">Source vector.</param>
        /// <param name="value2">Source vector.</param>
        /// <param name="result">[OutAttribute] The result of the subtraction.</param>
        public static void Subtract( ref Vector3 value1, ref Vector3 value2, out Vector3 result )
        {
            result.X = value1.X - value2.X;
            result.Y = value1.Y - value2.Y;
            result.Z = value1.Z - value2.Z;
        }

        /// <summary>
        /// Multiplies the components of two vectors by each other.
        /// </summary>
        /// <param name="value1">Source vector.</param>
        /// <param name="value2">Source vector.</param>
        public static Vector3 Multiply( Vector3 value1, Vector3 value2 )
        {
            return new Vector3 { X = value1.X * value2.X, Y = value1.Y * value2.Y, Z = value1.Z * value2.Z };
        }

        /// <summary>
        /// Multiplies the components of two vectors by each other.
        /// </summary>
        /// <param name="value1">Source vector.</param>
        /// <param name="value2">Source vector.</param>
        /// <param name="result">[OutAttribute] The result of the multiplication.</param>
        public static void Multiply( ref Vector3 value1, ref Vector3 value2, out Vector3 result )
        {
            result.X = value1.X * value2.X;
            result.Y = value1.Y * value2.Y;
            result.Z = value1.Z * value2.Z;
        }

        /// <summary>
        /// Multiplies a vector by a scalar value.
        /// </summary>
        /// <param name="value1">Source vector.</param>
        /// <param name="scaleFactor">Scalar value.</param>
        public static Vector3 Multiply( Vector3 value1, float scaleFactor )
        {
            return new Vector3 { X = value1.X * scaleFactor, Y = value1.Y * scaleFactor, Z = value1.Z * scaleFactor };
        }

        /// <summary>
        /// Multiplies a vector by a scalar value.
        /// </summary>
        /// <param name="value1">Source vector.</param>
        /// <param name="scaleFactor">Scalar value.</param>
        /// <param name="result">[OutAttribute] The result of the multiplication.</param>
        public static void Multiply( ref Vector3 value1, float scaleFactor, out Vector3 result )
        {
            result.X = value1.X * scaleFactor;
            result.Y = value1.Y * scaleFactor;
            result.Z = value1.Z * scaleFactor;
        }

        /// <summary>
        /// Divides the components of a vector by the components of another vector.
        /// </summary>
        /// <param name="value1">Source vector.</param>
        /// <param name="value2">Divisor vector.</param>
        public static Vector3 Divide( Vector3 value1, Vector3 value2 )
        {
            return new Vector3 { X = value1.X / value2.X, Y = value1.Y / value2.Y, Z = value1.Z / value2.Z };
        }

        /// <summary>
        /// Divides the components of a vector by the components of another vector.
        /// </summary>
        /// <param name="value1">Source vector.</param>
        /// <param name="value2">The divisor.</param>
        /// <param name="result">[OutAttribute] The result of the division.</param>
        public static void Divide( ref Vector3 value1, ref Vector3 value2, out Vector3 result )
        {
            result.X = value1.X / value2.X;
            result.Y = value1.Y / value2.Y;
            result.Z = value1.Z / value2.Z;
        }

        /// <summary>
        /// Divides a vector by a scalar value.
        /// </summary>
        /// <param name="value1">Source vector.</param>
        /// <param name="value2">The divisor.</param>
        public static Vector3 Divide( Vector3 value1, float value2 )
        {
            float num = 1f / value2;

            return new Vector3 { X = value1.X * num, Y = value1.Y * num, Z = value1.Z * num };
        }

        /// <summary>
        /// Divides a vector by a scalar value.
        /// </summary>
        /// <param name="value1">Source vector.</param>
        /// <param name="value2">The divisor.</param>
        /// <param name="result">[OutAttribute] The result of the division.</param>
        public static void Divide( ref Vector3 value1, float value2, out Vector3 result )
        {
            float num = 1f / value2;

            result.X = value1.X * num;
            result.Y = value1.Y * num;
            result.Z = value1.Z * num;
        }

        #endregion

        #region zh-CHS 共有静态方法 | en Public Static Methods

        /// <summary>
        /// Returns a vector pointing in the opposite direction.
        /// </summary>
        /// <param name="value">Source vector.</param>
        public static Vector3 operator -( Vector3 value )
        {
            return new Vector3 { X = -value.X, Y = -value.Y, Z = -value.Z };
        }

        /// <summary>
        /// Tests vectors for equality.
        /// </summary>
        /// <param name="value1">Source vector.</param>
        /// <param name="value2">Source vector.</param>
        public static bool operator ==( Vector3 value1, Vector3 value2 )
        {
            return ( ( ( value1.X == value2.X ) && ( value1.Y == value2.Y ) ) && ( value1.Z == value2.Z ) );
        }

        /// <summary>
        /// Tests vectors for inequality.
        /// </summary>
        /// <param name="value1">Vector to compare.</param>
        /// <param name="value2">Vector to compare.</param>
        public static bool operator !=( Vector3 value1, Vector3 value2 )
        {
            if ( ( value1.X == value2.X ) && ( value1.Y == value2.Y ) )
                return !( value1.Z == value2.Z );

            return true;
        }

        /// <summary>
        /// Adds two vectors.
        /// </summary>
        /// <param name="value1">Source vector.</param>
        /// <param name="value2">Source vector.</param>
        public static Vector3 operator +( Vector3 value1, Vector3 value2 )
        {
            return new Vector3 { X = value1.X + value2.X, Y = value1.Y + value2.Y, Z = value1.Z + value2.Z };
        }

        /// <summary>
        /// Subtracts a vector from a vector.
        /// </summary>
        /// <param name="value1">Source vector.</param>
        /// <param name="value2">Source vector.</param>
        public static Vector3 operator -( Vector3 value1, Vector3 value2 )
        {
            return new Vector3 { X = value1.X - value2.X, Y = value1.Y - value2.Y, Z = value1.Z - value2.Z };
        }

        /// <summary>
        /// Multiplies the components of two vectors by each other.
        /// </summary>
        /// <param name="value1">Source vector.</param>
        /// <param name="value2">Source vector.</param>
        public static Vector3 operator *( Vector3 value1, Vector3 value2 )
        {
            return new Vector3 { X = value1.X * value2.X, Y = value1.Y * value2.Y, Z = value1.Z * value2.Z };
        }

        /// <summary>
        /// Multiplies a vector by a scalar value.
        /// </summary>
        /// <param name="value">Source vector.</param>
        /// <param name="scaleFactor">Scalar value.</param>
        public static Vector3 operator *( Vector3 value, float scaleFactor )
        {
            return new Vector3 { X = value.X * scaleFactor, Y = value.Y * scaleFactor, Z = value.Z * scaleFactor };
        }

        /// <summary>
        /// Multiplies a vector by a scalar value.
        /// </summary>
        /// <param name="scaleFactor">Scalar value.</param>
        /// <param name="value">Source vector.</param>
        public static Vector3 operator *( float scaleFactor, Vector3 value )
        {
            return new Vector3 { X = value.X * scaleFactor, Y = value.Y * scaleFactor, Z = value.Z * scaleFactor };
        }

        /// <summary>
        /// Divides the components of a vector by the components of another vector.
        /// </summary>
        /// <param name="value1">Source vector.</param>
        /// <param name="value2">Divisor vector.</param>
        public static Vector3 operator /( Vector3 value1, Vector3 value2 )
        {
            return new Vector3 { X = value1.X / value2.X, Y = value1.Y / value2.Y, Z = value1.Z / value2.Z };
        }

        /// <summary>
        /// Divides a vector by a scalar value.
        /// </summary>
        /// <param name="value">Source vector.</param>
        /// <param name="divider">The divisor.</param>
        public static Vector3 operator /( Vector3 value, float divider )
        {
            float num = 1f / divider;

            return new Vector3 { X = value.X * num, Y = value.Y * num, Z = value.Z * num };
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="vector4"></param>
        /// <returns></returns>
        public static explicit operator Vector3( Vector4 vector4 )
        {
            return new Vector3( vector4.X, vector4.Y, vector4.Z );
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="vector2"></param>
        /// <returns></returns>
        public static explicit operator Vector3( Vector2 vector2 )
        {
            return new Vector3( vector2, 0 );
        }

        #endregion

        #region zh-CHS 方法覆盖 | en Override Methods

        /// <summary>
        /// Retrieves a string representation of the current object.
        /// </summary>
        public override string ToString()
        {
            return string.Format( CultureInfo.CurrentCulture, "{{X:{0} Y:{1} Z:{2}}}", new object[] { X.ToString( CultureInfo.CurrentCulture ), Y.ToString( CultureInfo.CurrentCulture ), Z.ToString( CultureInfo.CurrentCulture ) } );
        }

        /// <summary>
        /// Returns a value that indicates whether the current instance is equal to a specified object.
        /// </summary>
        /// <param name="obj">Object to make the comparison with.</param>
        public override bool Equals( object obj )
        {
            if ( obj is Vector3 )
                return Equals( (Vector3)obj );
            else
                return false;
        }

        /// <summary>
        /// Gets the hash code of the vector object.
        /// </summary>
        public override int GetHashCode()
        {
            return ( ( X.GetHashCode() + Y.GetHashCode() ) + Z.GetHashCode() );
        }

        #endregion

        #region zh-CHS 接口实现 | en Interface Implementation

        /// <summary>
        /// Determines whether the specified Object is equal to the Vector3.
        /// </summary>
        /// <param name="other">The Vector3 to compare with the current Vector3.</param>
        public bool Equals( Vector3 other )
        {
            return ( ( ( X == other.X ) && ( Y == other.Y ) ) && ( Z == other.Z ) );
        }

        #endregion

    }
}
#endregion
