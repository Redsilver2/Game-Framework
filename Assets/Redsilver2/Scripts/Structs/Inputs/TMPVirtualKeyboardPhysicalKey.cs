using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RedSilver2.Framework.Inputs
{
    public sealed class TMPVirtualKeyboardPhysicalKey : VirtualKeyboardPhysicalKey
    {
        private TextMeshProUGUI displayer;

        public TMPVirtualKeyboardPhysicalKey(VirtualKeyboard keyboard, Button button, KeyboardKey key) : base(keyboard, button, key)
        {
             displayer = button.GetComponentInChildren<TextMeshProUGUI>();  
        }

        public sealed override void UpdateTextDisplayed(bool displayUpperCaseText)
        {
            if (displayer != null) {
                displayer.text = InputManager.GetWriteableInputString(Key, displayUpperCaseText);
            }
        }
    }
}
