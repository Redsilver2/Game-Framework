using RedSilver2.Framework.Inputs.Configurations;
using RedSilver2.Framework.StateMachines.Controllers;
using UnityEngine;


namespace RedSilver2.Framework.StateMachines
{
    public sealed class PlayerCharacterControllerMovementStateMachine : PlayerMovementStateMachine
    {
        private readonly CharacterController characterController;

        public PlayerCharacterControllerMovementStateMachine(MovementStateMachineController controller, KeyboardVector2InputConfiguration configuration) : base(controller, configuration)
        {
            this.characterController = controller == null ? null : controller.GetComponent<CharacterController>();
        }

        public sealed override void Move(Vector3 nextPosition)
        {
            characterController?.Move(nextPosition);
            base.Move(characterController == null ? Vector3.zero : nextPosition);
        }
    }
}
