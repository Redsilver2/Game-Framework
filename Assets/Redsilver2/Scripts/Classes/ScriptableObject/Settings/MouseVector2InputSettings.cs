using RedSilver2.Framework.Inputs.Configurations;
using UnityEngine;

namespace RedSilver2.Framework.Inputs.Settings
{
    [CreateAssetMenu(fileName = "New Mouse Vector2 Input Settings", menuName = "Input/Settings/Vector2/Mouse")]
    public sealed class MouseVector2InputSettings : Vector2InputSettings
    {
        [Space]
        public GamepadStick DefaultGamepadStick = GamepadStick.RightStick;

        public sealed override Vector2InputConfiguration GetBaseConfiguration()
        {
            return GetConfiguration();
        }


        public MouseVector2InputConfiguration GetConfiguration() {
            return InputManager.GetOrCreateMouseInputConfiguration(InputName, this);
        }
    }
}
