using RedSilver2.Framework.Inputs.Configurations;
using UnityEngine;
using UnityEngine.Events;

namespace RedSilver2.Framework.Inputs.Settings
{
    public abstract class Vector2InputSettings : InputSettings {
        public sealed override void Disable() {
             GetBaseConfiguration()?.Disable();
        }

        public sealed override async void Enable() {
             GetBaseConfiguration()?.Enable();
        }

        public void AddOnEnableListener(UnityAction action)
        {
            GetBaseConfiguration()?.AddOnEnableListener(action);
        }
        public void RemoveOnEnableListener(UnityAction action)
        {
            GetBaseConfiguration()?.RemoveOnEnableListener(action);
        }

        public void AddOnDisableListener(UnityAction action)
        {
            GetBaseConfiguration()?.AddOnDisableListener(action);
        }
        public void RemoveOnDisableListener(UnityAction action)
        {
            GetBaseConfiguration()?.RemoveOnDisableListener(action);
        }

        public void AddOnUpdateListener(UnityAction<Vector2> action)
        {
            GetBaseConfiguration()?.AddOnUpdateListener(action);
        }
        public void RemoveOnUpdateListener(UnityAction<Vector2> action)
        {
            GetBaseConfiguration()?.RemoveOnUpdateListener(action);
        }

        public void AddOnLateUpdateListener(UnityAction action)
        {
            GetBaseConfiguration()?.AddOnLateUpdateListener(action);
        }
        public void RemoveOnLateUpdateListener(UnityAction action)
        {
            GetBaseConfiguration()?.RemoveOnLateUpdateListener(action);
        }
        
        public Vector2 GetValue() {
            var config = GetBaseConfiguration();
            return config == null ? Vector2.zero : config.Value;
        }

        protected abstract Vector2InputConfiguration GetBaseConfiguration(); 
    }
}
