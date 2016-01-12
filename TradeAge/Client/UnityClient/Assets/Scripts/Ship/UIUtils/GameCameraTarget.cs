using UnityEngine;

namespace Assets.Scripts.Ship.UIUtils
{
    [AddComponentMenu("Game/Camera Target")]
    public class GameCameraTarget : MonoBehaviour
    {
        public void Activate(bool val)
        {
            if (val) GameCamera.AddTarget(transform);
            else GameCamera.RemoveTarget(transform);
        }

        private void OnEnable()
        {
            Activate(true);
        }

        private void OnDisable()
        {
            Activate(false);
        }
    }
}