using RedSilver2.Framework.Inputs;
using UnityEngine;

public class InputStringDelete : InputButtonControl, IWriteableInput
{
    public InputStringDelete(string path) : base(path) {  }
    public InputStringDelete(string path, Sprite icon) : base(path, icon) { }

    public InputStringOperationType GetStringOperationType() {
        return InputStringOperationType.Delete;
    }

    public char GetValue(bool getUpperCase) {
        return '\0';
    }

    public void Write(bool isUpperCase, int wordsLimit, ref string value, out bool isExecuted) 
    {
        isExecuted = false;

        if(value.Length - 1 >= 0) {
            isExecuted = true;
            value      = value.Remove(value.Length - 1);
        }
    }
}
