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
using System.Runtime.InteropServices;
#endregion



namespace DogSE.Common
{
    /// <summary>
    /// Defines a matrix.
    /// </summary>
    public struct Matrix : IEquatable<Matrix>
    {
        #region zh-CHS 共有成员变量 | en Public Member Variables

        /// <summary>
        /// Value at row 1 column 1 of the matrix.
        /// </summary>
        public float M11;
        /// <summary>
        /// Value at row 1 column 2 of the matrix.
        /// </summary>
        public float M12;
        /// <summary>
        /// Value at row 1 column 3 of the matrix.
        /// </summary>
        public float M13;
        /// <summary>
        /// Value at row 1 column 4 of the matrix.
        /// </summary>
        public float M14;
        /// <summary>
        /// Value at row 2 column 1 of the matrix.
        /// </summary>
        public float M21;
        /// <summary>
        /// Value at row 2 column 2 of the matrix.
        /// </summary>
        public float M22;
        /// <summary>
        /// Value at row 2 column 3 of the matrix.
        /// </summary>
        public float M23;
        /// <summary>
        /// Value at row 2 column 4 of the matrix.
        /// </summary>
        public float M24;
        /// <summary>
        /// Value at row 3 column 1 of the matrix.
        /// </summary>
        public float M31;
        /// <summary>
        /// Value at row 3 column 2 of the matrix.
        /// </summary>
        public float M32;
        /// <summary>
        /// Value at row 3 column 3 of the matrix.
        /// </summary>
        public float M33;
        /// <summary>
        /// Value at row 3 column 4 of the matrix.
        /// </summary>
        public float M34;
        /// <summary>
        /// Value at row 4 column 1 of the matrix.
        /// </summary>
        public float M41;
        /// <summary>
        /// Value at row 4 column 2 of the matrix.
        /// </summary>
        public float M42;
        /// <summary>
        /// Value at row 4 column 3 of the matrix.
        /// </summary>
        public float M43;
        /// <summary>
        /// Value at row 4 column 4 of the matrix.
        /// </summary>
        public float M44;

        #endregion

        #region zh-CHS 构造和初始化和清理 | en Constructors and Initializers and Dispose

        /// <summary>
        /// Initializes a new instance of Matrix.
        /// </summary>
        /// <param name="m11">Value to initialize m11 to.</param>
        /// <param name="m12">Value to initialize m12 to.</param>
        /// <param name="m13">Value to initialize m13 to.</param>
        /// <param name="m14">Value to initialize m14 to.</param>
        /// <param name="m21">Value to initialize m21 to.</param>
        /// <param name="m22">Value to initialize m22 to.</param>
        /// <param name="m23">Value to initialize m23 to.</param>
        /// <param name="m24">Value to initialize m24 to.</param>
        /// <param name="m31">Value to initialize m31 to.</param>
        /// <param name="m32">Value to initialize m32 to.</param>
        /// <param name="m33">Value to initialize m33 to.</param>
        /// <param name="m34">Value to initialize m34 to.</param>
        /// <param name="m41">Value to initialize m41 to.</param>
        /// <param name="m42">Value to initialize m42 to.</param>
        /// <param name="m43">Value to initialize m43 to.</param>
        /// <param name="m44">Value to initialize m44 to.</param>
        public Matrix( float m11, float m12, float m13, float m14, float m21, float m22, float m23, float m24, float m31, float m32, float m33, float m34, float m41, float m42, float m43, float m44 )
        {
            M11 = m11;
            M12 = m12;
            M13 = m13;
            M14 = m14;
            M21 = m21;
            M22 = m22;
            M23 = m23;
            M24 = m24;
            M31 = m31;
            M32 = m32;
            M33 = m33;
            M34 = m34;
            M41 = m41;
            M42 = m42;
            M43 = m43;
            M44 = m44;
        }

        #endregion

        #region zh-CHS 共有属性 | en Public Properties

        /// <summary>
        /// Gets and sets the up vector of the Matrix.
        /// </summary>
        public Vector3 Up
        {
            get { return new Vector3 { X = M21, Y = M22, Z = M23 }; }
            set
            {
                M21 = value.X;
                M22 = value.Y;
                M23 = value.Z;
            }
        }

        /// <summary>
        /// Gets and sets the down vector of the Matrix.
        /// </summary>
        public Vector3 Down
        {
            get { return new Vector3 { X = -M21, Y = -M22, Z = -M23 }; }
            set
            {
                M21 = -value.X;
                M22 = -value.Y;
                M23 = -value.Z;
            }
        }

        /// <summary>
        /// Gets and sets the right vector of the Matrix.
        /// </summary>
        public Vector3 Right
        {
            get { return new Vector3 { X = M11, Y = M12, Z = M13 }; }
            set
            {
                M11 = value.X;
                M12 = value.Y;
                M13 = value.Z;
            }
        }

        /// <summary>
        /// Gets and sets the left vector of the Matrix.
        /// </summary>
        public Vector3 Left
        {
            get { return new Vector3 { X = -M11, Y = -M12, Z = -M13 }; }
            set
            {
                M11 = -value.X;
                M12 = -value.Y;
                M13 = -value.Z;
            }
        }

        /// <summary>
        /// Gets and sets the forward vector of the Matrix.
        /// </summary>
        public Vector3 Forward
        {
            get { return new Vector3 { X = -M31, Y = -M32, Z = -M33 }; }
            set
            {
                M31 = -value.X;
                M32 = -value.Y;
                M33 = -value.Z;
            }
        }

        /// <summary>
        /// Gets and sets the backward vector of the Matrix.
        /// </summary>
        public Vector3 Backward
        {
            get { return new Vector3 { X = M31, Y = M32, Z = M33 }; }
            set
            {
                M31 = value.X;
                M32 = value.Y;
                M33 = value.Z;
            }
        }

        /// <summary>
        /// Gets and sets the translation vector of the Matrix.
        /// </summary>
        public Vector3 Translation
        {
            get { return new Vector3 { X = M41, Y = M42, Z = M43 }; }
            set
            {
                M41 = value.X;
                M42 = value.Y;
                M43 = value.Z;
            }
        }

        #endregion

        #region zh-CHS 共有静态属性 | en Public Static Properties

        #region zh-CHS 私有静态成员变量 | en Private Static Member Variables
        /// <summary>
        /// 
        /// </summary>
        private static Matrix m_Identity = new Matrix( 1f, 0f, 0f, 0f, 0f, 1f, 0f, 0f, 0f, 0f, 1f, 0f, 0f, 0f, 0f, 1f );
        #endregion
        /// <summary>
        /// Returns an instance of the identity matrix.
        /// </summary>
        public static Matrix Identity
        {
            get { return m_Identity; }
        }

        #endregion

        #region zh-CHS 共有静态方法 | en Public Static Methods

        /// <summary>
        /// Creates a spherical billboard that rotates around a specified object position.
        /// </summary>
        /// <param name="objectPosition">Position of the object the billboard will rotate around.</param>
        /// <param name="cameraPosition">Position of the camera.</param>
        /// <param name="cameraUpVector">The up vector of the camera.</param>
        /// <param name="cameraForwardVector">Optional forward vector of the camera.</param>
        public static Matrix CreateBillboard( Vector3 objectPosition, Vector3 cameraPosition, Vector3 cameraUpVector, Vector3? cameraForwardVector )
        {
            Vector3 vector = new Vector3 { X = objectPosition.X - cameraPosition.X, Y = objectPosition.Y - cameraPosition.Y, Z = objectPosition.Z - cameraPosition.Z };

            float fLengthSquared = vector.LengthSquared();
            if ( fLengthSquared < 0.0001f )
                vector = cameraForwardVector.HasValue ? -cameraForwardVector.Value : Vector3.Forward;
            else
                Vector3.Multiply( ref vector, (float)( 1f / ( (float)Math.Sqrt( (double)fLengthSquared ) ) ), out vector );

            Vector3 vector1;
            Vector3.Cross( ref cameraUpVector, ref vector, out vector1 );

            vector1.Normalize();

            Vector3 vector2;
            Vector3.Cross( ref vector, ref vector1, out vector2 );

            return new Matrix
            {
                M11 = vector1.X,
                M12 = vector1.Y,
                M13 = vector1.Z,
                M14 = 0f,
                M21 = vector2.X,
                M22 = vector2.Y,
                M23 = vector2.Z,
                M24 = 0f,
                M31 = vector.X,
                M32 = vector.Y,
                M33 = vector.Z,
                M34 = 0f,
                M41 = objectPosition.X,
                M42 = objectPosition.Y,
                M43 = objectPosition.Z,
                M44 = 1f
            };
        }

        /// <summary>
        /// Creates a spherical billboard that rotates around a specified object position.
        /// </summary>
        /// <param name="objectPosition">Position of the object the billboard will rotate around.</param>
        /// <param name="cameraPosition">Position of the camera.</param>
        /// <param name="cameraUpVector">The up vector of the camera.</param>
        /// <param name="cameraForwardVector">Optional forward vector of the camera.</param>
        /// <param name="result">[OutAttribute] The created billboard matrix.</param>
        public static void CreateBillboard( ref Vector3 objectPosition, ref Vector3 cameraPosition, ref Vector3 cameraUpVector, Vector3? cameraForwardVector, out Matrix result )
        {
            Vector3 vector = new Vector3 { X = objectPosition.X - cameraPosition.X, Y = objectPosition.Y - cameraPosition.Y, Z = objectPosition.Z - cameraPosition.Z };

            float fLengthSquared = vector.LengthSquared();
            if ( fLengthSquared < 0.0001f )
                vector = cameraForwardVector.HasValue ? -cameraForwardVector.Value : Vector3.Forward;
            else
                Vector3.Multiply( ref vector, (float)( 1f / ( (float)Math.Sqrt( (double)fLengthSquared ) ) ), out vector );

            Vector3 vector1;
            Vector3.Cross( ref cameraUpVector, ref vector, out vector1 );

            vector1.Normalize();

            Vector3 vector2;
            Vector3.Cross( ref vector, ref vector1, out vector2 );

            result.M11 = vector1.X;
            result.M12 = vector1.Y;
            result.M13 = vector1.Z;
            result.M14 = 0f;

            result.M21 = vector2.X;
            result.M22 = vector2.Y;
            result.M23 = vector2.Z;
            result.M24 = 0f;

            result.M31 = vector.X;
            result.M32 = vector.Y;
            result.M33 = vector.Z;
            result.M34 = 0f;

            result.M41 = objectPosition.X;
            result.M42 = objectPosition.Y;
            result.M43 = objectPosition.Z;
            result.M44 = 1f;
        }

        /// <summary>
        /// Creates a cylindrical billboard that rotates around a specified axis.
        /// </summary>
        /// <param name="objectPosition">Position of the object the billboard will rotate around.</param>
        /// <param name="cameraPosition">Position of the camera.</param>
        /// <param name="rotateAxis">Axis to rotate the billboard around.</param>
        /// <param name="cameraForwardVector">Optional forward vector of the camera.</param>
        /// <param name="objectForwardVector">Optional forward vector of the object.</param>
        public static Matrix CreateConstrainedBillboard( Vector3 objectPosition, Vector3 cameraPosition, Vector3 rotateAxis, Vector3? cameraForwardVector, Vector3? objectForwardVector )
        {
            Vector3 vector = new Vector3 { X = objectPosition.X - cameraPosition.X, Y = objectPosition.Y - cameraPosition.Y, Z = objectPosition.Z - cameraPosition.Z };

            float fLengthSquared = vector.LengthSquared();
            if ( fLengthSquared < 0.0001f )
                vector = cameraForwardVector.HasValue ? -cameraForwardVector.Value : Vector3.Forward;
            else
                Vector3.Multiply( ref vector, (float)( 1f / ( (float)Math.Sqrt( (double)fLengthSquared ) ) ), out vector );

            float num;
            Vector3 vector1 = rotateAxis;
            Vector3.Dot( ref rotateAxis, ref vector, out num );

            Vector3 vector2;
            Vector3 vector3;
            if ( Math.Abs( num ) > 0.9982547f )
            {
                if ( objectForwardVector.HasValue )
                {
                    vector2 = objectForwardVector.Value;
                    Vector3.Dot( ref rotateAxis, ref vector2, out num );

                    if ( Math.Abs( num ) > 0.9982547f )
                    {
                        num = ( ( rotateAxis.X * Vector3.Forward.X ) + ( rotateAxis.Y * Vector3.Forward.Y ) ) + ( rotateAxis.Z * Vector3.Forward.Z );
                        vector2 = ( Math.Abs( num ) > 0.9982547f ) ? Vector3.Right : Vector3.Forward;
                    }
                }
                else
                {
                    num = ( ( rotateAxis.X * Vector3.Forward.X ) + ( rotateAxis.Y * Vector3.Forward.Y ) ) + ( rotateAxis.Z * Vector3.Forward.Z );
                    vector2 = ( Math.Abs( num ) > 0.9982547f ) ? Vector3.Right : Vector3.Forward;
                }

                Vector3.Cross( ref rotateAxis, ref vector2, out vector3 );
                vector3.Normalize();

                Vector3.Cross( ref vector3, ref rotateAxis, out vector2 );
                vector2.Normalize();
            }
            else
            {
                Vector3.Cross( ref rotateAxis, ref vector, out vector3 );
                vector3.Normalize();

                Vector3.Cross( ref vector3, ref vector1, out vector2 );
                vector2.Normalize();
            }

            return new Matrix
            {
                M11 = vector3.X,
                M12 = vector3.Y,
                M13 = vector3.Z,
                M14 = 0f,
                M21 = vector1.X,
                M22 = vector1.Y,
                M23 = vector1.Z,
                M24 = 0f,
                M31 = vector2.X,
                M32 = vector2.Y,
                M33 = vector2.Z,
                M34 = 0f,
                M41 = objectPosition.X,
                M42 = objectPosition.Y,
                M43 = objectPosition.Z,
                M44 = 1f
            };
        }

        /// <summary>
        /// Creates a cylindrical billboard that rotates around a specified axis.
        /// </summary>
        /// <param name="objectPosition">Position of the object the billboard will rotate around.</param>
        /// <param name="cameraPosition">Position of the camera.</param>
        /// <param name="rotateAxis">Axis to rotate the billboard around.</param>
        /// <param name="cameraForwardVector">Optional forward vector of the camera.</param>
        /// <param name="objectForwardVector">Optional forward vector of the object.</param>
        /// <param name="result">[OutAttribute] The created billboard matrix.</param>
        public static void CreateConstrainedBillboard( ref Vector3 objectPosition, ref Vector3 cameraPosition, ref Vector3 rotateAxis, Vector3? cameraForwardVector, Vector3? objectForwardVector, out Matrix result )
        {
            Vector3 vector = new Vector3 { X = objectPosition.X - cameraPosition.X, Y = objectPosition.Y - cameraPosition.Y, Z = objectPosition.Z - cameraPosition.Z };

            float fLengthSquared = vector.LengthSquared();
            if ( fLengthSquared < 0.0001f )
                vector = cameraForwardVector.HasValue ? -cameraForwardVector.Value : Vector3.Forward;
            else
                Vector3.Multiply( ref vector, (float)( 1f / ( (float)Math.Sqrt( (double)fLengthSquared ) ) ), out vector );

            float num;
            Vector3 vector1 = rotateAxis;
            Vector3.Dot( ref rotateAxis, ref vector, out num );

            Vector3 vector2;
            Vector3 vector3;
            if ( Math.Abs( num ) > 0.9982547f )
            {
                if ( objectForwardVector.HasValue )
                {
                    vector2 = objectForwardVector.Value;
                    Vector3.Dot( ref rotateAxis, ref vector2, out num );

                    if ( Math.Abs( num ) > 0.9982547f )
                    {
                        num = ( ( rotateAxis.X * Vector3.Forward.X ) + ( rotateAxis.Y * Vector3.Forward.Y ) ) + ( rotateAxis.Z * Vector3.Forward.Z );
                        vector2 = ( Math.Abs( num ) > 0.9982547f ) ? Vector3.Right : Vector3.Forward;
                    }
                }
                else
                {
                    num = ( ( rotateAxis.X * Vector3.Forward.X ) + ( rotateAxis.Y * Vector3.Forward.Y ) ) + ( rotateAxis.Z * Vector3.Forward.Z );
                    vector2 = ( Math.Abs( num ) > 0.9982547f ) ? Vector3.Right : Vector3.Forward;
                }

                Vector3.Cross( ref rotateAxis, ref vector2, out vector3 );
                vector3.Normalize();

                Vector3.Cross( ref vector3, ref rotateAxis, out vector2 );
                vector2.Normalize();
            }
            else
            {
                Vector3.Cross( ref rotateAxis, ref vector, out vector3 );
                vector3.Normalize();

                Vector3.Cross( ref vector3, ref vector1, out vector2 );
                vector2.Normalize();
            }

            result.M11 = vector3.X;
            result.M12 = vector3.Y;
            result.M13 = vector3.Z;
            result.M14 = 0f;

            result.M21 = vector1.X;
            result.M22 = vector1.Y;
            result.M23 = vector1.Z;
            result.M24 = 0f;

            result.M31 = vector2.X;
            result.M32 = vector2.Y;
            result.M33 = vector2.Z;
            result.M34 = 0f;

            result.M41 = objectPosition.X;
            result.M42 = objectPosition.Y;
            result.M43 = objectPosition.Z;
            result.M44 = 1f;
        }

