using UnityEngine;

namespace RedSilver2.Framework.Dialogs {
    [CreateAssetMenu(fileName = "New Character Subtitle Info", menuName = "Dialog/Subtitle/Info/Character")]
    public sealed class CharacterSubtitleInfo : SubtitleInfo {
        [SerializeField] private CharacterSubtitle subtitle;
        public sealed override Subtitle GetSubtitle() => subtitle;
    }
}
