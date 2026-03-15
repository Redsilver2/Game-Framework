namespace RedSilver2.Framework.Inputs
{
    public sealed class OverrideableHoldInput : HoldInput, IOverrideableSingleInput
    {
        public OverrideableHoldInput(string inputHandlerName, MouseButton defaultMouseButton, GamepadButton defaultGamepadButton) : base(inputHandlerName, defaultMouseButton, defaultGamepadButton)
        {
        }

        public OverrideableHoldInput(string inputHandlerName, KeyboardKey defaultKeyboardKey, GamepadButton defaultGamepadButton) : base(inputHandlerName, defaultKeyboardKey, defaultGamepadButton)
        {
        }

        public void OverrideKey(KeyboardKey key) => defaultControl = InputManager.GetKeyboardControl(key);
        public void OverrideKey(MouseButton button) => defaultControl = InputManager.GetMouseControl(button);
        public void OverrideKey(GamepadButton button) => gamepadControl = InputManager.GetGamepadControl(button);
    }
}