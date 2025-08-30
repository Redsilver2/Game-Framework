namespace RedSilver2.Framework.Inputs
{
    public interface IOverridableVector2Input
    {
        void OverrideStick   (GamepadStick stick);
        void OverrideUpKey   (KeyboardKey key);
        void OverrideDownKey (KeyboardKey key);
        void OverrideLeftKey (KeyboardKey key);
        void OverrideRightKey(KeyboardKey key);
    }
}
