using RedSilver2.Framework.Inputs.Configurations;
using RedSilver2.Framework.StateMachines.Controllers;
using UnityEngine;

namespace RedSilver2.Framework.StateMachines
{
    public sealed class PlayerRigidbodyMovementStateMachine : PlayerMovementStateMachine
    {
        private readonly Rigidbody Rigidbody;

        public PlayerRigidbodyMovementStateMachine(MovementStateMachineController controller, KeyboardVector2InputConfiguration configuration) : base(controller, configuration)
        {
            this.Rigidbody = controller == null ? null : Rigidbody.GetComponent<Rigidbody>();
        }

        public sealed override void Move(Vector3 nextPosition) {
            Rigidbody?.AddForce(nextPosition);
            base.Move(Rigidbody == null ? Vector3.zero : nextPosition);    
        }
    }
}
