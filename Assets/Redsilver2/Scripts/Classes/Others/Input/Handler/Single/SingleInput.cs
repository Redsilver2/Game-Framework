using System.IO;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

namespace RedSilver2.Framework.Inputs
{
    public abstract class SingleInput : InputHandler
    {
        private   UnityEvent     onUpdate;

        protected InputControl defaultControl;
        protected InputControl gamepadControl;
        protected InputControl xrControl;

        private bool allowXRPath;
        public bool Value { get; private set; }

        public bool AllowXRPath => allowXRPath;

        public SingleInput(string inputHandlerName, KeyboardKey defaultKeyboardKey, GamepadButton defaultGamepadButton) : base(inputHandlerName)
        {
            Value       = false;
            allowXRPath = false;
            onUpdate    = new UnityEvent();

            defaultControl = InputManager.GetKeyboardControl(defaultKeyboardKey); 
            gamepadControl = InputManager.GetGamepadControl(defaultGamepadButton);
            xrControl      = null;
        }

        public SingleInput(string inputHandlerName, MouseButton defaultMouseButton, GamepadButton defaultGamepadButton) : base(inputHandlerName)
        {
            Value       = false;
            allowXRPath = false;
            onUpdate    = new UnityEvent();

            defaultControl = InputManager.GetMouseControl(defaultMouseButton);
            gamepadControl = InputManager.GetGamepadControl(defaultGamepadButton);
            xrControl = null;
        }

        public sealed override void Update()
        {
            Value = IsEnabled ?  GetDefaultPathValue()
                              || GetGamepadPathValue() : false;

            if(IsEnabled && Value) { onUpdate?.Invoke(); }
        }

        public void AddOnUpdateListener(UnityAction action)
        {
            if(onUpdate != null && action != null) { onUpdate.AddListener(action); }
        }
        public void RemoveOnUpdateListener(UnityAction action)
        {
            if (onUpdate != null && action != null) { onUpdate.RemoveListener(action); }
        }

        public void AddOnUpdateListeners(UnityAction[] actions)
        {
            if (actions != null)
                foreach (UnityAction action in actions) AddOnUpdateListener(action);
        }
        public void RemoveOnUpdateListeners(UnityAction[] actions)
        {
            if (actions != null)
                foreach (UnityAction action in actions) RemoveOnUpdateListener(action);
        }

        public sealed override string GetPaths() {
            string result = string.Empty;

            if(defaultControl != null)
            {
               result = defaultControl.Path.Contains("Mouse", System.StringComparison.OrdinalIgnoreCase) ? 
                    $"Mouse Button Path: {defaultControl}" : $"Keyboard Key Path: {defaultControl}";
            }

           if(gamepadControl != null) result += $"\nGamepad Button Path: {gamepadControl}";
           if(xrControl      != null) result += $"\nXR Button Path:      {xrControl}";
           return result;
        }



        public bool GetDefaultPathValue()
        {
            return GetXRValue(defaultControl);
        }
        public bool GetGamepadPathValue()
        {
            return GetXRValue(gamepadControl);
        }
        public bool GetXRControllerPathValue()
        {
            return GetXRValue(xrControl);
        }

        protected abstract bool GetDefaultValue(InputControl control);
        protected abstract bool GetGamepadValue(InputControl control);
        protected abstract bool GetXRValue(InputControl control);
    }
}
