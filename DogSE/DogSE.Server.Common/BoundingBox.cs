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
using System.Collections.Generic;
using System.Globalization;
#endregion



namespace DogSE.Common
{

    /// <summary>
    /// Defines an axis-aligned box-shaped 3D volume.
    /// </summary>
    public struct BoundingBox : IEquatable<BoundingBox>
    {

        #region zh-CHS 共有常量 | en Public Constants

        /// <summary>
        /// Specifies the total number of corners (8) in the BoundingBox.
        /// </summary>
        public const int CornerCount = 8;

        #endregion

        #region zh-CHS 共有成员变量 | en Public Member Variables

        /// <summary>
        /// The minimum point the BoundingBox contains.
        /// </summary>
        public Vector3 Min;
        /// <summary>
        /// The maximum point the BoundingBox contains.
        /// </summary>
        public Vector3 Max;

        #endregion

        #region zh-CHS 构造和初始化和清理 | en Constructors and Initializers and Dispose

        /// <summary>
        /// Creates an instance of BoundingBox.
        /// </summary>
        /// <param name="min">The minimum point the BoundingBox includes.</param>
        /// <param name="max">The maximum point the BoundingBox includes.</param>
        public BoundingBox( Vector3 min, Vector3 max )
        {
            Min = min;
            Max = max;
        }

        #endregion

        #region zh-CHS 共有静态方法 | en Public Static Methods

        /// <summary>
        /// Creates the smallest BoundingBox that contains the two specified BoundingBox instances.
        /// </summary>
        /// <param name="original">One of the BoundingBoxs to contain.</param>
        /// <param name="additional">One of the BoundingBoxs to contain.</param>
        public static BoundingBox CreateMerged( BoundingBox original, BoundingBox additional )
        {
            BoundingBox box;

            Vector3.Min( ref original.Min, ref additional.Min, out box.Min );
            Vector3.Max( ref original.Max, ref additional.Max, out box.Max );

            return box;
        }

        /// <summary>
        /// Creates the smallest BoundingBox that contains the two specified BoundingBox instances.
        /// </summary>
        /// <param name="original">One of the BoundingBox instances to contain.</param>
        /// <param name="additional">One of the BoundingBox instances to contain.</param>
        /// <param name="result">[OutAttribute] The created BoundingBox.</param>
        public static void CreateMerged( ref BoundingBox original, ref BoundingBox additional, out BoundingBox result )
        {
            Vector3 vectorMin;
            Vector3.Min( ref original.Min, ref additional.Min, out vectorMin );

            Vector3 vectorMax;
            Vector3.Max( ref original.Max, ref additional.Max, out vectorMax );

            result.Min = vectorMin;
            result.Max = vectorMax;
        }

        /// <summary>
        /// Creates the smallest BoundingBox that will contain the specified BoundingSphere.
        /// </summary>
        /// <param name="sphere">The BoundingSphere to contain.</param>
        public static BoundingBox CreateFromSphere( BoundingSphere sphere )
        {
            BoundingBox box;

            box.Min.X = sphere.Center.X - sphere.Radius;
            box.Min.Y = sphere.Center.Y - sphere.Radius;
            box.Min.Z = sphere.Center.Z - sphere.Radius;
            box.Max.X = sphere.Center.X + sphere.Radius;
            box.Max.Y = sphere.Center.Y + sphere.Radius;
            box.Max.Z = sphere.Center.Z + sphere.Radius;

            return box;
        }

        /// <summary>
        /// Creates the smallest BoundingBox that will contain the specified BoundingSphere.
        /// </summary>
        /// <param name="sphere">The BoundingSphere to contain.</param>
        /// <param name="result">[OutAttribute] The created BoundingBox.</param>
        public static void CreateFromSphere( ref BoundingSphere sphere, out BoundingBox result )
        {
            result.Min.X = sphere.Center.X - sphere.Radius;
            result.Min.Y = sphere.Center.Y - sphere.Radius;
            result.Min.Z = sphere.Center.Z - sphere.Radius;
            result.Max.X = sphere.Center.X + sphere.Radius;
            result.Max.Y = sphere.Center.Y + sphere.Radius;
            result.Max.Z = sphere.Center.Z + sphere.Radius;
        }

