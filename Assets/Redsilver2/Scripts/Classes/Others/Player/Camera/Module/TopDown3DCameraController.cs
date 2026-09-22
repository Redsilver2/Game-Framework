using UnityEngine;

namespace RedSilver2.Framework.Player
{
    [System.Serializable]
    public partial class TopDown3DCameraController : TargetFollowCameraController
    {
        [SerializeField, HideInInspector] private float height;
        [SerializeField, HideInInspector] private Vector3 desiredRotation;

        public TopDown3DCameraController() : base() { }

        protected sealed override void LateUpdate(Camera camera, Transform target)
        {
            camera.transform.localRotation = Quaternion.Slerp(camera.transform.localRotation, Quaternion.Euler(desiredRotation), Time.deltaTime);


            if (camera != null) {
                if (target != null && camera != null) {
                    Vector3 result = Vector3.right * target.transform.position.x +
                                      Vector3.up * (target.transform.position.y + height) +
                                      Vector3.forward * target.transform.position.z;

                    camera.transform.localPosition = Vector3.Lerp(camera.transform.position, result, Time.deltaTime * 10f);
                }
            }
        }

        public void SetDesiredRotation(Vector3 desiredRotation)
        {
            this.desiredRotation = desiredRotation;
        }
    }

    public partial class TopDown3DCameraController : TargetFollowCameraController
    {
#if UNITY_EDITOR
        protected override void ShowBaseSettings(Color foldoutColor, Color buttonColor, Color backgroundColor)
        {
            base.ShowBaseSettings(foldoutColor, buttonColor, backgroundColor);

            EditorExtension.Space(10f);
            height = EditorExtension.DisplayFloatSlider("Height", height, 0f, 100f);
        }
#endif
    }
}
