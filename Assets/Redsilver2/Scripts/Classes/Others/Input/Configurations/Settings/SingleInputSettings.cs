using UnityEngine;
using RedSilver2.Framework.Inputs.Configurations;
using System.Threading.Tasks;

namespace RedSilver2.Framework.Inputs.Settings
{
    [CreateAssetMenu(fileName = "New Single Input Configuration Settings", menuName = "Input/Settings/Single Input")]
    public sealed class SingleInputSettings : InputSettings
    {
        [Space]
        public bool IsKeyboardDefault;
        public SingleInputType SingleInput;

        [Space]
        public KeyboardKey DefaultKeyboardKey;
        public MouseButton DefaultMouseButton;

        [Space]
        public GamepadButton DefaultGamepadButton;

        public sealed override async void Disable()
        {
            (await GetConfiguration())?.Disable();
        }

        public sealed override async void Enable()
        {
            (await GetConfiguration())?.Enable();
        }

        public async Awaitable<SingleInputConfiguration> GetConfiguration()
        {
            return await InputManager.GetOrCreateSingleInputConfiguration(InputName, this);
        }
    }
}
