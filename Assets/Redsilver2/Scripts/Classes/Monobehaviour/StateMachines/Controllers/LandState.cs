using RedSilver2.Framework.StateMachines.Controllers;

namespace RedSilver2.Framework.StateMachines.States
{
    public class LandState : MovementState
    {
        public const MovementStateType TYPE = MovementStateType.Land;

        protected sealed override bool CanTransition(MovementStateMachine stateMachine)
        {
            if (stateMachine == null || stateMachine.IsCurrentState(TYPE)) return false;
            return stateMachine.IsGrounded && stateMachine.IsCurrentState(FallState.TYPE);
        }

        protected sealed override void OnUpdate(MovementStateMachine stateMachine) {
            if (stateMachine == null) return;
            stateMachine?.SetFallSpeed(-10f);
        }

        protected override void SetMovementStateType(ref MovementStateType type) {
            type = TYPE;
        }

        public static LandState GetState(MovementStateMachine stateMachine)
        {
            if (stateMachine == null) return null;
            return stateMachine.GetState(TYPE) as LandState;
        }

#if UNITY_EDITOR
        protected override MovementStateType[] GetDefaultInvalidTypes()
        {
            return new MovementStateType[] { TYPE, FallState.TYPE, JumpState.TYPE };
        }
#endif
    }
}