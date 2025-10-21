namespace RedSilver2.Framework.Inputs
{
    public sealed class OverrideableVector2Input : Vector2Input
    {

        public OverrideableVector2Input(string name) : base(name)
        {
        }

        public OverrideableVector2Input(string name, Vector2GamepadStick gamepadStick) : base(name, gamepadStick)
        {
        }

        public void OverrideStick(Vector2GamepadStick gamepadStick) => this.gamepadStick = gamepadStick;
    }
}
