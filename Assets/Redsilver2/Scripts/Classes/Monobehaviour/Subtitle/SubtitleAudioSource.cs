using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RedSilver2.Framework.Dialogs {
    [RequireComponent(typeof(AudioSource))]

    [CreateAssetMenu(fileName = "New Subtitle Audio Source", menuName = "Subtitle/Audio Source")]
    public class SubtitleAudioSource : ScriptableObject {
        [SerializeField] private string subtitleAudioName;
        public AudioSource Source { get; private set; }

        private readonly static List<SubtitleAudioSource> Instances = new List<SubtitleAudioSource>();
        
        public void Initialize(AudioSource source)
        {
            this.Source = source;

            if (Instances == null || string.IsNullOrEmpty(subtitleAudioName) || Instances.Contains(this)) 
                return;

            Instances?.Add(this);
        }

        public static SubtitleAudioSource GetSubtitleAudio(string subtitleAudioName) {
            if(Instances == null || string.IsNullOrEmpty(subtitleAudioName)) return null;
            var results = Instances.Where(x => x != null)
                                   .Where(x => x.subtitleAudioName.ToLower().Equals(subtitleAudioName.ToLower()));

            return results.Count() > 0 ? results.First() : null;
            
        }
    }
}
