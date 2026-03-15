namespace RedSilver2.Framework.Inputs
{
    public sealed class OverrideableReleaseInput : ReleaseInput, IOverrideableSingleInput
    {
        public OverrideableReleaseInput(string inputHandlerName, KeyboardKey defaultKeyboardKey, GamepadButton defaultGamepadButton) : base(inputHandlerName, defaultKeyboardKey, defaultGamepadButton)
        {
        }

        public OverrideableReleaseInput(string inputHandlerName, MouseButton defaultMouseButton, GamepadButton defaultGamepadButton) : base(inputHandlerName, defaultMouseButton, defaultGamepadButton)
        {
        }

        public void OverrideKey(KeyboardKey key) => defaultControl = InputManager.GetKeyboardControl(key);
        public void OverrideKey(MouseButton button) => defaultControl = InputManager.GetMouseControl(button);
        public void OverrideKey(GamepadButton button) => gamepadControl = InputManager.GetGamepadControl(button);
    }
}
