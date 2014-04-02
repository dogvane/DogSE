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
    /// Defines a sphere.
    /// </summary>
    public struct BoundingSphere : IEquatable<BoundingSphere>
    {

        #region zh-CHS 共有成员变量 | en Public Member Variables

        /// <summary>
        /// The center point of the sphere.
        /// </summary>
        public Vector3 Center;
        /// <summary>
        /// The radius of the sphere.
        /// </summary>
        public float Radius;

        #endregion

        #region zh-CHS 构造和初始化和清理 | en Constructors and Initializers and Dispose

        /// <summary>
        /// Creates a new instance of BoundingSphere.
        /// </summary>
        /// <param name="center">Center point of the sphere.</param>
        /// <param name="radius">Radius of the sphere.</param>
        public BoundingSphere( Vector3 center, float radius )
        {
            if ( radius < 0f )
                throw new ArgumentException( "FrameworkResources.NegativeRadius" );

            Center = center;
            Radius = radius;
        }

        #endregion

        #region zh-CHS 共有方法 | en Public Methods

        /// <summary>
        /// Checks whether the current BoundingSphere intersects with a specified BoundingBox.
        /// </summary>
        /// <param name="box">The BoundingBox to check for intersection with the current BoundingSphere.</param>
        public bool Intersects( BoundingBox box )
        {
            Vector3 vector;
            Vector3.Clamp( ref Center, ref box.Min, ref box.Max, out vector );

            float num;
            Vector3.DistanceSquared( ref Center, ref vector, out num );

            return ( num <= ( Radius * Radius ) );
        }

        /// <summary>
        /// Checks whether the current BoundingSphere intersects a BoundingBox.
        /// </summary>
        /// <param name="box">The BoundingBox to check for intersection with.</param>
        /// <param name="result">[OutAttribute] true if the BoundingSphere and BoundingBox intersect; false otherwise.</param>
        public void Intersects( ref BoundingBox box, out bool result )
        {
            Vector3 vector;
            Vector3.Clamp( ref Center, ref box.Min, ref box.Max, out vector );

            float num;
            Vector3.DistanceSquared( ref Center, ref vector, out num );

            result = num <= ( Radius * Radius );
        }

        /// <summary>
        /// Checks whether the current BoundingSphere intersects with a specified BoundingFrustum.
        /// </summary>
        /// <param name="frustum">The BoundingFrustum to check for intersection with the current BoundingSphere.</param>
        public bool Intersects( BoundingFrustum frustum )
        {
            if ( null == frustum )
                throw new ArgumentNullException( "frustum", "FrameworkResources.NullNotAllowed" );

            bool flag;
            frustum.Intersects( ref this, out flag );

            return flag;
        }

        /// <summary>
        /// Checks whether the current BoundingSphere intersects with a specified Plane.
        /// </summary>
        /// <param name="plane">The Plane to check for intersection with the current BoundingSphere.</param>
        public PlaneIntersectionType Intersects( Plane plane )
        {
            return plane.Intersects( this );
        }

        /// <summary>
        /// Checks whether the current BoundingSphere intersects a Plane.
        /// </summary>
        /// <param name="plane">The Plane to check for intersection with.</param>
        /// <param name="result">[OutAttribute] An enumeration indicating whether the BoundingSphere intersects the Plane.</param>
        public void Intersects( ref Plane plane, out PlaneIntersectionType result )
        {
            plane.Intersects( ref this, out result );
        }

        ///// <summary>
        ///// Checks whether the current BoundingSphere intersects with a specified Ray.
        ///// </summary>
        ///// <param name="ray">The Ray to check for intersection with the current BoundingSphere.</param>
        //public float? Intersects(Ray ray)
        //{
        //    return ray.Intersects(this);
        //}

        ///// <summary>
        ///// Checks whether the current BoundingSphere intersects a Ray.
        ///// </summary>
        ///// <param name="ray">The Ray to check for intersection with.</param>
        ///// <param name="result">[OutAttribute] Distance at which the ray intersects the BoundingSphere or null if there is no intersection.</param>
        //public void Intersects(ref Ray ray, out float? result)
        //{
        //    ray.Intersects(ref this, out result);
        //}

        /// <summary>
        /// Checks whether the current BoundingSphere intersects with a specified BoundingSphere.
        /// </summary>
        /// <param name="sphere">The BoundingSphere to check for intersection with the current BoundingSphere.</param>
        public bool Intersects( BoundingSphere sphere )
        {
            float result;
            Vector3.DistanceSquared( ref Center, ref sphere.Center, out result );

            float num = sphere.Radius;
            if ( ( ( ( Radius * Radius ) + ( ( 2f * Radius ) * num ) ) + ( num * num ) ) <= result )
                return false;

            return true;
        }

        /// <summary>
        /// Checks whether the current BoundingSphere intersects another BoundingSphere.
        /// </summary>
        /// <param name="sphere">The BoundingSphere to check for intersection with.</param>
        /// <param name="result">[OutAttribute] true if the BoundingSphere instances intersect; false otherwise.</param>
        public void Intersects( ref BoundingSphere sphere, out bool result )
        {
            float num3;
            Vector3.DistanceSquared( ref Center, ref sphere.Center, out num3 );

            float num = sphere.Radius;
            result = ( ( ( Radius * Radius ) + ( ( 2f * Radius ) * num ) ) + ( num * num ) ) > num3;
        }

        /// <summary>
        /// Checks whether the current BoundingSphere contains the specified BoundingBox.
        /// </summary>
        /// <param name="box">The BoundingBox to check against the current BoundingSphere.</param>
        public ContainmentType Contains( BoundingBox box )
        {
            if ( !box.Intersects( this ) )
                return ContainmentType.Disjoint;

            Vector3 vector;
            float num = Radius * Radius;

            vector.X = Center.X - box.Min.X;
            vector.Y = Center.Y - box.Max.Y;
            vector.Z = Center.Z - box.Max.Z;
            if ( vector.LengthSquared() > num )
                return ContainmentType.Intersects;

            vector.X = Center.X - box.Max.X;
            vector.Y = Center.Y - box.Max.Y;
            vector.Z = Center.Z - box.Max.Z;
            if ( vector.LengthSquared() > num )
                return ContainmentType.Intersects;

            vector.X = Center.X - box.Max.X;
            vector.Y = Center.Y - box.Min.Y;
            vector.Z = Center.Z - box.Max.Z;
            if ( vector.LengthSquared() > num )
                return ContainmentType.Intersects;

            vector.X = Center.X - box.Min.X;
            vector.Y = Center.Y - box.Min.Y;
            vector.Z = Center.Z - box.Max.Z;
            if ( vector.LengthSquared() > num )
                return ContainmentType.Intersects;

            vector.X = Center.X - box.Min.X;
            vector.Y = Center.Y - box.Max.Y;
            vector.Z = Center.Z - box.Min.Z;
            if ( vector.LengthSquared() > num )
                return ContainmentType.Intersects;

            vector.X = Center.X - box.Max.X;
            vector.Y = Center.Y - box.Max.Y;
            vector.Z = Center.Z - box.Min.Z;
            if ( vector.LengthSquared() > num )
                return ContainmentType.Intersects;

            vector.X = Center.X - box.Max.X;
            vector.Y = Center.Y - box.Min.Y;
            vector.Z = Center.Z - box.Min.Z;
            if ( vector.LengthSquared() > num )
                return ContainmentType.Intersects;

            vector.X = Center.X - box.Min.X;
            vector.Y = Center.Y - box.Min.Y;
            vector.Z = Center.Z - box.Min.Z;
            if ( vector.LengthSquared() > num )
                return ContainmentType.Intersects;

            return ContainmentType.Contains;
        }

        /// <summary>
        /// Checks whether the current BoundingSphere contains the specified BoundingBox.
        /// </summary>
        /// <param name="box">The BoundingBox to test for overlap.</param>
        /// <param name="result">[OutAttribute] Enumeration indicating the extent of overlap.</param>
        public void Contains( ref BoundingBox box, out ContainmentType result )
        {
            bool flag;
            box.Intersects( ref this, out flag );

            if ( !flag )
                result = ContainmentType.Disjoint;
            else
            {
                Vector3 vector;
                float num = Radius * Radius;

                result = ContainmentType.Intersects;

                vector.X = Center.X - box.Min.X;
                vector.Y = Center.Y - box.Max.Y;
                vector.Z = Center.Z - box.Max.Z;
                if ( vector.LengthSquared() <= num )
                {

                    vector.X = Center.X - box.Max.X;
                    vector.Y = Center.Y - box.Max.Y;
                    vector.Z = Center.Z - box.Max.Z;
                    if ( vector.LengthSquared() <= num )
                    {

                        vector.X = Center.X - box.Max.X;
                        vector.Y = Center.Y - box.Min.Y;
                        vector.Z = Center.Z - box.Max.Z;
                        if ( vector.LengthSquared() <= num )
                        {

                            vector.X = Center.X - box.Min.X;
                            vector.Y = Center.Y - box.Min.Y;
                            vector.Z = Center.Z - box.Max.Z;
                            if ( vector.LengthSquared() <= num )
                            {

                                vector.X = Center.X - box.Min.X;
                                vector.Y = Center.Y - box.Max.Y;
                                vector.Z = Center.Z - box.Min.Z;
                                if ( vector.LengthSquared() <= num )
                                {

                                    vector.X = Center.X - box.Max.X;
                                    vector.Y = Center.Y - box.Max.Y;
                                    vector.Z = Center.Z - box.Min.Z;
                                    if ( vector.LengthSquared() <= num )
                                    {

                                        vector.X = Center.X - box.Max.X;
                                        vector.Y = Center.Y - box.Min.Y;
                                        vector.Z = Center.Z - box.Min.Z;
                                        if ( vector.LengthSquared() <= num )
                                        {

                                            vector.X = Center.X - box.Min.X;
                                            vector.Y = Center.Y - box.Min.Y;
                                            vector.Z = Center.Z - box.Min.Z;
                                            if ( vector.LengthSquared() <= num )
                                                result = ContainmentType.Contains;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Checks whether the current BoundingSphere contains the specified BoundingFrustum.
        /// </summary>
        /// <param name="frustum">The BoundingFrustum to check against the current BoundingSphere.</param>
        public ContainmentType Contains( BoundingFrustum frustum )
        {
            if ( null == frustum )
                throw new ArgumentNullException( "frustum", "FrameworkResources.NullNotAllowed" );

            if ( !frustum.Intersects( this ) )
                return ContainmentType.Disjoint;

            float num = Radius * Radius;
            foreach ( Vector3 vector2 in frustum.cornerArray )
            {
                Vector3 vector = new Vector3 { X = vector2.X - Center.X, Y = vector2.Y - Center.Y, Z = vector2.Z - Center.Z };

                if ( vector.LengthSquared() > num )
                    return ContainmentType.Intersects;
            }

            return ContainmentType.Contains;
        }

        /// <summary>
        /// Checks whether the current BoundingSphere contains the specified point.
        /// </summary>
        /// <param name="point">The point to check against the current BoundingSphere.</param>
        public ContainmentType Contains( Vector3 point )
        {
            if ( Vector3.DistanceSquared( point, Center ) >= ( Radius * Radius ) )
                return ContainmentType.Disjoint;

            return ContainmentType.Contains;
        }

        /// <summary>
        /// Checks whether the current BoundingSphere contains the specified point.
        /// </summary>
        /// <param name="point">The point to test for overlap.</param>
        /// <param name="result">[OutAttribute] Enumeration indicating the extent of overlap.</param>
        public void Contains( ref Vector3 point, out ContainmentType result )
        {
            float num;
            Vector3.DistanceSquared( ref point, ref Center, out num );

            result = ( num < ( Radius * Radius ) ) ? ContainmentType.Contains : ContainmentType.Disjoint;
        }

        /// <summary>
        /// Checks whether the current BoundingSphere contains the specified BoundingSphere.
        /// </summary>
        /// <param name="sphere">The BoundingSphere to check against the current BoundingSphere.</param>
        public ContainmentType Contains( BoundingSphere sphere )
        {
            float result;
            Vector3.Distance( ref Center, ref sphere.Center, out result );

            float radius = Radius;
            float num = sphere.Radius;

            if ( ( radius + num ) < result )
                return ContainmentType.Disjoint;

            if ( ( radius - num ) < result )
                return ContainmentType.Intersects;

            return ContainmentType.Contains;
        }

        /// <summary>
        /// Checks whether the current BoundingSphere contains the specified BoundingSphere.
        /// </summary>
        /// <param name="sphere">The BoundingSphere to test for overlap.</param>
        /// <param name="result">[OutAttribute] Enumeration indicating the extent of overlap.</param>
        public void Contains( ref BoundingSphere sphere, out ContainmentType result )
        {
            float num3;
            Vector3.Distance( ref Center, ref sphere.Center, out num3 );

            float num = sphere.Radius;
            result = ( ( Radius + num ) >= num3 ) ? ( ( ( Radius - num ) >= num3 ) ? ContainmentType.Contains : ContainmentType.Intersects ) : ContainmentType.Disjoint;
        }

        internal void SupportMapping( ref Vector3 v, out Vector3 result )
        {
            float num = Radius / v.Length();

            result.X = Center.X + ( v.X * num );
            result.Y = Center.Y + ( v.Y * num );
            result.Z = Center.Z + ( v.Z * num );
        }

        /// <summary>
        /// Translates and scales the BoundingSphere using a given Matrix.
        /// </summary>
        /// <param name="matrix">A transformation matrix that might include translation, rotation, or uniform scaling. Note that BoundingSphere.Transform will not return correct results if there are non-uniform scaling, shears, or other unusual transforms in this transformation matrix. This is because there is no way to shear or non-uniformly scale a sphere. Such an operation would cause the sphere to lose its shape as a sphere.</param>
        public BoundingSphere Transform( Matrix matrix )
        {
            BoundingSphere sphere = new BoundingSphere
            {
                Center = Vector3.Transform( Center, matrix )
            };

            float num = ( ( matrix.M11 * matrix.M11 ) + ( matrix.M12 * matrix.M12 ) ) + ( matrix.M13 * matrix.M13 );
            float num2 = ( ( matrix.M21 * matrix.M21 ) + ( matrix.M22 * matrix.M22 ) ) + ( matrix.M23 * matrix.M23 );
            float num3 = ( ( matrix.M31 * matrix.M31 ) + ( matrix.M32 * matrix.M32 ) ) + ( matrix.M33 * matrix.M33 );

            sphere.Radius = Radius * ( (float)Math.Sqrt( (double)Math.Max( num, Math.Max( num2, num3 ) ) ) );

            return sphere;
        }

        /// <summary>
        /// Translates and scales the BoundingSphere using a given Matrix.
        /// </summary>
        /// <param name="matrix">A transformation matrix that might include translation, rotation, or uniform scaling. Note that BoundingSphere.Transform will not return correct results if there are non-uniform scaling, shears, or other unusual transforms in this transformation matrix. This is because there is no way to shear or non-uniformly scale a sphere. Such an operation would cause the sphere to lose its shape as a sphere.</param>
        /// <param name="result">[OutAttribute] The transformed BoundingSphere.</param>
        public void Transform( ref Matrix matrix, out BoundingSphere result )
        {
            result.Center = Vector3.Transform( Center, matrix );

            float num = ( ( matrix.M11 * matrix.M11 ) + ( matrix.M12 * matrix.M12 ) ) + ( matrix.M13 * matrix.M13 );
            float num2 = ( ( matrix.M21 * matrix.M21 ) + ( matrix.M22 * matrix.M22 ) ) + ( matrix.M23 * matrix.M23 );
            float num3 = ( ( matrix.M31 * matrix.M31 ) + ( matrix.M32 * matrix.M32 ) ) + ( matrix.M33 * matrix.M33 );

            result.Radius = Radius * ( (float)Math.Sqrt( (double)Math.Max( num, Math.Max( num2, num3 ) ) ) );
        }

        #endregion

        #region zh-CHS 共有静态方法 | en Public Static Methods

        /// <summary>
        /// Creates a BoundingSphere that contains the two specified BoundingSphere instances.
        /// </summary>
        /// <param name="original">BoundingSphere to be merged.</param>
        /// <param name="additional">BoundingSphere to be merged.</param>
        public static BoundingSphere CreateMerged( BoundingSphere original, BoundingSphere additional )
        {
            Vector3 vector;
            Vector3.Subtract( ref additional.Center, ref original.Center, out vector );

            float length = vector.Length();
            float radius = original.Radius;
            float radius2 = additional.Radius;

            if ( ( radius + radius2 ) >= length )
            {
                if ( ( radius - radius2 ) >= length )
                    return original;

                if ( ( radius2 - radius ) >= length )
                    return additional;
            }

            float numMin = MathHelper.Min( -radius, length - radius2 );
            float numMax = ( MathHelper.Max( radius, length + radius2 ) - numMin ) * 0.5f;

            return new BoundingSphere
            {
                Center = original.Center + ( (Vector3)( (Vector3)( vector * ( 1f / length ) ) * ( numMax + numMin ) ) ),
                Radius = numMax
            };
        }

        /// <summary>
        /// Creates a BoundingSphere that contains the two specified BoundingSphere instances.
        /// </summary>
        /// <param name="original">BoundingSphere to be merged.</param>
        /// <param name="additional">BoundingSphere to be merged.</param>
        /// <param name="result">[OutAttribute] The created BoundingSphere.</param>
        public static void CreateMerged( ref BoundingSphere original, ref BoundingSphere additional, out BoundingSphere result )
        {
            Vector3 vector;
            Vector3.Subtract( ref additional.Center, ref original.Center, out vector );

            float length = vector.Length();
            float radius = original.Radius;
            float radius2 = additional.Radius;

            if ( ( radius + radius2 ) >= length )
            {
                if ( ( radius - radius2 ) >= length )
                {
                    result = original;
                    return;
                }

                if ( ( radius2 - radius ) >= length )
                {
                    result = additional;
                    return;
                }
            }

            float numMin = MathHelper.Min( -radius, length - radius2 );
            float numMax = ( MathHelper.Max( radius, length + radius2 ) - numMin ) * 0.5f;

            result.Center = original.Center + ( (Vector3)( (Vector3)( vector * ( 1f / length ) ) * ( numMax + numMin ) ) );
            result.Radius = numMax;
        }

        /// <summary>
        /// Creates the smallest BoundingSphere that can contain a specified BoundingBox.
        /// </summary>
        /// <param name="box">The BoundingBox to create the BoundingSphere from.</param>
        public static BoundingSphere CreateFromBoundingBox( BoundingBox box )
        {
            BoundingSphere sphere;
            Vector3.Lerp( ref box.Min, ref box.Max, 0.5f, out sphere.Center );

            float num;
            Vector3.Distance( ref box.Min, ref box.Max, out num );

            sphere.Radius = num * 0.5f;
            return sphere;
        }

        /// <summary>
        /// Creates the smallest BoundingSphere that can contain a specified BoundingBox.
        /// </summary>
        /// <param name="box">The BoundingBox to create the BoundingSphere from.</param>
        /// <param name="result">[OutAttribute] The created BoundingSphere.</param>
        public static void CreateFromBoundingBox( ref BoundingBox box, out BoundingSphere result )
        {
            Vector3.Lerp( ref box.Min, ref box.Max, 0.5f, out result.Center );

            float num;
            Vector3.Distance( ref box.Min, ref box.Max, out num );

            result.Radius = num * 0.5f;
        }

        /// <summary>
        /// Creates a BoundingSphere that can contain a specified list of points.
        /// </summary>
        /// <param name="points">List of points the BoundingSphere must contain.</param>
        public static BoundingSphere CreateFromPoints( IEnumerable<Vector3> points )
        {
            if ( points == null )
                throw new ArgumentNullException( "points" );

            IEnumerator<Vector3> enumerator = points.GetEnumerator();
            if ( !enumerator.MoveNext() )
                throw new ArgumentException( "FrameworkResources.BoundingSphereZeroPoints" );

            Vector3 vector5;
            Vector3 vector6;
            Vector3 vector7;
            Vector3 vector8;
            Vector3 vector9;
            Vector3 vector4 = vector5 = vector6 = vector7 = vector8 = vector9 = enumerator.Current;

            foreach ( Vector3 vector in points )
            {
                if ( vector.X < vector4.X )
                    vector4 = vector;

                if ( vector.X > vector5.X )
                    vector5 = vector;

                if ( vector.Y < vector6.Y )
                    vector6 = vector;

                if ( vector.Y > vector7.Y )
                    vector7 = vector;

                if ( vector.Z < vector8.Z )
                    vector8 = vector;

                if ( vector.Z > vector9.Z )
                    vector9 = vector;
            }

            float numDistance1;
            Vector3.Distance( ref vector5, ref vector4, out numDistance1 );

            float numDistance2;
            Vector3.Distance( ref vector7, ref vector6, out numDistance2 );

            float numDistance3;
            Vector3.Distance( ref vector9, ref vector8, out numDistance3 );

            float numDistance;
            Vector3 vectorResult;
            if ( numDistance1 > numDistance2 )
            {
                if ( numDistance1 > numDistance3 )
                {
                    Vector3.Lerp( ref vector5, ref vector4, 0.5f, out vectorResult );
                    numDistance = numDistance1 * 0.5f;
                }
                else
                {
                    Vector3.Lerp( ref vector9, ref vector8, 0.5f, out vectorResult );
                    numDistance = numDistance3 * 0.5f;
                }
            }
            else if ( numDistance2 > numDistance3 )
            {
                Vector3.Lerp( ref vector7, ref vector6, 0.5f, out vectorResult );
                numDistance = numDistance2 * 0.5f;
            }
            else
            {
                Vector3.Lerp( ref vector9, ref vector8, 0.5f, out vectorResult );
                numDistance = numDistance3 * 0.5f;
            }

            foreach ( Vector3 vector in points )
            {
                Vector3 vectorNew = new Vector3 { X = vector.X - vectorResult.X, Y = vector.Y - vectorResult.Y, Z = vector.Z - vectorResult.Z };

                float length = vectorNew.Length();
                if ( length > numDistance )
                {
                    numDistance = ( numDistance + length ) * 0.5f;
                    vectorResult += (Vector3)( ( 1f - ( numDistance / length ) ) * vectorNew );
                }
            }

            return new BoundingSphere { Center = vectorResult, Radius = numDistance };
        }

        /// <summary>
        /// Creates the smallest BoundingSphere that can contain a specified BoundingFrustum.
        /// </summary>
        /// <param name="frustum">The BoundingFrustum to create the BoundingSphere with.</param>
        public static BoundingSphere CreateFromFrustum( BoundingFrustum frustum )
        {
            if ( frustum == null )
                throw new ArgumentNullException( "frustum" );

            return CreateFromPoints( frustum.cornerArray );
        }

        #endregion

        #region zh-CHS 共有静态事件 | en Public Static Event

        /// <summary>
        /// Determines whether two instances of BoundingSphere are equal.
        /// </summary>
        /// <param name="a">The object to the left of the equality operator.</param>
        /// <param name="b">The object to the right of the equality operator.</param>
        public static bool operator ==( BoundingSphere a, BoundingSphere b )
        {
            return a.Equals( b );
        }

        /// <summary>
        /// Determines whether two instances of BoundingSphere are not equal.
        /// </summary>
        /// <param name="a">The BoundingSphere to the left of the inequality operator.</param>
        /// <param name="b">The BoundingSphere to the right of the inequality operator.</param>
        public static bool operator !=( BoundingSphere a, BoundingSphere b )
        {
            if ( a.Center == b.Center )
                return a.Radius == b.Radius;

            return false;
        }

        #endregion

        #region zh-CHS 方法覆盖 | en Override Methods

        /// <summary>
        /// Determines whether the specified Object is equal to the BoundingSphere.
        /// </summary>
        /// <param name="obj">The Object to compare with the current BoundingSphere.</param>
        public override bool Equals( object obj )
        {
            if ( obj is BoundingSphere )
                return Equals( (BoundingSphere)obj );
            else
                return false;
        }

        /// <summary>
        /// Gets the hash code for this instance.
        /// </summary>
        public override int GetHashCode()
        {
            return ( Center.GetHashCode() + Radius.GetHashCode() );
        }

        /// <summary>
        /// Returns a String that represents the current BoundingSphere.
        /// </summary>
        public override string ToString()
        {
            return string.Format( CultureInfo.CurrentCulture, "{{Center:{0} Radius:{1}}}", new object[] { Center.ToString(), Radius.ToString( CultureInfo.CurrentCulture ) } );
        }

        #endregion

        #region zh-CHS 接口实现 | en Interface Implementation

        /// <summary>
        /// Determines whether the specified BoundingSphere is equal to the current BoundingSphere.
        /// </summary>
        /// <param name="other">The BoundingSphere to compare with the current BoundingSphere.</param>
        public bool Equals( BoundingSphere other )
        {
            return ( ( Center == other.Center ) && ( Radius == other.Radius ) );
        }

        #endregion

    }
}
#endregion