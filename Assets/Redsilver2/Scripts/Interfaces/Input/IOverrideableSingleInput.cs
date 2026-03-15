namespace RedSilver2.Framework.Inputs
{
    public interface IOverrideableSingleInput
    {
        void OverrideKey(KeyboardKey key);
        void OverrideKey(MouseButton button);
        void OverrideKey(GamepadButton button);
    }
}
