using RedSilver2.Framework.StateMachines.States.Movement;

namespace RedSilver2.Framework.StateMachines.Controllers
{
    public class PlayerTransformController : PlayerController
    {
        protected sealed override PlayerMovementHandler GetMovementHandler() {
            return new PlayerTransformMovementHandler(this);
        }

        protected override PlayerStateMachine GetPlayerStateMachine(PlayerMovementHandler movementHandler) {
            return new PlayerStateMachine(this, movementHandler);
        }
    }
}
