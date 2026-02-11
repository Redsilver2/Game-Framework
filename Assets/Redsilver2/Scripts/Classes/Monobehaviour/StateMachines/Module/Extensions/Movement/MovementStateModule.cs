using RedSilver2.Framework.StateMachines.States;

namespace RedSilver2.Framework.StateMachines.Controllers
{
    public abstract class MovementStateModule : StateModule
    {
        protected sealed override void SetStateMachine(ref StateMachine stateMachine)
        {
            if(transform.root.TryGetComponent(out StateMachineController controller)) {
                if (controller is MovementStateMachineController) stateMachine = (controller as MovementStateMachineController).StateMachine;
            }
        }
    }
}
