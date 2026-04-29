using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace RedSilver2.Framework.Inputs
{
    public abstract class VirtualKeyboard : MonoBehaviour
    {
        [SerializeField] private Transform templateInputButtonParent;
        [SerializeField] private Button    templateInputButton;

        [Space]
        [SerializeField] private List<KeyboardKey> excludedKeys;

        [Space]
        [SerializeField] private List<VirtualKeyboardPhysicalKeyType> excludedPhysicalKeyTypes;

        [Space]
        [SerializeField] private bool holdUpperCase = false;
        [SerializeField][Range(1, 10000)] private int maxWordsCount;

        private bool    isUpperCase = false;
        private string  textValue   = string.Empty;

        private UnityEvent<VirtualKeyboardPhysicalKey[]>       onPhysicalKeysLayoutUpdated;
        private UnityEvent<VirtualKeyboardPhysicalKey>         onPhysicalKeyAdded, onPhysicalKeyRemoved;
        private UnityEvent<VirtualKeyboardPhysicalKey, string> onPhysicalKeyExecuted;

        private List<VirtualKeyboardPhysicalKey> physicalKeys; 

        public int MaxWordsCount => maxWordsCount;
        public bool IsUpperCase => isUpperCase;
        public string TextValue => textValue;
        public VirtualKeyboardPhysicalKey[] PhysicalKeys => physicalKeys.ToArray();

        #if UNITY_EDITOR
        protected virtual void OnValidate() {
            excludedKeys = excludedKeys.Where(x => !InputManager.IsExcludedWritingKey(x)).ToList();
        }
        #endif 


         protected virtual void Awake() {
            physicalKeys = new List<VirtualKeyboardPhysicalKey>();
            onPhysicalKeyExecuted = new UnityEvent<VirtualKeyboardPhysicalKey, string>();
            
            onPhysicalKeyAdded   = new UnityEvent<VirtualKeyboardPhysicalKey>();
            onPhysicalKeyRemoved = new UnityEvent<VirtualKeyboardPhysicalKey>();

            onPhysicalKeysLayoutUpdated = new UnityEvent<VirtualKeyboardPhysicalKey[]>();
            textValue = string.Empty;

            AddOnPhysicalKeyAddedListener(physicalKey => {
                physicalKey?.Enable();


                if (physicalKey != null) {
                    if (physicalKey.Button != null) physicalKey.Button.transform.SetParent(templateInputButtonParent, true);
                    physicalKeys?.Add(physicalKey);
                }
            });


            RemoveOnPhysicalKeyAddedListener(physicalKey => {
                physicalKey?.Disable();        
                if (physicalKey != null) physicalKeys?.Remove(physicalKey);
            });

            AddOnPhysicalKeyExecutedListener((key, value) => {
                 textValue = value;
                UpdateTextDisplayer(value);
            });

            InstantiateKeys();
            templateInputButton.gameObject.SetActive(false);
         }

        private void Update()
        {
            UpdateUnifiedUpperCase();
            UpdateKeyboardKey();
            UpdateLayout();
        }

        public void Execute(VirtualKeyboardPhysicalKey key, string text) {
            if(physicalKeys == null) return;

            if (physicalKeys.Contains(key)) {
                onPhysicalKeyExecuted.Invoke(key, text);
            }
        }

        public void AddOnPhysicalKeyAddedListener(UnityAction<VirtualKeyboardPhysicalKey> action) {
            if (action != null) onPhysicalKeyAdded?.AddListener(action);
        }
        public void RemoveOnPhysicalKeyAddedListener(UnityAction<VirtualKeyboardPhysicalKey> action)
        {
            if (action != null) onPhysicalKeyAdded?.RemoveListener(action);
        }

        public void AddOnPhysicalKeyRemovedListener(UnityAction<VirtualKeyboardPhysicalKey> action)
        {
            if (action != null) onPhysicalKeyRemoved?.AddListener(action);
        }
        public void RemoveOnPhysicalKeyRemovedListener(UnityAction<VirtualKeyboardPhysicalKey> action)
        {
            if (action != null) onPhysicalKeyRemoved?.RemoveListener(action);
        }


        public void AddOnPhysicalKeyExecutedListener(UnityAction<VirtualKeyboardPhysicalKey, string> action)
        {
            if (action != null) onPhysicalKeyExecuted?.AddListener(action);
        }
        public void RemoveOnPhysicalKeyExecutedListener(UnityAction<VirtualKeyboardPhysicalKey, string> action)
        {
            if (action != null) onPhysicalKeyExecuted?.RemoveListener(action);
        }

        public void AddOnPhysicalKeysLayoutUpdatedListener(UnityAction<VirtualKeyboardPhysicalKey[]> action)
        {
            if (action != null) onPhysicalKeysLayoutUpdated?.AddListener(action);
        }
        public void RemoveOnPhysicalKeysLayoutUpdatedListener(UnityAction<VirtualKeyboardPhysicalKey[]> action)
        {
            if (action != null) onPhysicalKeysLayoutUpdated?.RemoveListener(action);
        }

        private void UpdateKeyboardKey() {
            if (InputManager.TryGetActifWritteableInputKey(out KeyboardKey key) && InputManager.GetKey(key)) {       
                UpdateTextDisplayer(key);
            };
        }

        private void UpdateUnifiedUpperCase() {
            bool isInputTriggeredDown = InputManager.GetKeyDown(KeyboardKey.LeftShift) || InputManager.GetKeyDown(KeyboardKey.RightShift);

            if (holdUpperCase) {
                UpdateHoldCaseTrigger(isInputTriggeredDown);
            }
            else {
                if (isInputTriggeredDown) {
                    isUpperCase = holdUpperCase ? true : !isUpperCase;
                    UpdatePhysicalKeyTextDisplayed();
                }
            }
        }

        private void UpdateHoldCaseTrigger(bool isInputTriggeredDown)
        {
            bool isInputTriggeredUp = InputManager.GetKeyUp(KeyboardKey.LeftShift) || InputManager.GetKeyUp(KeyboardKey.RightShift);
            bool canUpdatePhysicalKeysDisplayers = false;

            if (isInputTriggeredDown)
            {
                isUpperCase = true;
                canUpdatePhysicalKeysDisplayers = true;
            }
            else if (isInputTriggeredUp) {
                isUpperCase = false;
                canUpdatePhysicalKeysDisplayers = true;
            }

            if(canUpdatePhysicalKeysDisplayers)
                UpdatePhysicalKeyTextDisplayed();
        }

        private void UpdatePhysicalKeyTextDisplayed()
        {
            foreach (var physicalKey in physicalKeys.Where(x => x != null))
                physicalKey?.UpdateTextDisplayed(isUpperCase);
        }

        private VirtualKeyboardPhysicalKey[] GetActifPhysicalKeys() {
            if (physicalKeys == null) return new VirtualKeyboardPhysicalKey[0];

            var excludedPhysicalKeys = physicalKeys.Where(x => x != null)
                                       .Where(x => excludedPhysicalKeyTypes.Contains(x.Type));

            return physicalKeys.Where(x => x != null)
                               .Where(x => !excludedKeys.Contains(x.Key))
                               .Where(x => !excludedPhysicalKeys.Contains(x))
                               .ToArray();
        }

        private void InstantiateKeys() {
            if (templateInputButton == null) return;

            foreach (KeyboardKey key in InputManager.GetWritingKeys()){
                if(physicalKeys.Where(x => x != null).Where(x => x.IsSimilar(key, isUpperCase)).Count() > 0 || key.ToString().Contains("Numpad")) {
                    continue;
                }
                
                SetPhysicalKeyButton(key, Instantiate(templateInputButton));
            }
        }

        public void ResetTextDisplayer() {
           
            UpdateTextDisplayer(string.Empty);
        }

        private void UpdateTextDisplayer(KeyboardKey key) {
            if (physicalKeys == null) return;
            var results = GetActifPhysicalKeys().Where(x => x.Key == key);

            if(results.Count() > 0) {
                results.First().Simulate();
            }
        }

        private void UpdateLayout()
        {
            var actifKeys   = GetActifPhysicalKeys();
            var inactifKeys = physicalKeys.Where(x => x != null).Where(x => !actifKeys.Contains(x));

            foreach (var key in physicalKeys.Where(x => x != null)) {
                if (inactifKeys.Contains(key)) key?.Disable();
                else                           key?.Enable();
            }

            onPhysicalKeysLayoutUpdated?.Invoke(actifKeys);
        }
        public void SetMaxWordsCount(int wordsCount) {
            this.maxWordsCount = Mathf.Clamp(wordsCount, 1, int.MaxValue);
        }

        public virtual void AddPhysicalKey(VirtualKeyboardPhysicalKey physicalKey) {
            if (physicalKey == null || physicalKeys == null) return;

            if (!physicalKeys.Contains(physicalKey)){
                onPhysicalKeyAdded?.Invoke(physicalKey);
            }
        }
        public virtual void RemovePhysicalKey(VirtualKeyboardPhysicalKey physicalKey) {
            if (physicalKey == null || physicalKeys == null) return;

            if (physicalKeys.Contains(physicalKey)) {
                onPhysicalKeyRemoved?.Invoke(physicalKey);  
            }
        }

        protected abstract void SetPhysicalKeyButton(KeyboardKey key, Button button);
        protected abstract void UpdateTextDisplayer(string text);
        protected abstract void UpdateUnifiedUpperCase(Button button, string text);
    }
}