        /// <summary>
        /// Creates a translation Matrix.
        /// </summary>
        /// <param name="position">Amounts to translate by on the x, y, and z axes.</param>
        public static Matrix CreateTranslation( Vector3 position )
        {
            return new Matrix { M11 = 1f, M12 = 0f, M13 = 0f, M14 = 0f, M21 = 0f, M22 = 1f, M23 = 0f, M24 = 0f, M31 = 0f, M32 = 0f, M33 = 1f, M34 = 0f, M41 = position.X, M42 = position.Y, M43 = position.Z, M44 = 1f };
        }

        /// <summary>
        /// Creates a translation Matrix.
        /// </summary>
        /// <param name="position">Amounts to translate by on the x, y, and z axes.</param>
        /// <param name="result">[OutAttribute] The created translation Matrix.</param>
        public static void CreateTranslation( ref Vector3 position, out Matrix result )
        {
            result.M11 = 1f;
            result.M12 = 0f;
            result.M13 = 0f;
            result.M14 = 0f;

            result.M21 = 0f;
            result.M22 = 1f;
            result.M23 = 0f;
            result.M24 = 0f;

            result.M31 = 0f;
            result.M32 = 0f;
            result.M33 = 1f;
            result.M34 = 0f;

            result.M41 = position.X;
            result.M42 = position.Y;
            result.M43 = position.Z;
            result.M44 = 1f;
        }

        /// <summary>
        /// Creates a translation Matrix.
        /// </summary>
        /// <param name="xPosition">Value to translate by on the x-axis.</param>
        /// <param name="yPosition">Value to translate by on the y-axis.</param>
        /// <param name="zPosition">Value to translate by on the z-axis.</param>
        public static Matrix CreateTranslation( float xPosition, float yPosition, float zPosition )
        {
            return new Matrix { M11 = 1f, M12 = 0f, M13 = 0f, M14 = 0f, M21 = 0f, M22 = 1f, M23 = 0f, M24 = 0f, M31 = 0f, M32 = 0f, M33 = 1f, M34 = 0f, M41 = xPosition, M42 = yPosition, M43 = zPosition, M44 = 1f };
        }

        /// <summary>
        /// Creates a translation Matrix.
        /// </summary>
        /// <param name="xPosition">Value to translate by on the x-axis.</param>
        /// <param name="yPosition">Value to translate by on the y-axis.</param>
        /// <param name="zPosition">Value to translate by on the z-axis.</param>
        /// <param name="result">[OutAttribute] The created translation Matrix.</param>
        public static void CreateTranslation( float xPosition, float yPosition, float zPosition, out Matrix result )
        {
            result.M11 = 1f;
            result.M12 = 0f;
            result.M13 = 0f;
            result.M14 = 0f;

            result.M21 = 0f;
            result.M22 = 1f;
            result.M23 = 0f;
            result.M24 = 0f;

            result.M31 = 0f;
            result.M32 = 0f;
            result.M33 = 1f;
            result.M34 = 0f;

            result.M41 = xPosition;
            result.M42 = yPosition;
            result.M43 = zPosition;
            result.M44 = 1f;
        }

        /// <summary>
        /// Creates a scaling Matrix.
        /// </summary>
        /// <param name="xScale">Value to scale by on the x-axis.</param>
        /// <param name="yScale">Value to scale by on the y-axis.</param>
        /// <param name="zScale">Value to scale by on the z-axis.</param>
        public static Matrix CreateScale( float xScale, float yScale, float zScale )
        {
            return new Matrix
            {
                M11 = xScale,
                M12 = 0f,
                M13 = 0f,
                M14 = 0f,
                M21 = 0f,
                M22 = yScale,
                M23 = 0f,
                M24 = 0f,
                M31 = 0f,
                M32 = 0f,
                M33 = zScale,
                M34 = 0f,
                M41 = 0f,
                M42 = 0f,
                M43 = 0f,
                M44 = 1f
            };
        }

        /// <summary>
        /// Creates a scaling Matrix.
        /// </summary>
        /// <param name="xScale">Value to scale by on the x-axis.</param>
        /// <param name="yScale">Value to scale by on the y-axis.</param>
        /// <param name="zScale">Value to scale by on the z-axis.</param>
        /// <param name="result">[OutAttribute] The created scaling Matrix.</param>
        public static void CreateScale( float xScale, float yScale, float zScale, out Matrix result )
        {
            result.M11 = xScale;
            result.M12 = 0f;
            result.M13 = 0f;
            result.M14 = 0f;

            result.M21 = 0f;
            result.M22 = yScale;
            result.M23 = 0f;
            result.M24 = 0f;

            result.M31 = 0f;
            result.M32 = 0f;
            result.M33 = zScale;
            result.M34 = 0f;

            result.M41 = 0f;
            result.M42 = 0f;
            result.M43 = 0f;
            result.M44 = 1f;
        }

        /// <summary>
        /// Creates a scaling Matrix.
        /// </summary>
        /// <param name="scales">Amounts to scale by on the x, y, and z axes.</param>
        public static Matrix CreateScale( Vector3 scales )
        {
            return new Matrix
            {
                M11 = scales.X,
                M12 = 0f,
                M13 = 0f,
                M14 = 0f,
                M21 = 0f,
                M22 = scales.Y,
                M23 = 0f,
                M24 = 0f,
                M31 = 0f,
                M32 = 0f,
                M33 = scales.Z,
                M34 = 0f,
                M41 = 0f,
                M42 = 0f,
                M43 = 0f,
                M44 = 1f
            };
        }

        /// <summary>
        /// Creates a scaling Matrix.
        /// </summary>
        /// <param name="scales">Amounts to scale by on the x, y, and z axes.</param>
        /// <param name="result">[OutAttribute] The created scaling Matrix.</param>
        public static void CreateScale( ref Vector3 scales, out Matrix result )
        {
            result.M11 = scales.X;
            result.M12 = 0f;
            result.M13 = 0f;
            result.M14 = 0f;

            result.M21 = 0f;
            result.M22 = scales.Y;
            result.M23 = 0f;
            result.M24 = 0f;

            result.M31 = 0f;
            result.M32 = 0f;
            result.M33 = scales.Z;
            result.M34 = 0f;

            result.M41 = 0f;
            result.M42 = 0f;
            result.M43 = 0f;
            result.M44 = 1f;
        }

        /// <summary>
        /// Creates a scaling Matrix.
        /// </summary>
        /// <param name="scale">Amount to scale by.</param>
        public static Matrix CreateScale( float scale )
        {
            return new Matrix
            {
                M11 = scale,
                M12 = 0f,
                M13 = 0f,
                M14 = 0f,
                M21 = 0f,
                M22 = scale,
                M23 = 0f,
                M24 = 0f,
                M31 = 0f,
                M32 = 0f,
                M33 = scale,
                M34 = 0f,
                M41 = 0f,
                M42 = 0f,
                M43 = 0f,
                M44 = 1f
            };
        }

        /// <summary>
        /// Creates a scaling Matrix.
        /// </summary>
        /// <param name="scale">Value to scale by.</param>
        /// <param name="result">[OutAttribute] The created scaling Matrix.</param>
        public static void CreateScale( float scale, out Matrix result )
        {
            result.M11 = scale;
            result.M12 = 0f;
            result.M13 = 0f;
            result.M14 = 0f;

            result.M21 = 0f;
            result.M22 = scale;
            result.M23 = 0f;
            result.M24 = 0f;

            result.M31 = 0f;
            result.M32 = 0f;
            result.M33 = scale;
            result.M34 = 0f;

            result.M41 = 0f;
            result.M42 = 0f;
            result.M43 = 0f;
            result.M44 = 1f;
        }

        /// <summary>
        /// Returns a matrix that can be used to rotate a set of vertices around the x-axis.
        /// </summary>
        /// <param name="radians">The amount, in radians, in which to rotate around the x-axis. Note that you can use ToRadians to convert degrees to radians.</param>
        public static Matrix CreateRotationX( float radians )
        {
            float fCos = (float)Math.Cos( (double)radians );
            float fSin = (float)Math.Sin( (double)radians );

            return new Matrix
            {
                M11 = 1f,
                M12 = 0f,
                M13 = 0f,
                M14 = 0f,
                M21 = 0f,
                M22 = fCos,
                M23 = fSin,
                M24 = 0f,
                M31 = 0f,
                M32 = -fSin,
                M33 = fCos,
                M34 = 0f,
                M41 = 0f,
                M42 = 0f,
                M43 = 0f,
                M44 = 1f
            };
        }

        /// <summary>
        /// Populates data into a user-specified matrix that can be used to rotate a set of vertices around the x-axis.
        /// </summary>
        /// <param name="radians">The amount, in radians, in which to rotate around the x-axis. Note that you can use ToRadians to convert degrees to radians.</param>
        /// <param name="result">[OutAttribute] The matrix in which to place the calculated data.</param>
        public static void CreateRotationX( float radians, out Matrix result )
        {
            float fCos = (float)Math.Cos( (double)radians );
            float fSin = (float)Math.Sin( (double)radians );

            result.M11 = 1f;
            result.M12 = 0f;
            result.M13 = 0f;
            result.M14 = 0f;

            result.M21 = 0f;
            result.M22 = fCos;
            result.M23 = fSin;
            result.M24 = 0f;

            result.M31 = 0f;
            result.M32 = -fSin;
            result.M33 = fCos;
            result.M34 = 0f;

            result.M41 = 0f;
            result.M42 = 0f;
            result.M43 = 0f;
            result.M44 = 1f;
        }

        /// <summary>
        /// Returns a matrix that can be used to rotate a set of vertices around the y-axis.
        /// </summary>
        /// <param name="radians">The amount, in radians, in which to rotate around the y-axis. Note that you can use ToRadians to convert degrees to radians.</param>
        public static Matrix CreateRotationY( float radians )
        {
            float fCos = (float)Math.Cos( (double)radians );
            float fSin = (float)Math.Sin( (double)radians );

            return new Matrix
            {
                M11 = fCos,
                M12 = 0f,
                M13 = -fSin,
                M14 = 0f,
                M21 = 0f,
                M22 = 1f,
                M23 = 0f,
                M24 = 0f,
                M31 = fSin,
                M32 = 0f,
                M33 = fCos,
                M34 = 0f,
                M41 = 0f,
                M42 = 0f,
                M43 = 0f,
                M44 = 1f
            };
        }

        /// <summary>
        /// Populates data into a user-specified matrix that can be used to rotate a set of vertices around the y-axis.
        /// </summary>
        /// <param name="radians">The amount, in radians, in which to rotate around the y-axis. Note that you can use ToRadians to convert degrees to radians.</param>
        /// <param name="result">[OutAttribute] The matrix in which to place the calculated data.</param>
        public static void CreateRotationY( float radians, out Matrix result )
        {
            float fCos = (float)Math.Cos( (double)radians );
            float fSin = (float)Math.Sin( (double)radians );

            result.M11 = fCos;
            result.M12 = 0f;
            result.M13 = -fSin;
            result.M14 = 0f;

            result.M21 = 0f;
            result.M22 = 1f;
            result.M23 = 0f;
            result.M24 = 0f;

            result.M31 = fSin;
            result.M32 = 0f;
            result.M33 = fCos;
            result.M34 = 0f;

            result.M41 = 0f;
            result.M42 = 0f;
            result.M43 = 0f;
            result.M44 = 1f;
        }

        /// <summary>
        /// Returns a matrix that can be used to rotate a set of vertices around the z-axis.
        /// </summary>
        /// <param name="radians">The amount, in radians, in which to rotate around the z-axis. Note that you can use ToRadians to convert degrees to radians.</param>
        public static Matrix CreateRotationZ( float radians )
        {
            float fCos = (float)Math.Cos( (double)radians );
            float fSin = (float)Math.Sin( (double)radians );

            return new Matrix
            {
                M11 = fCos,
                M12 = fSin,
                M13 = 0f,
                M14 = 0f,
                M21 = -fSin,
                M22 = fCos,
                M23 = 0f,
                M24 = 0f,
                M31 = 0f,
                M32 = 0f,
                M33 = 1f,
                M34 = 0f,
                M41 = 0f,
                M42 = 0f,
                M43 = 0f,
                M44 = 1f
            };
        }

        /// <summary>
        /// Populates data into a user-specified matrix that can be used to rotate a set of vertices around the z-axis.
        /// </summary>
        /// <param name="radians">The amount, in radians, in which to rotate around the z-axis. Note that you can use ToRadians to convert degrees to radians.</param>
        /// <param name="result">[OutAttribute] The rotation matrix.</param>
        public static void CreateRotationZ( float radians, out Matrix result )
        {
            float fCos = (float)Math.Cos( (double)radians );
            float fSin = (float)Math.Sin( (double)radians );

            result.M11 = fCos;
            result.M12 = fSin;
            result.M13 = 0f;
            result.M14 = 0f;

            result.M21 = -fSin;
            result.M22 = fCos;
            result.M23 = 0f;
            result.M24 = 0f;

            result.M31 = 0f;
            result.M32 = 0f;
            result.M33 = 1f;
            result.M34 = 0f;

            result.M41 = 0f;
            result.M42 = 0f;
            result.M43 = 0f;
            result.M44 = 1f;
        }

        /// <summary>
        /// Creates a new Matrix that rotates around an arbitrary vector.
        /// </summary>
        /// <param name="axis">The axis to rotate around.</param>
        /// <param name="angle">The angle to rotate around the vector.</param>
        public static Matrix CreateFromAxisAngle( Vector3 axis, float angle )
        {
            float x = axis.X;
            float y = axis.Y;
            float z = axis.Z;

            float fCos = (float)Math.Sin( (double)angle );
            float fSin = (float)Math.Cos( (double)angle );

            float result1 = x * x;
            float result2 = y * y;
            float result3 = z * z;
            float result4 = x * y;
            float result5 = x * z;
            float result6 = y * z;

            return new Matrix
            {
                M11 = result1 + ( fSin * ( 1f - result1 ) ),
                M12 = ( result4 - ( fSin * result4 ) ) + ( fCos * z ),
                M13 = ( result5 - ( fSin * result5 ) ) - ( fCos * y ),
                M14 = 0f,
                M21 = ( result4 - ( fSin * result4 ) ) - ( fCos * z ),
                M22 = result2 + ( fSin * ( 1f - result2 ) ),
                M23 = ( result6 - ( fSin * result6 ) ) + ( fCos * x ),
                M24 = 0f,
                M31 = ( result5 - ( fSin * result5 ) ) + ( fCos * y ),
                M32 = ( result6 - ( fSin * result6 ) ) - ( fCos * x ),
                M33 = result3 + ( fSin * ( 1f - result3 ) ),
                M34 = 0f,
                M41 = 0f,
                M42 = 0f,
                M43 = 0f,
                M44 = 1f
            };
        }

        /// <summary>
        /// Creates a new Matrix that rotates around an arbitrary vector.
        /// </summary>
        /// <param name="axis">The axis to rotate around.</param>
        /// <param name="angle">The angle to rotate around the vector.</param>
        /// <param name="result">[OutAttribute] The created Matrix.</param>
        public static void CreateFromAxisAngle( ref Vector3 axis, float angle, out Matrix result )
        {
            float x = axis.X;
            float y = axis.Y;
            float z = axis.Z;

            float fCos = (float)Math.Sin( (double)angle );
            float fSin = (float)Math.Cos( (double)angle );

            float result1 = x * x;
            float result2 = y * y;
            float result3 = z * z;
            float result4 = x * y;
            float result5 = x * z;
            float result6 = y * z;

            result.M11 = result1 + ( fSin * ( 1f - result1 ) );
            result.M12 = ( result4 - ( fSin * result4 ) ) + ( fCos * z );
            result.M13 = ( result5 - ( fSin * result5 ) ) - ( fCos * y );
            result.M14 = 0f;

            result.M21 = ( result4 - ( fSin * result4 ) ) - ( fCos * z );
            result.M22 = result2 + ( fSin * ( 1f - result2 ) );
            result.M23 = ( result6 - ( fSin * result6 ) ) + ( fCos * x );
            result.M24 = 0f;

            result.M31 = ( result5 - ( fSin * result5 ) ) + ( fCos * y );
            result.M32 = ( result6 - ( fSin * result6 ) ) - ( fCos * x );
            result.M33 = result3 + ( fSin * ( 1f - result3 ) );
            result.M34 = 0f;

            result.M41 = 0f;
            result.M42 = 0f;
            result.M43 = 0f;
            result.M44 = 1f;
        }

