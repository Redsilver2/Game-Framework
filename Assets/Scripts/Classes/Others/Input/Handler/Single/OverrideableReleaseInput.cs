namespace RedSilver2.Framework.Inputs
{
    public sealed class OverrideableReleaseInput : ReleaseInput, IOverridableSingleInput
    {
        public OverrideableReleaseInput(string name, KeyboardKey defaultKeyboardKey, GamepadKey defaultGamepadKey) : base(name, defaultKeyboardKey, defaultGamepadKey)
        {
        }
        public void OverrideKey(KeyboardKey key) => keyboardKey = key;
        public void OverrideKey(GamepadKey key) => gamepadKey = key;
    }
}
