using RedSilver2.Framework.StateMachines.Controllers;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace RedSilver2.Framework.StateMachines.States
{
    public abstract class MovementStateInitializer : StateInitializer
    {
        private MovementStateType[] inclusiveStates;

        protected override void Start() {
            inclusiveStates = GetIncludedStates();
            if (inclusiveStates != null) inclusiveStates = inclusiveStates.Distinct().ToArray();

            base.Start();
        }

        protected sealed override void OnStateAdded(State state) {
            if (IsInclusiveState(state as MovementState)) base.OnStateAdded(state);
        }

        protected sealed override void OnStateRemoved(State state) {
            if (IsInclusiveState(state as MovementState)) base.OnStateRemoved(state);
        }

        protected sealed override UnityAction<State> GetOnStateAddedAction() {
            return state => { OnStateAdded(state as MovementState); };
        }

        protected sealed override UnityAction<State> GetOnStateRemovedAction() {
            return state => { OnStateRemoved(state as MovementState); };
        }

        protected sealed override bool CanAddOrRemoveState(StateMachineController controller) {
            return controller is MovementStateMachineController;
        }

        public bool IsInclusiveState(MovementState state)
        {
            if (state == null || inclusiveStates == null) return false;
            return inclusiveStates.Where(x => x == state.Type).Count() > 0;
        }

        protected abstract void OnStateAdded(MovementState state);
        protected abstract void OnStateRemoved(MovementState state);
        protected abstract MovementStateType[] GetIncludedStates();
    }
}