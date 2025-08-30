using UnityEngine;

namespace RedSilver2.Framework.Player
{
    public class ClampedPlayerCameraController : PlayerCameraController
    {
        private float minRotationX;
        private float maxRotationX;

        private ClampedPlayerCameraController()
        {
        }

        public ClampedPlayerCameraController(Transform body, Transform head, float minRotationX, float maxRotationX) : base(body, head)
        {
            this.minRotationX = minRotationX;
            this.maxRotationX = maxRotationX;
        }

        protected override void OnInputUpdate(Vector2 input)
        {
            base.OnInputUpdate(input);
            rotationClampX = Mathf.Clamp(rotationClampX, minRotationX, maxRotationX);
        }
    }
}
