
namespace RedSilver2.Framework.StateMachines.States {
    public sealed class IdolState : PlayerState
    {
        public IdolState(PlayerStateMachine owner) : base(owner) {
            
        }

        public sealed override bool IsValidTransition() {
            if (MovementHandler == null) return false;

            return MovementHandler.IsGrounded && !MovementHandler.IsCrouching
                    && !MovementHandler.IsRunning && MovementHandler.GetMoveInputValue().magnitude == 0f;
        }

        protected sealed override void AddRequiredTransitionStates(PlayerStateMachine stateMachine) {
           
        }

        protected sealed override void SetIncompatibleStateTransitions(ref PlayerStateType[] results) {
            results = GetStateTypes(new PlayerStateType[] { PlayerStateType.Walk, PlayerStateType.Run, PlayerStateType.Fall, 
                                                            PlayerStateType.Jump, PlayerStateType.Crouch });
        }

        protected sealed override void SetPlayerStateType(ref PlayerStateType type) {
            type = PlayerStateType.Idol;
        }
    }
}
