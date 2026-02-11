namespace RedSilver2.Framework.StateMachines.States {
    public abstract class StateInitializer : StateModule {

        protected State defaultState;

        protected override void Start()
        {
            base.Start();
            defaultState = GetDefaultState(stateMachine);
            stateMachine?.AddState(defaultState);
        }


        protected sealed override void OnEnable() {
           base.OnEnable();
            stateMachine?.AddState(defaultState);
        }
        protected sealed override void OnDisable() {
           base.OnDisable();
            stateMachine?.RemoveState(defaultState);

        }

        protected abstract State GetDefaultState(StateMachine controller);
        protected abstract bool  CanAddOrRemoveState(StateMachine controller);
    }
}
