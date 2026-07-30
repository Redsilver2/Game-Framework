using RedSilver2.Framework.StateMachines.Controllers;
using System;
using System.Linq;
using UnityEngine;

namespace RedSilver2.Framework.StateMachines.States {

    [RequireComponent(typeof(FallState))]
    public abstract class JumpState : MovementState {

        [Space]
        [SerializeField] private float jumpForce;

        public const MovementStateType TYPE = MovementStateType.Jump;

#if UNITY_EDITOR
        protected override void OnValidate()
        {
            base.OnValidate();
            jumpForce = Mathf.Clamp(jumpForce, 0f, float.MaxValue);
        }

        protected sealed override MovementStateType[] GetDefaultInvalidTypes()
        {
            var results = Enum.GetValues(typeof(MovementStateType)) as MovementStateType[];
            if (results == null) return new MovementStateType[0];

            return results.Where(x => x != MovementStateType.Fall).ToArray();
        }

#endif
        protected sealed override void SetMovementStateType(ref MovementStateType type) {
            type = TYPE;
        }

        protected override bool CanTransition(MovementStateMachine stateMachine) {
            if (stateMachine == null || stateMachine.IsCurrentState(TYPE)) return false;
            return stateMachine.IsGrounded;
        }

        protected sealed override void OnUpdate(MovementStateMachine stateMachine) {
            stateMachine?.SetFallSpeed(jumpForce);
        }

        public void SetJumpForce(float jumpForce) {
            this.jumpForce = Mathf.Clamp(jumpForce, 0f, float.MaxValue);
        }

        public static JumpState GetState(MovementStateMachine stateMachine) {
            if(stateMachine == null) return null;
            return stateMachine.GetState(TYPE) as JumpState;
        }
    }
}
