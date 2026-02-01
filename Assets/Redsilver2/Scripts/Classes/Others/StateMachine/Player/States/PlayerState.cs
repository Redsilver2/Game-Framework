using RedSilver2.Framework.StateMachines.States.Movement;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RedSilver2.Framework.StateMachines.States
{

    public abstract class PlayerState : UpdateableState
    {
        private readonly PlayerStateType  Type;
        public  readonly PlayerStateType[] IncompatibleTransitionStates;

        public readonly PlayerMovementHandler MovementHandler;

        protected PlayerState(PlayerStateMachine owner) : base(owner) {
            SetMovementHandler(owner, ref MovementHandler);

            SetPlayerStateType(ref Type);
            SetIncompatibleStateTransitions(ref IncompatibleTransitionStates);
            
            IncompatibleTransitionStates = IncompatibleTransitionStates.Distinct().ToArray();
            owner?.AddState(Type, this);
        }


        private void SetMovementHandler(PlayerStateMachine stateMachine, ref PlayerMovementHandler handler)
        {
            Debug.Log("StateMachine: " + stateMachine.MovementHandler);
            if (stateMachine != null) handler = stateMachine.MovementHandler;
        }


        protected bool IsValidTransitionState(PlayerStateType stateType)  {
            if (IncompatibleTransitionStates == null || owner == null) return false;
            return IsValidTransitionState(owner.GetState(stateType.ToString()));
        }

        protected sealed override bool IsValidTransitionState(State state) {
            return IsValidTransitionState(state as PlayerState);
        }

        protected bool IsValidTransitionState(PlayerState state) {
            if      (state == null || IncompatibleTransitionStates == null) return false;
            return !IncompatibleTransitionStates.Contains(state.Type);
        }

        protected sealed override void AddRequiredTransitionStates(StateMachine stateMachine) {
            AddRequiredTransitionStates(stateMachine as PlayerStateMachine);
        }

        protected abstract void AddRequiredTransitionStates(PlayerStateMachine stateMachine);
        protected abstract void SetPlayerStateType(ref PlayerStateType type);
        protected abstract void SetIncompatibleStateTransitions(ref PlayerStateType[] results);

        public sealed override string GetStateName()
        {
            return Type.ToString();
        }

        protected static PlayerStateType[] GetStateTypes(){
            return ((PlayerStateType[])Enum.GetValues(typeof(PlayerStateType)));
        }

        protected static PlayerStateType[] GetStateTypes(PlayerStateType[] excludedStates) {
            if(excludedStates == null || excludedStates.Length == 0) return GetStateTypes();
            PlayerStateType[] results = GetStateTypes();

            foreach(PlayerStateType type in excludedStates) 
                results = results.Where(x => x != type).Distinct().ToArray();

            return results;
        }
    }
}
