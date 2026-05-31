using RedSilver2.Framework.Subtitles.Datas;
using UnityEngine;

namespace RedSilver2.Framework.Subtitles {
    [CreateAssetMenu(fileName = "New Default Subtitle", menuName = "Subtitle/Default")]
    public class DefaultSubtitle : Subtitle {

        [Space]
        [SerializeField] private SubtitleData[] datas;

        public sealed override SubtitleData[] GetSubtitleDatas() {
            return datas;
        }
    }
}
