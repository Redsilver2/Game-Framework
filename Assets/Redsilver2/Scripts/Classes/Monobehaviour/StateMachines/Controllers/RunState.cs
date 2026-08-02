using RedSilver2.Framework.StateMachines.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RedSilver2.Framework.StateMachines.States
{
    [RequireComponent(typeof(IdolState))]
    public abstract class RunState : MovementState {
        [Space]
        [SerializeField] private float runSpeed;
        [SerializeField] private float runTransitionSpeed;

        private bool isRunning;
        public bool IsRunning => isRunning;
        public const MovementStateType TYPE = MovementStateType.Run;

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

        protected override void OnExited(MovementStateMachine stateMachine)
        {
            base.OnExited(stateMachine);
            isRunning = false;
        }

        protected override void OnDisabled(MovementStateMachine stateMachine)
        {
            base.OnDisabled(stateMachine);
            isRunning = false;
        }

        public void SetRunSpeed(float runSpeed) {
            this.runSpeed = runSpeed;
        }
       
        public void SetRunTransitionSpeed(float runTransitionSpeed) {
            this.runTransitionSpeed = runTransitionSpeed;
        }


        protected sealed override void SetMovementStateType(ref MovementStateType type)
        {
            type = TYPE;
        }

        protected sealed override void OnUpdate(MovementStateMachine stateMachine) {
            stateMachine?.SetMoveSpeed(this.runSpeed, runTransitionSpeed);
        }

        public void SetIsRunning(bool isRunning) { this.isRunning = isRunning; }
        public sealed override bool CanTransition(MovementStateMachine stateMachine)
        {
            return base.CanTransition(stateMachine) && IsStateMachineRunning(stateMachine);
        }

       
        public static RunState GetState(MovementStateMachine stateMachine) {
            if (stateMachine == null) return null;
            return stateMachine.GetState(TYPE) as RunState;
        }

        public static bool IsStateMachineRunning(MovementStateMachine stateMachine) {
            RunState state = GetState(stateMachine);
            return state != null ? state.IsRunning : false;
        }

    }
}
