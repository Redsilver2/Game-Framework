using RedSilver2.Framework.Inputs.Configurations;
using UnityEngine;

namespace RedSilver2.Framework.Player
{
    public class FPSCameraController : CameraController
    {
        private readonly FPSCameraControllerModule module;

        public FPSCameraController(MouseVector2InputConfiguration configuration, FPSCameraControllerModule module, Transform body, Transform head) : base(configuration, body, head)
        {
            this.module = module;
        }

        protected sealed override void OnUpdate(Vector2 input)
        {
            base.OnUpdate(input);

            if (module != null)
                rotationClampX = Mathf.Clamp(rotationClampX, module.MinRotationX, module.MaxRotationX);
        }
    }
}
