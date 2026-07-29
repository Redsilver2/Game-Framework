using RedSilver2.Framework.StateMachines.Controllers;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RedSilver2.Framework.StateMachines.States
{
    public sealed class IdolState : MovementState
    {
        [Space]
        [SerializeField] private float moveSpeedTransition;

        public const MovementStateType TYPE = MovementStateType.Idol;

        protected sealed override bool CanTransition(MovementStateMachine stateMachine)
        {
            if (stateMachine == null || stateMachine.IsCurrentState(TYPE)) return false;
           
            return !stateMachine.IsMoving && stateMachine.IsGrounded
                && !RunState.GetIsRunningValue(stateMachine);     
        }

        protected sealed override void OnUpdate(MovementStateMachine stateMachine) {
            if(stateMachine == null) return;
            stateMachine?.SetMoveSpeed(0f, moveSpeedTransition);
        }

        protected sealed override void SetMovementStateType(ref MovementStateType type) {
            type = TYPE;
        }

        public static IdolState GetState(MovementStateMachine stateMachine)
        {
            if(stateMachine == null) return null;
            return stateMachine.GetState(TYPE) as IdolState;
        }


#if UNITY_EDITOR
        protected override void ValidateIncompatibleTransitionStates(ref MovementStateType[] stateTypes)
        {
            List<MovementStateType> results = stateTypes == null ? new List<MovementStateType>() : stateTypes.ToList();
            if (!results.Contains(TYPE)) results.Add(TYPE);

            stateTypes = results.ToArray();
        }
#endif
    }
}