using UnityEngine;

namespace RedSilver2.Framework.Subtitles
{
    public abstract class DialogChoiceEventHandler : MonoBehaviour
    {
        private void Start() {
            SetDefaultEvents(DialogChoiceManager.GetInstance(), true);
        }

        private void OnEnable() {
            SetDefaultEvents(DialogChoiceManager.GetInstance(), true);
        }

        private void OnDisable() {
            SetDefaultEvents(DialogChoiceManager.GetInstance(), false);
        }

        protected abstract void SetDefaultEvents(DialogChoiceManager manager, bool isAddingEvents);
    }
}
