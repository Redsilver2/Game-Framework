using UnityEngine;

namespace RedSilver2.Framework.Player
{
    [System.Serializable]
    public partial class TargetFollow3DCameraController : TargetFollowCameraController
    {
        [SerializeField, HideInInspector] private Vector3 positionOffset;
        [SerializeField, HideInInspector] private Vector3 rotation;

        public TargetFollow3DCameraController() : base() { }

        protected override void LateUpdate(Camera camera, Transform target)
        {
            UpdateCameraRotation(camera, rotation);

            UpdateCameraPosition(camera, target != null ? target.position + positionOffset : Vector3.zero);
        }
    }

    public partial class TargetFollow3DCameraController : TargetFollowCameraController
    {
#if UNITY_EDITOR
        protected override void ShowBaseSettings(Color foldoutColor, Color buttonColor, Color backgroundColor)
        {
            base.ShowBaseSettings(foldoutColor, buttonColor, backgroundColor);

            EditorExtension.Space(10f);
            positionOffset = EditorExtension.DisplayVector3Field("Position Offset", positionOffset);
            rotation       = EditorExtension.DisplayVector3Field("Rotation", rotation);
        }
#endif
    }
}