        /// <summary>
        /// Creates the smallest BoundingBox that will contain a group of points.
        /// </summary>
        /// <param name="points">A list of points the BoundingBox should contain.</param>
        public static BoundingBox CreateFromPoints( IEnumerable<Vector3> points )
        {
            if ( points == null )
                throw new ArgumentNullException();

            Vector3 vector3 = new Vector3( float.MaxValue );
            Vector3 vector2 = new Vector3( float.MinValue );

            bool flag = false;
            foreach ( Vector3 vector in points )
            {
                Vector3 vector4 = vector;

                Vector3.Min( ref vector3, ref vector4, out vector3 );
                Vector3.Max( ref vector2, ref vector4, out vector2 );

                flag = true;
            }

            if ( !flag )
                throw new ArgumentException( "FrameworkResources.BoundingBoxZeroPoints" );

            return new BoundingBox( vector3, vector2 );
        }

        #endregion

        #region zh-CHS 共有方法 | en Public Methods

        /// <summary>
        /// Gets an array of points that make up the corners of the BoundingBox.
        /// </summary>
        public Vector3[] GetCorners()
        {
            return new Vector3[]
            {
                new Vector3( Min.X, Max.Y, Max.Z ),
                new Vector3( Max.X, Max.Y, Max.Z ),
                new Vector3( Max.X, Min.Y, Max.Z ),
                new Vector3( Min.X, Min.Y, Max.Z ),
                new Vector3( Min.X, Max.Y, Min.Z ),
                new Vector3( Max.X, Max.Y, Min.Z ),
                new Vector3( Max.X, Min.Y, Min.Z ),
                new Vector3( Min.X, Min.Y, Min.Z )
            };
        }

        /// <summary>
        /// Gets the array of points that make up the corners of the BoundingBox.
        /// </summary>
        /// <param name="corners">An existing array of at least 8 Vector3 points where the corners of the BoundingBox are written.</param>
        public void GetCorners( Vector3[] corners )
        {
            if ( corners == null )
                throw new ArgumentNullException( "corners" );

            if ( corners.Length < 8 )
                throw new ArgumentOutOfRangeException( "corners", "FrameworkResources.NotEnoughCorners" );

            corners[0].X = Min.X;
            corners[0].Y = Max.Y;
            corners[0].Z = Max.Z;

            corners[1].X = Max.X;
            corners[1].Y = Max.Y;
            corners[1].Z = Max.Z;

            corners[2].X = Max.X;
            corners[2].Y = Min.Y;
            corners[2].Z = Max.Z;

            corners[3].X = Min.X;
            corners[3].Y = Min.Y;
            corners[3].Z = Max.Z;

            corners[4].X = Min.X;
            corners[4].Y = Max.Y;
            corners[4].Z = Min.Z;

            corners[5].X = Max.X;
            corners[5].Y = Max.Y;
            corners[5].Z = Min.Z;

            corners[6].X = Max.X;
            corners[6].Y = Min.Y;
            corners[6].Z = Min.Z;

            corners[7].X = Min.X;
            corners[7].Y = Min.Y;
            corners[7].Z = Min.Z;
        }

        /// <summary>
        /// Checks whether the current BoundingBox intersects another BoundingBox.
        /// </summary>
        /// <param name="box">The BoundingBox to check for intersection with.</param>
        public bool Intersects( BoundingBox box )
        {
            if ( ( Max.X < box.Min.X ) || ( Min.X > box.Max.X ) )
                return false;

            if ( ( Max.Y < box.Min.Y ) || ( Min.Y > box.Max.Y ) )
                return false;

            return ( ( Max.Z >= box.Min.Z ) && ( Min.Z <= box.Max.Z ) );
        }

