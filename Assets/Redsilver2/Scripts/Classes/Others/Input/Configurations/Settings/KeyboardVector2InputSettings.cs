using RedSilver2.Framework.Inputs.Configurations;
using UnityEngine;

namespace RedSilver2.Framework.Inputs.Settings
{
    [CreateAssetMenu(fileName = "New Keyboard Vector2 Input Settings", menuName = "Input/Settings/Vector2/Keyboard")]
    public sealed class KeyboardVector2InputSettings : Vector2InputSettings
    {
        [Space]
        public KeyboardKey DefaultUpKey    = KeyboardKey.W;
        public KeyboardKey DefaultDownKey  = KeyboardKey.S ;
        public KeyboardKey DefaultLeftKey  = KeyboardKey.A;
        public KeyboardKey DefaultRightKey = KeyboardKey.D;

        [Space]
        public GamepadStick DefaultGamepadStick = GamepadStick.LeftStick;

        public sealed override async void Disable()
        {
            (await GetConfiguration())?.Disable();
        }

        public sealed override async void Enable()
        {
            (await GetConfiguration())?.Enable();
        }


        public async Awaitable<KeyboardVector2InputConfiguration> GetConfiguration()
        {
            return await InputManager.GetOrCreateKeyboardInputConfiguration(InputName, this);
        }
    }
}
