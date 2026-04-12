
using RedSilver2.Framework.Inputs.Settings;
using UnityEngine;

namespace RedSilver2.Framework.StateMachines.Controllers
{
    [RequireComponent(typeof(CharacterController))]

    public class PlayerCharacterController : PlayerMovementController
    {
        protected override void SetStateMachineController(MovementStateMachineController controller, MovementInputSettings inputSettings) {
            
            controller?.SetStateMachine(new PlayerCharacterControllerMovementStateMachine(controller, inputSettings));
        }
    }
}
