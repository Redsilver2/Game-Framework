using UnityEngine;

namespace RedSilver2.Framework.Inputs {
    public interface IWriteableInput {
        void Write(bool isUpperCase, int wordsLimit, ref string value, out bool wasExecuted);
        char GetValue(bool getUpperCase);
        InputStringOperationType GetStringOperationType();
    }
}