        /// <summary>
        /// Builds a perspective projection matrix based on a field of view and returns by value.
        /// </summary>
        /// <param name="fieldOfView">Field of view in the y direction, in radians.</param>
        /// <param name="aspectRatio">Aspect ratio, defined as view space width divided by height. To match the aspect ratio of the viewport, the property AspectRatio.</param>
        /// <param name="nearPlaneDistance">Distance to the near view plane.</param>
        /// <param name="farPlaneDistance">Distance to the far view plane.</param>
        public static Matrix CreatePerspectiveFieldOfView( float fieldOfView, float aspectRatio, float nearPlaneDistance, float farPlaneDistance )
        {
            if ( ( fieldOfView <= 0f ) || ( fieldOfView >= 3.141593f ) )
                throw new ArgumentOutOfRangeException( "fieldOfView", string.Format( CultureInfo.CurrentCulture, "FrameworkResources.OutRangeFieldOfView {0}", new object[] { "fieldOfView" } ) );

            if ( nearPlaneDistance <= 0f )
                throw new ArgumentOutOfRangeException( "nearPlaneDistance", string.Format( CultureInfo.CurrentCulture, "FrameworkResources.NegativePlaneDistance {0}", new object[] { "nearPlaneDistance" } ) );

            if ( farPlaneDistance <= 0f )
                throw new ArgumentOutOfRangeException( "farPlaneDistance", string.Format( CultureInfo.CurrentCulture, "FrameworkResources.NegativePlaneDistance {0}", new object[] { "farPlaneDistance" } ) );

            if ( nearPlaneDistance >= farPlaneDistance )
                throw new ArgumentOutOfRangeException( "nearPlaneDistance", "FrameworkResources.OppositePlanes" );

            float num1 = 1f / ( (float)Math.Tan( (double)( fieldOfView * 0.5f ) ) );
            float num2 = num1 / aspectRatio;

            return new Matrix
            {
                M11 = num2,
                M12 = 0f,
                M13 = 0f,
                M14 = 0f,
                M22 = num1,
                M21 = 0f,
                M23 = 0f,
                M24 = 0f,
                M31 = 0f,
                M32 = 0f,
                M33 = farPlaneDistance / ( nearPlaneDistance - farPlaneDistance ),
                M34 = -1f,
                M41 = 0f,
                M42 = 0f,
                M44 = 0f,
                M43 = ( nearPlaneDistance * farPlaneDistance ) / ( nearPlaneDistance - farPlaneDistance )
            };
        }

        /// <summary>
        /// Builds a perspective projection matrix based on a field of view and returns by reference.
        /// </summary>
        /// <param name="fieldOfView">Field of view in the y direction, in radians.</param>
        /// <param name="aspectRatio">Aspect ratio, defined as view space width divided by height. To match the aspect ratio of the viewport, the property AspectRatio.</param>
        /// <param name="nearPlaneDistance">Distance to the near view plane.</param>
        /// <param name="farPlaneDistance">Distance to the far view plane.</param>
        /// <param name="result">[OutAttribute] The perspective projection matrix.</param>
        public static void CreatePerspectiveFieldOfView( float fieldOfView, float aspectRatio, float nearPlaneDistance, float farPlaneDistance, out Matrix result )
        {
            if ( ( fieldOfView <= 0f ) || ( fieldOfView >= 3.141593f ) )
                throw new ArgumentOutOfRangeException( "fieldOfView", string.Format( CultureInfo.CurrentCulture, "FrameworkResources.OutRangeFieldOfView {0}", new object[] { "fieldOfView" } ) );

            if ( nearPlaneDistance <= 0f )
                throw new ArgumentOutOfRangeException( "nearPlaneDistance", string.Format( CultureInfo.CurrentCulture, "FrameworkResources.NegativePlaneDistance {0}", new object[] { "nearPlaneDistance" } ) );

            if ( farPlaneDistance <= 0f )
                throw new ArgumentOutOfRangeException( "farPlaneDistance", string.Format( CultureInfo.CurrentCulture, "FrameworkResources.NegativePlaneDistance {0}", new object[] { "farPlaneDistance" } ) );

            if ( nearPlaneDistance >= farPlaneDistance )
                throw new ArgumentOutOfRangeException( "nearPlaneDistance", "FrameworkResources.OppositePlanes" );

            float num1 = 1f / ( (float)Math.Tan( (double)( fieldOfView * 0.5f ) ) );
            float num2 = num1 / aspectRatio;

            result.M11 = num2;
            result.M12 = result.M13 = result.M14 = 0f;

            result.M22 = num1;
            result.M21 = result.M23 = result.M24 = 0f;

            result.M31 = result.M32 = 0f;
            result.M33 = farPlaneDistance / ( nearPlaneDistance - farPlaneDistance );
            result.M34 = -1f;

            result.M41 = result.M42 = result.M44 = 0f;
            result.M43 = ( nearPlaneDistance * farPlaneDistance ) / ( nearPlaneDistance - farPlaneDistance );
        }

        /// <summary>
        /// Builds a perspective projection matrix and returns the result by value.
        /// </summary>
        /// <param name="width">Width of the view volume at the near view plane.</param>
        /// <param name="height">Height of the view volume at the near view plane.</param>
        /// <param name="nearPlaneDistance">Distance to the near view plane.</param>
        /// <param name="farPlaneDistance">Distance to the far view plane.</param>
        public static Matrix CreatePerspective( float width, float height, float nearPlaneDistance, float farPlaneDistance )
        {
            if ( nearPlaneDistance <= 0f )
                throw new ArgumentOutOfRangeException( "nearPlaneDistance", string.Format( CultureInfo.CurrentCulture, "FrameworkResources.NegativePlaneDistance {0}", new object[] { "nearPlaneDistance" } ) );

            if ( farPlaneDistance <= 0f )
                throw new ArgumentOutOfRangeException( "farPlaneDistance", string.Format( CultureInfo.CurrentCulture, "FrameworkResources.NegativePlaneDistance {0}", new object[] { "farPlaneDistance" } ) );

            if ( nearPlaneDistance >= farPlaneDistance )
                throw new ArgumentOutOfRangeException( "nearPlaneDistance", "FrameworkResources.OppositePlanes" );

            return new Matrix
            {
                M11 = ( 2f * nearPlaneDistance ) / width,
                M12 = 0f,
                M13 = 0f,
                M14 = 0f,
                M22 = ( 2f * nearPlaneDistance ) / height,
                M21 = 0f,
                M23 = 0f,
                M24 = 0f,
                M33 = farPlaneDistance / ( nearPlaneDistance - farPlaneDistance ),
                M31 = 0f,
                M32 = 0f,
                M34 = -1f,
                M41 = 0f,
                M42 = 0f,
                M44 = 0f,
                M43 = ( nearPlaneDistance * farPlaneDistance ) / ( nearPlaneDistance - farPlaneDistance )
            };
        }

        /// <summary>
        /// Builds a perspective projection matrix and returns the result by reference.
        /// </summary>
        /// <param name="width">Width of the view volume at the near view plane.</param>
        /// <param name="height">Height of the view volume at the near view plane.</param>
        /// <param name="nearPlaneDistance">Distance to the near view plane.</param>
        /// <param name="farPlaneDistance">Distance to the far view plane.</param>
        /// <param name="result">[OutAttribute] The projection matrix.</param>
        public static void CreatePerspective( float width, float height, float nearPlaneDistance, float farPlaneDistance, out Matrix result )
        {
            if ( nearPlaneDistance <= 0f )
                throw new ArgumentOutOfRangeException( "nearPlaneDistance", string.Format( CultureInfo.CurrentCulture, "FrameworkResources.NegativePlaneDistance {0}", new object[] { "nearPlaneDistance" } ) );

            if ( farPlaneDistance <= 0f )
                throw new ArgumentOutOfRangeException( "farPlaneDistance", string.Format( CultureInfo.CurrentCulture, "FrameworkResources.NegativePlaneDistance {0}", new object[] { "farPlaneDistance" } ) );

            if ( nearPlaneDistance >= farPlaneDistance )
                throw new ArgumentOutOfRangeException( "nearPlaneDistance", "FrameworkResources.OppositePlanes" );

            result.M11 = ( 2f * nearPlaneDistance ) / width;
            result.M12 = result.M13 = result.M14 = 0f;

            result.M22 = ( 2f * nearPlaneDistance ) / height;
            result.M21 = result.M23 = result.M24 = 0f;

            result.M33 = farPlaneDistance / ( nearPlaneDistance - farPlaneDistance );
            result.M31 = result.M32 = 0f;
            result.M34 = -1f;

            result.M41 = result.M42 = result.M44 = 0f;
            result.M43 = ( nearPlaneDistance * farPlaneDistance ) / ( nearPlaneDistance - farPlaneDistance );
        }

        /// <summary>
        /// Builds a customized, perspective projection matrix.
        /// </summary>
        /// <param name="left">Minimum x-value of the view volume at the near view plane.</param>
        /// <param name="right">Maximum x-value of the view volume at the near view plane.</param>
        /// <param name="bottom">Minimum y-value of the view volume at the near view plane.</param>
        /// <param name="top">Maximum y-value of the view volume at the near view plane.</param>
        /// <param name="nearPlaneDistance">Distance to the near view plane.</param>
        /// <param name="farPlaneDistance">Distance to of the far view plane.</param>
        public static Matrix CreatePerspectiveOffCenter( float left, float right, float bottom, float top, float nearPlaneDistance, float farPlaneDistance )
        {
            if ( nearPlaneDistance <= 0f )
            {
                throw new ArgumentOutOfRangeException( "nearPlaneDistance", string.Format( CultureInfo.CurrentCulture, "FrameworkResources.NegativePlaneDistance {0}", new object[] { "nearPlaneDistance" } ) );
            }
            if ( farPlaneDistance <= 0f )
            {
                throw new ArgumentOutOfRangeException( "farPlaneDistance", string.Format( CultureInfo.CurrentCulture, "FrameworkResources.NegativePlaneDistance {0}", new object[] { "farPlaneDistance" } ) );
            }
            if ( nearPlaneDistance >= farPlaneDistance )
            {
                throw new ArgumentOutOfRangeException( "nearPlaneDistance", "FrameworkResources.OppositePlanes" );
            }

            return new Matrix
            {
                M11 = ( 2f * nearPlaneDistance ) / ( right - left ),
                M12 = 0f,
                M13 = 0f,
                M14 = 0f,
                M22 = ( 2f * nearPlaneDistance ) / ( top - bottom ),
                M21 = 0f,
                M23 = 0f,
                M24 = 0f,
                M31 = ( left + right ) / ( right - left ),
                M32 = ( top + bottom ) / ( top - bottom ),
                M33 = farPlaneDistance / ( nearPlaneDistance - farPlaneDistance ),
                M34 = -1f,
                M43 = ( nearPlaneDistance * farPlaneDistance ) / ( nearPlaneDistance - farPlaneDistance ),
                M41 = 0f,
                M42 = 0f,
                M44 = 0f
            };
        }

        /// <summary>
        /// Builds a customized, perspective projection matrix.
        /// </summary>
        /// <param name="left">Minimum x-value of the view volume at the near view plane.</param>
        /// <param name="right">Maximum x-value of the view volume at the near view plane.</param>
        /// <param name="bottom">Minimum y-value of the view volume at the near view plane.</param>
        /// <param name="top">Maximum y-value of the view volume at the near view plane.</param>
        /// <param name="nearPlaneDistance">Distance to the near view plane.</param>
        /// <param name="farPlaneDistance">Distance to of the far view plane.</param>
        /// <param name="result">[OutAttribute] The created projection matrix.</param>
        public static void CreatePerspectiveOffCenter( float left, float right, float bottom, float top, float nearPlaneDistance, float farPlaneDistance, out Matrix result )
        {
            if ( nearPlaneDistance <= 0f )
                throw new ArgumentOutOfRangeException( "nearPlaneDistance", string.Format( CultureInfo.CurrentCulture, "FrameworkResources.NegativePlaneDistance {0}", new object[] { "nearPlaneDistance" } ) );

            if ( farPlaneDistance <= 0f )
                throw new ArgumentOutOfRangeException( "farPlaneDistance", string.Format( CultureInfo.CurrentCulture, "FrameworkResources.NegativePlaneDistance {0}", new object[] { "farPlaneDistance" } ) );

            if ( nearPlaneDistance >= farPlaneDistance )
                throw new ArgumentOutOfRangeException( "nearPlaneDistance", "FrameworkResources.OppositePlanes" );

            result.M11 = ( 2f * nearPlaneDistance ) / ( right - left );
            result.M12 = result.M13 = result.M14 = 0f;

            result.M21 = result.M23 = result.M24 = 0f;
            result.M22 = ( 2f * nearPlaneDistance ) / ( top - bottom );

            result.M31 = ( left + right ) / ( right - left );
            result.M32 = ( top + bottom ) / ( top - bottom );
            result.M33 = farPlaneDistance / ( nearPlaneDistance - farPlaneDistance );
            result.M34 = -1f;

            result.M41 = result.M42 = result.M44 = 0f;
            result.M43 = ( nearPlaneDistance * farPlaneDistance ) / ( nearPlaneDistance - farPlaneDistance );
        }

        /// <summary>
        /// Builds an orthogonal projection matrix.
        /// </summary>
        /// <param name="width">Width of the view volume.</param>
        /// <param name="height">Height of the view volume.</param>
        /// <param name="zNearPlane">Minimum z-value of the view volume.</param>
        /// <param name="zFarPlane">Maximum z-value of the view volume.</param>
        public static Matrix CreateOrthographic( float width, float height, float zNearPlane, float zFarPlane )
        {
            return new Matrix
            {
                M11 = 2f / width,
                M12 = 0f,
                M13 = 0f,
                M14 = 0f,
                M22 = 2f / height,
                M21 = 0f,
                M23 = 0f,
                M24 = 0f,
                M33 = 1f / ( zNearPlane - zFarPlane ),
                M31 = 0f,
                M32 = 0f,
                M34 = 0f,
                M41 = 0f,
                M42 = 0f,
                M43 = zNearPlane / ( zNearPlane - zFarPlane ),
                M44 = 1f
            };
        }

        /// <summary>
        /// Builds an orthogonal projection matrix.
        /// </summary>
        /// <param name="width">Width of the view volume.</param>
        /// <param name="height">Height of the view volume.</param>
        /// <param name="zNearPlane">Minimum z-value of the view volume.</param>
        /// <param name="zFarPlane">Maximum z-value of the view volume.</param>
        /// <param name="result">[OutAttribute] The projection matrix.</param>
        public static void CreateOrthographic( float width, float height, float zNearPlane, float zFarPlane, out Matrix result )
        {
            result.M11 = 2f / width;
            result.M12 = result.M13 = result.M14 = 0f;

            result.M22 = 2f / height;
            result.M21 = result.M23 = result.M24 = 0f;

            result.M33 = 1f / ( zNearPlane - zFarPlane );
            result.M31 = result.M32 = result.M34 = 0f;

            result.M41 = result.M42 = 0f;
            result.M43 = zNearPlane / ( zNearPlane - zFarPlane );
            result.M44 = 1f;
        }

        /// <summary>
        /// Builds a customized, orthogonal projection matrix.
        /// </summary>
        /// <param name="left">Minimum x-value of the view volume.</param>
        /// <param name="right">Maximum x-value of the view volume.</param>
        /// <param name="bottom">Minimum y-value of the view volume.</param>
        /// <param name="top">Maximum y-value of the view volume.</param>
        /// <param name="zNearPlane">Minimum z-value of the view volume.</param>
        /// <param name="zFarPlane">Maximum z-value of the view volume.</param>
        public static Matrix CreateOrthographicOffCenter( float left, float right, float bottom, float top, float zNearPlane, float zFarPlane )
        {
            return new Matrix
            {
                M11 = 2f / ( right - left ),
                M12 = 0f,
                M13 = 0f,
                M14 = 0f,
                M22 = 2f / ( top - bottom ),
                M21 = 0f,
                M23 = 0f,
                M24 = 0f,
                M33 = 1f / ( zNearPlane - zFarPlane ),
                M31 = 0f,
                M32 = 0f,
                M34 = 0f,
                M41 = ( left + right ) / ( left - right ),
                M42 = ( top + bottom ) / ( bottom - top ),
                M43 = zNearPlane / ( zNearPlane - zFarPlane ),
                M44 = 1f
            };
        }

        /// <summary>
        /// Builds a customized, orthogonal projection matrix.
        /// </summary>
        /// <param name="left">Minimum x-value of the view volume.</param>
        /// <param name="right">Maximum x-value of the view volume.</param>
        /// <param name="bottom">Minimum y-value of the view volume.</param>
        /// <param name="top">Maximum y-value of the view volume.</param>
        /// <param name="zNearPlane">Minimum z-value of the view volume.</param>
        /// <param name="zFarPlane">Maximum z-value of the view volume.</param>
        /// <param name="result">[OutAttribute] The projection matrix.</param>
        public static void CreateOrthographicOffCenter( float left, float right, float bottom, float top, float zNearPlane, float zFarPlane, out Matrix result )
        {
            result.M11 = 2f / ( right - left );
            result.M12 = result.M13 = result.M14 = 0f;

            result.M22 = 2f / ( top - bottom );
            result.M21 = result.M23 = result.M24 = 0f;

            result.M33 = 1f / ( zNearPlane - zFarPlane );
            result.M31 = result.M32 = result.M34 = 0f;

            result.M41 = ( left + right ) / ( left - right );
            result.M42 = ( top + bottom ) / ( bottom - top );
            result.M43 = zNearPlane / ( zNearPlane - zFarPlane );
            result.M44 = 1f;
        }

