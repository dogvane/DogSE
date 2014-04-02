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
using System.ComponentModel;
using System.Globalization;
using System.Runtime.InteropServices;
#endregion



namespace DogSE.Common
{

    /// <summary>
    /// Defines a four-dimensional vector (x,y,z,w), which is used to efficiently rotate an object about the (x, y, z) vector by the angle theta, where w = cos(theta/2).
    /// </summary>
    public struct Quaternion : IEquatable<Quaternion>
    {

        #region zh-CHS 共有成员变量 | en Public Member Variables

        /// <summary>
        /// Specifies the x-value of the vector component of the quaternion.
        /// </summary>
        public float X;
        /// <summary>
        /// Specifies the y-value of the vector component of the quaternion.
        /// </summary>
        public float Y;
        /// <summary>
        /// Specifies the z-value of the vector component of the quaternion.
        /// </summary>
        public float Z;
        /// <summary>
        /// Specifies the rotation component of the quaternion.
        /// </summary>
        public float W;

        #endregion

        #region zh-CHS 构造和初始化和清理 | en Constructors and Initializers and Dispose

        /// <summary>
        /// Initializes a new instance of Quaternion.
        /// </summary>
        /// <param name="x">The x-value of the quaternion.</param>
        /// <param name="y">The y-value of the quaternion.</param>
        /// <param name="z">The z-value of the quaternion.</param>
        /// <param name="w">The w-value of the quaternion.</param>
        public Quaternion( float x, float y, float z, float w )
        {
            X = x;
            Y = y;
            Z = z;
            W = w;
        }

        /// <summary>
        /// Initializes a new instance of Quaternion.
        /// </summary>
        /// <param name="vectorPart">The vector component of the quaternion.</param>
        /// <param name="scalarPart">The rotation component of the quaternion.</param>
        public Quaternion( Vector3 vectorPart, float scalarPart )
        {
            X = vectorPart.X;
            Y = vectorPart.Y;
            Z = vectorPart.Z;
            W = scalarPart;
        }

        #endregion

        #region zh-CHS 共有静态属性 | en Public Static Properties

        #region zh-CHS 私有静态成员变量 | en Private Static Member Variables
        /// <summary>
        /// 
        /// </summary>
        private static Quaternion m_Identity = new Quaternion( 0f, 0f, 0f, 1f );
        #endregion
        /// <summary>
        /// Returns a Quaternion representing no rotation.
        /// </summary>
        public static Quaternion Identity
        {
            get { return m_Identity; }
        }

        #endregion

        #region zh-CHS 共有方法 | en Public Methods

        /// <summary>
        /// Calculates the length squared of a Quaternion.
        /// </summary>
        public float LengthSquared()
        {
            return ( ( ( ( X * X ) + ( Y * Y ) ) + ( Z * Z ) ) + ( W * W ) );
        }

        /// <summary>
        /// Calculates the length of a Quaternion.
        /// </summary>
        public float Length()
        {
            return (float)Math.Sqrt( (double)( ( ( X * X ) + ( Y * Y ) ) + ( Z * Z ) ) + ( W * W ) );
        }

        /// <summary>
        /// Divides each component of the quaternion by the length of the quaternion.
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
        /// Divides each component of the quaternion by the length of the quaternion.
        /// </summary>
        /// <param name="quaternion">Source quaternion.</param>
        public static Quaternion Normalize( Quaternion quaternion )
        {
            float num = 1f / ( (float)Math.Sqrt( (double)( ( ( quaternion.X * quaternion.X ) + ( quaternion.Y * quaternion.Y ) ) + ( quaternion.Z * quaternion.Z ) ) + ( quaternion.W * quaternion.W ) ) );

            return new Quaternion { X = quaternion.X * num, Y = quaternion.Y * num, Z = quaternion.Z * num, W = quaternion.W * num };
        }

        /// <summary>
        /// Divides each component of the quaternion by the length of the quaternion.
        /// </summary>
        /// <param name="quaternion">Source quaternion.</param>
        /// <param name="result">[OutAttribute] Normalized quaternion.</param>
        public static void Normalize( ref Quaternion quaternion, out Quaternion result )
        {
            float num = 1f / ( (float)Math.Sqrt( (double)( ( ( quaternion.X * quaternion.X ) + ( quaternion.Y * quaternion.Y ) ) + ( quaternion.Z * quaternion.Z ) ) + ( quaternion.W * quaternion.W ) ) );

            result.X = quaternion.X * num;
            result.Y = quaternion.Y * num;
            result.Z = quaternion.Z * num;
            result.W = quaternion.W * num;
        }

        /// <summary>
        /// Transforms this Quaternion into its conjugate.
        /// </summary>
        public void Conjugate()
        {
            X = -X;
            Y = -Y;
            Z = -Z;
        }

        /// <summary>
        /// Returns the conjugate of a specified Quaternion.
        /// </summary>
        /// <param name="value">The Quaternion of which to return the conjugate.</param>
        public static Quaternion Conjugate( Quaternion value )
        {
            return new Quaternion { X = -value.X, Y = -value.Y, Z = -value.Z, W = value.W };
        }

        /// <summary>
        /// Returns the conjugate of a specified Quaternion.
        /// </summary>
        /// <param name="value">The Quaternion of which to return the conjugate.</param>
        /// <param name="result">[OutAttribute] An existing Quaternion filled in to be the conjugate of the specified one.</param>
        public static void Conjugate( ref Quaternion value, out Quaternion result )
        {
            result.X = -value.X;
            result.Y = -value.Y;
            result.Z = -value.Z;
            result.W = value.W;
        }

