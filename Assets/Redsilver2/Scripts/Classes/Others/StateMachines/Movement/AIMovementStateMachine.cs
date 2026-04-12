using RedSilver2.Framework.StateMachines.Controllers;
using UnityEngine;

namespace RedSilver2.Framework.StateMachines
{
    public class AIMovementStateMachine : MovementStateMachine
    {
        public AIMovementStateMachine(AIMovementController controller) : base(controller) {
            AddOnUpdateListener(() => {
                controller?.SetAgentSpeed(GetMoveSpeed());
            });
        }

        protected override void Move() {
            AIMovementController controller = GetAIMovementController();

            if(controller != null && Transform != null) {
                controller?.SetAgentSpeed(GetMoveSpeed());    
                Move(controller.GetVelocity());
            } 
        }

        public void SetTarget(Transform transform) {
            GetAIMovementController()?.SetTarget(transform);
        }

        protected AIMovementController GetAIMovementController() {
            return Controller as AIMovementController;  
        }
    }
}
