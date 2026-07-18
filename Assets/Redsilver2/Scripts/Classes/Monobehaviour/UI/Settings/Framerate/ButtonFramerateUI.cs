using RedSilver2.Framework.Settings;
using TMPro;
using UnityEngine;

namespace RedSilver2.Framework.UI
{
    public class ButtonFramerateUI : FramerateSettingUI {

        [SerializeField] private ButtonUISelection previous;
        [SerializeField] private ButtonUISelection next;

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

        public override void Apply() {
            base.Apply();

            if(displayer != null) {
                uint frames = CurrentFramerate();
                displayer.text = frames == uint.MaxValue ? "Unlimited" : $"{frames} FPS";
            }

        }
    }
}
