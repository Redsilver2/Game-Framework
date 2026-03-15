using UnityEngine;

namespace RedSilver2.Framework.Inputs
{
    public sealed class GamepadStickLeftControl : GamepadStickDirectionControl
    {
        public GamepadStickLeftControl(GamepadStick stick, Sprite icon) : base(stick, icon)
        {
        }

        public GamepadStickLeftControl(bool isLeftStick, Sprite icon) : base(isLeftStick, icon)
        {
        }
        protected sealed override float GetAxisValue(Vector2 vector)
        {
            if (vector.x < 0f) { return vector.x; }
            return 0f;
        }
    }
}
