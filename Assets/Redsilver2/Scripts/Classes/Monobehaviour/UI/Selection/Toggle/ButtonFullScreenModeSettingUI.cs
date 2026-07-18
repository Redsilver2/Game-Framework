using TMPro;
using UnityEngine;

namespace RedSilver2.Framework.UI
{
    public sealed class ButtonFullScreenModeSettingUI : FullScreenModeSettingUI {
        [SerializeField] private ButtonUISelection previous, next;

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

        public sealed override void Apply() {
            base.Apply();
            if (displayer != null) displayer.text = GetFullscreenMode().ToString();
        }
    }
}
