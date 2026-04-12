using RedSilver2.Framework.StateMachines.States;
using UnityEngine.Events;

namespace RedSilver2.Framework.StateMachines.Controllers
{
    public abstract class MovementStateModule : StateConfigurationModule
    {
        protected sealed override UpdateableStateMachineController GetStateMachineStateMachine(UpdateableStateMachineController controller)
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
    }
}
