using UnityEngine;

namespace RedSilver2.Framework.Player
{
    [System.Serializable]
    public abstract partial class TargetFollowCameraController : CameraController
    {
        [SerializeField, HideInInspector] private Transform target;

        protected TargetFollowCameraController() : base() 
        {
        }

        public void SetTarget(Transform target) {
            this.target = target;
        }

        protected sealed override void LateUpdate(Camera camera)  {
            LateUpdate(camera, target);
        }

        protected sealed override void Update(Camera camera) {
            if (camera != null) camera.transform.SetParent(null);
        }

        protected abstract void LateUpdate(Camera camera, Transform target);
    }

    public abstract partial class TargetFollowCameraController : CameraController
    {
#if UNITY_EDITOR
        protected override void ShowBaseSettings(Color foldoutColor, Color buttonColor, Color backgroundColor)
        {
            base.ShowBaseSettings(foldoutColor, buttonColor, backgroundColor);
            EditorExtension.Space(10f);
            target = EditorExtension.DisplayCustomField("Target", true, target);

        }
#endif
    }
}
