using RedSilver2.Framework.StateMachines.Controllers;

namespace RedSilver2.Framework.StateMachines.States
{
    public class LandState : MovementState
    {
        public const MovementStateType TYPE = MovementStateType.Land;

        public sealed override bool CanTransition(MovementStateMachine stateMachine)
        {
            if (stateMachine == null) return false;
            return stateMachine.IsGrounded;
        }

        protected sealed override void OnUpdate(MovementStateMachine stateMachine) {
            if (stateMachine == null) return;
            stateMachine?.SetFallSpeed(stateMachine.DefaultFallSpeed);
        }

        protected override void SetMovementStateType(ref MovementStateType type) {
            type = TYPE;
        }

        public static LandState GetState(MovementStateMachine stateMachine)
        {
            if (stateMachine == null) return null;
            return stateMachine.GetState(TYPE) as LandState;
        }

        protected sealed override MovementStateType[] GetDefaultInvalidTypes()
        {
            return new MovementStateType[] { TYPE, FallState.TYPE, JumpState.TYPE };
        }
    }
}