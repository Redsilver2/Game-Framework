using UnityEngine;

namespace RedSilver2.Framework.Dialogs
{
    [CreateAssetMenu(fileName = "New Subtitle Info", menuName = "Dialog/Subtitle/Info/Default")]
    public sealed class DefaultSubtitleInfo : SubtitleInfo
    {
        [SerializeField] private Subtitle subtitle;
        public sealed override Subtitle GetSubtitle() => subtitle;
    }
}