        /// <summary>
        /// Checks whether the current BoundingBox intersects another BoundingBox.
        /// </summary>
        /// <param name="box">The BoundingBox to check for intersection with.</param>
        /// <param name="result">[OutAttribute] true if the BoundingBox instances intersect; false otherwise.</param>
        public void Intersects( ref BoundingBox box, out bool result )
        {
            if ( Max.X >= box.Min.X && Min.X <= box.Max.X && Max.Y >= box.Min.Y && Min.Y <= box.Max.Y && Max.Z >= box.Min.Z && Min.Z <= box.Max.Z )
                result = true;
            else
                result = false;
        }

        /// <summary>
        /// Checks whether the current BoundingBox intersects a BoundingFrustum.
        /// </summary>
        /// <param name="frustum">The BoundingFrustum to check for intersection with.</param>
        public bool Intersects( BoundingFrustum frustum )
        {
            if ( null == frustum )
                throw new ArgumentNullException( "frustum", "FrameworkResources.NullNotAllowed" );

            return frustum.Intersects( this );
        }

        /// <summary>
        /// Checks whether the current BoundingBox intersects a Plane.
        /// </summary>
        /// <param name="plane">The Plane to check for intersection with.</param>
        public PlaneIntersectionType Intersects( Plane plane )
        {
            Vector3 vector1 = new Vector3 { X = ( plane.Normal.X >= 0f ) ? Min.X : Max.X, Y = ( plane.Normal.Y >= 0f ) ? Min.Y : Max.Y, Z = ( plane.Normal.Z >= 0f ) ? Min.Z : Max.Z };
            float num1 = ( ( plane.Normal.X * vector1.X ) + ( plane.Normal.Y * vector1.Y ) ) + ( plane.Normal.Z * vector1.Z );
            if ( ( num1 + plane.D ) > 0f )
                return PlaneIntersectionType.Front;

            Vector3 vector2 = new Vector3 { X = ( plane.Normal.X >= 0f ) ? Max.X : Min.X, Y = ( plane.Normal.Y >= 0f ) ? Max.Y : Min.Y, Z = ( plane.Normal.Z >= 0f ) ? Max.Z : Min.Z };
            float num2 = ( ( plane.Normal.X * vector2.X ) + ( plane.Normal.Y * vector2.Y ) ) + ( plane.Normal.Z * vector2.Z );
            if ( ( num2 + plane.D ) < 0f )
                return PlaneIntersectionType.Back;

            return PlaneIntersectionType.Intersecting;
        }

        /// <summary>
        /// Checks whether the current BoundingBox intersects a Plane.
        /// </summary>
        /// <param name="plane">The Plane to check for intersection with.</param>
        /// <param name="result">[OutAttribute] An enumeration indicating whether the BoundingBox intersects the Plane.</param>
        public void Intersects( ref Plane plane, out PlaneIntersectionType result )
        {
            Vector3 vector1 = new Vector3 { X = ( plane.Normal.X >= 0f ) ? Min.X : Max.X, Y = ( plane.Normal.Y >= 0f ) ? Min.Y : Max.Y, Z = ( plane.Normal.Z >= 0f ) ? Min.Z : Max.Z };
            float num1 = ( ( plane.Normal.X * vector1.X ) + ( plane.Normal.Y * vector1.Y ) ) + ( plane.Normal.Z * vector1.Z );
            if ( ( num1 + plane.D ) > 0f )
                result = PlaneIntersectionType.Front;
            else
            {
                Vector3 vector2 = new Vector3 { X = ( plane.Normal.X >= 0f ) ? Max.X : Min.X, Y = ( plane.Normal.Y >= 0f ) ? Max.Y : Min.Y, Z = ( plane.Normal.Z >= 0f ) ? Max.Z : Min.Z };
                float num2 = ( ( plane.Normal.X * vector2.X ) + ( plane.Normal.Y * vector2.Y ) ) + ( plane.Normal.Z * vector2.Z );
                if ( ( num2 + plane.D ) < 0f )
                    result = PlaneIntersectionType.Back;
                else
                    result = PlaneIntersectionType.Intersecting;
            }
        }

