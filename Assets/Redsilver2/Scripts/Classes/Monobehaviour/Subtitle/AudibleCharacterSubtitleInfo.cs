using UnityEngine;

namespace RedSilver2.Framework.Dialogs
{
    [CreateAssetMenu(fileName = "New Audible Character Subtitle Info", menuName = "Dialog/Subtitle/Info/Audible Character")]
    public sealed class AudibleCharacterSubtitleInfo : SubtitleInfo
    {
        [SerializeField] private AudibleCharacterSubtitle subtitle;
        public AudibleCharacterSubtitle Subtitle => subtitle;
        public sealed override Subtitle GetSubtitle() => subtitle;
    }
}
