using RedSilver2.Framework.Inputs.Settings;
using RedSilver2.Framework.Player;
using RedSilver2.Framework.StateMachines.Controllers;
using UnityEngine;

namespace RedSilver2.Framework.StateMachines
{
    public abstract class PlayerMovementController : PlayerController
    {
        [SerializeField] private CameraController defaultCameraController;

        private PlayerMovementStateMachine stateMachine;
        public CameraController DefaultCameraController => defaultCameraController;


        protected override void Awake()
        {
            base.Awake();
            stateMachine = GetComponent<PlayerMovementStateMachine>();

            AddOnDisabledListener(() => {
                CameraController.SetCursorVisibility(true);
                CameraController.SetCurrent(null as CameraController);
                if (stateMachine != null) stateMachine.enabled = false;
            });

            AddOnEnabledListener(() => {
                CameraController.SetCursorVisibility(false);
                CameraController.SetCurrent(defaultCameraController);

                if(stateMachine != null) stateMachine.enabled = true;
            });

            if (stateMachine != null) stateMachine.enabled = enabled;
        }

        protected virtual void Start()
        {
            if (defaultCameraController != null)
                defaultCameraController.enabled = true;
        }

        private void SetControllerState(bool isEnabled) {
            if(stateMachine != null) stateMachine.enabled = isEnabled;
        }
    }
}
