using RedSilver2.Framework.StateMachines.Events;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace RedSilver2.Framework.StateMachines.States
{
    [System.Serializable]
    public abstract class State {
        [HideInInspector] public string name;
        [SerializeField, HideInInspector] private string[] incompatibleTransitionStates;

        [SerializeField, SerializeReference] private List<StateEvent> events;
        [SerializeField, HideInInspector] private List<State> transitionStates;
        [SerializeReference, HideInInspector] private StateMachine stateMachine;

        private bool isEnabled;
        private bool isEntered;

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


        protected State(StateMachine stateMachine) {
            SetIncompatibleTransitionStates(ref incompatibleTransitionStates);
            isEntered = false;

            transitionStates = new List<State>();
            events = new List<StateEvent>();

            onAdded    = new UnityEvent();
            onRemoved  = new UnityEvent();

            onEnabled  = new UnityEvent();
            onDisabled = new UnityEvent();

            onEntered  = new UnityEvent();
            onExited   = new UnityEvent();

            onTransitionStateAdded   = new UnityEvent<State>();
            onTransitionStateRemoved = new UnityEvent<State>();

            this.stateMachine = stateMachine;

            AddOnEnteredListener(OnEntered);
            AddOnExitedListener(OnExited);

            AddOnEnabledListener(OnEnabled);
            AddOnDisabledListener(OnDisabled);

            AddOnAddedListener(OnAdded);
            AddOnRemovedListener(OnRemoved);
        }

        public void AddEvent(StateEvent _event)
        {
            if (events == null || _event == null) return;
            else if (!events.Contains(_event) && _event.IsOwner(this) && !ContainsEvent(name)) {
                events?.Add(_event);
                if (Application.isPlaying && isEnabled) _event.Enable();
            }
        }

        public void RemoveEvent(StateEvent _event)
        {
            if (events == null || _event == null) return;
            else if (events.Contains(_event) && _event.IsOwner(this)) {
                if (Application.isPlaying) _event.Disable();
                events?.Remove(_event);
            }
        }

        public bool ContainsEvent(StateEvent _event) {
            if (events == null || _event == null) return false;
            return events.Contains(_event);
        }

        public bool ContainsEvent(string name)
        {
            if(events == null || string.IsNullOrEmpty(name)) return false;
            name = name.ToLower();

            return events.Where(x => x != null).Where(x => x.Compare(name)).Count() > 0;
        }


        public bool IsCurrent()
        {
            if(stateMachine == null) return false;
            return stateMachine.IsCurrentState(this);
        }

        public void Enable() {
            if (!isEnabled) onEnabled?.Invoke();
        }
        public void Disable() { 
            if(isEnabled) onDisabled?.Invoke(); 
        }

        public void Enter()
        {
            if (!isEntered) {
                onEntered?.Invoke();
                isEntered = true;
            }
        }

        public void Exit()
        {
            if (isEntered) {
                onExited?.Invoke();
                isEntered = false;
            }
        }


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
            if (events != null) 
                foreach (StateEvent _event in events)
                    _event?.Disable();


            stateMachine?.RemoveActifState(this);
            isEnabled = false;
        }

        protected virtual void OnEnabled() {
            if (events != null) 
                foreach (StateEvent _event in events)
                    _event?.Enable();
           
            stateMachine?.AddActifState(this); 
            isEnabled = true;
        }

        protected virtual void OnEntered() { }
        protected virtual void OnExited()  { }

        protected virtual void OnAdded() { }
        protected virtual void OnRemoved() { }

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
       
        public virtual void Validate() {
            events = events != null ? events.Where(x => x != null).ToList() : new List<StateEvent>();
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
