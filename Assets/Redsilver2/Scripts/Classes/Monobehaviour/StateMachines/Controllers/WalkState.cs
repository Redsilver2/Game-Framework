using RedSilver2.Framework.StateMachines.Events;
using RedSilver2.Framework.StateMachines.Extensions;
using UnityEngine;

namespace RedSilver2.Framework.StateMachines.States
{
    [System.Serializable]
    public abstract partial class WalkState : MovementState
    {
        [Space]
        [SerializeField] private float moveSpeed;
        [SerializeField] private float moveTransitionSpeed;

        public float MoveSpeed => moveSpeed;
        public float MoveTransitionSpeed => moveTransitionSpeed;

        public const MovementStateType TYPE = MovementStateType.Walk;

        public WalkState(MovementStateMachine stateMachine) : base(stateMachine) {
           
        }

        public sealed override bool CanTransition() {
            MovementStateMachine movementStateMachine = GetMovementStateMachine(this);
            if (movementStateMachine == null) return false;

            return movementStateMachine.IsMoving && movementStateMachine.IsGrounded &&
                   !RunState.GetIsRunning(movementStateMachine) && !CrouchState.GetIsCrouching(movementStateMachine);
        }

        protected sealed override void OnUpdate() {
            base.OnUpdate();
            GetMovementStateMachine(this)?.SetMoveSpeed(moveSpeed, moveTransitionSpeed);
        }

        protected sealed override void SetMovementStateType(ref MovementStateType type) {
            type = TYPE;
        }

        public void SetWalkSpeed(float walkSpeed) {
            this.moveSpeed = Mathf.Clamp(walkSpeed, 0f, float.MaxValue);
        }

        public void SetTransitionSpeed(float transitionSpeed) {
            this.moveTransitionSpeed = Mathf.Clamp(transitionSpeed, 0f, float.MaxValue);
        }

        protected sealed override MovementStateType[] GetDefaultInvalidTypes()
        {
            return new MovementStateType[] { TYPE, LandState.TYPE };
        }

        public static WalkState GetState(MovementStateMachine stateMachine) { 
            if(stateMachine == null) return null;
            return stateMachine.GetState(TYPE) as WalkState;
        }
    }

    public abstract partial class WalkState : MovementState
    {

#if UNITY_EDITOR
        [SerializeField, HideInInspector] private bool showGroundWalkAudio;

        public override void Validate()
        {
            base.Validate();

            moveSpeed = Mathf.Clamp(moveSpeed, 0f, float.MaxValue);
            moveTransitionSpeed = Mathf.Clamp(moveTransitionSpeed, 0f, float.MaxValue);
        }

        protected sealed override void DisplayBaseSettings(StateMachine.StateInspectorVisualizer visualizer)
        {
            base.DisplayBaseSettings(visualizer);

            EditorExtension.Space(2.5f);
            SetWalkSpeed(EditorExtension.DisplayFloatSlider("Walk Speed 💨", moveSpeed, 0f, 1000f));
            SetTransitionSpeed(EditorExtension.DisplayFloatSlider("Walk Transition Speed", moveTransitionSpeed, 0f, 1000f));
        }

        protected override void DisplayExtensions(StateMachine.StateInspectorVisualizer visualizer)
        {
            base.DisplayExtensions(visualizer);
            GroundWalkAudio.DrawInspector(this, ref showGroundWalkAudio, visualizer);
        }

#endif

    }
}
