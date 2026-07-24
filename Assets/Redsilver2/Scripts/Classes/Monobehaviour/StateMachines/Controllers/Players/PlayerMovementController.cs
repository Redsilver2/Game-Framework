using RedSilver2.Framework.Inputs.Settings;
using RedSilver2.Framework.Player;
using RedSilver2.Framework.StateMachines.Controllers;
using RedSilver2.Framework.StateMachines.States.Configurations;
using UnityEngine;

namespace RedSilver2.Framework.StateMachines
{
    [RequireComponent(typeof(PlayerMovementStateMachineController))]
    public abstract class PlayerMovementController : PlayerController
    {
        [SerializeField] private MovementInputSettings inputSettings;
        [SerializeField] private CameraController defaultCameraController;

        [Space]
        [SerializeField] private int defaultSettingIndex;
        [SerializeField] private MovementStateSettings[] defaultSettings;

        private PlayerMovementStateMachineController movementController;

        public MovementInputSettings InputSettings => inputSettings;
        public CameraController DefaultCameraController => defaultCameraController;

        public MovementStateMachine StateMachine {
            get {
                if(movementController == null) return null;
                return movementController.StateMachine as MovementStateMachine;
            }
        }

        protected override void Awake()
        {
            base.Awake();
            movementController = GetComponent<PlayerMovementStateMachineController>();
           
            SetStateMachineController(movementController, inputSettings);
            SetDefaultConfigurations();

            inputSettings?.Enable();

            AddOnDisabledListener(() => {
                CameraController.SetCursorVisibility(true);
                CameraController.SetCurrent(null as CameraController);
                if (movementController != null) movementController.enabled = false;
            });

            AddOnEnabledListener(() => {
                CameraController.SetCursorVisibility(false);
                CameraController.SetCurrent(defaultCameraController);

                if(movementController != null) movementController.enabled = true;
            });
        }

        protected virtual void Start()
        {
            if (defaultCameraController != null)
                defaultCameraController.enabled = true;
        }

        private void SetControllerState(bool isEnabled) {
            if(movementController != null) movementController.enabled = isEnabled;
        }

        private void SetDefaultConfigurations() {
            foreach(var settings in defaultSettings)
                settings?.Register(StateMachine);

            if(defaultSettingIndex >= 0 && defaultSettingIndex < defaultSettings.Length - 1)
                StateMachine?.ChangeState(defaultSettings[defaultSettingIndex].GetBaseConfiguration(StateMachine));
        }

        protected abstract void SetStateMachineController(PlayerMovementStateMachineController controller, MovementInputSettings inputSettings);
    }
}
