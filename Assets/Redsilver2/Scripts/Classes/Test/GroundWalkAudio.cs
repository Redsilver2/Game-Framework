using RedSilver2.Framework.StateMachines.Events;
using RedSilver2.Framework.StateMachines.States;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

namespace RedSilver2.Framework.StateMachines.Extensions
{
    [RequireComponent(typeof(AudioSource))]
    [System.Serializable]
    public sealed partial class GroundWalkAudio : MovementAudio
    {
        [Space]
        [SerializeField] private MovementSoundData data;

        [Space]
        [SerializeField] private float soundTriggerTime;

        private float currentSoundTriggerTime;
        private const string EVENT_NAME = "Ground Walk Audio";

        private GroundWalkAudio(WalkState state) : base(state) {

        }

        private GroundWalkAudio(RunState state) : base(state) {

        }

        private GroundWalkAudio(CrouchState state) : base(state) {

        }


        protected sealed override void Enable(MovementState state)
        {
            currentSoundTriggerTime = 0f;
            state?.AddOnUpdateListener(OnUpdate(state));
        }

        protected sealed override void Disable(MovementState state)
        {
            state?.RemoveOnUpdateListener(OnUpdate(state));
        }

        private UnityAction OnUpdate(MovementState state)
        {
            string groundTag = string.Empty;
            AudioClip[] clips = null;

            return () => {
                MovementStateMachine stateMachine = MovementState.GetMovementStateMachine(state);

                if (state == null || stateMachine == null || data == null) return;
                else if (groundTag != stateMachine.GroundTag) {
                    groundTag = stateMachine.GroundTag;
                    clips = data.GetClips(groundTag);
                    currentSoundTriggerTime = soundTriggerTime;
                }
                else { currentSoundTriggerTime += Time.deltaTime; }

                Debug.Log("A");

                if (currentSoundTriggerTime >= soundTriggerTime) {
                    currentSoundTriggerTime = 0f;

                    if (clips != null) {
                        AudioClip clip = clips.Length <= 0 ? null : clips[Random.Range(0, clips.Length)];
                        if (clip != null) { Source?.PlayOneShot(clip); }
                    }
                }

            };
        }

        protected sealed override string GetName() {
            return EVENT_NAME;
        }

        public static GroundWalkAudio GetEvent(MovementState state) {
            if (state == null) return null;
            return state.GetEvent(EVENT_NAME) as GroundWalkAudio;
        }
    }

    public sealed partial class GroundWalkAudio : MovementAudio {
#if UNITY_EDITOR
        protected override void DrawInspectorSettings(StateMachine.StateInspectorVisualizer visualizer)
        {
            base.DrawInspectorSettings(visualizer);
            if (visualizer == null) return;

            data             = EditorExtension.DisplayCustomField("Sound Data", false, data);
            soundTriggerTime = EditorExtension.DisplayFloatSlider("Sound Trigger Time", soundTriggerTime, 0f, 100f);
        }



        public static void DrawInspector(WalkState state, ref bool showEvent, StateMachine.StateInspectorVisualizer visualizer)
        {
            if(visualizer == null) return;

            DrawInspector(state, EVENT_NAME, ref showEvent, visualizer,
              () => {
                  EditorExtension.Space(10);
                  EditorExtension.DisplayButton($"Add {EVENT_NAME}", visualizer.ButtonColor, () => { state?.AddEvent(EVENT_NAME, new GroundWalkAudio(state)); });
              },
              () =>
              {

                  EditorExtension.Space(10);
                  EditorExtension.DisplayButton($"Remove {EVENT_NAME}", visualizer.ButtonColor, () => { state?.RemoveEvent(EVENT_NAME); });
              });
        }

        public static void DrawInspector(RunState state, ref bool showEvent, StateMachine.StateInspectorVisualizer visualizer)
        {
            if (visualizer == null) return;


            DrawInspector(state, EVENT_NAME, ref showEvent, visualizer,
                          () => {

                              EditorExtension.Space(10);
                              EditorExtension.DisplayButton($"Add {EVENT_NAME}", visualizer.ButtonColor, () => { state?.AddEvent(EVENT_NAME, new GroundWalkAudio(state)); });
                          },
                          () =>
                          {

                              EditorExtension.Space(10);
                              EditorExtension.DisplayButton($"Remove {EVENT_NAME}", visualizer.ButtonColor, () => { state?.RemoveEvent(EVENT_NAME); });
                          });
        }

        public static void DrawInspector(CrouchState state, ref bool showEvent, StateMachine.StateInspectorVisualizer visualizer)
        {
            if (visualizer == null) return;

            DrawInspector(state, EVENT_NAME, ref showEvent, visualizer,
                          () => {

                              EditorExtension.Space(10);
                              EditorExtension.DisplayButton($"Add {EVENT_NAME}", visualizer.ButtonColor, () => { state?.AddEvent(EVENT_NAME, new GroundWalkAudio(state)); });
                          }, 
                          () =>
                          {
                              EditorExtension.Space(10);
                              EditorExtension.DisplayButton($"Remove {EVENT_NAME}", visualizer.ButtonColor, () => { state?.RemoveEvent(EVENT_NAME); });
                          });
        }


    }

#endif
}

