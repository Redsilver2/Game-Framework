using RedSilver2.Framework.Dialogs.Datas;
using System.Collections.Generic;
using UnityEngine;

namespace RedSilver2.Framework.Dialogs
{
    [System.Serializable]
    public class AudibleCharacterSubtitle : CharacterSubtitle, IAudibleSubtitle {
        [Space]
        [SerializeField] private AudioClip clip;
        private AudioSource source;

        public AudibleCharacterSubtitle(string characterName, AudioClip clip) : base(characterName) {
            this.clip = clip;
            AddOnPlayListener(() => { Play(0f); });
            AddOnStopListener(() => { this.source?.Stop(); });
        }
     
        public AudibleCharacterSubtitle(List<SubtitleData> datas, string characterName, AudioClip clip) : base(datas, characterName) {
            this.clip = clip;
            AddOnPlayListener(() => { Play(0f); });
            AddOnStopListener(() => { this.source?.Stop(); });
        }
        public AudibleCharacterSubtitle(SubtitleData[] datas, string characterName, AudioClip clip) : base(datas, characterName) {
            this.clip = clip;
            AddOnPlayListener(() => { Play(0f); });
            AddOnStopListener(() => { this.source?.Stop(); });
        }
      
        public AudibleCharacterSubtitle(List<SubtitleData> datas, string characterName, AudioSource source, AudioClip clip) : base(datas, characterName) {
            this.source = source;
            this.clip = clip;
          
            AddOnPlayListener(() => { Play(0f); });
            AddOnStopListener(() => { this.source?.Stop(); });
        }
        public AudibleCharacterSubtitle(SubtitleData[] datas, string characterName, AudioSource source, AudioClip clip) : base(datas, characterName) {
            this.source = source;
            this.clip = clip;
          
            AddOnPlayListener(() => { Play(0f); });
            AddOnStopListener(() => { this.source?.Stop(); });
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


        public sealed override bool IsSimilar(Subtitle subtitle) {
            if(subtitle is not AudibleCharacterSubtitle || clip == null) return base.IsSimilar(subtitle);
            return clip.Equals((subtitle as AudibleCharacterSubtitle).clip);
        }
    }
}