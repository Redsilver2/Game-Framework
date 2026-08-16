using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace RedSilver2.Framework.StateMachines.States
{
    [System.Serializable]
    public abstract class State {
        private bool isEnabled;
        private string stateName;
        private string[] incompatibleTransitionStates;

        private  StateMachine stateMachine;
        private readonly List<State> transitionStates;

        private readonly UnityEvent onEntered;
        private readonly UnityEvent onExited;

        private readonly UnityEvent onEnabled;
        private readonly UnityEvent onDisabled;

        private readonly UnityEvent<State> onTransitionStateAdded;
        private readonly UnityEvent<State> onTransitionStateRemoved;


        public bool IsEnabled => isEnabled;
        public string StateName => stateName;

        protected State() {
            transitionStates = new List<State>();
            SetIncompatibleTransitionStates(ref incompatibleTransitionStates);


            onEnabled  = new UnityEvent();
            onDisabled = new UnityEvent();

            onEntered = new UnityEvent();
            onExited  = new UnityEvent();

            onTransitionStateAdded = new UnityEvent<State>();
            onTransitionStateRemoved = new UnityEvent<State>();

            AddOnEnteredListener(OnEntered);
            AddOnExitedListener(OnExited);

            AddOnEnabledListener(OnEnabled);
            AddOnDisabledListener(OnDisabled);
        }

        protected void SetStateMachine(StateMachine stateMachine) {
            this.stateMachine = stateMachine;
        }

        public void Enable() {
            if (!isEnabled) onEnabled?.Invoke();
        }
        public void Disable() { 
            if(isEnabled) onDisabled?.Invoke(); 
        }

        public void Enter() { onEntered?.Invoke(); }
        public void Exit()  { onExited?.Invoke();  }


        public void AddTransitionState(State state) {
            if (CanAddTransitionState(state)) {
                transitionStates?.Add(state);
                onTransitionStateAdded?.Invoke(state);
            }
        }

        public void RemoveTransitionState(State state) {
            onTransitionStateRemoved?.Invoke(state);
            transitionStates?.Remove(state);
        }

        protected virtual void UpdateStateTransitions() {
            if (transitionStates == null) return;

            foreach (State state in transitionStates) {
                if (state == null || !state.CanTransition()) continue;
                stateMachine?.ChangeState(state);
            }
        }

        protected virtual void OnDisabled() {

            foreach (State _state in stateMachine.States)
                RemoveTransitionState(_state);

            stateMachine?.RemoveOnStateAddedListener(OnStateAdded);
            stateMachine?.RemoveOnStateRemovedListener(OnStateRemoved);

            stateMachine?.RemoveState(this);
            isEnabled = false;
        }

        protected virtual void OnEnabled() {
            stateMachine?.AddState(this);

            foreach (State state in stateMachine.States)
                AddTransitionState(state);

            stateMachine?.AddOnStateAddedListener(OnStateAdded);
            stateMachine?.AddOnStateRemovedListener(OnStateRemoved);

            isEnabled = true;
        }

        protected virtual void OnEntered() { }
        protected virtual void OnExited()  { }

        protected virtual void OnStateAdded(State state)
        {
            if (state == null || state == null || state == this) return;
            else { AddTransitionState(state); }
        }

        protected virtual void OnStateRemoved(State state)
        {
            if (stateMachine == null || state == null || state == this) return;
            else { RemoveTransitionState(state); }
        }

        protected void SetStateName(string stateName)
        {
            this.stateName = string.IsNullOrEmpty(stateName) ? string.Empty : stateName;
        }

        protected virtual bool CanAddTransitionState(State state)
        {
            if (stateMachine == null || state == null || state.StateName == stateName || state == this) return false;
            else if (transitionStates == null || transitionStates.Contains(state)) return false;
            else if (incompatibleTransitionStates == null || incompatibleTransitionStates.Contains(state.StateName.ToLower())) return false;

            return stateMachine.ContainsState(state);
        }

        public virtual bool CanTransition()
        {
            if (stateMachine == null || !isEnabled || !stateMachine.ContainsState(this)) return false;
            return true;
        }

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


        public void AddOnTransitionStateAddedListener(UnityAction<State> action)
        {
            if (action != null) onTransitionStateAdded?.AddListener(action);
        }
        public void RemoveOnTransitionStateAddedListener(UnityAction<State> action)
        {
            if (action != null) onTransitionStateAdded?.RemoveListener(action);
        }

        protected virtual void SetIncompatibleTransitionStates(ref string[] incompatibleStates) {
            if (incompatibleStates == null) incompatibleStates = new string[0];
            string[] results = new string[incompatibleStates.Length + 1];

            for (int i = 0; i < results.Length; i++) {
                if (i == results.Length - 1) results[i] = string.Empty;
                else results[i] = incompatibleStates[i].ToLower();
            }

            incompatibleStates = results;
        }
        public void AddOnTransitionStateRemovedListener(UnityAction<State> action) {
            if (action != null) onTransitionStateRemoved?.AddListener(action);
        }
        public void RemoveOnTransitionStateRemovedListener(UnityAction<State> action) {
            if (action != null) onTransitionStateRemoved?.RemoveListener(action);
        }

        public bool IsCurrentStateMachine(StateMachine stateMachine)
        {
            return this.stateMachine == stateMachine;
        }

#if UNITY_EDITOR
       
        protected virtual void Validate() { }
#endif
    }
}