        /// <summary>
        /// Checks whether the current BoundingBox intersects a Ray.
        /// </summary>
        /// <param name="ray">The Ray to check for intersection with.</param>
        public float? Intersects(Ray ray)
        {
            float num = 0f;
        //    float maxValue = float.MaxValue;
        //    if (Math.Abs(ray.Direction.X) < 1E-06f)
        //    {
        //        if ((ray.Position.X < Min.X) || (ray.Position.X > Max.X))
        //        {
        //            return null;
        //        }
        //    }
        //    else
        //    {
        //        float num11 = 1f / ray.Direction.X;
        //        float num8 = (Min.X - ray.Position.X) * num11;
        //        float num7 = (Max.X - ray.Position.X) * num11;
        //        if (num8 > num7)
        //        {
        //            float num14 = num8;
        //            num8 = num7;
        //            num7 = num14;
        //        }
        //        num = MathHelper.Max(num8, num);
        //        maxValue = MathHelper.Min(num7, maxValue);
        //        if (num > maxValue)
        //        {
        //            return null;
        //        }
        //    }
        //    if (Math.Abs(ray.Direction.Y) < 1E-06f)
        //    {
        //        if ((ray.Position.Y < Min.Y) || (ray.Position.Y > Max.Y))
        //        {
        //            return null;
        //        }
        //    }
        //    else
        //    {
        //        float num10 = 1f / ray.Direction.Y;
        //        float num6 = (Min.Y - ray.Position.Y) * num10;
        //        float num5 = (Max.Y - ray.Position.Y) * num10;
        //        if (num6 > num5)
        //        {
        //            float num13 = num6;
        //            num6 = num5;
        //            num5 = num13;
        //        }
        //        num = MathHelper.Max(num6, num);
        //        maxValue = MathHelper.Min(num5, maxValue);
        //        if (num > maxValue)
        //        {
        //            return null;
        //        }
        //    }
        //    if (Math.Abs(ray.Direction.Z) < 1E-06f)
        //    {
        //        if ((ray.Position.Z < Min.Z) || (ray.Position.Z > Max.Z))
        //        {
        //            return null;
        //        }
        //    }
        //    else
        //    {
        //        float num9 = 1f / ray.Direction.Z;
        //        float num4 = (Min.Z - ray.Position.Z) * num9;
        //        float num3 = (Max.Z - ray.Position.Z) * num9;
        //        if (num4 > num3)
        //        {
        //            float num12 = num4;
        //            num4 = num3;
        //            num3 = num12;
        //        }
        //        num = MathHelper.Max(num4, num);
        //        maxValue = MathHelper.Min(num3, maxValue);
        //        if (num > maxValue)
        //        {
        //            return null;
        //        }
        //    }
            return new float?( num );
        }

