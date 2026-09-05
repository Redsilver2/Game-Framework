using UnityEngine;

namespace RedSilver2.Framework.StateMachines.Events
{
    public abstract class MovementStateMachineEvent : UpdatableStateMachineEvent
    {
        protected MovementStateMachineEvent(string name, MovementStateMachine stateMachine) : base(name, stateMachine) {

        }

        protected sealed override void Disable(UpdatableStateMachine stateMachine)
        {
            Disable(stateMachine as MovementStateMachine);
        }

        protected sealed override void Enable(UpdatableStateMachine stateMachine)
        {
            Enable(stateMachine as MovementStateMachine);
        }

        protected abstract void Enable(MovementStateMachine stateMachine);
        protected abstract void Disable(MovementStateMachine stateMachine);
    }
}