        /// <summary>
        /// Creates a view matrix.
        /// </summary>
        /// <param name="cameraPosition">The position of the camera.</param>
        /// <param name="cameraTarget">The target towards which the camera is pointing.</param>
        /// <param name="cameraUpVector">The direction that is "up" from the camera's point of view.</param>
        public static Matrix CreateLookAt( Vector3 cameraPosition, Vector3 cameraTarget, Vector3 cameraUpVector )
        {
            Vector3 vector1 = Vector3.Normalize( cameraPosition - cameraTarget );
            Vector3 vector2 = Vector3.Normalize( Vector3.Cross( cameraUpVector, vector1 ) );
            Vector3 vector3 = Vector3.Cross( vector1, vector2 );

            return new Matrix
            {
                M11 = vector2.X,
                M12 = vector3.X,
                M13 = vector1.X,
                M14 = 0f,
                M21 = vector2.Y,
                M22 = vector3.Y,
                M23 = vector1.Y,
                M24 = 0f,
                M31 = vector2.Z,
                M32 = vector3.Z,
                M33 = vector1.Z,
                M34 = 0f,
                M41 = -Vector3.Dot( vector2, cameraPosition ),
                M42 = -Vector3.Dot( vector3, cameraPosition ),
                M43 = -Vector3.Dot( vector1, cameraPosition ),
                M44 = 1f
            };
        }

        /// <summary>
        /// Creates a view matrix.
        /// </summary>
        /// <param name="cameraPosition">The position of the camera.</param>
        /// <param name="cameraTarget">The target towards which the camera is pointing.</param>
        /// <param name="cameraUpVector">The direction that is "up" from the camera's point of view.</param>
        /// <param name="result">[OutAttribute] The created view matrix.</param>
        public static void CreateLookAt( ref Vector3 cameraPosition, ref Vector3 cameraTarget, ref Vector3 cameraUpVector, out Matrix result )
        {
            Vector3 vector1 = Vector3.Normalize( cameraPosition - cameraTarget );
            Vector3 vector2 = Vector3.Normalize( Vector3.Cross( cameraUpVector, vector1 ) );
            Vector3 vector3 = Vector3.Cross( vector1, vector2 );

            result.M11 = vector2.X;
            result.M12 = vector3.X;
            result.M13 = vector1.X;
            result.M14 = 0f;

            result.M21 = vector2.Y;
            result.M22 = vector3.Y;
            result.M23 = vector1.Y;
            result.M24 = 0f;

            result.M31 = vector2.Z;
            result.M32 = vector3.Z;
            result.M33 = vector1.Z;
            result.M34 = 0f;

            result.M41 = -Vector3.Dot( vector2, cameraPosition );
            result.M42 = -Vector3.Dot( vector3, cameraPosition );
            result.M43 = -Vector3.Dot( vector1, cameraPosition );
            result.M44 = 1f;
        }

        /// <summary>
        /// Creates a world matrix with the specified parameters.
        /// </summary>
        /// <param name="position">Position of the object. This value is used in translation operations.</param>
        /// <param name="forward">Forward direction of the object.</param>
        /// <param name="up">Upward direction of the object; usually [0, 1, 0].</param>
        public static Matrix CreateWorld( Vector3 position, Vector3 forward, Vector3 up )
        {
            Vector3 vector1 = Vector3.Normalize( -forward );
            Vector3 vector2 = Vector3.Normalize( Vector3.Cross( up, vector1 ) );
            Vector3 vector3 = Vector3.Cross( vector1, vector2 );

            return new Matrix
            {
                M11 = vector2.X,
                M12 = vector2.Y,
                M13 = vector2.Z,
                M14 = 0f,
                M21 = vector3.X,
                M22 = vector3.Y,
                M23 = vector3.Z,
                M24 = 0f,
                M31 = vector1.X,
                M32 = vector1.Y,
                M33 = vector1.Z,
                M34 = 0f,
                M41 = position.X,
                M42 = position.Y,
                M43 = position.Z,
                M44 = 1f
            };
        }

        /// <summary>
        /// Creates a world matrix with the specified parameters.
        /// </summary>
        /// <param name="position">Position of the object. This value is used in translation operations.</param>
        /// <param name="forward">Forward direction of the object.</param>
        /// <param name="up">Upward direction of the object; usually [0, 1, 0].</param>
        /// <param name="result">[OutAttribute] The created world matrix.</param>
        public static void CreateWorld( ref Vector3 position, ref Vector3 forward, ref Vector3 up, out Matrix result )
        {
            Vector3 vector1 = Vector3.Normalize( -forward );
            Vector3 vector2 = Vector3.Normalize( Vector3.Cross( up, vector1 ) );
            Vector3 vector3 = Vector3.Cross( vector1, vector2 );

            result.M11 = vector2.X;
            result.M12 = vector2.Y;
            result.M13 = vector2.Z;
            result.M14 = 0f;

            result.M21 = vector3.X;
            result.M22 = vector3.Y;
            result.M23 = vector3.Z;
            result.M24 = 0f;

            result.M31 = vector1.X;
            result.M32 = vector1.Y;
            result.M33 = vector1.Z;
            result.M34 = 0f;

            result.M41 = position.X;
            result.M42 = position.Y;
            result.M43 = position.Z;
            result.M44 = 1f;
        }

        /// <summary>
        /// Creates a rotation Matrix from a Quaternion.
        /// </summary>
        /// <param name="quaternion">Quaternion to create the Matrix from.</param>
        public static Matrix CreateFromQuaternion( Quaternion quaternion )
        {
            float xx = quaternion.X * quaternion.X;
            float yy = quaternion.Y * quaternion.Y;
            float zz = quaternion.Z * quaternion.Z;
            float xy = quaternion.X * quaternion.Y;
            float zw = quaternion.Z * quaternion.W;
            float zx = quaternion.Z * quaternion.X;
            float yw = quaternion.Y * quaternion.W;
            float yz = quaternion.Y * quaternion.Z;
            float xw = quaternion.X * quaternion.W;

            return new Matrix
            {
                M11 = 1f - ( 2f * ( yy + zz ) ),
                M12 = 2f * ( xy + zw ),
                M13 = 2f * ( zx - yw ),
                M14 = 0f,
                M21 = 2f * ( xy - zw ),
                M22 = 1f - ( 2f * ( zz + xx ) ),
                M23 = 2f * ( yz + xw ),
                M24 = 0f,
                M31 = 2f * ( zx + yw ),
                M32 = 2f * ( yz - xw ),
                M33 = 1f - ( 2f * ( yy + xx ) ),
                M34 = 0f,
                M41 = 0f,
                M42 = 0f,
                M43 = 0f,
                M44 = 1f
            };
        }

        /// <summary>
        /// Creates a rotation Matrix from a Quaternion.
        /// </summary>
        /// <param name="quaternion">Quaternion to create the Matrix from.</param>
        /// <param name="result">[OutAttribute] The created Matrix.</param>
        public static void CreateFromQuaternion( ref Quaternion quaternion, out Matrix result )
        {
            float xx = quaternion.X * quaternion.X;
            float yy = quaternion.Y * quaternion.Y;
            float zz = quaternion.Z * quaternion.Z;
            float xy = quaternion.X * quaternion.Y;
            float zw = quaternion.Z * quaternion.W;
            float zx = quaternion.Z * quaternion.X;
            float yw = quaternion.Y * quaternion.W;
            float yz = quaternion.Y * quaternion.Z;
            float xw = quaternion.X * quaternion.W;

            result.M11 = 1f - ( 2f * ( yy + zz ) );
            result.M12 = 2f * ( xy + zw );
            result.M13 = 2f * ( zx - yw );
            result.M14 = 0f;

            result.M21 = 2f * ( xy - zw );
            result.M22 = 1f - ( 2f * ( zz + xx ) );
            result.M23 = 2f * ( yz + xw );
            result.M24 = 0f;

            result.M31 = 2f * ( zx + yw );
            result.M32 = 2f * ( yz - xw );
            result.M33 = 1f - ( 2f * ( yy + xx ) );
            result.M34 = 0f;

            result.M41 = 0f;
            result.M42 = 0f;
            result.M43 = 0f;
            result.M44 = 1f;
        }

        /// <summary>
        /// Creates a new rotation matrix from a specified yaw, pitch, and roll.
        /// </summary>
        /// <param name="yaw">Angle of rotation, in radians, around the y-axis.</param>
        /// <param name="pitch">Angle of rotation, in radians, around the x-axis.</param>
        /// <param name="roll">Angle of rotation, in radians, around the z-axis.</param>
        public static Matrix CreateFromYawPitchRoll( float yaw, float pitch, float roll )
        {
            Quaternion quaternion;
            Quaternion.CreateFromYawPitchRoll( yaw, pitch, roll, out quaternion );

            Matrix matrix;
            CreateFromQuaternion( ref quaternion, out matrix );

            return matrix;
        }

        /// <summary>
        /// Fills in a rotation matrix from a specified yaw, pitch, and roll.
        /// </summary>
        /// <param name="yaw">Angle of rotation, in radians, around the y-axis.</param>
        /// <param name="pitch">Angle of rotation, in radians, around the x-axis.</param>
        /// <param name="roll">Angle of rotation, in radians, around the z-axis.</param>
        /// <param name="result">[OutAttribute] An existing matrix filled in to represent the specified yaw, pitch, and roll.</param>
        public static void CreateFromYawPitchRoll( float yaw, float pitch, float roll, out Matrix result )
        {
            Quaternion quaternion;
            Quaternion.CreateFromYawPitchRoll( yaw, pitch, roll, out quaternion );

            CreateFromQuaternion( ref quaternion, out result );
        }

        /// <summary>
        /// Creates a Matrix that flattens geometry into a specified Plane as if casting a shadow from a specified light source.
        /// </summary>
        /// <param name="lightDirection">A Vector3 specifying the direction from which the light that will cast the shadow is coming.</param>
        /// <param name="plane">The Plane onto which the new matrix should flatten geometry so as to cast a shadow.</param>
        public static Matrix CreateShadow( Vector3 lightDirection, Plane plane )
        {
            Plane outPlane;
            Plane.Normalize( ref plane, out outPlane );

            float num = ( ( outPlane.Normal.X * lightDirection.X ) + ( outPlane.Normal.Y * lightDirection.Y ) ) + ( outPlane.Normal.Z * lightDirection.Z );

            float planeX = -outPlane.Normal.X;
            float planeY = -outPlane.Normal.Y;
            float planeZ = -outPlane.Normal.Z;
            float planeD = -outPlane.D;

            return new Matrix
            {
                M11 = ( planeX * lightDirection.X ) + num,
                M12 = planeX * lightDirection.Y,
                M13 = planeX * lightDirection.Z,
                M14 = 0f,

                M21 = planeY * lightDirection.X,
                M22 = ( planeY * lightDirection.Y ) + num,
                M23 = planeY * lightDirection.Z,
                M24 = 0f,

                M31 = planeZ * lightDirection.X,
                M32 = planeZ * lightDirection.Y,
                M33 = ( planeZ * lightDirection.Z ) + num,
                M34 = 0f,

                M41 = planeD * lightDirection.X,
                M42 = planeD * lightDirection.Y,
                M43 = planeD * lightDirection.Z,
                M44 = num
            };
        }

        /// <summary>
        /// Fills in a Matrix to flatten geometry into a specified Plane as if casting a shadow from a specified light source.
        /// </summary>
        /// <param name="lightDirection">A Vector3 specifying the direction from which the light that will cast the shadow is coming.</param>
        /// <param name="plane">The Plane onto which the new matrix should flatten geometry so as to cast a shadow.</param>
        /// <param name="result">[OutAttribute] A Matrix that can be used to flatten geometry onto the specified plane from the specified direction.</param>
        public static void CreateShadow( ref Vector3 lightDirection, ref Plane plane, out Matrix result )
        {
            Plane outPlane;
            Plane.Normalize( ref plane, out outPlane );

            float num = ( ( outPlane.Normal.X * lightDirection.X ) + ( outPlane.Normal.Y * lightDirection.Y ) ) + ( outPlane.Normal.Z * lightDirection.Z );

            float planeX = -outPlane.Normal.X;
            float planeY = -outPlane.Normal.Y;
            float planeZ = -outPlane.Normal.Z;
            float planeD = -outPlane.D;

            result.M11 = ( planeX * lightDirection.X ) + num;
            result.M12 = planeX * lightDirection.Y;
            result.M13 = planeX * lightDirection.Z;
            result.M14 = 0f;

            result.M21 = planeY * lightDirection.X;
            result.M22 = ( planeY * lightDirection.Y ) + num;
            result.M23 = planeY * lightDirection.Z;
            result.M24 = 0f;

            result.M31 = planeZ * lightDirection.X;
            result.M32 = planeZ * lightDirection.Y;
            result.M33 = ( planeZ * lightDirection.Z ) + num;
            result.M34 = 0f;

            result.M41 = planeD * lightDirection.X;
            result.M42 = planeD * lightDirection.Y;
            result.M43 = planeD * lightDirection.Z;
            result.M44 = num;
        }

        /// <summary>
        /// Creates a Matrix that reflects the coordinate system about a specified Plane.
        /// </summary>
        /// <param name="value">The Plane about which to create a reflection.</param>
        public static Matrix CreateReflection( Plane value )
        {
            value.Normalize();

            float x = value.Normal.X;
            float y = value.Normal.Y;
            float z = value.Normal.Z;

            float numX = -2f * x;
            float numY = -2f * y;
            float numZ = -2f * z;

            return new Matrix
            {
                M11 = ( numX * x ) + 1f,
                M12 = numY * x,
                M13 = numZ * x,
                M14 = 0f,

                M21 = numX * y,
                M22 = ( numY * y ) + 1f,
                M23 = numZ * y,
                M24 = 0f,

                M31 = numX * z,
                M32 = numY * z,
                M33 = ( numZ * z ) + 1f,
                M34 = 0f,

                M41 = numX * value.D,
                M42 = numY * value.D,
                M43 = numZ * value.D,
                M44 = 1f
            };
        }

        /// <summary>
        /// Fills in an existing Matrix so that it reflects the coordinate system about a specified Plane.
        /// </summary>
        /// <param name="value">The Plane about which to create a reflection.</param>
        /// <param name="result">[OutAttribute] A Matrix that creates the reflection.</param>
        public static void CreateReflection( ref Plane value, out Matrix result )
        {
            Plane outPlane;
            Plane.Normalize( ref value, out outPlane );

            value.Normalize();

            float x = outPlane.Normal.X;
            float y = outPlane.Normal.Y;
            float z = outPlane.Normal.Z;

            float numX = -2f * x;
            float numY = -2f * y;
            float numZ = -2f * z;

            result.M11 = ( numX * x ) + 1f;
            result.M12 = numY * x;
            result.M13 = numZ * x;
            result.M14 = 0f;

            result.M21 = numX * y;
            result.M22 = ( numY * y ) + 1f;
            result.M23 = numZ * y;
            result.M24 = 0f;

            result.M31 = numX * z;
            result.M32 = numY * z;
            result.M33 = ( numZ * z ) + 1f;
            result.M34 = 0f;

            result.M41 = numX * outPlane.D;
            result.M42 = numY * outPlane.D;
            result.M43 = numZ * outPlane.D;
            result.M44 = 1f;
        }

