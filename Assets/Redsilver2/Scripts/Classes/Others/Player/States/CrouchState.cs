using RedSilver2.Framework.Inputs;
using RedSilver2.Framework.StateMachines.States.Movement;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace RedSilver2.Framework.StateMachines.States
{
    public class CrouchState : MovementState
    {
        private CrouchStateInitializer initializer;

        private const string HOLD_CROUCH_INPUT     = "Hold Crouch Input";
        private const string PRESS_CROUCH_INPUT    = "Press Crouch Input";
        private const string CROUCH_INPUT_SETTING  = "Crouch Input Setting";

        public CrouchState(MovementStateMachine owner, CrouchStateInitializer initializer) : base(owner) {
            this.initializer = initializer;

            AddOnStateEnteredListener(() => {
                if (this.initializer == null) return;
                this.initializer?.SetCanResetCrouch(false);
                this.initializer.SetHeight(this.initializer.CrouchHeight);
            });
            AddOnStateExitedListener(() => {
                if (this.initializer == null) return;
                this.initializer?.SetCanResetCrouch(true);
                this.initializer.SetHeight(this.initializer.StandingHeight);
            });

            AddOnUpdateListener(() => {
                if (this.initializer == null || MovementHandler == null) return;
                this.initializer?.SetCanResetCrouch(MovementHandler.GetTransform());
                MovementHandler?.UpdateMoveSpeed(this.initializer.MoveSpeed, this.initializer.CrouchTransitionSpeed);
            });

            AddOnLateUpdateListener(OnLateUpdate(this));

            owner?.AddOnStateModuleAddedListener(OnStateModuleAdded);
            owner?.AddOnStateAddedListener(OnStateAdded);
            owner?.AddOnStateRemovedListener(OnStateRemoved);

            if (owner != null) {
                foreach(State state in owner.GetStates()) {
                   OnStateAdded(state); 
                }
            }
        }

        protected override void RemoveAllListenersFromOwner(StateMachine owner)
        {
            base.RemoveAllListenersFromOwner(owner);

            owner?.RemoveOnStateModuleAddedListener(OnStateModuleAdded);
            owner?.RemoveOnStateAddedListener(OnStateAdded);
            owner?.RemoveOnStateRemovedListener(OnStateRemoved);
        }

        private void OnStateModuleAdded(StateModule module)
        {
            if(module is CrouchStateInitializer) initializer = module as CrouchStateInitializer;
        }

        private void OnStateAdded(State state) {
            MovementState movementState = state as MovementState;   
           
            if(movementState is not CrouchState) {
                movementState?.AddOnLateUpdateListener(OnLateUpdate(movementState));
            }
        }

        private void OnStateRemoved(State state) {
            MovementState movementState = state as MovementState;

            if (movementState is not CrouchState) {
                movementState?.RemoveOnLateUpdateListener(OnLateUpdate(movementState));
            }
        }

        private UnityAction OnLateUpdate(MovementState state)
        {
            if(state == null) return null;

            return () => {
                if (state == null || initializer == null) return;
                state.MovementHandler?.Crouch(initializer.Height, initializer.GetHeightTransitionSpeed());
            };
        }

        protected sealed override void SetIncompatibleStateTransitions(ref MovementStateType[] results) {
            results = GetExcludedStateTypes(new MovementStateType[] { MovementStateType.Walk, MovementStateType.Run, MovementStateType.Idol });
        }

        protected sealed override void SetPlayerStateType(ref MovementStateType type) {
            type = MovementStateType.Crouch;
        }

        public static OverrideableHoldInput GetHoldInput()
        {
            return InputManager.GetOrCreateOverrideableHoldInput(HOLD_CROUCH_INPUT, KeyboardKey.C, GamepadButton.ButtonEast);
        }

        public static OverrideablePressInput GetPressInput()
        {
            return InputManager.GetOrCreateOverrideablePressInput(PRESS_CROUCH_INPUT, KeyboardKey.C, GamepadButton.ButtonEast);
        }

        public static bool HasToHoldInput() {
            if (!PlayerPrefs.HasKey(CROUCH_INPUT_SETTING))
                return true;

            return PlayerPrefs.GetString(CROUCH_INPUT_SETTING).Equals("hold");
        }
    }
}
