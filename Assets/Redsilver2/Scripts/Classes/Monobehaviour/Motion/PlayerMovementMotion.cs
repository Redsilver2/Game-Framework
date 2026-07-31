using RedSilver2.Framework.StateMachines.Controllers;
using UnityEngine;


namespace RedSilver2.Framework.StateMachines.Events
{
    public abstract class PlayerMovementMotion : PlayerMovementStateMachineEvent {
        protected override void SetStateMachineEvents(PlayerMovementStateMachine stateMachine, bool isAddingEvents)
        {
            if (isAddingEvents) {
                stateMachine?.AddOnMoveInputUpdateListener(OnMoveInputUpdate);
                stateMachine?.AddOnLateUpdateListener(OnLateUpdate);
            }
            else {
                stateMachine?.RemoveOnMoveInputUpdateListener(OnMoveInputUpdate);
                stateMachine?.RemoveOnLateUpdateListener(OnLateUpdate);
            }
        }

        protected abstract void OnMoveInputUpdate(Vector2 input);
        protected abstract void OnLateUpdate();
    }
}
