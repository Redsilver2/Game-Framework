using UnityEngine;

namespace RedSilver2.Framework.Subtitles 
{
    public abstract class DialogEventHandler : MonoBehaviour
    {
        private void Awake() {
            SetDefaultEvents(SubtitleManager.GetInstance(), true);
        }

        private void OnEnable() {
            SetDefaultEvents(SubtitleManager.GetInstance(), true);
        }

        private void OnDisable() {
            SetDefaultEvents(SubtitleManager.GetInstance(), false);
        }

        protected abstract void SetDefaultEvents(SubtitleManager manager, bool isAddingEvents);
    }
}
