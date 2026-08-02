using RedSilver2.Framework.StateMachines.Controllers;
using System;
using System.Collections;
using System.Linq;
using UnityEngine;

namespace RedSilver2.Framework.StateMachines.States {

    [RequireComponent(typeof(FallState))]
    public abstract class JumpState : MovementState {

        [Space]
        [SerializeField] private float defaultJumpForce;
        private float jumpForce;    
        private bool isJumping;
        public bool IsJumping => isJumping;
        public const MovementStateType TYPE = MovementStateType.Jump;

#if UNITY_EDITOR
        protected override void OnValidate()
        {
            base.OnValidate();
            jumpForce           = Mathf.Clamp(jumpForce, 0f, float.MaxValue);

        }
#endif

        protected sealed override MovementStateType[] GetDefaultInvalidTypes()
        {
            var results = Enum.GetValues(typeof(MovementStateType)) as MovementStateType[];
            return results == null ? new MovementStateType[0] : results;
        }

        protected sealed override MovementStateType[] GetRequiredTypes() {
            return new MovementStateType[] { FallState.TYPE };
        }

        protected override void Awake()
        {
            base.Awake();
            ResetJumpForce();
        }

        protected override void OnEntered(MovementStateMachine stateMachine) {
            base.OnEntered(stateMachine);
            stateMachine?.SetFallSpeed(jumpForce);
        }


        protected override void OnExited(MovementStateMachine stateMachine)
        {
            base.OnExited(stateMachine);
            isJumping      = false;
        }

        protected override void OnDisabled(MovementStateMachine stateMachine)
        {
            base.OnDisabled(stateMachine);
            isJumping = false;
        }

        protected void SetIsJumping(bool isJumping) { this.isJumping = isJumping;  }
        protected sealed override void SetMovementStateType(ref MovementStateType type) { type = TYPE; }

        public sealed override bool CanTransition(MovementStateMachine stateMachine) {
            return base.CanTransition(stateMachine) && IsStateMachineJumping(stateMachine);
        }

        protected sealed override void OnUpdate(MovementStateMachine stateMachine) {
             stateMachine?.Move(Time.deltaTime * Vector3.up * jumpForce);
        }

        public void SetJumpForce(float jumpForce) {
            this.jumpForce = Mathf.Clamp(jumpForce, 0f, float.MaxValue);
        }

        public void ResetJumpForce() { SetJumpForce(defaultJumpForce); }

        public static JumpState GetState(MovementStateMachine stateMachine) {
            if(stateMachine == null) return null;
            return stateMachine.GetState(TYPE) as JumpState;
        }

        public static bool IsStateMachineJumping(MovementStateMachine stateMachine) {
            JumpState state = GetState(stateMachine);
            return state != null ? state.IsJumping : false;
        }
    }
}
