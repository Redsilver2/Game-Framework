using UnityEngine;

namespace RedSilver2.Framework.Player
{
    [System.Serializable]
    public sealed partial class TopDown2DCameraController : TargetFollowCameraController
    {
        [SerializeField, HideInInspector] private float fovHeight;

        public TopDown2DCameraController() : base()
        {

        }

        public void SetHeight(float height) { this.fovHeight = height; }

        protected sealed override void LateUpdate(Camera camera, Transform target)
        {
            if (camera != null)
            {
                camera.transform.localRotation = Quaternion.Slerp(camera.transform.localRotation, Quaternion.identity, Time.deltaTime);
                camera.fieldOfView = Mathf.Lerp(camera.fieldOfView, fovHeight, Time.deltaTime);

                if (target != null) {
                    camera.transform.position = Vector3.right   * target.position.x +
                                                Vector3.up      * target.position.y +
                                                Vector3.forward * target.position.z;
                }
            }
        }
    }

    public sealed partial class TopDown2DCameraController : TargetFollowCameraController
    {
#if UNITY_EDITOR
        protected override void ShowBaseSettings(Color foldoutColor, Color buttonColor, Color backgroundColor)
        {
            base.ShowBaseSettings(foldoutColor, buttonColor, backgroundColor);
          
            EditorExtension.Space(10f);
            fovHeight = EditorExtension.DisplayFloatSlider("Field Of View Height", fovHeight, 0f, 100f);
        }
#endif
    }
}