        /// <summary>
        /// Checks whether the current BoundingBox intersects a Ray.
        /// </summary>
        /// <param name="ray">The Ray to check for intersection with.</param>
        /// <param name="result">[OutAttribute] Distance at which the ray intersects the BoundingBox, or null if there is no intersection.</param>
        public void Intersects(ref Ray ray, out float? result)
        {
            result = 0;
            float num = 0f;
        //    float maxValue = float.MaxValue;
        //    if (Math.Abs(ray.Direction.X) < 1E-06f)
        //    {
        //        if ((ray.Position.X < Min.X) || (ray.Position.X > Max.X))
        //        {
        //            return;
        //        }
        //    }
        //    else
        //    {
        //        float num11 = 1f / ray.Direction.X;
        //        float num8 = (Min.X - ray.Position.X) * num11;
        //        float num7 = (Max.X - ray.Position.X) * num11;
        //        if (num8 > num7)
        //        {
        //            float num14 = num8;
        //            num8 = num7;
        //            num7 = num14;
        //        }
        //        num = MathHelper.Max(num8, num);
        //        maxValue = MathHelper.Min(num7, maxValue);
        //        if (num > maxValue)
        //        {
        //            return;
        //        }
        //    }
        //    if (Math.Abs(ray.Direction.Y) < 1E-06f)
        //    {
        //        if ((ray.Position.Y < Min.Y) || (ray.Position.Y > Max.Y))
        //        {
        //            return;
        //        }
        //    }
        //    else
        //    {
        //        float num10 = 1f / ray.Direction.Y;
        //        float num6 = (Min.Y - ray.Position.Y) * num10;
        //        float num5 = (Max.Y - ray.Position.Y) * num10;
        //        if (num6 > num5)
        //        {
        //            float num13 = num6;
        //            num6 = num5;
        //            num5 = num13;
        //        }
        //        num = MathHelper.Max(num6, num);
        //        maxValue = MathHelper.Min(num5, maxValue);
        //        if (num > maxValue)
        //        {
        //            return;
        //        }
        //    }
        //    if (Math.Abs(ray.Direction.Z) < 1E-06f)
        //    {
        //        if ((ray.Position.Z < Min.Z) || (ray.Position.Z > Max.Z))
        //        {
        //            return;
        //        }
        //    }
        //    else
        //    {
        //        float num9 = 1f / ray.Direction.Z;
        //        float num4 = (Min.Z - ray.Position.Z) * num9;
        //        float num3 = (Max.Z - ray.Position.Z) * num9;
        //        if (num4 > num3)
        //        {
        //            float num12 = num4;
        //            num4 = num3;
        //            num3 = num12;
        //        }
        //        num = MathHelper.Max(num4, num);
        //        maxValue = MathHelper.Min(num3, maxValue);
        //        if (num > maxValue)
        //        {
        //            return;
        //        }
        //    }
            result = new float?( num );
        }

        /// <summary>
        /// Checks whether the current BoundingBox intersects a BoundingSphere.
        /// </summary>
        /// <param name="sphere">The BoundingSphere to check for intersection with.</param>
        public bool Intersects( BoundingSphere sphere )
        {
            Vector3 vector;
            Vector3.Clamp( ref sphere.Center, ref Min, ref Max, out vector );

            float num;
            Vector3.DistanceSquared( ref sphere.Center, ref vector, out num );

            return ( num <= ( sphere.Radius * sphere.Radius ) );
        }

        /// <summary>
        /// Checks whether the current BoundingBox intersects a BoundingSphere.
        /// </summary>
        /// <param name="sphere">The BoundingSphere to check for intersection with.</param>
        /// <param name="result">[OutAttribute] true if the BoundingBox and BoundingSphere intersect; false otherwise.</param>
        public void Intersects( ref BoundingSphere sphere, out bool result )
        {
            Vector3 vector;
            Vector3.Clamp( ref sphere.Center, ref Min, ref Max, out vector );

            float num;
            Vector3.DistanceSquared( ref sphere.Center, ref vector, out num );

            result = num <= ( sphere.Radius * sphere.Radius );
        }

        /// <summary>
        /// Tests whether the BoundingBox contains another BoundingBox.
        /// </summary>
        /// <param name="box">The BoundingBox to test for overlap.</param>
        public ContainmentType Contains( BoundingBox box )
        {
            if ( ( Max.X < box.Min.X ) || ( Min.X > box.Max.X ) )
                return ContainmentType.Disjoint;

            if ( ( Max.Y < box.Min.Y ) || ( Min.Y > box.Max.Y ) )
                return ContainmentType.Disjoint;

            if ( ( Max.Z < box.Min.Z ) || ( Min.Z > box.Max.Z ) )
                return ContainmentType.Disjoint;

            if ( ( ( ( Min.X <= box.Min.X ) && ( box.Max.X <= Max.X ) ) && ( ( Min.Y <= box.Min.Y ) && ( box.Max.Y <= Max.Y ) ) ) && ( ( Min.Z <= box.Min.Z ) && ( box.Max.Z <= Max.Z ) ) )
                return ContainmentType.Contains;

            return ContainmentType.Intersects;
        }

