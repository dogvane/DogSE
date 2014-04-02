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
    /// <summary>Defines a plane.</summary>
    public struct Plane : IEquatable<Plane>
    {
        /// <summary>The normal vector of the Plane.</summary>
        public Vector3 Normal;
        /// <summary>The distance of the Plane along its normal from the origin.</summary>
        public float D;
        /// <summary>Creates a new instance of Plane.</summary>
        /// <param name="a">X component of the normal defining the Plane.</param>
        /// <param name="b">Y component of the normal defining the Plane.</param>
        /// <param name="c">Z component of the normal defining the Plane.</param>
        /// <param name="d">Distance of the Plane along its normal from the origin.</param>
        public Plane( float a, float b, float c, float d )
        {
            Normal.X = a;
            Normal.Y = b;
            Normal.Z = c;
            D = d;
        }

        /// <summary>Creates a new instance of Plane.</summary>
        /// <param name="normal">The normal vector to the Plane.</param>
        /// <param name="d">The Plane's distance along its normal from the origin.</param>
        public Plane( Vector3 normal, float d )
        {
            Normal = normal;
            D = d;
        }

        /// <summary>Creates a new instance of Plane.</summary>
        /// <param name="value">Vector4 with X, Y, and Z components defining the normal of the Plane. The W component defines the distance of the Plane along the normal from the origin.</param>
        public Plane( Vector4 value )
        {
            Normal.X = value.X;
            Normal.Y = value.Y;
            Normal.Z = value.Z;
            D = value.W;
        }

        /// <summary>Creates a new instance of Plane.</summary>
        /// <param name="point1">One point of a triangle defining the Plane.</param>
        /// <param name="point2">One point of a triangle defining the Plane.</param>
        /// <param name="point3">One point of a triangle defining the Plane.</param>
        public Plane( Vector3 point1, Vector3 point2, Vector3 point3 )
        {
            float num10 = point2.X - point1.X;
            float num9 = point2.Y - point1.Y;
            float num8 = point2.Z - point1.Z;
            float num7 = point3.X - point1.X;
            float num6 = point3.Y - point1.Y;
            float num5 = point3.Z - point1.Z;
            float num4 = ( num9 * num5 ) - ( num8 * num6 );
            float num3 = ( num8 * num7 ) - ( num10 * num5 );
            float num2 = ( num10 * num6 ) - ( num9 * num7 );
            float num11 = ( ( num4 * num4 ) + ( num3 * num3 ) ) + ( num2 * num2 );
            float num = 1f / ( (float)Math.Sqrt( (double)num11 ) );
            Normal.X = num4 * num;
            Normal.Y = num3 * num;
            Normal.Z = num2 * num;
            D = -( ( ( Normal.X * point1.X ) + ( Normal.Y * point1.Y ) ) + ( Normal.Z * point1.Z ) );
        }

        /// <summary>Determines whether the specified Plane is equal to the Plane.</summary>
        /// <param name="other">The Plane to compare with the current Plane.</param>
        public bool Equals( Plane other )
        {
            return ( ( ( ( Normal.X == other.Normal.X ) && ( Normal.Y == other.Normal.Y ) ) && ( Normal.Z == other.Normal.Z ) ) && ( D == other.D ) );
        }

        /// <summary>Determines whether the specified Object is equal to the Plane.</summary>
        /// <param name="obj">The Object to compare with the current Plane.</param>
        public override bool Equals( object obj )
        {
            if ( obj is Plane )
                return Equals( (Plane)obj );
            else
                return false;
        }

        /// <summary>Gets the hash code for this object.</summary>
        public override int GetHashCode()
        {
            return ( Normal.GetHashCode() + D.GetHashCode() );
        }

        /// <summary>Returns a String that represents the current Plane.</summary>
        public override string ToString()
        {
            return string.Format( CultureInfo.CurrentCulture, "{{Normal:{0} D:{1}}}", new object[] { Normal.ToString(), D.ToString( CultureInfo.CurrentCulture ) } );
        }

        /// <summary>Changes the coefficients of the Normal vector of this Plane to make it of unit length.</summary>
        public void Normalize()
        {
            float num2 = ( ( Normal.X * Normal.X ) + ( Normal.Y * Normal.Y ) ) + ( Normal.Z * Normal.Z );
            if ( Math.Abs( (float)( num2 - 1f ) ) >= 1.192093E-07f )
            {
                float num = 1f / ( (float)Math.Sqrt( (double)num2 ) );
                Normal.X *= num;
                Normal.Y *= num;
                Normal.Z *= num;
                D *= num;
            }
        }

        /// <summary>Changes the coefficients of the Normal vector of a Plane to make it of unit length.</summary>
        /// <param name="value">The Plane to normalize.</param>
        public static Plane Normalize( Plane value )
        {
            Plane plane;
            float num2 = ( ( value.Normal.X * value.Normal.X ) + ( value.Normal.Y * value.Normal.Y ) ) + ( value.Normal.Z * value.Normal.Z );
            if ( Math.Abs( (float)( num2 - 1f ) ) < 1.192093E-07f )
            {
                plane.Normal = value.Normal;
                plane.D = value.D;
                return plane;
            }
            float num = 1f / ( (float)Math.Sqrt( (double)num2 ) );
            plane.Normal.X = value.Normal.X * num;
            plane.Normal.Y = value.Normal.Y * num;
            plane.Normal.Z = value.Normal.Z * num;
            plane.D = value.D * num;
            return plane;
        }

        /// <summary>Changes the coefficients of the Normal vector of a Plane to make it of unit length.</summary>
        /// <param name="value">The Plane to normalize.</param>
        /// <param name="result">[OutAttribute] An existing plane Plane filled in with a normalized version of the specified plane.</param>
        public static void Normalize( ref Plane value, out Plane result )
        {
            float num2 = ( ( value.Normal.X * value.Normal.X ) + ( value.Normal.Y * value.Normal.Y ) ) + ( value.Normal.Z * value.Normal.Z );
            if ( Math.Abs( (float)( num2 - 1f ) ) < 1.192093E-07f )
            {
                result.Normal = value.Normal;
                result.D = value.D;
            }
            else
            {
                float num = 1f / ( (float)Math.Sqrt( (double)num2 ) );
                result.Normal.X = value.Normal.X * num;
                result.Normal.Y = value.Normal.Y * num;
                result.Normal.Z = value.Normal.Z * num;
                result.D = value.D * num;
            }
        }

        /// <summary>Transforms a normalized Plane by a Matrix.</summary>
        /// <param name="plane">The normalized Plane to transform. This Plane must already be normalized, so that its Normal vector is of unit length, before this method is called.</param>
        /// <param name="matrix">The transform Matrix to apply to the Plane.</param>
        public static Plane Transform( Plane plane, Matrix matrix )
        {
            Plane plane2;
            Matrix matrix2;
            Matrix.Invert( ref matrix, out matrix2 );
            float x = plane.Normal.X;
            float y = plane.Normal.Y;
            float z = plane.Normal.Z;
            float d = plane.D;
            plane2.Normal.X = ( ( ( x * matrix2.M11 ) + ( y * matrix2.M12 ) ) + ( z * matrix2.M13 ) ) + ( d * matrix2.M14 );
            plane2.Normal.Y = ( ( ( x * matrix2.M21 ) + ( y * matrix2.M22 ) ) + ( z * matrix2.M23 ) ) + ( d * matrix2.M24 );
            plane2.Normal.Z = ( ( ( x * matrix2.M31 ) + ( y * matrix2.M32 ) ) + ( z * matrix2.M33 ) ) + ( d * matrix2.M34 );
            plane2.D = ( ( ( x * matrix2.M41 ) + ( y * matrix2.M42 ) ) + ( z * matrix2.M43 ) ) + ( d * matrix2.M44 );
            return plane2;
        }

        /// <summary>Transforms a normalized Plane by a Matrix.</summary>
        /// <param name="plane">The normalized Plane to transform. This Plane must already be normalized, so that its Normal vector is of unit length, before this method is called.</param>
        /// <param name="matrix">The transform Matrix to apply to the Plane.</param>
        /// <param name="result">[OutAttribute] An existing Plane filled in with the results of applying the transform.</param>
        public static void Transform( ref Plane plane, ref Matrix matrix, out Plane result )
        {
            Matrix matrix2;
            Matrix.Invert( ref matrix, out matrix2 );
            float x = plane.Normal.X;
            float y = plane.Normal.Y;
            float z = plane.Normal.Z;
            float d = plane.D;
            result.Normal.X = ( ( ( x * matrix2.M11 ) + ( y * matrix2.M12 ) ) + ( z * matrix2.M13 ) ) + ( d * matrix2.M14 );
            result.Normal.Y = ( ( ( x * matrix2.M21 ) + ( y * matrix2.M22 ) ) + ( z * matrix2.M23 ) ) + ( d * matrix2.M24 );
            result.Normal.Z = ( ( ( x * matrix2.M31 ) + ( y * matrix2.M32 ) ) + ( z * matrix2.M33 ) ) + ( d * matrix2.M34 );
            result.D = ( ( ( x * matrix2.M41 ) + ( y * matrix2.M42 ) ) + ( z * matrix2.M43 ) ) + ( d * matrix2.M44 );
        }

        /// <summary>Transforms a normalized Plane by a Quaternion rotation.</summary>
        /// <param name="plane">The normalized Plane to transform. This Plane must already be normalized, so that its Normal vector is of unit length, before this method is called.</param>
        /// <param name="rotation">The Quaternion rotation to apply to the Plane.</param>
        public static Plane Transform( Plane plane, Quaternion rotation )
        {
            Plane plane2;
            float num15 = rotation.X + rotation.X;
            float num5 = rotation.Y + rotation.Y;
            float num = rotation.Z + rotation.Z;
            float num14 = rotation.W * num15;
            float num13 = rotation.W * num5;
            float num12 = rotation.W * num;
            float num11 = rotation.X * num15;
            float num10 = rotation.X * num5;
            float num9 = rotation.X * num;
            float num8 = rotation.Y * num5;
            float num7 = rotation.Y * num;
            float num6 = rotation.Z * num;
            float num24 = ( 1f - num8 ) - num6;
            float num23 = num10 - num12;
            float num22 = num9 + num13;
            float num21 = num10 + num12;
            float num20 = ( 1f - num11 ) - num6;
            float num19 = num7 - num14;
            float num18 = num9 - num13;
            float num17 = num7 + num14;
            float num16 = ( 1f - num11 ) - num8;
            float x = plane.Normal.X;
            float y = plane.Normal.Y;
            float z = plane.Normal.Z;
            plane2.Normal.X = ( ( x * num24 ) + ( y * num23 ) ) + ( z * num22 );
            plane2.Normal.Y = ( ( x * num21 ) + ( y * num20 ) ) + ( z * num19 );
            plane2.Normal.Z = ( ( x * num18 ) + ( y * num17 ) ) + ( z * num16 );
            plane2.D = plane.D;
            return plane2;
        }

        /// <summary>Transforms a normalized Plane by a Quaternion rotation.</summary>
        /// <param name="plane">The normalized Plane to transform. This Plane must already be normalized, so that its Normal vector is of unit length, before this method is called.</param>
        /// <param name="rotation">The Quaternion rotation to apply to the Plane.</param>
        /// <param name="result">[OutAttribute] An existing Plane filled in with the results of applying the rotation.</param>
        public static void Transform( ref Plane plane, ref Quaternion rotation, out Plane result )
        {
            float num15 = rotation.X + rotation.X;
            float num5 = rotation.Y + rotation.Y;
            float num = rotation.Z + rotation.Z;
            float num14 = rotation.W * num15;
            float num13 = rotation.W * num5;
            float num12 = rotation.W * num;
            float num11 = rotation.X * num15;
            float num10 = rotation.X * num5;
            float num9 = rotation.X * num;
            float num8 = rotation.Y * num5;
            float num7 = rotation.Y * num;
            float num6 = rotation.Z * num;
            float num24 = ( 1f - num8 ) - num6;
            float num23 = num10 - num12;
            float num22 = num9 + num13;
            float num21 = num10 + num12;
            float num20 = ( 1f - num11 ) - num6;
            float num19 = num7 - num14;
            float num18 = num9 - num13;
            float num17 = num7 + num14;
            float num16 = ( 1f - num11 ) - num8;
            float x = plane.Normal.X;
            float y = plane.Normal.Y;
            float z = plane.Normal.Z;
            result.Normal.X = ( ( x * num24 ) + ( y * num23 ) ) + ( z * num22 );
            result.Normal.Y = ( ( x * num21 ) + ( y * num20 ) ) + ( z * num19 );
            result.Normal.Z = ( ( x * num18 ) + ( y * num17 ) ) + ( z * num16 );
            result.D = plane.D;
        }

        /// <summary>Calculates the dot product of a specified Vector4 and this Plane.</summary>
        /// <param name="value">The Vector4 to multiply this Plane by.</param>
        public float Dot( Vector4 value )
        {
            return ( ( ( ( Normal.X * value.X ) + ( Normal.Y * value.Y ) ) + ( Normal.Z * value.Z ) ) + ( D * value.W ) );
        }

        /// <summary>Calculates the dot product of a specified Vector4 and this Plane.</summary>
        /// <param name="value">The Vector4 to multiply this Plane by.</param>
        /// <param name="result">[OutAttribute] The dot product of the specified Vector4 and this Plane.</param>
        public void Dot( ref Vector4 value, out float result )
        {
            result = ( ( ( Normal.X * value.X ) + ( Normal.Y * value.Y ) ) + ( Normal.Z * value.Z ) ) + ( D * value.W );
        }

        /// <summary>Returns the dot product of a specified Vector3 and the Normal vector of this Plane plus the distance (D) value of the Plane.</summary>
        /// <param name="value">The Vector3 to multiply by.</param>
        public float DotCoordinate( Vector3 value )
        {
            return ( ( ( ( Normal.X * value.X ) + ( Normal.Y * value.Y ) ) + ( Normal.Z * value.Z ) ) + D );
        }

        /// <summary>Returns the dot product of a specified Vector3 and the Normal vector of this Plane plus the distance (D) value of the Plane.</summary>
        /// <param name="value">The Vector3 to multiply by.</param>
        /// <param name="result">[OutAttribute] The resulting value.</param>
        public void DotCoordinate( ref Vector3 value, out float result )
        {
            result = ( ( ( Normal.X * value.X ) + ( Normal.Y * value.Y ) ) + ( Normal.Z * value.Z ) ) + D;
        }

        /// <summary>Returns the dot product of a specified Vector3 and the Normal vector of this Plane.</summary>
        /// <param name="value">The Vector3 to multiply by.</param>
        public float DotNormal( Vector3 value )
        {
            return ( ( ( Normal.X * value.X ) + ( Normal.Y * value.Y ) ) + ( Normal.Z * value.Z ) );
        }

        /// <summary>Returns the dot product of a specified Vector3 and the Normal vector of this Plane.</summary>
        /// <param name="value">The Vector3 to multiply by.</param>
        /// <param name="result">[OutAttribute] The resulting dot product.</param>
        public void DotNormal( ref Vector3 value, out float result )
        {
            result = ( ( Normal.X * value.X ) + ( Normal.Y * value.Y ) ) + ( Normal.Z * value.Z );
        }

        ///// <summary>Checks whether the current Plane intersects a specified BoundingBox.</summary>
        ///// <param name="box">The BoundingBox to test for intersection with.</param>
        //public PlaneIntersectionType Intersects(BoundingBox box)
        //{
        //    Vector3 vector;
        //    Vector3 vector2;
        //    vector2.X = (Normal.X >= 0f) ? box.Min.X : box.Max.X;
        //    vector2.Y = (Normal.Y >= 0f) ? box.Min.Y : box.Max.Y;
        //    vector2.Z = (Normal.Z >= 0f) ? box.Min.Z : box.Max.Z;
        //    vector.X = (Normal.X >= 0f) ? box.Max.X : box.Min.X;
        //    vector.Y = (Normal.Y >= 0f) ? box.Max.Y : box.Min.Y;
        //    vector.Z = (Normal.Z >= 0f) ? box.Max.Z : box.Min.Z;
        //    float num = ((Normal.X * vector2.X) + (Normal.Y * vector2.Y)) + (Normal.Z * vector2.Z);
        //    if ((num + D) > 0f)
        //    {
        //        return PlaneIntersectionType.Front;
        //    }
        //    num = ((Normal.X * vector.X) + (Normal.Y * vector.Y)) + (Normal.Z * vector.Z);
        //    if ((num + D) < 0f)
        //    {
        //        return PlaneIntersectionType.Back;
        //    }
        //    return PlaneIntersectionType.Intersecting;
        //}

        ///// <summary>Checks whether the current Plane intersects a BoundingBox.</summary>
        ///// <param name="box">The BoundingBox to check for intersection with.</param>
        ///// <param name="result">[OutAttribute] An enumeration indicating whether the Plane intersects the BoundingBox.</param>
        //public void Intersects(ref BoundingBox box, out PlaneIntersectionType result)
        //{
        //    Vector3 vector;
        //    Vector3 vector2;
        //    vector2.X = (Normal.X >= 0f) ? box.Min.X : box.Max.X;
        //    vector2.Y = (Normal.Y >= 0f) ? box.Min.Y : box.Max.Y;
        //    vector2.Z = (Normal.Z >= 0f) ? box.Min.Z : box.Max.Z;
        //    vector.X = (Normal.X >= 0f) ? box.Max.X : box.Min.X;
        //    vector.Y = (Normal.Y >= 0f) ? box.Max.Y : box.Min.Y;
        //    vector.Z = (Normal.Z >= 0f) ? box.Max.Z : box.Min.Z;
        //    float num = ((Normal.X * vector2.X) + (Normal.Y * vector2.Y)) + (Normal.Z * vector2.Z);
        //    if ((num + D) > 0f)
        //    {
        //        result = PlaneIntersectionType.Front;
        //    }
        //    else
        //    {
        //        num = ((Normal.X * vector.X) + (Normal.Y * vector.Y)) + (Normal.Z * vector.Z);
        //        if ((num + D) < 0f)
        //        {
        //            result = PlaneIntersectionType.Back;
        //        }
        //        else
        //        {
        //            result = PlaneIntersectionType.Intersecting;
        //        }
        //    }
        //}

        ///// <summary>Checks whether the current Plane intersects a specified BoundingFrustum.</summary>
        ///// <param name="frustum">The BoundingFrustum to check for intersection with.</param>
        //public PlaneIntersectionType Intersects(BoundingFrustum frustum)
        //{
        //    if (null == frustum)
        //    {
        //        throw new ArgumentNullException("frustum", FrameworkResources.NullNotAllowed);
        //    }
        //    return frustum.Intersects(this);
        //}

        /// <summary>Checks whether the current Plane intersects a specified BoundingSphere.</summary>
        /// <param name="sphere">The BoundingSphere to check for intersection with.</param>
        public PlaneIntersectionType Intersects( BoundingSphere sphere )
        {
            float num2 = ( ( sphere.Center.X * Normal.X ) + ( sphere.Center.Y * Normal.Y ) ) + ( sphere.Center.Z * Normal.Z );
            float num = num2 + D;
            if ( num > sphere.Radius )
            {
                return PlaneIntersectionType.Front;
            }
            if ( num < -sphere.Radius )
            {
                return PlaneIntersectionType.Back;
            }
            return PlaneIntersectionType.Intersecting;
        }

        /// <summary>Checks whether the current Plane intersects a BoundingSphere.</summary>
        /// <param name="sphere">The BoundingSphere to check for intersection with.</param>
        /// <param name="result">[OutAttribute] An enumeration indicating whether the Plane intersects the BoundingSphere.</param>
        public void Intersects( ref BoundingSphere sphere, out PlaneIntersectionType result )
        {
            float num2 = ( ( sphere.Center.X * Normal.X ) + ( sphere.Center.Y * Normal.Y ) ) + ( sphere.Center.Z * Normal.Z );
            float num = num2 + D;
            if ( num > sphere.Radius )
            {
                result = PlaneIntersectionType.Front;
            }
            else if ( num < -sphere.Radius )
            {
                result = PlaneIntersectionType.Back;
            }
            else
            {
                result = PlaneIntersectionType.Intersecting;
            }
        }

        /// <summary>Determines whether two instances of Plane are equal.</summary>
        /// <param name="lhs">The object to the left of the equality operator.</param>
        /// <param name="rhs">The object to the right of the equality operator.</param>
        public static bool operator ==( Plane lhs, Plane rhs )
        {
            return lhs.Equals( rhs );
        }

        /// <summary>Determines whether two instances of Plane are not equal.</summary>
        /// <param name="lhs">The object to the left of the inequality operator.</param>
        /// <param name="rhs">The object to the right of the inequality operator.</param>
        public static bool operator !=( Plane lhs, Plane rhs )
        {
            if ( ( ( lhs.Normal.X == rhs.Normal.X ) && ( lhs.Normal.Y == rhs.Normal.Y ) ) && ( lhs.Normal.Z == rhs.Normal.Z ) )
            {
                return !( lhs.D == rhs.D );
            }
            return true;
        }
    }
}
#endregion