        /// <summary>
        /// Returns the inverse of a Quaternion.
        /// </summary>
        /// <param name="quaternion">Source Quaternion.</param>
        public static Quaternion Inverse( Quaternion quaternion )
        {
            float num = 1f / ( ( ( quaternion.X * quaternion.X ) + ( quaternion.Y * quaternion.Y ) ) + ( quaternion.Z * quaternion.Z ) ) + ( quaternion.W * quaternion.W );

            return new Quaternion { X = -quaternion.X * num, Y = -quaternion.Y * num, Z = -quaternion.Z * num, W = quaternion.W * num };
        }

        /// <summary>
        /// Returns the inverse of a Quaternion.
        /// </summary>
        /// <param name="quaternion">Source Quaternion.</param>
        /// <param name="result">[OutAttribute] The inverse of the Quaternion.</param>
        public static void Inverse( ref Quaternion quaternion, out Quaternion result )
        {
            float num = 1f / ( ( ( quaternion.X * quaternion.X ) + ( quaternion.Y * quaternion.Y ) ) + ( quaternion.Z * quaternion.Z ) ) + ( quaternion.W * quaternion.W );

            result.X = -quaternion.X * num;
            result.Y = -quaternion.Y * num;
            result.Z = -quaternion.Z * num;
            result.W = quaternion.W * num;
        }

        /// <summary>
        /// Creates a Quaternion from a vector and an angle to rotate about the vector.
        /// </summary>
        /// <param name="axis">The vector to rotate around.</param>
        /// <param name="angle">The angle to rotate around the vector.</param>
        public static Quaternion CreateFromAxisAngle( Vector3 axis, float angle )
        {
            float num = angle * 0.5f;

            float result1 = (float)Math.Sin( (double)num );
            float result2 = (float)Math.Cos( (double)num );

            return new Quaternion { X = axis.X * result1, Y = axis.Y * result1, Z = axis.Z * result1, W = result2 };
        }

        /// <summary>
        /// Creates a Quaternion from a vector and an angle to rotate about the vector.
        /// </summary>
        /// <param name="axis">The vector to rotate around.</param>
        /// <param name="angle">The angle to rotate around the vector.</param>
        /// <param name="result">[OutAttribute] The created Quaternion.</param>
        public static void CreateFromAxisAngle( ref Vector3 axis, float angle, out Quaternion result )
        {
            float num = angle * 0.5f;

            float result1 = (float)Math.Sin( (double)num );
            float result2 = (float)Math.Cos( (double)num );

            result.X = axis.X * result1;
            result.Y = axis.Y * result1;
            result.Z = axis.Z * result1;
            result.W = result2;
        }

        /// <summary>
        /// Creates a new Quaternion from specified yaw, pitch, and roll angles.
        /// </summary>
        /// <param name="yaw">The yaw angle, in radians, around the y-axis.</param>
        /// <param name="pitch">The pitch angle, in radians, around the x-axis.</param>
        /// <param name="roll">The roll angle, in radians, around the z-axis.</param>
        public static Quaternion CreateFromYawPitchRoll( float yaw, float pitch, float roll )
        {
            float num1 = roll * 0.5f;
            float result1 = (float)Math.Sin( (double)num1 );
            float result2 = (float)Math.Cos( (double)num1 );

            float num2 = pitch * 0.5f;
            float result3 = (float)Math.Sin( (double)num2 );
            float result4 = (float)Math.Cos( (double)num2 );

            float num3 = yaw * 0.5f;
            float result5 = (float)Math.Sin( (double)num3 );
            float result6 = (float)Math.Cos( (double)num3 );

            return new Quaternion
            {
                X = ( ( result6 * result3 ) * result2 ) + ( ( result5 * result4 ) * result1 ),
                Y = ( ( result5 * result4 ) * result2 ) - ( ( result6 * result3 ) * result1 ),
                Z = ( ( result6 * result4 ) * result1 ) - ( ( result5 * result3 ) * result2 ),
                W = ( ( result6 * result4 ) * result2 ) + ( ( result5 * result3 ) * result1 )
            };
        }

        /// <summary>
        /// Creates a new Quaternion from specified yaw, pitch, and roll angles.
        /// </summary>
        /// <param name="yaw">The yaw angle, in radians, around the y-axis.</param>
        /// <param name="pitch">The pitch angle, in radians, around the x-axis.</param>
        /// <param name="roll">The roll angle, in radians, around the z-axis.</param>
        /// <param name="result">[OutAttribute] An existing Quaternion filled in to express the specified yaw, pitch, and roll angles.</param>
        public static void CreateFromYawPitchRoll( float yaw, float pitch, float roll, out Quaternion result )
        {
            float num1 = roll * 0.5f;
            float result1 = (float)Math.Sin( (double)num1 );
            float result2 = (float)Math.Cos( (double)num1 );

            float num2 = pitch * 0.5f;
            float result3 = (float)Math.Sin( (double)num2 );
            float result4 = (float)Math.Cos( (double)num2 );

            float num3 = yaw * 0.5f;
            float result5 = (float)Math.Sin( (double)num3 );
            float result6 = (float)Math.Cos( (double)num3 );

            result.X = ( ( result6 * result3 ) * result2 ) + ( ( result5 * result4 ) * result1 );
            result.Y = ( ( result5 * result4 ) * result2 ) - ( ( result6 * result3 ) * result1 );
            result.Z = ( ( result6 * result4 ) * result1 ) - ( ( result5 * result3 ) * result2 );
            result.W = ( ( result6 * result4 ) * result2 ) + ( ( result5 * result3 ) * result1 );
        }

