using System.Collections.Generic;
using UnityEngine;

namespace RedSilver2.Framework.References
{

    [RequireComponent(typeof(AudioSource))]
    public class AudioSourceReference : MonoBehaviour
    {
        [SerializeField] private string      sourceName;
        [SerializeField] private AudioSource source;

        public string SourceName  => sourceName;
        public AudioSource Source => source;
        private static readonly Dictionary<string, AudioSourceReference> references = new Dictionary<string, AudioSourceReference>();

        private void Awake()     {
            if (references != null) {
                if (!references.ContainsKey(sourceName.ToLower())) {
                    references?.Add(sourceName.ToLower(), this);
                }
            }
        }
        private void OnDestroy() {
            if (references != null) {
                if (references.ContainsKey(sourceName.ToLower())) {
                    references?.Remove(sourceName.ToLower());
                }
            }
        }

        public static AudioSource GetSource(string audioSourceReferenceName)
        {
            audioSourceReferenceName = audioSourceReferenceName.ToLower();
            if (references == null || !references.ContainsKey(audioSourceReferenceName)) return null;
            return references[audioSourceReferenceName] == null ? null : references[audioSourceReferenceName].Source;
        }
    }
}
