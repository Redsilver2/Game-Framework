using UnityEngine.Events;
using UnityEngine.UI;


namespace RedSilver2.Framework.Inputs
{
    public abstract class VirtualKeyboardPhysicalKey {
        public  readonly KeyboardKey Key;
        public  readonly VirtualKeyboardPhysicalKeyType Type;

        public readonly Button Button;
        private readonly IWriteableInput writeable;

        private VirtualKeyboard keyboard;
        private UnityEvent onEnabled, onDisabled;

        private bool isEnabled;
        public bool IsEnabled => isEnabled;

        protected VirtualKeyboardPhysicalKey(VirtualKeyboard keyboard, Button button, KeyboardKey key) {
            this.Key      = key;
            this.keyboard = keyboard;
         
            this.Button   = button;
            this.isEnabled = true;

            onEnabled  = new UnityEvent();
            onDisabled = new UnityEvent();

            writeable = InputManager.GetWriteableInput(key);     
            Type = GetType(key);  
            
            AddOnClickListener(OnDefaultClickEvent);  
            AddOnEnabledListener(OnEnable);
           
            AddOnDisabledListener(OnDisable);
            keyboard?.AddPhysicalKey(this);
        }

        private VirtualKeyboardPhysicalKeyType GetType(KeyboardKey key)
        {
            if (InputManager.IsNumberKey(key))  return VirtualKeyboardPhysicalKeyType.Number;
            else if (InputManager.IsCharacterKey(key)) return VirtualKeyboardPhysicalKeyType.Character;
            else if (InputManager.IsSpecialCharacterKey(key)) return VirtualKeyboardPhysicalKeyType.SpecialCharacter;
            return default;
        }

        public void Simulate()
        {
            if(Button != null)
                Button.onClick.Invoke();
        }

        private void OnDefaultClickEvent() {
            if(keyboard == null || writeable == null || !isEnabled) return;

            string text = keyboard.TextValue;
            writeable.Write(keyboard.IsUpperCase, keyboard.MaxWordsCount,
                              ref text, out bool wasExecuted);

            if (wasExecuted) {
                keyboard?.Execute(this, text);
            }
        }

        private void OnDisable()
        {
            isEnabled = false;

            if (Button != null)
                Button.gameObject.SetActive(false);
        }

        private void OnEnable() {
            isEnabled = true;

            if (Button != null)
                Button.gameObject.SetActive(true);
        }

        public void SetVirtualKeyboard(VirtualKeyboard keyboard) {
            this.keyboard?.RemovePhysicalKey(this);
            this.keyboard = keyboard;
            this.keyboard?.AddPhysicalKey(this);
        }

        public void AddOnClickListener(UnityAction action)
        {
            if (action != null && Button != null)
                Button.onClick.AddListener(action);
        }
        public void RemoveOnClickListener(UnityAction action) {
            if (action != null && Button != null)
                Button.onClick.AddListener(action);
        }

        public void AddOnEnabledListener(UnityAction action)
        {
            if (action != null) onEnabled?.AddListener(action);
        }
        public void RemoveOnEnabledListener(UnityAction action) {
            if (action != null) onEnabled?.RemoveListener(action);
        }

        public void AddOnDisabledListener(UnityAction action)
        {
            if (action != null) onDisabled?.AddListener(action);
        }
        public void RemoveOnDisabledListener(UnityAction action)
        {
            if(action != null) onDisabled?.RemoveListener(action);
        }

        public void Enable() { 
          if(!isEnabled) onEnabled?.Invoke();
        }
        public void Disable() {
            if(isEnabled) onDisabled?.Invoke(); 
        }

        public bool IsSimilar(KeyboardKey key, bool getUpperCase) {

            return InputManager.GetWriteableInputString(Key, getUpperCase)
                               .Equals(InputManager.GetWriteableInputString(key, getUpperCase));
        }

        public abstract void UpdateTextDisplayed(bool displayUpperCaseText);
    }
}