        ///// <summary>
        ///// Extracts the scalar, translation, and rotation components from a 3D scale/rotate/translate (SRT) Matrix.  Reference page contains code sample.
        ///// </summary>
        ///// <param name="scale">[OutAttribute] The scalar component of the transform matrix, expressed as a Vector3.</param>
        ///// <param name="rotation">[OutAttribute] The rotation component of the transform matrix, expressed as a Quaternion.</param>
        ///// <param name="translation">[OutAttribute] The translation component of the transform matrix, expressed as a Vector3.</param>
        //public unsafe bool Decompose(out Vector3 scale, out Quaternion rotation, out Vector3 translation)
        //{
        //    bool flag = true;
        //    fixed (float* numRef = &scale.X)
        //    {
        //        uint num;
        //        uint num3;
        //        uint num4;
        //        VectorBasis basis2;
        //        Vector3** vectorPtr = (Vector3**) &basis2;
        //        Matrix identity = Identity;
        //        CanonicalBasis basis = new CanonicalBasis();
        //        Vector3* vectorPtr2 = &basis.Row0;
        //        basis.Row0 = new Vector3(1f, 0f, 0f);
        //        basis.Row1 = new Vector3(0f, 1f, 0f);
        //        basis.Row2 = new Vector3(0f, 0f, 1f);
        //        translation.X = M41;
        //        translation.Y = M42;
        //        translation.Z = M43;
        //        *((IntPtr*) vectorPtr) = (IntPtr) &identity.M11;
        //        *((IntPtr*) (vectorPtr + 1)) = (IntPtr) &identity.M21;
        //        *((IntPtr*) (vectorPtr + 2)) = (IntPtr) &identity.M31;
        //        *(*((IntPtr*) vectorPtr)) = new Vector3(M11, M12, M13);
        //        *(*((IntPtr*) (vectorPtr + 1))) = new Vector3(M21, M22, M23);
        //        *(*((IntPtr*) (vectorPtr + 2))) = new Vector3(M31, M32, M33);
        //        scale.X = *(((IntPtr*) vectorPtr)).Length();
        //        scale.Y = *(((IntPtr*) (vectorPtr + 1))).Length();
        //        scale.Z = *(((IntPtr*) (vectorPtr + 2))).Length();
        //        float num11 = numRef[0];
        //        float num10 = numRef[4];
        //        float num7 = numRef[8];
        //        if (num11 < num10)
        //        {
        //            if (num10 < num7)
        //            {
        //                num = 2;
        //                num3 = 1;
        //                num4 = 0;
        //            }
        //            else
        //            {
        //                num = 1;
        //                if (num11 < num7)
        //                {
        //                    num3 = 2;
        //                    num4 = 0;
        //                }
        //                else
        //                {
        //                    num3 = 0;
        //                    num4 = 2;
        //                }
        //            }
        //        }
        //        else if (num11 < num7)
        //        {
        //            num = 2;
        //            num3 = 0;
        //            num4 = 1;
        //        }
        //        else
        //        {
        //            num = 0;
        //            if (num10 < num7)
        //            {
        //                num3 = 2;
        //                num4 = 1;
        //            }
        //            else
        //            {
        //                num3 = 1;
        //                num4 = 2;
        //            }
        //        }
        //        if (numRef[num * 4] < 0.0001f)
        //        {
        //            *(*((IntPtr*) (vectorPtr + (num * sizeof(Vector3*))))) = vectorPtr2[(int) (num * sizeof(Vector3))];
        //        }
        //        *(((IntPtr*) (vectorPtr + (num * sizeof(Vector3*))))).Normalize();
        //        if (numRef[num3 * 4] < 0.0001f)
        //        {
        //            uint num5;
        //            float num9 = Math.Abs(*(((IntPtr*) (vectorPtr + (num * sizeof(Vector3*))))).X);
        //            float num8 = Math.Abs(*(((IntPtr*) (vectorPtr + (num * sizeof(Vector3*))))).Y);
        //            float num6 = Math.Abs(*(((IntPtr*) (vectorPtr + (num * sizeof(Vector3*))))).Z);
        //            if (num9 < num8)
        //            {
        //                if (num8 < num6)
        //                {
        //                    num5 = 0;
        //                }
        //                else if (num9 < num6)
        //                {
        //                    num5 = 0;
        //                }
        //                else
        //                {
        //                    num5 = 2;
        //                }
        //            }
        //            else if (num9 < num6)
        //            {
        //                num5 = 1;
        //            }
        //            else if (num8 < num6)
        //            {
        //                num5 = 1;
        //            }
        //            else
        //            {
        //                num5 = 2;
        //            }
        //            Vector3.Cross(ref (Vector3) ref *(((IntPtr*) (vectorPtr + (num3 * sizeof(Vector3*))))), ref (Vector3) ref *(((IntPtr*) (vectorPtr + (num * sizeof(Vector3*))))), out (Vector3) ref (vectorPtr2 + (num5 * sizeof(Vector3))));
        //        }
        //        *(((IntPtr*) (vectorPtr + (num3 * sizeof(Vector3*))))).Normalize();
        //        if (numRef[num4 * 4] < 0.0001f)
        //        {
        //            Vector3.Cross(ref (Vector3) ref *(((IntPtr*) (vectorPtr + (num4 * sizeof(Vector3*))))), ref (Vector3) ref *(((IntPtr*) (vectorPtr + (num * sizeof(Vector3*))))), out (Vector3) ref *(((IntPtr*) (vectorPtr + (num3 * sizeof(Vector3*))))));
        //        }
        //        *(((IntPtr*) (vectorPtr + (num4 * sizeof(Vector3*))))).Normalize();
        //        float num2 = identity.Determinant();
        //        if (num2 < 0f)
        //        {
        //            numRef[num * 4] = -numRef[num * 4];
        //            *(*((IntPtr*) (vectorPtr + (num * sizeof(Vector3*))))) = -*(*(((IntPtr*) (vectorPtr + (num * sizeof(Vector3*))))));
        //            num2 = -num2;
        //        }
        //        num2--;
        //        num2 *= num2;
        //        if (0.0001f < num2)
        //        {
        //            rotation = Quaternion.Identity;
        //            flag = false;
        //        }
        //        else
        //        {
        //            Quaternion.CreateFromRotationMatrix(ref identity, out rotation);
        //        }
        //    }
        //    return flag;
        //}

        /// <summary>
        /// Transforms a Matrix by applying a Quaternion rotation.
        /// </summary>
        /// <param name="value">The Matrix to transform.</param>
        /// <param name="rotation">The rotation to apply, expressed as a Quaternion.</param>
        public static Matrix Transform( Matrix value, Quaternion rotation )
        {
            float xx = rotation.X + rotation.X;
            float yy = rotation.Y + rotation.Y;
            float zz = rotation.Z + rotation.Z;

            float num1 = rotation.W * xx;
            float num2 = rotation.W * yy;
            float num3 = rotation.W * zz;
            float num4 = rotation.X * xx;
            float num5 = rotation.X * yy;
            float num6 = rotation.X * zz;
            float num7 = rotation.Y * yy;
            float num8 = rotation.Y * zz;
            float num9 = rotation.Z * zz;

            float result1 = ( 1f - num7 ) - num9;
            float result2 = num5 - num3;
            float result3 = num6 + num2;
            float result4 = num5 + num3;
            float result5 = ( 1f - num4 ) - num9;
            float result6 = num8 - num1;
            float result7 = num6 - num2;
            float result8 = num8 + num1;
            float result9 = ( 1f - num4 ) - num7;

            return new Matrix
            {
                M11 = ( ( value.M11 * result1 ) + ( value.M12 * result2 ) ) + ( value.M13 * result3 ),
                M12 = ( ( value.M11 * result4 ) + ( value.M12 * result5 ) ) + ( value.M13 * result6 ),
                M13 = ( ( value.M11 * result7 ) + ( value.M12 * result8 ) ) + ( value.M13 * result9 ),
                M14 = value.M14,
                M21 = ( ( value.M21 * result1 ) + ( value.M22 * result2 ) ) + ( value.M23 * result3 ),
                M22 = ( ( value.M21 * result4 ) + ( value.M22 * result5 ) ) + ( value.M23 * result6 ),
                M23 = ( ( value.M21 * result7 ) + ( value.M22 * result8 ) ) + ( value.M23 * result9 ),
                M24 = value.M24,
                M31 = ( ( value.M31 * result1 ) + ( value.M32 * result2 ) ) + ( value.M33 * result3 ),
                M32 = ( ( value.M31 * result4 ) + ( value.M32 * result5 ) ) + ( value.M33 * result6 ),
                M33 = ( ( value.M31 * result7 ) + ( value.M32 * result8 ) ) + ( value.M33 * result9 ),
                M34 = value.M34,
                M41 = ( ( value.M41 * result1 ) + ( value.M42 * result2 ) ) + ( value.M43 * result3 ),
                M42 = ( ( value.M41 * result4 ) + ( value.M42 * result5 ) ) + ( value.M43 * result6 ),
                M43 = ( ( value.M41 * result7 ) + ( value.M42 * result8 ) ) + ( value.M43 * result9 ),
                M44 = value.M44
            };
        }

        /// <summary>
        /// Transforms a Matrix by applying a Quaternion rotation.
        /// </summary>
        /// <param name="value">The Matrix to transform.</param>
        /// <param name="rotation">The rotation to apply, expressed as a Quaternion.</param>
        /// <param name="result">[OutAttribute] An existing Matrix filled in with the result of the transform.</param>
        public static void Transform( ref Matrix value, ref Quaternion rotation, out Matrix result )
        {
            float xx = rotation.X + rotation.X;
            float yy = rotation.Y + rotation.Y;
            float zz = rotation.Z + rotation.Z;

            float num1 = rotation.W * xx;
            float num2 = rotation.W * yy;
            float num3 = rotation.W * zz;
            float num4 = rotation.X * xx;
            float num5 = rotation.X * yy;
            float num6 = rotation.X * zz;
            float num7 = rotation.Y * yy;
            float num8 = rotation.Y * zz;
            float num9 = rotation.Z * zz;

            float result1 = ( 1f - num7 ) - num9;
            float result2 = num5 - num3;
            float result3 = num6 + num2;
            float result4 = num5 + num3;
            float result5 = ( 1f - num4 ) - num9;
            float result6 = num8 - num1;
            float result7 = num6 - num2;
            float result8 = num8 + num1;
            float result9 = ( 1f - num4 ) - num7;

            result.M11 = ( ( value.M11 * result1 ) + ( value.M12 * result2 ) ) + ( value.M13 * result3 );
            result.M12 = ( ( value.M11 * result4 ) + ( value.M12 * result5 ) ) + ( value.M13 * result6 );
            result.M13 = ( ( value.M11 * result7 ) + ( value.M12 * result8 ) ) + ( value.M13 * result9 );
            result.M14 = value.M14;

            result.M21 = ( ( value.M21 * result1 ) + ( value.M22 * result2 ) ) + ( value.M23 * result3 );
            result.M22 = ( ( value.M21 * result4 ) + ( value.M22 * result5 ) ) + ( value.M23 * result6 );
            result.M23 = ( ( value.M21 * result7 ) + ( value.M22 * result8 ) ) + ( value.M23 * result9 );
            result.M24 = value.M24;

            result.M31 = ( ( value.M31 * result1 ) + ( value.M32 * result2 ) ) + ( value.M33 * result3 );
            result.M32 = ( ( value.M31 * result4 ) + ( value.M32 * result5 ) ) + ( value.M33 * result6 );
            result.M33 = ( ( value.M31 * result7 ) + ( value.M32 * result8 ) ) + ( value.M33 * result9 );
            result.M34 = value.M34;

            result.M41 = ( ( value.M41 * result1 ) + ( value.M42 * result2 ) ) + ( value.M43 * result3 );
            result.M42 = ( ( value.M41 * result4 ) + ( value.M42 * result5 ) ) + ( value.M43 * result6 );
            result.M43 = ( ( value.M41 * result7 ) + ( value.M42 * result8 ) ) + ( value.M43 * result9 );
            result.M44 = value.M44;
        }

        /// <summary>
        /// Transposes the rows and columns of a matrix.
        /// </summary>
        /// <param name="matrix">Source matrix.</param>
        public static Matrix Transpose( Matrix matrix )
        {
            return new Matrix
            {
                M11 = matrix.M11,
                M12 = matrix.M21,
                M13 = matrix.M31,
                M14 = matrix.M41,
                M21 = matrix.M12,
                M22 = matrix.M22,
                M23 = matrix.M32,
                M24 = matrix.M42,
                M31 = matrix.M13,
                M32 = matrix.M23,
                M33 = matrix.M33,
                M34 = matrix.M43,
                M41 = matrix.M14,
                M42 = matrix.M24,
                M43 = matrix.M34,
                M44 = matrix.M44
            };
        }

        /// <summary>
        /// Transposes the rows and columns of a matrix.
        /// </summary>
        /// <param name="matrix">Source matrix.</param>
        /// <param name="result">[OutAttribute] Transposed matrix.</param>
        public static void Transpose( ref Matrix matrix, out Matrix result )
        {
            result.M11 = matrix.M11;
            result.M12 = matrix.M21;
            result.M13 = matrix.M31;
            result.M14 = matrix.M41;

            result.M21 = matrix.M12;
            result.M22 = matrix.M22;
            result.M23 = matrix.M32;
            result.M24 = matrix.M42;

            result.M31 = matrix.M13;
            result.M32 = matrix.M23;
            result.M33 = matrix.M33;
            result.M34 = matrix.M43;

            result.M41 = matrix.M14;
            result.M42 = matrix.M24;
            result.M43 = matrix.M34;
            result.M44 = matrix.M44;
        }

        /// <summary>
        /// Calculates the determinant of the matrix.
        /// </summary>
        public float Determinant()
        {
            float num1 = ( M33 * M44 ) - ( M34 * M43 );
            float num2 = ( M32 * M44 ) - ( M34 * M42 );
            float num3 = ( M32 * M43 ) - ( M33 * M42 );
            float num4 = ( M31 * M44 ) - ( M34 * M41 );
            float num5 = ( M31 * M43 ) - ( M33 * M41 );
            float num6 = ( M31 * M42 ) - ( M32 * M41 );

            return ( ( ( ( M11 * ( ( ( M22 * num1 ) - ( M23 * num2 ) ) + ( M24 * num3 ) ) ) - ( M12 * ( ( ( M21 * num1 ) - ( M23 * num4 ) ) + ( M24 * num5 ) ) ) ) + ( M13 * ( ( ( M21 * num2 ) - ( M22 * num4 ) ) + ( M24 * num6 ) ) ) ) - ( M14 * ( ( ( M21 * num3 ) - ( M22 * num5 ) ) + ( M23 * num6 ) ) ) );
        }

        /// <summary>
        /// Calculates the inverse of a matrix.
        /// </summary>
        /// <param name="matrix">Source matrix.</param>
        public static Matrix Invert( Matrix matrix )
        {
            float result1 = ( matrix.M33 * matrix.M44 ) - ( matrix.M34 * matrix.M43 );
            float result2 = ( matrix.M32 * matrix.M44 ) - ( matrix.M34 * matrix.M42 );
            float result3 = ( matrix.M32 * matrix.M43 ) - ( matrix.M33 * matrix.M42 );
            float result4 = ( matrix.M31 * matrix.M44 ) - ( matrix.M34 * matrix.M41 );
            float result5 = ( matrix.M31 * matrix.M43 ) - ( matrix.M33 * matrix.M41 );
            float result6 = ( matrix.M31 * matrix.M42 ) - ( matrix.M32 * matrix.M41 );

            float result7 = ( ( matrix.M22 * result1 ) - ( matrix.M23 * result2 ) ) + ( matrix.M24 * result3 );
            float result8 = -( ( ( matrix.M21 * result1 ) - ( matrix.M23 * result4 ) ) + ( matrix.M24 * result5 ) );
            float result9 = ( ( matrix.M21 * result2 ) - ( matrix.M22 * result4 ) ) + ( matrix.M24 * result6 );
            float result10 = -( ( ( matrix.M21 * result3 ) - ( matrix.M22 * result5 ) ) + ( matrix.M23 * result6 ) );

            float num = 1f / ( ( ( ( matrix.M11 * result7 ) + ( matrix.M12 * result8 ) ) + ( matrix.M13 * result9 ) ) + ( matrix.M14 * result10 ) );

            float result11 = ( matrix.M23 * matrix.M44 ) - ( matrix.M24 * matrix.M43 );
            float result12 = ( matrix.M22 * matrix.M44 ) - ( matrix.M24 * matrix.M42 );
            float result13 = ( matrix.M22 * matrix.M43 ) - ( matrix.M23 * matrix.M42 );
            float result14 = ( matrix.M21 * matrix.M44 ) - ( matrix.M24 * matrix.M41 );
            float result15 = ( matrix.M21 * matrix.M43 ) - ( matrix.M23 * matrix.M41 );
            float result16 = ( matrix.M21 * matrix.M42 ) - ( matrix.M22 * matrix.M41 );

            float result17 = ( matrix.M23 * matrix.M34 ) - ( matrix.M24 * matrix.M33 );
            float result18 = ( matrix.M22 * matrix.M34 ) - ( matrix.M24 * matrix.M32 );
            float result19 = ( matrix.M22 * matrix.M33 ) - ( matrix.M23 * matrix.M32 );
            float result20 = ( matrix.M21 * matrix.M34 ) - ( matrix.M24 * matrix.M31 );
            float result21 = ( matrix.M21 * matrix.M33 ) - ( matrix.M23 * matrix.M31 );
            float result22 = ( matrix.M21 * matrix.M32 ) - ( matrix.M22 * matrix.M31 );

            return new Matrix
            {
                M11 = result7 * num,
                M21 = result8 * num,
                M31 = result9 * num,
                M41 = result10 * num,
                M12 = -( ( ( matrix.M12 * result1 ) - ( matrix.M13 * result2 ) ) + ( matrix.M14 * result3 ) ) * num,
                M22 = ( ( ( matrix.M11 * result1 ) - ( matrix.M13 * result4 ) ) + ( matrix.M14 * result5 ) ) * num,
                M32 = -( ( ( matrix.M11 * result2 ) - ( matrix.M12 * result4 ) ) + ( matrix.M14 * result6 ) ) * num,
                M42 = ( ( ( matrix.M11 * result3 ) - ( matrix.M12 * result5 ) ) + ( matrix.M13 * result6 ) ) * num,
                M13 = ( ( ( matrix.M12 * result11 ) - ( matrix.M13 * result12 ) ) + ( matrix.M14 * result13 ) ) * num,
                M23 = -( ( ( matrix.M11 * result11 ) - ( matrix.M13 * result14 ) ) + ( matrix.M14 * result15 ) ) * num,
                M33 = ( ( ( matrix.M11 * result12 ) - ( matrix.M12 * result14 ) ) + ( matrix.M14 * result16 ) ) * num,
                M43 = -( ( ( matrix.M11 * result13 ) - ( matrix.M12 * result15 ) ) + ( matrix.M13 * result16 ) ) * num,
                M14 = -( ( ( matrix.M12 * result17 ) - ( matrix.M13 * result18 ) ) + ( matrix.M14 * result19 ) ) * num,
                M24 = ( ( ( matrix.M11 * result17 ) - ( matrix.M13 * result20 ) ) + ( matrix.M14 * result21 ) ) * num,
                M34 = -( ( ( matrix.M11 * result18 ) - ( matrix.M12 * result20 ) ) + ( matrix.M14 * result22 ) ) * num,
                M44 = ( ( ( matrix.M11 * result19 ) - ( matrix.M12 * result21 ) ) + ( matrix.M13 * result22 ) ) * num
            };
        }

