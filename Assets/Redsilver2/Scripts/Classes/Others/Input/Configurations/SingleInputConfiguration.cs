using RedSilver2.Framework.Inputs.Settings;
using UnityEngine.Events;

namespace RedSilver2.Framework.Inputs.Configurations {
    [System.Serializable]
    public class SingleInputConfiguration : InputConfiguration {
        private bool   isKeyboardDefault;
        private SingleInputType singleInput;

        private KeyboardKey defaultKeyboardKey;
        private MouseButton defaultMouseButton;

        private GamepadButton defaultGamepadButton;
        private UnityEvent<bool> onUpdated;

        private UnityEvent<KeyboardKey,   SingleInputConfiguration>   onKeyboardKeyOverrided;
        private UnityEvent<MouseButton,   SingleInputConfiguration>   onMouseButtonOverrided;
        private UnityEvent<GamepadButton, SingleInputConfiguration>   onGamepadeButtonOverrided;

        public bool Value {  get; private set; }

        public SingleInputConfiguration(SingleInputSettings settings) : base(settings)
        {
            onUpdated = new UnityEvent<bool>();

            if (settings != null)
            {
                singleInput          = settings.SingleInput;
                defaultKeyboardKey   = settings.DefaultKeyboardKey;
                defaultMouseButton   = settings.DefaultMouseButton;
                defaultGamepadButton = settings.DefaultGamepadButton;

                Initialize();
            }
        }

        protected sealed override UnityAction GetOnUpdate()
        {
            return () => {
                SingleInput input = GetSingleInput();
              
                if (input == null || !input.Value) {
                    Value = false;
                }
                else {
                    Value = true;
                    onUpdated.Invoke(true); 
                }
            };
        }

        public void AddOnUpdatedListener(UnityAction<bool> action)
        {
            if(action != null) onUpdated?.AddListener(action); 
        }

        public void RemoveOnUpdatedListener(UnityAction<bool> action)
        {
            if (action != null) onUpdated?.RemoveListener(action);
        }



        public override bool IsOverrideable()
        {
            return GetSingleInput() is IOverrideableSingleInput;
        }

        protected sealed override void Initialize(string name, ref InputHandler handler)
        {
            if (string.IsNullOrEmpty(name)) return;

            if (isKeyboardDefault)
                handler = InputManager.GetOrCreateSingleInput(name, defaultKeyboardKey, defaultGamepadButton, singleInput);
            else
                handler = InputManager.GetOrCreateSingleInput(name, defaultMouseButton, defaultGamepadButton, singleInput);
        }

        public void OverrideKey(KeyboardKey key)
        {
            GetOverrideableSingleInput()?.OverrideKey(key);
            if(IsOverrideable()) onKeyboardKeyOverrided?.Invoke(key, this);
        }

        public void OverrideKey(MouseButton button)
        {
            GetOverrideableSingleInput()?.OverrideKey(button);
            if(IsOverrideable()) onMouseButtonOverrided?.Invoke(button, this);
        }

        public void OverrideKey(GamepadButton button)
        {
            GetOverrideableSingleInput()?.OverrideKey(button);
            if(IsOverrideable()) onGamepadeButtonOverrided?.Invoke(button, this);  
        }

        public sealed override void Reset()
        {
            if (isKeyboardDefault) OverrideKey(defaultKeyboardKey);
            else OverrideKey(defaultMouseButton);

            OverrideKey(defaultGamepadButton);
        }

        private IOverrideableSingleInput GetOverrideableSingleInput() {
            return GetSingleInput() as IOverrideableSingleInput;
        }

        private SingleInput GetSingleInput()
        {
            return GetInput() as SingleInput;   
        }
    }
}
