using System;
using UnityEditor;
using UnityEngine;

namespace RedSilver2.Framework.StateMachines.States
{
    [System.Serializable]
    public abstract class FallState : MovementState
    {

        [Space]
        [SerializeField] private float airbornTransitionTrigger;

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


        public float AirbornTransitionTrigger => airbornTransitionTrigger;
        public bool  CanAffectMovementSpeed       => canAffectMovementSpeed;
        
        public float MoveSpeed           => moveSpeed;
        public float MoveTransitionSpeed => moveTransitionSpeed;

        public float FallSpeed           => fallSpeed;
        public float FallTransitionSpeed => fallTransitionSpeed;

        public float GroundedFallSpeed => groundedFallSpeed;
        public float GroundedFallTransitionSpeed => groundedFallTransitionSpeed;

        public const MovementStateType TYPE = MovementStateType.Fall;

        public FallState(MovementStateMachine stateMachine) : base(stateMachine) {

        }

#if UNITY_EDITOR
        public override void Validate()
        {
            base.Validate();
            moveSpeed = Mathf.Clamp(moveSpeed, 0f, float.MaxValue);
            moveTransitionSpeed = Mathf.Clamp(moveTransitionSpeed, 0f, float.MaxValue);

            fallSpeed = Mathf.Clamp(fallSpeed, float.MinValue, 0f);
            fallTransitionSpeed = Mathf.Clamp(fallTransitionSpeed, 0f, float.MaxValue);

            groundedFallSpeed = Mathf.Clamp(groundedFallSpeed, float.MinValue, 0f);
            groundedFallTransitionSpeed = Mathf.Clamp(groundedFallTransitionSpeed, 0f, float.MaxValue);
        }

        protected override void DisplayBaseSettings(StateMachine.StateInspectorVisualizer visualizer)
        {
            base.DisplayBaseSettings(visualizer);
            if (visualizer == null) return;

            EditorExtension.Space(10f);
            SetAirbornTransitionTrigger(EditorExtension.DisplayFloatSlider("Airborn Transition Trigger", airbornTransitionTrigger, 0f, 1000f));

            EditorExtension.Space(10f);
            SetGroundedFallSpeed(EditorExtension.DisplayFloatSlider("Grounded Fall Speed", groundedFallSpeed, -1000f, 0f));
            SetGroundedFallTransitionSpeed(EditorExtension.DisplayFloatSlider("Grounded Fall Transition Speed", groundedFallTransitionSpeed, 0f, 1000f));

            EditorExtension.Space(10f);
            SetFallSpeed(EditorExtension.DisplayFloatSlider("Fall Speed", fallSpeed, -1000f, 0f));
            SetFallTransitionSpeed(EditorExtension.DisplayFloatSlider("Fall Transition Speed", fallTransitionSpeed, 0f, 1000f));

            EditorExtension.Space(10f);
            SetCanAffectMovementSpeed(EditorExtension.DisplayToggle("Can Affect Movement Speed", canAffectMovementSpeed));

            EditorExtension.Space(10f);
            SetMoveSpeed(EditorExtension.DisplayFloatSlider("Move Speed 💨", moveSpeed, 0f, 1000f));
            SetMoveTransitionSpeed(EditorExtension.DisplayFloatSlider("Move Transition Speed", moveTransitionSpeed, 0f, 1000f));
        }
#endif

        public void ResetFallSpeed()
        {
            this.fallSpeed = groundedFallSpeed;
        }

        protected override void OnAdded()
        {
            base.OnAdded();
            GetUpdatableStateMachine(this)?.AddOnUpdateListener(Update);
        }

        protected override void OnRemoved()
        {
            base.OnRemoved();
            GetUpdatableStateMachine(this)?.RemoveOnUpdateListener(Update);
        }

        private void Update()
        {
            MovementStateMachine movementStateMachine = GetMovementStateMachine(this);

            if (movementStateMachine != null) {

                bool isFalling = movementStateMachine.IsCurrentState(this);
                movementStateMachine?.SetFallSpeed(isFalling ? groundedFallSpeed : fallSpeed, isFalling ? groundedFallTransitionSpeed : fallTransitionSpeed);
            }
        }

        public sealed override bool CanTransition()
        {
            MovementStateMachine movementStateMachine = GetMovementStateMachine(this);

            if (movementStateMachine == null) return false;
            return base.CanTransition() && !movementStateMachine.IsGrounded && movementStateMachine.AirbornTime >= airbornTransitionTrigger;
        }

        protected sealed override void OnUpdate()
        {
            base.OnUpdate();
            if(canAffectMovementSpeed) GetMovementStateMachine(this)?.SetMoveSpeed(moveSpeed, moveTransitionSpeed);
        }

        protected sealed override void SetMovementStateType(ref MovementStateType type) {
            type = MovementStateType.Fall;
        }

        public void SetMoveSpeed(float moveSpeed){
            this.moveSpeed = moveSpeed;
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

        public void SetGroundedFallSpeed(float groundedFallSpeed)
        {
            this.groundedFallSpeed = groundedFallSpeed;
        }

        public void SetGroundedFallTransitionSpeed(float groundedFallTransitionSpeed)
        {
            this.groundedFallTransitionSpeed = groundedFallTransitionSpeed;
        }

        public void SetCanAffectMovementSpeed(bool canAffectMovementSpeed)
        {
            this.canAffectMovementSpeed = canAffectMovementSpeed;
        }

        public void SetAirbornTransitionTrigger(float airbornTransitionTrigger)
        {
            this.airbornTransitionTrigger = airbornTransitionTrigger;
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
