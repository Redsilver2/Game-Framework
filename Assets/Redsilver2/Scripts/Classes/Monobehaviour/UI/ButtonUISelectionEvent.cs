

using UnityEngine;

namespace RedSilver2.Framework.UI
{
    public abstract class ButtonUISelectionEvent : MonoBehaviour
    {
        private ButtonUISelection selection;
        protected ButtonUISelection Selection => selection;

        public void Start()
        {
            selection = GetComponent<ButtonUISelection>();
            SetEvents(selection, true);
        }

        private void OnEnable() { SetEvents(selection, true); }
        private void OnDisable() { SetEvents(selection, false); }

        protected abstract void SetEvents(ButtonUISelection selection, bool isAddingEvent);
    }
}
