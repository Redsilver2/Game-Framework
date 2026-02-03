using RedSilver2.Framework.StateMachines.Controllers;

namespace RedSilver2.Framework.StateMachines.States
{
    public class IdolStateInitializer : MovementStateInitializer
    {
        protected sealed override State GetInitializerState(StateMachineController controller) {
            if(controller == null || !CanAddOrRemoveState(controller)) return null;
            return new IdolState(controller.StateMachine as MovementStateMachine);
        }

        protected override MovementStateType[] GetIncludedStates() {
            return null;
        }

        protected sealed override void OnStateAdded(MovementState state) {

        }

        protected sealed override void OnStateRemoved(MovementState state) {

        }
    }
}
