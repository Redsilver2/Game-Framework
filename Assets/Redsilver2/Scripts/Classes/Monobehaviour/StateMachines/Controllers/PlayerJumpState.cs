using RedSilver2.Framework.Inputs.Settings;
using RedSilver2.Framework.StateMachines.Controllers;
using UnityEngine;

namespace RedSilver2.Framework.StateMachines.States
{
    [System.Serializable]
    public sealed class PlayerJumpState : JumpState {
        [Space]
        [SerializeField] private PressInputSettings inputSetting;

        public PlayerJumpState() : base() {
        
        }


#if UNITY_EDITOR
        public void Validate(PlayerMovementStateMachine stateMachine) { 
            SetStateMachine(stateMachine);
            Validate();
        }
#endif

        private void SetStateMachine(PlayerMovementStateMachine stateMachine)
        {
            this.MovementStateMachine = stateMachine;
            SetStateMachine(stateMachine as UpdatableStateMachine);
        }

        protected override void MovementStateMachineUpdate()
        {
            inputSetting?.Enable();
            base.MovementStateMachineUpdate();

            if (MovementStateMachine == null || inputSetting == null || CurrentJumpDelay > 0f || !IsEnabled) SetIsJumping(false);
            else if (inputSetting.GetValue()) {
               if (JumpCount == 0 && MovementStateMachine.IsGrounded) { SetIsJumping(true); }
               else if (JumpCount > 0 && JumpCount < MaxJumpCount)    { MovementStateMachine?.ChangeState(TYPE, false); }
               else { SetIsJumping(false); }
            }
            else { SetIsJumping(false); }
        }

        public static PlayerJumpState GetState(PlayerMovementStateMachine stateMachine) {
            return GetState(stateMachine as MovementStateMachine) as PlayerJumpState;
        }
    }
}
