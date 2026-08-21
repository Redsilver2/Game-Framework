using RedSilver2.Framework.Inputs.Settings;
using RedSilver2.Framework.StateMachines.Controllers;
using RedSilver2.Framework.StateMachines.Extensions;
using UnityEngine;
using UnityEngine.Events;

namespace RedSilver2.Framework.StateMachines.States
{
    [System.Serializable]
    public sealed class PlayerCrouchState : CrouchState
    {
        [Space]
        [SerializeField] private bool hasToHoldInput = true;

        [Space]
        [SerializeField] private PressInputSettings pressInput;
        [SerializeField] private HoldInputSettings holdInput;

        [Space]
        [SerializeField] private PlayerCrouchCameraUpdater cameraUpdater;
        [SerializeField] private PlayerMovementStateMotion positionSwayMotion;
        [SerializeField] private PlayerMovementStateMotion rotationSwayMotion;

        public bool HasToHoldInput           => hasToHoldInput;
        public PressInputSettings PressInput => pressInput;
        public HoldInputSettings  HoldInput  => holdInput;

        public PlayerCrouchCameraUpdater  CameraUpdater => cameraUpdater;
        public PlayerMovementStateMotion PositionSwayMotion => positionSwayMotion;
        public PlayerMovementStateMotion RotationSwayMotion => rotationSwayMotion;

        public PlayerCrouchState() : base() { }

#if UNITY_EDITOR
        public void Validate(PlayerMovementStateMachine stateMachine) {
            SetStateMachine(stateMachine);

            if(cameraUpdater == null) cameraUpdater = new PlayerCrouchCameraUpdater();
            cameraUpdater?.Validate(this, stateMachine);

            Validate();
        }

        protected override void Validate() {
            base.Validate();
            if (positionSwayMotion == null) positionSwayMotion = new PlayerMovementStateMotion();
            if (rotationSwayMotion == null) rotationSwayMotion = new PlayerMovementStateMotion();

            positionSwayMotion?.Validate(TYPE, MovementMotionUpdateMode.Sin, MovementMotionLateUpdateMode.Position, MovementMotionInputType.Move);
            rotationSwayMotion?.Validate(TYPE, MovementMotionUpdateMode.Sin, MovementMotionLateUpdateMode.Rotation, MovementMotionInputType.Move);
        }
#endif

        private void SetStateMachine(PlayerMovementStateMachine stateMachine)
        {
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
            rotationSwayMotion?.Disable();
            rotationSwayMotion?.SetStateMachine(null);

            positionSwayMotion?.Disable();
            positionSwayMotion?.SetStateMachine(null);
            base.OnDisabled();
        }
   
        public void UpdateInput() {
            pressInput?.Enable();
            holdInput?.Enable();

            if(!CanChangeState) { SetIsCrouching(true); }
            else if (MovementStateMachine == null || !IsEnabled || !MovementStateMachine.IsGrounded) SetIsCrouching(false);
            else if (RunState.IsStateMachineRunning(MovementStateMachine) || JumpState.IsStateMachineJumping(MovementStateMachine)) SetIsCrouching(false);
            else if (hasToHoldInput) { SetIsCrouching(holdInput != null ? holdInput.GetValue() : false); }
            else if (!hasToHoldInput) { SetIsCrouching(pressInput != null ? (pressInput.GetValue() ? !IsCrouching : IsCrouching) : false); }
        }

        protected sealed override int GetLayerToIgnore() {
            return GameManager.PlayerLayer;
        }
    }
}