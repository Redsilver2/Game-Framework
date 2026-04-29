namespace RedSilver2.Framework.Inputs
{
    public interface IInputStringSpace
    {
        InputStringOperationType GetStringOperationType();
        void Write(bool isUpperCase, int wordsLimit, ref string value, out bool wasExecuted);
    }
}