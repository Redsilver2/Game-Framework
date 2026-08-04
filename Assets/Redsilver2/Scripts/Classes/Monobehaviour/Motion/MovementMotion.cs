using UnityEngine;

namespace RedSilver2.Framework.StateMachines.Events
{
    public abstract class MovementMotion : PlayerMovementStateMachineEvent {
        protected abstract void OnInputUpdate(Vector2 input);
        protected abstract void OnLateUpdate();
    }
}
