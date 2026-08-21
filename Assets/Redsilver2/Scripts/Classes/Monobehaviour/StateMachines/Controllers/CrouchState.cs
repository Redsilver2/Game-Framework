using RedSilver2.Framework.StateMachines.Controllers;
using UnityEngine;

namespace RedSilver2.Framework.StateMachines.States
{
    [System.Serializable]
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
        private bool canChangeState;

        public bool IsCrouching => isCrouching;
        public bool CanChangeState => canChangeState;

        public float CrouchHeightTransitionSpeed => crouchHeightTransitionSpeed;

        public const MovementStateType TYPE = MovementStateType.Crouch;

        protected CrouchState() : base() {
            isCrouching = false;
        }


#if UNITY_EDITOR
        protected override void Validate() {
            crouchMoveSpeed = Mathf.Clamp(crouchMoveSpeed, 0f, float.MaxValue);
            crouchHeight = Mathf.Clamp(crouchHeight, 0f, float.MaxValue);

            crouchHeightTransitionSpeed = Mathf.Clamp(crouchHeightTransitionSpeed, 0f, float.MaxValue);
            crouchMoveTransitionSpeed   = Mathf.Clamp(crouchMoveTransitionSpeed, 0f, float.MaxValue);

            unCrouchSafetyCheckDistance = Mathf.Clamp(unCrouchSafetyCheckDistance, 0f, float.MaxValue);
        }
#endif

        protected void SetIsCrouching(bool isCrouching) { this.isCrouching = isCrouching; }

        public sealed override bool CanTransition() {
            return base.CanTransition() && IsStateMachineCrouching(MovementStateMachine);
        }

        protected sealed override void OnUpdate() {
            base.OnUpdate();
            MovementStateMachine?.SetMoveSpeed(crouchMoveSpeed, crouchMoveTransitionSpeed);
            MovementStateMachine?.SetHeight(crouchHeight, crouchHeightTransitionSpeed);
        }

        protected sealed override void SetMovementStateType(ref MovementStateType type) {
            type = TYPE;
        }

        protected override void OnDisabled()
        {
            base.OnDisabled();
            isCrouching = false;
        }

        protected sealed override MovementStateType[] GetDefaultInvalidTypes()
        {
            return new MovementStateType[] { TYPE, LandState.TYPE, FallState.TYPE };
        }

        protected override void OnEntered()
        {
            base.OnEntered();
            isCrouching = true;
        }

        protected sealed override void UpdateStateTransitions()
        {
            if(MovementStateMachine != null) {
                Transform transform = MovementStateMachine.transform;
                canChangeState = !Physics.Raycast(transform.position, transform.up, unCrouchSafetyCheckDistance, ~(1 << GetLayerToIgnore()));
               
                if (canChangeState) { base.UpdateStateTransitions(); }
                else { isCrouching = true; }
            }
        }

        protected abstract int GetLayerToIgnore();

        public static CrouchState GetState(MovementStateMachine stateMachine)
        {
            if(stateMachine == null) return null;
            return stateMachine?.GetState(TYPE) as CrouchState;
        }

        public static void ForceState(MovementStateMachine stateMachine) {
            CrouchState state = GetState(stateMachine);
            if (state != null) state.isCrouching = true; 
        }

        public static bool IsStateMachineCrouching(MovementStateMachine stateMachine){
            CrouchState state = GetState(stateMachine);
            return state != null ? state.IsCrouching : false;   
        }
    }
}
