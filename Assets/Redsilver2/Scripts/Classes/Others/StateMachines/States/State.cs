using RedSilver2.Framework.StateMachines.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

#if UNITY_EDITOR
using UnityEditor;
#endif


namespace RedSilver2.Framework.StateMachines.States
{
    [System.Serializable]
    public abstract partial class State {
        [HideInInspector] public string name;
        [SerializeField, HideInInspector] private string[] incompatibleTransitionStates;


        [SerializeField] private Dictionary<string, StateEventData> events;

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
        public State[] TransitionStates => transitionStates != null ? transitionStates.ToArray() : new State[0];


        protected State(StateMachine stateMachine) {
            SetIncompatibleTransitionStates(ref incompatibleTransitionStates);
            isInitialized = false;

            isEnabled = false;
            isEntered = false;

            events = new Dictionary<string, StateEventData>();

            onAdded    = new UnityEvent();
            onRemoved  = new UnityEvent();

            onEntered  = new UnityEvent();
            onExited   = new UnityEvent();

            onEnabled  = new UnityEvent();
            onDisabled = new UnityEvent();

            onTransitionStateAdded   = new UnityEvent<State>();
            onTransitionStateRemoved = new UnityEvent<State>();

            transitionStates  = new List<State>();
            this.stateMachine = stateMachine;
        }

        public void AddEvent(string name, StateEvent _event)
        {
            if (events == null || _event == null || string.IsNullOrEmpty(name)) return;
            else if (!ContainsEvent(name) && _event.IsOwner(this)) {
                events?.Add(name.ToLower(), new StateEventData(_event));
                if (Application.isPlaying && isEnabled) _event.Enable();
            }
        }

        public void RemoveEvent(string name)
        {
            if (events == null || !ContainsEvent(name)) return;
            events?.Remove(name.ToLower());  
        }

        public StateEvent GetEvent(string name)
        {
            if(events == null || !ContainsEvent(name)) return null;
            return events[name.ToLower()].Event;
        }

        public bool ContainsEvent(string name) {
            if(events == null || string.IsNullOrEmpty(name)) return false;
            return events.ContainsKey(name.ToLower());
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
            if (!isEnabled && isInitialized) { onEnabled?.Invoke(); }
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
            {
                StateEventData[] datas = events.Values.ToArray();

                foreach (StateEventData data in datas) {
                    if (data == null) continue;
                    data.Event?.Disable();
                }

            }

            stateMachine?.RemoveActifState(this);
            isEnabled = false;
        }

        protected virtual void OnEnabled() {
            if (events != null) {
                StateEventData[] datas = events.Values.ToArray();

                foreach (StateEventData data in datas) {
                    if(data == null) continue;
                    data.Event?.Enable();
                }
            }
      
            stateMachine?.AddActifState(this); 
            isEnabled = true;
        }

        protected virtual void OnEntered() { }
        protected virtual void OnExited()  { }

        protected virtual void OnAdded() { 
            if(stateMachine != null) 
                foreach(State state in stateMachine.ActifStates)
                    AddTransitionState(state);
        }
        protected virtual void OnRemoved() {
            if (stateMachine != null)
                foreach (State state in stateMachine.ActifStates)
                    RemoveTransitionState(state);
        }

        private void OnActifAdded(State state) {
            AddTransitionState(state);
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

        public static StateMachine GetStateMachine(State state)
        {
            return state != null ? state.stateMachine : null;
        }

        // Rare Comment: Hopefully they allow abstract classes serialization soon for Dictionnaries bruh-

        [System.Serializable]
        private sealed class StateEventData {
           [SerializeField, SerializeReference, HideInInspector] private StateEvent _event;
           public StateEvent Event => _event;

           public StateEventData(StateEvent _event){
                this._event = _event;
           }
        }
    }
    
    public abstract partial class State
    {
#if UNITY_EDITOR
        protected bool CanShowBaseSettings;
        protected bool CanShowExtensions;
        protected bool CanShowTransitions;

        [SerializeField, HideInInspector] private bool showBaseSettings;
        [SerializeField, HideInInspector] private bool showExtensions;
        [SerializeField, HideInInspector] private bool showTransitions;

        [SerializeField, HideInInspector] private bool showCompatibleStates;
        [SerializeField, HideInInspector] private bool showIncompatibleStates;

        public virtual void Validate() {
            CanShowBaseSettings = true;
            CanShowExtensions   = true;
            CanShowTransitions  = true;
        }

        public virtual void DrawInpsector(StateMachine.InspectorVisualizer visualizer)
        {
            Validate();
            if (visualizer == null) return;

            if (CanShowBaseSettings) {
                if (EditorExtension.DisplayFoldout("Base Settings", ref showBaseSettings, visualizer.FoldoutColor))
                {
                    EditorExtension.DrawVerticalHelpBox(() => {
                        DisplayBaseSettings(visualizer);
                    }, visualizer.BackgroundColor);
                }

            }
            else { showBaseSettings = false; }

            if (CanShowExtensions) {
                if (EditorExtension.DisplayFoldout("Extensions", ref showExtensions, visualizer.FoldoutColor)) {
                    DisplayExtensions(visualizer);
                }
            }
            else { showExtensions = false; }

            if (CanShowTransitions) {
                if (EditorExtension.DisplayFoldout("Transitions", ref showTransitions, visualizer.FoldoutColor))
                {
                    DisplayTransitions(visualizer);
                }
            }
            else { showTransitions = false; }

        }

        protected virtual void DisplayBaseSettings(StateMachine.InspectorVisualizer visualizer) { }

        protected virtual void DisplayExtensions(StateMachine.InspectorVisualizer visualizer) { }

        private void DisplayTransitions(StateMachine.InspectorVisualizer visualizer)
        {

            if (visualizer == null) return;
            else if (TryGetTransitionsInfo(out string[] compatible, out string[] incompatible))
            {
                EditorExtension.IncrementIndent();

                DisplayTransitions(compatible, "Compatible States", ref showCompatibleStates, visualizer);
                DisplayTransitions(incompatible, "Incompatible States", ref showIncompatibleStates, visualizer);

                EditorExtension.DecrementIndent();
            }
        }

        private void DisplayTransitions(string[] values, string label, ref bool showStates, StateMachine.InspectorVisualizer visualizer)
        {
            if (values == null || visualizer == null || values.Length == 0) return;
            if (EditorExtension.DisplayFoldout(label, ref showStates, visualizer.FoldoutColor))  {
                foreach (string s in values) {
                    if (string.IsNullOrEmpty(s)) continue;
                    EditorGUILayout.LabelField(s, EditorStyles.boldLabel);
                }
            }

        }

        private bool TryGetTransitionsInfo(out string[] compatible, out string[] incompatible)
        {
            compatible = new string[0];
            incompatible = new string[0];

            if (incompatibleTransitionStates == null) return false;
            MovementStateType[] types = Enum.GetValues(typeof(MovementStateType)) as MovementStateType[];

            List<string> compatibleStates = new List<string>();
            List<string> incompatibleStates = new List<string>();

            foreach (MovementStateType type in types)
            {
                if (incompatibleTransitionStates.Contains(type.ToString().ToLower())) incompatibleStates?.Add(type.ToString());
                else compatibleStates?.Add(type.ToString());
            }

            compatible = compatibleStates.ToArray();
            incompatible = incompatibleStates.ToArray();
            return true;
        }
#endif
    }
}


