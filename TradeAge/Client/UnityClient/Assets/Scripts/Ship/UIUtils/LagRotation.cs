using UnityEngine;

namespace Assets.Scripts.Ship.UIUtils
{
    [AddComponentMenu("Exploration/Lag Rotation")]
    public class LagRotation : MonoBehaviour
    {
        public float speed = 10f;

        private Transform mTrans;
        private Transform mParent;
        private Quaternion mRelative;
        private Quaternion mParentRot;

        private void Start()
        {
            mTrans = transform;
            mParent = mTrans.parent;
            mRelative = mTrans.localRotation;
            mParentRot = mParent.rotation;
        }

        private void LateUpdate()
        {
            mParentRot = Quaternion.Slerp(mParentRot, mParent.rotation, Time.deltaTime*speed);
            mTrans.rotation = mParentRot*mRelative;
        }
    }
}