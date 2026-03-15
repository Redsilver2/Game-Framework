using UnityEngine;

namespace RedSilver2.Framework.Inputs
{
    public sealed class GamepadStickDownControl : GamepadStickDirectionControl
    {
        public GamepadStickDownControl(bool isLeftStick, Sprite icon) : base(isLeftStick, icon)
        {
        }

        public GamepadStickDownControl(GamepadStick stick, Sprite icon) : base(stick, icon)
        {
        }

        protected sealed override float GetAxisValue(Vector2 vector)
        {
            if (vector.y < 0f) { return vector.y; }
            return 0f;
        }
    }
}
