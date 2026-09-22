using RedSilver2.Framework.StateMachines.States;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using RedSilver2.Framework.StateMachines.Events;


#if UNITY_EDITOR
using UnityEditor;
#endif

namespace RedSilver2.Framework.StateMachines
{
    public abstract partial class StateMachine : MonoBehaviour
    {
        [SerializeField, HideInInspector] private bool enableStateAutomatically;
        [SerializeField, HideInInspector] private Dictionary<string, StateData> states;

        [SerializeField, HideInInspector] private Dictionary<string, StateMachineEventData> events;

        private State currentState;
        private List<State> actifStates;

        private UnityEvent onEnabled;
        private UnityEvent onDisabled;

        private UnityEvent<State> onStateAdded;
        private UnityEvent<State> onStateRemoved;

        private UnityEvent<State> onStateEntered;
        private UnityEvent<State> onStateExited;

        private UnityEvent<State> onActifStateAdded;
        private UnityEvent<State> onActifStateRemoved;

        public State CurrentState => currentState;
        public State[] ActifStates => actifStates != null ? actifStates.ToArray() : new State[0];

        protected virtual void Awake()
        {
            actifStates = new List<State>();
            onEnabled = new UnityEvent();

            onDisabled = new UnityEvent();
            onStateAdded = new UnityEvent<State>();

            onStateRemoved = new UnityEvent<State>();
            onStateEntered = new UnityEvent<State>();

            onStateExited = new UnityEvent<State>();
            onActifStateAdded = new UnityEvent<State>();

            onActifStateAdded = new UnityEvent<State>();
            onActifStateRemoved = new UnityEvent<State>();

            AddOnStateAddedListener(OnStateAdded);
            AddOnStateRemovedListener(OnStateRemoved);

            AddOnStateEnteredListener(OnStateEntered);
            AddOnStateExitedListener(OnStateExited);

            AddOnDisabledListener(OnDisabled);
            AddOnEnabledListener(OnEnabled);
        }

        protected virtual void Start()
        {

            if (states != null) {
                foreach (State state in GetStates()) {
                    state?.Intialize();
                    state?.Enable();
                }
            }

            if(events != null)
            {
                foreach(var data in events.Values)
                {
                    data?._Event?.Enable();
                }
            }

        }

        private void OnDisable() { onDisabled?.Invoke(); }
        private void OnEnable() { onEnabled?.Invoke(); }

        public void AddEvent(StateMachineEvent _event)
        {
            string eventName = _event != null ? _event.Name.ToLower() : string.Empty;


            if (events == null || string.IsNullOrEmpty(eventName)) return;
            else {
                if (!events.ContainsKey(eventName)) events?.Add(eventName, null);
               
                if (events[eventName] == null || events[eventName]._Event == null) 
                    events[eventName] = new StateMachineEventData(_event);

            }
        }

        public void RemoveEvent(StateMachineEvent _event) {
            string eventName = _event != null ? _event.Name.ToLower() : string.Empty;

            if (events == null || string.IsNullOrEmpty(eventName) || !events.ContainsKey(eventName)) return;
            else {
                events?.Remove(eventName);
            }
        }


        public void AddState(State state)
        {
            if (states == null || state == null || !CanAddState(state) || states.ContainsKey(state.Name)) return;
            if (Application.isPlaying) {
                state?.Intialize();
                if (enableStateAutomatically) state?.Enable();
            }



            states?.Add(state.name, new StateData(state));
            onStateAdded?.Invoke(state);
        }

        public void AddActifState(State state)
        {
            if (states == null || state == null || !states.ContainsKey(state.Name)) return;
            else if (actifStates == null || state == null || actifStates.Contains(state)) return;

            actifStates?.Add(state);
            onActifStateAdded?.Invoke(state);
        }

        public void RemoveState(State state)
        {
            if (states == null || state == null || !states.ContainsKey(state.Name)) return;

            if (actifStates != null)
                if (actifStates.Contains(state)) { actifStates?.Remove(state); }

            state?.Unintialize();

            onStateRemoved?.Invoke(state);
            states?.Remove(state.Name);
        }

        public void RemoveActifState(State state)
        {
            if (states == null || state == null || !states.ContainsKey(state.Name)) return;
            else if (actifStates == null || state == null || !actifStates.Contains(state)) return;

            if (currentState == state) ChangeState(null as State, true);
            Debug.Log("Removed: " + state);

            onActifStateRemoved?.Invoke(state);
            actifStates?.Remove(state);
        }

        public virtual void ChangeState(State state)
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
            if (states == null || actifStates == null || (this.currentState == state && checkSimilarity)) return;
            else if (state != null) {
                if (!states.ContainsKey(state.Name) && !actifStates.Contains(state))
                    return;
            }

