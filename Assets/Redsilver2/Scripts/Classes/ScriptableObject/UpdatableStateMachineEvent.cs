using UnityEngine;

namespace RedSilver2.Framework.StateMachines.Events
{
    public abstract class UpdatableStateMachineEvent : StateMachineEvent
    {
        protected override void SetStateMachineEvents(StateMachine stateMachine, bool isAddingEvents) {
            SetStateMachineEvents(stateMachine as UpdatableStateMachine, isAddingEvents);
        }

        protected abstract void SetStateMachineEvents(UpdatableStateMachine stateMachine, bool isAddingEvents);
    }
}
