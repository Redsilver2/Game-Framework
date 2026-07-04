using UnityEngine;

namespace RedSilver2.Framework.Dialogs 
{
    public abstract class DialogEventHandler : MonoBehaviour
    {
        private void Awake() {
            SetDefaultEvents(DialogManager.GetInstance(), true);
        }

        private void OnEnable() {
            SetDefaultEvents(DialogManager.GetInstance(), true);
        }

        private void OnDisable() {
            SetDefaultEvents(DialogManager.GetInstance(), false);
        }

        protected abstract void SetDefaultEvents(DialogManager manager, bool isAddingEvents);
    }
}
