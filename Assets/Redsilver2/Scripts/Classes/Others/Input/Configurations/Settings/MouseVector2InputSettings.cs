using RedSilver2.Framework.Inputs.Configurations;
using UnityEngine;

namespace RedSilver2.Framework.Inputs.Settings
{
    [CreateAssetMenu(fileName = "New Mouse Vector2 Input Settings", menuName = "Input/Settings/Vector2/Mouse")]
    public sealed class MouseVector2InputSettings : Vector2InputSettings
    {
        [Space]
        public GamepadStick DefaultGamepadStick = GamepadStick.RightStick;

        public sealed override async void Disable()
        {
            (await GetConfiguration())?.Disable();
        }

        public sealed override async void Enable()
        {
            (await GetConfiguration())?.Enable();
        }


        public async Awaitable<MouseVector2InputConfiguration> GetConfiguration() {
            return await InputManager.GetOrCreateMouseInputConfiguration(InputName, this);
        }
    }
}
