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
    }
}