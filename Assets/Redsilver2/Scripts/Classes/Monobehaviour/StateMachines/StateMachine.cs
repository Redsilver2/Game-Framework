using RedSilver2.Framework.StateMachines.States;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

namespace RedSilver2.Framework.StateMachines
{
    public abstract class StateMachine : MonoBehaviour
    {
        [SerializeField, SerializeReference, HideInInspector] private List<State> states = new List<State>();

        private State currentState;
        private List<State> actifStates;

        private UnityEvent onEnabled, onDisabled;

        private UnityEvent<State> onStateAdded, onStateRemoved;
        private UnityEvent<State> onStateEntered, onStateExited;

        private UnityEvent<State> onActifStateAdded, onActifStateRemoved;

        public State CurrentState => currentState;
        public State[] States     => states != null ? states.ToArray() : new State[0];
        public State[] ActifStates => actifStates != null ? actifStates.ToArray() : new State[0];




#if UNITY_EDITOR
       [SerializeField, HideInInspector] private bool showEditorSettings;
       [SerializeField, HideInInspector] private bool showDefaultSettings;
       [SerializeField, HideInInspector] private bool showStateCreation;
       
       [SerializeField, HideInInspector] private bool[] showStates;
       [SerializeField, HideInInspector] private bool[] lockStates;

       [SerializeField, HideInInspector] private Color foldoutColor = new Color(0f, 0f, 0f, 0.525f);
       [SerializeField, HideInInspector] private Color fieldColor   = new Color(0.2f, 0.2f, 0.2f, 1f);

        protected Color FoldoutColor => foldoutColor;
        protected Color FieldColor   => fieldColor;

        protected virtual void OnValidate()  {
            if (states != null) {
                states = states.Where(x => x != null).ToList();

                foreach (State state in states)
                   state?.Validate();
            }
        }

        public virtual void DrawInspector() {
            EditorExtension.IncrementIndent();

            if (EditorExtension.DisplayFoldout("Default Settings ⚙️", ref showDefaultSettings, foldoutColor)) {
                EditorExtension.IncrementIndent();
                EditorExtension.Space(5f);
                
                DisplayDefaultSettings(foldoutColor, fieldColor);
                EditorExtension.DecrementIndent();
            }

            EditorGUILayout.Space(5f);

            if (EditorExtension.DisplayFoldout($"States", ref showStateCreation, foldoutColor)) {
                EditorExtension.Space(5f);
                Array values = GetInspectorValues();

                if(values != null) {
                    EditorExtension.IncrementIndent();
                    DisplayAddAllStateButton(values);
                    DisplayRemoveAllStateButton();

                    EditorExtension.Space(5f);
                    DisplayStates(values);
                    EditorExtension.DecrementIndent();
                }
            }

            EditorExtension.Space(5f);

            if (EditorExtension.DisplayFoldout("Editor Settings", ref showEditorSettings, foldoutColor)) {
                EditorExtension.IncrementIndent();
                EditorExtension.Space(5f);

                foldoutColor = EditorExtension.DisplayColorField("Foldout Color 🎨", foldoutColor, fieldColor);        
                fieldColor   = EditorExtension.DisplayColorField("Field Color 🎨", fieldColor, fieldColor);


                EditorExtension.Space(2.5f);

                EditorExtension.DisplayButton("Reset Colors", () => {
                    foldoutColor = new Color(0f, 0f, 0f, 0.525f);
                    fieldColor   = new Color(0.2f, 0.2f, 0.2f, 1f);
                }); 

                EditorExtension.DecrementIndent();
            }

            EditorExtension.DecrementIndent();
        }

        private void DisplayAddAllStateButton(Array values) {
            if (values == null || states == null || states.Count == values.Length) return;

           EditorExtension.DisplayButton("Add All States ➕", () => {
               for (int i = 0; i < values.Length; i++) {
                   AddState(GetInspectorState(i));
               }
           });
        }

        private void DisplayRemoveAllStateButton()
        {
            if (states == null || states.Count <= 0) return;

            EditorExtension.DisplayButton("Remove All States ➕", () => {
                State[] _states = states.ToArray();

                for (int i = 0; i < _states.Length; i++)
                    RemoveState(_states[i]);
            });
        }

        protected virtual void DisplayDefaultSettings(Color foldoutColor, Color fieldColor) {

        }

