namespace RedSilver2.Framework.StateMachines.States
{
    public class CrouchMovementStateCondition : MovementStateCondition
    {
        private CrouchStateInitializer initializer;


        protected override void Awake()
        {
            base.Awake();
            initializer = transform.root.GetComponentInChildren<CrouchStateInitializer>();
        }

        public override bool GetTransitionState()
        {
            if (initializer == null) return false;
            return initializer.TransitionState;
        }

        protected sealed override string GetModuleName() {
            return "Crouch Movement State Condition";
        }

        protected sealed override void OnStateModuleAdded(StateModule module)
        {
            if (module is CrouchStateInitializer) { initializer = module as CrouchStateInitializer; }
        }
    }
}
