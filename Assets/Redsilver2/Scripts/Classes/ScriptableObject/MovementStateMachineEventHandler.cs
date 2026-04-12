using RedSilver2.Framework.StateMachines.States.Conditions;
using UnityEngine;
using UnityEngine.Events;

namespace RedSilver2.Framework.StateMachines.Handlers
{
    public class MovementStateMachineEventHandler : UpdateableStateEventHandler
    {
        public void AddOnMoveListener(UnityAction<Vector2> action) {
            (stateMachine as MovementStateMachine)?.AddOnMovedListener(action);
        }

        public void RemoveOnMoveListener(UnityAction<Vector2> action) {
            (stateMachine as MovementStateMachine)?.RemoveOnMoveListener(action);
        }

        public void AddOnGroundTagChangedListener(UnityAction<string> action)
        {
            (stateMachine as MovementStateMachine)?.AddOnGroundTagChangedListener(action);
        }
        public void RemoveOnGroundTagChangedListener(UnityAction<string> action) {
            (stateMachine as MovementStateMachine)?.RemoveOnGroundTagChangedListener(action);
        }

        protected override void SetStateMachine(StateMachine stateMachine) {
            if (stateMachine is MovementStateMachine)
                base.SetStateMachine(stateMachine);
        }

        public bool IsMoving() {
            return MovementMoveCondition.IsMoving(stateMachine as MovementStateMachine);
        }

        public bool IsRunning() {
            return MovementRunCondition.IsRunning(stateMachine as MovementStateMachine);
        }

        public bool IsGrounded() {
            return MovementGroundCondition.IsGrounded(stateMachine as MovementStateMachine);
        }
    }
}
