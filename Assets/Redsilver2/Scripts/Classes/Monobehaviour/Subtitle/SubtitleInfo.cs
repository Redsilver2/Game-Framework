using RedSilver2.Framework.Dialogs.Datas;
using UnityEngine;
using UnityEngine.Events;

namespace RedSilver2.Framework.Dialogs
{
    public abstract class SubtitleInfo : ScriptableObject {
#if UNITY_EDITOR
        private void OnValidate()
        {
            GetSubtitle()?.ValidateDatas();
        }
#endif

        public void AddOnPlayListener(UnityAction action) {
            GetSubtitle()?.AddOnPlayListener(action);
        }
        public void RemoveOnPlayListener(UnityAction action) {
            GetSubtitle()?.RemoveOnPlayListener(action);
        }

        public void AddOnStopListener(UnityAction action)
        {
            GetSubtitle()?.AddOnStopListener(action);
        }
        public void RemoveOnStopListener(UnityAction action)
        {
            GetSubtitle()?.RemoveOnStopListener(action);
        }

        public void AddData(SubtitleData data)  {
            GetSubtitle()?.AddData(data);
        }

        public void RemoveData(SubtitleData data) {
            GetSubtitle()?.RemoveData(data);
        }

        public abstract Subtitle GetSubtitle();
    }
}
