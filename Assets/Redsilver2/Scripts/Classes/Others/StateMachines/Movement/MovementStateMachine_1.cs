using RedSilver2.Framework.Inputs.Configurations;
using RedSilver2.Framework.StateMachines.Controllers;
using UnityEngine;

namespace RedSilver2.Framework.StateMachines
{
    public abstract class PlayerMovementStateMachine : MovementStateMachine
    {
        private KeyboardVector2InputConfiguration configuration;

        protected PlayerMovementStateMachine(MovementStateMachineController controller, KeyboardVector2InputConfiguration configuration) : base(controller) {
            this.configuration = configuration;
        }

        protected override void Move()
        {
            Vector2 inputValue = configuration == null ? Vector2.zero : configuration.Value;
            inputValue.Normalize();

            if (Transform != null) {
                float moveSpeed = GetMoveSpeed(), fallSpeed = GetFallSpeed();
                Vector3 nextPosition = Time.deltaTime * ((Transform.right   * moveSpeed * inputValue.x) + 
                                                         (Transform.up      * fallSpeed)
                                                       + (Transform.forward * moveSpeed * inputValue.y));

                Move(nextPosition, fallSpeed, moveSpeed);
            }
        }

    }
}
