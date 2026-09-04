using UnityEngine;

namespace RedSilver2.Framework.StateMachines.States
{
    [System.Serializable]
    public abstract partial class IdolState : MovementState
    {
        [Space]
        [SerializeField] private float moveSpeedTransition;
        public const MovementStateType TYPE = MovementStateType.Idol;

        public float MoveSpeedTransition => moveSpeedTransition;

        public IdolState(MovementStateMachine stateMachine) : base(stateMachine) {  }

        public sealed override bool CanTransition()
        {
            MovementStateMachine movementStateMachine = GetMovementStateMachine(this);
            if (!base.CanTransition() || movementStateMachine == null) return false;
           
            return !movementStateMachine.IsMoving && movementStateMachine.IsGrounded
                && !RunState.GetIsRunning(movementStateMachine) && !CrouchState.GetIsCrouching(movementStateMachine)
                && !JumpState.GetIsJumping(movementStateMachine);     
        }

        protected sealed override void OnUpdate() {
            base.OnUpdate();
            GetMovementStateMachine(this)?.SetMoveSpeed(0f, moveSpeedTransition);
        }

        protected sealed override void SetMovementStateType(ref MovementStateType type) {
            type = TYPE;
        }

        public void SetMoveSpeedTransition(float moveSpeedTransition)
        {
            this.moveSpeedTransition = Mathf.Clamp(moveSpeedTransition, 0f, float.MaxValue);
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

    public abstract partial class IdolState : MovementState
    {

#if UNITY_EDITOR
        public override void Validate()
        {
            base.Validate();
            moveSpeedTransition = Mathf.Clamp(moveSpeedTransition, 0f, float.MaxValue);
        }

        protected sealed override void DisplayBaseSettings(StateMachine.StateInspectorVisualizer visualizer)
        {
            base.DisplayBaseSettings(visualizer);
            SetMoveSpeedTransition(EditorExtension.DisplayFloatSlider("Move Transition Speed ", moveSpeedTransition, 0f, 1000f)); 
        }
#endif
    }
}