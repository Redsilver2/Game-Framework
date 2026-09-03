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

#if UNITY_EDITOR

        private bool showCameraSettings;
        private bool showInputs;

        public override void DrawInpsector(Color foldoutColor, Color fieldColor)
        {
            DrawCameraSettings(foldoutColor, fieldColor);
            DrawInputSettings(foldoutColor, fieldColor);

            base.DrawInpsector(foldoutColor, fieldColor);
        }

        private void DrawCameraSettings(Color foldoutColor, Color fieldColor)
        {
            EditorExtension.IncrementIndent();

            if (EditorExtension.DisplayFoldout("Camera 📷", ref showCameraSettings, foldoutColor)) {
                EditorExtension.IncrementIndent();

                EditorExtension.Space(2.5f);         
                SetCameraParent(EditorExtension.DisplayCustomField("Camera Parent", true, cameraParent, fieldColor));
               
                EditorExtension.Space(2.5f);
                SetCrouchCameraPosition(EditorExtension.DisplayVector3Field("Crouch Position 📍", crouchCameraPosition, fieldColor));
               
                SetStandCameraPosition(EditorExtension.DisplayVector3Field("Stand Position 📍", standCameraPosition, fieldColor));
                EditorExtension.DecrementIndent();
            }


            EditorExtension.DecrementIndent();
        }

        private void DrawInputSettings(Color foldoutColor, Color fieldColor)
        {
            EditorExtension.IncrementIndent();

            if (EditorExtension.DisplayFoldout("Inputs 🕹️", ref showInputs, foldoutColor))
            {
                EditorExtension.IncrementIndent();
                EditorExtension.Space(2.5f);

                SetHasToHoldInput(EditorExtension.DisplayToggle("Has To Hold Input", hasToHoldInput, fieldColor));
                SetIsVerifyingRunCondition(EditorExtension.DisplayToggle("Is Verifying Run Condition", isVerifyingRunCondition, fieldColor));

                EditorExtension.Space(2.5f);
                SetPressInput(EditorExtension.DisplayCustomField("Press Input", false, pressInput, fieldColor));

                SetHoldInput(EditorExtension.DisplayCustomField("Hold Input", false, holdInput, fieldColor));
                EditorExtension.DecrementIndent();
            }

            EditorExtension.DecrementIndent();
        }



#endif

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
           
            holdInput?.Enable();
            pressInput?.Enable();

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
