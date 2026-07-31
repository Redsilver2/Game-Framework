using RedSilver2.Framework.StateMachines.Controllers;
using UnityEngine;
using UnityEngine.Events;

namespace RedSilver2.Framework.StateMachines.Events
{
    public abstract class MovementStateMachineEvent : UpdatableStateMachineEvent
    {
        protected override void SetStateMachineEvents(UpdatableStateMachine stateMachine, bool isAddingEvents) {
            SetStateMachineEvents(stateMachine as MovementStateMachine, isAddingEvents);
        }

        protected abstract void SetStateMachineEvents(MovementStateMachine stateMachine, bool isAddingEvents);
    }
}
