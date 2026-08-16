
using UnityEngine;

namespace RedSilver2.Framework.StateMachines.States
{
    [System.Serializable]
    public sealed class LandState : MovementState
    {
        public const MovementStateType TYPE = MovementStateType.Land;

        public LandState() : base()
        {
        }


#if UNITY_EDITOR
        public void Validate(MovementStateMachine stateMachine)
        {
            SetStateMachine(stateMachine);
            Validate();
        }
#endif

        public sealed override bool CanTransition()
        {
            if (MovementStateMachine == null) return false;
            return base.CanTransition() && MovementStateMachine.IsGrounded;
        }

        protected override void OnEntered()
        {
            base.OnEntered();
            JumpState.GetState(MovementStateMachine)?.ResetJumpCount();
        }

        protected sealed override void OnUpdate() {
            base.OnUpdate();

            if (MovementStateMachine == null) return;
            MovementStateMachine?.SetFallSpeed(MovementStateMachine.DefaultFallSpeed);
        }

        protected override void SetMovementStateType(ref MovementStateType type) {
            type = TYPE;
        }



        private void SetStateMachine(MovementStateMachine stateMachine)
        {
            this.MovementStateMachine = stateMachine;
            SetStateMachine(stateMachine as UpdatableStateMachine);
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