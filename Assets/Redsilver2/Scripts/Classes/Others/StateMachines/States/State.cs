using RedSilver2.Framework.StateMachines.Events;
using System;
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
        [SerializeField, SerializeReference, HideInInspector] private StateMachine stateMachine;
        private bool isEnabled;

        private bool isInitialized;
        private bool isEntered;

        [SerializeField, HideInInspector] private UnityEvent onAdded;
        [SerializeField, HideInInspector] private UnityEvent onRemoved;

        [SerializeField, HideInInspector] private UnityEvent onEntered;
        [SerializeField, HideInInspector] private UnityEvent onExited;

        [SerializeField, HideInInspector] private UnityEvent onEnabled;
        [SerializeField, HideInInspector] private UnityEvent onDisabled;

        [SerializeField, HideInInspector] private UnityEvent<State> onTransitionStateAdded;
        [SerializeField, HideInInspector] private UnityEvent<State> onTransitionStateRemoved;

        public bool IsEnabled => isEnabled;
        public string Name => name;

        public string[] IncompatibleTransitionStates => incompatibleTransitionStates != null ? incompatibleTransitionStates : new string[0];

        public StateEvent[] Events => events != null ? events.ToArray() : new StateEvent[0];
        public State[] TransitionStates => transitionStates != null ? transitionStates.ToArray() : new State[0];


        protected State(StateMachine stateMachine) {
            SetIncompatibleTransitionStates(ref incompatibleTransitionStates);
            isInitialized = false;

            isEnabled = false;
            isEntered = false;

            onAdded = new UnityEvent();
            onRemoved = new UnityEvent();

            onEntered = new UnityEvent();
            onExited = new UnityEvent();

            onEnabled = new UnityEvent();
            onDisabled = new UnityEvent();

            onTransitionStateAdded = new UnityEvent<State>();
            onTransitionStateRemoved = new UnityEvent<State>();

            transitionStates = new List<State>();
            this.stateMachine = stateMachine;
        }

        public void AddEvent(StateEvent _event)
        {
            if (events == null || _event == null) return;
            else if (!events.Contains(_event) && _event.IsOwner(this) && !ContainsEvent(name)) {
                events?.Add(_event);
                if (Application.isPlaying && isEnabled) _event.Enable();
            }
        }

        public void RemoveEvent(string eventName)
        {
            RemoveEvent(GetEvent(eventName));
        }

        public void RemoveEvent(StateEvent _event)
        {
            if (events == null || _event == null) return;
            else if (events.Contains(_event) && _event.IsOwner(this)) {
                if (Application.isPlaying) _event?.Disable();
                events?.Remove(_event);
            }
        }

        protected StateEvent GetEvent(string eventName)
        {
            if(events == null) return null;
            var results = events.Where(x => x != null).Where(x => x.Compare(eventName));
            return results.Count() > 0 ? results.First() : null;
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

        public void Intialize()
        {
            if(isInitialized || !Application.isPlaying) return;

            if (transitionStates == null) transitionStates = new List<State>();

            stateMachine?.AddOnActifStateAddedListener(OnActifAdded);
            stateMachine?.AddOnActifStateRemovedListener(OnActifRemoved);

            AddOnEnteredListener(OnEntered);
            AddOnExitedListener(OnExited);

            AddOnEnabledListener(OnEnabled);
            AddOnDisabledListener(OnDisabled);

            AddOnAddedListener(OnAdded);
            AddOnRemovedListener(OnRemoved);

            onAdded?.Invoke();
            isInitialized = true;
        }

        public void Unintialize()
        {
            if (!isInitialized || !Application.isPlaying) return;
            
            onRemoved?.Invoke();   
            stateMachine?.RemoveActifState(this);

            RemoveOnEnteredListener(OnEntered);
            RemoveOnExitedListener(OnExited);

            RemoveOnEnabledListener(OnEnabled);
            RemoveOnDisabledListener(OnDisabled);

            RemoveOnAddedListener(OnAdded);
            RemoveOnRemovedListener(OnRemoved);

            stateMachine?.RemoveOnActifStateAddedListener(OnActifAdded);
            stateMachine?.RemoveOnActifStateRemovedListener(OnActifRemoved);

            isInitialized = false;
        }



        public bool IsCurrent()
        {
            if(stateMachine == null) return false;
            return stateMachine.IsCurrentState(this);
        }

        public void Enable() {
            if (!isEnabled && isInitialized) {
                Debug.Log("?");
                onEnabled?.Invoke();
            }
        }

        public void Disable() { 
            if(isEnabled && isInitialized) onDisabled?.Invoke(); 
        }

        public void Enter()
        {
            if (stateMachine == null || isEntered || !isInitialized || !stateMachine.IsCurrentState(this)) return;
            isEntered = true;
            onEntered?.Invoke();
        }

        public void Exit()
        {
            if (stateMachine == null || !isEntered || !isInitialized || !stateMachine.IsCurrentState(this)) return;
            isEntered = false;
            onExited?.Invoke();
        }

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

        protected virtual void OnAdded() { 
            if(stateMachine != null) 
                foreach(State state in stateMachine.ActifStates)
                    AddTransitionState(state);

            Debug.Log(transitionStates.Count);
        }
        protected virtual void OnRemoved() {
            if (stateMachine != null)
                foreach (State state in stateMachine.ActifStates)
                    RemoveTransitionState(state);
        }

        private void OnActifAdded(State state) {
            AddTransitionState(state);
            Debug.Log(transitionStates.Count);
        }
        private void OnActifRemoved(State state) {
            RemoveTransitionState(state);
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
        [SerializeReference, HideInInspector] private bool showBaseSettings;
       
        [SerializeReference, HideInInspector] private bool showExtensions;
        [SerializeReference, HideInInspector] private bool showTransitions;
       
        [SerializeReference, HideInInspector] private bool showCompatibleStates;
        [SerializeReference, HideInInspector] private bool showIncompatibleStates;
        
        public virtual void Validate() {
            events = events != null ? events.Where(x => x != null).ToList() : new List<StateEvent>();
        }

        protected virtual void Validate(StateEvent _event) {

        }

        public virtual void DrawInpsector(Color foldoutColor, Color fieldColor) {
            Validate();
            EditorExtension.IncrementIndent();

            if(this is not LandState) {
                if (EditorExtension.DisplayFoldout("Base Settings", ref showBaseSettings, foldoutColor))
                    DisplayBaseSettings(foldoutColor, fieldColor);
            }

            EditorExtension.Space(5f);

            if (EditorExtension.DisplayFoldout("Extension", ref showExtensions, foldoutColor))
                DisplayExenstions(foldoutColor, fieldColor);


            EditorExtension.Space(5f);

            if (EditorExtension.DisplayFoldout("Transitions", ref showTransitions, foldoutColor))
                DisplayTransitions(foldoutColor, fieldColor);

            EditorExtension.DecrementIndent();
        }

        protected virtual void DisplayBaseSettings(Color foldoutColor, Color fieldColor) { }

        protected virtual void DisplayExenstions(Color foldoutColor, Color fieldColor) { }

        private void DisplayTransitions(Color foldoutColor, Color fieldColor)
        {
            if (TryGetTransitionsInfo(out string[] compatible, out string[] incompatible)) {
                EditorExtension.IncrementIndent();
               
                DisplayTransitions(compatible, "Compatible States", ref showCompatibleStates, foldoutColor, fieldColor);
                DisplayTransitions(incompatible, "Incompatible States", ref showIncompatibleStates, foldoutColor, fieldColor);
              
                EditorExtension.DecrementIndent();
            }
        }

        private void DisplayTransitions(string[] values, string label, ref bool showStates, Color foldoutColor, Color fieldColor)
        {
            if(values == null || values.Length == 0) return;

            if (EditorExtension.DisplayFoldout(label, ref showStates, foldoutColor)) {
                foreach (string s in values) EditorExtension.DisplayHelpBoxNone(s);
            }

        }

        private bool TryGetTransitionsInfo(out string[] compatible, out string[] incompatible)
        {
            compatible    = new string[0];
            incompatible = new string[0];

            if (incompatibleTransitionStates == null) return false;
            MovementStateType[] types = Enum.GetValues(typeof(MovementStateType)) as MovementStateType[];
            
            List<string> compatibleStates   = new List<string>();
            List<string> incompatibleStates = new List<string>();

            foreach (MovementStateType type in types)
            {
                if (incompatibleTransitionStates.Contains(type.ToString().ToLower())) incompatibleStates?.Add(type.ToString());
                else compatibleStates?.Add(type.ToString());
            }

            compatible    = compatibleStates.ToArray();
            incompatible = incompatibleStates.ToArray();
            return true;
        }
#endif

        public static StateMachine GetStateMachine(State state)
        {
            return state != null ? state.stateMachine : null;
        }
    }
}
