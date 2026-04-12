

using RedSilver2.Framework.StateMachines.States.Configurations;
using System.Linq;

namespace RedSilver2.Framework.StateMachines.States.Conditions {
    public sealed class ValidMovementStateTransitionsCondition : MovementStateCondition
    {
        private readonly MovementStateType[] validTransitionTypes;

        protected ValidMovementStateTransitionsCondition(MovementStateMachine stateMachine, MovementStateType[] validTransitionTypes) : base(stateMachine) {
            this.validTransitionTypes = validTransitionTypes;
        }

        public override bool GetTransitionState()
        {
            if (stateMachine == null) return false;
            MovementStateConfiguration configuration = stateMachine.CurrentStateConfiguration as MovementStateConfiguration;
            if (configuration == null || validTransitionTypes == null) return true;

            return validTransitionTypes.Contains(configuration.Type);
        }

        protected sealed override void SetConditionType(ref MovementStateConditionType conditionType)
        {
            conditionType = MovementStateConditionType.StateValidation;
        }

        public static ValidMovementStateTransitionsCondition GetOrCreate(MovementStateMachine stateMachine, MovementStateType[] types)
        {
            ValidMovementStateTransitionsCondition condition = GetCondition(stateMachine);
            if (condition != null) return condition;
            return new ValidMovementStateTransitionsCondition(stateMachine, types);
        }


        private static ValidMovementStateTransitionsCondition GetCondition(MovementStateMachine stateMachine)
        {
            var results = GetBaseConditions(stateMachine).Where(x => x is ValidMovementStateTransitionsCondition);
            if (results.Count() > 0) return results.First() as ValidMovementStateTransitionsCondition;
            return null;
        }
    }
}
