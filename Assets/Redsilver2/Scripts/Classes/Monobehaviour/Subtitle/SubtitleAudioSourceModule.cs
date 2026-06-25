using UnityEngine;

namespace RedSilver2.Framework.Dialogs {

    [RequireComponent(typeof(AudioSource))]
    public class SubtitleAudioSourceModule : MonoBehaviour {
        [SerializeField] private SubtitleAudioSource source;

        private void Awake() {
            source?.Initialize(GetComponent<AudioSource>());
        }
    }
}
