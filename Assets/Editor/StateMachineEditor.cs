using RedSilver2.Framework.StateMachines.States;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

namespace RedSilver2.Framework.StateMachines.EditorTools
{

    [CustomEditor(typeof(StateMachine))]
    public abstract class StateMachineEditor : Editor {
        private bool showAllStates = false;
        private bool showDefaultSettings;

        private bool[] showStates;
        private bool[] showStateBaseSettings;
        private bool[] showStateExtensions;
        private bool[] showStateTransitions;

        public override void OnInspectorGUI() {
            StateMachine stateMachine = target as StateMachine;
            serializedObject.Update();
          
            Undo.RecordObject(stateMachine, "Changed Any Asset Field");
            EditorGUILayout.Space(5f);

            if (DisplayFoldout("Default Settings ⚙️", ref showDefaultSettings)) {
                EditorGUILayout.Space(5f);
                DisplayDefaultSettings(stateMachine);
            }

            EditorGUILayout.Space(5f);

            if (DisplayFoldout($"States", ref showAllStates)) {

                EditorGUILayout.Space(5f);
               
                DisplayAddAllStateButton(stateMachine);
                DisplayRemoveAllStateButton(stateMachine);

                EditorGUILayout.Space(5f);
                DisplayStates(stateMachine, GetValues());
            }

            if (GUI.changed) EditorUtility.SetDirty(stateMachine);

            serializedObject.ApplyModifiedProperties();
        }

        private void DisplayAddAllStateButton(StateMachine stateMachine)
        {
            Array values = GetValues();
            if (stateMachine == null || values == null) return;
            else if (stateMachine.States.Length != values.Length - 1) {
                DisplayButton("Add All States ➕", () => {
                    for (int i = 0; i < values.Length; i++) {
                        if (values.GetValue(i) == null) continue;
                        AddState(stateMachine, values.GetValue(i).ToString(), i);
                    }
                });
            }
        }


        private void DisplayRemoveAllStateButton(StateMachine stateMachine)
        {
            State[] states = stateMachine != null ? stateMachine.States : null;

            if (states == null) return;
            else if (states.Length > 0) {
                DisplayButton("Remove All States ➖", () => {
                    if (states != null) {
                        foreach (State state in states)
                            stateMachine?.RemoveState(state);
                    }
                });
            }

        }

        protected void DisplayButton(string label, UnityAction clickAction)
        {
            if (GUILayout.Button(label))
                clickAction?.Invoke();
        }

        private void DisplayStates(StateMachine stateMachine, Array values) {
            if (values == null || stateMachine == null) return;
            List<string> _values = new List<string>();

            SetFoldoutChecks(ref showStates           , values.Length);
            SetFoldoutChecks(ref showStateBaseSettings, values.Length);
           
            SetFoldoutChecks(ref showStateExtensions  , values.Length);
            SetFoldoutChecks(ref showStateTransitions , values.Length);

            foreach (object value in values) {
                if (value == null) continue;
                _values?.Add(value.ToString());
            }

            DisplayStates(stateMachine, _values.ToArray());
        }

        private void DisplayStates(StateMachine stateMachine, string[] values) {
            if (values == null || stateMachine == null) return;
               
            for (int i = 0; i < values.Length; i++) 
                    DisplayStates(stateMachine, values[i], i);
        }


        private void DisplayStates(StateMachine stateMachine, string stateName, int stateIndex) {
            if (stateMachine == null || showStates == null || stateIndex < 0 || stateIndex >= showStates.Length) return;
            else if (!string.IsNullOrEmpty(stateName)) {
                Rect rect = EditorGUILayout.BeginVertical("HelpBox");
                EditorGUILayout.Space(5f);

                if (Event.current.type == EventType.Repaint) EditorGUI.DrawRect(rect, new Color(0.2f, 0.2f, 0.2f, 0.8f));
                DisplayStates(stateMachine, stateMachine.GetState(stateName), stateName, stateIndex, ref showStates[stateIndex]);
               
                EditorGUILayout.Space(5f);      
                EditorGUILayout.EndVertical();
            }
        }

        private void DisplayStates(StateMachine stateMachine, State state, string stateName, int stateIndex, ref bool stateShown) {
            string label = state != null ? $"{stateName} | ✅" : $"{stateName} | ❌";

            if (DisplayFoldout(label, ref stateShown)) {
                if (state == null) {
                    EditorGUILayout.Space();
                    DisplayButton($"Add {stateName} ➕", () => { AddState(stateMachine, stateName, stateIndex); });
                }
                else {
                    EditorGUILayout.Space();
                    
                    DisplayButton($"Remove {stateName} ➖", () => { RemoveState(stateMachine, state); });
                    DisplayStateSettings(state, ref showStateBaseSettings[stateIndex]);
                 
                    DisplayStateExtensions(stateMachine, state, ref showStateExtensions[stateIndex]);
                    DisplayStateTransitions(state, ref showStateTransitions[stateIndex]);
                }
            }
        }

