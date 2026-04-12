using RedSilver2.Framework.StateMachines.States.Conditions;

namespace RedSilver2.Framework.StateMachines.States.Configurations
{
    public sealed class IdolStateConfiguration : MovementStateConfiguration
    {
        public IdolStateConfiguration(MovementStateMachine stateMachine) : base(stateMachine) {
            AddDefaultStateCondition(MovementGroundCondition.GetOrCreate(stateMachine));
            AddDefaultStateCondition(MovementMoveCondition.GetOrCreate(stateMachine));
        }

        protected override MovementStateType[] GetValidStateTransitions()
        {
            return new MovementStateType[] {
                MovementStateType.Land,   MovementStateType.Walk, MovementStateType.Run,
                MovementStateType.Crouch
            };
        }

        protected override void SetInvalidConditionsTypes(ref MovementStateConditionType[] conditionTypes)
        { 
            conditionTypes = new MovementStateConditionType[] {
                MovementStateConditionType.Moving , MovementStateConditionType.Crouching, MovementStateConditionType.Running, 
                MovementStateConditionType.Falling, MovementStateConditionType.Jumping,
            };
        }

        protected sealed override void SetMovementType(ref MovementStateType type)
        {
            type = MovementStateType.Idol;
        }

        protected sealed override void SetValidConditionsTypes(ref MovementStateConditionType[] conditionTypes)
        {
            conditionTypes = new MovementStateConditionType[] {
               MovementStateConditionType.Grounded, MovementStateConditionType.StateValidation
            };
        }
    }
}
