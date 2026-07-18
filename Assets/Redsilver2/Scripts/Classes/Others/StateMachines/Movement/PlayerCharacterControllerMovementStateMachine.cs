using RedSilver2.Framework.Inputs.Configurations;
using RedSilver2.Framework.Inputs.Settings;
using RedSilver2.Framework.StateMachines.Controllers;
using Unity.VisualScripting;
using UnityEngine;


namespace RedSilver2.Framework.StateMachines
{
    public sealed class PlayerCharacterControllerMovementStateMachine : PlayerMovementStateMachine
    {
        private readonly CharacterController characterController;

        public PlayerCharacterControllerMovementStateMachine(PlayerMovementStateMachineController controller, MovementInputSettings settings) : base(controller, settings)
        {
            this.characterController = controller == null ? null : controller.GetOrAddComponent<CharacterController>();
        }

        public sealed override void Move(Vector3 nextPosition)
        {
            characterController?.Move(nextPosition);
            base.Move(characterController == null ? Vector3.zero : nextPosition);
        }
    }
}
