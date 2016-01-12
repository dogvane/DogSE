using UnityEngine;

public class Interpolation
{
	// Basic linear interpolation for two floating point values
	static public float Linear(float v0, float v1, float factor)
	{
		return v0 * (1.0f - factor) + v1 * factor;
	}

	// Basic linear interpolation for a pair of 2D vector
	static public Vector2 Linear(Vector2 v0, Vector2 v1, float factor)
	{
		return v0 * (1.0f - factor) + v1 * factor;
	}

	// Basic linear interpolation for a pair of 3D vectors
	static public Vector3 Linear(Vector3 v0, Vector3 v1, float factor)
	{
		return v0 * (1.0f - factor) + v1 * factor;
	}

	// Quaternion version of linear interpolation actually uses SLERP
	static public Quaternion Linear(Quaternion q0, Quaternion q1, float factor)
	{
		return Quaternion.Slerp(q0, q1, factor);
	}

	// Color version of linear interpolation
	static public Color Linear(Color c0, Color c1, float factor)
	{
		return c0 * (1.0f - factor) + c1 * factor;
	}

	// Cosine interpolation for two floating point values
	static public float Cosine(float v1, float v2, float factor)
	{
		factor = (1.0f - Mathf.Cos(factor * Mathf.PI)) * 0.5f;
		return (v1 * (1.0f - factor) + v2 * factor);
	}

	// Cosine interpolation for two vectors
	static public Vector3 Cosine(Vector3 v1, Vector3 v2, float factor)
	{
		factor = (1.0f - Mathf.Cos(factor * Mathf.PI)) * 0.5f;
		return (v1 * (1.0f - factor) + v2 * factor);
	}

	// Cosine interpolation for two quaternions
	static public Quaternion Cosine(Quaternion q1, Quaternion q2, float factor)
	{
		factor = (1.0f - Mathf.Cos(factor * Mathf.PI)) * 0.5f;
		return Quaternion.Slerp(q1, q2, factor);
	}

	// Hermite 2-value interpolation (ease in, ease out)
	static public float Hermite(float start, float end, float factor)
	{
		factor = factor * factor * (3.0f - 2.0f * factor);
		return Mathf.Lerp(start, end, factor);
	}

	// Hermite 2-vector interpolation (ease in, ease out)
	static public Vector3 Hermite(Vector3 v0, Vector3 v1, float factor)
	{
		factor = factor * factor * (3.0f - 2.0f * factor);
		return v0 * (1.0f - factor) + v1 * factor;
	}

	// Hermite 2-color interpolation (ease in, ease out)
	static public Color Hermite(Color v0, Color v1, float factor)
	{
		factor = factor * factor * (3.0f - 2.0f * factor);
		return v0 * (1.0f - factor) + v1 * factor;
	}

	// Hermite 2-quaternion interpolation (ease in, ease out)
	static public Quaternion Hermite(Quaternion q1, Quaternion q2, float factor)
	{
		factor = factor * factor * (3.0f - 2.0f * factor);
		return Quaternion.Slerp(q1, q2, factor);
	}

	// Hermite spline tangent for a float-based spline
	static public float GetHermiteTangent(float distance0, float distance1, float duration0, float duration1)
	{
		return (distance0 / duration0 + distance1 / duration1) * 0.5f;
	}

	// Calculates a hermite spline tangent at the given point using 3 vectors and two duration factors
	static public Vector3 GetHermiteTangent(Vector3 distance0, Vector3 distance1, float duration0, float duration1)
	{
		return distance0 * (0.5f / duration0) + distance1 * (0.5f / duration1);
	}

	// Hermite spline interpolation with the provided tangents
	static public float Hermite(float pos0, float pos1, float tan0, float tan1, float factor, float duration)
	{
		float factor2 = factor * factor;
		float factor3 = factor2 * factor;

		return	 pos0 * (2.0f * factor3 - 3.0f * factor2 + 1.0f) +
				 pos1 * (3.0f * factor2 - 2.0f * factor3) +
				(tan0 * (factor3 - 2.0f * factor2 + factor) +
				 tan1 * (factor3 - factor2)) * duration;
	}

