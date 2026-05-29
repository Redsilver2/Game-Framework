using UnityEngine;
using RedSilver2.Framework.Inputs.Configurations;
using UnityEngine.Events;

namespace RedSilver2.Framework.Inputs.Settings
{
    public abstract class SingleInputSettings : InputSettings
    {
        [Space]
        [SerializeField] private bool isKeyboardInputDefault;

        [Space]
        public KeyboardKey DefaultKeyboardKey;
        public MouseButton DefaultMouseButton;

        [Space]
        public GamepadButton DefaultGamepadButton;

        public bool IsKeyboardInputDefault => isKeyboardInputDefault;

        public sealed override async void Disable()
        {
            GetConfiguration()?.Disable();
        }
        public  sealed override void Enable()
        {
            GetConfiguration()?.Enable();
        }

        public void AddOnEnableListener(UnityAction action) {
            GetConfiguration()?.AddOnEnableListener(action);
        }
        public void RemoveOnEnableListener(UnityAction action)
        {
            GetConfiguration()?.RemoveOnEnableListener(action);
        }

        public void AddOnDisableListener(UnityAction action)
        {
            GetConfiguration()?.AddOnDisableListener(action);
        }
        public void RemoveOnDisableListener(UnityAction action)
        {
            GetConfiguration()?.RemoveOnDisableListener(action);
        }

        public void AddOnUpdateListener(UnityAction<bool> action)
        {
            GetConfiguration()?.AddOnUpdateListener(action);
        }
        public void RemoveOnUpdatedListener(UnityAction<bool> action)
        {
            GetConfiguration()?.RemoveOnUpdateListener(action);
        }

        public void AddOnLateUpdatedListener(UnityAction action)
        {
            GetConfiguration()?.AddOnLateUpdateListener(action);
        }
        public void RemoveLateOnUpdateListener(UnityAction action)
        {
            GetConfiguration()?.RemoveOnLateUpdateListener(action);
        }

        public bool GetValue()
        {
            var config = GetConfiguration();
            return config == null ? false : config.Value;
        }

        protected SingleInputConfiguration GetConfiguration()  {
            return InputManager.GetOrCreateSingleInputConfiguration(InputName, this);
        }
        public abstract SingleInputType GetInputType();
    }
}
