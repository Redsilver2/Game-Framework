using RedSilver2.Framework.Inputs.Settings;
using UnityEngine;

namespace RedSilver2.Framework.Interactions
{
    public class FPSInteractionHandler : InteractionHandler
    {
        private Camera camera;

        protected sealed override void Awake()
        {
            camera = GetComponent<Camera>();
            base.Awake();
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
}
