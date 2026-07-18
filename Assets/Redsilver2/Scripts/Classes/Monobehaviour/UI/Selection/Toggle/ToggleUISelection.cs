using System.Collections;
using UnityEngine.Events;
using UnityEngine.UI;

namespace RedSilver2.Framework.UI
{

    public sealed class ToggleUISelection : UISelection
    {
        private Toggle toggle;

        protected override void Awake()
        {
            base.Awake();
            toggle = GetComponentInChildren<Toggle>();
        }

        public void SetValue(bool value)
        {
            if (toggle != null) toggle.isOn = value;
        }

        public void AddOnValueChangeListener(UnityAction<bool> action)
        {
            if (action != null) toggle?.AddOnValueChangedListener(action);
        }

        public void RemoveOnValueChangeListener(UnityAction<bool> action)
        {
            if (action != null) toggle?.RemoveOnValueChangedListener(action);
        }

        protected override IEnumerator UpdateCoroutine() {
            while(toggle != null) {
                if(GameUIController.GetConfirmState()) toggle.isOn = !toggle.isOn;
                yield return null;
            }
        }
    }
}