        /// <summary>
        /// Calculates the inverse of a matrix.
        /// </summary>
        /// <param name="matrix">The source matrix.</param>
        /// <param name="result">[OutAttribute] The inverse of matrix. The same matrix can be used for both arguments.</param>
        public static void Invert( ref Matrix matrix, out Matrix result )
        {
            float result1 = ( matrix.M33 * matrix.M44 ) - ( matrix.M34 * matrix.M43 );
            float result2 = ( matrix.M32 * matrix.M44 ) - ( matrix.M34 * matrix.M42 );
            float result3 = ( matrix.M32 * matrix.M43 ) - ( matrix.M33 * matrix.M42 );
            float result4 = ( matrix.M31 * matrix.M44 ) - ( matrix.M34 * matrix.M41 );
            float result5 = ( matrix.M31 * matrix.M43 ) - ( matrix.M33 * matrix.M41 );
            float result6 = ( matrix.M31 * matrix.M42 ) - ( matrix.M32 * matrix.M41 );

            float result7 = ( ( matrix.M22 * result1 ) - ( matrix.M23 * result2 ) ) + ( matrix.M24 * result3 );
            float result8 = -( ( ( matrix.M21 * result1 ) - ( matrix.M23 * result4 ) ) + ( matrix.M24 * result5 ) );
            float result9 = ( ( matrix.M21 * result2 ) - ( matrix.M22 * result4 ) ) + ( matrix.M24 * result6 );
            float result10 = -( ( ( matrix.M21 * result3 ) - ( matrix.M22 * result5 ) ) + ( matrix.M23 * result6 ) );

            float num = 1f / ( ( ( ( matrix.M11 * result7 ) + ( matrix.M12 * result8 ) ) + ( matrix.M13 * result9 ) ) + ( matrix.M14 * result10 ) );

            float result11 = ( matrix.M23 * matrix.M44 ) - ( matrix.M24 * matrix.M43 );
            float result12 = ( matrix.M22 * matrix.M44 ) - ( matrix.M24 * matrix.M42 );
            float result13 = ( matrix.M22 * matrix.M43 ) - ( matrix.M23 * matrix.M42 );
            float result14 = ( matrix.M21 * matrix.M44 ) - ( matrix.M24 * matrix.M41 );
            float result15 = ( matrix.M21 * matrix.M43 ) - ( matrix.M23 * matrix.M41 );
            float result16 = ( matrix.M21 * matrix.M42 ) - ( matrix.M22 * matrix.M41 );

            float result17 = ( matrix.M23 * matrix.M34 ) - ( matrix.M24 * matrix.M33 );
            float result18 = ( matrix.M22 * matrix.M34 ) - ( matrix.M24 * matrix.M32 );
            float result19 = ( matrix.M22 * matrix.M33 ) - ( matrix.M23 * matrix.M32 );
            float result20 = ( matrix.M21 * matrix.M34 ) - ( matrix.M24 * matrix.M31 );
            float result21 = ( matrix.M21 * matrix.M33 ) - ( matrix.M23 * matrix.M31 );
            float result22 = ( matrix.M21 * matrix.M32 ) - ( matrix.M22 * matrix.M31 );

            result.M11 = result7 * num;
            result.M21 = result8 * num;
            result.M31 = result9 * num;
            result.M41 = result10 * num;

            result.M12 = -( ( ( matrix.M12 * result1 ) - ( matrix.M13 * result2 ) ) + ( matrix.M14 * result3 ) ) * num;
            result.M22 = ( ( ( matrix.M11 * result1 ) - ( matrix.M13 * result4 ) ) + ( matrix.M14 * result5 ) ) * num;
            result.M32 = -( ( ( matrix.M11 * result2 ) - ( matrix.M12 * result4 ) ) + ( matrix.M14 * result6 ) ) * num;
            result.M42 = ( ( ( matrix.M11 * result3 ) - ( matrix.M12 * result5 ) ) + ( matrix.M13 * result6 ) ) * num;

            result.M13 = ( ( ( matrix.M12 * result11 ) - ( matrix.M13 * result12 ) ) + ( matrix.M14 * result13 ) ) * num;
            result.M23 = -( ( ( matrix.M11 * result11 ) - ( matrix.M13 * result14 ) ) + ( matrix.M14 * result15 ) ) * num;
            result.M33 = ( ( ( matrix.M11 * result12 ) - ( matrix.M12 * result14 ) ) + ( matrix.M14 * result16 ) ) * num;
            result.M43 = -( ( ( matrix.M11 * result13 ) - ( matrix.M12 * result15 ) ) + ( matrix.M13 * result16 ) ) * num;

            result.M14 = -( ( ( matrix.M12 * result17 ) - ( matrix.M13 * result18 ) ) + ( matrix.M14 * result19 ) ) * num;
            result.M24 = ( ( ( matrix.M11 * result17 ) - ( matrix.M13 * result20 ) ) + ( matrix.M14 * result21 ) ) * num;
            result.M34 = -( ( ( matrix.M11 * result18 ) - ( matrix.M12 * result20 ) ) + ( matrix.M14 * result22 ) ) * num;
            result.M44 = ( ( ( matrix.M11 * result19 ) - ( matrix.M12 * result21 ) ) + ( matrix.M13 * result22 ) ) * num;
        }

        /// <summary>
        /// Linearly interpolates between the corresponding values of two matrices.
        /// </summary>
        /// <param name="matrix1">Source matrix.</param>
        /// <param name="matrix2">Source matrix.</param>
        /// <param name="amount">Interpolation value.</param>
        public static Matrix Lerp( Matrix matrix1, Matrix matrix2, float amount )
        {
            return new Matrix
            {
                M11 = matrix1.M11 + ( ( matrix2.M11 - matrix1.M11 ) * amount ),
                M12 = matrix1.M12 + ( ( matrix2.M12 - matrix1.M12 ) * amount ),
                M13 = matrix1.M13 + ( ( matrix2.M13 - matrix1.M13 ) * amount ),
                M14 = matrix1.M14 + ( ( matrix2.M14 - matrix1.M14 ) * amount ),
                M21 = matrix1.M21 + ( ( matrix2.M21 - matrix1.M21 ) * amount ),
                M22 = matrix1.M22 + ( ( matrix2.M22 - matrix1.M22 ) * amount ),
                M23 = matrix1.M23 + ( ( matrix2.M23 - matrix1.M23 ) * amount ),
                M24 = matrix1.M24 + ( ( matrix2.M24 - matrix1.M24 ) * amount ),
                M31 = matrix1.M31 + ( ( matrix2.M31 - matrix1.M31 ) * amount ),
                M32 = matrix1.M32 + ( ( matrix2.M32 - matrix1.M32 ) * amount ),
                M33 = matrix1.M33 + ( ( matrix2.M33 - matrix1.M33 ) * amount ),
                M34 = matrix1.M34 + ( ( matrix2.M34 - matrix1.M34 ) * amount ),
                M41 = matrix1.M41 + ( ( matrix2.M41 - matrix1.M41 ) * amount ),
                M42 = matrix1.M42 + ( ( matrix2.M42 - matrix1.M42 ) * amount ),
                M43 = matrix1.M43 + ( ( matrix2.M43 - matrix1.M43 ) * amount ),
                M44 = matrix1.M44 + ( ( matrix2.M44 - matrix1.M44 ) * amount )
            };
        }

        /// <summary>
        /// Linearly interpolates between the corresponding values of two matrices.
        /// </summary>
        /// <param name="matrix1">Source matrix.</param>
        /// <param name="matrix2">Source matrix.</param>
        /// <param name="amount">Interpolation value.</param>
        /// <param name="result">[OutAttribute] Resulting matrix.</param>
        public static void Lerp( ref Matrix matrix1, ref Matrix matrix2, float amount, out Matrix result )
        {
            result.M11 = matrix1.M11 + ( ( matrix2.M11 - matrix1.M11 ) * amount );
            result.M12 = matrix1.M12 + ( ( matrix2.M12 - matrix1.M12 ) * amount );
            result.M13 = matrix1.M13 + ( ( matrix2.M13 - matrix1.M13 ) * amount );
            result.M14 = matrix1.M14 + ( ( matrix2.M14 - matrix1.M14 ) * amount );

            result.M21 = matrix1.M21 + ( ( matrix2.M21 - matrix1.M21 ) * amount );
            result.M22 = matrix1.M22 + ( ( matrix2.M22 - matrix1.M22 ) * amount );
            result.M23 = matrix1.M23 + ( ( matrix2.M23 - matrix1.M23 ) * amount );
            result.M24 = matrix1.M24 + ( ( matrix2.M24 - matrix1.M24 ) * amount );

            result.M31 = matrix1.M31 + ( ( matrix2.M31 - matrix1.M31 ) * amount );
            result.M32 = matrix1.M32 + ( ( matrix2.M32 - matrix1.M32 ) * amount );
            result.M33 = matrix1.M33 + ( ( matrix2.M33 - matrix1.M33 ) * amount );
            result.M34 = matrix1.M34 + ( ( matrix2.M34 - matrix1.M34 ) * amount );

            result.M41 = matrix1.M41 + ( ( matrix2.M41 - matrix1.M41 ) * amount );
            result.M42 = matrix1.M42 + ( ( matrix2.M42 - matrix1.M42 ) * amount );
            result.M43 = matrix1.M43 + ( ( matrix2.M43 - matrix1.M43 ) * amount );
            result.M44 = matrix1.M44 + ( ( matrix2.M44 - matrix1.M44 ) * amount );
        }

        /// <summary>
        /// Negates individual elements of a matrix.
        /// </summary>
        /// <param name="matrix">Source matrix.</param>
        public static Matrix Negate( Matrix matrix )
        {
            return new Matrix
            {
                M11 = -matrix.M11,
                M12 = -matrix.M12,
                M13 = -matrix.M13,
                M14 = -matrix.M14,
                M21 = -matrix.M21,
                M22 = -matrix.M22,
                M23 = -matrix.M23,
                M24 = -matrix.M24,
                M31 = -matrix.M31,
                M32 = -matrix.M32,
                M33 = -matrix.M33,
                M34 = -matrix.M34,
                M41 = -matrix.M41,
                M42 = -matrix.M42,
                M43 = -matrix.M43,
                M44 = -matrix.M44
            };
        }

        /// <summary>
        /// Negates individual elements of a matrix.
        /// </summary>
        /// <param name="matrix">Source matrix.</param>
        /// <param name="result">[OutAttribute] Negated matrix.</param>
        public static void Negate( ref Matrix matrix, out Matrix result )
        {
            result.M11 = -matrix.M11;
            result.M12 = -matrix.M12;
            result.M13 = -matrix.M13;
            result.M14 = -matrix.M14;

            result.M21 = -matrix.M21;
            result.M22 = -matrix.M22;
            result.M23 = -matrix.M23;
            result.M24 = -matrix.M24;

            result.M31 = -matrix.M31;
            result.M32 = -matrix.M32;
            result.M33 = -matrix.M33;
            result.M34 = -matrix.M34;

            result.M41 = -matrix.M41;
            result.M42 = -matrix.M42;
            result.M43 = -matrix.M43;
            result.M44 = -matrix.M44;
        }

        /// <summary>
        /// Adds a matrix to another matrix.
        /// </summary>
        /// <param name="matrix1">Source matrix.</param>
        /// <param name="matrix2">Source matrix.</param>
        public static Matrix Add( Matrix matrix1, Matrix matrix2 )
        {
            return new Matrix
            {
                M11 = matrix1.M11 + matrix2.M11,
                M12 = matrix1.M12 + matrix2.M12,
                M13 = matrix1.M13 + matrix2.M13,
                M14 = matrix1.M14 + matrix2.M14,
                M21 = matrix1.M21 + matrix2.M21,
                M22 = matrix1.M22 + matrix2.M22,
                M23 = matrix1.M23 + matrix2.M23,
                M24 = matrix1.M24 + matrix2.M24,
                M31 = matrix1.M31 + matrix2.M31,
                M32 = matrix1.M32 + matrix2.M32,
                M33 = matrix1.M33 + matrix2.M33,
                M34 = matrix1.M34 + matrix2.M34,
                M41 = matrix1.M41 + matrix2.M41,
                M42 = matrix1.M42 + matrix2.M42,
                M43 = matrix1.M43 + matrix2.M43,
                M44 = matrix1.M44 + matrix2.M44
            };
        }

        /// <summary>
        /// Adds a matrix to another matrix.
        /// </summary>
        /// <param name="matrix1">Source matrix.</param>
        /// <param name="matrix2">Source matrix.</param>
        /// <param name="result">[OutAttribute] Resulting matrix.</param>
        public static void Add( ref Matrix matrix1, ref Matrix matrix2, out Matrix result )
        {
            result.M11 = matrix1.M11 + matrix2.M11;
            result.M12 = matrix1.M12 + matrix2.M12;
            result.M13 = matrix1.M13 + matrix2.M13;
            result.M14 = matrix1.M14 + matrix2.M14;

            result.M21 = matrix1.M21 + matrix2.M21;
            result.M22 = matrix1.M22 + matrix2.M22;
            result.M23 = matrix1.M23 + matrix2.M23;
            result.M24 = matrix1.M24 + matrix2.M24;

            result.M31 = matrix1.M31 + matrix2.M31;
            result.M32 = matrix1.M32 + matrix2.M32;
            result.M33 = matrix1.M33 + matrix2.M33;
            result.M34 = matrix1.M34 + matrix2.M34;

            result.M41 = matrix1.M41 + matrix2.M41;
            result.M42 = matrix1.M42 + matrix2.M42;
            result.M43 = matrix1.M43 + matrix2.M43;
            result.M44 = matrix1.M44 + matrix2.M44;
        }

        /// <summary>
        /// Subtracts matrices.
        /// </summary>
        /// <param name="matrix1">Source matrix.</param>
        /// <param name="matrix2">Source matrix.</param>
        public static Matrix Subtract( Matrix matrix1, Matrix matrix2 )
        {
            return new Matrix
            {
                M11 = matrix1.M11 - matrix2.M11,
                M12 = matrix1.M12 - matrix2.M12,
                M13 = matrix1.M13 - matrix2.M13,
                M14 = matrix1.M14 - matrix2.M14,
                M21 = matrix1.M21 - matrix2.M21,
                M22 = matrix1.M22 - matrix2.M22,
                M23 = matrix1.M23 - matrix2.M23,
                M24 = matrix1.M24 - matrix2.M24,
                M31 = matrix1.M31 - matrix2.M31,
                M32 = matrix1.M32 - matrix2.M32,
                M33 = matrix1.M33 - matrix2.M33,
                M34 = matrix1.M34 - matrix2.M34,
                M41 = matrix1.M41 - matrix2.M41,
                M42 = matrix1.M42 - matrix2.M42,
                M43 = matrix1.M43 - matrix2.M43,
                M44 = matrix1.M44 - matrix2.M44
            };
        }

        /// <summary>
        /// Subtracts matrices.
        /// </summary>
        /// <param name="matrix1">Source matrix.</param>
        /// <param name="matrix2">Source matrix.</param>
        /// <param name="result">[OutAttribute] Result of the subtraction.</param>
        public static void Subtract( ref Matrix matrix1, ref Matrix matrix2, out Matrix result )
        {
            result.M11 = matrix1.M11 - matrix2.M11;
            result.M12 = matrix1.M12 - matrix2.M12;
            result.M13 = matrix1.M13 - matrix2.M13;
            result.M14 = matrix1.M14 - matrix2.M14;

            result.M21 = matrix1.M21 - matrix2.M21;
            result.M22 = matrix1.M22 - matrix2.M22;
            result.M23 = matrix1.M23 - matrix2.M23;
            result.M24 = matrix1.M24 - matrix2.M24;

            result.M31 = matrix1.M31 - matrix2.M31;
            result.M32 = matrix1.M32 - matrix2.M32;
            result.M33 = matrix1.M33 - matrix2.M33;
            result.M34 = matrix1.M34 - matrix2.M34;

            result.M41 = matrix1.M41 - matrix2.M41;
            result.M42 = matrix1.M42 - matrix2.M42;
            result.M43 = matrix1.M43 - matrix2.M43;
            result.M44 = matrix1.M44 - matrix2.M44;
        }

