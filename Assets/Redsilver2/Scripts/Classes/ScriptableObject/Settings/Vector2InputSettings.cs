using RedSilver2.Framework.Inputs.Configurations;
using UnityEngine;

namespace RedSilver2.Framework.Inputs.Settings
{
    public abstract class Vector2InputSettings : InputSettings {
        public sealed override void Disable()
        {
             GetBaseConfiguration()?.Disable();
        }

        public sealed override async void Enable()
        {
             GetBaseConfiguration()?.Enable();
        }


        public abstract Vector2InputConfiguration GetBaseConfiguration(); 
    }
}
