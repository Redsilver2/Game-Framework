using System.Collections.Generic;
using UnityEngine;

namespace RedSilver2.Framework.StateMachines.States
{
    public class FallState : PlayerState
    {
        public FallState(PlayerStateMachine owner) : base(owner) {

        }

        public sealed override bool IsValidTransition() {
            if(MovementHandler == null) return false;
            return !MovementHandler.IsGrounded;
        }

        protected sealed override void AddRequiredTransitionStates(PlayerStateMachine stateMachine) {
            if (stateMachine == null) return;

            Debug.LogWarning("Create Land State | Contains: " + !stateMachine.ContainsState(PlayerStateType.Land) +  " | Is Valid Transition: " + IsValidTransitionState(PlayerStateType.Land));

            if (!stateMachine.ContainsState(PlayerStateType.Land) && IsValidTransitionState(PlayerStateType.Land)) {
                new LandState(stateMachine);
                AddTransitionState(stateMachine.GetState(PlayerStateType.Land));
            }
        }

        protected sealed override void SetIncompatibleStateTransitions(ref PlayerStateType[] results) {
           results = GetStateTypes(new PlayerStateType[] { PlayerStateType.Land });
        }

        protected sealed override void SetPlayerStateType(ref PlayerStateType type) {
            type = PlayerStateType.Fall;
        }
    }
}
