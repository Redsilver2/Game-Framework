using RedSilver2.Framework.StateMachines.Controllers;


namespace RedSilver2.Framework.StateMachines.Events
{
    public sealed class KeyboardMovementTiltMotion : MovementTiltMotion
    {
        protected sealed override void SetStateMachineEvents(PlayerMovementStateMachine stateMachine, bool isAddingEvents) {
            if (isAddingEvents) {
                stateMachine?.AddOnLateUpdateListener(OnLateUpdate);
                stateMachine?.AddOnMoveInputUpdateListener(OnInputUpdate);
            }
            else {
                stateMachine?.RemoveOnLateUpdateListener(OnLateUpdate);
                stateMachine?.RemoveOnMoveInputUpdateListener(OnInputUpdate);
            }
        }
    }
}
