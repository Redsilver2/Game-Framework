using RedSilver2.Framework.StateMachines.Events;
using System;
using UnityEngine;

namespace RedSilver2.Framework.StateMachines.States {

    [System.Serializable]
    public abstract class JumpState : MovementState {
       
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

        private const string SOUND_EVENT = "Jump Sound";
        public  const MovementStateType TYPE = MovementStateType.Jump;

        public JumpState(MovementStateMachine stateMachine) : base(stateMachine)
        {
            currentJumpCount = 0;
            currentJumpDelay = 0f;
            isJumping = false;
        }

#if UNITY_EDITOR
        public override void Validate() {
            base.Validate();
            jumpForce    = Mathf.Clamp(jumpForce, 0f, float.MaxValue);
            maxJumpCount = (uint)Mathf.Clamp(maxJumpCount, 1, uint.MaxValue); 
          
            maxJumpDelay = Mathf.Clamp(maxJumpDelay, 0f, float.MaxValue);  
            if(!ContainsEvent(SOUND_EVENT)) AddEvent(new JumpSound(SOUND_EVENT, this));
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
            MovementStateMachine movementStateMachine = GetMovementStateMachine(this);
            movementStateMachine?.ResetAirbornTime();      
            
            base.OnEntered();
         
            if(movementStateMachine != null) {
                if (movementStateMachine.FallSpeed < 0f) movementStateMachine?.SetFallSpeed(jumpForce);
                else movementStateMachine?.SetFallSpeed(movementStateMachine.FallSpeed + jumpForce);
            }

            currentJumpCount++;
            currentJumpDelay = maxJumpDelay;
        }


        protected override void OnExited()
        {
            base.OnExited();
            isJumping = false;
        }

        protected override void OnAdded()
        {
            base.OnAdded();
            GetMovementStateMachine(this)?.AddOnUpdateListener(Update);
        }

        protected override void OnRemoved()
        {
            base.OnRemoved();
            GetMovementStateMachine(this)?.RemoveOnUpdateListener(Update);
            isJumping = false;
        }

        protected override void OnDisabled() {
            isJumping = false;
            base.OnDisabled();
        }

        protected virtual void Update() { currentJumpDelay = Mathf.Clamp(currentJumpDelay - Time.deltaTime, 0f, maxJumpDelay); }
       
        public void ResetJumpCount() { currentJumpCount = 0; }
        public void SetIsJumping(bool isJumping) { this.isJumping = isJumping;  }
     
        public void SetMaxJumpCount(uint maxJumpCount) { this.maxJumpCount = maxJumpCount; }
        public void SetMaxJumpDelay(float maxJumpDelay) { this.maxJumpDelay = maxJumpDelay;  }


        protected sealed override void SetMovementStateType(ref MovementStateType type) { type = TYPE; }

        public sealed override bool CanTransition() {
            return base.CanTransition() && GetIsJumping(GetMovementStateMachine(this)) &&
                   currentJumpDelay <= 0f && currentJumpCount == 0;
        }

        protected sealed override void OnUpdate() {
            base.OnUpdate();
            GetMovementStateMachine(this)?.Move(Time.deltaTime * Vector3.up * jumpForce);
        }

        public void SetJumpForce(float jumpForce) {
            this.jumpForce = Mathf.Clamp(jumpForce, 0f, float.MaxValue);
        }

        public static JumpState GetState(MovementStateMachine stateMachine) {
            if(stateMachine == null) return null;
            return stateMachine.GetState(TYPE) as JumpState;
        }

        public static bool GetIsJumping(MovementStateMachine stateMachine) {
            JumpState state = GetState(stateMachine);
            return state != null ? state.IsJumping : false;
        }
    }
}
