using RedSilver2.Framework.StateMachines.States.Conditions;
using UnityEngine;
using UnityEngine.Events;

namespace RedSilver2.Framework.StateMachines.Handlers
{
    public class MovementStateMachineEventHandler : UpdateableStateEventHandler
    {
        public void AddOnMoveListener(UnityAction<Vector2> action) {
            GetMovementStateMachine()?.AddOnMovedListener(action);
        }

        public void RemoveOnMoveListener(UnityAction<Vector2> action) {
            GetMovementStateMachine()?.RemoveOnMoveListener(action);
        }

        public void AddOnGroundTagChangedListener(UnityAction<string> action) {
            GetMovementStateMachine()?.AddOnGroundTagChangedListener(action);
        }
        public void RemoveOnGroundTagChangedListener(UnityAction<string> action) {
            GetMovementStateMachine()?.RemoveOnGroundTagChangedListener(action);
        }

        protected override void SetStateMachine(StateMachine stateMachine) {
            if (stateMachine is MovementStateMachine)
                base.SetStateMachine(stateMachine);
        }

        public bool IsMoving() {
            return MovementMoveCondition.IsMoving(GetMovementStateMachine());
        }

        public bool IsRunning() {
            return MovementRunCondition.IsRunning(GetMovementStateMachine());
        }

        public bool IsGrounded() {
            return MovementGroundCondition.IsGrounded(GetMovementStateMachine());
        }

        private MovementStateMachine GetMovementStateMachine()
        {
            return stateMachine as MovementStateMachine;
        }
    }
}
