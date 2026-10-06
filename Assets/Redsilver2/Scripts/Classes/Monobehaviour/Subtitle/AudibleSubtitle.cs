using System.Collections.Generic;
using UnityEngine;

namespace RedSilver2.Framework.Subtitles
{
    [System.Serializable]
    public class AudibleSubtitle : Subtitle {
       
        [Space]
        [SerializeField] private AudioClip clip;
        private AudioSource source;

        public AudioSource Source => source;
        public AudioClip Clip => clip;

        public void Play(float time)
        {
            Debug.Log(source);
            Debug.Log(clip);

            if (source != null) {
                source.clip = clip;
                source?.Play();
            }
        }

        public void SetAudioSource(AudioSource source) {
            this.source = source;
        }
    }
}