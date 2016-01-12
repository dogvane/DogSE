using UnityEngine;
using System.Collections.Generic;
using UnityEditor;

namespace Assets.Scripts.Ship.UIUtils
{
    [AddComponentMenu("Exploration/Active Camera")]
    public class GameCamera : MonoBehaviour
    {
        private static List<Transform> mTargets = new List<Transform>();
        private static float mAlpha;
        private static GameCamera mInstance = null;
        public static Vector3 direction = Vector3.forward;
        public static Vector3 flatDirection = Vector3.forward;

        public float interpolationTime = 0.25f;

        private Transform mTrans;
        private Vector3 mPos;
        private Quaternion mRot;

        /// <summary>
        /// Target the camera is following.
        /// </summary>

        public static Transform target
        {
            get { return (mTargets.Count == 0) ? null : mTargets[mTargets.Count - 1]; }
            set
            {
                mTargets.Clear();
                if (value != null) mTargets.Add(value);
                mAlpha = 0f;
            }
        }

        /// <summary>
        /// Add a new target to the top of the list.
        /// </summary>

        public static void AddTarget(Transform t)
        {
            if (t != null)
            {
                mTargets.Remove(t);
                mTargets.Add(t);
                mAlpha = 0f;
            }
        }

        /// <summary>
        /// Remove the specified target from the list.
        /// </summary>

        public static void RemoveTarget(Transform t)
        {
            if (t != null)
            {
                if (target == t) mAlpha = 0f;
                mTargets.Remove(t);
            }
        }

        /// <summary>
        /// Detach the camera from the current parent.
        /// </summary>

        public static void DetachFromParent()
        {
            if (mInstance != null && mInstance.mTrans.parent != null)
            {
                mInstance.mTrans.parent = null;
            }
        }

        /// <summary>
        /// Detach the camera from the specified parent.
        /// </summary>

        public static bool DetachFromParent(Transform t)
        {
            if (mInstance != null && Tools.IsChild(t, mInstance.mTrans))
            {
                mInstance.mTrans.parent = null;
                return true;
            }
            return false;
        }

        /// <summary>
        /// Keep a singleton reference.
        /// </summary>

        private void Awake()
        {
            mInstance = this;
        }

        private void OnDestroy()
        {
            mInstance = null;
        }

        /// <summary>
        /// Cache the transform.
        /// </summary>

        private void Start()
        {
            mTrans = transform;
        }

        /// <summary>
        /// Interpolate the position.
        /// </summary>

        private void LateUpdate()
        {
            Transform t = target;

            if (t == null)
            {
                mTrans.parent = null;
            }
            else if (mAlpha < 1f)
            {
                // Start of the interpolation process -- record the position and rotation
                if (mAlpha == 0f)
                {
                    mTrans.parent = null;
                    mPos = mTrans.position;
                    mRot = mTrans.rotation;
                }

                // Advance the alpha
                if (interpolationTime > 0f) mAlpha += Time.deltaTime/interpolationTime;
                else mAlpha = 1f;

                if (mAlpha < 1f)
                {
                    // Interpolation process continues
                    mTrans.position = Vector3.Lerp(mPos, t.position, mAlpha);
                    mTrans.rotation = Quaternion.Slerp(mRot, t.rotation, mAlpha);
                }
                else
                {
                    // Interpolation finished -- parent the camera to the target and assume its orientation
                    mTrans.parent = t;
                    mTrans.position = t.position;
                    mTrans.rotation = t.rotation;
                }
            }

            // Update the directional and flat directional vectors
            direction = mTrans.rotation*Vector3.forward;
            flatDirection = direction;
            flatDirection.y = 0f;
            flatDirection.Normalize();
        }
    }
}