        /// <summary>
        /// Creates a Quaternion from a rotation Matrix.
        /// </summary>
        /// <param name="matrix">The rotation Matrix to create the Quaternion from.</param>
        public static Quaternion CreateFromRotationMatrix( Matrix matrix )
        {
            Quaternion quaternion = new Quaternion();

            float num = ( matrix.M11 + matrix.M22 ) + matrix.M33;
            if ( num > 0f )
            {
                float num1 = (float)Math.Sqrt( (double)( num + 1f ) );

                quaternion.W = num1 * 0.5f;

                num1 = 0.5f / num1;

                quaternion.X = ( matrix.M23 - matrix.M32 ) * num1;
                quaternion.Y = ( matrix.M31 - matrix.M13 ) * num1;
                quaternion.Z = ( matrix.M12 - matrix.M21 ) * num1;
            }
            else if ( ( matrix.M11 >= matrix.M22 ) && ( matrix.M11 >= matrix.M33 ) )
            {
                float num2 = (float)Math.Sqrt( (double)( ( ( 1f + matrix.M11 ) - matrix.M22 ) - matrix.M33 ) );
                float num3 = 0.5f / num2;

                quaternion.X = 0.5f * num2;
                quaternion.Y = ( matrix.M12 + matrix.M21 ) * num3;
                quaternion.Z = ( matrix.M13 + matrix.M31 ) * num3;
                quaternion.W = ( matrix.M23 - matrix.M32 ) * num3;
            }
            else if ( matrix.M22 > matrix.M33 )
            {
                float num4 = (float)Math.Sqrt( (double)( ( ( 1f + matrix.M22 ) - matrix.M11 ) - matrix.M33 ) );
                float num5 = 0.5f / num4;

                quaternion.X = ( matrix.M21 + matrix.M12 ) * num5;
                quaternion.Y = 0.5f * num4;
                quaternion.Z = ( matrix.M32 + matrix.M23 ) * num5;
                quaternion.W = ( matrix.M31 - matrix.M13 ) * num5;
            }
            else
            {
                float num6 = (float)Math.Sqrt( (double)( ( ( 1f + matrix.M33 ) - matrix.M11 ) - matrix.M22 ) );
                float num7 = 0.5f / num6;

                quaternion.X = ( matrix.M31 + matrix.M13 ) * num7;
                quaternion.Y = ( matrix.M32 + matrix.M23 ) * num7;
                quaternion.Z = 0.5f * num6;
                quaternion.W = ( matrix.M12 - matrix.M21 ) * num7;
            }

            return quaternion;
        }

        /// <summary>
        /// Creates a Quaternion from a rotation Matrix.
        /// </summary>
        /// <param name="matrix">The rotation Matrix to create the Quaternion from.</param>
        /// <param name="result">[OutAttribute] The created Quaternion.</param>
        public static void CreateFromRotationMatrix( ref Matrix matrix, out Quaternion result )
        {
            float num = ( matrix.M11 + matrix.M22 ) + matrix.M33;
            if ( num > 0f )
            {
                float num1 = (float)Math.Sqrt( (double)( num + 1f ) );

                result.W = num1 * 0.5f;
                num1 = 0.5f / num1;

                result.X = ( matrix.M23 - matrix.M32 ) * num1;
                result.Y = ( matrix.M31 - matrix.M13 ) * num1;
                result.Z = ( matrix.M12 - matrix.M21 ) * num1;
            }
            else if ( ( matrix.M11 >= matrix.M22 ) && ( matrix.M11 >= matrix.M33 ) )
            {
                float num2 = (float)Math.Sqrt( (double)( ( ( 1f + matrix.M11 ) - matrix.M22 ) - matrix.M33 ) );
                float num3 = 0.5f / num2;

                result.X = 0.5f * num2;
                result.Y = ( matrix.M12 + matrix.M21 ) * num3;
                result.Z = ( matrix.M13 + matrix.M31 ) * num3;
                result.W = ( matrix.M23 - matrix.M32 ) * num3;
            }
            else if ( matrix.M22 > matrix.M33 )
            {
                float num4 = (float)Math.Sqrt( (double)( ( ( 1f + matrix.M22 ) - matrix.M11 ) - matrix.M33 ) );
                float num5 = 0.5f / num4;

                result.X = ( matrix.M21 + matrix.M12 ) * num5;
                result.Y = 0.5f * num4;
                result.Z = ( matrix.M32 + matrix.M23 ) * num5;
                result.W = ( matrix.M31 - matrix.M13 ) * num5;
            }
            else
            {
                float num6 = (float)Math.Sqrt( (double)( ( ( 1f + matrix.M33 ) - matrix.M11 ) - matrix.M22 ) );
                float num7 = 0.5f / num6;

                result.X = ( matrix.M31 + matrix.M13 ) * num7;
                result.Y = ( matrix.M32 + matrix.M23 ) * num7;
                result.Z = 0.5f * num6;
                result.W = ( matrix.M12 - matrix.M21 ) * num7;
            }
        }

        /// <summary>
        /// Calculates the dot product of two Quaternions.
        /// </summary>
        /// <param name="quaternion1">Source Quaternion.</param>
        /// <param name="quaternion2">Source Quaternion.</param>
        public static float Dot( Quaternion quaternion1, Quaternion quaternion2 )
        {
            return ( ( ( ( quaternion1.X * quaternion2.X ) + ( quaternion1.Y * quaternion2.Y ) ) + ( quaternion1.Z * quaternion2.Z ) ) + ( quaternion1.W * quaternion2.W ) );
        }

