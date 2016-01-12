using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
[AddComponentMenu("Game/Reposition Water")]
public class RepositionWater : MonoBehaviour
{
	Transform mTrans;
	Transform mCamTrans;

	void Start ()
	{
		mTrans = transform;
	    if (Camera.main != null)
	        mCamTrans = Camera.main.transform;
	    else
	    {
	        var c = FindObjectOfType<Camera>();
	        if (c != null)
	            mCamTrans = c.transform;
	    }
	}

	void LateUpdate()
	{
		if (mCamTrans != null)
		{
			Vector3 pos = mCamTrans.position;
			pos.y = 0.0f;

			if (mTrans.position != pos)
			{
				mTrans.rotation = Quaternion.identity;
				mTrans.position = pos;
			}
		}
	}
}