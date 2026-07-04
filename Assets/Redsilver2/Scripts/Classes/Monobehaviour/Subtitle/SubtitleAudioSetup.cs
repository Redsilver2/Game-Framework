using UnityEngine;

namespace RedSilver2.Framework.Dialogs
{
    [RequireComponent(typeof(AudioSource))]
    public sealed class SubtitleAudioSetup : MonoBehaviour {
        [SerializeField] private SubtitleAudioInfo audioInfo;

        private void Awake() {
            audioInfo?.SetAudioSource(GetComponent<AudioSource>());
        }
    }
}
