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
    /// Contains commonly used precalculated values.
    /// </summary>
    public static class MathHelper
    {

        #region zh-CHS 共有常量 | en Public Constants

        /// <summary>
        /// Represents the mathematical constant e.
        /// </summary>
        public const float E = 2.718282f;
        /// <summary>
        /// Represents the log base ten of e.
        /// </summary>
        public const float Log10E = 0.4342945f;
        /// <summary>
        /// Represents the log base two of e.
        /// </summary>
        public const float Log2E = 1.442695f;
        /// <summary>
        /// Represents the value of pi.
        /// </summary>
        public const float Pi = 3.141593f;
        /// <summary>
        /// Represents the value of pi divided by two.
        /// </summary>
        public const float PiOver2 = 1.570796f;
        /// <summary>
        /// Represents the value of pi divided by four.
        /// </summary>
        public const float PiOver4 = 0.7853982f;
        /// <summary>
        /// Represents the value of pi times two.
        /// </summary>
        public const float TwoPi = 6.283185f;

        #endregion

        #region zh-CHS 共有静态方法 | en Public Static Methods

        /// <summary>
        /// Returns the Cartesian coordinate for one axis of a point that is defined by a given triangle and two normalized barycentric (areal) coordinates.
        /// </summary>
        /// <param name="value1">The coordinate on one axis of vertex 1 of the defining triangle.</param>
        /// <param name="value2">The coordinate on the same axis of vertex 2 of the defining triangle.</param>
        /// <param name="value3">The coordinate on the same axis of vertex 3 of the defining triangle.</param>
        /// <param name="amount1">The normalized barycentric (areal) coordinate b2, equal to the weighting factor for vertex 2, the coordinate of which is specified in value2.</param>
        /// <param name="amount2">The normalized barycentric (areal) coordinate b3, equal to the weighting factor for vertex 3, the coordinate of which is specified in value3.</param>
        public static float Barycentric( float value1, float value2, float value3, float amount1, float amount2 )
        {
            return ( ( value1 + ( amount1 * ( value2 - value1 ) ) ) + ( amount2 * ( value3 - value1 ) ) );
        }

        /// <summary>
        /// Performs a Catmull-Rom interpolation using the specified positions.
        /// </summary>
        /// <param name="value1">The first position in the interpolation.</param>
        /// <param name="value2">The second position in the interpolation.</param>
        /// <param name="value3">The third position in the interpolation.</param>
        /// <param name="value4">The fourth position in the interpolation.</param>
        /// <param name="amount">Weighting factor.</param>
        public static float CatmullRom( float value1, float value2, float value3, float value4, float amount )
        {
            float amountDouble = amount * amount;
            float amountTriple = amount * amountDouble;

            return ( 0.5f * ( ( ( ( 2f * value2 ) + ( ( -value1 + value3 ) * amount ) ) + ( ( ( ( ( 2f * value1 ) - ( 5f * value2 ) ) + ( 4f * value3 ) ) - value4 ) * amountDouble ) ) + ( ( ( ( -value1 + ( 3f * value2 ) ) - ( 3f * value3 ) ) + value4 ) * amountTriple ) ) );
        }

        /// <summary>
        /// Restricts a value to be within a specified range. Reference page contains links to related code samples.
        /// </summary>
        /// <param name="value">The value to clamp.</param>
        /// <param name="min">The minimum value. If value is less than min, min will be returned.</param>
        /// <param name="max">The maximum value. If value is greater than max, max will be returned.</param>
        public static float Clamp( float value, float min, float max )
        {
            value = ( value > max ) ? max : value;
            value = ( value < min ) ? min : value;

            return value;
        }

        /// <summary>
        /// Calculates the absolute value of the difference of two values.
        /// </summary>
        /// <param name="value1">Source value.</param>
        /// <param name="value2">Source value.</param>
        public static float Distance( float value1, float value2 )
        {
            return Math.Abs( value1 - value2 );
        }

        /// <summary>
        /// Performs a Hermite spline interpolation.
        /// </summary>
        /// <param name="value1">Source position.</param>
        /// <param name="tangent1">Source tangent.</param>
        /// <param name="value2">Source position.</param>
        /// <param name="tangent2">Source tangent.</param>
        /// <param name="amount">Weighting factor.</param>
        public static float Hermite( float value1, float tangent1, float value2, float tangent2, float amount )
        {
            float amountDouble = amount * amount;
            float amountTriple = amount * amountDouble;

            float result1 = ( ( 2f * amountTriple ) - ( 3f * amountDouble ) ) + 1f;
            float result2 = ( -2f * amountTriple ) + ( 3f * amountDouble );
            float result3 = ( amountTriple - ( 2f * amountDouble ) ) + amount;
            float result4 = amountTriple - amountDouble;

            return ( ( ( ( value1 * result1 ) + ( value2 * result2 ) ) + ( tangent1 * result3 ) ) + ( tangent2 * result4 ) );
        }

        /// <summary>
        /// Linearly interpolates between two values.
        /// </summary>
        /// <param name="value1">Source value.</param>
        /// <param name="value2">Source value.</param>
        /// <param name="amount">Value between 0 and 1 indicating the weight of value2.</param>
        public static float Lerp( float value1, float value2, float amount )
        {
            return ( value1 + ( ( value2 - value1 ) * amount ) );
        }

        /// <summary>
        /// Returns the greater of two values.
        /// </summary>
        /// <param name="value1">Source value.</param>
        /// <param name="value2">Source value.</param>
        public static float Max( float value1, float value2 )
        {
            return Math.Max( value1, value2 );
        }

        /// <summary>
        /// Returns the lesser of two values.
        /// </summary>
        /// <param name="value1">Source value.</param>
        /// <param name="value2">Source value.</param>
        public static float Min( float value1, float value2 )
        {
            return Math.Min( value1, value2 );
        }

        /// <summary>
        /// Interpolates between two values using a cubic equation.
        /// </summary>
        /// <param name="value1">Source value.</param>
        /// <param name="value2">Source value.</param>
        /// <param name="amount">Weighting value.</param>
        public static float SmoothStep( float value1, float value2, float amount )
        {
            float num = Clamp( amount, 0f, 1f );

            return Lerp( value1, value2, ( num * num ) * ( 3f - ( 2f * num ) ) );
        }

        /// <summary>
        /// Converts radians to degrees.
        /// </summary>
        /// <param name="radians">The angle in radians.</param>
        public static float ToDegrees( float radians )
        {
            return ( radians * 57.29578f );
        }

        /// <summary>
        /// Converts degrees to radians.
        /// </summary>
        /// <param name="degrees">The angle in degrees.</param>
        public static float ToRadians( float degrees )
        {
            return ( degrees * 0.01745329f );
        }

        /// <summary>
        /// Reduces a given angle to a value between π and -π.
        /// </summary>
        /// <param name="angle">The angle to reduce, in radians.</param>
        public static float WrapAngle( float angle )
        {
            angle = (float)Math.IEEERemainder( angle, 6.2831854820251465 );

            if ( angle <= -3.141593f )
                angle += 6.283185f;
            else if ( angle > 3.141593f )
                angle -= 6.283185f;

            return angle;
        }

        #endregion

    }
}
#endregion
