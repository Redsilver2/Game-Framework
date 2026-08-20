using RedSilver2.Framework.Inputs.Settings;
using RedSilver2.Framework.StateMachines.Controllers;
using UnityEngine;

namespace RedSilver2.Framework.StateMachines.States
{
    [System.Serializable]
    public sealed class PlayerRunState : RunState {
        [Space]
        [SerializeField] private bool hasToHoldInput = true;

        [Space]
        [SerializeField] private PressInputSettings pressInput;
        [SerializeField] private HoldInputSettings holdInput;

        [Space]
        [SerializeField] private HeadbobPositionMotion positionMotion;

        public PressInputSettings PressInput => pressInput;
        public HoldInputSettings  HoldInput  => holdInput;

        public HeadbobPositionMotion PositionMotion => positionMotion;

        public PlayerRunState() : base()  {
            positionMotion = new HeadbobPositionMotion(TYPE, false);
        }


#if UNITY_EDITOR
        public void Validate(PlayerMovementStateMachine stateMachine)
        {
            SetStateMachine(stateMachine);
            Validate();
        }

        protected override void Validate()
        {
            base.Validate();
            positionMotion?.Validate();
        }
#endif

        private void SetStateMachine(PlayerMovementStateMachine stateMachine) {
            this.MovementStateMachine = stateMachine;
            SetStateMachine(stateMachine as UpdatableStateMachine);
        }

        protected override void OnEnabled()
        {
            positionMotion?.Enable();
            MovementStateMachine?.AddOnUpdateListener(OnUpdateRunInput);

            positionMotion?.SetStateMachine(MovementStateMachine as PlayerMovementStateMachine);
            base.OnEnabled();
        }

        protected override void OnDisabled()
        {
            positionMotion?.Disable();
            MovementStateMachine?.RemoveOnUpdateListener(OnUpdateRunInput);

            positionMotion?.SetStateMachine(null);
            base.OnDisabled();
        }


        private void OnUpdateRunInput()
        {
            pressInput?.Enable();
            holdInput?.Enable();

            if (MovementStateMachine == null || !MovementStateMachine.IsGrounded || !MovementStateMachine.IsMoving) SetIsRunning(false);
            else if (CrouchState.IsStateMachineCrouching(MovementStateMachine) || JumpState.IsStateMachineJumping(MovementStateMachine)) SetIsRunning(false);
            else if (hasToHoldInput) SetIsRunning(holdInput != null ? holdInput.GetValue() : false);
            else if (!hasToHoldInput)
            {
                if (pressInput != null) {
                    if (pressInput.GetValue()) SetIsRunning(!IsRunning);
                }
                else SetIsRunning(false);
            }
        }

        public static PlayerRunState GetState(PlayerMovementStateMachine stateMachine) {
             return GetState(stateMachine as MovementStateMachine) as PlayerRunState;
        }
    }
}