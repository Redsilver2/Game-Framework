using RedSilver2.Framework.StateMachines.States;
using UnityEngine;

namespace RedSilver2.Framework.StateMachines.Events
{
    [System.Serializable]
    public abstract class MovementAudio : MovementStateEvent
    {
        [Space]
        [SerializeField] private AudioSource source;
        protected AudioSource Source => source;

        protected MovementAudio(MovementState state) : base(state) { }

        public void SetSource(AudioSource source)
        {
            this.source = source;
        }

#if UNITY_EDITOR
        protected override void DrawInspectorSettings(StateMachine.InspectorVisualizer visualizer) {
            base.DrawInspectorSettings(visualizer);
            source = EditorExtension.DisplayCustomField("Audio Source", true, source);
        }
#endif
    }
}