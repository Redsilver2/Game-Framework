using RedSilver2.Framework.StateMachines.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RedSilver2.Framework.StateMachines.States
{
    [RequireComponent(typeof(LandState))]
    public sealed class FallState : MovementState
    {
        [Space]
        [SerializeField] private float walkSpeed;
        [SerializeField] private float moveTransitionSpeed;

        [Space]
        [SerializeField] private float fallSpeed;
        [SerializeField] private float falltransitionSpeed;


        public const MovementStateType TYPE = MovementStateType.Fall;

        protected sealed override bool CanTransition(MovementStateMachine stateMachine)
        {
            return stateMachine != null ? !stateMachine.IsGrounded : false;
        }

        protected sealed override void OnUpdate(MovementStateMachine stateMachine)
        {
            if (stateMachine == null) return;
            stateMachine?.SetMoveSpeed(walkSpeed, moveTransitionSpeed);
            stateMachine?.SetFallSpeed(fallSpeed, falltransitionSpeed);
        }

        protected sealed override void SetMovementStateType(ref MovementStateType type) {
            type = MovementStateType.Fall;
        }

        public static FallState GetState(MovementStateMachine stateMachine)
        {
            if(stateMachine == null) return null;
            return stateMachine.GetState(TYPE) as FallState;    
        }



#if UNITY_EDITOR
        protected override void ValidateIncompatibleTransitionStates(ref MovementStateType[] stateTypes) {
            if (stateTypes.Length > 0 && !stateTypes.Contains(MovementStateType.Land)) { 
                return;
            }

            List<MovementStateType> results  = new List<MovementStateType>();

            foreach(MovementStateType stateType in Enum.GetValues(typeof(MovementStateType))) {
                if (stateType == MovementStateType.Land) continue;
                results?.Add(stateType);
            }

            stateTypes = results.ToArray();
        }
#endif
    }
}
