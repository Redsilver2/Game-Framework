using RedSilver2.Framework.Inputs.Settings;
using RedSilver2.Framework.StateMachines.Controllers;
using UnityEngine;

namespace RedSilver2.Framework.StateMachines.States
{
    public sealed class PlayerJumpState : JumpState
    {
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
