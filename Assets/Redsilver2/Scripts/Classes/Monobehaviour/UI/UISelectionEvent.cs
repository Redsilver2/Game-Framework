using UnityEngine;

namespace RedSilver2.Framework.UI
{
    public abstract class UISelectionEvent : MonoBehaviour
    {
        private UISelection selection;
        protected UISelection Selection => selection;

        public void Start(){
            selection = GetComponent<UISelection>();
            SetEvents(selection, true);
        }

        private void OnEnable() { SetEvents(selection, true); }
        private void OnDisable() { SetEvents(selection, false); }

        protected abstract void SetEvents(UISelection selection, bool isAddingEvent);
    }
}
