using RedSilver2.Framework.StateMachines.Controllers;
using System.Linq;
using UnityEngine.Events;
using UnityEngine;

namespace RedSilver2.Framework.StateMachines.States {
    public abstract class MovementStateExtension : MovementStateModule
    {
        [SerializeField] private MovementStateType[] inclusiveStates;

        protected sealed override void OnStateAdded(State state)
        {
           if(IsInclusiveState(state as MovementState)) base.OnStateAdded(state);
        }

        protected sealed override void OnStateRemoved(State state)
        {
            if (IsInclusiveState(state as MovementState)) base.OnStateRemoved(state);
        }

        protected sealed override UnityAction<State> GetOnStateAddedAction()
        {
            return state => { if (state is MovementState) OnStateAdded(state as MovementState); };
        }
        protected sealed override UnityAction<State> GetOnStateRemovedAction()
        {
            return state => { if (state is MovementState) OnStateRemoved(state as MovementState); };
        }

        public bool IsInclusiveState(MovementState state) {
            if (state == null || inclusiveStates == null) return false;
            return inclusiveStates.Where(x => x == state.Type).Count() > 0;
        }

        protected abstract void OnStateAdded(MovementState state);
        protected abstract void OnStateRemoved(MovementState state);
    }
}
