using RedSilver2.Framework.StateMachines.States.Movement;

namespace RedSilver2.Framework.StateMachines.States
{
    public sealed class JumpState : MovementState
    {
        public JumpState(MovementStateMachine owner) : base(owner) {
            AddOnStateEnteredListener(() => {
                MovementHandler?.SetJumpHeight(50f);
            });
        }

        public sealed override bool IsValidTransition() {
            if (MovementHandler == null) return false;
            return MovementHandler.IsGrounded && MovementHandler.IsJumping;
        }

        protected sealed override void AddRequiredTransitionStates(MovementStateMachine stateMachine) {
             if(stateMachine == null) return;
            if (!stateMachine.ContainsState(MovementStateType.Fall) && IsValidTransitionState(MovementStateType.Fall)) {
                new FallState(stateMachine);
                AddTransitionState(stateMachine.GetState(MovementStateType.Fall));
            }
        }

        protected sealed override void SetIncompatibleStateTransitions(ref MovementStateType[] results) {
            results = GetExcludedStateTypes(new MovementStateType[] { MovementStateType.Fall });
        }

        protected sealed override void SetPlayerInputsEvents(PlayerMovementHandler handler)
        {
            if (handler == null) return;
            handler?.EnableJumpInputUpdate();

            AddOnStateRemovedListener(() => {
                handler?.DisableJumpInputUpdate();
            });
        }

        protected sealed override void SetPlayerStateType(ref MovementStateType type) {
            type = MovementStateType.Jump;
        }
    }
}
