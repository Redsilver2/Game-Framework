using RedSilver2.Framework.Inputs.Settings;
using UnityEngine;

namespace RedSilver2.Framework.Inputs.Configurations
{
    [System.Serializable]
    public sealed class KeyboardVector2InputConfiguration : Vector2InputConfiguration
    {
        private KeyboardKey  defaultUpKey;
        private KeyboardKey  defaultDownKey;
        private KeyboardKey  defaultLeftKey;
        private KeyboardKey  defaultRightKey;

        private GamepadStick defaultGamepadStick;

        public KeyboardVector2InputConfiguration(KeyboardVector2InputSettings settings) : base(settings)
        {
            if (settings != null)
            {
                defaultUpKey   = settings.DefaultUpKey;
                defaultDownKey = settings.DefaultDownKey;

                defaultLeftKey = settings.DefaultLeftKey;
                defaultRightKey = settings.DefaultRightKey;

                defaultGamepadStick = settings.DefaultGamepadStick;

                Initialize();
            }
        }

        public override void Reset() {
            OverrideUpKey(defaultUpKey);
            OverrideDownKey(defaultDownKey);
           
            OverrideLeftKey(defaultLeftKey);
            OverrideRightKey(defaultRightKey);

            OverrideGamepadStick(defaultGamepadStick);  
        }

        public void OverrideUpKey(KeyboardKey key)
        {
            GetOverrideableKeyboard()?.OverrideUpKey(key);
        }
        public void OverrideDownKey(KeyboardKey key)
        {
            GetOverrideableKeyboard()?.OverrideDownKey(key);
        }

        public void OverrideLeftKey(KeyboardKey key)
        {
            GetOverrideableKeyboard()?.OverrideLeftKey(key);
        }
        public void OverrideRightKey(KeyboardKey key)
        {
            GetOverrideableKeyboard()?.OverrideRightKey(key);
        }

        public void OverrideGamepadStick(GamepadStick stick)
        {
            GetOverrideableKeyboard()?.OverrideGamepadStick(stick);
        }

        protected sealed override void Initialize(string name, ref InputHandler handler)
        {
            if (base.IsOverrideable())
               handler = InputManager.GetOrCreateKeyboardVector2Input(name, new KeyboardVector2Input.Vector2Keyboard(
                                                                      defaultUpKey, defaultDownKey, defaultLeftKey, defaultRightKey), 
                                                                      defaultGamepadStick);
            else
                handler = InputManager.GetOrCreateOverrideableKeyboardVector2Input(name, new KeyboardVector2Input.Vector2Keyboard(
                                                       defaultUpKey, defaultDownKey, defaultLeftKey, defaultRightKey),
                                                       defaultGamepadStick);
        }

        public sealed override bool IsOverrideable()
        {
            return GetVector2Input() is IOverrideableKeyboardVector2Input;
        }

        private OverrideableKeyboardVector2Input GetOverrideableKeyboard()
        {
            return GetVector2Input() as OverrideableKeyboardVector2Input;
        }
    }
}
