using UnityEngine;

namespace RedSilver2.Framework.Dialogs
{
    public abstract class SubtitleInfo : ScriptableObject {
#if UNITY_EDITOR
        private void OnValidate()
        {
            GetSubtitle()?.ValidateDatas();
        }
#endif

        public abstract Subtitle GetSubtitle();
    }
}
