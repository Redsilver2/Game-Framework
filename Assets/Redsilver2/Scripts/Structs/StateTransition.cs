using UnityEngine;

namespace RedSilver2.Framework.StateMachines.States {
    public struct StateTransition {
        private bool showOppositeResult;
        private ICheckableStateTransition stateTransition;

        public StateTransition(bool showOppositeResult, ICheckableStateTransition stateTransition) {
            this.showOppositeResult = showOppositeResult;
            this.stateTransition    = stateTransition;
        }

        public bool GetTransitionResult() {
            if(stateTransition == null) return true;

            bool result = stateTransition.GetTransitionState();
            return showOppositeResult ? !result : result; 
        }
    }
}
