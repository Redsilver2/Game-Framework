using System;
using UnityEngine;

namespace RedSilver2.Framework.StateMachines.States
{
    [System.Serializable]
    public sealed class FallState : MovementState
    {

        [Space]
        [SerializeField] private float airbornTimeTransitionTrigger;

        [Space]
        [SerializeField] private float moveSpeed;
        [SerializeField] private float moveTransitionSpeed;

        [Space]
        [SerializeField] private bool canAffectMovementSpeed;

        [Space]
        [SerializeField] private float fallSpeed;
        [SerializeField] private float fallTransitionSpeed;


        [Space]
        [SerializeField] private float groundedFallSpeed;
        [SerializeField] private float groundedFallTransitionSpeed;


        public float AirbornTimeTransitionTrigger => airbornTimeTransitionTrigger;
        
        public float MoveSpeed           => moveSpeed;
        public float MoveTransitionSpeed => moveTransitionSpeed;

        public float FallSpeed           => fallSpeed;
        public float FallTransitionSpeed => fallTransitionSpeed;

        public float DefaultFallSpeed => groundedFallSpeed;
        public float DefaulltFallTransitionSpeed => groundedFallTransitionSpeed;

        public const MovementStateType TYPE = MovementStateType.Fall;

        public FallState() : base() {

        }

#if UNITY_EDITOR
        protected override void Validate()
        {
            base.Validate();
            moveSpeed = Mathf.Clamp(moveSpeed, 0f, float.MaxValue);
            moveTransitionSpeed = Mathf.Clamp(moveTransitionSpeed, 0f, float.MaxValue);

            fallSpeed = Mathf.Clamp(fallSpeed, float.MinValue, 0f);
            fallTransitionSpeed = Mathf.Clamp(fallTransitionSpeed, 0f, float.MaxValue);
        }
#endif

        public void ResetFallSpeed()
        {
            this.fallSpeed = groundedFallSpeed;
        }

        public void Update()
        {
            if (MovementStateMachine != null) {

                bool isFalling = MovementStateMachine.IsCurrentState(this);
                MovementStateMachine?.SetFallSpeed(isFalling ? groundedFallSpeed : fallSpeed, isFalling ? groundedFallTransitionSpeed : fallTransitionSpeed);
            }
        }

        public sealed override bool CanTransition()
        {
            if(MovementStateMachine == null) return false;
            return base.CanTransition() && !MovementStateMachine.IsGrounded && MovementStateMachine.AirbornTime >= airbornTimeTransitionTrigger;
        }

        protected sealed override void OnUpdate()
        {
            base.OnUpdate();
            if (!IsEnabled) return;
            if(canAffectMovementSpeed) MovementStateMachine?.SetMoveSpeed(moveSpeed, moveTransitionSpeed);
        }

        protected sealed override void SetMovementStateType(ref MovementStateType type) {
            type = MovementStateType.Fall;
        }

        public void SetWalkSpeed(float walkSpeed){
            this.moveSpeed = walkSpeed;
        }

        public void SetMoveTransitionSpeed(float moveTransitionSpeed)
        {
            this.moveTransitionSpeed = moveTransitionSpeed;
        }

        public void SetFallSpeed(float fallSpeed){
            this.fallSpeed = fallSpeed;
        }

        public void SetFallTransitionSpeed(float falltransitionSpeed)
        {
            this.fallTransitionSpeed = falltransitionSpeed;
        }

        public static FallState GetState(MovementStateMachine stateMachine)
        {
            if(stateMachine == null) return null;
            return stateMachine.GetState(TYPE) as FallState;    
        }


        protected sealed override MovementStateType[] GetDefaultInvalidTypes()
        {
            var results = Enum.GetValues(typeof(MovementStateType)) as MovementStateType[];
            return results == null ? new MovementStateType[0] : results;
        }

        protected sealed override MovementStateType[] GetRequiredTypes()
        {
            return new MovementStateType[] { LandState.TYPE };
        }
    }
}
