using RedSilver2.Framework.StateMachines.States.Movement;
using UnityEngine;

namespace RedSilver2.Framework.StateMachines.States
{
    public class MoveMovementStateCondition : MovementStateCondition
    {
        public override bool GetTransitionState()
        {
            if (stateMachine is not MovementStateMachine) return false;
            MovementHandler handler = (stateMachine as MovementStateMachine).MovementHandler;
            
            if(handler == null) return false;
            return handler.GetMagnitude() > 0f;
        }
        protected sealed override void OnStateModuleAdded(StateModule module) { }

        protected sealed override string GetModuleName()
        {
            return "Move Movement State Condition";
        }

    }
}
