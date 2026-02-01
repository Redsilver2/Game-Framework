using System.Collections.Generic;
using System.Linq;
using UnityEngine.Events;

namespace RedSilver2.Framework.StateMachines.States
{
    public abstract class State {
        private   readonly UnityEvent onStateRemoved;
        private   readonly UnityEvent onStateAdded;

        private   readonly UnityEvent onStateEntered;
        private   readonly UnityEvent onStateExited;

        private   readonly UnityEvent<State> onTransitionStateAdded;
        private   readonly UnityEvent<State> onTransitionStateRemoved;

        private   readonly List<State> transitionStates;
        protected readonly StateMachine owner;

        protected State[] TransitionStates {
            get {
                if (transitionStates == null) return null;
                return transitionStates.ToArray();
            }
        }

        protected State(StateMachine owner) {
            onStateEntered           = new UnityEvent();
            onStateExited            = new UnityEvent();

            onStateRemoved           = new UnityEvent();
            onStateAdded             = new UnityEvent();

            onTransitionStateAdded   = new UnityEvent<State>();
            onTransitionStateRemoved = new UnityEvent<State>();

            transitionStates = new List<State>();
            this.owner = owner;

            owner?.AddOnStateAddedListener  (GetOnStateAddedListener());
            owner?.AddOnStateRemovedListener(GetOnStateRemovedListener());

            owner?.AddOnStateEnteredListener(GetOnStateEnteredListener());
            owner?.AddOnStateExitedListener (GetOnStateExitedListener());

            if (this is not UpdateableState) {
                AddOnStateEnteredListener(() => { UpdateStateTransition(TransitionStates); });
            }

            AddRequiredTransitionStates(owner);
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

        public void AddOnStateEnteredListener(UnityAction action) {
            if (action != null) onStateEntered?.AddListener(action);
        }
        public void RemoveOnStateEnteredListener(UnityAction action) {
            if (action != null) onStateEntered?.RemoveListener(action);
        }

        public void AddOnStateExitedListener(UnityAction action) {
            if (action != null) onStateExited?.AddListener(action);
        }
        public void RemoveOnStateExitedListener(UnityAction action) {
            if (action != null) onStateExited?.RemoveListener(action);
        }

        public void AddOnTransitionStateAddedListener(UnityAction<State> action) {
            if(action != null) onTransitionStateAdded?.AddListener(action); 
        }
        public void RemoveOnTransitionStateAddedListener(UnityAction<State> action) {
            if (action != null) onTransitionStateAdded?.RemoveListener(action);
        }

        public void AddOnTransitionStateRemovedListener(UnityAction<State> action)
        {
            if (action != null) onTransitionStateRemoved?.AddListener(action);
        }
        public void RemoveOnTransitionStateRemovedListener(UnityAction<State> action)
        {
            if (action != null) onTransitionStateRemoved?.RemoveListener(action);
        }

        public void AddTransitionState(State state) {
            if (state == null || state == this) return;

            if (transitionStates != null) {
                if (!transitionStates.Contains(state) && IsValidTransitionState(state)) {
                    transitionStates?.Add(state);
                    onTransitionStateAdded?.Invoke(state);
                }
            }
        }
        public void RemoveTransitionState(State state) {
            if (state == null) return;

            if (transitionStates != null) {
                if (transitionStates.Contains(state)) {
                    transitionStates?.Remove(state);
                    onTransitionStateRemoved?.Invoke(state);
                }
            }
        }

        private UnityAction<State> GetOnStateEnteredListener() {
            return state => {
                if (state == this) onStateEntered?.Invoke();
            };
        }
        private UnityAction<State> GetOnStateExitedListener() {
            return state => {
                if (state == this) onStateExited?.Invoke();
            };
        }


        private UnityAction<State> GetOnStateAddedListener() {
            return state => {
                AddTransitionState(state);
                if (state == this) onStateAdded?.Invoke();
            };
        }
        private UnityAction<State> GetOnStateRemovedListener() {
            return state => {
                RemoveTransitionState(state);

                if (state == this) {
                    onStateRemoved?.Invoke();
                    RemoveAllListenersFromOwner(owner);
                }
            };
        }

        protected virtual void RemoveAllListenersFromOwner(StateMachine owner) {
            owner?.RemoveOnStateAddedListener(GetOnStateAddedListener());
            owner?.RemoveOnStateEnteredListener(GetOnStateEnteredListener());

            owner?.RemoveOnStateExitedListener(GetOnStateExitedListener());
            owner?.RemoveOnStateRemovedListener(GetOnStateRemovedListener());
        }


        protected void UpdateStateTransition(State[] transitionStates)
        {
            var results = transitionStates.Where(x => x != null).Where(x => x.IsValidTransition());

            if (results.Count() > 0)
            {
                State state = results.First();
                owner?.ChangeState(state.GetStateName());
            }
        }

        public string[] GetTransitionStateNames() {
            List<string> results = new List<string>();

            if (transitionStates != null) {
                foreach(State state in transitionStates)
                    results.Add(state.GetStateName());
            }

            return results.ToArray();
        }

        protected abstract void AddRequiredTransitionStates(StateMachine stateMachine);
        protected abstract bool IsValidTransitionState(State state);

        public abstract bool IsValidTransition();
        public abstract string GetStateName();
    }
}
