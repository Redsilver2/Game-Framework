

using RedSilver2.Framework.StateMachines.Extensions;

namespace RedSilver2.Framework.StateMachines.States
{
    [System.Serializable]
    public abstract partial class LandState : MovementState
    {
        private const string SOUND_EVENT = "Land Sound";
        public const MovementStateType TYPE = MovementStateType.Land;
    
        public LandState(MovementStateMachine stateMachine) : base(stateMachine) {

        }

        public sealed override bool CanTransition()
        {
            MovementStateMachine movementStateMachine = GetMovementStateMachine(this);

            if (movementStateMachine == null) return false;
            return base.CanTransition() && movementStateMachine.IsGrounded;
        }

        protected override void OnEntered()
        {
            base.OnEntered();

            MovementStateMachine movementStateMachine = GetMovementStateMachine(this);
            movementStateMachine?.ResetAirbornTime();

            FallState.GetState(movementStateMachine)?.ResetFallSpeed();
            JumpState.GetState(movementStateMachine)?.ResetJumpCount();
        }

        protected sealed override void SetMovementStateType(ref MovementStateType type) {
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

    public abstract partial class LandState : MovementState
    {
#if UNITY_EDITOR
        public override void Validate()
        {
            base.Validate();
            CanShowBaseSettings = false;
        }
#endif
    }
}