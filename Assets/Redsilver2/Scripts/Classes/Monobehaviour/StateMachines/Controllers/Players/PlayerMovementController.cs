using RedSilver2.Framework.Inputs.Settings;
using RedSilver2.Framework.Player;
using RedSilver2.Framework.StateMachines.Controllers;
using UnityEngine;

namespace RedSilver2.Framework.StateMachines
{
    public abstract class PlayerMovementController : PlayerController
    {
        [SerializeField] private CameraController defaultCameraController;

        private PlayerMovementStateMachine movementController;
        public CameraController DefaultCameraController => defaultCameraController;


        protected override void Awake()
        {
            base.Awake();
            movementController = GetComponent<PlayerMovementStateMachine>();

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
    }
}
