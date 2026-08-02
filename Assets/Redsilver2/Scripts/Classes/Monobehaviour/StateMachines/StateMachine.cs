using RedSilver2.Framework.StateMachines.States;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace RedSilver2.Framework.StateMachines
{
    public abstract class StateMachine : MonoBehaviour
    {
        private State currentState;

        private List<State> states;
        private UnityEvent onEnabled, onDisabled;

        private UnityEvent<State> onStateAdded, onStateRemoved;
        private UnityEvent<State> onStateEntered, onStateExited;

        public State[] States => states != null ? states.ToArray() : new State[0];

        protected virtual void Awake()
        {
            states = new List<State>();

            onEnabled = new UnityEvent();
            onDisabled = new UnityEvent();

            onStateAdded = new UnityEvent<State>();
            onStateRemoved = new UnityEvent<State>();

            onStateEntered = new UnityEvent<State>();
            onStateExited = new UnityEvent<State>();

            AddOnStateAddedListener(OnStateAdded);
            AddOnStateRemovedListener(OnStateRemoved);

            AddOnStateEnteredListener(OnStateEntered);
            AddOnStateExitedListener(OnStateExited);

            AddOnDisabledListener(OnDisabled);
            AddOnEnabledListener(OnEnabled);
        }

        private void OnDisable() { onDisabled?.Invoke(); }
        private void OnEnable() { onEnabled?.Invoke(); }

        public void AddState(State state)
        {
            if (CanAddState(state))
            {
                states?.Add(state);
                onStateAdded?.Invoke(state);
            }
        }

        public void RemoveState(State state)
        {
            if (states == null || state == null || !states.Contains(state)) return;
            onStateRemoved?.Invoke(state);
            states?.Remove(state);
        }

        public void ChangeState(State state)
        {
            ChangeState(state, true);
        }
        public void ChangeState(string stateName)
        {
            ChangeState(GetState(stateName), true);
        }

        public void ChangeState(string stateName, bool checkSimilarity)
        {
            ChangeState(GetState(stateName), checkSimilarity);
        }
        public void ChangeState(State state, bool checkSimilarity)
        {
            if (states == null || (this.currentState == state && checkSimilarity)) return;
            else if (state != null && !states.Contains(state)) return;

            onStateExited?.Invoke(currentState);
            onStateEntered?.Invoke(state);
        }

        public bool IsCurrentState(State state)
        {
            return currentState == state;
        }

        protected virtual bool CanAddState(State state)
        {
            if (states == null || state == null || states.Contains(state)) return false;
            return true;
        }

        protected virtual void OnEnabled() { }
        protected virtual void OnDisabled() { }

        protected abstract void OnStateAdded(State state);
        protected abstract void OnStateRemoved(State state);

        protected virtual void OnStateEntered(State state)
        {
            currentState = state;
            currentState?.Enter();
        }
        protected virtual void OnStateExited(State state)
        {
            currentState?.Exit();
            currentState = null;
        }

        public bool ContainsState(string stateName)
        {
           return ContainsState(GetState(stateName));
        }

        public bool ContainsState(State state)
        {
            if (states == null || state == null) return false;
            return states.Contains(state);
        }

        public State GetState(string stateName)
        {
            if (states == null || string.IsNullOrEmpty(stateName)) return null;

            for (int i = 0; i < states.Count; i++) {
                if(states[i] == null) continue;
                string _state = states[i].StateName;

                if(string.IsNullOrEmpty(_state) || _state != stateName) continue;
                return states[i];
            }

            return null;
        }


        public void AddOnEnabledListener(UnityAction action)
        {
            if (action != null) onEnabled?.AddListener(action);
        }
        public void RemoveOnEnabledListener(UnityAction action)
        {
            if (action != null) onEnabled?.RemoveListener(action);
        }

        public void AddOnDisabledListener(UnityAction action)
        {
            if (action != null) onDisabled?.AddListener(action);
        }
        public void RemoveOnDisabledListener(UnityAction action)
        {
            if (action != null) onDisabled?.RemoveListener(action);
        }

        public void AddOnStateAddedListener(UnityAction<State> action)
        {
            if (action != null) onStateAdded?.AddListener(action);
        }
        public void RemoveOnStateAddedListener(UnityAction<State> action)
        {
            if (action != null) onStateAdded?.RemoveListener(action);
        }

        public void AddOnStateRemovedListener(UnityAction<State> action)
        {
            if (action != null) onStateRemoved?.AddListener(action);
        }
        public void RemoveOnStateRemovedListener(UnityAction<State> action)
        {
            if (action != null) onStateRemoved?.RemoveListener(action);
        }

        public void AddOnStateEnteredListener(UnityAction<State> action)
        {
            if (action != null) onStateEntered?.AddListener(action);
        }
        public void RemoveOnStateEnteredListener(UnityAction<State> action)
        {
            if (action != null) onStateEntered?.RemoveListener(action);
        }

        public void AddOnStateExitedListener(UnityAction<State> action)
        {
            if (action != null) onStateExited?.AddListener(action);
        }
        public void RemoveOnStateExitedListener(UnityAction<State> action)
        {
            if (action != null) onStateExited?.RemoveListener(action);
        }
    }
}
