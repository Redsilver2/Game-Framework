using RedSilver2.Framework.Player;
using RedSilver2.Framework.StateMachines.Controllers;
using UnityEngine;

namespace RedSilver2.Framework.StateMachines.Events
{
    public class MouseMovementTiltMotion : MovementTiltMotion
    {
        protected sealed override void SetStateMachineEvents(PlayerMovementStateMachine stateMachine, bool isAddingEvents)
        {
            CameraController controller = stateMachine != null ? stateMachine.CameraController : null;

            if (isAddingEvents) {
                controller?.AddOnLateUpdateListener(OnLateUpdate);
                controller?.AddOnUpdateListener(OnInputUpdate);
            }
            else {
                controller?.RemoveOnLateUpdateListener(OnLateUpdate);
                controller?.RemoveOnUpdateListener(OnInputUpdate);
            }
        }

        protected sealed override void UpdateRotation(Vector2 input, ref Vector3 desired)
        {
            base.UpdateRotation(input, ref desired);
            float x = desired.y, y = desired.x;

            desired.x = y;
            desired.y = Original.y;
            desired.z = x;
        }
    }
}
