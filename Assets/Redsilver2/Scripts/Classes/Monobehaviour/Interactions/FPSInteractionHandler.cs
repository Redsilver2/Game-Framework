using UnityEngine;

namespace RedSilver2.Framework.Interactions
{
    [System.Serializable]
    public partial class FPSInteractionHandler : InteractionHandler
    {
        [SerializeField, HideInInspector] private Camera camera;

        public FPSInteractionHandler() : base() {

        }

        public void SetCamera(Camera camera) {
            this.camera = camera;
        }
        
        protected sealed override Collider GetCollider(float interactionRange) {
            return GetCollider(interactionRange, camera);
        }

        protected virtual Collider GetCollider(float interactionRange, Camera camera)
        {
            Transform transform;
            if (camera == null) return null;

            transform = camera.transform;
            Debug.DrawRay(transform.position, transform.forward, Color.blue);

            Physics.Raycast(transform.position, transform.forward, out RaycastHit hitInfo, interactionRange, ~GameManager.PlayerLayer);
            return hitInfo.collider;
        }
    }

    public partial class FPSInteractionHandler : InteractionHandler
    {
#if UNITY_EDITOR
        protected override void DrawBaseSettings(Color foldoutColor, Color backgroundColor, Color buttonColor)
        {
            base.DrawBaseSettings(foldoutColor, backgroundColor, buttonColor);

            EditorExtension.Space(10f);
            camera = EditorExtension.DisplayCustomField("Camera ", true, camera);
        }
#endif
    }
}
