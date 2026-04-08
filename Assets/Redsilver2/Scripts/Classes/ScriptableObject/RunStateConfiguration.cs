using RedSilver2.Framework.StateMachines.States.Conditions;
using UnityEngine.Events;

namespace RedSilver2.Framework.StateMachines.States.Configurations
{
    public sealed class RunStateConfiguration : MovementStateConfiguration
    {
        public RunStateConfiguration(MovementStateMachine stateMachine, RunStateSettings stateSettings) : base(stateMachine)
        {
            AddDefaultStateCondition(MovementRunCondition.GetOrCreate(stateMachine));

            AddOnAddedListener(() => {
                stateMachine?.AddOnUpdateListener(GetUpdateAction(stateSettings));
            });

            AddOnRemovedListener(() => {
                stateMachine?.RemoveOnUpdateListener(GetUpdateAction(stateSettings));
                stateMachine?.SetIsRunning(false);
            });
        }

        private UnityAction GetUpdateAction(RunStateSettings stateSettings)
        {
            return () =>
            {
                if (stateSettings == null) return;
                var hold = stateSettings.HoldRunInputSettings != null ? stateSettings.HoldRunInputSettings.GetConfiguration() : null;
                var pressed = stateSettings.PressRunInputSettings != null ? stateSettings.PressRunInputSettings.GetConfiguration() : null;

                if (hold != null) {
                    (StateMachine as MovementStateMachine)?.SetIsRunning(hold.Value);
                }
            };
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
