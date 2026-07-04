using UnityEngine;
using UnityEngine.Audio;

namespace RedSilver2.Framework.Dialogs
{
    [CreateAssetMenu(fileName = "New Subtitle Audio Info", menuName = "Dialog/Subtitle/Audio Info")]
    public sealed class SubtitleAudioInfo : ScriptableObject {
        [SerializeField] private AudibleSubtitleInfo[] audibleSubtitleInfos;
        [SerializeField] private AudibleCharacterSubtitleInfo[] audibleCharacterSubtitlesInfos;

        public void SetAudioSource(AudioSource audioSource) {
            SetAudioSource(audibleSubtitleInfos, audioSource);
            SetAudioSource(audibleCharacterSubtitlesInfos, audioSource);
        }

        private void SetAudioSource(AudibleCharacterSubtitleInfo[] infos, AudioSource source) {
            if (infos == null || infos.Length == 0) return;

            foreach (AudibleCharacterSubtitleInfo info in infos) {
                if (info == null) continue;
                info.Subtitle?.SetAudioSource(source);
            }
        }


        private void SetAudioSource(AudibleSubtitleInfo[] infos, AudioSource source) {
            if (infos == null || infos.Length == 0) return;

            foreach (AudibleSubtitleInfo info in infos) {
                if (info == null) continue;
                info.Subtitle?.SetAudioSource(source);
            }
        }
    }
}
