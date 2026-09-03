using RedSilver2.Framework.StateMachines.Events;
using RedSilver2.Framework.StateMachines.States;
using UnityEngine;
using UnityEngine.Events;

namespace RedSilver2.Framework.StateMachines.Extensions
{
    [RequireComponent(typeof(AudioSource))]
    [System.Serializable]
    public sealed partial class MovementWalkSound : MovementSound
    {
        [Space]
        [SerializeField] private float soundTriggerTime;

        [Space]
        [SerializeField] private float minPitch;
        [SerializeField] private float maxPitch;

        [Space]
        [SerializeField] private float minVolume;
        [SerializeField] private float maxVolume;

        private float currentSoundTriggerTime;

        public MovementWalkSound(string name, WalkState state) : base(name, state)
        {

        }

        public MovementWalkSound(string name, RunState state) : base(name, state)
        {

        }

        public MovementWalkSound(string name, CrouchState state) : base(name, state)
        {

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
                MovementSoundData data = Data;

                if (state == null || stateMachine == null || data == null) return;
                else if (groundTag != stateMachine.GroundTag)
                {
                    groundTag = stateMachine.GroundTag;
                    clips = data.GetClips(groundTag);
                    currentSoundTriggerTime = soundTriggerTime;
                }
                else { currentSoundTriggerTime += Time.deltaTime; }

                if (currentSoundTriggerTime >= soundTriggerTime)
                {
                    AudioSource source = Source;
                    currentSoundTriggerTime = 0f;

                    if (source != null && clips != null)
                    {

                        AudioClip clip = clips.Length <= 0 ? null : clips[Random.Range(0, clips.Length)];
                        if (clip != null)
                        {
                            source.volume = Random.Range(minVolume, maxVolume);
                            source.pitch = Random.Range(minPitch, maxPitch);
                            source?.PlayOneShot(clip);
                        }
                    }
                }

            };
        }
    }

    public sealed partial class MovementWalkSound : MovementSound {
#if UNITY_EDITOR
        protected override void DrawInspectorSettings(Color foldoutColor, Color fieldColor)
        {
            base.DrawInspectorSettings(foldoutColor, fieldColor);
            soundTriggerTime = EditorExtension.DisplayFloatSlider("Sound Trigger Time", soundTriggerTime, 0f, 100f, fieldColor);
            minPitch = EditorExtension.DisplayFloatSlider("MinPitch", minPitch, 0f, 100f, fieldColor);
         
            maxPitch = EditorExtension.DisplayFloatSlider("Max Pitch", maxPitch, 0f, 100f, fieldColor);
            minVolume = EditorExtension.DisplayFloatSlider("Min Volume", minVolume, 0f, 100f, fieldColor);

            maxVolume = EditorExtension.DisplayFloatSlider("Max Volume", maxVolume, 0f, 100f, fieldColor);
        }

#endif
    }
}
