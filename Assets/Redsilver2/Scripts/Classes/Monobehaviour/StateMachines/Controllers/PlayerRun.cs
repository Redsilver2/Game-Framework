using RedSilver2.Framework.Inputs.Settings;
using UnityEngine;

namespace RedSilver2.Framework.StateMachines.States
{
    [System.Serializable]
    public sealed class PlayerRun : PlayerMovement {
        [Space]
        [SerializeField] private bool hasToHoldInput = true;

        [Space]
        [SerializeField] private PressInputSettings pressInput;
        [SerializeField] private HoldInputSettings holdInput;

        public RunState BaseState => GetState() as RunState;

        public PlayerRun() : base()  { }

        private void Update()
        {
            RunState baseState = BaseState;

            pressInput?.Enable();
            holdInput?.Enable();

            if (baseState == null) return;
            bool isRunning = false;

            if (stateMachine != null && baseState.IsEnabled){
                if(stateMachine.IsGrounded && stateMachine.IsMoving && !CrouchState.IsStateMachineCrouching(stateMachine) && !JumpState.IsStateMachineJumping(stateMachine)) {
                    if (hasToHoldInput) isRunning = holdInput  != null ? holdInput.GetValue() : false;
                    else                isRunning = pressInput != null ?  (pressInput.GetValue() ? !baseState.IsRunning : baseState.IsRunning) : false;
                }
            }
            
            baseState?.SetIsRunning(isRunning);
        }

        protected override void InitializeEvent(MovementState state, MovementStateMachine stateMachine) {
            base.InitializeEvent(state, stateMachine);
            stateMachine?.AddOnUpdateListener(Update);
        }

        protected override void UnInitializeEvent(MovementState state, MovementStateMachine stateMachine)
        {
            base.UnInitializeEvent(state, stateMachine);
            stateMachine?.RemoveOnUpdateListener(Update);
        }

        protected sealed override void SetBaseState(ref MovementState state) {
            state = new RunState();
        }
    }
}