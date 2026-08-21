using RedSilver2.Framework.Inputs.Settings;
using RedSilver2.Framework.StateMachines.Controllers;
using RedSilver2.Framework.StateMachines.Extensions;
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
        [SerializeField] private PlayerMovementStateMotion positionSwayMotion;
        [SerializeField] private PlayerMovementStateMotion rotationSwayMotion;

        public PressInputSettings PressInput => pressInput;
        public HoldInputSettings  HoldInput  => holdInput;

        public PlayerMovementStateMotion PositionSwayMotion => positionSwayMotion;
        public PlayerMovementStateMotion RotationSwayMotion => rotationSwayMotion;

        public PlayerRunState() : base()  { }

#if UNITY_EDITOR
        public void Validate(PlayerMovementStateMachine stateMachine)
        {
            SetStateMachine(stateMachine);
            Validate();
        }

        protected override void Validate()
        {
            base.Validate();

            if (positionSwayMotion == null) positionSwayMotion = new PlayerMovementStateMotion();
            if (rotationSwayMotion == null) rotationSwayMotion = new PlayerMovementStateMotion();

            positionSwayMotion?.Validate(TYPE, MovementMotionUpdateMode.Sin, MovementMotionLateUpdateMode.Position, MovementMotionInputType.Move);
            rotationSwayMotion?.Validate(TYPE, MovementMotionUpdateMode.Sin, MovementMotionLateUpdateMode.Rotation, MovementMotionInputType.Move);

            rotationSwayMotion?.Validate();
            positionSwayMotion?.Validate();
        }
#endif

        private void SetStateMachine(PlayerMovementStateMachine stateMachine) {
            this.MovementStateMachine = stateMachine;
            SetStateMachine(stateMachine as UpdatableStateMachine);
        }

        protected override void OnEnabled()
        {
            positionSwayMotion?.Enable();
            positionSwayMotion?.SetStateMachine(MovementStateMachine as PlayerMovementStateMachine);

            rotationSwayMotion?.Enable();
            rotationSwayMotion?.SetStateMachine(MovementStateMachine as PlayerMovementStateMachine);

            base.OnEnabled();
        }

        protected override void OnDisabled()
        {
            positionSwayMotion?.Disable();
            positionSwayMotion?.SetStateMachine(null);

            rotationSwayMotion?.Enable();
            rotationSwayMotion?.SetStateMachine(null);

            base.OnDisabled();
        }


        public void UpdateInput()
        {
            pressInput?.Enable();
            holdInput?.Enable();

            if (MovementStateMachine == null || !MovementStateMachine.IsGrounded || !MovementStateMachine.IsMoving || !IsEnabled) SetIsRunning(false);
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