        /// <summary>
        /// Calculates the dot product of two Quaternions.
        /// </summary>
        /// <param name="quaternion1">Source Quaternion.</param>
        /// <param name="quaternion2">Source Quaternion.</param>
        /// <param name="result">[OutAttribute] Dot product of the Quaternions.</param>
        public static void Dot( ref Quaternion quaternion1, ref Quaternion quaternion2, out float result )
        {
            result = ( ( ( quaternion1.X * quaternion2.X ) + ( quaternion1.Y * quaternion2.Y ) ) + ( quaternion1.Z * quaternion2.Z ) ) + ( quaternion1.W * quaternion2.W );
        }

        /// <summary>
        /// Interpolates between two quaternions, using spherical linear interpolation.
        /// </summary>
        /// <param name="quaternion1">Source quaternion.</param>
        /// <param name="quaternion2">Source quaternion.</param>
        /// <param name="amount">Value that indicates how far to interpolate between the quaternions.</param>
        public static Quaternion Slerp( Quaternion quaternion1, Quaternion quaternion2, float amount )
        {
            bool flag = false;
            float num = ( ( ( quaternion1.X * quaternion2.X ) + ( quaternion1.Y * quaternion2.Y ) ) + ( quaternion1.Z * quaternion2.Z ) ) + ( quaternion1.W * quaternion2.W );
            if ( num < 0f )
            {
                flag = true;
                num = -num;
            }

            float num2;
            float num3;
            if ( num > 0.999999f )
            {
                num2 = 1f - amount;
                num3 = flag ? -amount : amount;
            }
            else
            {
                float result1 = (float)Math.Acos( (double)num );
                float result2 = (float)( 1.0 / Math.Sin( (double)result1 ) );

                num2 = ( (float)Math.Sin( (double)( ( 1f - amount ) * result1 ) ) ) * result2;
                num3 = flag ? ( ( (float)-Math.Sin( (double)( amount * result1 ) ) ) * result2 ) : ( ( (float)Math.Sin( (double)( amount * result1 ) ) ) * result2 );
            }

            return new Quaternion
            {
                X = ( num2 * quaternion1.X ) + ( num3 * quaternion2.X ),
                Y = ( num2 * quaternion1.Y ) + ( num3 * quaternion2.Y ),
                Z = ( num2 * quaternion1.Z ) + ( num3 * quaternion2.Z ),
                W = ( num2 * quaternion1.W ) + ( num3 * quaternion2.W )
            };
        }

        /// <summary>
        /// Interpolates between two quaternions, using spherical linear interpolation.
        /// </summary>
        /// <param name="quaternion1">Source quaternion.</param>
        /// <param name="quaternion2">Source quaternion.</param>
        /// <param name="amount">Value that indicates how far to interpolate between the quaternions.</param>
        /// <param name="result">[OutAttribute] Result of the interpolation.</param>
        public static void Slerp( ref Quaternion quaternion1, ref Quaternion quaternion2, float amount, out Quaternion result )
        {
            bool flag = false;
            float num = ( ( ( quaternion1.X * quaternion2.X ) + ( quaternion1.Y * quaternion2.Y ) ) + ( quaternion1.Z * quaternion2.Z ) ) + ( quaternion1.W * quaternion2.W );
            if ( num < 0f )
            {
                flag = true;
                num = -num;
            }

            float num2;
            float num3;
            if ( num > 0.999999f )
            {
                num2 = 1f - amount;
                num3 = flag ? -amount : amount;
            }
            else
            {
                float result1 = (float)Math.Acos( (double)num );
                float result2 = (float)( 1.0 / Math.Sin( (double)result1 ) );

                num2 = ( (float)Math.Sin( (double)( ( 1f - amount ) * result1 ) ) ) * result2;
                num3 = flag ? ( ( (float)-Math.Sin( (double)( amount * result1 ) ) ) * result2 ) : ( ( (float)Math.Sin( (double)( amount * result1 ) ) ) * result2 );
            }

            result.X = ( num2 * quaternion1.X ) + ( num3 * quaternion2.X );
            result.Y = ( num2 * quaternion1.Y ) + ( num3 * quaternion2.Y );
            result.Z = ( num2 * quaternion1.Z ) + ( num3 * quaternion2.Z );
            result.W = ( num2 * quaternion1.W ) + ( num3 * quaternion2.W );
        }

        /// <summary>
        /// Linearly interpolates between two quaternions.
        /// </summary>
        /// <param name="quaternion1">Source quaternion.</param>
        /// <param name="quaternion2">Source quaternion.</param>
        /// <param name="amount">Value indicating how far to interpolate between the quaternions.</param>
        public static Quaternion Lerp( Quaternion quaternion1, Quaternion quaternion2, float amount )
        {
            float num = 1f - amount;

            float num2 = ( ( ( quaternion1.X * quaternion2.X ) + ( quaternion1.Y * quaternion2.Y ) ) + ( quaternion1.Z * quaternion2.Z ) ) + ( quaternion1.W * quaternion2.W );
            Quaternion quaternion = new Quaternion();
            if ( num2 >= 0f )
            {
                quaternion.X = ( num * quaternion1.X ) + ( amount * quaternion2.X );
                quaternion.Y = ( num * quaternion1.Y ) + ( amount * quaternion2.Y );
                quaternion.Z = ( num * quaternion1.Z ) + ( amount * quaternion2.Z );
                quaternion.W = ( num * quaternion1.W ) + ( amount * quaternion2.W );
            }
            else
            {
                quaternion.X = ( num * quaternion1.X ) - ( amount * quaternion2.X );
                quaternion.Y = ( num * quaternion1.Y ) - ( amount * quaternion2.Y );
                quaternion.Z = ( num * quaternion1.Z ) - ( amount * quaternion2.Z );
                quaternion.W = ( num * quaternion1.W ) - ( amount * quaternion2.W );
            }

            float num3 = 1f / ( (float)Math.Sqrt( (double)( ( ( quaternion.X * quaternion.X ) + ( quaternion.Y * quaternion.Y ) ) + ( quaternion.Z * quaternion.Z ) ) + ( quaternion.W * quaternion.W ) ) );

            quaternion.X *= num3;
            quaternion.Y *= num3;
            quaternion.Z *= num3;
            quaternion.W *= num3;

            return quaternion;
        }

