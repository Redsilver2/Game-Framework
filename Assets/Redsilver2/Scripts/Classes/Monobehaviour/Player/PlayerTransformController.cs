using RedSilver2.Framework.StateMachines.States.Movement;

namespace RedSilver2.Framework.StateMachines.Controllers
{
    public class PlayerTransformController : PlayerController
    {
        protected sealed override PlayerMovementHandler GetPlayerMovementHandler() {
            return new PlayerTransformMovementHandler(this);
        }
    }
}
