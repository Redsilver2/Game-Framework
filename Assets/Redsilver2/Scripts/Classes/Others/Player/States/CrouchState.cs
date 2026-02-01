using RedSilver2.Framework.StateMachines.Controllers;
using UnityEngine;

namespace RedSilver2.Framework.StateMachines.States
{
    public class CrouchState : PlayerState
    {
        public CrouchState(PlayerStateMachine owner) : base(owner) {

            MovementHandler?.EnableCrouchInputUpdate();

            AddOnStateRemovedListener(() => { MovementHandler?.DisableCrouchInputUpdate(); });
            AddOnStateEnteredListener(() => { MovementHandler?.SetCanResetCrouch(false); });
            AddOnStateExitedListener (() => { MovementHandler?.SetCanResetCrouch(true);  });

            AddOnUpdateListener(() => {
                if (MovementHandler == null || owner == null) return;

                Transform transform                 =  MovementHandler.GetTransform();
                StateMachineController stateMachine = owner.Controller;
                MovementHandler?.SetCanResetCrouch(CanResetCrouch(transform, stateMachine));

                Debug.DrawRay(transform.position, transform.up, Color.red);
            });
        }

        private bool CanResetCrouch(Transform transform, StateMachineController controller)
        {
            if(transform == null || controller == null) return false;
            return !Physics.Raycast(transform.position, transform.up, controller.CrouchCheckDistance, ~transform.gameObject.layer);
        }

        public sealed override bool IsValidTransition() {
            if (MovementHandler == null) return false;
            return MovementHandler.IsGrounded && MovementHandler.IsCrouching;
        }

        protected sealed override void AddRequiredTransitionStates(PlayerStateMachine stateMachine) {

        }

        protected sealed override void SetIncompatibleStateTransitions(ref PlayerStateType[] results) {
            results = GetStateTypes(new PlayerStateType[] { PlayerStateType.Walk, PlayerStateType.Run, PlayerStateType.Idol });
        }

        protected sealed override void SetPlayerStateType(ref PlayerStateType type) {
            type = PlayerStateType.Crouch;
        }
    }
}
