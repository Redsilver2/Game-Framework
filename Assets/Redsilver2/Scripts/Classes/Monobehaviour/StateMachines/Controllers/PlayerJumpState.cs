using RedSilver2.Framework.Inputs.Settings;
using RedSilver2.Framework.StateMachines.Controllers;
using RedSilver2.Framework.StateMachines.States;
using UnityEngine;

namespace RedSilver2.Framework.StateMachines
{
    public class PlayerJumpState : JumpState
    {
        [Space]
        [SerializeField] private PressInputSettings pressInput;
        public PressInputSettings PressInput => pressInput;

        public PlayerJumpState(PlayerMovementStateMachine stateMachine) : base(stateMachine) {

        }

#if UNITY_EDITOR
        private bool showInputs;

        public sealed override void DrawInpsector(StateMachine.InspectorVisualizer visualizer)
        {
            if (visualizer == null) return;
            else if (EditorExtension.DisplayFoldout("Inputs 🕹️", ref showInputs, visualizer.FoldoutColor)) {
                EditorExtension.DrawVerticalHelpBox(() => {
                    SetPressInput(EditorExtension.DisplayCustomField("Press Input", false, pressInput));
                }, visualizer.FoldoutColor, true);
            }


            base.DrawInpsector(visualizer);
        }
#endif

        public void SetPressInput(PressInputSettings pressInput) {
            this.pressInput = pressInput;
        }

        protected override void Update()
        {
            MovementStateMachine stateMachine = GetMovementStateMachine(this);
            pressInput?.Enable();

            base.Update();

            if(stateMachine != null && pressInput != null && CurrentJumpDelay <= 0f && JumpCount < MaxJumpCount) {
                if(pressInput.GetValue()) {
                    SetIsJumping(true);
                    if(JumpCount <= 0) { stateMachine?.ChangeState(this); }
                    else               { stateMachine?.ChangeState(this, false); }
                }
                else { SetIsJumping(false); }
            }
            else { SetIsJumping(false); }
        }
    }
}
