using UnityEngine;

namespace RedSilver2.Framework.Inputs
{
    public abstract class GamepadStickDirectionControl : InputStickDirectionControl
    {
        public readonly GamepadStick Stick;

        protected GamepadStickDirectionControl(bool isLeftStick, Sprite icon) : base(icon) {
            Stick = isLeftStick ? GamepadStick.LeftStick : GamepadStick.RightStick;
        }

        protected GamepadStickDirectionControl(GamepadStick stick, Sprite icon) : base(icon) {
            Stick = stick;
        }

        protected sealed override float GetAxisValue()
        {
            return Mathf.Round(GetAxisValue(InputManager.GetGamepadVector2(Stick)));
        }

        protected abstract float GetAxisValue(Vector2 vector);
    }
}