        private void DisplayStateTransitions(State state, ref bool showStateTransitions) {
            if (state == null) return;

            if (DisplayFoldout("Transitions", ref showStateTransitions)) {
                Array values = GetValues();
                string compatileStates    = "Compatible States:";
                string incompatibleStates = "Incompatible States:";

                foreach (object value in values) {
                    if (state.IncompatibleTransitionStates.Contains(value.ToString().ToLower())) {
                        incompatibleStates += $"\n{value.ToString()}";
                    }
                    else { compatileStates += $"\n{value.ToString()}"; }
                }

                EditorGUILayout.HelpBox(compatileStates, MessageType.Info);
                EditorGUILayout.HelpBox(incompatibleStates, MessageType.Error);
            }
        }

        protected virtual void DisplayStateExtensions(StateMachine stateMachine, State state, ref bool showExtension) {
            if (DisplayFoldout("Extensions", ref showExtension)) {
                DisplayStateExtensions(stateMachine, state);
            }
        }


        protected virtual void AddState(StateMachine stateMachine, string stateName, int stateIndex)
        {
            if (stateMachine == null || stateMachine.ContainsState(stateName)) return;
            stateMachine?.AddState(GetState(stateMachine, stateIndex));
        }

        private void RemoveState(StateMachine stateMachine, State state)
        {
            stateMachine?.RemoveState(state);
        }

        private void SetFoldoutChecks(ref bool[] values, int length)
        {
            if (values == null || values.Length != length)
                values = new bool[length];
        }

        protected uint DisplayUIntSlider(string label, uint currentValue, uint maxValue){
            return (uint)DisplayIntSlider(label, (int)currentValue, (int)uint.MinValue, (int)maxValue);
        }

        protected int DisplayIntSlider(string label, int currentValue, int minValue, int  maxValue)
        {
            Rect rect = EditorGUILayout.BeginHorizontal("HelpBox");
            if (Event.current.type == EventType.Repaint) EditorGUI.DrawRect(rect, new Color(0.225f, 0.225f, 0.225f, 0.8f));

            EditorGUILayout.LabelField(label, EditorStyles.boldLabel);
            int newValue = EditorGUILayout.IntSlider(currentValue, minValue, maxValue);

            EditorGUILayout.EndHorizontal();
            return newValue;
        }

        protected float DisplayFloatSlider(string label, float currentValue, float minValue, float maxValue)
        {
            Rect rect = EditorGUILayout.BeginHorizontal("HelpBox");
            if (Event.current.type == EventType.Repaint) EditorGUI.DrawRect(rect, new Color(0.225f, 0.225f, 0.225f, 0.8f));

            EditorGUILayout.LabelField(label, EditorStyles.boldLabel);
            float newValue = EditorGUILayout.Slider(currentValue, minValue, maxValue);

            EditorGUILayout.EndHorizontal();
            return newValue;
        }


        protected Vector3 DisplayVector3Field(string label, Vector3 currentValue)
        {
            Rect rect = EditorGUILayout.BeginHorizontal("HelpBox");
            if (Event.current.type == EventType.Repaint) EditorGUI.DrawRect(rect, new Color(0.225f, 0.225f, 0.225f, 0.8f));

            EditorGUILayout.LabelField(label, EditorStyles.boldLabel);
            currentValue = EditorGUILayout.Vector3Field(string.Empty, currentValue);


            EditorGUILayout.EndHorizontal();
            return currentValue;
        }


        protected bool DisplayToggle(string label, bool currentValue)
        {
            Rect rect = EditorGUILayout.BeginHorizontal("HelpBox");
            if (Event.current.type == EventType.Repaint) EditorGUI.DrawRect(rect, new Color(0.225f, 0.225f, 0.225f, 0.8f));

            EditorGUILayout.LabelField(label, EditorStyles.boldLabel);
            currentValue = EditorGUILayout.Toggle(string.Empty, currentValue);
          
            EditorGUILayout.EndHorizontal();
            return currentValue;
        }

        protected bool DisplayFoldout(string label, ref bool currentValue)
        {
            Rect rect = EditorGUILayout.BeginVertical("HelpBox");
            if (Event.current.type == EventType.Repaint) EditorGUI.DrawRect(rect, new Color(0.15f, 0.15f, 0.15f, 0.8f));

            currentValue = EditorGUILayout.Foldout(currentValue, label, true);
            EditorGUILayout.EndVertical();
            return currentValue;
        }

        public T DisplayCustomField<T>(string label, bool isScenePrefabAllowed, T value) where T : UnityEngine.Object
        {
            Rect rect = EditorGUILayout.BeginHorizontal("HelpBox");
            if (Event.current.type == EventType.Repaint) EditorGUI.DrawRect(rect, new Color(0.225f, 0.225f, 0.225f, 0.8f));

            EditorGUILayout.LabelField(label, EditorStyles.boldLabel);

            T result = (T)EditorGUILayout.ObjectField(
              value,
              typeof(T),
              isScenePrefabAllowed
            );

            EditorGUILayout.EndHorizontal();
            return result;
        }

        protected abstract void DisplayDefaultSettings(StateMachine stateMachine);

        protected abstract void DisplayStateSettings(State state, ref bool showBaseSettings);
        protected abstract void DisplayStateExtensions(StateMachine stateMachine, State state);
       
        protected abstract State GetState(StateMachine stateMachine, int stateIndex);
        protected abstract Array GetValues();
    }


}