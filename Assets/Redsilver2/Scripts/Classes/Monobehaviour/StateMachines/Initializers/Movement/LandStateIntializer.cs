using RedSilver2.Framework.StateMachines.Controllers;

namespace RedSilver2.Framework.StateMachines.States
{
    public class LandStateIntializer : MovementStateInitializer
    {
        protected sealed override State GetInitializerState(StateMachineController controller)
        {
            if(controller == null || !CanAddOrRemoveState(controller)) return null;
            return new LandState(controller.StateMachine as MovementStateMachine);
        }

        protected sealed override void OnStateAdded(MovementState state) {

        }

        protected sealed override void OnStateRemoved(MovementState state) {

        }

        protected sealed override MovementStateType[] GetIncludedStates()
        {
            return null;
        }
    }
}
