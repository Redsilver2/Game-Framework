using RedSilver2.Framework.StateMachines.States.Conditions;

namespace RedSilver2.Framework.StateMachines.States.Configurations
{
    public sealed class RunStateConfiguration : MovementStateConfiguration
    {
        public RunStateConfiguration(MovementStateMachine stateMachine) : base(stateMachine)
        {
            AddDefaultStateCondition(MovementGroundCondition.GetOrCreate(stateMachine));
            AddDefaultStateCondition(MovementMoveCondition.GetOrCreate(stateMachine));
            AddDefaultStateCondition(MovementRunCondition.GetOrCreate(stateMachine));
        }

        protected override void SetValidConditionsTypes(ref MovementStateConditionType[] conditionTypes)
        {
            conditionTypes = new MovementStateConditionType[] {  
                MovementStateConditionType.Moving  , MovementStateConditionType.Running,
                MovementStateConditionType.Grounded, MovementStateConditionType.StateValidation
            };
        }

        protected override void SetInvalidConditionsTypes(ref MovementStateConditionType[] conditionTypes)
        {
            conditionTypes = new MovementStateConditionType[] { 
                MovementStateConditionType.Crouching, MovementStateConditionType.Jumping,
            };
        }

        protected sealed override MovementStateType[] GetValidStateTransitions() {
            return new MovementStateType[] {
                MovementStateType.Idol, MovementStateType.Walk,
                MovementStateType.Land, MovementStateType.Crouch,
            };
        }

        protected sealed override void SetMovementType(ref MovementStateType type)
        {
            type = MovementStateType.Run;
        }
    }
}