        /// <summary>
        /// Tests whether the BoundingBox contains a BoundingBox.
        /// </summary>
        /// <param name="box">The BoundingBox to test for overlap.</param>
        /// <param name="result">[OutAttribute] Enumeration indicating the extent of overlap.</param>
        public void Contains( ref BoundingBox box, out ContainmentType result )
        {
            if ( Max.X >= box.Min.X && Min.X <= box.Max.X && Max.Y >= box.Min.Y && Min.Y <= box.Max.Y && Max.Z >= box.Min.Z && Min.Z <= box.Max.Z )
                result = ( ( ( ( Min.X <= box.Min.X ) && ( box.Max.X <= Max.X ) ) && ( ( Min.Y <= box.Min.Y ) && ( box.Max.Y <= Max.Y ) ) ) && ( ( Min.Z <= box.Min.Z ) && ( box.Max.Z <= Max.Z ) ) ) ? ContainmentType.Contains : ContainmentType.Intersects;
            else
                result = ContainmentType.Disjoint;
        }

        /// <summary>Tests whether the BoundingBox contains a BoundingFrustum.</summary>
        /// <param name="frustum">The BoundingFrustum to test for overlap.</param>
        public ContainmentType Contains( BoundingFrustum frustum )
        {
            if ( null == frustum )
                throw new ArgumentNullException( "frustum", "FrameworkResources.NullNotAllowed" );

            if ( !frustum.Intersects( this ) )
                return ContainmentType.Disjoint;

            foreach ( Vector3 vector in frustum.cornerArray )
            {
                if ( Contains( vector ) == ContainmentType.Disjoint )
                    return ContainmentType.Intersects;
            }

            return ContainmentType.Contains;
        }

        /// <summary>
        /// Tests whether the BoundingBox contains a point.
        /// </summary>
        /// <param name="point">The point to test for overlap.</param>
        public ContainmentType Contains( Vector3 point )
        {
            if ( ( ( ( Min.X <= point.X ) && ( point.X <= Max.X ) ) && ( ( Min.Y <= point.Y ) && ( point.Y <= Max.Y ) ) ) && ( ( Min.Z <= point.Z ) && ( point.Z <= Max.Z ) ) )
                return ContainmentType.Contains;

            return ContainmentType.Disjoint;
        }

        /// <summary>
        /// Tests whether the BoundingBox contains a point.
        /// </summary>
        /// <param name="point">The point to test for overlap.</param>
        /// <param name="result">[OutAttribute] Enumeration indicating the extent of overlap.</param>
        public void Contains( ref Vector3 point, out ContainmentType result )
        {
            result = ( ( ( ( Min.X <= point.X ) && ( point.X <= Max.X ) ) && ( ( Min.Y <= point.Y ) && ( point.Y <= Max.Y ) ) ) && ( ( Min.Z <= point.Z ) && ( point.Z <= Max.Z ) ) ) ? ContainmentType.Contains : ContainmentType.Disjoint;
        }

        /// <summary>
        /// Tests whether the BoundingBox contains a BoundingSphere.
        /// </summary>
        /// <param name="sphere">The BoundingSphere to test for overlap.</param>
        public ContainmentType Contains( BoundingSphere sphere )
        {
            Vector3 vector;
            Vector3.Clamp( ref sphere.Center, ref Min, ref Max, out vector );

            float num;
            Vector3.DistanceSquared( ref sphere.Center, ref vector, out num );

            float radius = sphere.Radius;
            if ( num > ( radius * radius ) )
                return ContainmentType.Disjoint;

            if ( ( ( ( ( Min.X + radius ) <= sphere.Center.X ) && ( sphere.Center.X <= ( Max.X - radius ) ) ) && ( ( ( Max.X - Min.X ) > radius ) && ( ( Min.Y + radius ) <= sphere.Center.Y ) ) ) && ( ( ( sphere.Center.Y <= ( Max.Y - radius ) ) && ( ( Max.Y - Min.Y ) > radius ) ) && ( ( ( ( Min.Z + radius ) <= sphere.Center.Z ) && ( sphere.Center.Z <= ( Max.Z - radius ) ) ) && ( ( Max.X - Min.X ) > radius ) ) ) )
                return ContainmentType.Contains;

            return ContainmentType.Intersects;
        }

