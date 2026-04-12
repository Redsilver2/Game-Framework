using RedSilver2.Framework.StateMachines.States.Conditions;
using UnityEngine;

namespace RedSilver2.Framework.StateMachines.States.Configurations
{
    public sealed class WalkStateConfiguration : MovementStateConfiguration
    {
        public WalkStateConfiguration(MovementStateMachine stateMachine) : base(stateMachine) {
            AddDefaultStateCondition(MovementGroundCondition.GetOrCreate(stateMachine));
            AddDefaultStateCondition(MovementMoveCondition.GetOrCreate(stateMachine));
        }



        protected override void SetValidConditionsTypes(ref MovementStateConditionType[] conditionTypes) {
            conditionTypes = new MovementStateConditionType[] {
                MovementStateConditionType.Moving, MovementStateConditionType.StateValidation,
                MovementStateConditionType.Grounded
            };
        }

        protected override void SetInvalidConditionsTypes(ref MovementStateConditionType[] conditionTypes) {
            conditionTypes = new MovementStateConditionType[] { 
                MovementStateConditionType.Falling, MovementStateConditionType.Running,
                MovementStateConditionType.Jumping
            };
        }

        protected sealed override MovementStateType[] GetValidStateTransitions()
        {
            return new MovementStateType[] { 
                MovementStateType.Crouch, MovementStateType.Land, MovementStateType.Idol,
                MovementStateType.Run
            };
        }

        protected sealed override void SetMovementType(ref MovementStateType type)
        {
            type = MovementStateType.Walk;
        }
    }
}