            onStateExited?.Invoke(currentState);
            onStateEntered?.Invoke(state);
        }

        public bool IsCurrentState(State state)
        {
            return currentState == state;
        }

        protected virtual bool CanAddState(State state)
        {
            if (states == null || state == null || states.ContainsKey(state.Name) || State.GetStateMachine(state) == null) return false;
            return true;
        }

        protected virtual void OnEnabled() { }
        protected virtual void OnDisabled() { }

        protected virtual void OnStateAdded(State state)
        {
            if (states != null) {
                foreach (State _state in GetStates()) {
                    _state?.AddTransitionState(state);
                    state?.AddTransitionState(_state);
                }
            }
        }
        protected virtual void OnStateRemoved(State state)
        {
            if (states != null) {
                foreach (State _state in GetStates()) {
                    _state?.RemoveTransitionState(state);
                    state?.RemoveTransitionState(_state);
                }
            }
        }

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
            return states.ContainsKey(state.Name);
        }

        public StateMachineEvent GetEvent(string eventName)
        {
            eventName = eventName.ToLower();
            if (events == null || string.IsNullOrEmpty(eventName) || !events.ContainsKey(eventName)) return null;

            StateMachineEventData data = events[eventName];
            return data != null ? data._Event : null;
        }

        public State GetState(string stateName)
        {
            if (states == null || string.IsNullOrEmpty(stateName) || !states.ContainsKey(stateName)) return null;
            return states[stateName].State;
        }

        public State[] GetStates()
        {
            List<State> states = new List<State>();
            if (states == null) return states.ToArray();

            foreach (var value in this.states.Values) {
                if (value == null) continue;
                states?.Add(value.State);
            }

            return states.ToArray();
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

        public void AddOnActifStateAddedListener(UnityAction<State> action)
        {
            if (action != null) onActifStateAdded?.AddListener(action);
        }

        public void RemoveOnActifStateAddedListener(UnityAction<State> action)
        {
            if (action != null) onActifStateAdded?.RemoveListener(action);
        }


        public void AddOnActifStateRemovedListener(UnityAction<State> action)
        {
            if (action != null) onActifStateRemoved?.AddListener(action);
        }

        public void RemoveOnActifStateRemovedListener(UnityAction<State> action)
        {
            if (action != null) onActifStateRemoved?.RemoveListener(action);
        }

        [System.Serializable]
        private class StateData {
            [SerializeField, SerializeReference, HideInInspector] private State state;
            public State State => state;

            public StateData(State state)
            {
                this.state = state;
            }
        }

        [System.Serializable]
        private class StateMachineEventData
        {
            [SerializeField, SerializeReference, HideInInspector] private StateMachineEvent _event;
            public StateMachineEvent _Event => _event;

            public StateMachineEventData(StateMachineEvent _event) {
                this._event = _event;
            }

        }
    }

    public abstract partial class StateMachine : MonoBehaviour
    {
#if UNITY_EDITOR
        [SerializeField, HideInInspector] private bool showEditorSettings;

        [SerializeField, HideInInspector] private bool showDebuggerSettings;

        [SerializeField, HideInInspector] private bool showDebugStates;
        [SerializeField, HideInInspector] private bool showDebugActifStates;

        [SerializeField, HideInInspector] private bool showDefaultSettings;
        [SerializeField, HideInInspector] private bool showStateFoldout;
        [SerializeField, HideInInspector] private bool showStateAddOrRemove;
        [SerializeField, HideInInspector] private bool showDefaultStates;
        [SerializeField, HideInInspector] private bool showDefaultEditorSettings;

        [SerializeField, HideInInspector] private int addStateSelectedIndex;

        [SerializeField, HideInInspector] private bool[] showStates;
        [SerializeField, HideInInspector] private bool[] lockStates;

        [SerializeField, HideInInspector] private Color defaultFoldoutColor;
        [SerializeField, HideInInspector] private Color defaultFieldColor;

        [SerializeField, HideInInspector] private Color defaultButtonColor;
        [SerializeField, HideInInspector] private Color defaultBackgroundColor;

        [SerializeField, HideInInspector] private InspectorVisualizer visualizer;

        protected virtual void OnValidate()
        {
            if (states != null) {
                foreach (State state in GetStates())
                    state?.Validate();
            }
        }

        public virtual void DrawInspector()
        {
            EditorExtension.DrawVerticalHelpBox(() => {
                EditorExtension.Space(5f);
                EditorExtension.IncrementIndent();

                if (EditorExtension.DisplayFoldout("Debuggger 🛠️", ref showDebuggerSettings, defaultFoldoutColor))
                {
                    EditorExtension.DrawVerticalHelpBox(() => {
                        DisplayDebugger(visualizer);
                    }, defaultBackgroundColor);
                }

                if (EditorExtension.DisplayFoldout("Default Settings ⚙️", ref showDefaultSettings, defaultFoldoutColor)) {

                    EditorExtension.DrawVerticalHelpBox(() => {
                       DisplayDefaultSettings(visualizer);
                    }, defaultBackgroundColor);
                }

                if (EditorExtension.DisplayFoldout($"States", ref showStateFoldout, defaultFoldoutColor)) {
                    Array values = GetInspectorValues();

                    if (values != null) {
                        EditorExtension.IncrementIndent();
                        EditorExtension.Space(5f);

                        if (EditorExtension.DisplayFoldout("Add / Remove", ref showStateAddOrRemove, defaultFoldoutColor))
                        {
                            EditorExtension.DrawVerticalHelpBox(() => {
                                AddSpecificState(values, ref addStateSelectedIndex);
                                EditorExtension.Space(10f);

                                EditorExtension.DrawHorizontal(() => {
                                    DisplayAddAllStateButton(values);
                                    DisplayRemoveAllStateButton();
                                });

                            }, defaultBackgroundColor, false);
                        }

                        EditorExtension.Space(5f);

                        if (EditorExtension.DisplayFoldout("Show", ref showDefaultStates, defaultFoldoutColor)) {
                            EditorExtension.DrawVerticalHelpBox(() => {
                                DisplayStates(values);
                            }, defaultBackgroundColor);
                        }

                        EditorExtension.Space(10f);
                        EditorExtension.DecrementIndent();
                    }
                }


                if (EditorExtension.DisplayFoldout("Editor Settings", ref showEditorSettings, defaultFoldoutColor))
                {
                    EditorExtension.DrawVerticalHelpBox(() => {
                        visualizer?.DrawInpsector("Default");
                        EditorExtension.Space(10f);
                    }, defaultBackgroundColor, true);
                }

                EditorExtension.DecrementIndent();
            }, defaultBackgroundColor);


        }

        private void AddSpecificState(Array values, ref int selectedState)
        {

            if (values == null) return;
            List<string> inspectorStatesNames = new List<string>();
            List<int> inspectorStatesIndexes = new List<int>();

            for (int i = 0; i < values.Length; i++)
            {
                var value = values.GetValue(i);
                if (value == null || ContainsState(value.ToString())) continue;

                EditorExtension.Space(10f);

                EditorExtension.DrawHorizontal(() => {
                    EditorExtension.DisplayBoldLabel(value.ToString());
                    EditorExtension.DisplayButton($"Add {value.ToString()} State ➕", () => { AddState(GetInspectorState(i)); });
                });
            }

            if (inspectorStatesNames.Count > 0) {
                int _stateIndex = selectedState;

                EditorExtension.DrawHorizontalHelpBox(() => {
                    _stateIndex = Mathf.Clamp(_stateIndex, 0, inspectorStatesIndexes.Count - 1);
                    _stateIndex = EditorGUILayout.Popup(_stateIndex, inspectorStatesNames.ToArray());
                }, defaultBackgroundColor);

                selectedState = _stateIndex;
            }
        }

        private void DisplayAddAllStateButton(Array values)
        {
            if (values == null || states == null || states.Count == values.Length) return;

            EditorExtension.DisplayButton("Add All States ➕", () => {
                for (int i = 0; i < values.Length; i++)
                    AddState(GetInspectorState(i));
            });
        }

        private void DisplayRemoveAllStateButton()
        {
            if (states == null || states.Count <= 0) return;

            EditorExtension.DisplayButton("Remove All States ➕", () => {
                State[] _states = GetStates();

                for (int i = 0; i < _states.Length; i++)
                    RemoveState(_states[i]);
            });
        }

        protected virtual void DisplayDebugger(InspectorVisualizer visualizer) {
            if (visualizer == null) return;

            if (Application.isPlaying)
            {
                EditorExtension.DrawHorizontal(() =>
                {
                    EditorExtension.DisplayBoldLabel("Current State");
                    EditorExtension.DisplayBoldLabel(currentState != null ? currentState.name : "None");
                });

                if(actifStates != null) {
                    EditorExtension.IncrementIndent();
                    EditorExtension.Space(10f);

                    if (EditorExtension.DisplayFoldout("Actif States", ref showDebugStates, visualizer.FoldoutColor))
                    {
                        foreach(var state in actifStates) {
                             if(state == null) continue;

                            EditorExtension.Space(10f);
                            EditorExtension.DisplayBoldLabel(state.Name);
                        }
                    }

                    EditorExtension.DecrementIndent();
                }

            }
            if(states != null) {
                EditorExtension.IncrementIndent();
                if (EditorExtension.DisplayFoldout("States", ref showDebugStates, visualizer.FoldoutColor)) {
                    foreach(StateData data in states.Values) {
                        State state = data != null ? data.State : null;
                        if (state == null) continue;

                        EditorExtension.Space(10f);
                        EditorExtension.DisplayBoldLabel(state.Name);
                    }
                }

                EditorExtension.DecrementIndent();
            }
        }

        protected virtual void DisplayDefaultSettings(InspectorVisualizer visualizer) {
            enableStateAutomatically = EditorExtension.DisplayToggle("Enable State Automatically", enableStateAutomatically);
        }

        private void DisplayStates(Array values)
        {
            if (values == null) return;

            SetFoldoutChecks(ref showStates, values.Length);
            SetFoldoutChecks(ref lockStates, values.Length);

            for (int i = 0; i < values.Length; i++) {
                if (values.GetValue(i) == null) continue;;
                DisplayState(values.GetValue(i).ToString(), i, ref showStates[i]);
            }
        }

        private void SetFoldoutChecks(ref bool[] values, int length)
        {
            if (values == null || values.Length != length)
                values = new bool[length];
        }

        private void DisplayState(string stateName, int stateIndex, ref bool stateShown)
        {
            State state = GetState(stateName);
            string label = $"{stateName} " + (lockStates[stateIndex] ? "🔒" : "🔓");
            if (state == null || visualizer == null) return;
            EditorExtension.Space(10f);

            if (EditorExtension.DisplayFoldout(label, ref stateShown, visualizer.FoldoutColor)) {
                EditorExtension.DrawVerticalHelpBox(() => {
                    EditorExtension.Space(5f);

                    EditorGUILayout.BeginHorizontal();
                    EditorExtension.DisplayButton(lockStates[stateIndex] ? "Unlock State 🔓" : "Lock State 🔒", visualizer.ButtonColor, () => { lockStates[stateIndex] = !lockStates[stateIndex]; });
                    EditorExtension.DisplayButton($"Reset {stateName} 🔄", visualizer.ButtonColor, () => {
                        RemoveState(GetState(stateName));
                        AddState(GetInspectorState(stateIndex));
                    });


                    if (!lockStates[stateIndex])
                        EditorExtension.DisplayButton($"Remove {stateName} ➖", visualizer.ButtonColor, () => { RemoveState(state); });

                    EditorGUILayout.EndHorizontal();
                    EditorExtension.Space(5f);
                }, visualizer.BackgroundColor, true);

                EditorExtension.Space(5f);
                state?.DrawInpsector(visualizer);
            }
        }


        protected abstract State GetInspectorState(int stateIndex);
        protected abstract Array GetInspectorValues();

        [System.Serializable]
        public class InspectorVisualizer {
            [SerializeField, HideInInspector] private bool showVisualizer;

            [SerializeField, HideInInspector] private Color foldoutColor;
            [SerializeField, HideInInspector] private Color buttonColor;
            [SerializeField, HideInInspector] private Color backgroundColor;

            public Color FoldoutColor => foldoutColor;
            public Color ButtonColor => buttonColor;
            public Color BackgroundColor => backgroundColor;

            public void SetFoldoutColor(Color foldoutColor) { this.foldoutColor = foldoutColor; }
            public void SetButtonColor(Color buttonColor)   { this.buttonColor  = buttonColor;  }
            public void SetBackgroundColor(Color backgroundColor) { this.backgroundColor = backgroundColor; }

            private InspectorVisualizer() { }

            public void DrawInpsector(string foldoutLabel) {
                if(EditorExtension.DisplayFoldout(foldoutLabel, ref showVisualizer, foldoutColor)) {
                    EditorExtension.Space(10f);
                    this.foldoutColor = EditorExtension.DisplayColorField("Foldout Color 🎨", foldoutColor);

                    EditorExtension.Space(5f);
                    EditorExtension.DisplayButton("Reset Foldout Color", buttonColor, () => { foldoutColor = new Color(0f, 0f, 0f, 0.6588f); });

                    EditorExtension.Space(10f);
                    this.buttonColor = EditorExtension.DisplayColorField("Button Color 🎨", buttonColor);

                    EditorExtension.Space(5f);
                    EditorExtension.DisplayButton("Reset Button Color", buttonColor, () => { buttonColor = new Color(0f, 0f, 0f, 0.4627f); });

                    EditorExtension.Space(10f);
                    backgroundColor = EditorExtension.DisplayColorField("Background Color 🎨", backgroundColor);

                    EditorExtension.Space(5f);
                    EditorExtension.DisplayButton("ResetBackground Color", buttonColor, () => { backgroundColor = new Color(0.161f, 0.161f, 0.161f, 0.008f); });
                }
            }
        }
#endif
    }
}
