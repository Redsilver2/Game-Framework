using UnityEngine;

namespace RedSilver2.Framework.Player
{
    public partial class TPSCameraController : FPSCameraController
    {
        [SerializeField, HideInInspector] private Vector3 cameraPosition;
        [SerializeField, HideInInspector] private Vector3 cameraRotation;

        protected sealed override void UpdateCameraTransform(Camera camera, float updateSpeed)
        {
            if (camera != null) {
                camera.transform.localPosition = Vector3.Lerp(camera.transform.localPosition, cameraPosition, Time.deltaTime * updateSpeed);
                camera.transform.localRotation = Quaternion.Slerp(camera.transform.localRotation, Quaternion.Euler(cameraRotation), Time.deltaTime * updateSpeed);
            }
        }
    }

    public partial class TPSCameraController : FPSCameraController
    {
#if UNITY_EDITOR
        protected override void ShowBaseSettings(Color foldoutColor, Color buttonColor, Color backgroundColor)
        {
            base.ShowBaseSettings(foldoutColor, buttonColor, backgroundColor);

            EditorExtension.Space(10f);
            cameraPosition = EditorExtension.DisplayVector3Field("Camera Position", cameraPosition);
            cameraRotation = EditorExtension.DisplayVector3Field("Camera Rotation", cameraRotation);
        }
#endif
    }
}
