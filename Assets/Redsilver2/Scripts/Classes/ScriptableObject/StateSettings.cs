using System.Collections.Generic;
using UnityEngine;

namespace RedSilver2.Framework.StateMachines.States.Configurations {
    public abstract class StateSettings : ScriptableObject {

        private Dictionary<StateMachine, StateConfiguration> stateConfigurations;
        private static Dictionary<StateMachine, List<StateSettings>> stateSettings = new Dictionary<StateMachine, List<StateSettings>>();

        public virtual async void Register(StateMachine stateMachine) {

            if(stateConfigurations == null)
                stateConfigurations = new Dictionary<StateMachine, StateConfiguration>();

            if (stateMachine == null || stateConfigurations == null || stateConfigurations.ContainsKey(stateMachine))
                return;

            StateConfiguration configuration = CreateBaseConfiguration(stateMachine);
            configuration?.Add();

            if (configuration != null)
            {
                stateConfigurations?.Add(stateMachine, configuration);
                AddStateSetting(stateMachine, this);
            }
        }

        public void Unregister(StateMachine stateMachine) {
            if (stateMachine == null || stateConfigurations == null || !stateConfigurations.ContainsKey(stateMachine))
                return;

            stateConfigurations[stateMachine]?.Remove();
            stateConfigurations?.Remove(stateMachine);

            RemoveStateSetting(stateMachine, this);
        }

        public StateConfiguration GetBaseConfiguration(StateMachine stateMachine) {
            if (stateMachine == null || stateConfigurations == null || !stateConfigurations.ContainsKey(stateMachine))
                return null;

            return stateConfigurations[stateMachine];
        }

        private static void AddStateSetting(StateMachine stateMachine, StateSettings settings)
        {
            if (stateMachine == null || settings == null || stateSettings == null)
                return;

            if (!stateSettings.ContainsKey(stateMachine))
                stateSettings?.Add(stateMachine, new List<StateSettings>());

            if (!stateSettings[stateMachine].Contains(settings))
                stateSettings[stateMachine]?.Add(settings);
        }

        private static void RemoveStateSetting(StateMachine stateMachine, StateSettings settings)
        {
            if (stateMachine == null || settings == null || stateSettings == null || !stateSettings.ContainsKey(stateMachine))
                return;

            if (stateSettings[stateMachine].Contains(settings)) {
                stateSettings[stateMachine]?.Remove(settings);
            }
        }

        public static StateSettings[] GetBaseSettings(StateMachine machine)
        {
            if(machine == null || stateSettings == null || !stateSettings.ContainsKey(machine)) 
                return new StateSettings[0];

            return stateSettings[machine].ToArray();
        }

        protected abstract StateConfiguration CreateBaseConfiguration(StateMachine stateMachine);
    }
}