        /// <summary>
        /// Linearly interpolates between two quaternions.
        /// </summary>
        /// <param name="quaternion1">Source quaternion.</param>
        /// <param name="quaternion2">Source quaternion.</param>
        /// <param name="amount">Value indicating how far to interpolate between the quaternions.</param>
        /// <param name="result">[OutAttribute] The resulting quaternion.</param>
        public static void Lerp( ref Quaternion quaternion1, ref Quaternion quaternion2, float amount, out Quaternion result )
        {
            float num = 1f - amount;

            float num2 = ( ( ( quaternion1.X * quaternion2.X ) + ( quaternion1.Y * quaternion2.Y ) ) + ( quaternion1.Z * quaternion2.Z ) ) + ( quaternion1.W * quaternion2.W );
            if ( num2 >= 0f )
            {
                result.X = ( num * quaternion1.X ) + ( amount * quaternion2.X );
                result.Y = ( num * quaternion1.Y ) + ( amount * quaternion2.Y );
                result.Z = ( num * quaternion1.Z ) + ( amount * quaternion2.Z );
                result.W = ( num * quaternion1.W ) + ( amount * quaternion2.W );
            }
            else
            {
                result.X = ( num * quaternion1.X ) - ( amount * quaternion2.X );
                result.Y = ( num * quaternion1.Y ) - ( amount * quaternion2.Y );
                result.Z = ( num * quaternion1.Z ) - ( amount * quaternion2.Z );
                result.W = ( num * quaternion1.W ) - ( amount * quaternion2.W );
            }

            float num3 = 1f / ( (float)Math.Sqrt( (double)( ( ( result.X * result.X ) + ( result.Y * result.Y ) ) + ( result.Z * result.Z ) ) + ( result.W * result.W ) ) );

            result.X *= num3;
            result.Y *= num3;
            result.Z *= num3;
            result.W *= num3;
        }

        /// <summary>
        /// Concatenates two Quaternions; the result represents the value1 rotation followed by the value2 rotation.
        /// </summary>
        /// <param name="value1">The first Quaternion rotation in the series.</param>
        /// <param name="value2">The second Quaternion rotation in the series.</param>
        public static Quaternion Concatenate( Quaternion value1, Quaternion value2 )
        {
            float x = value1.X;
            float y = value1.Y;
            float z = value1.Z;
            float w = value1.W;

            float x2 = value2.X;
            float y2 = value2.Y;
            float z2 = value2.Z;
            float w2 = value2.W;

            float result1 = ( y2 * z ) - ( z2 * y );
            float result2 = ( z2 * x ) - ( x2 * z );
            float result3 = ( x2 * y ) - ( y2 * x );
            float result4 = ( ( x2 * x ) + ( y2 * y ) ) + ( z2 * z );

            return new Quaternion
            {
                X = ( ( x2 * w ) + ( x * w2 ) ) + result1,
                Y = ( ( y2 * w ) + ( y * w2 ) ) + result2,
                Z = ( ( z2 * w ) + ( z * w2 ) ) + result3,
                W = ( w2 * w ) - result4
            };
        }

        /// <summary>
        /// Concatenates two Quaternions; the result represents the value1 rotation followed by the value2 rotation.
        /// </summary>
        /// <param name="value1">The first Quaternion rotation in the series.</param>
        /// <param name="value2">The second Quaternion rotation in the series.</param>
        /// <param name="result">[OutAttribute] The Quaternion rotation representing the concatenation of value1 followed by value2.</param>
        public static void Concatenate( ref Quaternion value1, ref Quaternion value2, out Quaternion result )
        {
            float x = value1.X;
            float y = value1.Y;
            float z = value1.Z;
            float w = value1.W;

            float x2 = value2.X;
            float y2 = value2.Y;
            float z2 = value2.Z;
            float w2 = value2.W;

            float result1 = ( y2 * z ) - ( z2 * y );
            float result2 = ( z2 * x ) - ( x2 * z );
            float result3 = ( x2 * y ) - ( y2 * x );
            float result4 = ( ( x2 * x ) + ( y2 * y ) ) + ( z2 * z );

            result.X = ( ( x2 * w ) + ( x * w2 ) ) + result1;
            result.Y = ( ( y2 * w ) + ( y * w2 ) ) + result2;
            result.Z = ( ( z2 * w ) + ( z * w2 ) ) + result3;
            result.W = ( w2 * w ) - result4;
        }

        /// <summary>
        /// Flips the sign of each component of the quaternion.
        /// </summary>
        /// <param name="quaternion">Source quaternion.</param>
        public static Quaternion Negate( Quaternion quaternion )
        {
            return new Quaternion { X = -quaternion.X, Y = -quaternion.Y, Z = -quaternion.Z, W = -quaternion.W };
        }

