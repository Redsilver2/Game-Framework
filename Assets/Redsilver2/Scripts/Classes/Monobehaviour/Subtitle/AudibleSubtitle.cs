using RedSilver2.Framework.Subtitles.Datas;
using System.Collections.Generic;
using UnityEngine;

namespace RedSilver2.Framework.Subtitles
{
    public class AudibleSubtitle : Subtitle, IAudibleSubtitle {
       
        [Space]
        [SerializeField] private AudioClip clip;
        private AudioSource source;

        public AudioSource Source => source;
        public AudioClip Clip => clip;

        public AudibleSubtitle(List<SubtitleData> datas, AudioClip clip) : base(datas) {
            this.source = null;
            this.clip = clip;
        }

        public AudibleSubtitle(SubtitleData[] datas, AudioClip clip) : base(datas) {
            this.source = null;
            this.clip = clip;
        }

        public AudibleSubtitle(List<SubtitleData> datas, AudioSource source, AudioClip clip) : base(datas) {
            this.source = source;
            this.clip = clip;
        }

        public AudibleSubtitle(SubtitleData[] datas, AudioSource source, AudioClip clip) : base(datas) {
            this.source = source;
            this.clip = clip;
        }


        public void Play() {
            Play(0f);
        }

        public void Play(float time)
        {
            if (source != null)
            {
                source.clip = clip;
                source?.Play();
            }
        }

        public void Stop()
        {
            source?.Stop();
        }

        public void SetAudioSource(AudioSource source) { this.source = source; }
    }
}