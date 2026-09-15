using RedSilver2.Framework.Player;
using RedSilver2.Framework.StateMachines.Controllers;
using UnityEngine;


namespace RedSilver2.Framework.StateMachines.Events
{
    public class PlayerStateMachineCameraController : UpdatableStateMachineEvent
    {
        [SerializeField, SerializeReference, HideInInspector] private CameraController cameraController;
        public CameraController CameraController => cameraController;

        private const string EVENT_NAME = "Player Camera Controller";

        protected PlayerStateMachineCameraController(string name, PlayerMovementStateMachine stateMachine, CameraController controller) : base(name, stateMachine) {
            this.cameraController = controller;
        }

        public void SetCameraController(CameraController cameraController)
        {
            this.cameraController = cameraController;
        }

        protected sealed override void Disable(UpdatableStateMachine stateMachine)
        {
            stateMachine?.RemoveOnUpdateListener(OnUpdate);
            stateMachine?.RemoveOnLateUpdateListener(OnLateUpdate);
        }

        protected sealed override void Enable(UpdatableStateMachine stateMachine)
        {
            stateMachine?.AddOnUpdateListener(OnUpdate);
            stateMachine?.AddOnLateUpdateListener(OnLateUpdate);
        }

        private void OnUpdate() {
            cameraController?.Update();
        }

        private void OnLateUpdate() {
            cameraController?.LateUpdate();
        }

        public static void Create(PlayerMovementStateMachine stateMachine)
        {
            Create(stateMachine, null);
        }

        public static void Create(PlayerMovementStateMachine stateMachine, CameraController controller) {
            if (stateMachine == null || Get(stateMachine) != null) return;
            stateMachine?.AddEvent(new PlayerStateMachineCameraController(EVENT_NAME, stateMachine, controller));
        }

        public static PlayerStateMachineCameraController Get(PlayerMovementStateMachine stateMachine) {
            if (stateMachine == null) return null;
            return stateMachine.GetEvent(EVENT_NAME) as PlayerStateMachineCameraController; 
        }
    }
}