        private void DisplayStates(Array values) {
            if (values == null) return;

            SetFoldoutChecks(ref showStates, values.Length);
            SetFoldoutChecks(ref lockStates, values.Length);

            for (int i = 0; i < values.Length; i++) {
                if (values.GetValue(i) == null) continue;
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
            State state  = GetState(stateName);
            string label = state != null ? $"{stateName} ✅ " + (lockStates[stateIndex] ? "🔒" : "🔒" )  : $"{stateName} ❌";

            if (EditorExtension.DisplayFoldout(label, ref stateShown, foldoutColor)){
                if (state == null) {
                    EditorExtension.Space(5f);
                    EditorExtension.DisplayButton($"Add {stateName} ➕", () => { AddState(GetInspectorState(stateIndex)); });
                }
                else {
                    EditorExtension.Space(5f);
                    EditorExtension.DisplayButton(lockStates[stateIndex] ? "Unlock State 🔒" : "Lock State 🔒", () => { lockStates[stateIndex] = !lockStates[stateIndex]; });

                    if (!lockStates[stateIndex]) 
                        EditorExtension.DisplayButton($"Remove {stateName} ➖", () => { RemoveState(state); });
  
                    EditorExtension.Space(5f);
                    state?.DrawInpsector(foldoutColor, fieldColor);
                }

                EditorExtension.Space(5f);
            }

            EditorExtension.Space(2.5f);
        }


        protected abstract State GetInspectorState(int stateIndex);
        protected abstract Array GetInspectorValues();
#endif

        protected virtual void Awake()
        {
            actifStates = new List<State>();

            onEnabled  = new UnityEvent();
            onDisabled = new UnityEvent();

            onStateAdded   = new UnityEvent<State>();
            onStateRemoved = new UnityEvent<State>();

            onStateEntered = new UnityEvent<State>();
            onStateExited  = new UnityEvent<State>();

            onActifStateAdded   = new UnityEvent<State>();
            onActifStateRemoved = new UnityEvent<State>();

            AddOnStateAddedListener(OnStateAdded);
            AddOnStateRemovedListener(OnStateRemoved);

            AddOnStateEnteredListener(OnStateEntered);
            AddOnStateExitedListener(OnStateExited);

            AddOnDisabledListener(OnDisabled);
            AddOnEnabledListener(OnEnabled);

        }

        private void OnDisable() { onDisabled?.Invoke(); }
        private void OnEnable() { onEnabled?.Invoke(); }

        protected void EnableState(State state)
        {
            state?.Enable();
        }

        protected void DisableState(State state)
        {
            state?.Disable();
        }

        public void AddState(State state)
        {
            if (states == null || !CanAddState(state) || states.Contains(state)) return;
            state?.Added();

            states?.Add(state);
            onStateAdded?.Invoke(state);
        }

        public void AddActifState(State state)
        {
            if (states == null || !states.Contains(state)) return;
            else if (actifStates == null || state == null || actifStates.Contains(state)) return;
        

            actifStates?.Add(state);
            onActifStateAdded?.Invoke(state);
        }

        public void RemoveState(State state)
        {
            if (states == null || state == null || !states.Contains(state)) return;
   
            if(actifStates != null)
                if (actifStates.Contains(state)) { actifStates?.Remove(state); }

            state?.Removed();
            onStateRemoved?.Invoke(state);
          
            states?.Remove(state);
        }

        public void RemoveActifState(State state)
        {
            if (states == null || !states.Contains(state)) return;
            else if (actifStates == null || state == null || !actifStates.Contains(state)) return;

            if(currentState == state) ChangeState(null as State, true);
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
            else if (state != null && !states.Contains(state) && !actifStates.Contains(state)) return;

            onStateExited?.Invoke(currentState);
            onStateEntered?.Invoke(state);
        }

        public bool IsCurrentState(State state)
        {
            return currentState == state;
        }

        protected virtual bool CanAddState(State state)
        {
            if (states == null || state == null || states.Contains(state) || State.GetStateMachine(state) == null) return false;
            return true;
        }

        protected virtual void OnEnabled() { }
        protected virtual void OnDisabled() { }

        protected virtual void OnStateAdded(State state)
        {
            if (states != null) {
                foreach (State _state in states) {
                    _state?.AddTransitionState(state);
                    state?.AddTransitionState(_state);
                }
            }
        }
        protected virtual void OnStateRemoved(State state)
        {
            if (states != null) {
                foreach (State _state in states) {
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
            return states.Contains(state);
        }

        public State GetState(string stateName)
        {
            if (states == null || string.IsNullOrEmpty(stateName)) return null;

            for (int i = 0; i < states.Count; i++) {
                if(states[i] == null) continue;
                string _state = states[i].Name;

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

        public void AddOnActifStateAddedListener(UnityAction<State> action)
        {
            if(action != null) onActifStateAdded?.AddListener(action);
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
    }
}
