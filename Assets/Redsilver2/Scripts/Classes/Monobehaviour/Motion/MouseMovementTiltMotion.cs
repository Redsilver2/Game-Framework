using RedSilver2.Framework.Player;
using RedSilver2.Framework.StateMachines.Controllers;

namespace RedSilver2.Framework.StateMachines.Events
{
    public class MouseMovementTiltMotion : MovementTiltMotion
    {
        protected sealed override void SetStateMachineEvents(PlayerMovementStateMachine stateMachine, bool isAddingEvents)
        {
            CameraController controller = stateMachine != null ? stateMachine.CameraController : null;

            if (isAddingEvents)  {
                controller?.AddOnLateUpdateListener(OnLateUpdate);
                controller?.AddOnUpdateListener(OnInputUpdate);
            }
            else {
                controller?.RemoveOnLateUpdateListener(OnLateUpdate);
                controller?.RemoveOnUpdateListener(OnInputUpdate);
            }
        }
    }
}
