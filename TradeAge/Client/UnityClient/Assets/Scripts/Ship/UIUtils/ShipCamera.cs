using Assets.Scripts.Ship;
using UnityEngine;

namespace Assets.Scripts.Ship.UIUtils
{
    [AddComponentMenu("Exploration/Ship Camera")]
    public class ShipCamera : MonoBehaviour
    {
        public ShipController control;
        public AnimationCurve distance;
        public AnimationCurve angle;

        private Transform mTrans;

        private void Start()
        {
            mTrans = transform;
        }

        private void Update()
        {
            if (control != null)
            {
                float speed = control.Speed;
                Quaternion rot = Quaternion.Euler(angle.Evaluate(speed), 0f, 0f);
                mTrans.localPosition = rot*Vector3.back*distance.Evaluate(speed);
                mTrans.localRotation = rot;
            }
        }
    }
}