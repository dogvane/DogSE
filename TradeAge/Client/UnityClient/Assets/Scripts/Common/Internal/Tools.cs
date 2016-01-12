using UnityEngine;
using System;
using System.Collections.Generic;

static public class Tools
{
	/// <summary>
	/// Wraps the angle, ensuring that it's always in the -360 to 360 range.
	/// </summary>

	static public float WrapAngle (float a)
	{
		while (a < -180.0f) a += 360.0f;
		while (a > 180.0f) a -= 360.0f;
		return a;
	}

	/// <summary>
	/// Finds the rigidbody responsible for the specified transform.
	/// </summary>
	
	static public Rigidbody GetRigidbody (Transform trans)
	{
		Rigidbody rb = null;
		
		while (rb == null && trans != null)
		{
			rb = trans.rigidbody;
			trans = trans.parent;
		}
		return rb;
	}

	/// <summary>
	/// Converts a list of colliders into a list or rigidbodies.
	/// </summary>

	static public List<Rigidbody> GetRigidbodies (Collider[] cols)
	{
		List<Rigidbody> list = new List<Rigidbody>();

		foreach (Collider c in cols)
		{
			if (c != null)
			{
				Rigidbody rb = GetRigidbody(c.transform);
				if (rb != null && !list.Contains(rb)) list.Add(rb);
			}
		}
		return list;
	}
	
	/// <summary>
	/// Calculates the world bounds of the specified object.
	/// </summary>
	
	static public Bounds CalculateWorldBounds (Transform transform)
	{
		Collider collider = transform.collider;
		Bounds bounds = (collider != null) ? collider.bounds : new Bounds(transform.position, Vector3.zero);
		Collider[] colliders = transform.GetComponentsInChildren<Collider>();
		foreach (Collider col in colliders) bounds.Encapsulate(col.bounds);
		return bounds;
	}
	
	/// <summary>
	/// Calculates the local bounds of the specified object.
	/// </summary>
	
	static public Bounds CalculateLocalBounds (Transform transform)
	{
		Bounds bounds = CalculateWorldBounds(transform);
		return new Bounds(bounds.min - transform.position, bounds.max - transform.position);
	}
	
	/// <summary>
	/// Calculates a ray-cast distance to the specified plane.
	/// </summary>
	
	static public float DistanceToPlane (Vector3 planeOrigin, Vector3 planeNormal, Vector3 rayOrigin, Vector3 rayNormal)
	{
	    float dist  = Vector3.Dot(planeOrigin, planeNormal);
	    float a = Vector3.Dot(planeNormal, rayOrigin);
	    float b = Vector3.Dot(planeNormal, rayNormal);
	    return -((a + dist) / b);
	}
	
	/// <summary>
	/// Helper function that encodes a floating-point value into an integer.
	/// This is done this way because -1 is 0xFFFFFFFF in bits, which can't be bitwise-OR'd with anything afterwards.
	/// Encoding -1 as 0x11, with the least-important bit containing the sign, on the other hand, works perfectly fine.
	/// </summary>
	
	static public int EncodeFloatToInt (float val)
	{
		int a = Mathf.RoundToInt(val);
		
		if (a < 0)
		{
			a = -a;
			a <<= 1;
			a |= 1;
		}
		else
		{
			a <<= 1;
		}
		return a;
	}
	
	/// <summary>
	/// Encode the specified position into an integer ID that can be quickly matched later.
	/// </summary>
	
	static public int GetPositionID (Vector3 pos)
	{
		return (EncodeFloatToInt(pos.x) << 16) | EncodeFloatToInt(pos.z);
	}

	/// <summary>
	/// Determines whether the 'parent' contains a 'child' in its hierarchy.
	/// </summary>

	static public bool IsChild (Transform parent, Transform child)
	{
		if (parent == null || child == null) return false;

		while (child != null)
		{
			if (child == parent) return true;
			child = child.parent;
		}
		return false;
	}

	/// <summary>
	/// Returns the hierarchy of the object in a human-readable format.
	/// </summary>
	
	static public string GetHierarchy (GameObject obj)
	{
	    string path = "/" + obj.name;

	    while (obj.transform.parent != null)
	    {
	        obj = obj.transform.parent.gameObject;
	        path = "/" + obj.name + path;
	    }
	    return path;
	}
	
	/// <summary>
	/// Helper function that determines whether the specified path is a web address.
	/// </summary>
	
	static public bool IsWebAddress (string s)
	{
		return 	s.StartsWith("http:", StringComparison.OrdinalIgnoreCase) ||
				s.StartsWith("file:", StringComparison.OrdinalIgnoreCase);
	}
	
	/// <summary>
	/// Helper function that figures out what the full path supposed to be, given a partial one.
	/// </summary>
	
	static public string GetFullPath (string s)
	{
		string path = Application.dataPath;
		if (!path.EndsWith("/")) path += "/";
		path += s;
		path = path.Replace("\\", "/");

		for (;;)
		{
			int idx = path.IndexOf("/../");
			if (idx == -1) break;
			string left = path.Remove(idx);
			string right = path.Substring(idx + 4);
			idx = left.LastIndexOf('/');
			if (idx == -1) break;
			left = left.Remove(idx);
			path = left + "/" + right;
		}
		return path;
	}
	
	/// <summary>
	/// Helper function that applies a power-based ramp to the specified value.
	/// </summary>
	
	static public float Ramp (float val, float factor)
	{
		float sign = Mathf.Sign(val);
		val *= sign;
		return sign * Mathf.Lerp(val, val * val, factor);
	}

	/// <summary>
	/// Check if the two specified values differ by more than the specified amount.
	/// </summary>

	static public bool Deviates (float a, float b, float units)
	{
		return Mathf.Abs(a - b) > units;
	}

	/// <summary>
	/// Quickly checks if the two specified vector values deviate by more than the specified amount.
	/// </summary>

	static public bool Deviates (Vector3 a, Vector3 b, float units)
	{
		Vector3 diff = a - b;
		return	Mathf.Abs(diff.x) > units ||
				Mathf.Abs(diff.y) > units ||
				Mathf.Abs(diff.z) > units;
	}

	/// <summary>
	/// Call the specified function on all objects in the scene.
	/// </summary>

	static public void Broadcast (string funcName)
	{
		GameObject[] gos = GameObject.FindObjectsOfType(typeof(GameObject)) as GameObject[];
		foreach (GameObject go in gos) go.SendMessage(funcName, SendMessageOptions.DontRequireReceiver);
	}
}