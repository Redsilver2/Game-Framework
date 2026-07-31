using RedSilver2.Framework.StateMachines.Controllers;
using UnityEngine;

namespace RedSilver2.Framework.StateMachines.States
{
    [RequireComponent(typeof(IdolState))]
    public sealed class WalkState : MovementState
    {
        [Space]
        [SerializeField] private float walkSpeed;
        [SerializeField] private float moveTransitionSpeed;

        public const MovementStateType TYPE = MovementStateType.Walk;

        protected sealed override bool CanTransition(MovementStateMachine stateMachine) {
            if (!base.CanTransition(stateMachine)) return false;
            return stateMachine.IsMoving && !RunState.IsStateMachineRunning(stateMachine)
                   && !CrouchState.IsStateMachineCrouching(stateMachine) && stateMachine.IsGrounded;
        }

        protected sealed override void OnUpdate(MovementStateMachine stateMachine) {
            if (stateMachine == null) return;
            stateMachine?.SetMoveSpeed(walkSpeed, moveTransitionSpeed);
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


#if UNITY_EDITOR
        protected sealed override MovementStateType[] GetDefaultInvalidTypes()
        {
            return new MovementStateType[] { TYPE, LandState.TYPE };
        }
#endif
    }
}