        /// <summary>
        /// Flips the sign of each component of the quaternion.
        /// </summary>
        /// <param name="quaternion">Source quaternion.</param>
        /// <param name="result">[OutAttribute] Negated quaternion.</param>
        public static void Negate( ref Quaternion quaternion, out Quaternion result )
        {
            result.X = -quaternion.X;
            result.Y = -quaternion.Y;
            result.Z = -quaternion.Z;
            result.W = -quaternion.W;
        }

        /// <summary>
        /// Adds two Quaternions.
        /// </summary>
        /// <param name="quaternion1">Quaternion to add.</param>
        /// <param name="quaternion2">Quaternion to add.</param>
        public static Quaternion Add( Quaternion quaternion1, Quaternion quaternion2 )
        {
            return new Quaternion { X = quaternion1.X + quaternion2.X, Y = quaternion1.Y + quaternion2.Y, Z = quaternion1.Z + quaternion2.Z, W = quaternion1.W + quaternion2.W };
        }

        /// <summary>
        /// Adds two Quaternions.
        /// </summary>
        /// <param name="quaternion1">Quaternion to add.</param>
        /// <param name="quaternion2">Quaternion to add.</param>
        /// <param name="result">[OutAttribute] Result of adding the Quaternions.</param>
        public static void Add( ref Quaternion quaternion1, ref Quaternion quaternion2, out Quaternion result )
        {
            result.X = quaternion1.X + quaternion2.X;
            result.Y = quaternion1.Y + quaternion2.Y;
            result.Z = quaternion1.Z + quaternion2.Z;
            result.W = quaternion1.W + quaternion2.W;
        }

        /// <summary>
        /// Subtracts a quaternion from another quaternion.
        /// </summary>
        /// <param name="quaternion1">Source quaternion.</param>
        /// <param name="quaternion2">Source quaternion.</param>
        public static Quaternion Subtract( Quaternion quaternion1, Quaternion quaternion2 )
        {
            return new Quaternion { X = quaternion1.X - quaternion2.X, Y = quaternion1.Y - quaternion2.Y, Z = quaternion1.Z - quaternion2.Z, W = quaternion1.W - quaternion2.W };
        }

        /// <summary>
        /// Subtracts a quaternion from another quaternion.
        /// </summary>
        /// <param name="quaternion1">Source quaternion.</param>
        /// <param name="quaternion2">Source quaternion.</param>
        /// <param name="result">[OutAttribute] Result of the subtraction.</param>
        public static void Subtract( ref Quaternion quaternion1, ref Quaternion quaternion2, out Quaternion result )
        {
            result.X = quaternion1.X - quaternion2.X;
            result.Y = quaternion1.Y - quaternion2.Y;
            result.Z = quaternion1.Z - quaternion2.Z;
            result.W = quaternion1.W - quaternion2.W;
        }

        /// <summary>
        /// Multiplies two quaternions.
        /// </summary>
        /// <param name="quaternion1">The quaternion on the left of the multiplication.</param>
        /// <param name="quaternion2">The quaternion on the right of the multiplication.</param>
        public static Quaternion Multiply( Quaternion quaternion1, Quaternion quaternion2 )
        {
            float x = quaternion1.X;
            float y = quaternion1.Y;
            float z = quaternion1.Z;
            float w = quaternion1.W;

            float x2 = quaternion2.X;
            float y2 = quaternion2.Y;
            float z2 = quaternion2.Z;
            float w2 = quaternion2.W;

            float result1 = ( y * z2 ) - ( z * y2 );
            float result2 = ( z * x2 ) - ( x * z2 );
            float result3 = ( x * y2 ) - ( y * x2 );
            float result4 = ( ( x * x2 ) + ( y * y2 ) ) + ( z * z2 );

            return new Quaternion
            {
                X = ( ( x * w2 ) + ( x2 * w ) ) + result1,
                Y = ( ( y * w2 ) + ( y2 * w ) ) + result2,
                Z = ( ( z * w2 ) + ( z2 * w ) ) + result3,
                W = ( w * w2 ) - result4
            };
        }

        /// <summary>
        /// Multiplies two quaternions.
        /// </summary>
        /// <param name="quaternion1">The quaternion on the left of the multiplication.</param>
        /// <param name="quaternion2">The quaternion on the right of the multiplication.</param>
        /// <param name="result">[OutAttribute] The result of the multiplication.</param>
        public static void Multiply( ref Quaternion quaternion1, ref Quaternion quaternion2, out Quaternion result )
        {
            float x = quaternion1.X;
            float y = quaternion1.Y;
            float z = quaternion1.Z;
            float w = quaternion1.W;

            float x2 = quaternion2.X;
            float y2 = quaternion2.Y;
            float z2 = quaternion2.Z;
            float w2 = quaternion2.W;

            float result1 = ( y * z2 ) - ( z * y2 );
            float result2 = ( z * x2 ) - ( x * z2 );
            float result3 = ( x * y2 ) - ( y * x2 );
            float result4 = ( ( x * x2 ) + ( y * y2 ) ) + ( z * z2 );

            result.X = ( ( x * w2 ) + ( x2 * w ) ) + result1;
            result.Y = ( ( y * w2 ) + ( y2 * w ) ) + result2;
            result.Z = ( ( z * w2 ) + ( z2 * w ) ) + result3;
            result.W = ( w * w2 ) - result4;
        }

        /// <summary>
        /// Multiplies a quaternion by a scalar value.
        /// </summary>
        /// <param name="quaternion1">Source quaternion.</param>
        /// <param name="scaleFactor">Scalar value.</param>
        public static Quaternion Multiply( Quaternion quaternion1, float scaleFactor )
        {
            return new Quaternion { X = quaternion1.X * scaleFactor, Y = quaternion1.Y * scaleFactor, Z = quaternion1.Z * scaleFactor, W = quaternion1.W * scaleFactor };
        }

