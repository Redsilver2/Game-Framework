using RedSilver2.Framework.StateMachines.Controllers;
using UnityEngine;
using UnityEngine.Events;

namespace RedSilver2.Framework.StateMachines.States
{
    [RequireComponent(typeof(AudioSource))]
    public sealed class MovementSoundModule : MovementStateModule {
        private AudioSource source;
        private float currentMinPitch , currentMaxPitch; 
        private float currentMinVolume, currentMaxVolume;

        protected override void Awake() {
            base.Awake();
            source = GetComponent<AudioSource>();
        }

        public void SetPitch(float pitch) {
            if(source != null) source.pitch = pitch;
        }

        public void SetMinPitch(float pitch) {
            if (source != null) currentMinPitch = pitch;
        }

        public void SetMaxPitch(float pitch) {
            if (source != null) currentMaxPitch = pitch;
        }

        public void UpdateMinPitch(float minPitch, float updateSpeed) {
            currentMinPitch = Mathf.Lerp(currentMinPitch, minPitch, Time.deltaTime * updateSpeed);
        }

        public void UpdateMaxPitch(float maxPitch, float updateSpeed) {
            currentMaxPitch = Mathf.Lerp(currentMinPitch, maxPitch, Time.deltaTime * updateSpeed);
        }

        public void RandomizePitch() {
            RandomizePitch(currentMinPitch, currentMaxPitch);
        }

        public void RandomizePitch(float minPitch, float maxPitch) {
            if(source != null) source.pitch = Random.Range(minPitch, maxPitch);
        }

        public void RandomizeVolume() {
            RandomizeVolume(currentMinVolume, currentMaxVolume);
        }

        public void RandomizeVolume(float minVolume, float maxVolume) {
            if (source != null) source.volume = Random.Range(minVolume, maxVolume);
        }

        public void PlayAudio(AudioClip clip) {
            if(source == null) return;
            source.clip = clip;
            source.Play();
        }

        protected sealed override UnityAction<State> GetOnStateAddedAction() {
            return null;
        }

        protected sealed override UnityAction<State> GetOnStateRemovedAction() {
            return null;
        }

        protected override string GetModuleName()  {
            return "Movement Sound Module";
        }
    }
}