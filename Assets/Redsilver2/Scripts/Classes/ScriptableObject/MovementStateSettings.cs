namespace RedSilver2.Framework.StateMachines.States.Configurations
{
    public abstract class MovementStateSettings : StateSettings
    {
        public override void Register(StateMachine stateMachine)
        {
            if(stateMachine is MovementStateMachine)
                 base.Register(stateMachine);
        }

        protected sealed override StateConfiguration CreateBaseConfiguration(StateMachine stateMachine) {
            return GetMovementStateConfiguration(stateMachine as MovementStateMachine);            
        }

        protected abstract MovementStateConfiguration GetMovementStateConfiguration(MovementStateMachine stateMachine);
    }
}