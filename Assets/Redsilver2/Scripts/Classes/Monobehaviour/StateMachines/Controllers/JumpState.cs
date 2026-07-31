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
        [SerializeField] private float defaultJumpDuration;

        [Space]
        [SerializeField] private float jumpForce;
        [SerializeField] private float jumpDuration;
       

        private bool isJumping;
        private IEnumerator jumpUpdater;

        public bool IsJumping => isJumping;
        public const MovementStateType TYPE = MovementStateType.Jump;

#if UNITY_EDITOR
        protected override void OnValidate()
        {
            base.OnValidate();
            jumpForce           = Mathf.Clamp(jumpForce, 0f, float.MaxValue);
            defaultJumpForce    = Mathf.Clamp(defaultJumpForce, 0f, float.MaxValue);
            defaultJumpDuration = Mathf.Clamp(defaultJumpDuration, 0f, float.MaxValue);
        }

        protected sealed override MovementStateType[] GetDefaultInvalidTypes()
        {
            var results = Enum.GetValues(typeof(MovementStateType)) as MovementStateType[];
            return results == null ? new MovementStateType[0] : results;
        }

        protected sealed override MovementStateType[] GetRequiredTypes() {
            return new MovementStateType[] { FallState.TYPE };
        }

#endif

        protected override void Awake()
        {
            base.Awake();

            ResetJumpForce();
            ResetJumpDuration();
        }

        protected override void OnEntered(MovementStateMachine stateMachine) {
            Cancel();
            jumpUpdater = UpdateJump(stateMachine);
            StartCoroutine(jumpUpdater);
        }

        public void Cancel()
        {
            if (jumpUpdater != null) StopCoroutine(jumpUpdater);
            jumpUpdater = null;
        }

        protected override void OnExited()
        {
            base.OnExited();
            isJumping      = false;
        }

        protected override void OnDisabled()
        {
            base.OnDisabled();
            isJumping = false;
        }

        private IEnumerator UpdateJump(MovementStateMachine stateMachine) {
            float t = 0f;

            while(t < jumpDuration) {
                stateMachine?.Move(Time.deltaTime * (Vector3.up * jumpForce));
                t += Time.deltaTime;
                yield return null;
            }
        }

        protected void SetIsJumping(bool isJumping) { this.isJumping = isJumping;  }
        protected sealed override void SetMovementStateType(ref MovementStateType type) { type = TYPE; }

        protected override bool CanTransition(MovementStateMachine stateMachine) {
            return base.CanTransition(stateMachine) && IsStateMachineJumping(stateMachine);
        }

        protected sealed override void OnUpdate(MovementStateMachine stateMachine) {
             stateMachine?.Move(Time.deltaTime * Vector3.up * jumpForce);
        }

        public void SetJumpForce(float jumpForce) {
            this.jumpForce = Mathf.Clamp(jumpForce, 0f, float.MaxValue);
        }

        public void SetJumpDuration(float jumpDuration)
        {
            this.jumpDuration = Mathf.Clamp(jumpDuration, 0f, float.MaxValue);
        }

        public void ResetJumpForce() { SetJumpForce(defaultJumpForce); }
        public void ResetJumpDuration() { SetJumpDuration(defaultJumpDuration); }

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
