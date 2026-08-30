using RedSilver2.Framework.StateMachines.Controllers;
using RedSilver2.Framework.StateMachines.States;

namespace RedSilver2.Framework.StateMachines.Events
{
    public abstract class PlayerMovementStateEvent : MovementStateEvent
    {
        protected sealed override void Add(MovementState state, MovementStateMachine stateMachine)
        {
            Add(state, stateMachine as PlayerMovementStateMachine);
        }

        protected sealed override void Remove(MovementState state, MovementStateMachine stateMachine)
        {
            Remove(state, stateMachine as PlayerMovementStateMachine);
        }

        protected sealed override bool IsValid(MovementState state, MovementStateMachine stateMachine)
        {
            if(base.IsValid(state, stateMachine))
               return IsValid(state, stateMachine as PlayerMovementStateMachine);

            return false;
        }

        private bool IsValid(MovementState state, PlayerMovementStateMachine stateMachine)
        {
            return state != null && stateMachine != null;
        }

        protected abstract void Add(MovementState state, PlayerMovementStateMachine stateMachine);
        protected abstract void Remove(MovementState state, PlayerMovementStateMachine stateMachine);
    }
}
