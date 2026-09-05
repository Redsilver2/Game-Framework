using RedSilver2.Framework.Player;
using RedSilver2.Framework.StateMachines.Controllers;
using RedSilver2.Framework.StateMachines.States;
using UnityEngine;
using UnityEngine.Events;

namespace RedSilver2.Framework.StateMachines.Events
{
    public abstract class PlayerSideScrollerMovementUpdater : PlayerMovementHandler
    {
        protected PlayerSideScrollerMovementUpdater(PlayerMovementStateMachine stateMachine) : base(stateMachine) {
            
        }

        protected override Vector3 GetNextPosition(PlayerMovementStateMachine stateMachine) {
            if(stateMachine == null) return Vector3.zero;
           
            Vector3 moveInput = stateMachine.MoveInput;
            moveInput.Normalize();

            return Time.deltaTime * (stateMachine.transform.right * moveInput.x * stateMachine.MoveSpeed +
                                     stateMachine.transform.up    * FallState.GetFallSpeed(stateMachine));
        }
    }
}
