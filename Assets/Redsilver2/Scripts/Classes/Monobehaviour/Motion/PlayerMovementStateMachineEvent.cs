using RedSilver2.Framework.StateMachines.Controllers;
using UnityEngine;


namespace RedSilver2.Framework.StateMachines.Events {
    public abstract class PlayerMovementStateMachineEvent : MovementStateMachineEvent {
        protected PlayerMovementStateMachineEvent(string name, PlayerMovementStateMachine stateMachine) : base(name, stateMachine) {

        }

        protected sealed override void Disable(MovementStateMachine stateMachine)
        {
            Disable(stateMachine as PlayerMovementStateMachine);
        }

        protected sealed override void Enable(MovementStateMachine stateMachine)
        {
            Enable(stateMachine as PlayerMovementStateMachine);
        }

        protected abstract void Disable(PlayerMovementStateMachine stateMachine);
        protected abstract void Enable(PlayerMovementStateMachine stateMachine);
    }
}
