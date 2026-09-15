using UnityEditor.Rendering;
using UnityEngine;

namespace RedSilver2.Framework.Player
{
    public class TargetFollow2DCameraController : TargetFollowCameraController
    {
        public TargetFollow2DCameraController() : base() { }
        protected sealed override void LateUpdate(Camera camera, Transform target)
        {
            UpdateCameraRotation(camera, Vector3.zero);

            UpdateCameraPosition(camera, target != null ?
                                           Vector2.right * target.transform.position.x
                                         + Vector2.up * target.transform.position.y :
                                           Vector2.zero);
        }
    }
}