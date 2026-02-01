namespace RedSilver2.Framework.StateMachines.States
{
    public sealed class RunState : PlayerState
    {
        public RunState(PlayerStateMachine owner) : base(owner) {
            MovementHandler?.EnableRunInputUpdate();

            AddOnStateRemovedListener(() => {
                MovementHandler?.DisableRunInputUpdate();
            });
        }

        public sealed override bool IsValidTransition() {
            return MovementHandler.IsGrounded && !MovementHandler.IsCrouching
                && MovementHandler.IsRunning  && MovementHandler.GetMoveInputValue().magnitude > 0f;
        }

        protected sealed override void AddRequiredTransitionStates(PlayerStateMachine stateMachine) {
            if (stateMachine == null) return;
            if (!stateMachine.ContainsState(PlayerStateType.Idol) && IsValidTransitionState(PlayerStateType.Idol)) {
                new IdolState(stateMachine);
                AddTransitionState(stateMachine.GetState(PlayerStateType.Idol));
            }
        }

        protected sealed override void SetIncompatibleStateTransitions(ref PlayerStateType[] results) {
            results = GetStateTypes(new PlayerStateType[] { PlayerStateType.Walk, PlayerStateType.Idol, PlayerStateType.Fall, PlayerStateType.Crouch, PlayerStateType.Jump });
        }

        protected sealed override void SetPlayerStateType(ref PlayerStateType type) {
            type = PlayerStateType.Run;
        }
    }
}                                                                                                                                                                                                                                         
