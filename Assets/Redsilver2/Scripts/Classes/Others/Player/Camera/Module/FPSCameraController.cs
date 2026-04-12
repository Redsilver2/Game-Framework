using UnityEngine;

namespace RedSilver2.Framework.Player
{
    public class FPSCameraController : CameraController
    {
        [Space]
        [SerializeField] private float minRotationX = -45f;
        [SerializeField] private float maxRotationX = 45f;

        protected sealed override void UpdateHeadRotation(Transform head, ref float rotation)
        {
            base.UpdateHeadRotation(head, ref rotation);
            rotation = Mathf.Clamp(rotation, minRotationX, maxRotationX);
        }
    }
}
