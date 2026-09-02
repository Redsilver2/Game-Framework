using RedSilver2.Framework.StateMachines.States;
using System;
using UnityEditor;
using UnityEngine;

namespace RedSilver2.Framework.StateMachines.EditorTools
{
    [CustomEditor(typeof(MovementStateMachine))]
    public abstract class MovementStateMachineEditor : StateMachineEditor {

        protected override void DisplayDefaultSettings(StateMachine stateMachine) {
            DisplayDefaultSettings(stateMachine as MovementStateMachine);
        }

        protected virtual void DisplayDefaultSettings(MovementStateMachine stateMachine)
        {
            if(stateMachine != null) {
                stateMachine?.SetIs2DMovement(DisplayToggle("Is 2D Movement", stateMachine.Is2DMovement));
                stateMachine?.SetGroundCheckRange(DisplayFloatSlider("Ground Check Range", stateMachine.GroundCheckRange, 0f, 1000f));
            }
        }



        protected sealed override void DisplayStateSettings(State state, ref bool showBaseSettings)
        {
            DisplayState(state as MovementState, ref showBaseSettings);
        }

        protected sealed override void DisplayStateExtensions(StateMachine stateMachine, State state)
        {
            if (stateMachine == null || state == null) return;
            DisplayStateExtensions(stateMachine as MovementStateMachine, state as MovementState);
        }

        private void DisplayStateExtensions(MovementStateMachine stateMachine, MovementState state) {
            if (stateMachine == null || state == null) return;
            switch (state.Type) {
                case MovementStateType.Land: DisplayStateExtensions(stateMachine, state as LandState); break;
            }

        }

        protected virtual void DisplayStateExtensions(MovementStateMachine stateMachine, LandState state)
        {
            if (stateMachine == null || state == null) return;

        }


        private void DisplayState(MovementState state, ref bool showBaseSettings) {
            if(state == null) return;

            switch(state.Type) {  
                case MovementStateType.Idol:   DisplayState(state as IdolState, ref showBaseSettings);   break;
                case MovementStateType.Walk:   DisplayState(state as WalkState, ref showBaseSettings);   break;
                case MovementStateType.Fall:   DisplayState(state as FallState, ref showBaseSettings);   break;
                case MovementStateType.Jump:   DisplayState(state as JumpState, ref showBaseSettings);   break;
                case MovementStateType.Land:                                       break;
                case MovementStateType.Crouch: DisplayState(state as CrouchState, ref showBaseSettings); break;
                case MovementStateType.Run:    DisplayState(state as RunState, ref showBaseSettings);    break;
                default: break;
            }

        }


        protected virtual void DisplayState(FallState state, ref bool showBaseSettings)
        {
            if (state == null) return;
            EditorGUILayout.Space(2.5f);

            if (DisplayFoldout("Base Settings ⚙️", ref showBaseSettings)) {
                state?.SetAirbornTransitionTrigger(DisplayFloatSlider("Airborn Transition Trigger", state.AirbornTransitionTrigger, 0f, 1000f));

                EditorGUILayout.Space(5f);
                state?.SetGroundedFallSpeed(DisplayFloatSlider("Grounded Fall Speed", state.GroundedFallSpeed, -1000f, 0f));
                state?.SetGroundedFallTransitionSpeed(DisplayFloatSlider("Grounded Fall Transition Speed", state.GroundedFallTransitionSpeed, 0f, 1000f));

                EditorGUILayout.Space(5f);
                state?.SetFallSpeed(DisplayFloatSlider("Fall Speed", state.FallSpeed, -1000f, 0f));
                state?.SetFallTransitionSpeed(DisplayFloatSlider("Fall Transition Speed", state.FallTransitionSpeed, 0f, 1000f));

                EditorGUILayout.Space(5f);
                state?.SetCanAffectMovementSpeed(DisplayToggle("Can Affect Movement Speed", state.CanAffectMovementSpeed));

                EditorGUILayout.Space(5f);
                state?.SetMoveSpeed(DisplayFloatSlider("Move Speed 💨", state.MoveSpeed, 0f, 1000f));
                state?.SetMoveTransitionSpeed(DisplayFloatSlider("Move Transition Speed", state.MoveTransitionSpeed, 0f, 1000f));
            }
        }