        /// <summary>
        /// Multiplies a matrix by another matrix.
        /// </summary>
        /// <param name="matrix1">Source matrix.</param>
        /// <param name="matrix2">Source matrix.</param>
        public static Matrix Multiply( Matrix matrix1, Matrix matrix2 )
        {
            return new Matrix
            {
                M11 = ( ( ( matrix1.M11 * matrix2.M11 ) + ( matrix1.M12 * matrix2.M21 ) ) + ( matrix1.M13 * matrix2.M31 ) ) + ( matrix1.M14 * matrix2.M41 ),
                M12 = ( ( ( matrix1.M11 * matrix2.M12 ) + ( matrix1.M12 * matrix2.M22 ) ) + ( matrix1.M13 * matrix2.M32 ) ) + ( matrix1.M14 * matrix2.M42 ),
                M13 = ( ( ( matrix1.M11 * matrix2.M13 ) + ( matrix1.M12 * matrix2.M23 ) ) + ( matrix1.M13 * matrix2.M33 ) ) + ( matrix1.M14 * matrix2.M43 ),
                M14 = ( ( ( matrix1.M11 * matrix2.M14 ) + ( matrix1.M12 * matrix2.M24 ) ) + ( matrix1.M13 * matrix2.M34 ) ) + ( matrix1.M14 * matrix2.M44 ),
                M21 = ( ( ( matrix1.M21 * matrix2.M11 ) + ( matrix1.M22 * matrix2.M21 ) ) + ( matrix1.M23 * matrix2.M31 ) ) + ( matrix1.M24 * matrix2.M41 ),
                M22 = ( ( ( matrix1.M21 * matrix2.M12 ) + ( matrix1.M22 * matrix2.M22 ) ) + ( matrix1.M23 * matrix2.M32 ) ) + ( matrix1.M24 * matrix2.M42 ),
                M23 = ( ( ( matrix1.M21 * matrix2.M13 ) + ( matrix1.M22 * matrix2.M23 ) ) + ( matrix1.M23 * matrix2.M33 ) ) + ( matrix1.M24 * matrix2.M43 ),
                M24 = ( ( ( matrix1.M21 * matrix2.M14 ) + ( matrix1.M22 * matrix2.M24 ) ) + ( matrix1.M23 * matrix2.M34 ) ) + ( matrix1.M24 * matrix2.M44 ),
                M31 = ( ( ( matrix1.M31 * matrix2.M11 ) + ( matrix1.M32 * matrix2.M21 ) ) + ( matrix1.M33 * matrix2.M31 ) ) + ( matrix1.M34 * matrix2.M41 ),
                M32 = ( ( ( matrix1.M31 * matrix2.M12 ) + ( matrix1.M32 * matrix2.M22 ) ) + ( matrix1.M33 * matrix2.M32 ) ) + ( matrix1.M34 * matrix2.M42 ),
                M33 = ( ( ( matrix1.M31 * matrix2.M13 ) + ( matrix1.M32 * matrix2.M23 ) ) + ( matrix1.M33 * matrix2.M33 ) ) + ( matrix1.M34 * matrix2.M43 ),
                M34 = ( ( ( matrix1.M31 * matrix2.M14 ) + ( matrix1.M32 * matrix2.M24 ) ) + ( matrix1.M33 * matrix2.M34 ) ) + ( matrix1.M34 * matrix2.M44 ),
                M41 = ( ( ( matrix1.M41 * matrix2.M11 ) + ( matrix1.M42 * matrix2.M21 ) ) + ( matrix1.M43 * matrix2.M31 ) ) + ( matrix1.M44 * matrix2.M41 ),
                M42 = ( ( ( matrix1.M41 * matrix2.M12 ) + ( matrix1.M42 * matrix2.M22 ) ) + ( matrix1.M43 * matrix2.M32 ) ) + ( matrix1.M44 * matrix2.M42 ),
                M43 = ( ( ( matrix1.M41 * matrix2.M13 ) + ( matrix1.M42 * matrix2.M23 ) ) + ( matrix1.M43 * matrix2.M33 ) ) + ( matrix1.M44 * matrix2.M43 ),
                M44 = ( ( ( matrix1.M41 * matrix2.M14 ) + ( matrix1.M42 * matrix2.M24 ) ) + ( matrix1.M43 * matrix2.M34 ) ) + ( matrix1.M44 * matrix2.M44 )
            };
        }

        /// <summary>
        /// Multiplies a matrix by another matrix.
        /// </summary>
        /// <param name="matrix1">Source matrix.</param>
        /// <param name="matrix2">Source matrix.</param>
        /// <param name="result">[OutAttribute] Result of the multiplication.</param>
        public static void Multiply( ref Matrix matrix1, ref Matrix matrix2, out Matrix result )
        {
            result.M11 = ( ( ( matrix1.M11 * matrix2.M11 ) + ( matrix1.M12 * matrix2.M21 ) ) + ( matrix1.M13 * matrix2.M31 ) ) + ( matrix1.M14 * matrix2.M41 );
            result.M12 = ( ( ( matrix1.M11 * matrix2.M12 ) + ( matrix1.M12 * matrix2.M22 ) ) + ( matrix1.M13 * matrix2.M32 ) ) + ( matrix1.M14 * matrix2.M42 );
            result.M13 = ( ( ( matrix1.M11 * matrix2.M13 ) + ( matrix1.M12 * matrix2.M23 ) ) + ( matrix1.M13 * matrix2.M33 ) ) + ( matrix1.M14 * matrix2.M43 );
            result.M14 = ( ( ( matrix1.M11 * matrix2.M14 ) + ( matrix1.M12 * matrix2.M24 ) ) + ( matrix1.M13 * matrix2.M34 ) ) + ( matrix1.M14 * matrix2.M44 );

            result.M21 = ( ( ( matrix1.M21 * matrix2.M11 ) + ( matrix1.M22 * matrix2.M21 ) ) + ( matrix1.M23 * matrix2.M31 ) ) + ( matrix1.M24 * matrix2.M41 );
            result.M22 = ( ( ( matrix1.M21 * matrix2.M12 ) + ( matrix1.M22 * matrix2.M22 ) ) + ( matrix1.M23 * matrix2.M32 ) ) + ( matrix1.M24 * matrix2.M42 );
            result.M23 = ( ( ( matrix1.M21 * matrix2.M13 ) + ( matrix1.M22 * matrix2.M23 ) ) + ( matrix1.M23 * matrix2.M33 ) ) + ( matrix1.M24 * matrix2.M43 );
            result.M24 = ( ( ( matrix1.M21 * matrix2.M14 ) + ( matrix1.M22 * matrix2.M24 ) ) + ( matrix1.M23 * matrix2.M34 ) ) + ( matrix1.M24 * matrix2.M44 );

            result.M31 = ( ( ( matrix1.M31 * matrix2.M11 ) + ( matrix1.M32 * matrix2.M21 ) ) + ( matrix1.M33 * matrix2.M31 ) ) + ( matrix1.M34 * matrix2.M41 );
            result.M32 = ( ( ( matrix1.M31 * matrix2.M12 ) + ( matrix1.M32 * matrix2.M22 ) ) + ( matrix1.M33 * matrix2.M32 ) ) + ( matrix1.M34 * matrix2.M42 );
            result.M33 = ( ( ( matrix1.M31 * matrix2.M13 ) + ( matrix1.M32 * matrix2.M23 ) ) + ( matrix1.M33 * matrix2.M33 ) ) + ( matrix1.M34 * matrix2.M43 );
            result.M34 = ( ( ( matrix1.M31 * matrix2.M14 ) + ( matrix1.M32 * matrix2.M24 ) ) + ( matrix1.M33 * matrix2.M34 ) ) + ( matrix1.M34 * matrix2.M44 );

            result.M41 = ( ( ( matrix1.M41 * matrix2.M11 ) + ( matrix1.M42 * matrix2.M21 ) ) + ( matrix1.M43 * matrix2.M31 ) ) + ( matrix1.M44 * matrix2.M41 );
            result.M42 = ( ( ( matrix1.M41 * matrix2.M12 ) + ( matrix1.M42 * matrix2.M22 ) ) + ( matrix1.M43 * matrix2.M32 ) ) + ( matrix1.M44 * matrix2.M42 );
            result.M43 = ( ( ( matrix1.M41 * matrix2.M13 ) + ( matrix1.M42 * matrix2.M23 ) ) + ( matrix1.M43 * matrix2.M33 ) ) + ( matrix1.M44 * matrix2.M43 );
            result.M44 = ( ( ( matrix1.M41 * matrix2.M14 ) + ( matrix1.M42 * matrix2.M24 ) ) + ( matrix1.M43 * matrix2.M34 ) ) + ( matrix1.M44 * matrix2.M44 );
        }

        /// <summary>
        /// Multiplies a matrix by a scalar value.
        /// </summary>
        /// <param name="matrix1">Source matrix.</param>
        /// <param name="scaleFactor">Scalar value.</param>
        public static Matrix Multiply( Matrix matrix1, float scaleFactor )
        {
            return new Matrix
            {
                M11 = matrix1.M11 * scaleFactor,
                M12 = matrix1.M12 * scaleFactor,
                M13 = matrix1.M13 * scaleFactor,
                M14 = matrix1.M14 * scaleFactor,
                M21 = matrix1.M21 * scaleFactor,
                M22 = matrix1.M22 * scaleFactor,
                M23 = matrix1.M23 * scaleFactor,
                M24 = matrix1.M24 * scaleFactor,
                M31 = matrix1.M31 * scaleFactor,
                M32 = matrix1.M32 * scaleFactor,
                M33 = matrix1.M33 * scaleFactor,
                M34 = matrix1.M34 * scaleFactor,
                M41 = matrix1.M41 * scaleFactor,
                M42 = matrix1.M42 * scaleFactor,
                M43 = matrix1.M43 * scaleFactor,
                M44 = matrix1.M44 * scaleFactor
            };
        }

        /// <summary>
        /// Multiplies a matrix by a scalar value.
        /// </summary>
        /// <param name="matrix1">Source matrix.</param>
        /// <param name="scaleFactor">Scalar value.</param>
        /// <param name="result">[OutAttribute] The result of the multiplication.</param>
        public static void Multiply( ref Matrix matrix1, float scaleFactor, out Matrix result )
        {
            result.M11 = matrix1.M11 * scaleFactor;
            result.M12 = matrix1.M12 * scaleFactor;
            result.M13 = matrix1.M13 * scaleFactor;
            result.M14 = matrix1.M14 * scaleFactor;

            result.M21 = matrix1.M21 * scaleFactor;
            result.M22 = matrix1.M22 * scaleFactor;
            result.M23 = matrix1.M23 * scaleFactor;
            result.M24 = matrix1.M24 * scaleFactor;

            result.M31 = matrix1.M31 * scaleFactor;
            result.M32 = matrix1.M32 * scaleFactor;
            result.M33 = matrix1.M33 * scaleFactor;
            result.M34 = matrix1.M34 * scaleFactor;

            result.M41 = matrix1.M41 * scaleFactor;
            result.M42 = matrix1.M42 * scaleFactor;
            result.M43 = matrix1.M43 * scaleFactor;
            result.M44 = matrix1.M44 * scaleFactor;
        }

        /// <summary>
        /// Divides the components of a matrix by the corresponding components of another matrix.
        /// </summary>
        /// <param name="matrix1">Source matrix.</param>
        /// <param name="matrix2">The divisor.</param>
        public static Matrix Divide( Matrix matrix1, Matrix matrix2 )
        {
            return new Matrix
            {
                M11 = matrix1.M11 / matrix2.M11,
                M12 = matrix1.M12 / matrix2.M12,
                M13 = matrix1.M13 / matrix2.M13,
                M14 = matrix1.M14 / matrix2.M14,
                M21 = matrix1.M21 / matrix2.M21,
                M22 = matrix1.M22 / matrix2.M22,
                M23 = matrix1.M23 / matrix2.M23,
                M24 = matrix1.M24 / matrix2.M24,
                M31 = matrix1.M31 / matrix2.M31,
                M32 = matrix1.M32 / matrix2.M32,
                M33 = matrix1.M33 / matrix2.M33,
                M34 = matrix1.M34 / matrix2.M34,
                M41 = matrix1.M41 / matrix2.M41,
                M42 = matrix1.M42 / matrix2.M42,
                M43 = matrix1.M43 / matrix2.M43,
                M44 = matrix1.M44 / matrix2.M44
            };
        }

        /// <summary>
        /// Divides the components of a matrix by the corresponding components of another matrix.
        /// </summary>
        /// <param name="matrix1">Source matrix.</param>
        /// <param name="matrix2">The divisor.</param>
        /// <param name="result">[OutAttribute] Result of the division.</param>
        public static void Divide( ref Matrix matrix1, ref Matrix matrix2, out Matrix result )
        {
            result.M11 = matrix1.M11 / matrix2.M11;
            result.M12 = matrix1.M12 / matrix2.M12;
            result.M13 = matrix1.M13 / matrix2.M13;
            result.M14 = matrix1.M14 / matrix2.M14;

            result.M21 = matrix1.M21 / matrix2.M21;
            result.M22 = matrix1.M22 / matrix2.M22;
            result.M23 = matrix1.M23 / matrix2.M23;
            result.M24 = matrix1.M24 / matrix2.M24;

            result.M31 = matrix1.M31 / matrix2.M31;
            result.M32 = matrix1.M32 / matrix2.M32;
            result.M33 = matrix1.M33 / matrix2.M33;
            result.M34 = matrix1.M34 / matrix2.M34;

            result.M41 = matrix1.M41 / matrix2.M41;
            result.M42 = matrix1.M42 / matrix2.M42;
            result.M43 = matrix1.M43 / matrix2.M43;
            result.M44 = matrix1.M44 / matrix2.M44;
        }

        /// <summary>
        /// Divides the components of a matrix by a scalar.
        /// </summary>
        /// <param name="matrix1">Source matrix.</param>
        /// <param name="divider">The divisor.</param>
        public static Matrix Divide( Matrix matrix1, float divider )
        {
            float num = 1f / divider;

            return new Matrix
            {
                M11 = matrix1.M11 * num,
                M12 = matrix1.M12 * num,
                M13 = matrix1.M13 * num,
                M14 = matrix1.M14 * num,
                M21 = matrix1.M21 * num,
                M22 = matrix1.M22 * num,
                M23 = matrix1.M23 * num,
                M24 = matrix1.M24 * num,
                M31 = matrix1.M31 * num,
                M32 = matrix1.M32 * num,
                M33 = matrix1.M33 * num,
                M34 = matrix1.M34 * num,
                M41 = matrix1.M41 * num,
                M42 = matrix1.M42 * num,
                M43 = matrix1.M43 * num,
                M44 = matrix1.M44 * num
            };
        }

        /// <summary>
        /// Divides the components of a matrix by a scalar.
        /// </summary>
        /// <param name="matrix1">Source matrix.</param>
        /// <param name="divider">The divisor.</param>
        /// <param name="result">[OutAttribute] Result of the division.</param>
        public static void Divide( ref Matrix matrix1, float divider, out Matrix result )
        {
            float num = 1f / divider;

            result.M11 = matrix1.M11 * num;
            result.M12 = matrix1.M12 * num;
            result.M13 = matrix1.M13 * num;
            result.M14 = matrix1.M14 * num;

            result.M21 = matrix1.M21 * num;
            result.M22 = matrix1.M22 * num;
            result.M23 = matrix1.M23 * num;
            result.M24 = matrix1.M24 * num;

            result.M31 = matrix1.M31 * num;
            result.M32 = matrix1.M32 * num;
            result.M33 = matrix1.M33 * num;
            result.M34 = matrix1.M34 * num;

            result.M41 = matrix1.M41 * num;
            result.M42 = matrix1.M42 * num;
            result.M43 = matrix1.M43 * num;
            result.M44 = matrix1.M44 * num;
        }

        #endregion

        #region zh-CHS 共有静态方法 | en Public Static Methods

        /// <summary>
        /// Negates individual elements of a matrix.
        /// </summary>
        /// <param name="matrix1">Source matrix.</param>
        public static Matrix operator -( Matrix matrix1 )
        {
            return new Matrix
            {
                M11 = -matrix1.M11,
                M12 = -matrix1.M12,
                M13 = -matrix1.M13,
                M14 = -matrix1.M14,
                M21 = -matrix1.M21,
                M22 = -matrix1.M22,
                M23 = -matrix1.M23,
                M24 = -matrix1.M24,
                M31 = -matrix1.M31,
                M32 = -matrix1.M32,
                M33 = -matrix1.M33,
                M34 = -matrix1.M34,
                M41 = -matrix1.M41,
                M42 = -matrix1.M42,
                M43 = -matrix1.M43,
                M44 = -matrix1.M44
            };
        }

