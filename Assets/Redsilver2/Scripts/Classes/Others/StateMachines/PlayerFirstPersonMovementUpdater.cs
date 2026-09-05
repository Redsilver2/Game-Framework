using RedSilver2.Framework.Player;
using RedSilver2.Framework.StateMachines.Controllers;
using RedSilver2.Framework.StateMachines.States;
using UnityEngine;


namespace RedSilver2.Framework.StateMachines.Events
{
    [System.Serializable]
    public class PlayerFirstPersonMovementUpdater : PlayerMovementHandler
    {
        public PlayerFirstPersonMovementUpdater(PlayerMovementStateMachine stateMachine) : base(stateMachine)
        {
            SetCameraController(new FPSCameraController());
        }

        protected override Vector3 GetNextPosition(PlayerMovementStateMachine stateMachine)
        {
            if (stateMachine == null) return Vector3.zero;

            Vector3 moveInput = stateMachine.MoveInput;
            float moveSpeed = stateMachine.MoveSpeed;

            moveInput.Normalize();

            return Time.deltaTime * (Vector3.right   * moveInput.x * moveSpeed              +
                                     Vector3.up      * FallState.GetFallSpeed(stateMachine) +
                                     Vector3.forward * moveInput.y * moveSpeed);
        }
    }
}