using RedSilver2.Framework.Subtitles.Datas;
using System.Collections.Generic;
using UnityEngine;

namespace RedSilver2.Framework.Subtitles
{
    public class AudibleCharacterSubtitle : CharacterSubtitle, IAudibleSubtitle {
        [Space]
        [SerializeField] private AudioClip clip;
        private AudioSource source;

        public AudibleCharacterSubtitle(string characterName, AudioClip clip) : base(characterName) { this.clip = clip; }
     
        public AudibleCharacterSubtitle(List<SubtitleData> datas, string characterName, AudioClip clip) : base(datas, characterName) { }
        public AudibleCharacterSubtitle(SubtitleData[] datas, string characterName, AudioClip clip) : base(datas, characterName) { }
      
        public AudibleCharacterSubtitle(List<SubtitleData> datas, string characterName, AudioSource source, AudioClip clip) : base(datas, characterName) { this.source = source; }
        public AudibleCharacterSubtitle(SubtitleData[] datas, string characterName, AudioSource source, AudioClip clip) : base(datas, characterName) { this.source = source; }

        public void Play() {
            Play(0f);
        }

        public void Play(float time)
        {
            if(source != null) {
                source.clip = clip;
                source?.Play();
                source.time = time;
            }
        }

        public void Stop()
        {
            source?.Stop();
        }

        public void SetAudioSource(AudioSource source) {
            this.source = source;
        }


        public sealed override bool IsSimilar(Subtitle subtitle) {
            if(subtitle is not AudibleCharacterSubtitle || clip == null) return base.IsSimilar(subtitle);
            return clip.Equals((subtitle as AudibleCharacterSubtitle).clip);
        }
    }
}