        /// <summary>
        /// Compares a matrix for equality with another matrix.
        /// </summary>
        /// <param name="matrix1">Source matrix.</param>
        /// <param name="matrix2">Source matrix.</param>
        public static bool operator ==( Matrix matrix1, Matrix matrix2 )
        {
            return ( ( ( ( (
                ( matrix1.M11 == matrix2.M11 )
                && ( matrix1.M22 == matrix2.M22 ) )
                && ( ( matrix1.M33 == matrix2.M33 )
                && ( matrix1.M44 == matrix2.M44 ) ) )
                && ( ( ( matrix1.M12 == matrix2.M12 )
                && ( matrix1.M13 == matrix2.M13 ) )
                && ( ( matrix1.M14 == matrix2.M14 )
                && ( matrix1.M21 == matrix2.M21 ) ) ) )
                && ( ( ( ( matrix1.M23 == matrix2.M23 )
                && ( matrix1.M24 == matrix2.M24 ) )
                && ( ( matrix1.M31 == matrix2.M31 )
                && ( matrix1.M32 == matrix2.M32 ) ) )
                && ( ( ( matrix1.M34 == matrix2.M34 )
                && ( matrix1.M41 == matrix2.M41 ) )
                && ( matrix1.M42 == matrix2.M42 ) ) ) )
                && ( matrix1.M43 == matrix2.M43 ) );
        }

        /// <summary>
        /// Tests a matrix for inequality with another matrix.
        /// </summary>
        /// <param name="matrix1">The matrix on the left of the equal sign.</param>
        /// <param name="matrix2">The matrix on the right of the equal sign.</param>
        public static bool operator !=( Matrix matrix1, Matrix matrix2 )
        {
            if ( ( ( (
                ( matrix1.M11 == matrix2.M11 )
                && ( matrix1.M12 == matrix2.M12 ) )
                && ( ( matrix1.M13 == matrix2.M13 )
                && ( matrix1.M14 == matrix2.M14 ) ) )
                && ( ( ( matrix1.M21 == matrix2.M21 )
                && ( matrix1.M22 == matrix2.M22 ) )
                && ( ( matrix1.M23 == matrix2.M23 )
                && ( matrix1.M24 == matrix2.M24 ) ) ) )
                && ( ( ( ( matrix1.M31 == matrix2.M31 )
                && ( matrix1.M32 == matrix2.M32 ) )
                && ( ( matrix1.M33 == matrix2.M33 )
                && ( matrix1.M34 == matrix2.M34 ) ) )
                && ( ( ( matrix1.M41 == matrix2.M41 )
                && ( matrix1.M42 == matrix2.M42 ) )
                && ( matrix1.M43 == matrix2.M43 ) ) ) )
                return !( matrix1.M44 == matrix2.M44 );

            return true;
        }

        /// <summary>
        /// Adds a matrix to another matrix.
        /// </summary>
        /// <param name="matrix1">Source matrix.</param>
        /// <param name="matrix2">Source matrix.</param>
        public static Matrix operator +( Matrix matrix1, Matrix matrix2 )
        {
            return new Matrix
            {
                M11 = matrix1.M11 + matrix2.M11,
                M12 = matrix1.M12 + matrix2.M12,
                M13 = matrix1.M13 + matrix2.M13,
                M14 = matrix1.M14 + matrix2.M14,
                M21 = matrix1.M21 + matrix2.M21,
                M22 = matrix1.M22 + matrix2.M22,
                M23 = matrix1.M23 + matrix2.M23,
                M24 = matrix1.M24 + matrix2.M24,
                M31 = matrix1.M31 + matrix2.M31,
                M32 = matrix1.M32 + matrix2.M32,
                M33 = matrix1.M33 + matrix2.M33,
                M34 = matrix1.M34 + matrix2.M34,
                M41 = matrix1.M41 + matrix2.M41,
                M42 = matrix1.M42 + matrix2.M42,
                M43 = matrix1.M43 + matrix2.M43,
                M44 = matrix1.M44 + matrix2.M44
            };
        }

        /// <summary>
        /// Subtracts matrices.
        /// </summary>
        /// <param name="matrix1">Source matrix.</param>
        /// <param name="matrix2">Source matrix.</param>
        public static Matrix operator -( Matrix matrix1, Matrix matrix2 )
        {
            return new Matrix
            {
                M11 = matrix1.M11 - matrix2.M11,
                M12 = matrix1.M12 - matrix2.M12,
                M13 = matrix1.M13 - matrix2.M13,
                M14 = matrix1.M14 - matrix2.M14,
                M21 = matrix1.M21 - matrix2.M21,
                M22 = matrix1.M22 - matrix2.M22,
                M23 = matrix1.M23 - matrix2.M23,
                M24 = matrix1.M24 - matrix2.M24,
                M31 = matrix1.M31 - matrix2.M31,
                M32 = matrix1.M32 - matrix2.M32,
                M33 = matrix1.M33 - matrix2.M33,
                M34 = matrix1.M34 - matrix2.M34,
                M41 = matrix1.M41 - matrix2.M41,
                M42 = matrix1.M42 - matrix2.M42,
                M43 = matrix1.M43 - matrix2.M43,
                M44 = matrix1.M44 - matrix2.M44
            };
        }

        /// <summary>
        /// Multiplies a matrix by another matrix.
        /// </summary>
        /// <param name="matrix1">Source matrix.</param>
        /// <param name="matrix2">Source matrix.</param>
        public static Matrix operator *( Matrix matrix1, Matrix matrix2 )
        {
            return new Matrix
            {
                M11 = ( ( ( matrix1.M11 * matrix2.M11 ) + ( matrix1.M12 * matrix2.M21 ) ) + ( matrix1.M13 * matrix2.M31 ) ) + ( matrix1.M14 * matrix2.M41 ),
                M12 = ( ( ( matrix1.M11 * matrix2.M12 ) + ( matrix1.M12 * matrix2.M22 ) ) + ( matrix1.M13 * matrix2.M32 ) ) + ( matrix1.M14 * matrix2.M42 ),
                M13 = ( ( ( matrix1.M11 * matrix2.M13 ) + ( matrix1.M12 * matrix2.M23 ) ) + ( matrix1.M13 * matrix2.M33 ) ) + ( matrix1.M14 * matrix2.M43 ),
                M14 = ( ( ( matrix1.M11 * matrix2.M14 ) + ( matrix1.M12 * matrix2.M24 ) ) + ( matrix1.M13 * matrix2.M34 ) ) + ( matrix1.M14 * matrix2.M44 ),
                M21 = ( ( ( matrix1.M21 * matrix2.M11 ) + ( matrix1.M22 * matrix2.M21 ) ) + ( matrix1.M23 * matrix2.M31 ) ) + ( matrix1.M24 * matrix2.M41 ),
                M22 = ( ( ( matrix1.M21 * matrix2.M12 ) + ( matrix1.M22 * matrix2.M22 ) ) + ( matrix1.M23 * matrix2.M32 ) ) + ( matrix1.M24 * matrix2.M42 ),
                M23 = ( ( ( matrix1.M21 * matrix2.M13 ) + ( matrix1.M22 * matrix2.M23 ) ) + ( matrix1.M23 * matrix2.M33 ) ) + ( matrix1.M24 * matrix2.M43 ),
                M24 = ( ( ( matrix1.M21 * matrix2.M14 ) + ( matrix1.M22 * matrix2.M24 ) ) + ( matrix1.M23 * matrix2.M34 ) ) + ( matrix1.M24 * matrix2.M44 ),
                M31 = ( ( ( matrix1.M31 * matrix2.M11 ) + ( matrix1.M32 * matrix2.M21 ) ) + ( matrix1.M33 * matrix2.M31 ) ) + ( matrix1.M34 * matrix2.M41 ),
                M32 = ( ( ( matrix1.M31 * matrix2.M12 ) + ( matrix1.M32 * matrix2.M22 ) ) + ( matrix1.M33 * matrix2.M32 ) ) + ( matrix1.M34 * matrix2.M42 ),
                M33 = ( ( ( matrix1.M31 * matrix2.M13 ) + ( matrix1.M32 * matrix2.M23 ) ) + ( matrix1.M33 * matrix2.M33 ) ) + ( matrix1.M34 * matrix2.M43 ),
                M34 = ( ( ( matrix1.M31 * matrix2.M14 ) + ( matrix1.M32 * matrix2.M24 ) ) + ( matrix1.M33 * matrix2.M34 ) ) + ( matrix1.M34 * matrix2.M44 ),
                M41 = ( ( ( matrix1.M41 * matrix2.M11 ) + ( matrix1.M42 * matrix2.M21 ) ) + ( matrix1.M43 * matrix2.M31 ) ) + ( matrix1.M44 * matrix2.M41 ),
                M42 = ( ( ( matrix1.M41 * matrix2.M12 ) + ( matrix1.M42 * matrix2.M22 ) ) + ( matrix1.M43 * matrix2.M32 ) ) + ( matrix1.M44 * matrix2.M42 ),
                M43 = ( ( ( matrix1.M41 * matrix2.M13 ) + ( matrix1.M42 * matrix2.M23 ) ) + ( matrix1.M43 * matrix2.M33 ) ) + ( matrix1.M44 * matrix2.M43 ),
                M44 = ( ( ( matrix1.M41 * matrix2.M14 ) + ( matrix1.M42 * matrix2.M24 ) ) + ( matrix1.M43 * matrix2.M34 ) ) + ( matrix1.M44 * matrix2.M44 )
            };
        }

        /// <summary>
        /// Multiplies a matrix by a scalar value.
        /// </summary>
        /// <param name="matrix">Source matrix.</param>
        /// <param name="scaleFactor">Scalar value.</param>
        public static Matrix operator *( Matrix matrix, float scaleFactor )
        {
            float num = scaleFactor;

            return new Matrix
            {
                M11 = matrix.M11 * num,
                M12 = matrix.M12 * num,
                M13 = matrix.M13 * num,
                M14 = matrix.M14 * num,
                M21 = matrix.M21 * num,
                M22 = matrix.M22 * num,
                M23 = matrix.M23 * num,
                M24 = matrix.M24 * num,
                M31 = matrix.M31 * num,
                M32 = matrix.M32 * num,
                M33 = matrix.M33 * num,
                M34 = matrix.M34 * num,
                M41 = matrix.M41 * num,
                M42 = matrix.M42 * num,
                M43 = matrix.M43 * num,
                M44 = matrix.M44 * num
            };
        }

        /// <summary>
        /// Multiplies a matrix by a scalar value.
        /// </summary>
        /// <param name="scaleFactor">Scalar value.</param>
        /// <param name="matrix">Source matrix.</param>
        public static Matrix operator *( float scaleFactor, Matrix matrix )
        {
            float num = scaleFactor;

            return new Matrix
            {
                M11 = matrix.M11 * num,
                M12 = matrix.M12 * num,
                M13 = matrix.M13 * num,
                M14 = matrix.M14 * num,
                M21 = matrix.M21 * num,
                M22 = matrix.M22 * num,
                M23 = matrix.M23 * num,
                M24 = matrix.M24 * num,
                M31 = matrix.M31 * num,
                M32 = matrix.M32 * num,
                M33 = matrix.M33 * num,
                M34 = matrix.M34 * num,
                M41 = matrix.M41 * num,
                M42 = matrix.M42 * num,
                M43 = matrix.M43 * num,
                M44 = matrix.M44 * num
            };
        }

        /// <summary>
        /// Divides the components of a matrix by the corresponding components of another matrix.
        /// </summary>
        /// <param name="matrix1">Source matrix.</param>
        /// <param name="matrix2">The divisor.</param>
        public static Matrix operator /( Matrix matrix1, Matrix matrix2 )
        {
            return new Matrix
            {
                M11 = matrix1.M11 / matrix2.M11,
                M12 = matrix1.M12 / matrix2.M12,
                M13 = matrix1.M13 / matrix2.M13,
                M14 = matrix1.M14 / matrix2.M14,
                M21 = matrix1.M21 / matrix2.M21,
                M22 = matrix1.M22 / matrix2.M22,
                M23 = matrix1.M23 / matrix2.M23,
                M24 = matrix1.M24 / matrix2.M24,
                M31 = matrix1.M31 / matrix2.M31,
                M32 = matrix1.M32 / matrix2.M32,
                M33 = matrix1.M33 / matrix2.M33,
                M34 = matrix1.M34 / matrix2.M34,
                M41 = matrix1.M41 / matrix2.M41,
                M42 = matrix1.M42 / matrix2.M42,
                M43 = matrix1.M43 / matrix2.M43,
                M44 = matrix1.M44 / matrix2.M44
            };
        }

        /// <summary>
        /// Divides the components of a matrix by a scalar.
        /// </summary>
        /// <param name="matrix1">Source matrix.</param>
        /// <param name="divider">The divisor.</param>
        public static Matrix operator /( Matrix matrix1, float divider )
        {
            float num = 1f / divider;

            return new Matrix
            {
                M11 = matrix1.M11 * num,
                M12 = matrix1.M12 * num,
                M13 = matrix1.M13 * num,
                M14 = matrix1.M14 * num,
                M21 = matrix1.M21 * num,
                M22 = matrix1.M22 * num,
                M23 = matrix1.M23 * num,
                M24 = matrix1.M24 * num,
                M31 = matrix1.M31 * num,
                M32 = matrix1.M32 * num,
                M33 = matrix1.M33 * num,
                M34 = matrix1.M34 * num,
                M41 = matrix1.M41 * num,
                M42 = matrix1.M42 * num,
                M43 = matrix1.M43 * num,
                M44 = matrix1.M44 * num
            };
        }

        #endregion

        #region zh-CHS 方法覆盖 | en Override Methods
        /// <summary>
        /// Retrieves a string representation of the current object.
        /// </summary>
        public override string ToString()
        {
            return ( String.Format( "{{ {0}{1}{2}{3}}}", string.Format( CultureInfo.CurrentCulture, "{{M11:{0} M12:{1} M13:{2} M14:{3}}} ", new object[] { M11.ToString( CultureInfo.CurrentCulture ), M12.ToString( CultureInfo.CurrentCulture ), M13.ToString( CultureInfo.CurrentCulture ), M14.ToString( CultureInfo.CurrentCulture ) } ), string.Format( CultureInfo.CurrentCulture, "{{M21:{0} M22:{1} M23:{2} M24:{3}}} ", new object[] { M21.ToString( CultureInfo.CurrentCulture ), M22.ToString( CultureInfo.CurrentCulture ), M23.ToString( CultureInfo.CurrentCulture ), M24.ToString( CultureInfo.CurrentCulture ) } ), string.Format( CultureInfo.CurrentCulture, "{{M31:{0} M32:{1} M33:{2} M34:{3}}} ", new object[] { M31.ToString( CultureInfo.CurrentCulture ), M32.ToString( CultureInfo.CurrentCulture ), M33.ToString( CultureInfo.CurrentCulture ), M34.ToString( CultureInfo.CurrentCulture ) } ), string.Format( CultureInfo.CurrentCulture, "{{M41:{0} M42:{1} M43:{2} M44:{3}}} ", new object[] { M41.ToString( CultureInfo.CurrentCulture ), M42.ToString( CultureInfo.CurrentCulture ), M43.ToString( CultureInfo.CurrentCulture ), M44.ToString( CultureInfo.CurrentCulture ) } ) ) );
        }


        /// <summary>
        /// Returns a value that indicates whether the current instance is equal to a specified object.
        /// </summary>
        /// <param name="obj">Object with which to make the comparison.</param>
        public override bool Equals( object obj )
        {
            if ( obj is Matrix )
                return Equals( (Matrix)obj );
            else
                return false;
        }

        /// <summary>
        /// Gets the hash code of this object.
        /// </summary>
        public override int GetHashCode()
        {
            return ( ( ( ( ( ( ( ( ( ( ( ( ( ( ( M11.GetHashCode() + M12.GetHashCode() ) + M13.GetHashCode() ) + M14.GetHashCode() ) + M21.GetHashCode() ) + M22.GetHashCode() ) + M23.GetHashCode() ) + M24.GetHashCode() ) + M31.GetHashCode() ) + M32.GetHashCode() ) + M33.GetHashCode() ) + M34.GetHashCode() ) + M41.GetHashCode() ) + M42.GetHashCode() ) + M43.GetHashCode() ) + M44.GetHashCode() );
        }
        #endregion

        #region zh-CHS 接口实现 | en Interface Implementation
        /// <summary>
        /// Determines whether the specified Object is equal to the Matrix.
        /// </summary>
        /// <param name="other">The Object to compare with the current Matrix.</param>
        public bool Equals( Matrix other )
        {
            return ( ( ( ( ( ( M11 == other.M11 ) && ( M22 == other.M22 ) ) && ( ( M33 == other.M33 ) && ( M44 == other.M44 ) ) ) && ( ( ( M12 == other.M12 ) && ( M13 == other.M13 ) ) && ( ( M14 == other.M14 ) && ( M21 == other.M21 ) ) ) ) && ( ( ( ( M23 == other.M23 ) && ( M24 == other.M24 ) ) && ( ( M31 == other.M31 ) && ( M32 == other.M32 ) ) ) && ( ( ( M34 == other.M34 ) && ( M41 == other.M41 ) ) && ( M42 == other.M42 ) ) ) ) && ( M43 == other.M43 ) );
        }
        #endregion

        [StructLayout( LayoutKind.Sequential )]
        private struct CanonicalBasis
        {
            public Vector3 Row0;
            public Vector3 Row1;
            public Vector3 Row2;
        }

        [StructLayout( LayoutKind.Sequential )]
        private struct VectorBasis
        {
            public Vector3 Element0;
            public Vector3 Element1;
            public Vector3 Element2;
        }
    }
}
#endregion
