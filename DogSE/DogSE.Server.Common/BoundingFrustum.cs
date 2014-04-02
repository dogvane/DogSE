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

    /// <summary>Defines a frustum and helps determine whether forms intersect with it.</summary>
    public class BoundingFrustum : IEquatable<BoundingFrustum>
    {

        #region zh-CHS 共有常量 | en Public Constants

        /// <summary>
        /// Specifies the total number of corners (8) in the BoundingFrustum.
        /// </summary>
        public const int CornerCount = 8;

        #endregion

        #region zh-CHS 私有常量 | en Private Constants

        private const int BottomPlaneIndex = 5;

        private const int FarPlaneIndex = 1;

        private const int LeftPlaneIndex = 2;

        private const int NearPlaneIndex = 0;

        private const int NumPlanes = 6;

        private const int RightPlaneIndex = 3;

        private const int TopPlaneIndex = 4;

        #endregion

        #region zh-CHS 私有成员变量 | en Private Member Variables

        //private Gjk gjk;

        private Matrix matrix;

        private Plane[] planes = new Plane[6];

        internal Vector3[] cornerArray = new Vector3[8];

        #endregion

        #region zh-CHS 构造和初始化和清理 | en Constructors and Initializers and Dispose
        /// <summary>
        /// 
        /// </summary>
        private BoundingFrustum()
        {
        }


        /// <summary>
        /// Creates a new instance of BoundingFrustum.
        /// </summary>
        /// <param name="value">Combined matrix that usually takes view × projection matrix.</param>
        public BoundingFrustum( Matrix value )
        {
            SetMatrix( ref value );
        }

        #endregion

        #region zh-CHS 共有属性 | en Public Properties

        /// <summary>
        /// Gets the bottom plane of the BoundingFrustum.
        /// </summary>
        public Plane Bottom
        {
            get { return planes[BottomPlaneIndex]; }
        }

        /// <summary>
        /// Gets the far plane of the BoundingFrustum.
        /// </summary>
        public Plane Far
        {
            get { return planes[FarPlaneIndex]; }
        }

        /// <summary>
        /// Gets the left plane of the BoundingFrustum.
        /// </summary>
        public Plane Left
        {
            get { return planes[LeftPlaneIndex]; }
        }

        /// <summary>
        /// Gets or sets the Matrix that describes this bounding frustum.
        /// </summary>
        public Matrix Matrix
        {
            get { return matrix; }
            set { SetMatrix( ref value ); }
        }

        /// <summary>
        /// Gets the near plane of the BoundingFrustum.
        /// </summary>
        public Plane Near
        {
            get { return planes[NearPlaneIndex]; }
        }

        /// <summary>
        /// Gets the right plane of the BoundingFrustum.
        /// </summary>
        public Plane Right
        {
            get { return planes[RightPlaneIndex]; }
        }

        /// <summary>
        /// Gets the top plane of the BoundingFrustum.
        /// </summary>
        public Plane Top
        {
            get { return planes[TopPlaneIndex]; }
        }

        #endregion

        #region zh-CHS 共有方法 | en Public Methods

        private static Vector3 ComputeIntersection( ref Plane plane, ref Ray ray )
        {
            float num = ( -plane.D - Vector3.Dot( plane.Normal, ray.Position ) ) / Vector3.Dot( plane.Normal, ray.Direction );
            return ( ray.Position + ( (Vector3)( ray.Direction * num ) ) );
        }

        private static Ray ComputeIntersectionLine( ref Plane p1, ref Plane p2 )
        {
            Ray ray = new Ray
            {
                Direction = Vector3.Cross( p1.Normal, p2.Normal )
            };
            float num = ray.Direction.LengthSquared();
            ray.Position = (Vector3)( Vector3.Cross( (Vector3)( ( -p1.D * p2.Normal ) + ( p2.D * p1.Normal ) ), ray.Direction ) / num );
            return ray;
        }

        /// <summary>
        /// Checks whether the current BoundingFrustum contains the specified BoundingBox.
        /// </summary>
        /// <param name="box">The BoundingBox to check against the current BoundingFrustum.</param>
        public ContainmentType Contains( BoundingBox box )
        {
            bool flag = false;
            foreach ( Plane plane in planes )
            {
                switch ( box.Intersects( plane ) )
                {
                    case PlaneIntersectionType.Front:
                        return ContainmentType.Disjoint;

                    case PlaneIntersectionType.Intersecting:
                        flag = true;
                        break;

                    case PlaneIntersectionType.Back:
                    default:
                        break;
                }
            }

            if ( !flag )
                return ContainmentType.Contains;

            return ContainmentType.Intersects;
        }

        /// <summary>
        /// Checks whether the current BoundingFrustum contains the specified BoundingFrustum.
        /// </summary>
        /// <param name="frustum">The BoundingFrustum to check against the current BoundingFrustum.</param>
        public ContainmentType Contains( BoundingFrustum frustum )
        {
            if ( frustum == null )
                throw new ArgumentNullException( "frustum" );

            ContainmentType disjoint = ContainmentType.Disjoint;
            if ( Intersects( frustum ) )
            {
                disjoint = ContainmentType.Contains;

                for ( int i = 0; i < cornerArray.Length; i++ )
                {
                    if ( Contains( frustum.cornerArray[i] ) == ContainmentType.Disjoint )
                        return ContainmentType.Intersects;
                }
            }

            return disjoint;
        }

        /// <summary>
        /// Checks whether the current BoundingFrustum contains the specified BoundingSphere.
        /// </summary>
        /// <param name="sphere">The BoundingSphere to check against the current BoundingFrustum.</param>
        public ContainmentType Contains( BoundingSphere sphere )
        {
            Vector3 center = sphere.Center;
            float radius = sphere.Radius;

            int num = 0;
            foreach ( Plane plane in planes )
            {
                float num1 = ( ( plane.Normal.X * center.X ) + ( plane.Normal.Y * center.Y ) ) + ( plane.Normal.Z * center.Z ) + plane.D;

                if ( num1 > radius )
                    return ContainmentType.Disjoint;

                if ( num1 < -radius )
                    num++;
            }

            if ( num != NumPlanes )
                return ContainmentType.Intersects;

            return ContainmentType.Contains;
        }

        /// <summary>
        /// Checks whether the current BoundingFrustum contains the specified point.
        /// </summary>
        /// <param name="point">The point to check against the current BoundingFrustum.</param>
        public ContainmentType Contains( Vector3 point )
        {
            foreach ( Plane plane in planes )
            {
                float num = ( ( ( plane.Normal.X * point.X ) + ( plane.Normal.Y * point.Y ) ) + ( plane.Normal.Z * point.Z ) ) + plane.D;

                if ( num > 1E-05f )
                    return ContainmentType.Disjoint;
            }

            return ContainmentType.Contains;
        }

        /// <summary>
        /// Checks whether the current BoundingFrustum contains the specified BoundingBox.
        /// </summary>
        /// <param name="box">The BoundingBox to test for overlap.</param>
        /// <param name="result">[OutAttribute] Enumeration indicating the extent of overlap.</param>
        public void Contains( ref BoundingBox box, out ContainmentType result )
        {
            bool flag = false;

            foreach ( Plane plane in planes )
            {
                switch ( box.Intersects( plane ) )
                {
                    case PlaneIntersectionType.Front:
                        result = ContainmentType.Disjoint;
                        return;

                    case PlaneIntersectionType.Intersecting:
                        flag = true;
                        break;

                    case PlaneIntersectionType.Back:
                    default:
                        break;
                }
            }

            result = flag ? ContainmentType.Intersects : ContainmentType.Contains;
        }

        /// <summary>
        /// Checks whether the current BoundingFrustum contains the specified BoundingSphere.
        /// </summary>
        /// <param name="sphere">The BoundingSphere to test for overlap.</param>
        /// <param name="result">[OutAttribute] Enumeration indicating the extent of overlap.</param>
        public void Contains( ref BoundingSphere sphere, out ContainmentType result )
        {
            Vector3 center = sphere.Center;
            float radius = sphere.Radius;

            int num = 0;
            foreach ( Plane plane in planes )
            {
                float num1 = ( ( plane.Normal.X * center.X ) + ( plane.Normal.Y * center.Y ) ) + ( plane.Normal.Z * center.Z ) + plane.D;

                if ( num1 > radius )
                {
                    result = ContainmentType.Disjoint;
                    return;
                }

                if ( num1 < -radius )
                    num++;
            }

            result = ( num == NumPlanes ) ? ContainmentType.Contains : ContainmentType.Intersects;
        }

        /// <summary>
        /// Checks whether the current BoundingFrustum contains the specified point.
        /// </summary>
        /// <param name="point">The point to test for overlap.</param>
        /// <param name="result">[OutAttribute] Enumeration indicating the extent of overlap.</param>
        public void Contains( ref Vector3 point, out ContainmentType result )
        {
            foreach ( Plane plane in planes )
            {
                float num = ( ( ( plane.Normal.X * point.X ) + ( plane.Normal.Y * point.Y ) ) + ( plane.Normal.Z * point.Z ) ) + plane.D;

                if ( num > 1E-05f )
                {
                    result = ContainmentType.Disjoint;
                    return;
                }
            }

            result = ContainmentType.Contains;
        }

        /// <summary>
        /// Gets an array of points that make up the corners of the BoundingFrustum.
        /// </summary>
        public Vector3[] GetCorners()
        {
            return (Vector3[])cornerArray.Clone();
        }

        /// <summary>
        /// Gets an array of points that make up the corners of the BoundingFrustum.
        /// </summary>
        /// <param name="corners">An existing array of at least 8 Vector3 points where the corners of the BoundingFrustum are written.</param>
        public void GetCorners( Vector3[] corners )
        {
            if ( corners == null )
                throw new ArgumentNullException( "corners" );

            if ( corners.Length < 8 )
                throw new ArgumentOutOfRangeException( "corners", "FrameworkResources.NotEnoughCorners" );

            cornerArray.CopyTo( corners, 0 );
        }


        /// <summary>Checks whether the current BoundingFrustum intersects the specified BoundingBox.</summary>
        /// <param name="box">The BoundingBox to check for intersection.</param>
        public bool Intersects( BoundingBox box )
        {
            bool flag;
            Intersects( ref box, out flag );

            return flag;
        }

        /// <summary>Checks whether the current BoundingFrustum intersects the specified BoundingFrustum.</summary>
        /// <param name="frustum">The BoundingFrustum to check for intersection.</param>
        public bool Intersects( BoundingFrustum frustum )
        {
            if ( frustum == null )
                throw new ArgumentNullException( "frustum" );

            //if (gjk == null)
            //    gjk = new Gjk();

            //gjk.Reset();

            Vector3 closestPoint;
            Vector3.Subtract( ref cornerArray[0], ref frustum.cornerArray[0], out closestPoint );

            if ( closestPoint.LengthSquared() < 1E-05f )
                Vector3.Subtract( ref cornerArray[0], ref frustum.cornerArray[1], out closestPoint );

            //float maxValue = float.MaxValue;
            //float num3 = 0f;
            //do
            //{
            //    Vector3 vector5 = new Vector3 { X = -closestPoint.X, Y = -closestPoint.Y, Z = -closestPoint.Z };

            //    Vector3 vector4;
            //    SupportMapping( ref vector5, out vector4 );

            //    Vector3 vector3;
            //    frustum.SupportMapping( ref closestPoint, out vector3 );

            //    Vector3 vector2;
            //    Vector3.Subtract( ref vector4, ref vector3, out vector2 );

            //    float num4 = ( ( closestPoint.X * vector2.X ) + ( closestPoint.Y * vector2.Y ) ) + ( closestPoint.Z * vector2.Z );
            //    if ( num4 > 0f )
            //        return false;

            //    //gjk.AddSupportPoint( ref vector2 );

            //    closestPoint = gjk.ClosestPoint;

            //    float num2 = maxValue;
            //    maxValue = closestPoint.LengthSquared();

            //    num3 = 4E-05f * gjk.MaxLengthSquared;

            //    if ( ( num2 - maxValue ) <= ( 1E-05f * num2 ) )
            //        return false;

            //} while ( !gjk.FullSimplex && ( maxValue >= num3 ) );

            return true;
        }

        /// <summary>Checks whether the current BoundingFrustum intersects the specified BoundingSphere.</summary>
        /// <param name="sphere">The BoundingSphere to check for intersection.</param>
        public bool Intersects( BoundingSphere sphere )
        {
            bool flag;
            Intersects( ref sphere, out flag );

            return flag;
        }

        /// <summary>Checks whether the current BoundingFrustum intersects the specified Plane.</summary>
        /// <param name="plane">The Plane to check for intersection.</param>
        public PlaneIntersectionType Intersects( Plane plane )
        {
            int num = 0;
            for ( int i = 0; i < 8; i++ )
            {
                float num1;
                Vector3.Dot( ref cornerArray[i], ref plane.Normal, out num1 );

                if ( ( num1 + plane.D ) > 0f )
                    num |= 1;
                else
                    num |= 2;

                if ( num == 3 )
                    return PlaneIntersectionType.Intersecting;
            }

            if ( num != 1 )
                return PlaneIntersectionType.Back;

            return PlaneIntersectionType.Front;
        }

        /// <summary>Checks whether the current BoundingFrustum intersects the specified Ray.</summary>
        /// <param name="ray">The Ray to check for intersection.</param>
        public float? Intersects( Ray ray )
        {
            float? nullable;
            Intersects( ref ray, out nullable );
            return nullable;
        }

        /// <summary>Checks whether the current BoundingFrustum intersects a BoundingBox.</summary>
        /// <param name="box">The BoundingBox to check for intersection with.</param>
        /// <param name="result">[OutAttribute] true if the BoundingFrustum and BoundingBox intersect; false otherwise.</param>
        public void Intersects( ref BoundingBox box, out bool result )
        {
            Vector3 closestPoint;
            Vector3 vector2;
            Vector3 vector3;
            Vector3 vector4;
            Vector3 vector5;

            //if ( gjk == null )
            //    gjk = new Gjk();

            //gjk.Reset();

            Vector3.Subtract( ref cornerArray[0], ref box.Min, out closestPoint );
            if ( closestPoint.LengthSquared() < 1E-05f )
                Vector3.Subtract( ref cornerArray[0], ref box.Max, out closestPoint );

            float maxValue = float.MaxValue;
            //float num3 = 0f;
            result = false;

        //Label_006D:
            vector5.X = -closestPoint.X;
            vector5.Y = -closestPoint.Y;
            vector5.Z = -closestPoint.Z;

            SupportMapping( ref vector5, out vector4 );
            box.SupportMapping( ref closestPoint, out vector3 );
            Vector3.Subtract( ref vector4, ref vector3, out vector2 );

            float num4 = ( ( closestPoint.X * vector2.X ) + ( closestPoint.Y * vector2.Y ) ) + ( closestPoint.Z * vector2.Z );
            if ( num4 <= 0f )
            {
                //gjk.AddSupportPoint( ref vector2 );
                //closestPoint = gjk.ClosestPoint;

                float num2 = maxValue;
                maxValue = closestPoint.LengthSquared();
                if ( ( num2 - maxValue ) > ( 1E-05f * num2 ) )
                {
                    //num3 = 4E-05f * gjk.MaxLengthSquared;
                    //if ( !gjk.FullSimplex && ( maxValue >= num3 ) )
                    //    goto Label_006D;

                    result = true;
                }
            }
        }

        /// <summary>Checks whether the current BoundingFrustum intersects a BoundingSphere.</summary>
        /// <param name="sphere">The BoundingSphere to check for intersection with.</param>
        /// <param name="result">[OutAttribute] true if the BoundingFrustum and BoundingSphere intersect; false otherwise.</param>
        public void Intersects( ref BoundingSphere sphere, out bool result )
        {
            Vector3 unitX;
            Vector3 vector2;
            Vector3 vector3;
            Vector3 vector4;
            Vector3 vector5;

            //if ( gjk == null )
            //    gjk = new Gjk();

            //gjk.Reset();

            Vector3.Subtract( ref cornerArray[0], ref sphere.Center, out unitX );

            if ( unitX.LengthSquared() < 1E-05f )
                unitX = Vector3.UnitX;

            float maxValue = float.MaxValue;
            //float num3 = 0f;
            result = false;

        //Label_005A:

            vector5.X = -unitX.X;
            vector5.Y = -unitX.Y;
            vector5.Z = -unitX.Z;

            SupportMapping( ref vector5, out vector4 );
            sphere.SupportMapping( ref unitX, out vector3 );
            Vector3.Subtract( ref vector4, ref vector3, out vector2 );

            float num4 = ( ( unitX.X * vector2.X ) + ( unitX.Y * vector2.Y ) ) + ( unitX.Z * vector2.Z );
            if ( num4 <= 0f )
            {
                //gjk.AddSupportPoint( ref vector2 );
                //unitX = gjk.ClosestPoint;

                float num2 = maxValue;
                maxValue = unitX.LengthSquared();

                if ( ( num2 - maxValue ) > ( 1E-05f * num2 ) )
                {
                    //num3 = 4E-05f * gjk.MaxLengthSquared;
                    //if ( !gjk.FullSimplex && ( maxValue >= num3 ) )
                    //    goto Label_005A;

                    result = true;
                }
            }
        }

        /// <summary>
        /// Checks whether the current BoundingFrustum intersects a Plane.
        /// </summary>
        /// <param name="plane">The Plane to check for intersection with.</param>
        /// <param name="result">[OutAttribute] An enumeration indicating whether the BoundingFrustum intersects the Plane.</param>
        public void Intersects( ref Plane plane, out PlaneIntersectionType result )
        {
            int num = 0;
            for ( int i = 0; i < 8; i++ )
            {
                float num1;
                Vector3.Dot( ref cornerArray[i], ref plane.Normal, out num1 );

                if ( ( num1 + plane.D ) > 0f )
                    num |= 1;
                else
                    num |= 2;

                if ( num == 3 )
                {
                    result = PlaneIntersectionType.Intersecting;
                    return;
                }
            }

            result = ( num == 1 ) ? PlaneIntersectionType.Front : PlaneIntersectionType.Back;
        }

        /// <summary>
        /// Checks whether the current BoundingFrustum intersects a Ray.
        /// </summary>
        /// <param name="ray">The Ray to check for intersection with.</param>
        /// <param name="result">[OutAttribute] Distance at which the ray intersects the BoundingFrustum or null if there is no intersection.</param>
        public void Intersects(ref Ray ray, out float? result)
        {
        //    ContainmentType type;
        //    Contains(ref ray.Position, out type);
        //    if (type == ContainmentType.Contains)
        //    {
            result = 0f;
        //    }
        //    else
        //    {
        //        float minValue = float.MinValue;
        //        float maxValue = float.MaxValue;
        //        result = 0;
        //        foreach (Plane plane in planes)
        //        {
        //            float num3;
        //            float num6;
        //            Vector3 normal = plane.Normal;
        //            Vector3.Dot(ref ray.Direction, ref normal, out num6);
        //            Vector3.Dot(ref ray.Position, ref normal, out num3);
        //            num3 += plane.D;
        //            if (Math.Abs(num6) < 1E-05f)
        //            {
        //                if (num3 > 0f)
        //                {
        //                    return;
        //                }
        //            }
        //            else
        //            {
        //                float num = -num3 / num6;
        //                if (num6 < 0f)
        //                {
        //                    if (num > maxValue)
        //                    {
        //                        return;
        //                    }
        //                    if (num > minValue)
        //                    {
        //                        minValue = num;
        //                    }
        //                }
        //                else
        //                {
        //                    if (num < minValue)
        //                    {
        //                        return;
        //                    }
        //                    if (num < maxValue)
        //                    {
        //                        maxValue = num;
        //                    }
        //                }
        //            }
        //        }
        //        float num7 = (minValue >= 0f) ? minValue : maxValue;
        //        if (num7 >= 0f)
        //        {
        //            result = new float?(num7);
        //        }
        //    }
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
            float num;
            Vector3.Dot( ref cornerArray[0], ref v, out num );

            int index = 0;
            for ( int i = 1; i < cornerArray.Length; i++ )
            {
                float num2;
                Vector3.Dot( ref cornerArray[i], ref v, out num2 );

                if ( num2 > num )
                {
                    index = i;
                    num = num2;
                }
            }

            result = cornerArray[index];
        }

        #endregion

        #region zh-CHS 私有方法 | en Private Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        private void SetMatrix( ref Matrix value )
        {
            matrix = value;

            planes[2].Normal.X = -value.M14 - value.M11;
            planes[2].Normal.Y = -value.M24 - value.M21;
            planes[2].Normal.Z = -value.M34 - value.M31;
            planes[2].D = -value.M44 - value.M41;

            planes[3].Normal.X = -value.M14 + value.M11;
            planes[3].Normal.Y = -value.M24 + value.M21;
            planes[3].Normal.Z = -value.M34 + value.M31;
            planes[3].D = -value.M44 + value.M41;

            planes[4].Normal.X = -value.M14 + value.M12;
            planes[4].Normal.Y = -value.M24 + value.M22;
            planes[4].Normal.Z = -value.M34 + value.M32;
            planes[4].D = -value.M44 + value.M42;

            planes[5].Normal.X = -value.M14 - value.M12;
            planes[5].Normal.Y = -value.M24 - value.M22;
            planes[5].Normal.Z = -value.M34 - value.M32;
            planes[5].D = -value.M44 - value.M42;

            planes[0].Normal.X = -value.M13;
            planes[0].Normal.Y = -value.M23;
            planes[0].Normal.Z = -value.M33;
            planes[0].D = -value.M43;

            planes[1].Normal.X = -value.M14 + value.M13;
            planes[1].Normal.Y = -value.M24 + value.M23;
            planes[1].Normal.Z = -value.M34 + value.M33;
            planes[1].D = -value.M44 + value.M43;

            for ( int i = 0; i < NumPlanes; i++ )
            {
                float num2 = planes[i].Normal.Length();

                planes[i].Normal = (Vector3)( planes[i].Normal / num2 );
                planes[i].D /= num2;
            }

            //Ray ray = ComputeIntersectionLine( ref planes[0], ref planes[2] );
            //cornerArray[0] = ComputeIntersection( ref planes[4], ref ray );
            //cornerArray[3] = ComputeIntersection( ref planes[5], ref ray );

            //ray = ComputeIntersectionLine( ref planes[3], ref planes[0] );
            //cornerArray[1] = ComputeIntersection( ref planes[4], ref ray );
            //cornerArray[2] = ComputeIntersection( ref planes[5], ref ray );

            //ray = ComputeIntersectionLine( ref planes[2], ref planes[1] );
            //cornerArray[4] = ComputeIntersection( ref planes[4], ref ray );
            //cornerArray[7] = ComputeIntersection( ref planes[5], ref ray );

            //ray = ComputeIntersectionLine( ref planes[1], ref planes[3] );
            //cornerArray[5] = ComputeIntersection( ref planes[4], ref ray );
            //cornerArray[6] = ComputeIntersection( ref planes[5], ref ray );
        }
        #endregion

        #region zh-CHS 共有静态方法 | en Public Static Methods

        /// <summary>Determines whether two instances of BoundingFrustum are equal.</summary>
        /// <param name="a">The BoundingFrustum to the left of the equality operator.</param>
        /// <param name="b">The BoundingFrustum to the right of the equality operator.</param>
        public static bool operator ==( BoundingFrustum a, BoundingFrustum b )
        {
            return object.Equals( a, b );
        }

        /// <summary>Determines whether two instances of BoundingFrustum are not equal.</summary>
        /// <param name="a">The BoundingFrustum to the left of the inequality operator.</param>
        /// <param name="b">The BoundingFrustum to the right of the inequality operator.</param>
        public static bool operator !=( BoundingFrustum a, BoundingFrustum b )
        {
            return !object.Equals( a, b );
        }

        #endregion

        #region zh-CHS 方法覆盖 | en Override Methods

        /// <summary>
        /// Determines whether the specified Object is equal to the BoundingFrustum.
        /// </summary>
        /// <param name="obj">The Object to compare with the current BoundingFrustum.</param>
        public override bool Equals( object obj )
        {
            BoundingFrustum frustum = obj as BoundingFrustum;
            if ( frustum != null )
                return matrix == frustum.matrix;
            else
                return false;
        }

        /// <summary>
        /// Gets the hash code for this instance.
        /// </summary>
        public override int GetHashCode()
        {
            return matrix.GetHashCode();
        }

        /// <summary>Returns a String that represents the current BoundingFrustum.</summary>
        public override string ToString()
        {
            return string.Format( CultureInfo.CurrentCulture, "{{Near:{0} Far:{1} Left:{2} Right:{3} Top:{4} Bottom:{5}}}", new object[] { Near.ToString(), Far.ToString(), Left.ToString(), Right.ToString(), Top.ToString(), Bottom.ToString() } );
        }

        #endregion

        #region zh-CHS 接口实现 | en Interface Implementation

        /// <summary>
        /// Determines whether the specified BoundingFrustum is equal to the current BoundingFrustum.
        /// </summary>
        /// <param name="other">The BoundingFrustum to compare with the current BoundingFrustum.</param>
        public bool Equals( BoundingFrustum other )
        {
            if ( other == null )
                return false;

            return ( matrix == other.matrix );
        }

        #endregion

    }
}
#endregion
