using UnityEngine;

namespace RedSilver2.Framework.Inputs
{
    public class ReleaseInput : SingleInput
    {
        public ReleaseInput(string inputHandlerName, KeyboardKey defaultKeyboardKey, GamepadButton defaultGamepadButton) : base(inputHandlerName, defaultKeyboardKey, defaultGamepadButton)
        {
        }

        public ReleaseInput(string inputHandlerName, MouseButton defaultMouseButton, GamepadButton defaultGamepadButton) : base(inputHandlerName, defaultMouseButton, defaultGamepadButton)
        {
        }

        protected sealed override bool GetDefaultValue(InputControl control)
        {
            if (control == null) return false;
            return control.GetKeyUp();
        }

        protected sealed override bool GetGamepadValue(InputControl control)
        {
            if (control == null) return false;
            return control.GetKeyUp();
        }

        protected sealed override bool GetXRValue(InputControl control)
        {
            if (control == null) return false;
            return control.GetKeyUp();
        }
    }
}
