using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace RedSilver2.Framework.StateMachines.States
{
    public abstract class State : MonoBehaviour {

        [SerializeField] private bool isActif;

        private string   stateName;
        private string[] incompatibleTransitionStates;

        private StateMachine stateMachine;
        private List<State>  transitionStates;

        private  UnityEvent onEntered;
        private  UnityEvent onExited;

        private  UnityEvent onEnabled;
        private  UnityEvent onDisabled;

        private UnityEvent<State> onTransitionStateAdded;
        private UnityEvent<State> onTransitionStateRemoved;

        public bool IsActif => isActif;
        public string StateName => stateName;

#if UNITY_EDITOR
        protected virtual void OnValidate() { }
#endif

        protected virtual void Awake() {
            transitionStates = new List<State>();
            SetIncompatibleTransitionStates(ref incompatibleTransitionStates);  

            onEnabled  = new UnityEvent();
            onDisabled = new UnityEvent();

            onEntered  = new UnityEvent();
            onExited   = new UnityEvent();

            onTransitionStateAdded   = new UnityEvent<State>();
            onTransitionStateRemoved = new UnityEvent<State>();

            SetStateMachine(transform.root != null ? transform.root.GetComponentInChildren<StateMachine>() :
                                                                    GetComponentInChildren<StateMachine>());

            AddOnEnteredListener(OnEntered);
            AddOnExitedListener(OnExited);

            AddOnEnabledListener(OnEnabled);
            AddOnDisabledListener(OnDisabled);
        }

        protected void Start()
        {
            OnEnabled(stateMachine);
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

        protected virtual void UpdateStateTransitions() {
            if (transitionStates == null) return;

            foreach (MovementState state in transitionStates) {
                if (state == null || !state.CanTransition()) continue;
                stateMachine?.ChangeState(state);
            }
        }

        private void OnDisabled() { 
          OnDisabled(stateMachine);  
        }

        private void OnEnabled() {
            OnEnabled(stateMachine);
        }

        private void OnEntered() {
            OnEntered(stateMachine);
        }
        
        private void OnExited() {
           OnExited(stateMachine);
        }

        protected virtual void OnDisabled(StateMachine stateMachine) {
            if (stateMachine == null || !stateMachine.ContainsState(this)) return;
            stateMachine?.RemoveState(this);
            stateMachine?.RemoveOnStateAddedListener(OnStateAdded);
            stateMachine?.RemoveOnStateRemovedListener(OnStateRemoved);
        }

        protected virtual void OnEnabled(StateMachine stateMachine) {
            if (stateMachine == null || stateMachine.ContainsState(this)) return;
            stateMachine?.AddOnStateAddedListener(OnStateAdded);
            stateMachine?.AddOnStateRemovedListener(OnStateRemoved);
            stateMachine?.AddState(this);
        }

        protected virtual void OnEntered(StateMachine stateMachine) { }
        protected virtual void OnExited(StateMachine stateMachine) { }

        protected virtual void OnStateAdded(State state)
        {
            if (stateMachine == null || state == null) return;
            else if (state == this) {
                foreach (State _state in stateMachine.States)
                    AddTransitionState(_state);
            }
            else { AddTransitionState(state); }
        }

        protected virtual void OnStateRemoved(State state)
        {
            if (stateMachine == null || state == null) return;
            else if (state == this) {
                foreach (State _state in stateMachine.States)
                    RemoveTransitionState(_state);
            }
            else { RemoveTransitionState(state); }
        }

        protected virtual void SetStateMachine(StateMachine stateMachine){
            this.stateMachine = stateMachine;
        }

        protected void SetStateName(string stateName)
        {
            this.stateName = string.IsNullOrEmpty(stateName) ?  string.Empty : stateName;
        }

        protected virtual bool CanAddTransitionState(State state)
        {
            if (stateMachine == null || state == null || state.StateName == stateName || state == this) return false;
            else if (transitionStates == null || transitionStates.Contains(state)) return false;
            else if(incompatibleTransitionStates == null || incompatibleTransitionStates.Contains(state.StateName.ToLower())) return false;

            return stateMachine.ContainsState(state);
        }

        public bool CanTransition()
        {
            if (enabled == false || !isActif) return false;
            return CanTransition(stateMachine);
        }

        public virtual bool CanTransition(StateMachine stateMachine)
        {
            if (stateMachine == null || !stateMachine.ContainsState(this)) return false;
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
            if(incompatibleStates == null) incompatibleStates = new string[0];
            string[] results = new string[incompatibleStates.Length + 1];

            for (int i = 0; i < results.Length; i++){
                if(i == results.Length - 1) results[i] = string.Empty;
                else                        results[i] = incompatibleStates[i].ToLower();
            }

            incompatibleStates = results;
        }
        public void AddOnTransitionStateRemovedListener(UnityAction<State> action)
        {
            if (action != null) onTransitionStateRemoved?.AddListener(action);
        }
        public void RemoveOnTransitionStateRemovedListener(UnityAction<State> action)
        {
            if (action != null) onTransitionStateRemoved?.RemoveListener(action);
        }
    }
}
