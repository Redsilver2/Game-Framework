using UnityEngine;

namespace RedSilver2.Framework.Dialogs {
    [CreateAssetMenu(fileName = "New Subtitle Parent Info", menuName = "Dialog/Subtitle/Parent Info")]
    public class SubtitleParentInfo : ScriptableObject {
        [SerializeField] private SubtitleInfo[] infos;

        public void SetParent(RectTransform transform) {
            foreach(SubtitleInfo info in infos) {
                if(info ==  null) continue;
                info.GetSubtitle()?.SetParent(transform);
            }
        }
    }
}
