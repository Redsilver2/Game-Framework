using RedSilver2.Framework.Inputs.Configurations;
using RedSilver2.Framework.Inputs.Settings;
using RedSilver2.Framework.StateMachines.Controllers;
using RedSilver2.Framework.StateMachines.States.Configurations;
using UnityEngine;

namespace RedSilver2.Framework.StateMachines
{
    [RequireComponent(typeof(MovementStateMachineController))]
    public abstract class PlayerMovementController : PlayerController
    {
        [SerializeField] private KeyboardVector2InputSettings moveInputSettings;

        [Space]
        [SerializeField] private int defaultSettingIndex;
        [SerializeField] private MovementStateSettings[] defaultSettings;

        private MovementStateMachineController controller;
        public MovementStateMachine StateMachine {
            get
            {
                if(controller == null) return null;
                return controller.StateMachine as MovementStateMachine;
            }
        }

        protected override void Awake()
        {
            base.Awake();
            controller = GetComponent<MovementStateMachineController>();
           
            SetStateMachineController(controller, moveInputSettings == null ? null : moveInputSettings.GetConfiguration());
            SetDefaultConfigurations();
        }

        private void SetControllerState(bool isEnabled) {
            if(controller != null)
                controller.enabled = isEnabled;
        }

        private void SetDefaultConfigurations() {
            foreach(var settings in defaultSettings) {
                if (settings == null) continue;
                settings.Register(StateMachine);
            }

            if(defaultSettingIndex >= 0 && defaultSettingIndex < defaultSettings.Length - 1) {
                StateMachine?.ChangeState(defaultSettings[defaultSettingIndex].GetBaseConfiguration(StateMachine));
            }
        }

        protected abstract void SetStateMachineController(MovementStateMachineController controller, KeyboardVector2InputConfiguration configuration);
    }
}
