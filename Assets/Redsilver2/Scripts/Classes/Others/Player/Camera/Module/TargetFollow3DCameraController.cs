using UnityEngine;

namespace RedSilver2.Framework.Player
{
    [System.Serializable]
    public class TargetFollow3DCameraController : TargetFollowCameraController
    {
        [SerializeField] private Vector2 positionOffset;

        public TargetFollow3DCameraController() : base()
        {
        }

        protected sealed override void LateUpdate(Camera camera, Transform target)
        {
            if(camera != null) {
                Vector3 position = Vector3.zero;
                camera.transform.localRotation = Quaternion.Slerp(camera.transform.localRotation, Quaternion.identity, Time.deltaTime);


                if (target == null) {
                    position = (Vector2.right * (target.position.x - positionOffset.x) +
                                Vector2.up * (target.position.y - positionOffset.y));
                }

                camera.transform.position = Vector3.Lerp(camera.transform.position, position, Time.deltaTime);

            }
        }
    }
}
