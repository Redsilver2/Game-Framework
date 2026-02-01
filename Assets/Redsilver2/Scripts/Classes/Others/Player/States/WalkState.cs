using System.Collections.Generic;
using UnityEngine;


namespace RedSilver2.Framework.StateMachines.States
{
    public sealed class WalkState : PlayerState
    {
        public WalkState(PlayerStateMachine owner) : base(owner)
        {
        }

        public sealed override bool IsValidTransition() {
            if (MovementHandler == null) return false;
            return MovementHandler.IsGrounded && !MovementHandler.IsCrouching
                   && !MovementHandler.IsRunning  && MovementHandler.GetMoveInputValue().magnitude > 0f;
        }

        protected sealed override void AddRequiredTransitionStates(PlayerStateMachine stateMachine) {
            if (stateMachine == null) return;

            if (!stateMachine.ContainsState(PlayerStateType.Idol) && IsValidTransitionState(PlayerStateType.Idol)) {
                new IdolState(stateMachine);
                AddTransitionState(stateMachine.GetState(PlayerStateType.Idol));
            }
        }

        protected sealed override void SetIncompatibleStateTransitions(ref PlayerStateType[] results) {
            results = GetStateTypes(new PlayerStateType[] { PlayerStateType.Fall, PlayerStateType.Idol, PlayerStateType.Run, PlayerStateType.Crouch, PlayerStateType.Jump });
        }

        protected sealed override void SetPlayerStateType(ref PlayerStateType type)  {
            type = PlayerStateType.Walk;
        }
    }
}
