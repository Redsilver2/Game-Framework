using RedSilver2.Framework.StateMachines.Controllers;
using UnityEngine;

namespace RedSilver2.Framework.StateMachines.States
{
    public abstract class CrouchState : MovementState
    {
        [Space]
        [SerializeField] private float crouchMoveSpeed;
        [SerializeField] private float crouchMoveTransitionSpeed;

        [Space]
        [SerializeField] private float crouchHeight;
        [SerializeField] private float crouchHeightTransitionSpeed;

        [Space]
        [SerializeField] private float unCrouchSafetyCheckDistance;

        private bool isCrouching;
        public bool IsCrouching => isCrouching;

        public const MovementStateType TYPE = MovementStateType.Crouch;

        protected void SetIsCrouching(bool isCrouching) { this.isCrouching = isCrouching; }

        public sealed override bool CanTransition(MovementStateMachine stateMachine) {
            return base.CanTransition(stateMachine) && IsStateMachineCrouching(stateMachine);
        }

        protected sealed override void OnUpdate(MovementStateMachine stateMachine) {
            stateMachine?.SetMoveSpeed(crouchMoveSpeed, crouchMoveTransitionSpeed);
            stateMachine?.SetHeight(crouchHeight, crouchHeightTransitionSpeed);
        }

        protected sealed override void SetMovementStateType(ref MovementStateType type) {
            type = TYPE;
        }

        protected override void OnDisabled(MovementStateMachine stateMachine)
        {
            base.OnDisabled(stateMachine);
            isCrouching = false;
        }

        protected sealed override MovementStateType[] GetDefaultInvalidTypes()
        {
            return new MovementStateType[] { TYPE, LandState.TYPE, FallState.TYPE };
        }

        protected sealed override void UpdateStateTransitions()
        {
            if (!Physics.Raycast(transform.position, transform.up, out RaycastHit hit, unCrouchSafetyCheckDistance, ~(1 << GetLayerToIgnore()))) {
                base.UpdateStateTransitions();
            }
        }

        protected abstract int GetLayerToIgnore();

        public static CrouchState GetState(MovementStateMachine stateMachine)
        {
            if(stateMachine == null) return null;
            return stateMachine?.GetState(TYPE) as CrouchState;
        }

        public static bool IsStateMachineCrouching(MovementStateMachine stateMachine){
            CrouchState state = GetState(stateMachine);
            return state != null ? state.IsCrouching : false;   
        }
    }
}
