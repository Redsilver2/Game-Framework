using UnityEngine;
namespace RedSilver2.Framework.Inputs {
    public class InputStringSpace : InputButtonControl, IWriteableInput
    {
        public InputStringSpace(string path) : base(path) { }
        public InputStringSpace(string path, Sprite icon) : base(path, icon) { }

        public InputStringOperationType GetStringOperationType() {
            return InputStringOperationType.Space;
        }

        public char GetValue(bool getUpperCase)
        {
            return ' ';
        }

        public void Write(bool isUpperCase, int wordsLimit, ref string value, out bool wasExecuted)
        {
            wasExecuted = false;

            if (value.Length + 1 <= wordsLimit)
            {
                wasExecuted = true;
                value += GetValue(isUpperCase);
            }
        }
    }
}