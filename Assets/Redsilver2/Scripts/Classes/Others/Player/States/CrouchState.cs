using RedSilver2.Framework.StateMachines.Controllers;
using RedSilver2.Framework.StateMachines.States.Movement;
using UnityEngine;

namespace RedSilver2.Framework.StateMachines.States
{
    public class CrouchState : MovementState
    {
        public CrouchState(MovementStateMachine owner) : base(owner) {

        }

        private bool CanResetCrouch(Transform transform, StateMachineController controller)
        {
            if(transform == null || controller == null) return false;
            return !Physics.Raycast(transform.position, transform.up, 2f, ~transform.gameObject.layer);
        }

        public sealed override bool IsValidTransition() {
            if (MovementHandler == null) return false;
            return MovementHandler.IsGrounded && MovementHandler.IsCrouching;
        }

        protected sealed override void AddRequiredTransitionStates(MovementStateMachine stateMachine) {

        }

        protected sealed override void SetIncompatibleStateTransitions(ref MovementStateType[] results) {
            results = GetExcludedStateTypes(new MovementStateType[] { MovementStateType.Walk, MovementStateType.Run, MovementStateType.Idol });
        }

        protected sealed override void SetPlayerStateType(ref MovementStateType type) {
            type = MovementStateType.Crouch;
        }

        protected sealed override void SetPlayerInputsEvents(PlayerMovementHandler handler)
        {
            if(handler == null) return;
            handler?.EnableCrouchInputUpdate();

            AddOnStateRemovedListener(() => { handler?.DisableCrouchInputUpdate(); });
            AddOnStateEnteredListener(() => { handler?.SetCanResetCrouch(false); });
            AddOnStateExitedListener (() => { handler?.SetCanResetCrouch(true); });

            AddOnUpdateListener(() => {
                if (handler == null || owner == null) return;

                Transform transform = handler.GetTransform();
                StateMachineController stateMachine = owner.Controller;
                handler?.SetCanResetCrouch(CanResetCrouch(transform, stateMachine));

                Debug.DrawRay(transform.position, transform.up, Color.red);
            });
        }
    }
}
