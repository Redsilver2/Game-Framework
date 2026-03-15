using UnityEngine;

namespace RedSilver2.Framework.Inputs
{
    public sealed class GamepadStickRightControl : GamepadStickDirectionControl
    {
        public GamepadStickRightControl(bool isLeftStick, Sprite icon) : base(isLeftStick, icon)
        {

        }

        public GamepadStickRightControl(GamepadStick stick, Sprite icon) : base(stick, icon)
        {

        }

        protected sealed override float GetAxisValue(Vector2 vector)
        {
            if (vector.x > 0f) { return vector.x; }
            return 0f;
        }
    }
}
