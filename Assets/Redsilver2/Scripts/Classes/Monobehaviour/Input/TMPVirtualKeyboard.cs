using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RedSilver2.Framework.Inputs
{
    public abstract class TMPVirtualKeyboard : VirtualKeyboard
    {
        [SerializeField] private TextMeshProUGUI displayer;

        public void SetDisplayer(TextMeshProUGUI displayer) {
            this.displayer = displayer;
        }

        protected sealed override void UpdateTextDisplayer(string text) {
            if (displayer != null) {
              
                displayer.text = text;
                Debug.Log(displayer.text);
            }
        }

        protected sealed override void UpdateUnifiedUpperCase(Button button, string text)
        {
            if(button == null) return;  
            TextMeshProUGUI displayer = button.GetComponentInChildren<TextMeshProUGUI>();

            if (displayer == null) return;
            displayer.text = text;       
        }

        protected sealed override void SetPhysicalKeyButton(KeyboardKey key, Button button) {
            var displayer =  new TMPVirtualKeyboardPhysicalKey(this, button, key);
            displayer?.UpdateTextDisplayed(IsUpperCase);
        }
    }
}
