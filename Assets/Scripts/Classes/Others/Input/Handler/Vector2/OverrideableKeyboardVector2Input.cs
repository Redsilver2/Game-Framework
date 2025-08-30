using UnityEngine;


namespace RedSilver2.Framework.Inputs
{
    public sealed class OverrideableKeyboardVector2Input : KeyboardVector2Input
    {
        public OverrideableKeyboardVector2Input(string name) : base(name)
        {
        }

        public OverrideableKeyboardVector2Input(string name, Vector2Keyboard keyboardKeys) : base(name, keyboardKeys)
        {
        }

        public OverrideableKeyboardVector2Input(string name, GamepadStick gamepadStick) : base(name, gamepadStick)
        {
        }

        public OverrideableKeyboardVector2Input(string name, Vector2Keyboard keyboardKeys, GamepadStick gamepadStick) : base(name, keyboardKeys, gamepadStick)
        {
        }

        public void OverrideUpKey(KeyboardKey key)    => keyboardKeys.OverrideUp(key);
        public void OverrideDownKey(KeyboardKey key)  => keyboardKeys.OverrideDown(key);
        public void OverrideLeftKey(KeyboardKey key)  => keyboardKeys.OverrideLeft(key);
        public void OverrideRightKey(KeyboardKey key) => keyboardKeys.OverrideRight(key);
    }
}
