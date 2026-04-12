using UnityEngine;
using RedSilver2.Framework.Inputs.Configurations;
using System.Threading.Tasks;

namespace RedSilver2.Framework.Inputs.Settings
{
    public abstract class SingleInputSettings : InputSettings
    {
        [Space]
        public bool IsKeyboardDefault;

        [Space]
        public KeyboardKey DefaultKeyboardKey;
        public MouseButton DefaultMouseButton;

        [Space]
        public GamepadButton DefaultGamepadButton;

        public sealed override async void Disable()
        {
            GetConfiguration()?.Disable();
        }

        public  sealed override void Enable()
        {
            GetConfiguration()?.Enable();
        }

        public SingleInputConfiguration GetConfiguration()
        {
            return InputManager.GetOrCreateSingleInputConfiguration(InputName, this);
        }

        public abstract SingleInputType GetInputType();
    }
}
