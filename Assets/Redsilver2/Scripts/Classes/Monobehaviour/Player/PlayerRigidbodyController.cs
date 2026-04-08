using RedSilver2.Framework.Inputs.Configurations;
using UnityEngine;

namespace RedSilver2.Framework.StateMachines.Controllers
{
    [RequireComponent(typeof(Rigidbody))]
    public class PlayerRigidbodyController : PlayerMovementController
    {

        protected override void SetStateMachineController(MovementStateMachineController controller, KeyboardVector2InputConfiguration configuration)
        {
            controller?.SetStateMachine(new PlayerRigidbodyMovementStateMachine(controller, configuration));
        }
    }
}