        /// <summary>
        /// Tests whether the BoundingBox contains a BoundingSphere.
        /// </summary>
        /// <param name="sphere">The BoundingSphere to test for overlap.</param>
        /// <param name="result">[OutAttribute] Enumeration indicating the extent of overlap.</param>
        public void Contains( ref BoundingSphere sphere, out ContainmentType result )
        {
            Vector3 vector;
            Vector3.Clamp( ref sphere.Center, ref Min, ref Max, out vector );

            float num;
            Vector3.DistanceSquared( ref sphere.Center, ref vector, out num );

            float radius = sphere.Radius;
            if ( num > ( radius * radius ) )
                result = ContainmentType.Disjoint;
            else
                result = ( ( ( ( ( Min.X + radius ) <= sphere.Center.X ) && ( sphere.Center.X <= ( Max.X - radius ) ) ) && ( ( ( Max.X - Min.X ) > radius ) && ( ( Min.Y + radius ) <= sphere.Center.Y ) ) ) && ( ( ( sphere.Center.Y <= ( Max.Y - radius ) ) && ( ( Max.Y - Min.Y ) > radius ) ) && ( ( ( ( Min.Z + radius ) <= sphere.Center.Z ) && ( sphere.Center.Z <= ( Max.Z - radius ) ) ) && ( ( Max.X - Min.X ) > radius ) ) ) ) ? ContainmentType.Contains : ContainmentType.Intersects;
        }

        #endregion

        #region zh-CHS 内部方法 | en Internal Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="v"></param>
        /// <param name="result"></param>
        internal void SupportMapping( ref Vector3 v, out Vector3 result )
        {
            result.X = ( v.X >= 0f ) ? Max.X : Min.X;
            result.Y = ( v.Y >= 0f ) ? Max.Y : Min.Y;
            result.Z = ( v.Z >= 0f ) ? Max.Z : Min.Z;
        }

        #endregion

        #region zh-CHS 共有静态方法 | en Public Static Methods

        /// <summary>
        /// Determines whether two instances of BoundingBox are equal.
        /// </summary>
        /// <param name="a">BoundingBox to compare.</param>
        /// <param name="b">BoundingBox to compare.</param>
        public static bool operator ==( BoundingBox a, BoundingBox b )
        {
            return a.Equals( b );
        }

        /// <summary>
        /// Determines whether two instances of BoundingBox are not equal.
        /// </summary>
        /// <param name="a">The object to the left of the inequality operator.</param>
        /// <param name="b">The object to the right of the inequality operator.</param>
        public static bool operator !=( BoundingBox a, BoundingBox b )
        {
            if ( !( a.Min != b.Min ) )
            {
                return ( a.Max != b.Max );
            }
            return true;
        }

        #endregion

        #region zh-CHS 方法覆盖 | en Override Methods

        /// <summary>
        /// Determines whether two instances of BoundingBox are equal.
        /// </summary>
        /// <param name="obj">The Object to compare with the current BoundingBox.</param>
        public override bool Equals( object obj )
        {
            if ( obj is BoundingBox )
                return Equals( (BoundingBox)obj );
            else
                return false;
        }

        /// <summary>
        /// Gets the hash code for this instance.
        /// </summary>
        public override int GetHashCode()
        {
            return ( Min.GetHashCode() + Max.GetHashCode() );
        }

        /// <summary>
        /// Returns a String that represents the current BoundingBox.
        /// </summary>
        public override string ToString()
        {
            return string.Format( CultureInfo.CurrentCulture, "{{Min:{0} Max:{1}}}", new object[] { Min.ToString(), Max.ToString() } );
        }

        #endregion

        #region zh-CHS 接口实现 | en Interface Implementation

        /// <summary>
        /// Determines whether two instances of BoundingBox are equal.
        /// </summary>
        /// <param name="other">The BoundingBox to compare with the current BoundingBox.</param>
        public bool Equals( BoundingBox other )
        {
            return ( ( Min == other.Min ) && ( Max == other.Max ) );
        }

        #endregion

    }
}
#endregion