
using System.Collections.Generic;
using UnityEngine;

namespace RedSilver2.Framework.Subtitles
{
    [System.Serializable]
    public class AudibleCharacterSubtitle : CharacterSubtitle {
        [Space]
        [SerializeField] private AudioClip clip;
        private AudioSource source;

        public AudibleCharacterSubtitle(string characterName, AudioClip clip) : base(characterName) {
            this.clip = clip;
        }
     
        public void Play(float time)
        {
            if(source != null) {
                source.clip = clip;
                source?.Play();
                source.time = time;
            }
        }

        public void SetAudioSource(AudioSource source) {
            this.source = source;
        }

    }
}