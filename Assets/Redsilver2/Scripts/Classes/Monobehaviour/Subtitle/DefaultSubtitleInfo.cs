using UnityEngine;

namespace RedSilver2.Framework.Subtitles
{
    [CreateAssetMenu(fileName = "New Subtitle Info", menuName = "Dialog/Subtitle/Info/Default")]
    public sealed class DefaultSubtitleInfo : SubtitleInfo
    {
        [SerializeField] private Subtitle subtitle;
    }
}
