using RedSilver2.Framework.Inputs.Settings;
using RedSilver2.Framework.StateMachines.Controllers;
using UnityEngine;
using UnityEngine.Events;

namespace RedSilver2.Framework.StateMachines.States
{
    [System.Serializable]
    public sealed class PlayerCrouchState : CrouchState
    {
        [Space]
        [SerializeField] private Transform cameraCrouchTransform;

        [Space]
        [SerializeField] private Vector3 crouchCameraPosition;
        [SerializeField] private Vector3 standCameraPosition;

        [Space]
        [SerializeField] private float crouchCameraUpdateSpeed;


        [Space]
        [SerializeField] private bool hasToHoldInput = true;


        [Space]
        [SerializeField] private PressInputSettings pressInput;

        [Space]
        [SerializeField] private HoldInputSettings holdInput;

        public bool HasToHoldInput           => hasToHoldInput;
        public PressInputSettings PressInput => pressInput;
        public HoldInputSettings  HoldInput  => holdInput;

        public PlayerCrouchState() : base() {

        }

#if UNITY_EDITOR
        public void Validate(PlayerMovementStateMachine stateMachine) {
            SetStateMachine(stateMachine);
            Validate();
        }

        protected override void Validate() {
            base.Validate();
            crouchCameraUpdateSpeed = Mathf.Clamp(crouchCameraUpdateSpeed, 0f, float.MaxValue);
        }
#endif

        protected void SetStateMachine(PlayerMovementStateMachine stateMachine)
        {
            this.MovementStateMachine = stateMachine;
            SetStateMachine(stateMachine as UpdatableStateMachine);
        }

        protected override void OnEnabled()
        {
            MovementStateMachine?.AddOnUpdateListener(OnUpdateCrouch);
            base.OnEnabled();
        }

        protected override void OnDisabled()
        {
            MovementStateMachine?.RemoveOnUpdateListener(OnUpdateCrouch);
            base.OnDisabled();
        }

        private void UpdateCameraCrouchTransform(Vector3 position)
        {
            if (cameraCrouchTransform != null)
                cameraCrouchTransform.localPosition = Vector3.Lerp(cameraCrouchTransform.localPosition, position, Time.deltaTime * crouchCameraUpdateSpeed);
        }

         
        private void OnUpdateCrouch() {
            pressInput?.Enable();
            holdInput?.Enable();

            if (MovementStateMachine == null || !IsEnabled || !MovementStateMachine.IsGrounded) SetIsCrouching(false);
            else if (RunState.IsStateMachineRunning(MovementStateMachine) || JumpState.IsStateMachineJumping(MovementStateMachine)) SetIsCrouching(false);
            else if (hasToHoldInput) {
                SetIsCrouching(holdInput != null ? holdInput.GetValue() : false);
            }
            else if (!hasToHoldInput) {
                SetIsCrouching(pressInput != null ? (pressInput.GetValue() ? !IsCrouching : IsCrouching) : false);
            }

            UpdateCameraCrouchTransform(IsCrouching ? crouchCameraPosition : standCameraPosition);
        }

        protected sealed override int GetLayerToIgnore() {
            return GameManager.PlayerLayer;
        }
    }
}