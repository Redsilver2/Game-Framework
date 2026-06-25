using UnityEngine;

namespace RedSilver2.Framework.Dialogs {
    [CreateAssetMenu(fileName = "New Audible Subtitle Info", menuName = "Dialog/Subtitle/Info/Audible")]
    public sealed class AudibleSubtitleInfo : SubtitleInfo {
        [SerializeField] private AudibleSubtitle subtitle;
        public AudibleSubtitle Subtitle => subtitle;
        public sealed override Subtitle GetSubtitle() => subtitle;
    }
}
