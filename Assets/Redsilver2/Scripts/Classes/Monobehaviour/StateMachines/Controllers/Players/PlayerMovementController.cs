using RedSilver2.Framework.Inputs.Settings;
using RedSilver2.Framework.Player;
using RedSilver2.Framework.StateMachines.Controllers;
using RedSilver2.Framework.StateMachines.States.Configurations;
using UnityEngine;

namespace RedSilver2.Framework.StateMachines
{
    [RequireComponent(typeof(MovementStateMachineController))]
    public abstract class PlayerMovementController : PlayerController
    {
        [SerializeField] private MovementInputSettings inputSettings;

        [Space]
        [SerializeField] private int defaultSettingIndex;
        [SerializeField] private MovementStateSettings[] defaultSettings;

        private MovementStateMachineController movementController;
        private CameraController cameraController;

        public MovementStateMachine StateMachine {
            get
            {
                if(movementController == null) return null;
                return movementController.StateMachine as MovementStateMachine;
            }
        }

        protected override void Awake()
        {
            base.Awake();
            movementController = GetComponent<MovementStateMachineController>();
            cameraController = GetComponentInChildren<CameraController>();

            inputSettings?.Enable();
            CameraController.SetCursorVisibility(false);    
           
            SetStateMachineController(movementController, inputSettings);
            SetDefaultConfigurations();

            AddOnDisabledListener(() =>
            {
                if (movementController != null) movementController.enabled = false;
                if(cameraController != null) cameraController.enabled = false;
            });

            AddOnEnabledListener(() =>
            {
                if(movementController != null) movementController.enabled = true;
                if (cameraController != null) cameraController.enabled = true;
            });
        }

        private void SetControllerState(bool isEnabled) {
            if(movementController != null)
                movementController.enabled = isEnabled;
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

        protected abstract void SetStateMachineController(MovementStateMachineController controller, MovementInputSettings inputSettings);
    }
}
