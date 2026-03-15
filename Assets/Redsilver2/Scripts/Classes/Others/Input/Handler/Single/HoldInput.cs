using UnityEngine;
using UnityEngine.InputSystem.Controls;

namespace RedSilver2.Framework.Inputs
{
    public class HoldInput : SingleInput
    {
        public HoldInput(string inputHandlerName, MouseButton defaultMouseButton, GamepadButton defaultGamepadButton) : base(inputHandlerName, defaultMouseButton, defaultGamepadButton) {
       
        }

        public HoldInput(string inputHandlerName, KeyboardKey defaultKeyboardKey, GamepadButton defaultGamepadButton) : base(inputHandlerName, defaultKeyboardKey, defaultGamepadButton) {
       
        }

        protected sealed override bool GetDefaultValue(InputControl control)
        {
            if (control == null) return false;
            return control.GetKey();
        }

        protected sealed override bool GetGamepadValue(InputControl control)
        {
            if(control == null) return false;
            return control.GetKey();
        }

        protected sealed override bool GetXRValue(InputControl control)
        {
            if (control == null) return false;
            return control.GetKey();
        }
    }
}
