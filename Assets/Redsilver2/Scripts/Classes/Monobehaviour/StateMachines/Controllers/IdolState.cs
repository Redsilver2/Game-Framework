using UnityEngine;

namespace RedSilver2.Framework.StateMachines.States
{
    [System.Serializable]
    public sealed class IdolState : MovementState
    {
        [Space]
        [SerializeField] private float moveSpeedTransition;
        public const MovementStateType TYPE = MovementStateType.Idol;
       
        public IdolState() : base() {  }


#if UNITY_EDITOR
        protected override void Validate()
        {
            base.Validate();
            moveSpeedTransition = Mathf.Clamp(moveSpeedTransition, 0f, float.MaxValue);
        }
#endif

        public sealed override bool CanTransition()
        {
            if (!base.CanTransition() || MovementStateMachine == null) return false;
           
            return !MovementStateMachine.IsMoving && MovementStateMachine.IsGrounded
                && !RunState.IsStateMachineRunning(MovementStateMachine) && !CrouchState.IsStateMachineCrouching(MovementStateMachine)
                && !JumpState.IsStateMachineJumping(MovementStateMachine);     
        }

        protected sealed override void OnUpdate() {
            base.OnUpdate();
            MovementStateMachine?.SetMoveSpeed(0f, moveSpeedTransition);
        }

        protected sealed override void SetMovementStateType(ref MovementStateType type) {
            type = TYPE;
        }

        public static IdolState GetState(MovementStateMachine stateMachine)
        {
            if(stateMachine == null) return null;
            return stateMachine.GetState(TYPE) as IdolState;
        }

        protected sealed override MovementStateType[] GetDefaultInvalidTypes()
        {
            return new MovementStateType[] { TYPE, LandState.TYPE };
        }
    }
}