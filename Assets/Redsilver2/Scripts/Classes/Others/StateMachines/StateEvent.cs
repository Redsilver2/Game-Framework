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
         
        public void DrawInspector(StateMachine.StateInspectorVisualizer visualizer) {

            if (visualizer == null) return;

            EditorExtension.DrawVerticalHelpBox(() =>{
                DrawInspectorSettings(visualizer);
            }, visualizer.BackgroundColor, true);
        }

        protected virtual void DrawInspectorSettings(StateMachine.StateInspectorVisualizer visualizer) { }

        protected static void DrawInspector(State state, string eventName, ref bool showEvent, StateMachine.StateInspectorVisualizer visualizer, UnityAction onAddUpdate, UnityAction onRemoveUpdate)
        {
            if (state == null) return;
            EditorExtension.Space(5f);
            EditorExtension.IncrementIndent();

            if (EditorExtension.DisplayFoldout(eventName, ref showEvent, visualizer.FoldoutColor)) {
                StateEvent _event = state.GetEvent(eventName);

                if (_event == null) { onAddUpdate?.Invoke(); }
                else { 
                    onRemoveUpdate?.Invoke();
                    _event?.DrawInspector(visualizer);
                }
            }

            EditorExtension.DecrementIndent();
        }
#endif 
    }
}