        protected virtual void DisplayState(CrouchState state, ref bool showBaseSettings)
        {
            if (state == null) return;
            EditorGUILayout.Space(2.5f);

            if (DisplayFoldout("Base Settings ⚙️", ref showBaseSettings)) {
                state?.SetMoveSpeed(DisplayFloatSlider("Crouch Speed 💨", state.MoveSpeed, 0f, 1000f));
                state?.SetMoveTransitionSpeed(DisplayFloatSlider("Crouch Transition Speed ", state.MoveTransitionSpeed, 0f, 1000f));

                EditorGUILayout.Space(5f);
                state?.SetCrouchHeight(DisplayFloatSlider("Crouch Height", state.CrouchHeight, 0f, 1000f));
                state?.SetCrouchHeightTransitionSpeed(DisplayFloatSlider("Crouch Height Transition Speed", state.CrouchHeightTransitionSpeed, 0f, 1000f));

                EditorGUILayout.Space(5f);
                state?.SetStandHeight(DisplayFloatSlider("Stand Height", state.StandHeight, 0f, 1000f));
                state?.SetStandHeightTransitionSpeed(DisplayFloatSlider("Stand Height Transition Speed", state.StandHeightTransitionSpeed, 0f, 1000f));

                EditorGUILayout.Space(5f);
                state?.SetCrouchSafetyCheckDistance(DisplayFloatSlider("Crouch Safety Check Distance", state.CrouchSafetyCheckDistance, 0f, 1000f));
               
            }
        }

        protected virtual void DisplayState(JumpState state, ref bool showBaseSettings)
        {
            if (state == null) return;
            EditorGUILayout.Space(2.5f);

            if (DisplayFoldout("Base Settings ⚙️", ref showBaseSettings)) {       
                state?.SetJumpForce(DisplayFloatSlider("Jump Force 💪", state.JumpForce, 0f, 1000f));
                state?.SetMaxJumpCount(DisplayUIntSlider("Max Jump Count ❓", state.MaxJumpCount, 100));
                state?.SetMaxJumpDelay(DisplayFloatSlider("Max Jump Delay ⌛", state.MaxJumpDelay, 0f, 1000f));
            }
        }

        protected virtual void DisplayState(IdolState state, ref bool showBaseSettings) {
            if(state == null) return;
            EditorGUILayout.Space(2.5f);

            if (DisplayFoldout("Base Settings ⚙️", ref showBaseSettings)) {
                state?.SetMoveSpeedTransition(DisplayFloatSlider("Move Transition Speed ", state.MoveSpeedTransition, 0f, 1000f));
            }
        }

        protected virtual void DisplayState(WalkState state, ref bool showBaseSettings)
        {
            if (state == null) return;
            EditorGUILayout.Space(2.5f);
            if(DisplayFoldout("Base Settings ⚙️", ref showBaseSettings)) {
                EditorGUILayout.Space(2.5f);
                state?.SetWalkSpeed(DisplayFloatSlider("Walk Speed 💨", state.MoveSpeed, 0f, 1000f));
                state?.SetTransitionSpeed(DisplayFloatSlider("Walk Transition Speed", state.MoveTransitionSpeed, 0f, 1000f));
            }
        }

        protected virtual void DisplayState(RunState state, ref bool showBaseSettings)
        {
            if (state == null) return;
            EditorGUILayout.Space(2.5f);

            if(DisplayFoldout("Base Settings ⚙️", ref showBaseSettings)) {
                EditorGUILayout.Space(2.5f);
                state?.SetRunSpeed(DisplayFloatSlider("Run Speed 💨", state.MoveSpeed, 0f, 1000f));
                state?.SetRunTransitionSpeed(DisplayFloatSlider("Run Transition Speed", state.MoveTransitionSpeed, 0f, 1000f));
            }
        }

        protected sealed override State GetState(StateMachine stateMachine, int stateIndex)
        {
            MovementStateType[] types = GetValues() as MovementStateType[];

            if (stateMachine == null || types == null || stateIndex < 0 || stateIndex >= types.Length) {
                return null;
            }

            return GetState(stateMachine as MovementStateMachine, types[stateIndex]);
        }

        protected sealed override Array GetValues()
        {
            return Enum.GetValues(typeof(MovementStateType));
        }

        protected abstract MovementState GetState(MovementStateMachine stateMachine, MovementStateType type);
    }
}