	// Hermite spline interpolation with the provided tangents
	static public Vector3 Hermite(Vector3 pos0, Vector3 pos1, Vector3 tan0, Vector3 tan1, float factor, float duration)
	{
		float factor2 = factor * factor;
		float factor3 = factor2 * factor;

		return	 pos0 * (2.0f * factor3 - 3.0f * factor2 + 1.0f) +
				 pos1 * (3.0f * factor2 - 2.0f * factor3) +
				(tan0 * (factor3 - 2.0f * factor2 + factor) +
				 tan1 * (factor3 - factor2)) * duration;
	}

	// Optimized function for cases where val0 = 0, and val1 = 1
	static public float Hermite(float tan0, float tan1, float factor, float duration)
	{
		float factor2 = factor * factor;
		float factor3 = factor2 * factor;

		return (3.0f * factor2 - 2.0f * factor3) +
			(tan0 * (factor3 - 2.0f * factor2 + factor) +
			 tan1 * (factor3 - factor2)) * duration;
	}

	// Spherical linear interpolation that does not choose the shortest path (for SQUAD)
	private static Quaternion SlerpNoInvert (Quaternion from, Quaternion to, float factor)
	{
		float dot = Quaternion.Dot(from, to);

		if (Mathf.Abs(dot) > 0.999999f)
		{
			return Quaternion.Lerp(from, to, factor);
		}
		
		float theta		= Mathf.Acos( Mathf.Clamp(dot, -1.0f, 1.0f) );
		float sinInv	= 1.0f / Mathf.Sin(theta);
		float first		= Mathf.Sin((1.0f - factor) * theta) * sinInv;
		float second	= Mathf.Sin(factor * theta) * sinInv;

		return new Quaternion(	first * from.x + second * to.x,
								first * from.y + second * to.y,
								first * from.z + second * to.z,
								first * from.w + second * to.w );
	}

	// Quaternion Logarithm
	private static Quaternion Log(Quaternion q)
	{
		float a = Mathf.Acos( Mathf.Clamp(q.w, -1.0f, 1.0f) ), s = Mathf.Sin(a);

		if (Mathf.Abs(s) < 0.000001f)
		{
			return new Quaternion( 0.0f, 0.0f, 0.0f, 0.0f );
		}
		a /= s;
		return new Quaternion(q.x * a,  q.y * a,  q.z * a, 0.0f);
	}

	// Quaternion Exponent
	private static Quaternion Exp(Quaternion q)
	{
		float a = Mathf.Sqrt(q.x * q.x + q.y * q.y + q.z * q.z),
		      s = Mathf.Sin(a),
		      c = Mathf.Cos(a);

		if (Mathf.Abs(a) < 0.000001f)
		{
			return new Quaternion(0.0f, 0.0f, 0.0f, c);
		}
		s /= a;
		return new Quaternion(q.x * s, q.y * s, q.z * s, c);
	}

	// Finds the control quaternion for the specified quaternion to be used in SQUAD
	static public Quaternion GetSquadControlRotation(Quaternion past, Quaternion current, Quaternion future)
	{
		Quaternion inv = new Quaternion( -current.x, -current.y, -current.z, current.w );
		Quaternion q0  = Log(inv * past);
		Quaternion q1  = Log(inv * future);
		Quaternion sum = new Quaternion(
			-0.25f * (q0.x + q1.x),
			-0.25f * (q0.y + q1.y),
			-0.25f * (q0.z + q1.z),
			-0.25f * (q0.w + q1.w));
		return current * Exp(sum);
	}

	// Spherical Cubic interpolation -- ctrlFrom and ctrlTo are control quaternions
	static public Quaternion Squad(Quaternion from, Quaternion to, Quaternion ctrlFrom, Quaternion ctrlTo, float factor)
	{
		return SlerpNoInvert( SlerpNoInvert(from, to, factor),
							  SlerpNoInvert(ctrlFrom, ctrlTo, factor),
							  factor * 2.0f * (1.0f - factor) );
	}
}