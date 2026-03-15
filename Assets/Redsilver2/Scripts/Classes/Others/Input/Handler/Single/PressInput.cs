namespace RedSilver2.Framework.Inputs
{
    public class PressInput : SingleInput
    {
        public PressInput(string inputHandlerName, KeyboardKey defaultKeyboardKey, GamepadButton defaultGamepadKey) : base(inputHandlerName, defaultKeyboardKey, defaultGamepadKey)
        {
        }

        public PressInput(string inputHandlerName, MouseButton defaultMouseButton, GamepadButton defaultGamepadButton) : base(inputHandlerName, defaultMouseButton, defaultGamepadButton)
        {
        }

        protected sealed override bool GetDefaultValue(InputControl control)
        {
            if (control == null) return false;
            return control.GetKeyDown();
        }

        protected sealed override bool GetGamepadValue(InputControl control)
        {
            if (control == null) return false;
            return control.GetKeyDown();
        }

        protected sealed override bool GetXRValue(InputControl control)
        {
            if (control == null) return false;
            return control.GetKeyDown();
        }
    }
}
