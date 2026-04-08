using System;
using System.Linq;

namespace RedSilver2.Framework.StateMachines.States
{
    public abstract class MovementState : UpdateableState
    {
        public readonly MovementStateType   Type;
        public readonly MovementStateType[] IncompatibleTransitionStates;

        protected MovementState(MovementStateMachine owner) : base(owner) {
            SetPlayerStateType(ref Type);
            SetIncompatibleStateTransitions(ref IncompatibleTransitionStates);
            IncompatibleTransitionStates = IncompatibleTransitionStates.Distinct().ToArray();;
        }

        protected bool IsValidTransitionState(MovementStateType stateType)  {
            if (IncompatibleTransitionStates == null || owner == null) return false;
            return !IncompatibleTransitionStates.Contains(stateType);
        }

        protected sealed override bool IsValidTransitionState(State state) {
            return IsValidTransitionState(state as MovementState);
        }

        protected bool IsValidTransitionState(MovementState state) {
            if  (state == null || IncompatibleTransitionStates == null) return false;
            return IsValidTransitionState(state.Type);
        }

        public sealed override string GetStateName()
        {
            return Type.ToString();
        }

        public MovementStateMachine GetMovementStateMachine()
        {
            return owner as MovementStateMachine;
        }

        protected abstract void SetPlayerStateType(ref MovementStateType type);
        protected abstract void SetIncompatibleStateTransitions(ref MovementStateType[] results);
        public static MovementStateType[] GetStateTypes(){
            return ((MovementStateType[])Enum.GetValues(typeof(MovementStateType)));
        }

        public static MovementStateType[] GetExcludedStateTypes(MovementStateType[] ignoredStates) {
            if(ignoredStates == null || ignoredStates.Length == 0) return GetStateTypes();
            MovementStateType[] results = GetStateTypes();

            foreach(MovementStateType type in ignoredStates) 
                results = results.Where(x => x != type).Distinct().ToArray();

            return results;
        }

        public static MovementStateType[] GetIncludedStateTypes(MovementStateType[] includedStates)
        {
            if (includedStates == null || includedStates.Length == 0) return GetStateTypes();
            MovementStateType[] results = GetStateTypes();

            foreach (MovementStateType type in includedStates) results = results.Where(x => x == type).Distinct().ToArray();
            return results;
        }
    }
}
