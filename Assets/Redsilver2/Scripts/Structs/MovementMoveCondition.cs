
using RedSilver2.Framework.StateMachines.States.Configurations;
using System.Linq;

namespace RedSilver2.Framework.StateMachines.States.Conditions {
    public sealed class MovementMoveCondition : MovementStateCondition
    {
        protected MovementMoveCondition(MovementStateMachine stateMachine) : base(stateMachine) {

        }

        public override bool GetTransitionState()
        {
            return stateMachine == null ? false : stateMachine.IsMoving;
        }

        protected sealed override void SetConditionType(ref MovementStateConditionType conditionType) {
            conditionType = MovementStateConditionType.Moving;
        }

        public static MovementMoveCondition GetOrCreate(MovementStateMachine stateMachine)
        {
            MovementMoveCondition condition = GetCondition(stateMachine);
            if (condition != null) return condition;
            return new MovementMoveCondition(stateMachine);
        }


        private static MovementMoveCondition GetCondition(MovementStateMachine movementStateMachine) {
            var results = GetBaseConditions(movementStateMachine).Where(x => x is MovementMoveCondition);
            if (results.Count() > 0) return results.First() as MovementMoveCondition;
            return null;
        }

        public static bool IsMoving(MovementStateMachine configuration) {
            MovementStateCondition condition = GetCondition(configuration);
            return condition == null ? false : condition.GetTransitionState();
        }
    }
}
