using RedSilver2.Framework.StateMachines.States.Conditions;
using System.Linq;
using UnityEngine;

namespace RedSilver2.Framework.StateMachines.States.Configurations {
    public abstract class MovementStateConfiguration : UpdateableStateConfiguration
    { 
        public  readonly MovementStateType             Type;
        private readonly MovementStateType         []  validTypeTransitions;

        private readonly MovementStateConditionType[]  validConditionTypes;
        private readonly MovementStateConditionType[]  invalidConditionTypes;
        
        protected MovementStateConfiguration(MovementStateMachine stateMachine) : base(stateMachine) {
            SetInvalidConditionsTypes(ref invalidConditionTypes);
            SetValidConditionsTypes  (ref validConditionTypes);

            validTypeTransitions = GetValidStateTransitions();
            SetMovementType(ref Type);
        }

        protected abstract void SetValidConditionsTypes(ref MovementStateConditionType[] conditionTypes);
        protected abstract void SetInvalidConditionsTypes(ref MovementStateConditionType[] conditionTypes);
        protected abstract MovementStateType[] GetValidStateTransitions();
        protected abstract void SetMovementType(ref MovementStateType type);

        public sealed override bool IsValidCondition(StateCondition condition) {
            MovementStateCondition stateCondition = condition as MovementStateCondition;    
            
            if(stateCondition == null || validConditionTypes == null || validConditionTypes.Length == 0) 
                return false;

            return validConditionTypes.Contains(stateCondition.ConditionType);
        }

        public override bool IsInvalidCondition(StateCondition condition)
        {
            MovementStateCondition stateCondition = condition as MovementStateCondition;

            if (stateCondition == null || invalidConditionTypes == null || invalidConditionTypes.Length == 0)
                return false;

            return invalidConditionTypes.Contains(stateCondition.ConditionType);
        }

        public override bool CanTransition()
        {
            MovementStateMachine stateMachine = StateMachine as MovementStateMachine;
            if (stateMachine == null) return false;

            if(!CanTransition(stateMachine.CurrentStateConfiguration as MovementStateConfiguration)) {
                return false;
            }

            return base.CanTransition();
        }

        private bool CanTransition(MovementStateConfiguration configuration)
        {
            if (configuration == null) return false;
            
            if(validTypeTransitions != null) {
                if (!validTypeTransitions.Contains(configuration.Type))
                    return false;
            }

            return true;
        }

        public MovementStateMachine GetMovementStateMachine() {
            return StateMachine as MovementStateMachine;   
        }
    }
}
