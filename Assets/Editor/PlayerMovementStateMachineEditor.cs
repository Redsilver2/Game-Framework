using RedSilver2.Framework.StateMachines.States;
using RedSilver2.Framework.StateMachines.Controllers;
using UnityEditor;

namespace RedSilver2.Framework.StateMachines.EditorTools
{
    [CustomEditor(typeof(PlayerCharacterControllerStateMachine))]
    public class PlayerMovementStateMachineEditor : MovementStateMachineEditor
    {

        private bool showRunInputs;
        private bool showCrouchCameraSettings;
        private bool showCrouchInputs;
        private bool showJumpInputs;

        protected sealed override void DisplayDefaultSettings(MovementStateMachine stateMachine)
        {
            base.DisplayDefaultSettings(stateMachine);
            DisplayDefaultSettings(stateMachine as PlayerMovementStateMachine);
        }

        protected virtual void DisplayDefaultSettings(PlayerMovementStateMachine stateMachine)
        {
            if(stateMachine != null) {
                stateMachine?.SetInputSetting(DisplayCustomField("Move Input Setting", false, stateMachine.MoveInputSetting));
            }
        }

        protected override void DisplayState(CrouchState state, ref bool showBaseSettings)
        {
            if (state != null) base.DisplayState(state, ref showBaseSettings);
            DisplayState(state as PlayerCrouchState);
        }

        protected sealed override void DisplayState(RunState state, ref bool showBaseSettings)
        {
            if (state != null) base.DisplayState(state, ref showBaseSettings);
            DisplayState(state as PlayerRunState);
        }

        protected sealed override void DisplayState(JumpState state, ref bool showBaseSettings)
        {
            if (state != null) base.DisplayState(state, ref showBaseSettings);
            DisplayState(state as PlayerJumpState);
        }

        protected virtual void DisplayState(PlayerCrouchState state)
        {
            if (state == null) return;

            if (DisplayFoldout("Camera Settings", ref showCrouchCameraSettings)) {
                EditorGUILayout.Space(2.5f);
                state?.SetCameraParent(DisplayCustomField("Camera Parent", true, state.CameraParent));

                EditorGUILayout.Space(2.5f);
                state?.SetCrouchCameraPosition(DisplayVector3Field("Crouch Position", state.CrouchPosition));
                state?.SetStandCameraPosition(DisplayVector3Field("Stand Position", state.StandPosition));
            }

            if (DisplayFoldout("Input Settings", ref showCrouchInputs)) {
                EditorGUILayout.Space(2.5f);
                state?.SetHasToHoldInput(DisplayToggle("Has To Hold Input", state.HasToHoldInput));
                state?.SetIsVerifyingRunCondition(DisplayToggle("Is Verifying Run Condition", state.IsVerifyingRunCondition));

                EditorGUILayout.Space(2.5f);
                state?.SetPressInput(DisplayCustomField("Press Input", false, state.PressInput));
                state?.SetHoldInput(DisplayCustomField("Hold Input", false, state.HoldInput));
            }
        }

        protected virtual void DisplayState(PlayerRunState state)
        {
            if (state == null) return;

            if (DisplayFoldout("Inputs Settings", ref showRunInputs)) {
                EditorGUILayout.Space(2.5f);
                state?.SetHasToHoldInput(DisplayToggle("Has To Hold Input", state.HasToHoldInput));
                state?.SetPressInput(DisplayCustomField("Press Input", false, state.PressInput));
                state?.SetHoldInput(DisplayCustomField("Hold Input", false, state.HoldInput));
            }
        }

        protected virtual void DisplayState(PlayerJumpState state)
        {
            if(state == null) return;

            if (DisplayFoldout("Inputs Settings", ref showJumpInputs))
            {
                EditorGUILayout.Space(2.5f);  
                state?.SetPressInput(DisplayCustomField("Press Input", false, state.PressInput));
            }
        }
        protected sealed override MovementState GetState(MovementStateMachine stateMachine, MovementStateType type) {
            return GetState(stateMachine as PlayerMovementStateMachine, type);
        }

        private MovementState GetState(PlayerMovementStateMachine stateMachine, MovementStateType type)
        {
            if (stateMachine == null || stateMachine.ContainsState(type)) return null;

            switch (type)
            {
                case MovementStateType.Idol  : return new PlayerIdolState(stateMachine);
                
                case MovementStateType.Walk  : return new PlayerWalkState(stateMachine);
                case MovementStateType.Fall  : return new PlayerFallState(stateMachine);
               
                case MovementStateType.Jump  : return new PlayerJumpState(stateMachine);
                case MovementStateType.Land  : return new PlayerLandState(stateMachine);
               
                case MovementStateType.Crouch: return new PlayerCrouchState(stateMachine);
                case MovementStateType.Run   : return new PlayerRunState(stateMachine);
                
                default: return null;
            }
        }
    }
}