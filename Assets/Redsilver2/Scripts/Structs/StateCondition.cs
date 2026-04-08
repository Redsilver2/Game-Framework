using RedSilver2.Framework.StateMachines.States.Configurations;
using System.Collections.Generic;
using System.Linq;

namespace RedSilver2.Framework.StateMachines.States.Conditions {
    public abstract class StateCondition : ICheckableStateTransition {

        private   static   Dictionary<StateMachine, List<StateCondition>> stateConditions = new Dictionary<StateMachine, List<StateCondition>>();
        protected StateCondition(StateMachine stateMachine) {
            AddCondition(stateMachine, this);
        }

        public abstract bool GetTransitionState();


        private static void AddCondition(StateMachine stateMachine, StateCondition condition)
        {
            if (stateMachine == null || condition == null || stateConditions == null)
                return;

            if(!stateConditions.ContainsKey(stateMachine))
                stateConditions?.Add(stateMachine, new List<StateCondition>());

            if (!stateConditions[stateMachine].Contains(condition)) {
                stateConditions[stateMachine]?.Add(condition);
            } 
        }

        protected static void RemoveCondition(StateMachine stateMachine, StateConfiguration configuration, StateCondition condition){
            if (configuration == null || condition == null || stateConditions == null)
                return;

            configuration?.RemoveStateCondition(condition);
            RemoveCondition(stateMachine, condition);
        }


        protected static void RemoveCondition(StateMachine stateMachine, StateCondition condition) {
            if (stateMachine == null || condition == null || stateConditions == null || !stateConditions.ContainsKey(stateMachine))
                return;

            if (stateConditions[stateMachine].Contains(condition)) {
                var configs = StateConfiguration.GetBaseConfigurations(stateMachine).Where(x => x != null).ToArray();

                if (CanRemoveCondition(configs, condition)) {
                    foreach(StateConfiguration configuration in configs) configuration?.RemoveStateCondition(condition);
                    stateConditions[stateMachine]?.Remove(condition);
                }
            }
        }

        private static bool CanRemoveCondition(StateConfiguration[] configurations, StateCondition condition) {
            if(configurations == null || condition == null) return false; 

            foreach (var config in configurations.Where(x => x != null)) {
                if (config == null) break;

                if (config.IsInvalidCondition(condition) || config.IsValidCondition(condition))
                    return false;
            }

            return true;
        }

        public static StateCondition[] GetBaseConditions(StateMachine configuration) {
            if (configuration == null || stateConditions == null || !stateConditions.ContainsKey(configuration))
                return new StateCondition[0];

            return stateConditions[configuration].ToArray();
        }
    }
}