        /// <summary>
        /// Multiplies a quaternion by a scalar value.
        /// </summary>
        /// <param name="quaternion1">Source quaternion.</param>
        /// <param name="scaleFactor">Scalar value.</param>
        /// <param name="result">[OutAttribute] The result of the multiplication.</param>
        public static void Multiply( ref Quaternion quaternion1, float scaleFactor, out Quaternion result )
        {
            result.X = quaternion1.X * scaleFactor;
            result.Y = quaternion1.Y * scaleFactor;
            result.Z = quaternion1.Z * scaleFactor;
            result.W = quaternion1.W * scaleFactor;
        }

        /// <summary>
        /// Divides a Quaternion by another Quaternion.
        /// </summary>
        /// <param name="quaternion1">Source Quaternion.</param>
        /// <param name="quaternion2">The divisor.</param>
        public static Quaternion Divide( Quaternion quaternion1, Quaternion quaternion2 )
        {
            float x = quaternion1.X;
            float y = quaternion1.Y;
            float z = quaternion1.Z;
            float w = quaternion1.W;

            float num = 1f / ( ( ( quaternion2.X * quaternion2.X ) + ( quaternion2.Y * quaternion2.Y ) ) + ( quaternion2.Z * quaternion2.Z ) ) + ( quaternion2.W * quaternion2.W );

            float num1 = -quaternion2.X * num;
            float num2 = -quaternion2.Y * num;
            float num3 = -quaternion2.Z * num;
            float num4 = quaternion2.W * num;

            float result1 = ( y * num3 ) - ( z * num2 );
            float result2 = ( z * num1 ) - ( x * num3 );
            float result3 = ( x * num2 ) - ( y * num1 );
            float result4 = ( ( x * num1 ) + ( y * num2 ) ) + ( z * num3 );

            return new Quaternion
            {
                X = ( ( x * num4 ) + ( num1 * w ) ) + result1,
                Y = ( ( y * num4 ) + ( num2 * w ) ) + result2,
                Z = ( ( z * num4 ) + ( num3 * w ) ) + result3,
                W = ( w * num4 ) - result4
            };
        }

        /// <summary>
        /// Divides a Quaternion by another Quaternion.
        /// </summary>
        /// <param name="quaternion1">Source Quaternion.</param>
        /// <param name="quaternion2">The divisor.</param>
        /// <param name="result">[OutAttribute] Result of the division.</param>
        public static void Divide( ref Quaternion quaternion1, ref Quaternion quaternion2, out Quaternion result )
        {
            float x = quaternion1.X;
            float y = quaternion1.Y;
            float z = quaternion1.Z;
            float w = quaternion1.W;

            float num = 1f / ( ( ( quaternion2.X * quaternion2.X ) + ( quaternion2.Y * quaternion2.Y ) ) + ( quaternion2.Z * quaternion2.Z ) ) + ( quaternion2.W * quaternion2.W );

            float num1 = -quaternion2.X * num;
            float num2 = -quaternion2.Y * num;
            float num3 = -quaternion2.Z * num;
            float num4 = quaternion2.W * num;

            float result1 = ( y * num3 ) - ( z * num2 );
            float result2 = ( z * num1 ) - ( x * num3 );
            float result3 = ( x * num2 ) - ( y * num1 );
            float result4 = ( ( x * num1 ) + ( y * num2 ) ) + ( z * num3 );

            result.X = ( ( x * num4 ) + ( num1 * w ) ) + result1;
            result.Y = ( ( y * num4 ) + ( num2 * w ) ) + result2;
            result.Z = ( ( z * num4 ) + ( num3 * w ) ) + result3;
            result.W = ( w * num4 ) - result4;
        }

        #endregion

        #region zh-CHS 共有静态方法 | en Public Static Methods

        /// <summary>
        /// Flips the sign of each component of the quaternion.
        /// </summary>
        /// <param name="quaternion">Source quaternion.</param>
        public static Quaternion operator -( Quaternion quaternion )
        {
            return new Quaternion { X = -quaternion.X, Y = -quaternion.Y, Z = -quaternion.Z, W = -quaternion.W };
        }

        /// <summary>
        /// Compares two Quaternions for equality.
        /// </summary>
        /// <param name="quaternion1">Source Quaternion.</param>
        /// <param name="quaternion2">Source Quaternion.</param>
        public static bool operator ==( Quaternion quaternion1, Quaternion quaternion2 )
        {
            return ( ( ( ( quaternion1.X == quaternion2.X ) && ( quaternion1.Y == quaternion2.Y ) ) && ( quaternion1.Z == quaternion2.Z ) ) && ( quaternion1.W == quaternion2.W ) );
        }

        /// <summary>
        /// Compare two Quaternions for inequality.
        /// </summary>
        /// <param name="quaternion1">Source Quaternion.</param>
        /// <param name="quaternion2">Source Quaternion.</param>
        public static bool operator !=( Quaternion quaternion1, Quaternion quaternion2 )
        {
            if ( ( ( quaternion1.X == quaternion2.X ) && ( quaternion1.Y == quaternion2.Y ) ) && ( quaternion1.Z == quaternion2.Z ) )
                return !( quaternion1.W == quaternion2.W );

            return true;
        }

        /// <summary>
        /// Adds two Quaternions.
        /// </summary>
        /// <param name="quaternion1">Quaternion to add.</param>
        /// <param name="quaternion2">Quaternion to add.</param>
        public static Quaternion operator +( Quaternion quaternion1, Quaternion quaternion2 )
        {
            return new Quaternion
            {
                X = quaternion1.X + quaternion2.X,
                Y = quaternion1.Y + quaternion2.Y,
                Z = quaternion1.Z + quaternion2.Z,
                W = quaternion1.W + quaternion2.W
            };
        }

