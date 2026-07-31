using RedSilver2.Framework.Inputs.Settings;
using RedSilver2.Framework.StateMachines.Controllers;
using UnityEngine;
using UnityEngine.Events;

namespace RedSilver2.Framework.StateMachines.States
{
    public sealed class PlayerJumpState : JumpState {
        [Space]
        [SerializeField] private PressInputSettings inputSetting;

        protected override void OnEnabled() {
            base.OnEnabled();
            inputSetting?.Enable();
        }

        protected override void OnDisabled()
        {
            base.OnDisabled();
            inputSetting?.Disable();
        }

        protected sealed override void OnEnabled(MovementStateMachine stateMachine)
        {
            if (stateMachine == null || stateMachine.ContainsState(this)) return;
            stateMachine?.AddOnUpdateListener(OnUpdateJumpInput(stateMachine));

            base.OnEnabled(stateMachine);
        }

        protected sealed override void OnDisabled(MovementStateMachine stateMachine)
        {
            base.OnDisabled(stateMachine);

            if (stateMachine == null || !stateMachine.ContainsState(this)) return;
            stateMachine?.RemoveOnUpdateListener(OnUpdateJumpInput(stateMachine));  
        }


        private UnityAction OnUpdateJumpInput(MovementStateMachine stateMachine) {
            return () => {
                if (stateMachine == null || inputSetting == null || !stateMachine.IsGrounded) SetIsJumping(false);
                else if (inputSetting.GetValue()) { SetIsJumping(true); }
            };
        }


        public void SetInputSetting(PressInputSettings inputSetting) {
            this.inputSetting = inputSetting;
        }

        protected sealed override bool CanTransition(MovementStateMachine stateMachine) {
            if (inputSetting == null) return false;
            return base.CanTransition(stateMachine) && inputSetting.GetValue();
           
        }

        public static PlayerJumpState GetState(PlayerMovementStateMachine stateMachine) {
            return GetState(stateMachine as MovementStateMachine) as PlayerJumpState;
        }
    }
}
