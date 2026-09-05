using UnityEngine;

namespace RedSilver2.Framework.Player
{
    [System.Serializable]
    public sealed class TopDown2DCameraController : TargetFollowCameraController
    {
        [SerializeField, HideInInspector] private float height;

        public TopDown2DCameraController() : base() {

        }

        public void SetHeight(float height) { this.height = height;  }

        protected sealed override void LateUpdate(Camera camera, Transform target)
        {
            if (camera != null) {
                camera.transform.localRotation = Quaternion.Slerp(camera.transform.localRotation, Quaternion.identity, Time.deltaTime);

                if(target != null) {
                    camera.transform.position = Vector3.right * target.transform.position.x +
                             Vector3.up * height +
                             Vector3.forward * target.transform.position.z;
                }                    
            }
        }
    }
}
