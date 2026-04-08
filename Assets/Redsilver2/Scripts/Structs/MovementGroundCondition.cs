using RedSilver2.Framework.StateMachines.States.Configurations;
using System.Linq;

namespace RedSilver2.Framework.StateMachines.States.Conditions
{
    public sealed class MovementGroundCondition : MovementStateCondition
    {
        protected MovementGroundCondition(MovementStateMachine stateMachine) : base(stateMachine) {

        }

        public override bool GetTransitionState() {

            return stateMachine == null ? false : stateMachine.IsGrounded;
        }

        protected override void SetConditionType(ref MovementStateConditionType conditionType)
        {
            conditionType = MovementStateConditionType.Grounded;
        }

        public static MovementGroundCondition GetOrCreate(MovementStateMachine stateMachine) {

            return new MovementGroundCondition(stateMachine);
        }


        private static MovementGroundCondition GetCondition(MovementStateMachine stateMachine)
        {
            var results = GetBaseConditions(stateMachine).Where(x => x is MovementGroundCondition);
            if (results.Count() > 0) return results.First() as MovementGroundCondition;
            return null;
        }

        public static bool IsGrounded(MovementStateMachine stateConfiguration) {
            MovementGroundCondition condition = GetCondition(stateConfiguration);
            return condition == null ? false : condition.GetTransitionState();
        }
    }
}
