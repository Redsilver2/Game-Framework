using RedSilver2.Framework.Settings;
using TMPro;
using UnityEngine;

namespace RedSilver2.Framework.UI
{
    public class ButtonFramerateUI : FramerateSettingUI {

        [SerializeField] private UISelectionButton previous;
        [SerializeField] private UISelectionButton next;

        [Space]
        [SerializeField] private TextMeshProUGUI displayer;

        private void Start()
        {
            previous?.AddOnClickListener(() =>
            {
                uint nextIndex = Index - 1;
                SetIndex(nextIndex);
            });


            next?.AddOnClickListener(() =>
            {
                uint nextIndex = Index + 1;
                SetIndex(nextIndex);
            });
        }

        public override void ApplySetting() {
            base.ApplySetting();

            if(displayer != null) {
                uint frames = CurrentFramerate();
                displayer.text = frames == uint.MaxValue ? "Unlimited" : $"{frames} FPS";
            }

        }
    }
}
