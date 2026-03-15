namespace RedSilver2.Framework.Inputs
{
    public sealed class OverrideablePressInput : PressInput, IOverrideableSingleInput
    {
        public OverrideablePressInput(string inputHandlerName, KeyboardKey defaultKeyboardKey, GamepadButton defaultGamepadKey) : base(inputHandlerName, defaultKeyboardKey, defaultGamepadKey)
        {
        }

        public OverrideablePressInput(string inputHandlerName, MouseButton defaultMouseButton, GamepadButton defaultGamepadButton) : base(inputHandlerName, defaultMouseButton, defaultGamepadButton)
        {
        }

        public void OverrideKey(KeyboardKey key) => defaultControl      = InputManager.GetKeyboardControl(key);
        public void OverrideKey(MouseButton button) => defaultControl   = InputManager.GetMouseControl(button);
        public void OverrideKey(GamepadButton button) => gamepadControl = InputManager.GetGamepadControl(button);
    }
}
