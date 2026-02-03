using RedSilver2.Framework.StateMachines.States.Movement;

namespace RedSilver2.Framework.StateMachines.States
{
    public sealed class RunState : MovementState
    {
        public RunState(MovementStateMachine owner) : base(owner) {
   
        }

        public sealed override bool IsValidTransition() {
            return MovementHandler.IsGrounded && !MovementHandler.IsCrouching
                && MovementHandler.IsRunning  && MovementHandler.GetMoveMagnitude() > 0f;
        }

        protected sealed override void AddRequiredTransitionStates(MovementStateMachine stateMachine) {
            if (stateMachine == null) return;
            if (!stateMachine.ContainsState(MovementStateType.Idol) && IsValidTransitionState(MovementStateType.Idol)) {
                new IdolState(stateMachine);
                AddTransitionState(stateMachine.GetState(MovementStateType.Idol));
            }
        }

        protected sealed override void SetIncompatibleStateTransitions(ref MovementStateType[] results) {
            results = GetExcludedStateTypes(new MovementStateType[] { MovementStateType.Walk, MovementStateType.Idol, MovementStateType.Fall, MovementStateType.Crouch, MovementStateType.Jump });
        }

        protected override void SetPlayerInputsEvents(PlayerMovementHandler handler)
        {
            if(handler == null) return;
            handler?.EnableRunInputUpdate();

            AddOnStateRemovedListener(() => {
                handler?.DisableRunInputUpdate();
            });
        }

        protected sealed override void SetPlayerStateType(ref MovementStateType type) {
            type = MovementStateType.Run;
        }
    }
}                                                                                                                                                                                                                                         
