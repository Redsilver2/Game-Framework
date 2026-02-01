namespace RedSilver2.Framework.StateMachines.States
{
    public sealed class JumpState : PlayerState
    {
        public JumpState(PlayerStateMachine owner) : base(owner) {
            MovementHandler?.EnableJumpInputUpdate();

            AddOnStateEnteredListener(() => {
                if (owner == null || owner.Controller == null) return;
                MovementHandler?.SetJumpHeight(owner.Controller.JumpForce);
            });

            AddOnStateRemovedListener(() => {
                MovementHandler?.DisableJumpInputUpdate();
            });
        }

        public sealed override bool IsValidTransition() {
            if (MovementHandler == null) return false;
            return MovementHandler.IsGrounded && MovementHandler.IsJumping;
        }

        protected sealed override void AddRequiredTransitionStates(PlayerStateMachine stateMachine) {
             if(stateMachine == null) return;
            if (!stateMachine.ContainsState(PlayerStateType.Fall) && IsValidTransitionState(PlayerStateType.Fall)) {
                new FallState(stateMachine);
                AddTransitionState(stateMachine.GetState(PlayerStateType.Fall));
            }
        }

        protected sealed override void SetIncompatibleStateTransitions(ref PlayerStateType[] results) {
            results = GetStateTypes(new PlayerStateType[] { PlayerStateType.Fall });
        }

        protected sealed override void SetPlayerStateType(ref PlayerStateType type) {
            type = PlayerStateType.Jump;
        }
    }
}
