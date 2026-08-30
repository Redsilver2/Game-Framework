using RedSilver2.Framework.StateMachines.States;
using UnityEngine;
using UnityEngine.Events;

namespace RedSilver2.Framework.StateMachines.Extensions
{
    [System.Serializable]
    public class LandSound : LandStateEvent
    {
        [SerializeField] private AudioSource source;

        [Space]
        [SerializeField] private LandSoundData data;

        [Space]
        [SerializeField] private float minPitch;
        [SerializeField] private float maxPitch;

        [Space]
        [SerializeField] private float minVolume;
        [SerializeField] private float maxVolume;   

        public LandSound() : base() { }

        protected sealed override void Add(MovementState state, MovementStateMachine stateMachine) {
            if(stateMachine != null && source != null) {
                AudioClip[] clips = data.GetClips(stateMachine.GroundTag);
                if (clips == null || clips.Length == 0) return;

                AudioClip clip = clips[Random.Range(0, clips.Length - 1)];
                
                if(source != null) {
                    source.pitch = Random.Range(minPitch, maxPitch);
                    source.volume = Random.Range(minVolume, maxVolume);
                }

                source.clip = clip;
                source.Play();
            }
        }
    }
}
