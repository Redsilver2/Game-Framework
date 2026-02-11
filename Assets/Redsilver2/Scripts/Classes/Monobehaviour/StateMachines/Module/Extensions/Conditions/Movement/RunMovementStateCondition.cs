namespace RedSilver2.Framework.StateMachines.States
{
    public class RunMovementStateCondition : MovementStateCondition
    {
        private RunStateInitializer initializer;

        protected override void Awake()
        {
            base.Awake();
            initializer = transform.root.GetComponentInChildren<RunStateInitializer>();
        }

        protected sealed override void OnStateModuleAdded(StateModule module)
        {
            if (module is RunStateInitializer) { initializer = module as RunStateInitializer; }
        }


        public sealed override bool GetTransitionState()
        {
            if(initializer == null) return false;
            return initializer.TransitionState;
        }

        protected sealed override string GetModuleName()
        {
            return "Run Movement State Condition";
        }
    }
}
