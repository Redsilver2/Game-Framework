using RedSilver2.Framework.Inputs.Configurations;
using RedSilver2.Framework.Inputs.Settings;
using UnityEngine;

namespace RedSilver2.Framework.StateMachines.Controllers
{
    [RequireComponent(typeof(Rigidbody))]
    public class PlayerRigidbodyController : PlayerMovementController
    {

        protected override void SetStateMachineController(MovementStateMachineController controller, MovementInputSettings settings)
        {
            controller?.SetStateMachine(new PlayerRigidbodyMovementStateMachine(controller, settings));
        }
    }
}
