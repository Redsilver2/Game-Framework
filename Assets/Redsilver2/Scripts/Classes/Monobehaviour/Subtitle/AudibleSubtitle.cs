using RedSilver2.Framework.Dialogs.Datas;
using System.Collections.Generic;
using UnityEngine;

namespace RedSilver2.Framework.Dialogs
{
    [System.Serializable]
    public class AudibleSubtitle : Subtitle, IAudibleSubtitle {
       
        [Space]
        [SerializeField] private AudioClip clip;
        private AudioSource source;

        public AudioSource Source => source;
        public AudioClip Clip => clip;

        public AudibleSubtitle() : base()
        {
            this.source = null;
            this.clip = null;

            AddOnPlayListener(() => { Play(0f); });
            AddOnStopListener(() => { this.source?.Stop(); });
        }

        public AudibleSubtitle(List<SubtitleData> datas, AudioClip clip) : base(datas) {
            this.source = null;
            this.clip = clip;
           
            AddOnPlayListener(() => { Play(0f); });
            AddOnStopListener(() => { this.source?.Stop(); });
        }

        public AudibleSubtitle(SubtitleData[] datas, AudioClip clip) : base(datas) {
            this.source = null;
            this.clip = clip;
            
            AddOnPlayListener(() => { Play(0f); });
            AddOnStopListener(() => { this.source?.Stop(); });
        }

        public AudibleSubtitle(List<SubtitleData> datas, AudioSource source, AudioClip clip) : base(datas) {
            this.source = source;
            this.clip = clip;
           
            AddOnPlayListener(() => { Play(0f); });
            AddOnStopListener(() => { this.source?.Stop(); });
        }

        public AudibleSubtitle(SubtitleData[] datas, AudioSource source, AudioClip clip) : base(datas) {
            this.source = source;
            this.clip = clip;
            
            AddOnPlayListener(() => { Play(0f); });
            AddOnStopListener(() => { this.source?.Stop(); });
        }


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