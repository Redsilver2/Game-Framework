using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace RedSilver2.Framework.StateMachines.States
{
    public abstract class State : MonoBehaviour {
        private  UnityEvent onEntered;
        private  UnityEvent onExited;

        private  UnityEvent onEnabled;
        private  UnityEvent onDisabled;

        // private List<StateCondition> conditions;

        protected virtual void Awake() {
            // conditions = new List<StateCondition>();
            onEnabled = new UnityEvent();
            onDisabled = new UnityEvent();

            onEntered = new UnityEvent();
            onExited  = new UnityEvent();
        }

        private void OnEnable() { onEnabled?.Invoke(); }
        private void OnDisable() { onDisabled?.Invoke(); }

        public void AddOnEnteredListener(UnityAction action) {
            if (action != null) onEntered?.AddListener(action);
        }

        public void RemoveOnEnteredListener(UnityAction action) {
            if (action != null) onEntered?.RemoveListener(action);
        }

        public void AddOnExitedListener(UnityAction action) {
            if (action != null) onExited?.AddListener(action);
        }

        public void RemoveOnExitedListener(UnityAction action) {
            if (action != null) onExited?.RemoveListener(action);
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

        public void Enter() { onEntered?.Invoke(); }
        public void Exit()  { onExited?.Invoke(); }

        protected abstract bool IsValidTransitionState(State state);
        public abstract string GetStateName();
    }
}
