using RedSilver2.Framework.StateMachines.Controllers;
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

#if UNITY_EDITOR
        protected sealed override void ValidateIncompatibleTransitionStates(ref MovementStateType[] stateTypes)
        {
            List<MovementStateType> results = stateTypes != null ? stateTypes.ToList() : new List<MovementStateType>();

            foreach(MovementStateType type in new MovementStateType[] { TYPE }) {
                if(!results.Contains(type)) results.Add(type);
            }

            stateTypes = results.ToArray();
        }
#endif


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
        protected sealed override bool CanTransition(MovementStateMachine stateMachine)
        {
            if (stateMachine == null) return false;
            return stateMachine.IsGrounded && isRunning;
        }

       
        public static RunState GetState(MovementStateMachine stateMachine) {
            if (stateMachine == null) return null;
            return stateMachine.GetState(TYPE) as RunState;
        }

        public static bool GetIsRunningValue(MovementStateMachine stateMachine) {
            RunState state = GetState(stateMachine);
            return state != null ? state.IsRunning : false;
        }

    }
}
