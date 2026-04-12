using System.Collections.Generic;
using UnityEngine.Events;

namespace RedSilver2.Framework.StateMachines.States
{
    public abstract class State {
        private   readonly UnityEvent onStateRemoved = new UnityEvent();
        private   readonly UnityEvent onStateAdded   = new UnityEvent();

        private   readonly UnityEvent onStateEntered = new UnityEvent();
        private   readonly UnityEvent onStateExited  = new UnityEvent();

        private   readonly List<State>                         transitionStates;  
        protected readonly StateMachine owner;

        protected State[] TransitionStates {
            get {
                if (transitionStates == null) return null;
                return transitionStates.ToArray();
            }
        }

        protected State(StateMachine owner) {
            transitionStates = new List<State>();
            this.owner = owner;
        }

        public void AddOnStateAddedListener(UnityAction action) {
            if(action != null) onStateAdded?.AddListener(action);
        }
        public void RemoveOnStateAddedListener(UnityAction action) {
            if (action != null) onStateAdded?.RemoveListener(action);
        }

        public void AddOnStateRemovedListener(UnityAction action) {
            if (action != null) onStateRemoved?.AddListener(action);
        }
        public void RemoveOnStateRemovedListener(UnityAction action)  {
            if (action != null) onStateRemoved?.RemoveListener(action);
        }

        public void Enter()
        {
            onStateEntered?.Invoke();
        }

        public void Exit()
        {
            onStateExited?.Invoke();
        }

        protected abstract bool IsValidTransitionState(State state);
        public abstract string GetStateName();
    }
}
