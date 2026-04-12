using RedSilver2.Framework.Inputs.Configurations;
using RedSilver2.Framework.Inputs.Settings;

namespace RedSilver2.Framework.StateMachines.Controllers
{
    public class PlayerTransformController : PlayerMovementController
    {
        protected override void SetStateMachineController(MovementStateMachineController controller, MovementInputSettings settings)
        {
            if (controller == null || controller == null) return;
            controller?.SetStateMachine(new PlayerTransformMovementStateMachine(controller, settings));
        }
    }
}
