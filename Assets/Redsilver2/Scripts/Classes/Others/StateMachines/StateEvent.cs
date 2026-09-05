using RedSilver2.Framework.StateMachines.States;
using UnityEngine;
using UnityEngine.Events;

namespace RedSilver2.Framework.StateMachines.Events
{
    [System.Serializable]
    public abstract class StateEvent
    {
        [SerializeReference, HideInInspector] private State state;
        private bool isEnabled = false;

        public bool IsEnabled => isEnabled;

        protected StateEvent(State state)  {
            this.state = state;
            isEnabled  = false;
        }

        public void Enable() {
            if (!isEnabled) {
                Enable(state);
                isEnabled = true;
            }
        }

        public void Disable() {
            if (isEnabled)  {
                Disable(state);
                isEnabled = false;
            }
        }

        protected abstract void Enable(State state);
        protected abstract void Disable(State state);
        protected abstract string GetName();

        public bool IsOwner(State state) {
            return this.state == state;
        }

#if UNITY_EDITOR
       [SerializeField, HideInInspector] private bool showSettings;
         
        public void DrawInspector(StateMachine.InspectorVisualizer visualizer) {

            if (visualizer == null) return;

            EditorExtension.DrawVerticalHelpBox(() =>{
                DrawInspectorSettings(visualizer);
            }, visualizer.BackgroundColor, true);
        }

        protected virtual void DrawInspectorSettings(StateMachine.InspectorVisualizer visualizer) { }

        protected static void DrawInspector(State state, string eventName, ref bool showEvent, StateMachine.InspectorVisualizer visualizer, UnityAction onAddUpdate, UnityAction onRemoveUpdate)
        {
            if (state == null || visualizer == null) return;
            EditorExtension.IncrementIndent();

            if (EditorExtension.DisplayFoldout(eventName, ref showEvent, visualizer.FoldoutColor)) {
                if (!state.ContainsEvent(eventName)) { onAddUpdate?.Invoke(); }
                else { onRemoveUpdate?.Invoke(); }
            }

            EditorExtension.DecrementIndent();
        }
#endif 
    }
}
