using RedSilver2.Framework.Player;
using RedSilver2.Framework.StateMachines.Controllers;


namespace RedSilver2.Framework.StateMachines
{
    public abstract class PlayerMovementController : PlayerController
    {
        private PlayerMovementStateMachine stateMachine;

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
                if(stateMachine != null) stateMachine.enabled = true;
            });

            if (stateMachine != null) stateMachine.enabled = enabled;
        }
    }
}
