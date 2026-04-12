using RedSilver2.Framework.StateMachines.Controllers;
using RedSilver2.Framework.StateMachines.States;
using RedSilver2.Framework.StateMachines.States.Configurations;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;


namespace RedSilver2.Framework.StateMachines
{
    [System.Serializable]
    public abstract partial class StateMachine 
    {
        private bool isEnabled;
        private StateConfiguration currentStateConfiguration;

        private readonly UnityEvent onDisabled;
        private readonly UnityEvent onEnabled;

        private readonly UnityEvent<StateConfiguration> onStateConfigurationAdded;
        private readonly UnityEvent<StateConfiguration> onStateConfigurationRemoved;

        private readonly UnityEvent<StateConfiguration> onStateEntered;    
        private readonly UnityEvent<StateConfiguration> onStateExited;

        private readonly List<StateConfiguration> stateConfigurations;
        protected readonly StateMachineController Controller;

        public bool  IsEnabled    => isEnabled;
        public StateConfiguration CurrentStateConfiguration => currentStateConfiguration;

        protected StateMachine(StateMachineController controller) {
            stateConfigurations         = new List<StateConfiguration>();
          
            onStateConfigurationAdded   = new UnityEvent<StateConfiguration>();
            onStateConfigurationRemoved = new UnityEvent<StateConfiguration>();

            onStateEntered = new UnityEvent<StateConfiguration>();
            onStateExited  = new UnityEvent<StateConfiguration>();

            onDisabled = new UnityEvent();
            onEnabled  = new UnityEvent();

            Controller = controller;

            AddOnEnabledListener(()  => { isEnabled = true; });
            AddOnDisabledListener(() => { isEnabled = false; });

            AddOnStateEnteredListener(configuration => { 
                currentStateConfiguration = configuration;
                currentStateConfiguration?.Enter();
            });
            AddOnStateExitedListener (configuration => {
                if (currentStateConfiguration == configuration) {
                    currentStateConfiguration?.Exit();
                    currentStateConfiguration = null;
                }
            });

            isEnabled = false;
        }

        public void Enable(){
            onEnabled?.Invoke(); 
        }

        public void Disable() {
            onDisabled?.Invoke();
        }

        public bool IsCurrentConfiguration(StateConfiguration configuration) {
            if(currentStateConfiguration == null) return false;
            return currentStateConfiguration.Equals(configuration);
        }

        public void ClearCurrentState() {
            onStateExited?.Invoke(currentStateConfiguration);
        }

        public void ChangeState(StateConfiguration configuration) {
            if (stateConfigurations == null || !stateConfigurations.Contains(configuration))
                return;

            onStateExited?.Invoke(currentStateConfiguration);
            onStateEntered?.Invoke(configuration);
        }

        public virtual void AddStateConfiguration(StateConfiguration configuration) {
            if (configuration == null || stateConfigurations == null || stateConfigurations.Contains(configuration))
                return;

            stateConfigurations?.Add(configuration);
            onStateConfigurationAdded?.Invoke(configuration);   
        }

        public void RemoveStateConfiguration(StateConfiguration configuration) {
            if(configuration == null || stateConfigurations == null || !stateConfigurations.Contains(configuration)) 
                return;

            stateConfigurations?.Remove(configuration);
            onStateConfigurationRemoved?.Invoke(configuration);
        }

        public bool ContainsStateConfiguration(StateConfiguration configuration) {
            return stateConfigurations == null ? false : stateConfigurations.Contains(configuration);
        }

        public void AddOnEnabledListener(UnityAction action)
        {
            if (action != null)
                onEnabled?.AddListener(action);
        }
        public void RemoveOnEnabledListener(UnityAction action)
        {
            if (action != null)
                onEnabled?.RemoveListener(action);
        }

        public void AddOnDisabledListener(UnityAction action)
        {
            if (action != null)
                onDisabled?.AddListener(action);
        }
        public void RemoveOnDisabledListener(UnityAction action)
        {
            if (action != null)
                onDisabled?.RemoveListener(action);
        }

        public void AddOnStateAddedListener(UnityAction<StateConfiguration> action)
        {
            if(action != null)
                onStateConfigurationAdded?.AddListener(action);
        }
        public void RemoveOnStateAddedListener(UnityAction<StateConfiguration> action)
        {
            if (action != null)
                onStateConfigurationAdded?.RemoveListener(action);
        }
      
        public void AddOnStateRemovedListener(UnityAction<StateConfiguration> action)
        {
            if (action != null)
                onStateConfigurationRemoved?.AddListener(action);
        }
        public void RemoveOnStateRemovedListener(UnityAction<StateConfiguration> action) {
            if (action != null)
                onStateConfigurationRemoved?.RemoveListener(action);
        }

        public void AddOnStateEnteredListener(UnityAction<StateConfiguration> action)
        {
            if (action != null)
                onStateEntered?.AddListener(action);
        }
        public void RemoveOnStateEnteredListener(UnityAction<StateConfiguration> action)
        {
            if (action != null)
                onStateEntered?.RemoveListener(action);
        }

        public void AddOnStateExitedListener(UnityAction<StateConfiguration> action)
        {
            if (action != null)
                onStateExited?.AddListener(action);
        }
        public void RemoveOnStateExitedListener(UnityAction<StateConfiguration> action)
        {
            if (action != null)
                onStateExited?.RemoveListener(action);
        }

        public StateConfiguration[] GetStateConfigurations()
        {
            if(stateConfigurations == null) return new StateConfiguration[0];
            return stateConfigurations.ToArray();   
        }
    }

}
