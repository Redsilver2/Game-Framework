using RedSilver2.Framework.StateMachines.Controllers;

namespace RedSilver2.Framework.StateMachines.States {
    [System.Serializable]
    public class PlayerIdolState : IdolState
    {
        public PlayerIdolState(PlayerMovementStateMachine stateMachine) : base(stateMachine) {

        }

        public static PlayerIdolState GetState(PlayerMovementStateMachine stateMachine)
        {
            return GetState(stateMachine as MovementStateMachine) as PlayerIdolState;
        }
    }
}