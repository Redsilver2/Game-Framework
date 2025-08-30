namespace RedSilver2.Framework.Inputs
{
    public sealed class OverrideablePressInput : PressInput, IOverridableSingleInput
    {
        public OverrideablePressInput(string inputHandlerName, KeyboardKey defaultKeyboardKey, GamepadKey defaultGamepadKey) : base(inputHandlerName, defaultKeyboardKey, defaultGamepadKey)
        {
        }

        public void OverrideKey(KeyboardKey key) => keyboardKey = key;
        public void OverrideKey(GamepadKey key)  => gamepadKey  = key;
    }
}
