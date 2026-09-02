using RedSilver2.Framework.StateMachines.Controllers;
using UnityEngine;


namespace RedSilver2.Framework.StateMachines.States
{
    [System.Serializable]
    public class PlayerFallState : FallState
    {
        public PlayerFallState(PlayerMovementStateMachine stateMachine) : base(stateMachine) {

        }

        public static PlayerFallState GetState(PlayerMovementStateMachine stateMachine)
        {
            return GetState(stateMachine as MovementStateMachine) as PlayerFallState;
        }
    }
}
