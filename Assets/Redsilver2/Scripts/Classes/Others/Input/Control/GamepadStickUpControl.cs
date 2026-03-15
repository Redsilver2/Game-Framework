using System.IO;
using UnityEngine;
using UnityEngine.Events;


namespace RedSilver2.Framework.Inputs
{
    public sealed class GamepadStickUpControl : GamepadStickDirectionControl
    {
        public GamepadStickUpControl(bool isLeftStick, Sprite icon) : base(isLeftStick, icon)
        {
        }

        public GamepadStickUpControl(GamepadStick stick, Sprite icon) : base(stick, icon)
        {
        }

        protected sealed override float GetAxisValue(Vector2 vector)
        {
            if(vector.y > 0f) { return vector.y; }
            return 0f;
        }
    }
}
