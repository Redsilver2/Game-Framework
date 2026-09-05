using RedSilver2.Framework.StateMachines.Controllers;
using UnityEngine;


namespace RedSilver2.Framework.StateMachines.Events {
    public abstract class PlayerMovementStateMachineEvent : MovementStateMachineEvent {
        protected PlayerMovementStateMachineEvent(string name, PlayerMovementStateMachine stateMachine) : base(name, stateMachine) {

        }
    }
}
