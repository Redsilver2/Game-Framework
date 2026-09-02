using RedSilver2.Framework.Inputs.Settings;
using RedSilver2.Framework.StateMachines.Controllers;
using UnityEngine;



namespace RedSilver2.Framework.StateMachines.States
{
    [System.Serializable]
    public class PlayerCrouchState : CrouchState
    {
        [Space]
        [SerializeField] private Transform cameraParent;

        [Space]
        [SerializeField] private Vector3 crouchCameraPosition;
        [SerializeField] private Vector3 standCameraPosition;

        [Space]
        [SerializeField] private bool hasToHoldInput;
        [SerializeField] private bool isVerifyingRunCondition;


        [Space]
        [SerializeField] private PressInputSettings pressInput;
        [SerializeField] private HoldInputSettings holdInput;

        public bool HasToHoldInput => hasToHoldInput;
        public bool IsVerifyingRunCondition => isVerifyingRunCondition;

        public Vector3 CrouchPosition => crouchCameraPosition;
        public Vector3 StandPosition => standCameraPosition;
        public Transform CameraParent => cameraParent;

        public PressInputSettings PressInput => pressInput;
        public HoldInputSettings HoldInput => holdInput;

        public PlayerCrouchState(PlayerMovementStateMachine stateMachine) : base(stateMachine)
        {

        }

        public void SetHasToHoldInput(bool hasToHoldInput)
        {
            this.hasToHoldInput = hasToHoldInput;
        }

        public void SetIsVerifyingRunCondition(bool isVerifyingRunCondition)
        {
            this.isVerifyingRunCondition = isVerifyingRunCondition;
        }

        public void SetCameraParent(Transform cameraParent)
        {
            this.cameraParent = cameraParent;
        }

        public void SetStandCameraPosition(Vector3 standCameraPosition)
        {
            this.standCameraPosition = standCameraPosition;
        }

        public void SetCrouchCameraPosition(Vector3 crouchCameraPosition)
        {
            this.crouchCameraPosition = crouchCameraPosition;
        }

        public void SetPressInput(PressInputSettings pressInput)
        {
            this.pressInput = pressInput;
        }

        public void SetHoldInput(HoldInputSettings holdInput)
        {
            this.holdInput = holdInput;
        }

        protected override void OnRemoved()
        {
            base.OnRemoved();
            if (cameraParent != null) cameraParent.localPosition = standCameraPosition;
        }

        protected override void Update()
        {
            MovementStateMachine stateMachine = GetMovementStateMachine(this);

            if (stateMachine != null && IsEnabled) {
                if (stateMachine.IsMoving && stateMachine.IsGrounded && !JumpState.GetIsJumping(stateMachine)){
                    if(isVerifyingRunCondition && RunState.GetIsRunning(stateMachine)) SetIsCrouching(false);
                    else if (hasToHoldInput && holdInput != null) SetIsCrouching(holdInput.GetValue());
                    else if (!hasToHoldInput && pressInput != null) SetIsCrouching(pressInput.GetValue() ? !IsCrouching : IsCrouching);
                    else SetIsCrouching(false);
                }
                else { SetIsCrouching(false); }
            }
            else { SetIsCrouching(false); }

            base.Update();

            if (cameraParent != null)
                cameraParent.localPosition = Vector3.Lerp(cameraParent.localPosition, IsCrouching ? crouchCameraPosition : standCameraPosition,
                                                          Time.deltaTime * (IsCrouching ? CrouchHeightTransitionSpeed : StandHeightTransitionSpeed));
        }

        public static PlayerCrouchState GetState(PlayerMovementStateMachine stateMachine) {
            return GetState(stateMachine as MovementStateMachine) as PlayerCrouchState;
        }
    }
}
