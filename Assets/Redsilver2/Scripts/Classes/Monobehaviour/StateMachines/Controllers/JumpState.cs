using System;
using UnityEngine;

namespace RedSilver2.Framework.StateMachines.States {

    [System.Serializable]
    public sealed class JumpState : MovementState {
       
        [Space]
        [SerializeField] private float jumpForce;

        [Space]
        [SerializeField] private uint maxJumpCount;

        [Space]
        [SerializeField] private float maxJumpDelay; 

        private uint  currentJumpCount; 
        private float currentJumpDelay;
        
        private bool isJumping;


        public  bool IsJumping => isJumping;
        public  float JumpForce => jumpForce;

        public uint   MaxJumpCount => maxJumpCount;
        public uint   JumpCount    => currentJumpCount;

        public float MaxJumpDelay     => maxJumpDelay;
        public float CurrentJumpDelay => currentJumpDelay;


        public  const MovementStateType TYPE = MovementStateType.Jump;

        public JumpState() : base()
        {
            currentJumpCount = 0;
            currentJumpDelay = 0f;
            isJumping = false;
        }

#if UNITY_EDITOR
        protected override void Validate() {
            base.Validate();
            jumpForce    = Mathf.Clamp(jumpForce, 0f, float.MaxValue);
            maxJumpCount = (uint)Mathf.Clamp(maxJumpCount, 1, uint.MaxValue); 
            maxJumpDelay = Mathf.Clamp(maxJumpDelay, 0f, float.MaxValue);       
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

        protected override void OnEntered() {
            MovementStateMachine?.ResetAirbornTime();         
            base.OnEntered();
         
            if(MovementStateMachine != null) {
                if (MovementStateMachine.FallSpeed < 0f) MovementStateMachine?.SetFallSpeed(jumpForce);
                else MovementStateMachine?.SetFallSpeed(MovementStateMachine.FallSpeed + jumpForce);
            }

            currentJumpCount++;
            currentJumpDelay = maxJumpDelay;
        }


        protected override void OnExited()
        {
            base.OnExited();
            isJumping = false;
        }

        protected override void OnEnabled()
        {
            base.OnEnabled();
            Debug.Log("?!");
        }

        protected override void OnDisabled() {
            isJumping = false;
            base.OnDisabled();
        }

        public void Update() { currentJumpDelay = Mathf.Clamp(currentJumpDelay - Time.deltaTime, 0f, maxJumpDelay); }
       
        public void ResetJumpCount() { currentJumpCount = 0; }
        public void SetIsJumping(bool isJumping) { this.isJumping = isJumping;  }
        protected sealed override void SetMovementStateType(ref MovementStateType type) { type = TYPE; }

        public sealed override bool CanTransition() {
            return base.CanTransition() && IsStateMachineJumping(MovementStateMachine) &&
                   currentJumpDelay <= 0f && currentJumpCount == 0;
        }

        protected sealed override void OnUpdate() {
            base.OnUpdate();
            MovementStateMachine?.Move(Time.deltaTime * Vector3.up * jumpForce);
        }

        public void SetJumpForce(float jumpForce) {
            this.jumpForce = Mathf.Clamp(jumpForce, 0f, float.MaxValue);
        }

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
