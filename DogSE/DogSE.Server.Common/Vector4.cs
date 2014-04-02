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
    /// Defines a vector with four components.
    /// </summary>
    public struct Vector4 : IEquatable<Vector4>
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
        /// <summary>
        /// Gets or sets the w-component of the vector.
        /// </summary>
        public float W;

        #endregion

        #region zh-CHS 构造和初始化和清理 | en Constructors and Initializers and Dispose

        /// <summary>
        /// Initializes a new instance of Vector4.
        /// </summary>
        /// <param name="x">Initial value for the x-component of the vector.</param>
        /// <param name="y">Initial value for the y-component of the vector.</param>
        /// <param name="z">Initial value for the z-component of the vector.</param>
        /// <param name="w">Initial value for the w-component of the vector.</param>
        public Vector4( float x, float y, float z, float w )
        {
            X = x;
            Y = y;
            Z = z;
            W = w;
        }

        /// <summary>
        /// Initializes a new instance of Vector4.
        /// </summary>
        /// <param name="value">A vector containing the values to initialize x and y components with.</param>
        /// <param name="z">Initial value for the z-component of the vector.</param>
        /// <param name="w">Initial value for the w-component of the vector.</param>
        public Vector4( Vector2 value, float z, float w )
        {
            X = value.X;
            Y = value.Y;
            Z = z;
            W = w;
        }

        /// <summary>
        /// Initializes a new instance of Vector4.
        /// </summary>
        /// <param name="value">A vector containing the values to initialize x, y, and z components with.</param>
        /// <param name="w">Initial value for the w-component of the vector.</param>
        public Vector4( Vector3 value, float w )
        {
            X = value.X;
            Y = value.Y;
            Z = value.Z;
            W = w;
        }

        /// <summary>
        /// Creates a new instance of Vector4.
        /// </summary>
        /// <param name="value">Value to initialize each component to.</param>
        public Vector4( float value )
        {
            X = Y = Z = W = value;
        }

        #endregion

        #region zh-CHS 共有静态属性 | en Public Static Properties

        #region zh-CHS 私有静态成员变量 | en Private Static Member Variables
        /// <summary>
        /// 
        /// </summary>
        private static Vector4 m_Zero = new Vector4();
        #endregion
        /// <summary>
        /// Returns a Vector4 with all of its components set to zero.
        /// </summary>
        public static Vector4 Zero
        {
            get { return m_Zero; }
        }

        #region zh-CHS 私有静态成员变量 | en Private Static Member Variables
        /// <summary>
        /// 
        /// </summary>
        private static Vector4 m_One = new Vector4( 1f, 1f, 1f, 1f );
        #endregion
        /// <summary>
        /// Returns a Vector4 with all of its components set to one.
        /// </summary>
        public static Vector4 One
        {
            get { return m_One; }
        }

        #region zh-CHS 私有静态成员变量 | en Private Static Member Variables
        /// <summary>
        /// 
        /// </summary>
        private static Vector4 m_UnitX = new Vector4( 1f, 0f, 0f, 0f );
        #endregion
        /// <summary>
        /// Returns the Vector4 (1, 0, 0, 0).
        /// </summary>
        public static Vector4 UnitX
        {
            get { return m_UnitX; }
        }

        #region zh-CHS 私有静态成员变量 | en Private Static Member Variables
        /// <summary>
        /// 
        /// </summary>
        private static Vector4 m_UnitY = new Vector4( 0f, 1f, 0f, 0f );
        #endregion
        /// <summary>
        /// Returns the Vector4 (0, 1, 0, 0).
        /// </summary>
        public static Vector4 UnitY
        {
            get { return m_UnitY; }
        }

        #region zh-CHS 私有静态成员变量 | en Private Static Member Variables
        /// <summary>
        /// 
        /// </summary>
        private static Vector4 m_UnitZ = new Vector4( 0f, 0f, 1f, 0f );
        #endregion
        /// <summary>
        /// Returns the Vector4 (0, 0, 1, 0).
        /// </summary>
        public static Vector4 UnitZ
        {
            get { return m_UnitZ; }
        }

        #region zh-CHS 私有静态成员变量 | en Private Static Member Variables
        /// <summary>
        /// 
        /// </summary>
        private static Vector4 m_UnitW = new Vector4( 0f, 0f, 0f, 1f );
        #endregion
        /// <summary>
        /// Returns the Vector4 (0, 0, 0, 1).
        /// </summary>
        public static Vector4 UnitW
        {
            get { return m_UnitW; }
        }

        #endregion

        #region zh-CHS 共有方法 | en Public Methods

        /// <summary>
        /// Calculates the length of the vector.
        /// </summary>
        public float Length()
        {
            return (float)Math.Sqrt( (double)( ( ( X * X ) + ( Y * Y ) ) + ( Z * Z ) ) + ( W * W ) );
        }

        /// <summary>
        /// Calculates the length of the vector squared.
        /// </summary>
        public float LengthSquared()
        {
            return ( ( ( ( X * X ) + ( Y * Y ) ) + ( Z * Z ) ) + ( W * W ) );
        }

        /// <summary>
        /// Turns the current vector into a unit vector.
        /// </summary>
        public void Normalize()
        {
            float num = 1f / ( (float)Math.Sqrt( (double)( ( ( X * X ) + ( Y * Y ) ) + ( Z * Z ) ) + ( W * W ) ) );

            X *= num;
            Y *= num;
            Z *= num;
            W *= num;
        }

        #endregion

        #region zh-CHS 共有静态方法 | en Public Static Methods

        /// <summary>
        /// Calculates the distance between two vectors.
        /// </summary>
        /// <param name="value1">Source vector.</param>
        /// <param name="value2">Source vector.</param>
        public static float Distance( Vector4 value1, Vector4 value2 )
        {
            float num1 = value1.X - value2.X;
            float num2 = value1.Y - value2.Y;
            float num3 = value1.Z - value2.Z;
            float num4 = value1.W - value2.W;

            return (float)Math.Sqrt( (double)( ( ( num1 * num1 ) + ( num2 * num2 ) ) + ( num3 * num3 ) ) + ( num4 * num4 ) );
        }

        /// <summary>
        /// Calculates the distance between two vectors.
        /// </summary>
        /// <param name="value1">Source vector.</param>
        /// <param name="value2">Source vector.</param>
        /// <param name="result">[OutAttribute] The distance between the vectors.</param>
        public static void Distance( ref Vector4 value1, ref Vector4 value2, out float result )
        {
            float num1 = value1.X - value2.X;
            float num2 = value1.Y - value2.Y;
            float num3 = value1.Z - value2.Z;
            float num4 = value1.W - value2.W;

            result = (float)Math.Sqrt( (double)( ( ( num1 * num1 ) + ( num2 * num2 ) ) + ( num3 * num3 ) ) + ( num4 * num4 ) );
        }

        /// <summary>
        /// Calculates the distance between two vectors squared.
        /// </summary>
        /// <param name="value1">Source vector.</param>
        /// <param name="value2">Source vector.</param>
        public static float DistanceSquared( Vector4 value1, Vector4 value2 )
        {
            float num1 = value1.X - value2.X;
            float num2 = value1.Y - value2.Y;
            float num3 = value1.Z - value2.Z;
            float num4 = value1.W - value2.W;

            return ( ( ( ( num1 * num1 ) + ( num2 * num2 ) ) + ( num3 * num3 ) ) + ( num4 * num4 ) );
        }

        /// <summary>
        /// Calculates the distance between two vectors squared.
        /// </summary>
        /// <param name="value1">Source vector.</param>
        /// <param name="value2">Source vector.</param>
        /// <param name="result">[OutAttribute] The distance between the two vectors squared.</param>
        public static void DistanceSquared( ref Vector4 value1, ref Vector4 value2, out float result )
        {
            float num1 = value1.X - value2.X;
            float num2 = value1.Y - value2.Y;
            float num3 = value1.Z - value2.Z;
            float num4 = value1.W - value2.W;

            result = ( ( ( num1 * num1 ) + ( num2 * num2 ) ) + ( num3 * num3 ) ) + ( num4 * num4 );
        }

        /// <summary>
        /// Calculates the dot product of two vectors.
        /// </summary>
        /// <param name="vector1">Source vector.</param>
        /// <param name="vector2">Source vector.</param>
        public static float Dot( Vector4 vector1, Vector4 vector2 )
        {
            return ( ( ( ( vector1.X * vector2.X ) + ( vector1.Y * vector2.Y ) ) + ( vector1.Z * vector2.Z ) ) + ( vector1.W * vector2.W ) );
        }

        /// <summary>
        /// Calculates the dot product of two vectors.
        /// </summary>
        /// <param name="vector1">Source vector.</param>
        /// <param name="vector2">Source vector.</param>
        /// <param name="result">[OutAttribute] The dot product of the two vectors.</param>
        public static void Dot( ref Vector4 vector1, ref Vector4 vector2, out float result )
        {
            result = ( ( ( vector1.X * vector2.X ) + ( vector1.Y * vector2.Y ) ) + ( vector1.Z * vector2.Z ) ) + ( vector1.W * vector2.W );
        }

        /// <summary>
        /// Creates a unit vector from the specified vector.
        /// </summary>
        /// <param name="vector">The source Vector4.</param>
        public static Vector4 Normalize( Vector4 vector )
        {
            float num = 1f / ( (float)Math.Sqrt( (double)( ( ( vector.X * vector.X ) + ( vector.Y * vector.Y ) ) + ( vector.Z * vector.Z ) ) + ( vector.W * vector.W ) ) );

            return new Vector4 { X = vector.X * num, Y = vector.Y * num, Z = vector.Z * num, W = vector.W * num };
        }

        /// <summary>
        /// Returns a normalized version of the specified vector.
        /// </summary>
        /// <param name="vector">Source vector.</param>
        /// <param name="result">[OutAttribute] The normalized vector.</param>
        public static void Normalize( ref Vector4 vector, out Vector4 result )
        {
            float num = 1f / ( (float)Math.Sqrt( (double)( ( ( vector.X * vector.X ) + ( vector.Y * vector.Y ) ) + ( vector.Z * vector.Z ) ) + ( vector.W * vector.W ) ) );

            result.X = vector.X * num;
            result.Y = vector.Y * num;
            result.Z = vector.Z * num;
            result.W = vector.W * num;
        }

        /// <summary>
        /// Returns a vector that contains the lowest value from each matching pair of components.
        /// </summary>
        /// <param name="value1">Source vector.</param>
        /// <param name="value2">Source vector.</param>
        public static Vector4 Min( Vector4 value1, Vector4 value2 )
        {
            return new Vector4 { X = ( value1.X < value2.X ) ? value1.X : value2.X, Y = ( value1.Y < value2.Y ) ? value1.Y : value2.Y, Z = ( value1.Z < value2.Z ) ? value1.Z : value2.Z, W = ( value1.W < value2.W ) ? value1.W : value2.W };
        }

        /// <summary>
        /// Returns a vector that contains the lowest value from each matching pair of components.
        /// </summary>
        /// <param name="value1">Source vector.</param>
        /// <param name="value2">Source vector.</param>
        /// <param name="result">[OutAttribute] The minimized vector.</param>
        public static void Min( ref Vector4 value1, ref Vector4 value2, out Vector4 result )
        {
            result.X = ( value1.X < value2.X ) ? value1.X : value2.X;
            result.Y = ( value1.Y < value2.Y ) ? value1.Y : value2.Y;
            result.Z = ( value1.Z < value2.Z ) ? value1.Z : value2.Z;
            result.W = ( value1.W < value2.W ) ? value1.W : value2.W;
        }

        /// <summary>
        /// Returns a vector that contains the highest value from each matching pair of components.
        /// </summary>
        /// <param name="value1">Source vector.</param>
        /// <param name="value2">Source vector.</param>
        public static Vector4 Max( Vector4 value1, Vector4 value2 )
        {
            return new Vector4
            {
                X = ( value1.X > value2.X ) ? value1.X : value2.X,
                Y = ( value1.Y > value2.Y ) ? value1.Y : value2.Y,
                Z = ( value1.Z > value2.Z ) ? value1.Z : value2.Z,
                W = ( value1.W > value2.W ) ? value1.W : value2.W
            };
        }

        /// <summary>
        /// Returns a vector that contains the highest value from each matching pair of components.
        /// </summary>
        /// <param name="value1">Source vector.</param>
        /// <param name="value2">Source vector.</param>
        /// <param name="result">[OutAttribute] The maximized vector.</param>
        public static void Max( ref Vector4 value1, ref Vector4 value2, out Vector4 result )
        {
            result.X = ( value1.X > value2.X ) ? value1.X : value2.X;
            result.Y = ( value1.Y > value2.Y ) ? value1.Y : value2.Y;
            result.Z = ( value1.Z > value2.Z ) ? value1.Z : value2.Z;
            result.W = ( value1.W > value2.W ) ? value1.W : value2.W;
        }

        /// <summary>
        /// Restricts a value to be within a specified range.
        /// </summary>
        /// <param name="value1">The value to clamp.</param>
        /// <param name="min">The minimum value.</param>
        /// <param name="max">The maximum value.</param>
        public static Vector4 Clamp( Vector4 value1, Vector4 min, Vector4 max )
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

            float w = value1.W;
            w = ( w > max.W ) ? max.W : w;
            w = ( w < min.W ) ? min.W : w;

            return new Vector4 { X = x, Y = y, Z = z, W = w };
        }

        /// <summary>
        /// Restricts a value to be within a specified range.
        /// </summary>
        /// <param name="value1">The value to clamp.</param>
        /// <param name="min">The minimum value.</param>
        /// <param name="max">The maximum value.</param>
        /// <param name="result">[OutAttribute] The clamped value.</param>
        public static void Clamp( ref Vector4 value1, ref Vector4 min, ref Vector4 max, out Vector4 result )
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

            float w = value1.W;
            w = ( w > max.W ) ? max.W : w;
            w = ( w < min.W ) ? min.W : w;

            result.X = x;
            result.Y = y;
            result.Z = z;
            result.W = w;
        }

        /// <summary>
        /// Performs a linear interpolation between two vectors.
        /// </summary>
        /// <param name="value1">Source vector.</param>
        /// <param name="value2">Source vector.</param>
        /// <param name="amount">Value between 0 and 1 indicating the weight of value2.</param>
        public static Vector4 Lerp( Vector4 value1, Vector4 value2, float amount )
        {
            return new Vector4
            {
                X = value1.X + ( ( value2.X - value1.X ) * amount ),
                Y = value1.Y + ( ( value2.Y - value1.Y ) * amount ),
                Z = value1.Z + ( ( value2.Z - value1.Z ) * amount ),
                W = value1.W + ( ( value2.W - value1.W ) * amount )
            };
        }

        /// <summary>
        /// Performs a linear interpolation between two vectors.
        /// </summary>
        /// <param name="value1">Source vector.</param>
        /// <param name="value2">Source vector.</param>
        /// <param name="amount">Value between 0 and 1 indicating the weight of value2.</param>
        /// <param name="result">[OutAttribute] The result of the interpolation.</param>
        public static void Lerp( ref Vector4 value1, ref Vector4 value2, float amount, out Vector4 result )
        {
            result.X = value1.X + ( ( value2.X - value1.X ) * amount );
            result.Y = value1.Y + ( ( value2.Y - value1.Y ) * amount );
            result.Z = value1.Z + ( ( value2.Z - value1.Z ) * amount );
            result.W = value1.W + ( ( value2.W - value1.W ) * amount );
        }

        /// <summary>
        /// Returns a Vector4 containing the 4D Cartesian coordinates of a point specified in barycentric (areal) coordinates relative to a 4D triangle.
        /// </summary>
        /// <param name="value1">A Vector4 containing the 4D Cartesian coordinates of vertex 1 of the triangle.</param>
        /// <param name="value2">A Vector4 containing the 4D Cartesian coordinates of vertex 2 of the triangle.</param>
        /// <param name="value3">A Vector4 containing the 4D Cartesian coordinates of vertex 3 of the triangle.</param>
        /// <param name="amount1">Barycentric coordinate b2, which expresses the weighting factor toward vertex 2 (specified in value2).</param>
        /// <param name="amount2">Barycentric coordinate b3, which expresses the weighting factor toward vertex 3 (specified in value3).</param>
        public static Vector4 Barycentric( Vector4 value1, Vector4 value2, Vector4 value3, float amount1, float amount2 )
        {
            return new Vector4
            {
                X = ( value1.X + ( amount1 * ( value2.X - value1.X ) ) ) + ( amount2 * ( value3.X - value1.X ) ),
                Y = ( value1.Y + ( amount1 * ( value2.Y - value1.Y ) ) ) + ( amount2 * ( value3.Y - value1.Y ) ),
                Z = ( value1.Z + ( amount1 * ( value2.Z - value1.Z ) ) ) + ( amount2 * ( value3.Z - value1.Z ) ),
                W = ( value1.W + ( amount1 * ( value2.W - value1.W ) ) ) + ( amount2 * ( value3.W - value1.W ) )
            };
        }

        /// <summary>
        /// Returns a Vector4 containing the 4D Cartesian coordinates of a point specified in Barycentric (areal) coordinates relative to a 4D triangle.
        /// </summary>
        /// <param name="value1">A Vector4 containing the 4D Cartesian coordinates of vertex 1 of the triangle.</param>
        /// <param name="value2">A Vector4 containing the 4D Cartesian coordinates of vertex 2 of the triangle.</param>
        /// <param name="value3">A Vector4 containing the 4D Cartesian coordinates of vertex 3 of the triangle.</param>
        /// <param name="amount1">Barycentric coordinate b2, which expresses the weighting factor toward vertex 2 (specified in value2).</param>
        /// <param name="amount2">Barycentric coordinate b3, which expresses the weighting factor toward vertex 3 (specified in value3).</param>
        /// <param name="result">[OutAttribute] The 4D Cartesian coordinates of the specified point are placed in this Vector4 on exit.</param>
        public static void Barycentric( ref Vector4 value1, ref Vector4 value2, ref Vector4 value3, float amount1, float amount2, out Vector4 result )
        {
            result.X = ( value1.X + ( amount1 * ( value2.X - value1.X ) ) ) + ( amount2 * ( value3.X - value1.X ) );
            result.Y = ( value1.Y + ( amount1 * ( value2.Y - value1.Y ) ) ) + ( amount2 * ( value3.Y - value1.Y ) );
            result.Z = ( value1.Z + ( amount1 * ( value2.Z - value1.Z ) ) ) + ( amount2 * ( value3.Z - value1.Z ) );
            result.W = ( value1.W + ( amount1 * ( value2.W - value1.W ) ) ) + ( amount2 * ( value3.W - value1.W ) );
        }

        /// <summary>
        /// Interpolates between two values using a cubic equation.
        /// </summary>
        /// <param name="value1">Source value.</param>
        /// <param name="value2">Source value.</param>
        /// <param name="amount">Weighting value.</param>
        public static Vector4 SmoothStep( Vector4 value1, Vector4 value2, float amount )
        {
            amount = ( amount > 1f ) ? 1f : ( ( amount < 0f ) ? 0f : amount );
            amount = ( amount * amount ) * ( 3f - ( 2f * amount ) );

            return new Vector4
            {
                X = value1.X + ( ( value2.X - value1.X ) * amount ),
                Y = value1.Y + ( ( value2.Y - value1.Y ) * amount ),
                Z = value1.Z + ( ( value2.Z - value1.Z ) * amount ),
                W = value1.W + ( ( value2.W - value1.W ) * amount )
            };
        }

        /// <summary>
        /// Interpolates between two values using a cubic equation.
        /// </summary>
        /// <param name="value1">Source vector.</param>
        /// <param name="value2">Source vector.</param>
        /// <param name="amount">Weighting factor.</param>
        /// <param name="result">[OutAttribute] The interpolated value.</param>
        public static void SmoothStep( ref Vector4 value1, ref Vector4 value2, float amount, out Vector4 result )
        {
            amount = ( amount > 1f ) ? 1f : ( ( amount < 0f ) ? 0f : amount );
            amount = ( amount * amount ) * ( 3f - ( 2f * amount ) );

            result.X = value1.X + ( ( value2.X - value1.X ) * amount );
            result.Y = value1.Y + ( ( value2.Y - value1.Y ) * amount );
            result.Z = value1.Z + ( ( value2.Z - value1.Z ) * amount );
            result.W = value1.W + ( ( value2.W - value1.W ) * amount );
        }

        /// <summary>
        /// Performs a Catmull-Rom interpolation using the specified positions.
        /// </summary>
        /// <param name="value1">The first position in the interpolation.</param>
        /// <param name="value2">The second position in the interpolation.</param>
        /// <param name="value3">The third position in the interpolation.</param>
        /// <param name="value4">The fourth position in the interpolation.</param>
        /// <param name="amount">Weighting factor.</param>
        public static Vector4 CatmullRom( Vector4 value1, Vector4 value2, Vector4 value3, Vector4 value4, float amount )
        {
            float amountDouble = amount * amount;
            float amountTriple = amount * amountDouble;

            return new Vector4
            {
                X = 0.5f * ( ( ( ( 2f * value2.X ) + ( ( -value1.X + value3.X ) * amount ) ) + ( ( ( ( ( 2f * value1.X ) - ( 5f * value2.X ) ) + ( 4f * value3.X ) ) - value4.X ) * amountDouble ) ) + ( ( ( ( -value1.X + ( 3f * value2.X ) ) - ( 3f * value3.X ) ) + value4.X ) * amountTriple ) ),
                Y = 0.5f * ( ( ( ( 2f * value2.Y ) + ( ( -value1.Y + value3.Y ) * amount ) ) + ( ( ( ( ( 2f * value1.Y ) - ( 5f * value2.Y ) ) + ( 4f * value3.Y ) ) - value4.Y ) * amountDouble ) ) + ( ( ( ( -value1.Y + ( 3f * value2.Y ) ) - ( 3f * value3.Y ) ) + value4.Y ) * amountTriple ) ),
                Z = 0.5f * ( ( ( ( 2f * value2.Z ) + ( ( -value1.Z + value3.Z ) * amount ) ) + ( ( ( ( ( 2f * value1.Z ) - ( 5f * value2.Z ) ) + ( 4f * value3.Z ) ) - value4.Z ) * amountDouble ) ) + ( ( ( ( -value1.Z + ( 3f * value2.Z ) ) - ( 3f * value3.Z ) ) + value4.Z ) * amountTriple ) ),
                W = 0.5f * ( ( ( ( 2f * value2.W ) + ( ( -value1.W + value3.W ) * amount ) ) + ( ( ( ( ( 2f * value1.W ) - ( 5f * value2.W ) ) + ( 4f * value3.W ) ) - value4.W ) * amountDouble ) ) + ( ( ( ( -value1.W + ( 3f * value2.W ) ) - ( 3f * value3.W ) ) + value4.W ) * amountTriple ) )
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
        public static void CatmullRom( ref Vector4 value1, ref Vector4 value2, ref Vector4 value3, ref Vector4 value4, float amount, out Vector4 result )
        {
            float amountDouble = amount * amount;
            float amountTriple = amount * amountDouble;

            result.X = 0.5f * ( ( ( ( 2f * value2.X ) + ( ( -value1.X + value3.X ) * amount ) ) + ( ( ( ( ( 2f * value1.X ) - ( 5f * value2.X ) ) + ( 4f * value3.X ) ) - value4.X ) * amountDouble ) ) + ( ( ( ( -value1.X + ( 3f * value2.X ) ) - ( 3f * value3.X ) ) + value4.X ) * amountTriple ) );
            result.Y = 0.5f * ( ( ( ( 2f * value2.Y ) + ( ( -value1.Y + value3.Y ) * amount ) ) + ( ( ( ( ( 2f * value1.Y ) - ( 5f * value2.Y ) ) + ( 4f * value3.Y ) ) - value4.Y ) * amountDouble ) ) + ( ( ( ( -value1.Y + ( 3f * value2.Y ) ) - ( 3f * value3.Y ) ) + value4.Y ) * amountTriple ) );
            result.Z = 0.5f * ( ( ( ( 2f * value2.Z ) + ( ( -value1.Z + value3.Z ) * amount ) ) + ( ( ( ( ( 2f * value1.Z ) - ( 5f * value2.Z ) ) + ( 4f * value3.Z ) ) - value4.Z ) * amountDouble ) ) + ( ( ( ( -value1.Z + ( 3f * value2.Z ) ) - ( 3f * value3.Z ) ) + value4.Z ) * amountTriple ) );
            result.W = 0.5f * ( ( ( ( 2f * value2.W ) + ( ( -value1.W + value3.W ) * amount ) ) + ( ( ( ( ( 2f * value1.W ) - ( 5f * value2.W ) ) + ( 4f * value3.W ) ) - value4.W ) * amountDouble ) ) + ( ( ( ( -value1.W + ( 3f * value2.W ) ) - ( 3f * value3.W ) ) + value4.W ) * amountTriple ) );
        }

        /// <summary>
        /// Performs a Hermite spline interpolation.
        /// </summary>
        /// <param name="value1">Source position vector.</param>
        /// <param name="tangent1">Source tangent vector.</param>
        /// <param name="value2">Source position vector.</param>
        /// <param name="tangent2">Source tangent vector.</param>
        /// <param name="amount">Weighting factor.</param>
        public static Vector4 Hermite( Vector4 value1, Vector4 tangent1, Vector4 value2, Vector4 tangent2, float amount )
        {
            float amountDouble = amount * amount;
            float amountTriple = amount * amountDouble;

            float num1 = ( ( 2f * amountTriple ) - ( 3f * amountDouble ) ) + 1f;
            float num2 = ( -2f * amountTriple ) + ( 3f * amountDouble );
            float num3 = ( amountTriple - ( 2f * amountDouble ) ) + amount;
            float num4 = amountTriple - amountDouble;

            return new Vector4
            {
                X = ( ( ( value1.X * num1 ) + ( value2.X * num2 ) ) + ( tangent1.X * num3 ) ) + ( tangent2.X * num4 ),
                Y = ( ( ( value1.Y * num1 ) + ( value2.Y * num2 ) ) + ( tangent1.Y * num3 ) ) + ( tangent2.Y * num4 ),
                Z = ( ( ( value1.Z * num1 ) + ( value2.Z * num2 ) ) + ( tangent1.Z * num3 ) ) + ( tangent2.Z * num4 ),
                W = ( ( ( value1.W * num1 ) + ( value2.W * num2 ) ) + ( tangent1.W * num3 ) ) + ( tangent2.W * num4 )
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
        public static void Hermite( ref Vector4 value1, ref Vector4 tangent1, ref Vector4 value2, ref Vector4 tangent2, float amount, out Vector4 result )
        {
            float amountDouble = amount * amount;
            float amountTriple = amount * amountDouble;

            float num1 = ( ( 2f * amountTriple ) - ( 3f * amountDouble ) ) + 1f;
            float num2 = ( -2f * amountTriple ) + ( 3f * amountDouble );
            float num3 = ( amountTriple - ( 2f * amountDouble ) ) + amount;
            float num4 = amountTriple - amountDouble;

            result.X = ( ( ( value1.X * num1 ) + ( value2.X * num2 ) ) + ( tangent1.X * num3 ) ) + ( tangent2.X * num4 );
            result.Y = ( ( ( value1.Y * num1 ) + ( value2.Y * num2 ) ) + ( tangent1.Y * num3 ) ) + ( tangent2.Y * num4 );
            result.Z = ( ( ( value1.Z * num1 ) + ( value2.Z * num2 ) ) + ( tangent1.Z * num3 ) ) + ( tangent2.Z * num4 );
            result.W = ( ( ( value1.W * num1 ) + ( value2.W * num2 ) ) + ( tangent1.W * num3 ) ) + ( tangent2.W * num4 );
        }

        /// <summary>
        /// Transforms a Vector2 by the given Matrix.
        /// </summary>
        /// <param name="position">The source Vector2.</param>
        /// <param name="matrix">The transformation Matrix.</param>
        public static Vector4 Transform( Vector2 position, Matrix matrix )
        {
            return new Vector4
            {
                X = ( ( position.X * matrix.M11 ) + ( position.Y * matrix.M21 ) ) + matrix.M41,
                Y = ( ( position.X * matrix.M12 ) + ( position.Y * matrix.M22 ) ) + matrix.M42,
                Z = ( ( position.X * matrix.M13 ) + ( position.Y * matrix.M23 ) ) + matrix.M43,
                W = ( ( position.X * matrix.M14 ) + ( position.Y * matrix.M24 ) ) + matrix.M44
            };
        }

        /// <summary>
        /// Transforms a Vector2 by the given Matrix.
        /// </summary>
        /// <param name="position">The source Vector2.</param>
        /// <param name="matrix">The transformation Matrix.</param>
        /// <param name="result">[OutAttribute] The Vector4 resulting from the transformation.</param>
        public static void Transform( ref Vector2 position, ref Matrix matrix, out Vector4 result )
        {
            result.X = ( ( position.X * matrix.M11 ) + ( position.Y * matrix.M21 ) ) + matrix.M41;
            result.Y = ( ( position.X * matrix.M12 ) + ( position.Y * matrix.M22 ) ) + matrix.M42;
            result.Z = ( ( position.X * matrix.M13 ) + ( position.Y * matrix.M23 ) ) + matrix.M43;
            result.W = ( ( position.X * matrix.M14 ) + ( position.Y * matrix.M24 ) ) + matrix.M44;
        }

        /// <summary>
        /// Transforms a Vector3 by the given Matrix.
        /// </summary>
        /// <param name="position">The source Vector3.</param>
        /// <param name="matrix">The transformation Matrix.</param>
        public static Vector4 Transform( Vector3 position, Matrix matrix )
        {
            return new Vector4
            {
                X = ( ( ( position.X * matrix.M11 ) + ( position.Y * matrix.M21 ) ) + ( position.Z * matrix.M31 ) ) + matrix.M41,
                Y = ( ( ( position.X * matrix.M12 ) + ( position.Y * matrix.M22 ) ) + ( position.Z * matrix.M32 ) ) + matrix.M42,
                Z = ( ( ( position.X * matrix.M13 ) + ( position.Y * matrix.M23 ) ) + ( position.Z * matrix.M33 ) ) + matrix.M43,
                W = ( ( ( position.X * matrix.M14 ) + ( position.Y * matrix.M24 ) ) + ( position.Z * matrix.M34 ) ) + matrix.M44
            };
        }

        /// <summary>
        /// Transforms a Vector3 by the given Matrix.
        /// </summary>
        /// <param name="position">The source Vector3.</param>
        /// <param name="matrix">The transformation Matrix.</param>
        /// <param name="result">[OutAttribute] The Vector4 resulting from the transformation.</param>
        public static void Transform( ref Vector3 position, ref Matrix matrix, out Vector4 result )
        {
            result.X = ( ( ( position.X * matrix.M11 ) + ( position.Y * matrix.M21 ) ) + ( position.Z * matrix.M31 ) ) + matrix.M41;
            result.Y = ( ( ( position.X * matrix.M12 ) + ( position.Y * matrix.M22 ) ) + ( position.Z * matrix.M32 ) ) + matrix.M42;
            result.Z = ( ( ( position.X * matrix.M13 ) + ( position.Y * matrix.M23 ) ) + ( position.Z * matrix.M33 ) ) + matrix.M43;
            result.W = ( ( ( position.X * matrix.M14 ) + ( position.Y * matrix.M24 ) ) + ( position.Z * matrix.M34 ) ) + matrix.M44;
        }

        /// <summary>
        /// Transforms a Vector4 by the specified Matrix.
        /// </summary>
        /// <param name="vector">The source Vector4.</param>
        /// <param name="matrix">The transformation Matrix.</param>
        public static Vector4 Transform( Vector4 vector, Matrix matrix )
        {
            return new Vector4
            {
                X = ( ( ( vector.X * matrix.M11 ) + ( vector.Y * matrix.M21 ) ) + ( vector.Z * matrix.M31 ) ) + ( vector.W * matrix.M41 ),
                Y = ( ( ( vector.X * matrix.M12 ) + ( vector.Y * matrix.M22 ) ) + ( vector.Z * matrix.M32 ) ) + ( vector.W * matrix.M42 ),
                Z = ( ( ( vector.X * matrix.M13 ) + ( vector.Y * matrix.M23 ) ) + ( vector.Z * matrix.M33 ) ) + ( vector.W * matrix.M43 ),
                W = ( ( ( vector.X * matrix.M14 ) + ( vector.Y * matrix.M24 ) ) + ( vector.Z * matrix.M34 ) ) + ( vector.W * matrix.M44 )
            };
        }

        /// <summary>
        /// Transforms a Vector4 by the given Matrix.
        /// </summary>
        /// <param name="vector">The source Vector4.</param>
        /// <param name="matrix">The transformation Matrix.</param>
        /// <param name="result">[OutAttribute] The Vector4 resulting from the transformation.</param>
        public static void Transform( ref Vector4 vector, ref Matrix matrix, out Vector4 result )
        {
            result.X = ( ( ( vector.X * matrix.M11 ) + ( vector.Y * matrix.M21 ) ) + ( vector.Z * matrix.M31 ) ) + ( vector.W * matrix.M41 );
            result.Y = ( ( ( vector.X * matrix.M12 ) + ( vector.Y * matrix.M22 ) ) + ( vector.Z * matrix.M32 ) ) + ( vector.W * matrix.M42 );
            result.Z = ( ( ( vector.X * matrix.M13 ) + ( vector.Y * matrix.M23 ) ) + ( vector.Z * matrix.M33 ) ) + ( vector.W * matrix.M43 );
            result.W = ( ( ( vector.X * matrix.M14 ) + ( vector.Y * matrix.M24 ) ) + ( vector.Z * matrix.M34 ) ) + ( vector.W * matrix.M44 );
        }

        /// <summary>
        /// Transforms a Vector2 by a specified Quaternion into a Vector4.
        /// </summary>
        /// <param name="value">The Vector2 to transform.</param>
        /// <param name="rotation">The Quaternion rotation to apply.</param>
        public static Vector4 Transform( Vector2 value, Quaternion rotation )
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

            return new Vector4
            {
                X = ( value.X * ( ( 1f - num7 ) - num9 ) ) + ( value.Y * ( num5 - num3 ) ),
                Y = ( value.X * ( num5 + num3 ) ) + ( value.Y * ( ( 1f - num4 ) - num9 ) ),
                Z = ( value.X * ( num6 - num2 ) ) + ( value.Y * ( num8 + num1 ) ),
                W = 1f
            };
        }

        /// <summary>
        /// Transforms a Vector2 by a specified Quaternion into a Vector4.
        /// </summary>
        /// <param name="value">The Vector2 to transform.</param>
        /// <param name="rotation">The Quaternion rotation to apply.</param>
        /// <param name="result">[OutAttribute] The Vector4 resulting from the transformation.</param>
        public static void Transform( ref Vector2 value, ref Quaternion rotation, out Vector4 result )
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

            result.X = ( value.X * ( ( 1f - num7 ) - num9 ) ) + ( value.Y * ( num5 - num3 ) );
            result.Y = ( value.X * ( num5 + num3 ) ) + ( value.Y * ( ( 1f - num4 ) - num9 ) );
            result.Z = ( value.X * ( num6 - num2 ) ) + ( value.Y * ( num8 + num1 ) );
            result.W = 1f;
        }

        /// <summary>
        /// Transforms a Vector3 by a specified Quaternion into a Vector4.
        /// </summary>
        /// <param name="value">The Vector3 to transform.</param>
        /// <param name="rotation">The Quaternion rotation to apply.</param>
        public static Vector4 Transform( Vector3 value, Quaternion rotation )
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

            return new Vector4
            {
                X = ( ( value.X * ( ( 1f - num7 ) - num9 ) ) + ( value.Y * ( num5 - num3 ) ) ) + ( value.Z * ( num6 + num2 ) ),
                Y = ( ( value.X * ( num5 + num3 ) ) + ( value.Y * ( ( 1f - num4 ) - num9 ) ) ) + ( value.Z * ( num8 - num1 ) ),
                Z = ( ( value.X * ( num6 - num2 ) ) + ( value.Y * ( num8 + num1 ) ) ) + ( value.Z * ( ( 1f - num4 ) - num7 ) ),
                W = 1f
            };
        }

        /// <summary>
        /// Transforms a Vector3 by a specified Quaternion into a Vector4.
        /// </summary>
        /// <param name="value">The Vector3 to transform.</param>
        /// <param name="rotation">The Quaternion rotation to apply.</param>
        /// <param name="result">[OutAttribute] The Vector4 resulting from the transformation.</param>
        public static void Transform( ref Vector3 value, ref Quaternion rotation, out Vector4 result )
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
            result.W = 1f;
        }

        /// <summary>
        /// Transforms a Vector4 by a specified Quaternion.
        /// </summary>
        /// <param name="value">The Vector4 to transform.</param>
        /// <param name="rotation">The Quaternion rotation to apply.</param>
        public static Vector4 Transform( Vector4 value, Quaternion rotation )
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

            return new Vector4
            {
                X = ( ( value.X * ( ( 1f - num7 ) - num9 ) ) + ( value.Y * ( num5 - num3 ) ) ) + ( value.Z * ( num6 + num2 ) ),
                Y = ( ( value.X * ( num5 + num3 ) ) + ( value.Y * ( ( 1f - num4 ) - num9 ) ) ) + ( value.Z * ( num8 - num1 ) ),
                Z = ( ( value.X * ( num6 - num2 ) ) + ( value.Y * ( num8 + num1 ) ) ) + ( value.Z * ( ( 1f - num4 ) - num7 ) ),
                W = value.W
            };
        }

        /// <summary>
        /// Transforms a Vector4 by a specified Quaternion.
        /// </summary>
        /// <param name="value">The Vector4 to transform.</param>
        /// <param name="rotation">The Quaternion rotation to apply.</param>
        /// <param name="result">[OutAttribute] The Vector4 resulting from the transformation.</param>
        public static void Transform( ref Vector4 value, ref Quaternion rotation, out Vector4 result )
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
            result.W = value.W;
        }

        /// <summary>
        /// Transforms an array of Vector4s by a specified Matrix.
        /// </summary>
        /// <param name="sourceArray">The array of Vector4s to transform.</param>
        /// <param name="matrix">The transform Matrix to apply.</param>
        /// <param name="destinationArray">The existing destination array into which the transformed Vector4s are written.</param>
        public static void Transform( Vector4[] sourceArray, ref Matrix matrix, Vector4[] destinationArray )
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
                float w = sourceArray[i].W;

                destinationArray[i].X = ( ( ( x * matrix.M11 ) + ( y * matrix.M21 ) ) + ( z * matrix.M31 ) ) + ( w * matrix.M41 );
                destinationArray[i].Y = ( ( ( x * matrix.M12 ) + ( y * matrix.M22 ) ) + ( z * matrix.M32 ) ) + ( w * matrix.M42 );
                destinationArray[i].Z = ( ( ( x * matrix.M13 ) + ( y * matrix.M23 ) ) + ( z * matrix.M33 ) ) + ( w * matrix.M43 );
                destinationArray[i].W = ( ( ( x * matrix.M14 ) + ( y * matrix.M24 ) ) + ( z * matrix.M34 ) ) + ( w * matrix.M44 );
            }
        }

        /// <summary>
        /// Transforms a specified range in an array of Vector4s by a specified Matrix into a specified range in a destination array.
        /// </summary>
        /// <param name="sourceArray">The array of Vector4s containing the range to transform.</param>
        /// <param name="sourceIndex">The index in the source array of the first Vector4 to transform.</param>
        /// <param name="matrix">The transform Matrix to apply.</param>
        /// <param name="destinationArray">The existing destination array of Vector4s into which to write the results.</param>
        /// <param name="destinationIndex">The index in the destination array of the first result Vector4 to write.</param>
        /// <param name="length">The number of Vector4s to transform.</param>
        public static void Transform( Vector4[] sourceArray, int sourceIndex, ref Matrix matrix, Vector4[] destinationArray, int destinationIndex, int length )
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
                float w = sourceArray[sourceIndex].W;

                destinationArray[destinationIndex].X = ( ( ( x * matrix.M11 ) + ( y * matrix.M21 ) ) + ( z * matrix.M31 ) ) + ( w * matrix.M41 );
                destinationArray[destinationIndex].Y = ( ( ( x * matrix.M12 ) + ( y * matrix.M22 ) ) + ( z * matrix.M32 ) ) + ( w * matrix.M42 );
                destinationArray[destinationIndex].Z = ( ( ( x * matrix.M13 ) + ( y * matrix.M23 ) ) + ( z * matrix.M33 ) ) + ( w * matrix.M43 );
                destinationArray[destinationIndex].W = ( ( ( x * matrix.M14 ) + ( y * matrix.M24 ) ) + ( z * matrix.M34 ) ) + ( w * matrix.M44 );

                sourceIndex++;
                destinationIndex++;
                length--;
            }
        }

        /// <summary>
        /// Transforms an array of Vector4s by a specified Quaternion.
        /// </summary>
        /// <param name="sourceArray">The array of Vector4s to transform.</param>
        /// <param name="rotation">The Quaternion rotation to apply.</param>
        /// <param name="destinationArray">The existing destination array into which the transformed Vector4s are written.</param>
        public static void Transform( Vector4[] sourceArray, ref Quaternion rotation, Vector4[] destinationArray )
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
                destinationArray[i].W = sourceArray[i].W;
            }
        }

        /// <summary>
        /// Transforms a specified range in an array of Vector4s by a specified Quaternion into a specified range in a destination array.
        /// </summary>
        /// <param name="sourceArray">The array of Vector4s containing the range to transform.</param>
        /// <param name="sourceIndex">The index in the source array of the first Vector4 to transform.</param>
        /// <param name="rotation">The Quaternion rotation to apply.</param>
        /// <param name="destinationArray">The existing destination array of Vector4s into which to write the results.</param>
        /// <param name="destinationIndex">The index in the destination array of the first result Vector4 to write.</param>
        /// <param name="length">The number of Vector4s to transform.</param>
        public static void Transform( Vector4[] sourceArray, int sourceIndex, ref Quaternion rotation, Vector4[] destinationArray, int destinationIndex, int length )
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
                float w = sourceArray[sourceIndex].W;

                destinationArray[destinationIndex].X = ( ( x * result1 ) + ( y * result2 ) ) + ( z * result3 );
                destinationArray[destinationIndex].Y = ( ( x * result4 ) + ( y * result5 ) ) + ( z * result6 );
                destinationArray[destinationIndex].Z = ( ( x * result7 ) + ( y * result8 ) ) + ( z * result9 );
                destinationArray[destinationIndex].W = w;

                sourceIndex++;
                destinationIndex++;
                length--;
            }
        }

        /// <summary>
        /// Returns a vector pointing in the opposite direction.
        /// </summary>
        /// <param name="value">Source vector.</param>
        public static Vector4 Negate( Vector4 value )
        {
            return new Vector4 { X = -value.X, Y = -value.Y, Z = -value.Z, W = -value.W };
        }

        /// <summary>
        /// Returns a vector pointing in the opposite direction.
        /// </summary>
        /// <param name="value">Source vector.</param>
        /// <param name="result">[OutAttribute] Vector pointing in the opposite direction.</param>
        public static void Negate( ref Vector4 value, out Vector4 result )
        {
            result.X = -value.X;
            result.Y = -value.Y;
            result.Z = -value.Z;
            result.W = -value.W;
        }

        /// <summary>
        /// Adds two vectors.
        /// </summary>
        /// <param name="value1">Source vector.</param>
        /// <param name="value2">Source vector.</param>
        public static Vector4 Add( Vector4 value1, Vector4 value2 )
        {
            return new Vector4 { X = value1.X + value2.X, Y = value1.Y + value2.Y, Z = value1.Z + value2.Z, W = value1.W + value2.W };
        }

        /// <summary>
        /// Adds two vectors.
        /// </summary>
        /// <param name="value1">Source vector.</param>
        /// <param name="value2">Source vector.</param>
        /// <param name="result">[OutAttribute] Sum of the source vectors.</param>
        public static void Add( ref Vector4 value1, ref Vector4 value2, out Vector4 result )
        {
            result.X = value1.X + value2.X;
            result.Y = value1.Y + value2.Y;
            result.Z = value1.Z + value2.Z;
            result.W = value1.W + value2.W;
        }

        /// <summary>
        /// Subtracts a vector from a vector.
        /// </summary>
        /// <param name="value1">Source vector.</param>
        /// <param name="value2">Source vector.</param>
        public static Vector4 Subtract( Vector4 value1, Vector4 value2 )
        {
            return new Vector4 { X = value1.X - value2.X, Y = value1.Y - value2.Y, Z = value1.Z - value2.Z, W = value1.W - value2.W };
        }

        /// <summary>
        /// Subtracts a vector from a vector.
        /// </summary>
        /// <param name="value1">Source vector.</param>
        /// <param name="value2">Source vector.</param>
        /// <param name="result">[OutAttribute] The result of the subtraction.</param>
        public static void Subtract( ref Vector4 value1, ref Vector4 value2, out Vector4 result )
        {
            result.X = value1.X - value2.X;
            result.Y = value1.Y - value2.Y;
            result.Z = value1.Z - value2.Z;
            result.W = value1.W - value2.W;
        }

        /// <summary>
        /// Multiplies the components of two vectors by each other.
        /// </summary>
        /// <param name="value1">Source vector.</param>
        /// <param name="value2">Source vector.</param>
        public static Vector4 Multiply( Vector4 value1, Vector4 value2 )
        {
            return new Vector4 { X = value1.X * value2.X, Y = value1.Y * value2.Y, Z = value1.Z * value2.Z, W = value1.W * value2.W };
        }

        /// <summary>
        /// Multiplies the components of two vectors by each other.
        /// </summary>
        /// <param name="value1">Source vector.</param>
        /// <param name="value2">Source vector.</param>
        /// <param name="result">[OutAttribute] The result of the multiplication.</param>
        public static void Multiply( ref Vector4 value1, ref Vector4 value2, out Vector4 result )
        {
            result.X = value1.X * value2.X;
            result.Y = value1.Y * value2.Y;
            result.Z = value1.Z * value2.Z;
            result.W = value1.W * value2.W;
        }

        /// <summary>
        /// Multiplies a vector by a scalar.
        /// </summary>
        /// <param name="value1">Source vector.</param>
        /// <param name="scaleFactor">Scalar value.</param>
        public static Vector4 Multiply( Vector4 value1, float scaleFactor )
        {
            return new Vector4 { X = value1.X * scaleFactor, Y = value1.Y * scaleFactor, Z = value1.Z * scaleFactor, W = value1.W * scaleFactor };
        }

        /// <summary>
        /// Multiplies a vector by a scalar value.
        /// </summary>
        /// <param name="value1">Source vector.</param>
        /// <param name="scaleFactor">Scalar value.</param>
        /// <param name="result">[OutAttribute] The result of the multiplication.</param>
        public static void Multiply( ref Vector4 value1, float scaleFactor, out Vector4 result )
        {
            result.X = value1.X * scaleFactor;
            result.Y = value1.Y * scaleFactor;
            result.Z = value1.Z * scaleFactor;
            result.W = value1.W * scaleFactor;
        }

        /// <summary>
        /// Divides the components of a vector by the components of another vector.
        /// </summary>
        /// <param name="value1">Source vector.</param>
        /// <param name="value2">Divisor vector.</param>
        public static Vector4 Divide( Vector4 value1, Vector4 value2 )
        {
            return new Vector4 { X = value1.X / value2.X, Y = value1.Y / value2.Y, Z = value1.Z / value2.Z, W = value1.W / value2.W };
        }

        /// <summary>
        /// Divides the components of a vector by the components of another vector.
        /// </summary>
        /// <param name="value1">Source vector.</param>
        /// <param name="value2">The divisor.</param>
        /// <param name="result">[OutAttribute] The result of the division.</param>
        public static void Divide( ref Vector4 value1, ref Vector4 value2, out Vector4 result )
        {
            result.X = value1.X / value2.X;
            result.Y = value1.Y / value2.Y;
            result.Z = value1.Z / value2.Z;
            result.W = value1.W / value2.W;
        }

        /// <summary>
        /// Divides a vector by a scalar value.
        /// </summary>
        /// <param name="value1">Source vector.</param>
        /// <param name="divider">The divisor.</param>
        public static Vector4 Divide( Vector4 value1, float divider )
        {
            float num = 1f / divider;

            return new Vector4 { X = value1.X * num, Y = value1.Y * num, Z = value1.Z * num, W = value1.W * num };
        }

        /// <summary>
        /// Divides a vector by a scalar value.
        /// </summary>
        /// <param name="value1">Source vector.</param>
        /// <param name="divider">The divisor.</param>
        /// <param name="result">[OutAttribute] The result of the division.</param>
        public static void Divide( ref Vector4 value1, float divider, out Vector4 result )
        {
            float num = 1f / divider;

            result.X = value1.X * num;
            result.Y = value1.Y * num;
            result.Z = value1.Z * num;
            result.W = value1.W * num;
        }

        #endregion

        #region zh-CHS 共有静态方法 | en Public Static Methods

        /// <summary>
        /// Returns a vector pointing in the opposite direction.
        /// </summary>
        /// <param name="value">Source vector.</param>
        public static Vector4 operator -( Vector4 value )
        {
            return new Vector4 { X = -value.X, Y = -value.Y, Z = -value.Z, W = -value.W };
        }

        /// <summary>
        /// Tests vectors for equality.
        /// </summary>
        /// <param name="value1">Source vector.</param>
        /// <param name="value2">Source vector.</param>
        public static bool operator ==( Vector4 value1, Vector4 value2 )
        {
            return ( ( ( ( value1.X == value2.X ) && ( value1.Y == value2.Y ) ) && ( value1.Z == value2.Z ) ) && ( value1.W == value2.W ) );
        }

        /// <summary>
        /// Tests vectors for inequality.
        /// </summary>
        /// <param name="value1">Vector to compare.</param>
        /// <param name="value2">Vector to compare.</param>
        public static bool operator !=( Vector4 value1, Vector4 value2 )
        {
            if ( ( ( value1.X == value2.X ) && ( value1.Y == value2.Y ) ) && ( value1.Z == value2.Z ) )
                return !( value1.W == value2.W );

            return true;
        }

        /// <summary>
        /// Adds two vectors.
        /// </summary>
        /// <param name="value1">Source vector.</param>
        /// <param name="value2">Source vector.</param>
        public static Vector4 operator +( Vector4 value1, Vector4 value2 )
        {
            return new Vector4 { X = value1.X + value2.X, Y = value1.Y + value2.Y, Z = value1.Z + value2.Z, W = value1.W + value2.W };
        }

        /// <summary>
        /// Subtracts a vector from a vector.
        /// </summary>
        /// <param name="value1">Source vector.</param>
        /// <param name="value2">Source vector.</param>
        public static Vector4 operator -( Vector4 value1, Vector4 value2 )
        {
            return new Vector4 { X = value1.X - value2.X, Y = value1.Y - value2.Y, Z = value1.Z - value2.Z, W = value1.W - value2.W };
        }

        /// <summary>
        /// Multiplies the components of two vectors by each other.
        /// </summary>
        /// <param name="value1">Source vector.</param>
        /// <param name="value2">Source vector.</param>
        public static Vector4 operator *( Vector4 value1, Vector4 value2 )
        {
            return new Vector4 { X = value1.X * value2.X, Y = value1.Y * value2.Y, Z = value1.Z * value2.Z, W = value1.W * value2.W };
        }

        /// <summary>
        /// Multiplies a vector by a scalar.
        /// </summary>
        /// <param name="value1">Source vector.</param>
        /// <param name="scaleFactor">Scalar value.</param>
        public static Vector4 operator *( Vector4 value1, float scaleFactor )
        {
            return new Vector4 { X = value1.X * scaleFactor, Y = value1.Y * scaleFactor, Z = value1.Z * scaleFactor, W = value1.W * scaleFactor };
        }

        /// <summary>
        /// Multiplies a vector by a scalar.
        /// </summary>
        /// <param name="scaleFactor">Scalar value.</param>
        /// <param name="value1">Source vector.</param>
        public static Vector4 operator *( float scaleFactor, Vector4 value1 )
        {
            return new Vector4 { X = value1.X * scaleFactor, Y = value1.Y * scaleFactor, Z = value1.Z * scaleFactor, W = value1.W * scaleFactor };
        }

        /// <summary>
        /// Divides the components of a vector by the components of another vector.
        /// </summary>
        /// <param name="value1">Source vector.</param>
        /// <param name="value2">Divisor vector.</param>
        public static Vector4 operator /( Vector4 value1, Vector4 value2 )
        {
            return new Vector4 { X = value1.X / value2.X, Y = value1.Y / value2.Y, Z = value1.Z / value2.Z, W = value1.W / value2.W };
        }

        /// <summary>
        /// Divides a vector by a scalar value.
        /// </summary>
        /// <param name="value1">Source vector.</param>
        /// <param name="divider">The divisor.</param>
        public static Vector4 operator /( Vector4 value1, float divider )
        {
            float num = 1f / divider;

            return new Vector4 { X = value1.X * num, Y = value1.Y * num, Z = value1.Z * num, W = value1.W * num };
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="vector3"></param>
        /// <returns></returns>
        public static explicit operator Vector4( Vector3 vector3 )
        {
            return new Vector4( vector3, 0 );
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="vector2"></param>
        /// <returns></returns>
        public static explicit operator Vector4( Vector2 vector2 )
        {
            return new Vector4( vector2, 0, 0 );
        }

        #endregion

        #region zh-CHS 方法覆盖 | en Override Methods

        /// <summary>
        /// Retrieves a string representation of the current object.
        /// </summary>
        public override string ToString()
        {
            return string.Format( CultureInfo.CurrentCulture, "{{X:{0} Y:{1} Z:{2} W:{3}}}", new object[] { X.ToString( CultureInfo.CurrentCulture ), Y.ToString( CultureInfo.CurrentCulture ), Z.ToString( CultureInfo.CurrentCulture ), W.ToString( CultureInfo.CurrentCulture ) } );
        }


        /// <summary>
        /// Returns a value that indicates whether the current instance is equal to a specified object.
        /// </summary>
        /// <param name="obj">Object with which to make the comparison.</param>
        public override bool Equals( object obj )
        {
            if ( obj is Vector4 )
                return Equals( (Vector4)obj );
            else
                return false;
        }

        /// <summary>
        /// Gets the hash code of this object.
        /// </summary>
        public override int GetHashCode()
        {
            return ( ( ( X.GetHashCode() + Y.GetHashCode() ) + Z.GetHashCode() ) + W.GetHashCode() );
        }

        #endregion

        #region zh-CHS 接口实现 | en Interface Implementation

        /// <summary>
        /// Determines whether the specified Object is equal to the Vector4.
        /// </summary>
        /// <param name="other">The Vector4 to compare with the current Vector4.</param>
        public bool Equals( Vector4 other )
        {
            return ( ( ( ( X == other.X ) && ( Y == other.Y ) ) && ( Z == other.Z ) ) && ( W == other.W ) );
        }

        #endregion

    }
}
#endregion
