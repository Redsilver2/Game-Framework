using UnityEngine;

namespace RedSilver2.Framework.StateMachines.Controllers {
    public class PlayerMovementStateMachineController : MovementStateMachineController
    {
        public void SetStateMachine(PlayerMovementStateMachine stateMachine) {
            SetStateMachine(stateMachine as StateMachine);
        }
    }
}