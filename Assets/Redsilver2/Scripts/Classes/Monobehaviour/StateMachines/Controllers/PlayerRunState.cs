
using RedSilver2.Framework.Inputs.Settings;
using RedSilver2.Framework.StateMachines.Controllers;
using UnityEngine;

namespace RedSilver2.Framework.StateMachines.States
{
    [System.Serializable]
    public class PlayerRunState : RunState
    {
        [SerializeField] private bool hasToHoldInput;

        [Space]
        [SerializeField] private PressInputSettings pressInput;
        [SerializeField] private HoldInputSettings holdInput;

        public bool HasToHoldInput => hasToHoldInput;
        public PressInputSettings PressInput => pressInput;
        public HoldInputSettings HoldInput => holdInput;

        public PlayerRunState(PlayerMovementStateMachine stateMachine) : base(stateMachine) {

        }

#if UNITY_EDITOR
        private bool showInputs;

        public sealed override void DrawInpsector(Color foldoutColor, Color fieldColor)
        {
            EditorExtension.IncrementIndent();

            if (EditorExtension.DisplayFoldout("Inputs 🕹️", ref showInputs, foldoutColor)) {
                EditorExtension.IncrementIndent();
                EditorExtension.Space(2.5f);

                SetHasToHoldInput(EditorExtension.DisplayToggle("Has To Hold Input", hasToHoldInput, fieldColor));
                SetPressInput(EditorExtension.DisplayCustomField("Press Input", false, pressInput, fieldColor));
                SetHoldInput(EditorExtension.DisplayCustomField("Hold Input", false, holdInput, fieldColor));
               
                EditorExtension.DecrementIndent();
            }

            EditorExtension.DecrementIndent();

            base.DrawInpsector(foldoutColor, fieldColor);
        }
#endif

        public void SetHasToHoldInput(bool hasToHoldInput)
        {
            this.hasToHoldInput = hasToHoldInput;
        }

        public void SetPressInput(PressInputSettings pressInput)
        {
            this.pressInput = pressInput;
        }

        public void SetHoldInput(HoldInputSettings holdInput)
        {
            Debug.Log("hold: " + holdInput);
            this.holdInput = holdInput;
        }

        public void Update() {
            MovementStateMachine stateMachine = GetMovementStateMachine(this);
         
            if (stateMachine != null && IsEnabled) {
                if (stateMachine.IsMoving && stateMachine.IsGrounded && !JumpState.GetIsJumping(stateMachine) && CrouchState.GetIsCrouching(stateMachine)) {
                    if      (hasToHoldInput  && holdInput  != null) SetIsRunning(holdInput.GetValue());
                    else if (!hasToHoldInput && pressInput != null) SetIsRunning(pressInput.GetValue() ? !IsRunning : IsRunning);
                    else SetIsRunning(false);
                }
                else { SetIsRunning(false); }
            }
            else { SetIsRunning(false); }
        }

        public static PlayerRunState GetState(PlayerMovementStateMachine stateMachine)
        {
            return GetState(stateMachine as MovementStateMachine) as PlayerRunState;
        }
    }
}
