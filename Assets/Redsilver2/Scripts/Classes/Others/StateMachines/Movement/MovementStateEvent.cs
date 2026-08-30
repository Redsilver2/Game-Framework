using RedSilver2.Framework.StateMachines.States;
using System.Linq;
using UnityEngine;

namespace RedSilver2.Framework.StateMachines.Events {
    [System.Serializable]
    public abstract class MovementStateEvent : UpdatableStateEvent {
        private readonly MovementStateType[] compatibleStateTypes;


        protected MovementStateEvent() : base() {
            SetCompatibleStateTypes(ref compatibleStateTypes);
            compatibleStateTypes = compatibleStateTypes != null ? compatibleStateTypes.Distinct().ToArray() : new MovementStateType[0];

        }


        public sealed override void Add(State state, StateMachine stateMachine) {
            Add(state as MovementState, stateMachine as MovementStateMachine);
        }

        public sealed override void Remove(State state, StateMachine stateMachine) {
            Remove(state as MovementState, stateMachine as MovementStateMachine);
        }

        public sealed override bool IsValid(State state, StateMachine stateMachine) {
            return IsValid(state as MovementState, stateMachine as MovementStateMachine);
        }

        protected abstract void SetCompatibleStateTypes(ref MovementStateType[] types);

        protected virtual void Add(MovementState state, MovementStateMachine stateMachine) { }
        protected virtual void Remove(MovementState state, MovementStateMachine stateMachine) { }
        protected virtual bool IsValid(MovementState state, MovementStateMachine stateMachine)
        {
            if(state != null && stateMachine != null) {
                if (compatibleStateTypes.Contains(state.Type)) {
                    return true;
                }
            }

            return false;
        }
    }
}