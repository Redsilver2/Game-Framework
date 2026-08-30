using RedSilver2.Framework.Inputs.Settings;
using RedSilver2.Framework.StateMachines.Controllers;
using RedSilver2.Framework.StateMachines.Extensions;
using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;

namespace RedSilver2.Framework.StateMachines.States
{
    [System.Serializable]
    public sealed class PlayerCrouch : PlayerMovement
    {
        [Space]
        [SerializeField] private PlayerCrouchCameraUpdater cameraUpdater;

        [Space]
        [SerializeField] private bool hasToHoldInput = true;

        [Space]
        [SerializeField] private PressInputSettings pressInput;
        [SerializeField] private HoldInputSettings holdInput;

        [Space]
        [SerializeField] private PlayerMovementStateMotion rotationMotion;
        [SerializeField] private PlayerMovementStateMotion positionMotion;

        public bool        HasToHoldInput => hasToHoldInput;
        public CrouchState BaseState      => GetState() as CrouchState;

        public PlayerCrouch() : base() { }

#if UNITY_EDITOR
        [Space]
        [SerializeField] private bool canApplyPositionMotion;
        [SerializeField] private bool canApplyRotationMotion;

        private const string POSITION_MOTION_EVENT = "Position Motion";
        private const string ROTATION_MOTION_EVENT = "Rotation Motion";

        protected override void Validate(PlayerMovementStateMachine stateMachine) {
            base.Validate(stateMachine);

            if (cameraUpdater == null) cameraUpdater = new PlayerCrouchCameraUpdater();
            cameraUpdater?.Validate(stateMachine);
        }

        protected override void Validate(MovementState state)
        {
           if(state != null) {
              SetMovementMotion(ref rotationMotion, ROTATION_MOTION_EVENT, MovementMotionUpdateMode.Sin, MovementMotionLateUpdateMode.Rotation, MovementMotionInputType.Move);
              SetMovementMotion(ref positionMotion, POSITION_MOTION_EVENT, MovementMotionUpdateMode.Sin, MovementMotionLateUpdateMode.Position, MovementMotionInputType.Move);
           }
        }

        private void SetMovementMotion(ref PlayerMovementStateMotion motion, string name, MovementMotionUpdateMode updateMode, MovementMotionLateUpdateMode lateUpdateMode, MovementMotionInputType inputType)
        {
            if(motion == null) motion = new PlayerMovementStateMotion();
           
            motion.name = name;
            motion?.SetUpdateMode(updateMode);
           
            motion?.SetLateUpdateMode(lateUpdateMode);
            motion?.SetInputType(inputType);
        }
#endif
        private void Update() {
            CrouchState baseState = BaseState;

            holdInput?.Enable();
            pressInput?.Enable();
            
            baseState?.Update();
            if (baseState == null) return;

            bool isCrouching = false;

            if (!baseState.CanChangeState) { isCrouching = true; }
            else if(stateMachine != null && baseState.IsEnabled && !JumpState.IsStateMachineJumping(stateMachine)) {
                if (hasToHoldInput) { isCrouching = holdInput != null ? holdInput.GetValue() : false; }
                else { isCrouching = pressInput != null ? (pressInput.GetValue() ? !baseState.IsCrouching : baseState.IsCrouching) : false; }
            }

            baseState?.SetIsCrouching(isCrouching);
        }

        private void LateUpdate() { cameraUpdater?.LateUpdate(); }

        protected override void InitializeEvent(MovementState state, MovementStateMachine stateMachine)
        {

            base.InitializeEvent(state, stateMachine);
            stateMachine?.AddOnUpdateListener(Update);
            stateMachine?.AddOnLateUpdateListener(LateUpdate);

        }

        protected override void UnInitializeEvent(MovementState state, MovementStateMachine stateMachine)
        {
            base.UnInitializeEvent(state, stateMachine);
            stateMachine?.RemoveOnUpdateListener(Update);
            stateMachine?.RemoveOnLateUpdateListener(LateUpdate);
        }

        protected sealed override void SetBaseState(ref MovementState state)
        {
            state = new CrouchState();
        }
    }
}