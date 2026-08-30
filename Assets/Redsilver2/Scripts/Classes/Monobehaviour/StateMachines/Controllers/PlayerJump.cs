using RedSilver2.Framework.Inputs.Settings;
using UnityEngine;

namespace RedSilver2.Framework.StateMachines.States
{
    [System.Serializable]
    public sealed class PlayerJump : PlayerMovement {
        [Space]
        [SerializeField] private PressInputSettings inputSetting;

        public JumpState BaseState => GetState() as JumpState;

        public PlayerJump() : base() { }


        private void Update()
        {
            inputSetting?.Enable();
            Update(BaseState);
        }

        private void Update(JumpState baseState)
        {
            bool isJumping = false;
            baseState?.Update();

            if (baseState == null) return;

            if (stateMachine != null && inputSetting != null && baseState.CurrentJumpDelay == 0f && baseState.IsEnabled) {
                if (inputSetting.GetValue()) {
                    if (baseState.JumpCount == 0 && stateMachine.IsGrounded) { isJumping = true; }
                    else if (baseState.JumpCount > 0 && baseState.JumpCount < baseState.MaxJumpCount) {
                        isJumping = true;
                        stateMachine?.ChangeState(JumpState.TYPE, false);
                    }
                }
            }

            baseState?.SetIsJumping(isJumping);
        }

        protected override void InitializeEvent(MovementState state, MovementStateMachine stateMachine)
        {
            base.InitializeEvent(state, stateMachine);
            stateMachine?.AddOnUpdateListener(Update);
        }

        protected override void UnInitializeEvent(MovementState state, MovementStateMachine stateMachine)
        {
            base.UnInitializeEvent(state, stateMachine);
            stateMachine?.RemoveOnUpdateListener(Update);
        }


        protected override void SetBaseState(ref MovementState state)
        {
            state = new JumpState();
        }
    }
}
