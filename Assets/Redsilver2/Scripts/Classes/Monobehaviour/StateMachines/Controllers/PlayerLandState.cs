using RedSilver2.Framework.StateMachines.Controllers;

namespace RedSilver2.Framework.StateMachines.States
{
    [System.Serializable]
    public class PlayerLandState : LandState
    {
        public PlayerLandState(PlayerMovementStateMachine stateMachine) : base(stateMachine) {

        }

        public static PlayerLandState GetState(PlayerMovementStateMachine stateMachine)
        {
            return GetState(stateMachine as MovementStateMachine) as PlayerLandState;
        }
    }
}