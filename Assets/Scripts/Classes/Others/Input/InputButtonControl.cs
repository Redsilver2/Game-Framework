
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

namespace RedSilver2.Framework.Inputs
{
    public sealed class InputButtonControl : InputControl
    {
        private readonly ButtonControl control;

        public InputButtonControl(string path, Sprite icon) : base(path, icon)
        {
            control = InputSystem.FindControl(path) as ButtonControl;
        }

        public bool GetKey()
        {
            if (control != null) return control.isPressed;
            return false;
        }

        public bool GetKeyDown()
        {
            if (control != null) return control.wasPressedThisFrame;
            return false;
        }

        public bool GetKeyUp()
        {
            if (control != null) return control.wasReleasedThisFrame;
            return false;
        }
    }
}
