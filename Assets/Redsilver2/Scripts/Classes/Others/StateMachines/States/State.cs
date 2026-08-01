using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace RedSilver2.Framework.StateMachines.States
{
    public abstract class State : MonoBehaviour {

        [SerializeField] private bool isActif;

        private StateMachine stateMachine;
        private List<State>  transitionStates;

        private  UnityEvent onEntered;
        private  UnityEvent onExited;

        private  UnityEvent onEnabled;
        private  UnityEvent onDisabled;

        private UnityEvent<State> onTransitionStateAdded;
        private UnityEvent<State> onTransitionStateRemoved;

        public bool IsActif => isActif;

        protected virtual void Awake() {
            transitionStates = new List<State>();

            onEnabled  = new UnityEvent();
            onDisabled = new UnityEvent();

            onEntered  = new UnityEvent();
            onExited   = new UnityEvent();

            onTransitionStateAdded   = new UnityEvent<State>();
            onTransitionStateRemoved = new UnityEvent<State>();

            SetStateMachine(GetComponent<StateMachine>());

            AddOnEnteredListener(OnEntered);
            AddOnExitedListener(OnExited);

            AddOnEnabledListener(OnEnabled);
            AddOnDisabledListener(OnDisabled);
        }

        private void OnEnable() { onEnabled?.Invoke(); }
        private void OnDisable() { onDisabled?.Invoke(); }

        public void Enter() { onEntered?.Invoke(); }
        public void Exit() { onExited?.Invoke(); }
        public void SetIsActif(bool isActif) { this.isActif = isActif; }

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

        protected void UpdateStateTransitions() {
            if (transitionStates == null) return;

            foreach (MovementState state in transitionStates) {
                if (state == null || !state.CanTransition()) continue;
                stateMachine?.ChangeState(state);
            }
        }

        protected virtual void OnDisabled() { }
        protected virtual void OnEnabled() { }

        protected virtual void OnEntered() { }
        protected virtual void OnExited() { }

        protected virtual void SetStateMachine(StateMachine stateMachine){
            this.stateMachine = stateMachine;
        }

        protected virtual bool CanAddTransitionState(State state)
        {
            if (stateMachine == null || state == null || state == this) return false;
            else if (transitionStates == null || transitionStates.Contains(state)) return false;


            Debug.Log("Contains StateMachine State: " + stateMachine.ContainsState(state));
            return stateMachine.ContainsState(state);
        }

        public virtual bool CanTransition()
        {
            if (enabled == false || !isActif) return false;
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


        public void AddOnTransitionStateRemovedListener(UnityAction<State> action)
        {
            if (action != null) onTransitionStateRemoved?.AddListener(action);
        }
        public void RemoveOnTransitionStateRemovedListener(UnityAction<State> action)
        {
            if (action != null) onTransitionStateRemoved?.RemoveListener(action);
        }

        public abstract string GetStateName();
    }
}
