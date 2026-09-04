using RedSilver2.Framework.StateMachines.Events;
using RedSilver2.Framework.StateMachines.States;
using UnityEngine;
using UnityEngine.Events;

namespace RedSilver2.Framework.StateMachines.Extensions
{
    [System.Serializable]
    public sealed partial class LandSound : MovementAudio
    {
        [Space]
        [SerializeField] private MovementSoundData data;
        private const string EVENT_NAME = "Land Audio";

        public LandSound(LandState state) : base(state) { }

        protected sealed override void Enable(MovementState state) {
            state?.AddOnEnteredListener(GetOnEnteredListener(state));
        }

        protected sealed override void Disable(MovementState state)
        {
            state?.RemoveOnEnteredListener(GetOnEnteredListener(state));
        }

        private UnityAction GetOnEnteredListener(MovementState state)
        {
            return () => {
                Enter(Source, data, MovementState.GetMovementStateMachine(state));
            };
        }


        protected sealed override string GetName() {
            return "Land Audio";
        }

        private void Enter(AudioSource source, MovementSoundData data, MovementStateMachine stateMachine)
        {
            if (stateMachine == null || source == null || data == null) return;
            AudioClip[] clips = data.GetClips(stateMachine.GroundTag);

            if (clips == null || clips.Length == 0) return;
            AudioClip clip = clips[Random.Range(0, clips.Length - 1)];

            if (source == null) return;
            source.clip = clip;
            source.Play();
        }

        public static LandSound GetEvent(MovementState state) {
            if (state == null) return null;
            return state.GetEvent(EVENT_NAME) as LandSound;
        }
    }

    public sealed partial class LandSound : MovementAudio {
#if UNITY_EDITOR
        public static void DrawInspector(LandState state, ref bool showEvent, StateMachine.StateInspectorVisualizer visualizer)
        {
            DrawInspector(state, EVENT_NAME, ref showEvent, visualizer,
              () => {
                  EditorExtension.DisplayButton($"Add {EVENT_NAME}", () => { state?.AddEvent(EVENT_NAME, new LandSound(state)); });
              },
              () =>
              {
                  EditorExtension.DisplayButton($"Remove {EVENT_NAME}", () => { state?.RemoveEvent(EVENT_NAME); });
              });
        }
#endif
    }
}