        /// <summary>
        /// Subtracts a quaternion from another quaternion.
        /// </summary>
        /// <param name="quaternion1">Source quaternion.</param>
        /// <param name="quaternion2">Source quaternion.</param>
        public static Quaternion operator -( Quaternion quaternion1, Quaternion quaternion2 )
        {
            return new Quaternion
            {
                X = quaternion1.X - quaternion2.X,
                Y = quaternion1.Y - quaternion2.Y,
                Z = quaternion1.Z - quaternion2.Z,
                W = quaternion1.W - quaternion2.W
            };
        }

        /// <summary>
        /// Multiplies two quaternions.
        /// </summary>
        /// <param name="quaternion1">Source quaternion.</param>
        /// <param name="quaternion2">Source quaternion.</param>
        public static Quaternion operator *( Quaternion quaternion1, Quaternion quaternion2 )
        {
            float x = quaternion1.X;
            float y = quaternion1.Y;
            float z = quaternion1.Z;
            float w = quaternion1.W;

            float x2 = quaternion2.X;
            float y2 = quaternion2.Y;
            float z2 = quaternion2.Z;
            float w2 = quaternion2.W;

            float result1 = ( y * z2 ) - ( z * y2 );
            float result2 = ( z * x2 ) - ( x * z2 );
            float result3 = ( x * y2 ) - ( y * x2 );
            float result4 = ( ( x * x2 ) + ( y * y2 ) ) + ( z * z2 );

            return new Quaternion
            {
                X = ( ( x * w2 ) + ( x2 * w ) ) + result1,
                Y = ( ( y * w2 ) + ( y2 * w ) ) + result2,
                Z = ( ( z * w2 ) + ( z2 * w ) ) + result3,
                W = ( w * w2 ) - result4
            };
        }

        /// <summary>
        /// Multiplies a quaternion by a scalar value.
        /// </summary>
        /// <param name="quaternion1">Source quaternion.</param>
        /// <param name="scaleFactor">Scalar value.</param>
        public static Quaternion operator *( Quaternion quaternion1, float scaleFactor )
        {
            return new Quaternion
            {
                X = quaternion1.X * scaleFactor,
                Y = quaternion1.Y * scaleFactor,
                Z = quaternion1.Z * scaleFactor,
                W = quaternion1.W * scaleFactor
            };
        }

        /// <summary>
        /// Divides a Quaternion by another Quaternion.
        /// </summary>
        /// <param name="quaternion1">Source Quaternion.</param>
        /// <param name="quaternion2">The divisor.</param>
        public static Quaternion operator /( Quaternion quaternion1, Quaternion quaternion2 )
        {
            float x = quaternion1.X;
            float y = quaternion1.Y;
            float z = quaternion1.Z;
            float w = quaternion1.W;

            float num = 1f / ( ( ( quaternion2.X * quaternion2.X ) + ( quaternion2.Y * quaternion2.Y ) ) + ( quaternion2.Z * quaternion2.Z ) ) + ( quaternion2.W * quaternion2.W );

            float result = quaternion2.W * num;

            float result1 = -quaternion2.X * num;
            float result2 = -quaternion2.Y * num;
            float result3 = -quaternion2.Z * num;

            float result4 = ( y * result3 ) - ( z * result2 );
            float result5 = ( z * result1 ) - ( x * result3 );
            float result6 = ( x * result2 ) - ( y * result1 );

            float result8 = ( ( x * result1 ) + ( y * result2 ) ) + ( z * result3 );

            return new Quaternion
            {
                X = ( ( x * result ) + ( result1 * w ) ) + result4,
                Y = ( ( y * result ) + ( result2 * w ) ) + result5,
                Z = ( ( z * result ) + ( result3 * w ) ) + result6,
                W = ( w * result ) - result8
            };
        }

        #endregion

        #region zh-CHS 方法覆盖 | en Override Methods
        /// <summary>
        /// Retireves a string representation of the current object.
        /// </summary>
        public override string ToString()
        {
            return string.Format( CultureInfo.CurrentCulture, "{{X:{0} Y:{1} Z:{2} W:{3}}}", new object[] { X.ToString( CultureInfo.CurrentCulture ), Y.ToString( CultureInfo.CurrentCulture ), Z.ToString( CultureInfo.CurrentCulture ), W.ToString( CultureInfo.CurrentCulture ) } );
        }

        /// <summary>
        /// Returns a value that indicates whether the current instance is equal to a specified object.
        /// </summary>
        /// <param name="obj">Object to make the comparison with.</param>
        public override bool Equals( object obj )
        {
            if ( obj is Quaternion )
                return Equals( (Quaternion)obj );
            else
                return false;
        }

        /// <summary>
        /// Get the hash code of this object.
        /// </summary>
        public override int GetHashCode()
        {
            return ( ( ( X.GetHashCode() + Y.GetHashCode() ) + Z.GetHashCode() ) + W.GetHashCode() );
        }
        #endregion

        #region zh-CHS 接口实现 | en Interface Implementation
        /// <summary>
        /// Determines whether the specified Object is equal to the Quaternion.
        /// </summary>
        /// <param name="other">The Quaternion to compare with the current Quaternion.</param>
        public bool Equals( Quaternion other )
        {
            return ( ( ( ( X == other.X ) && ( Y == other.Y ) ) && ( Z == other.Z ) ) && ( W == other.W ) );
        }
        #endregion

    }
}
#endregion
