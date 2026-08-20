using RedSilver2.Framework.StateMachines.Controllers;
using RedSilver2.Framework.StateMachines.States;
using UnityEngine;

namespace RedSilver2.Framework.StateMachines.Extensions
{
    [System.Serializable]
    public abstract class PlayerMovementStateMotion : PlayerMovementStateEvent {
        [Space]
        [SerializeField] private Transform transform;

        protected PlayerMovementStateMotion(MovementStateType Type) : base(Type) { }
        public void SetTransform(Transform transform) { this.transform = transform; }


        protected sealed override void OnStateUpdate(PlayerMovementStateMachine stateMachine) {
            OnStateUpdate(transform, stateMachine != null ? stateMachine.MoveInput : Vector2.zero, stateMachine != null ? stateMachine.Is2DMovement : false);
        }

        protected sealed override void OnStateLateUpdate() {
            OnStateLateUpdate(transform);
        }

        protected abstract void OnStateUpdate(Transform transform, Vector2 input, bool is2DMovement);
        protected abstract void OnStateLateUpdate(Transform transform); 

    }
}
