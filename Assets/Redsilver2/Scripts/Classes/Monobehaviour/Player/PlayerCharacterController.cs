
using RedSilver2.Framework.Inputs.Settings;
using UnityEngine;

namespace RedSilver2.Framework.StateMachines.Controllers
{
    [RequireComponent(typeof(CharacterController))]

    public class PlayerCharacterController : PlayerMovementController
    {
        protected sealed override void SetStateMachineController(PlayerMovementStateMachineController controller, MovementInputSettings inputSettings) {
            if (controller == null || inputSettings == null) return;
            controller?.SetStateMachine(new PlayerCharacterControllerMovementStateMachine(controller, inputSettings));
        }

    }
}
