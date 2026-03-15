using RedSilver2.Framework.StateMachines.Controllers;
using System.Linq;
using UnityEngine;

namespace RedSilver2.Framework.StateMachines.States
{
    public abstract class MovementStateInitializer : StateInitializer
    {
        [SerializeField] private MovementStateType[] transitionStates;

        protected sealed override StateMachineController GetStateMachineStateMachine(StateMachineController controller)
        {
            if (controller is not MovementStateMachineController) return null;
            return base.GetStateMachineStateMachine(controller);
        }

        public sealed override bool IsTransitionState(MovementState state)
        {
            if (state == null || transitionStates == null) return false;
            return transitionStates.Contains(state.Type);
        }

        protected sealed override State GetDefaultState(StateMachine stateMachine)
        {
            if (stateMachine is not MovementStateMachine) return null;
            return GetDefaultState(stateMachine as MovementStateMachine);
        }

        protected sealed override bool CanAddOrRemoveState(State state)
        {
            if(state is MovementState && transitionStates != null)
                return transitionStates.Contains((state as MovementState).Type);

            return false;
        }

        protected abstract MovementState GetDefaultState(MovementStateMachine stateMachine);
    }
}