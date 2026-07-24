using TMPro;
using UnityEngine;

namespace RedSilver2.Framework.UI {
    public class VSyncToggle : VSyncSettingUI
    {
        [Space]
        [SerializeField] private UISelectionToggle toggle;

        [Space]
        [SerializeField] private TextMeshProUGUI displayer;

        private void Start() {
            toggle?.AddOnValueChangeListener(value => {
                SetIndex((uint)(value ? 1 : 0));
            });
        }

        public sealed override void ApplySetting()
        {
            base.ApplySetting();
            if (displayer != null) displayer.text = GetDisplayedValue();
        }
    }
}
