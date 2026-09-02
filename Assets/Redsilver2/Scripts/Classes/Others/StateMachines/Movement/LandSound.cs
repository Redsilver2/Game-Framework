using RedSilver2.Framework.StateMachines.Events;
using RedSilver2.Framework.StateMachines.States;
using UnityEngine;
using UnityEngine.Events;

namespace RedSilver2.Framework.StateMachines.Extensions
{
    [System.Serializable]
    public sealed class LandSound : MovementSound
    {

        [Space]
        [SerializeField] private float minPitch;
        [SerializeField] private float maxPitch;

        [Space]
        [SerializeField] private float minVolume;
        [SerializeField] private float maxVolume;

        public LandSound(string name, LandState state) : base(name, state) {

        }

        public void SetMinPitch(float minPitch)
        {
            this.minPitch = minPitch;
        }

        public void SetMaxPitch(float maxPitch)
        {
            this.maxPitch = maxPitch;
        }

        public void SetMinVolume(float minVolume)
        {
            this.minVolume = minVolume;
        }

        public void SetMaxVolume(float maxVolume)
        {
            this.maxVolume = maxVolume;
        }

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
                Enter(Source, Data, MovementState.GetMovementStateMachine(state));
            };
        }

        private void Enter(AudioSource source, MovementSoundData data, MovementStateMachine stateMachine)
        {
            if (stateMachine == null || source == null || data == null) return;
            AudioClip[] clips = data.GetClips(stateMachine.GroundTag);
           
            if (clips == null || clips.Length == 0) return;
            AudioClip clip = clips[Random.Range(0, clips.Length - 1)];
            
            if (source == null) return;
            source.pitch = Random.Range(minPitch, maxPitch);
                source.volume = Random.Range(minVolume, maxVolume);

                source.clip = clip;
                source.Play();
        }
    }
}
