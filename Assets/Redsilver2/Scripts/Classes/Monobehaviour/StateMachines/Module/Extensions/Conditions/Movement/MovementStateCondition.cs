using RedSilver2.Framework.StateMachines.States.Configurations;

namespace RedSilver2.Framework.StateMachines.States.Conditions
{
    public abstract class MovementStateCondition : StateCondition
    {
        protected readonly MovementStateMachine stateMachine;
        public readonly MovementStateConditionType ConditionType;

        protected MovementStateCondition(MovementStateMachine stateMachine) : base(stateMachine) {
            SetConditionType(ref ConditionType);
            this.stateMachine = stateMachine;
        }

        protected abstract void SetConditionType(ref MovementStateConditionType conditionType);
    }
}
