using RedSilver2.Framework.Player;
using RedSilver2.Framework.StateMachines.Controllers;
using RedSilver2.Framework.StateMachines.States;
using UnityEngine;
using UnityEngine.Events;


namespace RedSilver2.Framework.StateMachines.Events
{
    [System.Serializable]
    public abstract partial class PlayerMovementHandler 
    {
        [SerializeField, SerializeReference] private PlayerMovementStateMachine stateMachine;
        [SerializeField, SerializeReference] private CameraController cameraController;

        private Vector3 nextPosition; 

        protected PlayerMovementHandler(PlayerMovementStateMachine stateMachine) {
            this.stateMachine = stateMachine;
        }

        protected void SetCameraController(CameraController cameraController) {
             this.cameraController = cameraController;
        }

        public void Update()     { 
            cameraController?.Update();
            Update(stateMachine); 
        }
        public void LateUpdate() {
            cameraController?.LateUpdate();
            LateUpdate(stateMachine);
        }

        private void Update(PlayerMovementStateMachine stateMachine) {
            nextPosition = GetNextPosition(stateMachine);
        }

        private void LateUpdate(PlayerMovementStateMachine stateMachine) {
            stateMachine?.Move(nextPosition);
        }

        protected abstract Vector3 GetNextPosition(PlayerMovementStateMachine stateMachine);
    }
    public abstract partial class PlayerMovementHandler
    {
#if UNITY_EDITOR
        public void DrawInspector(StateMachine.InspectorVisualizer visualizer) {
            if (visualizer == null || cameraController == null) return;
            EditorExtension.Space(10f);
            cameraController?.DrawInspector(visualizer.FoldoutColor, visualizer.ButtonColor, visualizer.BackgroundColor);
        }
#endif
    }
}
