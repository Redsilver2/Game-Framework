using RedSilver2.Framework.Inputs.Configurations;
using RedSilver2.Framework.Inputs.Settings;
using RedSilver2.Framework.StateMachines.Controllers;
using UnityEngine;

namespace RedSilver2.Framework.StateMachines
{
    public class PlayerTransformMovementStateMachine : PlayerMovementStateMachine
    {
        public PlayerTransformMovementStateMachine(MovementStateMachineController controller, MovementInputSettings settings) : base(controller, settings) {

        }

        public sealed override void Move(Vector3 nextPosition)
        {
            if (Transform != null)
                Transform.localPosition += nextPosition;

            base.Move(Transform == null ? Vector3.zero : nextPosition);
        }
    }
}
