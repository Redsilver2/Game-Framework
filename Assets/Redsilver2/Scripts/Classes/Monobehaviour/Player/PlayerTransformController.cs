using RedSilver2.Framework.Inputs.Configurations;

namespace RedSilver2.Framework.StateMachines.Controllers
{
    public class PlayerTransformController : PlayerMovementController
    {
        protected override void SetStateMachineController(MovementStateMachineController controller, KeyboardVector2InputConfiguration configuration)
        {
            if (controller == null || controller == null) return;
            controller?.SetStateMachine(new PlayerTransformMovementStateMachine(controller, configuration));
        }
    }
}
