using RedSilver2.Framework.Inputs.Settings;
using UnityEngine;


namespace RedSilver2.Framework.Inputs.Configurations
{
    [System.Serializable]
    public sealed class MouseVector2InputConfiguration : Vector2InputConfiguration
    {
        private GamepadStick defaultGamepadStick;

        public MouseVector2InputConfiguration(MouseVector2InputSettings settings) : base(settings)
        {
            if (settings != null) {
                defaultGamepadStick = settings.DefaultGamepadStick;
               
                Initialize();
            }
        }

        public override void Reset() {
            Debug.Log("Nothing to override bruh");
        }

        protected override void Initialize(string name, ref InputHandler handler)
        {
            if (base.IsOverrideable()) {
                // Is Overrideable 

            }
            else {
                handler = InputManager.GetOrCreateMouseVector2Input(name, defaultGamepadStick);
            }
        }
    }
}
