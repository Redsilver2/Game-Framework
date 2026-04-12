using RedSilver2.Framework.StateMachines.States.Configurations;
using System.Linq;

namespace RedSilver2.Framework.StateMachines.States.Conditions
{
    public sealed class MovementRunCondition : MovementStateCondition
    {
        protected MovementRunCondition(MovementStateMachine machine) : base(machine) {

        }

        public sealed override bool GetTransitionState()
        {
            return stateMachine == null ? false :  stateMachine.IsRunning;
        }

        protected sealed override void SetConditionType(ref MovementStateConditionType conditionType)
        {
            conditionType = MovementStateConditionType.Running;
        }

        public static MovementRunCondition GetOrCreate(MovementStateMachine stateMachine)
        {
            MovementRunCondition condition = GetCondition(stateMachine);
            if (condition != null) return condition;

            return new MovementRunCondition(stateMachine);
        }


        private static MovementRunCondition GetCondition(MovementStateMachine stateMachine)
        {
            var results = GetBaseConditions(stateMachine).Where(x => x is MovementRunCondition);
            if (results.Count() > 0) return results.First() as MovementRunCondition;
            return null;
        }

        public static bool IsRunning(MovementStateMachine stateMachine) {
            MovementRunCondition condition = GetCondition(stateMachine);
            return condition == null ? false : condition.GetTransitionState();
        }
    }
}
