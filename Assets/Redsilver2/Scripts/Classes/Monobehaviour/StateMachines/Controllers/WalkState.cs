using UnityEngine;

namespace RedSilver2.Framework.StateMachines.States
{
    [System.Serializable]
    public abstract class WalkState : MovementState
    {
        [Space]
        [SerializeField] private float walkSpeed;
        [SerializeField] private float moveTransitionSpeed;

        public const MovementStateType TYPE = MovementStateType.Walk;

        public WalkState() : base() {
           
        }


#if UNITY_EDITOR
        protected override void Validate()
        {
            base.Validate();
            walkSpeed = Mathf.Clamp(walkSpeed, 0f, float.MaxValue);
            moveTransitionSpeed = Mathf.Clamp(moveTransitionSpeed, 0f, float.MaxValue);
        }
#endif

        public sealed override bool CanTransition() {
            if (MovementStateMachine == null) return false;

            return MovementStateMachine.IsMoving && !RunState.IsStateMachineRunning(MovementStateMachine)
                   && !CrouchState.IsStateMachineCrouching(MovementStateMachine) && MovementStateMachine.IsGrounded;
        }

        protected sealed override void OnUpdate() {
            base.OnUpdate();
            MovementStateMachine?.SetMoveSpeed(walkSpeed, moveTransitionSpeed);
        }

        protected sealed override void SetMovementStateType(ref MovementStateType type) {
            type = TYPE;
        }

        public void SetWalkSpeed(float walkSpeed) {
            this.walkSpeed = Mathf.Clamp(walkSpeed, 0f, float.MaxValue);
        }

        public void SetTransitionSpeed(float transitionSpeed) {
            this.moveTransitionSpeed = Mathf.Clamp(transitionSpeed, 0f, float.MaxValue);
        }

        public static WalkState GetState(MovementStateMachine stateMachine) { 
            if(stateMachine == null) return null;
            return stateMachine.GetState(TYPE) as WalkState;
        }


        protected sealed override MovementStateType[] GetDefaultInvalidTypes()
        {
            return new MovementStateType[] { TYPE, LandState.TYPE };
        }
    }
}
