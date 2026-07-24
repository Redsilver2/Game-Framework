

using UnityEngine;

namespace RedSilver2.Framework.UI
{
    public abstract class UISelectionButtonEvent : MonoBehaviour
    {
        private UISelectionButton selection;

        public void Start()
        {
            selection = GetComponent<UISelectionButton>();
            SetEvents(selection, true);
        }

        private void OnEnable() { SetEvents(selection, true); }
        private void OnDisable() { SetEvents(selection, false); }

        protected abstract void SetEvents(UISelectionButton selection, bool isAddingEvent);
    }
}
