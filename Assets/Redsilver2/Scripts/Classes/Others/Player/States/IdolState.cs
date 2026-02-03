
using RedSilver2.Framework.StateMachines.States.Movement;

namespace RedSilver2.Framework.StateMachines.States {
    public sealed class IdolState : MovementState
    {
        public IdolState(MovementStateMachine owner) : base(owner) {
            
        }

        public sealed override bool IsValidTransition() {
            if (MovementHandler == null) return false;

            return MovementHandler.IsGrounded && !MovementHandler.IsCrouching
                    && !MovementHandler.IsRunning && MovementHandler.GetMoveMagnitude() == 0f;
        }

        protected sealed override void AddRequiredTransitionStates(MovementStateMachine stateMachine) {
           
        }

        protected sealed override void SetIncompatibleStateTransitions(ref MovementStateType[] results) {
            results = GetExcludedStateTypes(new MovementStateType[] { MovementStateType.Walk, MovementStateType.Run, MovementStateType.Fall, 
                                                            MovementStateType.Jump, MovementStateType.Crouch });
        }

        protected sealed override void SetPlayerInputsEvents(PlayerMovementHandler handler) {

        }

        protected sealed override void SetPlayerStateType(ref MovementStateType type) {
            type = MovementStateType.Idol;
        }
    }
}
