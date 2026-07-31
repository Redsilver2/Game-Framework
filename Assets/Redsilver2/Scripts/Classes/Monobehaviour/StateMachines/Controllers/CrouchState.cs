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


        private bool isCrouching;
        public bool IsCrouching => isCrouching;

        public const MovementStateType TYPE = MovementStateType.Crouch;

        protected void SetIsCrouching(bool isCrouching) { this.isCrouching = isCrouching; }

        protected override void OnEntered()
        {
            base.OnEntered();
            Debug.Log("Crouch");

        }

        protected sealed override bool CanTransition(MovementStateMachine stateMachine) {
            return base.CanTransition(stateMachine) && IsStateMachineCrouching(stateMachine);
        }

        protected sealed override void OnUpdate(MovementStateMachine stateMachine) {
            stateMachine?.SetMoveSpeed(crouchMoveSpeed, crouchMoveTransitionSpeed);
            stateMachine?.SetHeight(crouchHeight, crouchHeightTransitionSpeed);
        }

        protected sealed override void SetMovementStateType(ref MovementStateType type) {
            type = TYPE;
        }

        protected override void OnDisabled()
        {
            base.OnDisabled();
            isCrouching = false;
        }

#if UNITY_EDITOR
        protected sealed override MovementStateType[] GetDefaultInvalidTypes()
        {
            return new MovementStateType[] { TYPE, LandState.TYPE, FallState.TYPE };
        }
#endif


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
