using RedSilver2.Framework.StateMachines.Controllers;
using UnityEngine;


namespace RedSilver2.Framework.StateMachines.Events {
    public abstract class PlayerMovementStateMachineEvent : MovementStateMachineEvent
    {
        protected override void SetStateMachineEvents(MovementStateMachine stateMachine, bool isAddingEvents)
        {
            SetStateMachineEvents(stateMachine as PlayerMovementStateMachine, isAddingEvents);
        }

        protected abstract void SetStateMachineEvents(PlayerMovementStateMachine stateMachine, bool isAddingEvents);
    }
}
