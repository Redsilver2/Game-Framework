using RedSilver2.Framework.StateMachines.Controllers;
using Unity.VisualScripting;
using UnityEngine;

namespace RedSilver2.Framework.StateMachines
{
    [RequireComponent(typeof(CharacterController))]
    public sealed class PlayerCharacterControllerStateMachine : PlayerMovementStateMachine
    {
        private CharacterController controller;

        protected sealed override void Awake() {
            base.Awake();
            controller = gameObject.GetOrAddComponent<CharacterController>();   
        }

        protected sealed override void OnMoved(Vector3 nextPosition)
        {
            controller?.Move(nextPosition);
        }


        public sealed override void SetHeight(float height)
        {
            if (controller != null) {
                controller.height = height;
                controller.center = Vector3.zero + Vector3.up * Mathf.Clamp01(controller.height / DefaultHeight);
            }
        }

        public sealed override void SetHeight(float height, float transitionSpeed)
        {
            if (controller != null) SetHeight(Mathf.Lerp(controller.height, height, Time.deltaTime * transitionSpeed));
        }
    }
}