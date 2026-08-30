using RedSilver2.Framework.StateMachines.Events;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace RedSilver2.Framework.StateMachines.States
{
    [System.Serializable]
    public abstract class State {
        [SerializeField, SerializeReference] private List<StateEvent> events;
        [SerializeField, HideInInspector] private StateMachine stateMachine;

        private bool isEnabled;
        private string name;

        private readonly string[] incompatibleTransitionStates;
        private readonly List<State> transitionStates;


        private readonly UnityEvent onAdded;
        private readonly UnityEvent onRemoved;

        private readonly UnityEvent onEntered;
        private readonly UnityEvent onExited;

        private readonly UnityEvent onEnabled;
        private readonly UnityEvent onDisabled;

        private readonly UnityEvent<State> onTransitionStateAdded;
        private readonly UnityEvent<State> onTransitionStateRemoved;

        public bool IsEnabled => isEnabled;
        public string Name => name;

        public string[] IncompatibleTransitionStates => incompatibleTransitionStates != null ? incompatibleTransitionStates : new string[0];

        public StateEvent[] Events => events != null ? events.ToArray() : new StateEvent[0];
        public State[] TransitionStates => transitionStates != null ? transitionStates.ToArray() : new State[0];

        protected State() {
            SetIncompatibleTransitionStates(ref incompatibleTransitionStates);
            
            transitionStates = new List<State>();
            events = new List<StateEvent>();


            onAdded = new UnityEvent();
            onRemoved  = new UnityEvent();

            onEnabled  = new UnityEvent();
            onDisabled = new UnityEvent();

            onEntered  = new UnityEvent();
            onExited   = new UnityEvent();

            onTransitionStateAdded   = new UnityEvent<State>();
            onTransitionStateRemoved = new UnityEvent<State>();

            AddOnEnteredListener(OnEntered);
            AddOnExitedListener(OnExited);

            AddOnEnabledListener(OnEnabled);
            AddOnDisabledListener(OnDisabled);
        }

        public void AddEvent(StateEvent _event)
        {
            if (events == null || _event == null) return;
            else if (!events.Contains(_event)) {
                if (_event.IsValid(this, stateMachine) && _event.IsValid(this, stateMachine)) {
                    events?.Add(_event);
                }
            }
        }

        public void RemoveEvent(StateEvent _event)
        {
            if (events == null || _event == null) return;
            else if (events.Contains(_event)) {
                events?.Remove(_event);
            }
        }



        public bool IsCurrent()
        {
            if(stateMachine == null) return false;
            return stateMachine.IsCurrentState(this);
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

        public void Added()   { onAdded?.Invoke(); }
        public void Removed() { onRemoved?.Invoke(); }


        public void AddTransitionState(State state) {
            if (CanAddTransitionState(state)) {
                transitionStates?.Add(state);
                onTransitionStateAdded?.Invoke(state);
            }
        }

        public void RemoveTransitionState(State state) {
            if (transitionStates == null || !transitionStates.Contains(state)) return;
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
            stateMachine?.RemoveActifState(this);
            isEnabled = false;
        }

        protected virtual void OnEnabled() {
            stateMachine?.AddActifState(this); 
            isEnabled = true;
        }

        protected virtual void OnEntered() {
           if(events != null) {
                foreach (StateEvent _event in events)
                    _event?.Add(this, stateMachine);
           }   
        }
        protected virtual void OnExited()  {
            if (events != null) {
                foreach (StateEvent _event in events)
                    _event?.Remove(this, stateMachine);
            }
        }

        protected void SetStateName(string stateName)
        {
            this.name = string.IsNullOrEmpty(stateName) ? string.Empty : stateName;
        }

        protected virtual bool CanAddTransitionState(State state)
        {
            if (stateMachine == null || state == null || state.Name == name || state == this) return false;
            else if (transitionStates == null || transitionStates.Contains(state)) return false;
            else if (incompatibleTransitionStates == null || incompatibleTransitionStates.Contains(state.Name.ToLower())) return false;

            return stateMachine.ContainsState(state);
        }

        public virtual bool CanTransition()
        {
            if (stateMachine == null || !isEnabled || !stateMachine.ContainsState(this)) return false;
            return true;
        }

        public void AddOnAddedListener(UnityAction action)
        {
            if (action != null) onAdded?.AddListener(action);
        }
        public void RemoveOnAddedListener(UnityAction action)
        {
            if (action != null) onAdded?.RemoveListener(action);
        }

        public void AddOnRemovedListener(UnityAction action)
        {
            if (action != null) onRemoved?.AddListener(action);
        }
        public void RemoveOnRemovedListener(UnityAction action)
        {
            if (action != null) onRemoved?.RemoveListener(action);
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

        protected virtual void SetIncompatibleTransitionStates(string[] incompatibleTransitionStates)
        {
            incompatibleTransitionStates = incompatibleTransitionStates != null ? incompatibleTransitionStates : new string[0];
        }

#if UNITY_EDITOR
       
        protected virtual void Validate() {

        }

        protected virtual void Validate(StateEvent _event) {

        }
#endif

        public static StateMachine GetStateMachine(State state)
        {
            return state != null ? state.stateMachine : null;
        }
    }
}
