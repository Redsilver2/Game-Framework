using UnityEngine;
using UnityEngine.InputSystem;

namespace RedSilver2.Framework.Inputs {
    public sealed class InputStringCharacter : InputButtonControl, IWriteableInput
    {
     

        private readonly char upperCaseValue;
        private readonly char lowerCaseValue;

        public InputStringCharacter(string path, char value) : base(path) {
            upperCaseValue = char.ToUpper(value);
            lowerCaseValue = char.ToLower(value);
        }

        public InputStringCharacter(string path, char upperCaseValue, char lowerCaseValue) : base(path)
        {
            this.upperCaseValue = char.ToUpper(upperCaseValue);
            this.lowerCaseValue = char.ToLower(lowerCaseValue);
        }

        public InputStringCharacter(string path, char value, Sprite icon) : base(path, icon) {
            upperCaseValue = char.ToUpper(value);
            lowerCaseValue = char.ToLower(value);
        }

        public InputStringCharacter(string path, char upperCaseValue, char lowerCaseValue, Sprite icon) : base(path, icon)
        {
            this.upperCaseValue = char.ToUpper(upperCaseValue);
            this.lowerCaseValue = char.ToLower(lowerCaseValue);
        }

        public InputStringOperationType GetStringOperationType()
        {
            return InputStringOperationType.Character;
        }

        public char GetValue(bool isUpperCase) {
            return isUpperCase? upperCaseValue : lowerCaseValue;    
        }

        public void Write(bool isUpperCase, int wordsLimit, ref string value, out bool isExecuted) {
            isExecuted = false;
            
            if(value.Length + 1 <= wordsLimit) {
                isExecuted = true;
                value += GetValue(isUpperCase);
            }    
        }

        public void Write(bool isUpperCase, ref string value) {
            if (!string.IsNullOrEmpty(value)) value += GetValue(isUpperCase);
        }
    }
}
