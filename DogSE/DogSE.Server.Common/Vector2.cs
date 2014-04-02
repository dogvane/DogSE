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
    /// Defines a vector with two components.
    /// </summary>
    public struct Vector2 : IEquatable<Vector2>
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

        #endregion

        #region zh-CHS 共有静态属性 | en Public Static Properties

        #region zh-CHS 私有静态成员变量 | en Private Static Member Variables
        /// <summary>
        /// 
        /// </summary>
        private static Vector2 m_Zero = new Vector2();
        #endregion
        /// <summary>
        /// Returns a Vector2 with all of its components set to zero.
        /// </summary>
        public static Vector2 Zero
        {
            get { return m_Zero; }
        }

        #region zh-CHS 私有静态成员变量 | en Private Static Member Variables
        /// <summary>
        /// 
        /// </summary>
        private static Vector2 m_One = new Vector2( 1f, 1f );
        #endregion
        /// <summary>
        /// Returns a Vector2 with both of its components set to one.
        /// </summary>
        public static Vector2 One
        {
            get { return m_One; }
        }

        #region zh-CHS 私有静态成员变量 | en Private Static Member Variables
        /// <summary>
        /// 
        /// </summary>
        private static Vector2 m_UnitX = new Vector2( 1f, 0f );
        #endregion
        /// <summary>
        /// Returns the unit vector for the x-axis.
        /// </summary>
        public static Vector2 UnitX
        {
            get { return m_UnitX; }
        }

        #region zh-CHS 私有静态成员变量 | en Private Static Member Variables
        /// <summary>
        /// 
        /// </summary>
        private static Vector2 m_UnitY = new Vector2( 0f, 1f );
        #endregion
        /// <summary>
        /// Returns the unit vector for the y-axis.
        /// </summary>
        public static Vector2 UnitY
        {
            get { return m_UnitY; }
        }

        #endregion

        #region zh-CHS 构造和初始化和清理 | en Constructors and Initializers and Dispose

        /// <summary>
        /// Initializes a new instance of Vector2.
        /// </summary>
        /// <param name="x">Initial value for the x-component of the vector.</param>
        /// <param name="y">Initial value for the y-component of the vector.</param>
        public Vector2( float x, float y )
        {
            X = x;
            Y = y;
        }

        /// <summary>
        /// Creates a new instance of Vector2.
        /// </summary>
        /// <param name="value">Value to initialize both components to.</param>
        public Vector2( float value )
        {
            X = Y = value;
        }

        #endregion

        #region zh-CHS 共有方法 | en Public Methods

        /// <summary>
        /// Calculates the length of the vector.
        /// </summary>
        public float Length()
        {
            return (float)Math.Sqrt( (double)( X * X ) + ( Y * Y ) );
        }

        /// <summary>
        /// Calculates the length of the vector squared.
        /// </summary>
        public float LengthSquared()
        {
            return ( ( X * X ) + ( Y * Y ) );
        }

        /// <summary>
        /// Turns the current vector into a unit vector. The result is a vector one unit in length pointing in the same direction as the original vector.
        /// </summary>
        public void Normalize()
        {
            float num = 1f / ( (float)Math.Sqrt( (double)( X * X ) + ( Y * Y ) ) );

            X *= num;
            Y *= num;
        }

        #endregion

        #region zh-CHS 共有静态方法 | en Public Static Methods

        /// <summary>
        /// Calculates the distance between two vectors.
        /// </summary>
        /// <param name="value1">Source vector.</param>
        /// <param name="value2">Source vector.</param>
        public static float Distance( Vector2 value1, Vector2 value2 )
        {
            float numX = value1.X - value2.X;
            float numY = value1.Y - value2.Y;

            return (float)Math.Sqrt( (double)( numX * numX ) + ( numY * numY ) );
        }

        /// <summary>
        /// Calculates the distance between two vectors.
        /// </summary>
        /// <param name="value1">Source vector.</param>
        /// <param name="value2">Source vector.</param>
        /// <param name="result">[OutAttribute] The distance between the vectors.</param>
        public static void Distance( ref Vector2 value1, ref Vector2 value2, out float result )
        {
            float numX = value1.X - value2.X;
            float numY = value1.Y - value2.Y;

            result = (float)Math.Sqrt( (double)( numX * numX ) + ( numY * numY ) );
        }

        /// <summary>
        /// Calculates the distance between two vectors squared.
        /// </summary>
        /// <param name="value1">Source vector.</param>
        /// <param name="value2">Source vector.</param>
        public static float DistanceSquared( Vector2 value1, Vector2 value2 )
        {
            float numX = value1.X - value2.X;
            float numY = value1.Y - value2.Y;

            return ( ( numX * numX ) + ( numY * numY ) );
        }

        /// <summary>
        /// Calculates the distance between two vectors squared.
        /// </summary>
        /// <param name="value1">Source vector.</param>
        /// <param name="value2">Source vector.</param>
        /// <param name="result">[OutAttribute] The distance between the vectors squared.</param>
        public static void DistanceSquared( ref Vector2 value1, ref Vector2 value2, out float result )
        {
            float numX = value1.X - value2.X;
            float numY = value1.Y - value2.Y;

            result = ( numX * numX ) + ( numY * numY );
        }

        /// <summary>
        /// Calculates the dot product of two vectors. If the two vectors are unit vectors, the dot product returns a floating point value between -1 and 1 that can be used to determine some properties of the angle between two vectors. For example, it can show whether the vectors are orthogonal, parallel, or have an acute or obtuse angle between them.
        /// </summary>
        /// <param name="value1">Source vector.</param>
        /// <param name="value2">Source vector.</param>
        public static float Dot( Vector2 value1, Vector2 value2 )
        {
            return ( ( value1.X * value2.X ) + ( value1.Y * value2.Y ) );
        }

        /// <summary>
        /// Calculates the dot product of two vectors and writes the result to a user-specified variable. If the two vectors are unit vectors, the dot product returns a floating point value between -1 and 1 that can be used to determine some properties of the angle between two vectors. For example, it can show whether the vectors are orthogonal, parallel, or have an acute or obtuse angle between them.
        /// </summary>
        /// <param name="value1">Source vector.</param>
        /// <param name="value2">Source vector.</param>
        /// <param name="result">[OutAttribute] The dot product of the two vectors.</param>
        public static void Dot( ref Vector2 value1, ref Vector2 value2, out float result )
        {
            result = ( value1.X * value2.X ) + ( value1.Y * value2.Y );
        }

        /// <summary>
        /// Creates a unit vector from the specified vector. The result is a vector one unit in length pointing in the same direction as the original vector.
        /// </summary>
        /// <param name="value">Source Vector2.</param>
        public static Vector2 Normalize( Vector2 value )
        {
            float num = 1f / ( (float)Math.Sqrt( (double)( value.X * value.X ) + ( value.Y * value.Y ) ) );

            return new Vector2 { X = value.X * num, Y = value.Y * num };
        }

        /// <summary>
        /// Creates a unit vector from the specified vector, writing the result to a user-specified variable. The result is a vector one unit in length pointing in the same direction as the original vector.
        /// </summary>
        /// <param name="value">Source vector.</param>
        /// <param name="result">[OutAttribute] Normalized vector.</param>
        public static void Normalize( ref Vector2 value, out Vector2 result )
        {
            float num = 1f / ( (float)Math.Sqrt( (double)( value.X * value.X ) + ( value.Y * value.Y ) ) );

            result.X = value.X * num;
            result.Y = value.Y * num;
        }

        /// <summary>
        /// Determines the reflect vector of the given vector and normal.
        /// </summary>
        /// <param name="vector">Source vector.</param>
        /// <param name="normal">Normal of vector.</param>
        public static Vector2 Reflect( Vector2 vector, Vector2 normal )
        {
            float num = ( vector.X * normal.X ) + ( vector.Y * normal.Y );

            return new Vector2 { X = vector.X - ( ( 2f * num ) * normal.X ), Y = vector.Y - ( ( 2f * num ) * normal.Y ) };
        }

        /// <summary>
        /// Determines the reflect vector of the given vector and normal.
        /// </summary>
        /// <param name="vector">Source vector.</param>
        /// <param name="normal">Normal of vector.</param>
        /// <param name="result">[OutAttribute] The created reflect vector.</param>
        public static void Reflect( ref Vector2 vector, ref Vector2 normal, out Vector2 result )
        {
            float num = ( vector.X * normal.X ) + ( vector.Y * normal.Y );

            result.X = vector.X - ( ( 2f * num ) * normal.X );
            result.Y = vector.Y - ( ( 2f * num ) * normal.Y );
        }

        /// <summary>
        /// Returns a vector that contains the lowest value from each matching pair of components.
        /// </summary>
        /// <param name="value1">Source vector.</param>
        /// <param name="value2">Source vector.</param>
        public static Vector2 Min( Vector2 value1, Vector2 value2 )
        {
            return new Vector2 { X = ( value1.X < value2.X ) ? value1.X : value2.X, Y = ( value1.Y < value2.Y ) ? value1.Y : value2.Y };
        }

        /// <summary>
        /// Returns a vector that contains the lowest value from each matching pair of components.
        /// </summary>
        /// <param name="value1">Source vector.</param>
        /// <param name="value2">Source vector.</param>
        /// <param name="result">[OutAttribute] The minimized vector.</param>
        public static void Min( ref Vector2 value1, ref Vector2 value2, out Vector2 result )
        {
            result.X = ( value1.X < value2.X ) ? value1.X : value2.X;
            result.Y = ( value1.Y < value2.Y ) ? value1.Y : value2.Y;
        }

        /// <summary>
        /// Returns a vector that contains the highest value from each matching pair of components.
        /// </summary>
        /// <param name="value1">Source vector.</param>
        /// <param name="value2">Source vector.</param>
        public static Vector2 Max( Vector2 value1, Vector2 value2 )
        {
            return new Vector2 { X = ( value1.X > value2.X ) ? value1.X : value2.X, Y = ( value1.Y > value2.Y ) ? value1.Y : value2.Y };
        }

        /// <summary>
        /// Returns a vector that contains the highest value from each matching pair of components.
        /// </summary>
        /// <param name="value1">Source vector.</param>
        /// <param name="value2">Source vector.</param>
        /// <param name="result">[OutAttribute] The maximized vector.</param>
        public static void Max( ref Vector2 value1, ref Vector2 value2, out Vector2 result )
        {
            result.X = ( value1.X > value2.X ) ? value1.X : value2.X;
            result.Y = ( value1.Y > value2.Y ) ? value1.Y : value2.Y;
        }

        /// <summary>
        /// Restricts a value to be within a specified range.
        /// </summary>
        /// <param name="value1">The value to clamp.</param>
        /// <param name="min">The minimum value.</param>
        /// <param name="max">The maximum value.</param>
        public static Vector2 Clamp( Vector2 value1, Vector2 min, Vector2 max )
        {
            float x = value1.X;
            x = ( x > max.X ) ? max.X : x;
            x = ( x < min.X ) ? min.X : x;

            float y = value1.Y;
            y = ( y > max.Y ) ? max.Y : y;
            y = ( y < min.Y ) ? min.Y : y;

            return new Vector2 { X = x, Y = y };
        }

        /// <summary>
        /// Restricts a value to be within a specified range.
        /// </summary>
        /// <param name="value1">The value to clamp.</param>
        /// <param name="min">The minimum value.</param>
        /// <param name="max">The maximum value.</param>
        /// <param name="result">[OutAttribute] The clamped value.</param>
        public static void Clamp( ref Vector2 value1, ref Vector2 min, ref Vector2 max, out Vector2 result )
        {
            float x = value1.X;
            x = ( x > max.X ) ? max.X : x;
            x = ( x < min.X ) ? min.X : x;

            float y = value1.Y;
            y = ( y > max.Y ) ? max.Y : y;
            y = ( y < min.Y ) ? min.Y : y;

            result.X = x;
            result.Y = y;
        }

        /// <summary>
        /// Performs a linear interpolation between two vectors.
        /// </summary>
        /// <param name="value1">Source vector.</param>
        /// <param name="value2">Source vector.</param>
        /// <param name="amount">Value between 0 and 1 indicating the weight of value2.</param>
        public static Vector2 Lerp( Vector2 value1, Vector2 value2, float amount )
        {
            return new Vector2 { X = value1.X + ( ( value2.X - value1.X ) * amount ), Y = value1.Y + ( ( value2.Y - value1.Y ) * amount ) };
        }

        /// <summary>
        /// Performs a linear interpolation between two vectors.
        /// </summary>
        /// <param name="value1">Source vector.</param>
        /// <param name="value2">Source vector.</param>
        /// <param name="amount">Value between 0 and 1 indicating the weight of value2.</param>
        /// <param name="result">[OutAttribute] The result of the interpolation.</param>
        public static void Lerp( ref Vector2 value1, ref Vector2 value2, float amount, out Vector2 result )
        {
            result.X = value1.X + ( ( value2.X - value1.X ) * amount );
            result.Y = value1.Y + ( ( value2.Y - value1.Y ) * amount );
        }

        /// <summary>
        /// Returns a Vector2 containing the 2D Cartesian coordinates of a point specified in barycentric (areal) coordinates relative to a 2D triangle.
        /// </summary>
        /// <param name="value1">A Vector2 containing the 2D Cartesian coordinates of vertex 1 of the triangle.</param>
        /// <param name="value2">A Vector2 containing the 2D Cartesian coordinates of vertex 2 of the triangle.</param>
        /// <param name="value3">A Vector2 containing the 2D Cartesian coordinates of vertex 3 of the triangle.</param>
        /// <param name="amount1">Barycentric coordinate b2, which expresses the weighting factor toward vertex 2 (specified in value2).</param>
        /// <param name="amount2">Barycentric coordinate b3, which expresses the weighting factor toward vertex 3 (specified in value3).</param>
        public static Vector2 Barycentric( Vector2 value1, Vector2 value2, Vector2 value3, float amount1, float amount2 )
        {
            return new Vector2 { X = ( value1.X + ( amount1 * ( value2.X - value1.X ) ) ) + ( amount2 * ( value3.X - value1.X ) ), Y = ( value1.Y + ( amount1 * ( value2.Y - value1.Y ) ) ) + ( amount2 * ( value3.Y - value1.Y ) ) };
        }

        /// <summary>
        /// Returns a Vector2 containing the 2D Cartesian coordinates of a point specified in barycentric (areal) coordinates relative to a 2D triangle.
        /// </summary>
        /// <param name="value1">A Vector2 containing the 2D Cartesian coordinates of vertex 1 of the triangle.</param>
        /// <param name="value2">A Vector2 containing the 2D Cartesian coordinates of vertex 2 of the triangle.</param>
        /// <param name="value3">A Vector2 containing the 2D Cartesian coordinates of vertex 3 of the triangle.</param>
        /// <param name="amount1">Barycentric coordinate b2, which expresses the weighting factor toward vertex 2 (specified in value2).</param>
        /// <param name="amount2">Barycentric coordinate b3, which expresses the weighting factor toward vertex 3 (specified in value3).</param>
        /// <param name="result">[OutAttribute] The 2D Cartesian coordinates of the specified point are placed in this Vector2 on exit.</param>
        public static void Barycentric( ref Vector2 value1, ref Vector2 value2, ref Vector2 value3, float amount1, float amount2, out Vector2 result )
        {
            result.X = ( value1.X + ( amount1 * ( value2.X - value1.X ) ) ) + ( amount2 * ( value3.X - value1.X ) );
            result.Y = ( value1.Y + ( amount1 * ( value2.Y - value1.Y ) ) ) + ( amount2 * ( value3.Y - value1.Y ) );
        }

        /// <summary>
        /// Interpolates between two values using a cubic equation.
        /// </summary>
        /// <param name="value1">Source value.</param>
        /// <param name="value2">Source value.</param>
        /// <param name="amount">Weighting value.</param>
        public static Vector2 SmoothStep( Vector2 value1, Vector2 value2, float amount )
        {
            amount = ( amount > 1f ) ? 1f : ( ( amount < 0f ) ? 0f : amount );
            amount = ( amount * amount ) * ( 3f - ( 2f * amount ) );

            return new Vector2 { X = value1.X + ( ( value2.X - value1.X ) * amount ), Y = value1.Y + ( ( value2.Y - value1.Y ) * amount ) };
        }

        /// <summary>
        /// Interpolates between two values using a cubic equation.
        /// </summary>
        /// <param name="value1">Source value.</param>
        /// <param name="value2">Source value.</param>
        /// <param name="amount">Weighting value.</param>
        /// <param name="result">[OutAttribute] The interpolated value.</param>
        public static void SmoothStep( ref Vector2 value1, ref Vector2 value2, float amount, out Vector2 result )
        {
            amount = ( amount > 1f ) ? 1f : ( ( amount < 0f ) ? 0f : amount );
            amount = ( amount * amount ) * ( 3f - ( 2f * amount ) );

            result.X = value1.X + ( ( value2.X - value1.X ) * amount );
            result.Y = value1.Y + ( ( value2.Y - value1.Y ) * amount );
        }

        /// <summary>
        /// Performs a Catmull-Rom interpolation using the specified positions.
        /// </summary>
        /// <param name="value1">The first position in the interpolation.</param>
        /// <param name="value2">The second position in the interpolation.</param>
        /// <param name="value3">The third position in the interpolation.</param>
        /// <param name="value4">The fourth position in the interpolation.</param>
        /// <param name="amount">Weighting factor.</param>
        public static Vector2 CatmullRom( Vector2 value1, Vector2 value2, Vector2 value3, Vector2 value4, float amount )
        {
            float amountDouble = amount * amount;
            float amountTriple = amount * amountDouble;

            return new Vector2
            {
                X = 0.5f * ( ( ( ( 2f * value2.X ) + ( ( -value1.X + value3.X ) * amount ) ) + ( ( ( ( ( 2f * value1.X ) - ( 5f * value2.X ) ) + ( 4f * value3.X ) ) - value4.X ) * amountDouble ) ) + ( ( ( ( -value1.X + ( 3f * value2.X ) ) - ( 3f * value3.X ) ) + value4.X ) * amountTriple ) ),
                Y = 0.5f * ( ( ( ( 2f * value2.Y ) + ( ( -value1.Y + value3.Y ) * amount ) ) + ( ( ( ( ( 2f * value1.Y ) - ( 5f * value2.Y ) ) + ( 4f * value3.Y ) ) - value4.Y ) * amountDouble ) ) + ( ( ( ( -value1.Y + ( 3f * value2.Y ) ) - ( 3f * value3.Y ) ) + value4.Y ) * amountTriple ) )
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
        public static void CatmullRom( ref Vector2 value1, ref Vector2 value2, ref Vector2 value3, ref Vector2 value4, float amount, out Vector2 result )
        {
            float amountDouble = amount * amount;
            float amountTriple = amount * amountDouble;

            result.X = 0.5f * ( ( ( ( 2f * value2.X ) + ( ( -value1.X + value3.X ) * amount ) ) + ( ( ( ( ( 2f * value1.X ) - ( 5f * value2.X ) ) + ( 4f * value3.X ) ) - value4.X ) * amountDouble ) ) + ( ( ( ( -value1.X + ( 3f * value2.X ) ) - ( 3f * value3.X ) ) + value4.X ) * amountTriple ) );
            result.Y = 0.5f * ( ( ( ( 2f * value2.Y ) + ( ( -value1.Y + value3.Y ) * amount ) ) + ( ( ( ( ( 2f * value1.Y ) - ( 5f * value2.Y ) ) + ( 4f * value3.Y ) ) - value4.Y ) * amountDouble ) ) + ( ( ( ( -value1.Y + ( 3f * value2.Y ) ) - ( 3f * value3.Y ) ) + value4.Y ) * amountTriple ) );
        }

        /// <summary>
        /// Performs a Hermite spline interpolation.
        /// </summary>
        /// <param name="value1">Source position vector.</param>
        /// <param name="tangent1">Source tangent vector.</param>
        /// <param name="value2">Source position vector.</param>
        /// <param name="tangent2">Source tangent vector.</param>
        /// <param name="amount">Weighting factor.</param>
        public static Vector2 Hermite( Vector2 value1, Vector2 tangent1, Vector2 value2, Vector2 tangent2, float amount )
        {
            float amountDouble = amount * amount;
            float amountTriple = amount * amountDouble;

            float num1 = amountTriple - amountDouble;
            float num2 = ( amountTriple - ( 2f * amountDouble ) ) + amount;
            float num3 = ( -2f * amountTriple ) + ( 3f * amountDouble );
            float num4 = ( ( 2f * amountTriple ) - ( 3f * amountDouble ) ) + 1f;

            return new Vector2
            {
                X = ( ( ( value1.X * num4 ) + ( value2.X * num3 ) ) + ( tangent1.X * num2 ) ) + ( tangent2.X * num1 ),
                Y = ( ( ( value1.Y * num4 ) + ( value2.Y * num3 ) ) + ( tangent1.Y * num2 ) ) + ( tangent2.Y * num1 )
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
        public static void Hermite( ref Vector2 value1, ref Vector2 tangent1, ref Vector2 value2, ref Vector2 tangent2, float amount, out Vector2 result )
        {
            float amountDouble = amount * amount;
            float amountTriple = amount * amountDouble;

            float num1 = amountTriple - amountDouble;
            float num2 = ( amountTriple - ( 2f * amountDouble ) ) + amount;
            float num3 = ( -2f * amountTriple ) + ( 3f * amountDouble );
            float num4 = ( ( 2f * amountTriple ) - ( 3f * amountDouble ) ) + 1f;

            result.X = ( ( ( value1.X * num4 ) + ( value2.X * num3 ) ) + ( tangent1.X * num2 ) ) + ( tangent2.X * num1 );
            result.Y = ( ( ( value1.Y * num4 ) + ( value2.Y * num3 ) ) + ( tangent1.Y * num2 ) ) + ( tangent2.Y * num1 );
        }

        /// <summary>
        /// Transforms the vector (x, y, 0, 1) by the specified matrix.
        /// </summary>
        /// <param name="position">The source vector.</param>
        /// <param name="matrix">The transformation matrix.</param>
        public static Vector2 Transform( Vector2 position, Matrix matrix )
        {
            float numX = ( ( position.X * matrix.M11 ) + ( position.Y * matrix.M21 ) ) + matrix.M41;
            float numY = ( ( position.X * matrix.M12 ) + ( position.Y * matrix.M22 ) ) + matrix.M42;

            return new Vector2 { X = numX, Y = numY };
        }

        /// <summary>
        /// Transforms a Vector2 by the given Matrix.
        /// </summary>
        /// <param name="position">The source Vector2.</param>
        /// <param name="matrix">The transformation Matrix.</param>
        /// <param name="result">[OutAttribute] The Vector2 resulting from the transformation.</param>
        public static void Transform( ref Vector2 position, ref Matrix matrix, out Vector2 result )
        {
            result.X = ( ( position.X * matrix.M11 ) + ( position.Y * matrix.M21 ) ) + matrix.M41;
            result.Y = ( ( position.X * matrix.M12 ) + ( position.Y * matrix.M22 ) ) + matrix.M42;
        }

        /// <summary>
        /// Transforms a 2D vector normal by a matrix.
        /// </summary>
        /// <param name="normal">The source vector.</param>
        /// <param name="matrix">The transformation matrix.</param>
        public static Vector2 TransformNormal( Vector2 normal, Matrix matrix )
        {
            float numX = ( normal.X * matrix.M11 ) + ( normal.Y * matrix.M21 );
            float numY = ( normal.X * matrix.M12 ) + ( normal.Y * matrix.M22 );

            return new Vector2 { X = numX, Y = numY };
        }

        /// <summary>
        /// Transforms a vector normal by a matrix.
        /// </summary>
        /// <param name="normal">The source vector.</param>
        /// <param name="matrix">The transformation matrix.</param>
        /// <param name="result">[OutAttribute] The Vector2 resulting from the transformation.</param>
        public static void TransformNormal( ref Vector2 normal, ref Matrix matrix, out Vector2 result )
        {
            result.X = ( normal.X * matrix.M11 ) + ( normal.Y * matrix.M21 );
            result.Y = ( normal.X * matrix.M12 ) + ( normal.Y * matrix.M22 );
        }

        /// <summary>
        /// Transforms a single Vector2, or the vector normal (x, y, 0, 0), by a specified Quaternion rotation.
        /// </summary>
        /// <param name="value">The vector to rotate.</param>
        /// <param name="rotation">The Quaternion rotation to apply.</param>
        public static Vector2 Transform( Vector2 value, Quaternion rotation )
        {
            float rotationX = rotation.X + rotation.X;
            float rotationY = rotation.Y + rotation.Y;
            float rotationZ = rotation.Z + rotation.Z;

            float num1 = rotation.W * rotationZ;
            float num2 = rotation.X * rotationX;
            float num3 = rotation.X * rotationY;
            float num4 = rotation.Y * rotationY;
            float num5 = rotation.Z * rotationZ;

            float numX = ( value.X * ( ( 1f - num4 ) - num5 ) ) + ( value.Y * ( num3 - num1 ) );
            float numY = ( value.X * ( num3 + num1 ) ) + ( value.Y * ( ( 1f - num2 ) - num5 ) );

            return new Vector2 { X = numX, Y = numY };
        }

        /// <summary>
        /// Transforms a Vector2, or the vector normal (x, y, 0, 0), by a specified Quaternion rotation.
        /// </summary>
        /// <param name="value">The vector to rotate.</param>
        /// <param name="rotation">The Quaternion rotation to apply.</param>
        /// <param name="result">[OutAttribute] An existing Vector2 filled in with the result of the rotation.</param>
        public static void Transform( ref Vector2 value, ref Quaternion rotation, out Vector2 result )
        {
            float rotationX = rotation.X + rotation.X;
            float rotationY = rotation.Y + rotation.Y;
            float rotationZ = rotation.Z + rotation.Z;

            float num1 = rotation.W * rotationZ;
            float num2 = rotation.X * rotationX;
            float num3 = rotation.X * rotationY;
            float num4 = rotation.Y * rotationY;
            float num5 = rotation.Z * rotationZ;

            result.X = ( value.X * ( ( 1f - num4 ) - num5 ) ) + ( value.Y * ( num3 - num1 ) );
            result.Y = ( value.X * ( num3 + num1 ) ) + ( value.Y * ( ( 1f - num2 ) - num5 ) );
        }

        /// <summary>
        /// Transforms an array of Vector2s by a specified Matrix.
        /// </summary>
        /// <param name="sourceArray">The array of Vector2s to transform.</param>
        /// <param name="matrix">The transform Matrix to apply.</param>
        /// <param name="destinationArray">An existing array into which the transformed Vector2s are written.</param>
        public static void Transform( Vector2[] sourceArray, ref Matrix matrix, Vector2[] destinationArray )
        {
            if ( sourceArray == null )
                throw new ArgumentNullException( "sourceArray" );

            if ( destinationArray == null )
                throw new ArgumentNullException( "destinationArray" );

            if ( destinationArray.Length < sourceArray.Length )
                throw new ArgumentException( "FrameworkResources.NotEnoughTargetSize" );

            for ( int iIndex = 0; iIndex < sourceArray.Length; iIndex++ )
            {
                float x = sourceArray[iIndex].X;
                float y = sourceArray[iIndex].Y;

                destinationArray[iIndex].X = ( ( x * matrix.M11 ) + ( y * matrix.M21 ) ) + matrix.M41;
                destinationArray[iIndex].Y = ( ( x * matrix.M12 ) + ( y * matrix.M22 ) ) + matrix.M42;
            }
        }

        /// <summary>
        /// Transforms a specified range in an array of Vector2s by a specified Matrix and places the results in a specified range in a destination array.
        /// </summary>
        /// <param name="sourceArray">The source array.</param>
        /// <param name="sourceIndex">The index of the first Vector2 to transform in the source array.</param>
        /// <param name="matrix">The Matrix to transform by.</param>
        /// <param name="destinationArray">The destination array into which the resulting Vector2s will be written.</param>
        /// <param name="destinationIndex">The index of the position in the destination array where the first result Vector2 should be written.</param>
        /// <param name="length">The number of Vector2s to be transformed.</param>
        public static void Transform( Vector2[] sourceArray, int sourceIndex, ref Matrix matrix, Vector2[] destinationArray, int destinationIndex, int length )
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

                destinationArray[destinationIndex].X = ( ( x * matrix.M11 ) + ( y * matrix.M21 ) ) + matrix.M41;
                destinationArray[destinationIndex].Y = ( ( x * matrix.M12 ) + ( y * matrix.M22 ) ) + matrix.M42;

                sourceIndex++;
                destinationIndex++;
                length--;
            }
        }

        /// <summary>
        /// Transforms an array of Vector2 vector normals by a specified Matrix.
        /// </summary>
        /// <param name="sourceArray">The array of vector normals to transform.</param>
        /// <param name="matrix">The transform Matrix to apply.</param>
        /// <param name="destinationArray">An existing array into which the transformed vector normals are written.</param>
        public static void TransformNormal( Vector2[] sourceArray, ref Matrix matrix, Vector2[] destinationArray )
        {
            if ( sourceArray == null )
                throw new ArgumentNullException( "sourceArray" );

            if ( destinationArray == null )
                throw new ArgumentNullException( "destinationArray" );

            if ( destinationArray.Length < sourceArray.Length )
                throw new ArgumentException( "FrameworkResources.NotEnoughTargetSize" );

            for ( int iIndex = 0; iIndex < sourceArray.Length; iIndex++ )
            {
                float x = sourceArray[iIndex].X;
                float y = sourceArray[iIndex].Y;

                destinationArray[iIndex].X = ( x * matrix.M11 ) + ( y * matrix.M21 );
                destinationArray[iIndex].Y = ( x * matrix.M12 ) + ( y * matrix.M22 );
            }
        }

        /// <summary>
        /// Transforms a specified range in an array of Vector2 vector normals by a specified Matrix and places the results in a specified range in a destination array.
        /// </summary>
        /// <param name="sourceArray">The source array.</param>
        /// <param name="sourceIndex">The index of the first Vector2 to transform in the source array.</param>
        /// <param name="matrix">The Matrix to apply.</param>
        /// <param name="destinationArray">The destination array into which the resulting Vector2s are written.</param>
        /// <param name="destinationIndex">The index of the position in the destination array where the first result Vector2 should be written.</param>
        /// <param name="length">The number of vector normals to be transformed.</param>
        public static void TransformNormal( Vector2[] sourceArray, int sourceIndex, ref Matrix matrix, Vector2[] destinationArray, int destinationIndex, int length )
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

                destinationArray[destinationIndex].X = ( x * matrix.M11 ) + ( y * matrix.M21 );
                destinationArray[destinationIndex].Y = ( x * matrix.M12 ) + ( y * matrix.M22 );

                sourceIndex++;
                destinationIndex++;
                length--;
            }
        }

        /// <summary>
        /// Transforms an array of Vector2s by a specified Quaternion.
        /// </summary>
        /// <param name="sourceArray">The array of Vector2s to transform.</param>
        /// <param name="rotation">The transform Matrix to use.</param>
        /// <param name="destinationArray">An existing array into which the transformed Vector2s are written.</param>
        public static void Transform( Vector2[] sourceArray, ref Quaternion rotation, Vector2[] destinationArray )
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

            float num1 = rotation.W * rotationZ;
            float num2 = rotation.X * rotationX;
            float num3 = rotation.X * rotationY;
            float num4 = rotation.Y * rotationY;
            float num5 = rotation.Z * rotationZ;

            float result1 = ( 1f - num4 ) - num5;
            float result2 = num3 - num1;
            float result3 = num3 + num1;
            float result4 = ( 1f - num2 ) - num5;

            for ( int iIndex = 0; iIndex < sourceArray.Length; iIndex++ )
            {
                float x = sourceArray[iIndex].X;
                float y = sourceArray[iIndex].Y;

                destinationArray[iIndex].X = ( x * result1 ) + ( y * result2 );
                destinationArray[iIndex].Y = ( x * result3 ) + ( y * result4 );
            }
        }

        /// <summary>
        /// Transforms a specified range in an array of Vector2s by a specified Quaternion and places the results in a specified range in a destination array.
        /// </summary>
        /// <param name="sourceArray">The source array.</param>
        /// <param name="sourceIndex">The index of the first Vector2 to transform in the source array.</param>
        /// <param name="rotation">The Quaternion rotation to apply.</param>
        /// <param name="destinationArray">The destination array into which the resulting Vector2s are written.</param>
        /// <param name="destinationIndex">The index of the position in the destination array where the first result Vector2 should be written.</param>
        /// <param name="length">The number of Vector2s to be transformed.</param>
        public static void Transform( Vector2[] sourceArray, int sourceIndex, ref Quaternion rotation, Vector2[] destinationArray, int destinationIndex, int length )
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

            float num1 = rotation.W * rotationZ;
            float num2 = rotation.X * rotationX;
            float num3 = rotation.X * rotationY;
            float num4 = rotation.Y * rotationY;
            float num5 = rotation.Z * rotationZ;

            float result1 = ( 1f - num4 ) - num5;
            float result2 = num3 - num1;
            float result3 = num3 + num1;
            float result4 = ( 1f - num2 ) - num5;

            while ( length > 0 )
            {
                float x = sourceArray[sourceIndex].X;
                float y = sourceArray[sourceIndex].Y;

                destinationArray[destinationIndex].X = ( x * result1 ) + ( y * result2 );
                destinationArray[destinationIndex].Y = ( x * result3 ) + ( y * result4 );

                sourceIndex++;
                destinationIndex++;
                length--;
            }
        }

        /// <summary>
        /// Returns a vector pointing in the opposite direction.
        /// </summary>
        /// <param name="value">Source vector.</param>
        public static Vector2 Negate( Vector2 value )
        {
            return new Vector2 { X = -value.X, Y = -value.Y };
        }

        /// <summary>
        /// Returns a vector pointing in the opposite direction.
        /// </summary>
        /// <param name="value">Source vector.</param>
        /// <param name="result">[OutAttribute] Vector pointing in the opposite direction.</param>
        public static void Negate( ref Vector2 value, out Vector2 result )
        {
            result.X = -value.X;
            result.Y = -value.Y;
        }

        /// <summary>
        /// Adds two vectors.
        /// </summary>
        /// <param name="value1">Source vector.</param>
        /// <param name="value2">Source vector.</param>
        public static Vector2 Add( Vector2 value1, Vector2 value2 )
        {
            return new Vector2 { X = value1.X + value2.X, Y = value1.Y + value2.Y };
        }

        /// <summary>
        /// Adds two vectors.
        /// </summary>
        /// <param name="value1">Source vector.</param>
        /// <param name="value2">Source vector.</param>
        /// <param name="result">[OutAttribute] Sum of the source vectors.</param>
        public static void Add( ref Vector2 value1, ref Vector2 value2, out Vector2 result )
        {
            result.X = value1.X + value2.X;
            result.Y = value1.Y + value2.Y;
        }

        /// <summary>
        /// Subtracts a vector from a vector.
        /// </summary>
        /// <param name="value1">Source vector.</param>
        /// <param name="value2">Source vector.</param>
        public static Vector2 Subtract( Vector2 value1, Vector2 value2 )
        {
            return new Vector2 { X = value1.X - value2.X, Y = value1.Y - value2.Y };
        }

        /// <summary>
        /// Subtracts a vector from a vector.
        /// </summary>
        /// <param name="value1">Source vector.</param>
        /// <param name="value2">Source vector.</param>
        /// <param name="result">[OutAttribute] The result of the subtraction.</param>
        public static void Subtract( ref Vector2 value1, ref Vector2 value2, out Vector2 result )
        {
            result.X = value1.X - value2.X;
            result.Y = value1.Y - value2.Y;
        }

        /// <summary>
        /// Multiplies the components of two vectors by each other.
        /// </summary>
        /// <param name="value1">Source vector.</param>
        /// <param name="value2">Source vector.</param>
        public static Vector2 Multiply( Vector2 value1, Vector2 value2 )
        {
            return new Vector2 { X = value1.X * value2.X, Y = value1.Y * value2.Y };
        }

        /// <summary>
        /// Multiplies the components of two vectors by each other.
        /// </summary>
        /// <param name="value1">Source vector.</param>
        /// <param name="value2">Source vector.</param>
        /// <param name="result">[OutAttribute] The result of the multiplication.</param>
        public static void Multiply( ref Vector2 value1, ref Vector2 value2, out Vector2 result )
        {
            result.X = value1.X * value2.X;
            result.Y = value1.Y * value2.Y;
        }

        /// <summary>
        /// Multiplies a vector by a scalar value.
        /// </summary>
        /// <param name="value1">Source vector.</param>
        /// <param name="scaleFactor">Scalar value.</param>
        public static Vector2 Multiply( Vector2 value1, float scaleFactor )
        {
            return new Vector2 { X = value1.X * scaleFactor, Y = value1.Y * scaleFactor };
        }

        /// <summary>
        /// Multiplies a vector by a scalar value.
        /// </summary>
        /// <param name="value1">Source vector.</param>
        /// <param name="scaleFactor">Scalar value.</param>
        /// <param name="result">[OutAttribute] The result of the multiplication.</param>
        public static void Multiply( ref Vector2 value1, float scaleFactor, out Vector2 result )
        {
            result.X = value1.X * scaleFactor;
            result.Y = value1.Y * scaleFactor;
        }

        /// <summary>
        /// Divides the components of a vector by the components of another vector.
        /// </summary>
        /// <param name="value1">Source vector.</param>
        /// <param name="value2">Divisor vector.</param>
        public static Vector2 Divide( Vector2 value1, Vector2 value2 )
        {
            return new Vector2 { X = value1.X / value2.X, Y = value1.Y / value2.Y };
        }

        /// <summary>
        /// Divides the components of a vector by the components of another vector.
        /// </summary>
        /// <param name="value1">Source vector.</param>
        /// <param name="value2">The divisor.</param>
        /// <param name="result">[OutAttribute] The result of the division.</param>
        public static void Divide( ref Vector2 value1, ref Vector2 value2, out Vector2 result )
        {
            result.X = value1.X / value2.X;
            result.Y = value1.Y / value2.Y;
        }

        /// <summary>
        /// Divides a vector by a scalar value.
        /// </summary>
        /// <param name="value1">Source vector.</param>
        /// <param name="divider">The divisor.</param>
        public static Vector2 Divide( Vector2 value1, float divider )
        {
            float num = 1f / divider;

            return new Vector2 { X = value1.X * num, Y = value1.Y * num };
        }

        /// <summary>
        /// Divides a vector by a scalar value.
        /// </summary>
        /// <param name="value1">Source vector.</param>
        /// <param name="divider">The divisor.</param>
        /// <param name="result">[OutAttribute] The result of the division.</param>
        public static void Divide( ref Vector2 value1, float divider, out Vector2 result )
        {
            float num = 1f / divider;

            result.X = value1.X * num;
            result.Y = value1.Y * num;
        }

        #endregion

        #region zh-CHS 共有静态方法 | en Public Static Methods

        /// <summary>
        /// Returns a vector pointing in the opposite direction.
        /// </summary>
        /// <param name="value">Source vector.</param>
        public static Vector2 operator -( Vector2 value )
        {
            return new Vector2 { X = -value.X, Y = -value.Y };
        }

        /// <summary>
        /// Tests vectors for equality.
        /// </summary>
        /// <param name="value1">Source vector.</param>
        /// <param name="value2">Source vector.</param>
        public static bool operator ==( Vector2 value1, Vector2 value2 )
        {
            return ( ( value1.X == value2.X ) && ( value1.Y == value2.Y ) );
        }

        /// <summary>
        /// Tests vectors for inequality.
        /// </summary>
        /// <param name="value1">Vector to compare.</param>
        /// <param name="value2">Vector to compare.</param>
        public static bool operator !=( Vector2 value1, Vector2 value2 )
        {
            if ( value1.X == value2.X )
                return !( value1.Y == value2.Y );

            return true;
        }

        /// <summary>
        /// Adds two vectors.
        /// </summary>
        /// <param name="value1">Source vector.</param>
        /// <param name="value2">Source vector.</param>
        public static Vector2 operator +( Vector2 value1, Vector2 value2 )
        {
            return new Vector2 { X = value1.X + value2.X, Y = value1.Y + value2.Y };
        }

        /// <summary>
        /// Subtracts a vector from a vector.
        /// </summary>
        /// <param name="value1">Source vector.</param>
        /// <param name="value2">source vector.</param>
        public static Vector2 operator -( Vector2 value1, Vector2 value2 )
        {
            return new Vector2 { X = value1.X - value2.X, Y = value1.Y - value2.Y };
        }

        /// <summary>
        /// Multiplies the components of two vectors by each other.
        /// </summary>
        /// <param name="value1">Source vector.</param>
        /// <param name="value2">Source vector.</param>
        public static Vector2 operator *( Vector2 value1, Vector2 value2 )
        {
            return new Vector2 { X = value1.X * value2.X, Y = value1.Y * value2.Y };
        }

        /// <summary>
        /// Multiplies a vector by a scalar value.
        /// </summary>
        /// <param name="value">Source vector.</param>
        /// <param name="scaleFactor">Scalar value.</param>
        public static Vector2 operator *( Vector2 value, float scaleFactor )
        {
            return new Vector2 { X = value.X * scaleFactor, Y = value.Y * scaleFactor };
        }

        /// <summary>
        /// Multiplies a vector by a scalar value.
        /// </summary>
        /// <param name="scaleFactor">Scalar value.</param>
        /// <param name="value">Source vector.</param>
        public static Vector2 operator *( float scaleFactor, Vector2 value )
        {
            return new Vector2 { X = value.X * scaleFactor, Y = value.Y * scaleFactor };
        }

        /// <summary>
        /// Divides the components of a vector by the components of another vector.
        /// </summary>
        /// <param name="value1">Source vector.</param>
        /// <param name="value2">Divisor vector.</param>
        public static Vector2 operator /( Vector2 value1, Vector2 value2 )
        {
            return new Vector2 { X = value1.X / value2.X, Y = value1.Y / value2.Y };
        }

        /// <summary>
        /// Divides a vector by a scalar value.
        /// </summary>
        /// <param name="value1">Source vector.</param>
        /// <param name="divider">The divisor.</param>
        public static Vector2 operator /( Vector2 value1, float divider )
        {
            float num = 1f / divider;

            return new Vector2 { X = value1.X * num, Y = value1.Y * num };
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="vector3"></param>
        /// <returns></returns>
        public static explicit operator Vector2( Vector3 vector3 )
        {
            return new Vector2( vector3.X, vector3.Y );
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="vector4"></param>
        /// <returns></returns>
        public static explicit operator Vector2( Vector4 vector4 )
        {
            return new Vector2( vector4.X, vector4.Y );
        }

        #endregion

        #region zh-CHS 方法覆盖 | en Override Methods

        /// <summary>
        /// Retrieves a string representation of the current object.
        /// </summary>
        public override string ToString()
        {
            return string.Format( CultureInfo.CurrentCulture, "{{X:{0} Y:{1}}}", new object[] { X.ToString( CultureInfo.CurrentCulture ), Y.ToString( CultureInfo.CurrentCulture ) } );
        }


        /// <summary>
        /// Returns a value that indicates whether the current instance is equal to a specified object.
        /// </summary>
        /// <param name="obj">Object to make the comparison with.</param>
        public override bool Equals( object obj )
        {
            if ( obj is Vector2 )
                return Equals( (Vector2)obj );
            else
                return false;
        }

        /// <summary>
        /// Gets the hash code of the vector object.
        /// </summary>
        public override int GetHashCode()
        {
            return ( X.GetHashCode() + Y.GetHashCode() );
        }

        #endregion

        #region zh-CHS 接口实现 | en Interface Implementation

        /// <summary>
        /// Determines whether the specified Object is equal to the Vector2.
        /// </summary>
        /// <param name="other">The Object to compare with the current Vector2.</param>
        public bool Equals( Vector2 other )
        {
            return ( ( X == other.X ) && ( Y == other.Y ) );
        }

        #endregion

    }
}
#endregion
