using TMPro;
using UnityEngine;

namespace RedSilver2.Framework.UI
{
    public sealed class ButtonFullScreenModeSettingUI : FullScreenModeSettingUI {
        [Space]
        [SerializeField] private UISelectionButton previous, next;

        [Space]
        [SerializeField] private TextMeshProUGUI displayer;

        private void Start()
        {
            previous?.AddOnClickListener(() => {
                uint nextValue = Index - 1;
                SetIndex(nextValue);
            });

            next?.AddOnClickListener(() => {
                uint nextValue = Index + 1;
                SetIndex(nextValue);
            });
        }

        public sealed override void ApplySetting() {
            base.ApplySetting();
            if (displayer != null) displayer.text = GetFullscreenMode().ToString();
        }
    }
}
