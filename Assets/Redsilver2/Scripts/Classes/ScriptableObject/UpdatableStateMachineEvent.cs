using UnityEngine;

namespace RedSilver2.Framework.StateMachines.Events
{
    public abstract class UpdatableStateMachineEvent : StateMachineEvent
    {
        protected UpdatableStateMachineEvent(string name, UpdatableStateMachine stateMachine) : base(name, stateMachine) {
       
        }

        protected sealed override void Enable(StateMachine stateMachine)
        {
            Enable(stateMachine as UpdatableStateMachine);
        }

        protected sealed override void Disable(StateMachine stateMachine)
        {
            Disable(stateMachine as UpdatableStateMachine);
        }

        protected abstract void Enable(UpdatableStateMachine stateMachine);
        protected abstract void Disable(UpdatableStateMachine stateMachine);
    }
}
