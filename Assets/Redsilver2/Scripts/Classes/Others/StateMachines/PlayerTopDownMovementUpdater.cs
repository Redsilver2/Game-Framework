using RedSilver2.Framework.StateMachines.Controllers;
using RedSilver2.Framework.StateMachines.States;
using UnityEngine;

namespace RedSilver2.Framework.StateMachines.Events {
    [System.Serializable]
    public abstract class PlayerTopDownMovementUpdater : PlayerMovementHandler
    {

        protected PlayerTopDownMovementUpdater(PlayerMovementStateMachine stateMachine) : base(stateMachine) {

        }


        protected override Vector3 GetNextPosition(PlayerMovementStateMachine stateMachine) {
            if (stateMachine == null) return Vector3.zero;
          
            bool    is2DMovement = Is2DMovement();
            Vector3 input    = stateMachine.MoveInput;

            float moveSpeed = stateMachine.MoveSpeed;
            float fallSpeed = stateMachine.FallSpeed;

            input.Normalize();

            return Time.deltaTime * (stateMachine.transform.right   * input.x * stateMachine.MoveSpeed +
                                     stateMachine.transform.up      * (is2DMovement ? (input.y * stateMachine.MoveSpeed) : FallState.GetFallSpeed(stateMachine)) +
                                     stateMachine.transform.forward * (is2DMovement ? 0f : input.y * stateMachine.MoveSpeed));
        }

        protected abstract bool Is2DMovement();
    }
}
