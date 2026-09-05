using RedSilver2.Framework.StateMachines.Controllers;
using UnityEngine;

namespace RedSilver2.Framework.StateMachines.Events
{
    public abstract class MovementMotion : PlayerMovementStateMachineEvent {
        protected MovementMotion(string name, PlayerMovementStateMachine stateMachine) : base(name, stateMachine)
        {
        }

        protected abstract void OnInputUpdate(Vector2 input);
        protected abstract void OnLateUpdate();
    }
}
