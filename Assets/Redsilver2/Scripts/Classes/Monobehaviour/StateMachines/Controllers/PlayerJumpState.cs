using RedSilver2.Framework.Inputs.Settings;
using RedSilver2.Framework.StateMachines.Controllers;
using UnityEngine;
using UnityEngine.Events;

namespace RedSilver2.Framework.StateMachines.States
{
    public sealed class PlayerJumpState : JumpState {
        [Space]
        [SerializeField] private PressInputSettings inputSetting;

        protected override void OnEnabled(MovementStateMachine stateMachine) {
            if (stateMachine == null || stateMachine.ContainsState(this)) return;
            stateMachine?.AddOnUpdateListener(OnUpdateJumpInput(stateMachine));

            inputSetting?.Enable();
            base.OnEnabled(stateMachine);
        }

        protected override void OnDisabled(MovementStateMachine stateMachine)
        {
            inputSetting?.Disable();
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

        public static PlayerJumpState GetState(PlayerMovementStateMachine stateMachine) {
            return GetState(stateMachine as MovementStateMachine) as PlayerJumpState;
        }
    }
}
