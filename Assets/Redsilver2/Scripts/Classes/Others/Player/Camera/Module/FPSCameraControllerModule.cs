using RedSilver2.Framework.Inputs.Configurations;
using RedSilver2.Framework.Inputs.Settings;
using System.Threading.Tasks;
using UnityEngine;

namespace RedSilver2.Framework.Player
{
    public class FPSCameraControllerModule : CameraControllerModule
    {
        [SerializeField] private float minRotationX = -45f;
        [SerializeField] private float maxRotationX = 45f;

        public float MinRotationX => minRotationX;
        public float MaxRotationX => maxRotationX;

        protected override async Awaitable<CameraController> GetCameraController(MouseVector2InputSettings settings)
        {
            if      (settings == null)   return null;
            else if (Controller != null) return Controller;

            return new FPSCameraController(await settings.GetConfiguration(), this, transform.root, transform);
        }
    }
}
