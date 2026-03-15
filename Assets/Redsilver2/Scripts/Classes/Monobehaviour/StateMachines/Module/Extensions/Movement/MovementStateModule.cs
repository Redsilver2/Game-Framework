using RedSilver2.Framework.StateMachines.States;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

namespace RedSilver2.Framework.StateMachines.Controllers
{
    public abstract class MovementStateModule : StateModule
    {
        protected sealed override StateMachineController GetStateMachineStateMachine(StateMachineController controller)
        {
            if (controller is not MovementStateMachineController) return null;
            return  base.GetStateMachineStateMachine(controller);
        }

        protected sealed override UnityAction<State> GetOnStateAddedAction()
        {
            return state =>
            {
                OnStateAdded(state as MovementState);
            };
        }
        protected sealed override UnityAction<State> GetOnStateRemovedAction()
        {
            return state =>
            {
                OnStateRemoved(state as MovementState);
            };
        }

        protected abstract void OnStateAdded(MovementState state);
        protected abstract void OnStateRemoved(MovementState state);
    }
}
