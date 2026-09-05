using RedSilver2.Framework.StateMachines.Extensions;
using System;
using System.Linq;
using UnityEngine;

namespace RedSilver2.Framework.StateMachines.States
{
    [System.Serializable]
    public abstract partial class RunState : MovementState {
        [Space]
        [SerializeField] private float moveSpeed;
        [SerializeField] private float moveTransitionSpeed;

        private bool isRunning;

        public float MoveSpeed => moveSpeed;
        public float MoveTransitionSpeed => moveTransitionSpeed;
        public bool IsRunning => isRunning;

        private const string SOUND_EVENT = "Walk Sound";

        public const MovementStateType TYPE = MovementStateType.Run;

        public RunState(MovementStateMachine stateMachine) : base(stateMachine) { }

        protected sealed override MovementStateType[] GetDefaultInvalidTypes()
        {
            var results = Enum.GetValues(typeof(MovementStateType)) as MovementStateType[];
            if (results == null) return new MovementStateType[0];

            return results.ToArray();
        }

        protected sealed override MovementStateType[] GetRequiredTypes()
        {
            return new MovementStateType[] { FallState.TYPE, WalkState.TYPE, CrouchState.TYPE, JumpState.TYPE };
        }

        protected override void OnExited()
        {
            base.OnExited();
            isRunning = false;
        }

        protected override void OnDisabled()
        {
            base.OnDisabled();
            isRunning = false;
        }

        public void SetRunSpeed(float runSpeed) {
            this.moveSpeed = runSpeed;
        }
       
        public void SetRunTransitionSpeed(float runTransitionSpeed) {
            this.moveTransitionSpeed = runTransitionSpeed;
        }


        protected sealed override void SetMovementStateType(ref MovementStateType type)
        {
            type = TYPE;
        }

        protected sealed override void OnUpdate() {
            base.OnUpdate();
            GetMovementStateMachine(this)?.SetMoveSpeed(this.moveSpeed, moveTransitionSpeed);
        }

        public void SetIsRunning(bool isRunning) {
            this.isRunning = isRunning;
        }

        public sealed override bool CanTransition() {
            return base.CanTransition() && GetIsRunning(GetMovementStateMachine(this));
        }

       
        public static RunState GetState(MovementStateMachine stateMachine) {
            if (stateMachine == null) return null;
            return stateMachine.GetState(TYPE) as RunState;
        }

        public static bool GetIsRunning(MovementStateMachine stateMachine) {
            RunState state = GetState(stateMachine);
            return state != null ? state.IsRunning : false;
        }

    }
    public abstract partial class RunState : MovementState
    {
#if UNITY_EDITOR
        [SerializeField, HideInInspector] private bool showGroundWalkAudio;

        public override void Validate()
        {
            base.Validate();
            moveSpeed = Mathf.Clamp(moveSpeed, 0f, float.MaxValue);
            moveTransitionSpeed = Mathf.Clamp(moveTransitionSpeed, 0f, float.MaxValue);
        }

        protected override void DisplayBaseSettings(StateMachine.InspectorVisualizer visualizer)
        {
            base.DisplayBaseSettings(visualizer);
            if(visualizer == null) return;

            EditorExtension.Space(2.5f);
            SetRunSpeed(EditorExtension.DisplayFloatSlider("Run Speed 💨", moveSpeed, 0f, 1000f));
            SetRunTransitionSpeed(EditorExtension.DisplayFloatSlider("Run Transition Speed", moveTransitionSpeed, 0f, 1000f));
        }

        protected override void DisplayExtensions(StateMachine.InspectorVisualizer visualizer)
        {
            base.DisplayExtensions(visualizer);
            if(visualizer == null) return;
            GroundWalkAudio.DrawInspector(this, ref showGroundWalkAudio, visualizer);
        }
#endif

    }
}
