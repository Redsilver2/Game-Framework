using RedSilver2.Framework.StateMachines.States.Conditions;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Events;

namespace RedSilver2.Framework.StateMachines.States.Configurations
{
    public abstract class StateConfiguration {
        private List<StateCondition>          invalidConditions;
        private List<StateCondition>          validConditions;
        private readonly List<StateCondition> defaultStateConditions;

        private UnityEvent onAdded;
        private UnityEvent onRemoved;

        private UnityEvent onEntered;
        private UnityEvent onExited;

        protected readonly StateMachine  StateMachine;

        protected StateConfiguration(StateMachine stateMachine) {
            this.StateMachine    = stateMachine;


            this.onAdded    = new UnityEvent();
            this.onRemoved  = new UnityEvent();

            this.onEntered = new UnityEvent();
            this.onExited  = new UnityEvent();

            invalidConditions  = new List<StateCondition>();
            validConditions    = new List<StateCondition>();
 
            defaultStateConditions = new List<StateCondition>();


            AddOnAddedListener(() => {
                foreach (StateCondition condition in defaultStateConditions) 
                    AddStateCondition(condition);

                StateMachine?.AddStateConfiguration(this);
            });

            AddOnRemovedListener(() => {
                foreach (StateCondition condition in defaultStateConditions)
                    RemoveStateCondition(condition);

                StateMachine?.RemoveStateConfiguration(this);
            });

            foreach (StateCondition condition in StateCondition.GetBaseConditions(stateMachine))
                AddStateCondition(condition);
        }


        public void AddOnEnteredListener(UnityAction action)
        {
            if (action != null) onEntered?.AddListener(action);
        }

        public void RemoveOnEnteredListener(UnityAction action)
        {
            if (action != null) onEntered?.RemoveListener(action);
        }

        public void AddOnExitedListener(UnityAction action)
        {
            if (action != null) onExited?.AddListener(action);
        }
        public void RemoveOnExitedListener(UnityAction action)
        {
            if (action != null) onExited?.RemoveListener(action);
        }

        public void Enter()
        {
            onEntered?.Invoke();
        }

        public void Exit()
        {
            onExited?.Invoke();
        }

        public void Add() {

            if (StateMachine == null || StateMachine.ContainsStateConfiguration(this))
                return;

            onAdded?.Invoke();
        }

        public void Remove() {
            if (StateMachine == null || !StateMachine.ContainsStateConfiguration(this))
                return;

            foreach (StateCondition condition in defaultStateConditions) RemoveStateCondition(condition);
            onRemoved?.Invoke();
        }

        public virtual bool CanTransition() {
            if (invalidConditions == null || validConditions == null)
                return false;

            if (!ValidateResults(invalidConditions.ToArray(), true))   return false;
            if (!ValidateResults(validConditions.ToArray(),   false))  return false;

            return true;
        }

        private bool ValidateResults(StateCondition[] conditions, bool returnOppositeResults) {
            if (conditions == null) return false;
            var results = returnOppositeResults ? conditions.Where(x => x != null).Where(x => !x.GetTransitionState())
                                                : conditions.Where(x => x != null).Where(x => x.GetTransitionState());

            return results.Count() == conditions.Where(x => x != null).Count();
        }

        public void AddStateCondition(StateCondition condition)
        {
            if (invalidConditions != null && IsInvalidCondition(condition))  {
                if(!invalidConditions.Contains(condition)) invalidConditions?.Add(condition);
            }
            else if (validConditions != null && IsValidCondition(condition)) {
                if (!validConditions.Contains(condition)) validConditions?.Add(condition);
            }
        }

        public void RemoveStateCondition(StateCondition condition) {
            RemoveStateCondition(ref invalidConditions, condition);
            RemoveStateCondition(ref validConditions,   condition);
        }

        protected void AddDefaultStateCondition(StateCondition condition)
        {
            if (condition == null || defaultStateConditions == null) return;

            if(IsValidCondition(condition) || IsInvalidCondition(condition)) {
                defaultStateConditions?.Add(condition);
            }
        }
       

        private void RemoveStateCondition(ref List<StateCondition> results, StateCondition condition)
        {
            if(results == null || results.Count() == 0) return;
            if(results.Contains(condition)) results?.Remove(condition);
        }

        public void AddOnAddedListener(UnityAction action)
        {
            if (action != null) onAdded?.AddListener(action);
        }

        public void RemoveOnAddedListener(UnityAction action) {
            if (action != null) onAdded?.RemoveListener(action);
        }

        public void AddOnRemovedListener(UnityAction action) {
            if (action != null) onRemoved?.AddListener(action);
        }

        public void RemoveOnRemovedListener(UnityAction action) {
            if (action != null) onRemoved?.RemoveListener(action);
        }
        public abstract bool IsValidCondition(StateCondition condition);
        public abstract bool IsInvalidCondition(StateCondition condition);

        public static StateConfiguration[] GetBaseConfigurations(StateMachine machine) {
            List<StateConfiguration> baseConfigurations = new List<StateConfiguration>();
            StateSettings[] settings = StateSettings.GetBaseSettings(machine);

            if(settings == null || settings.Length == 0) return baseConfigurations.ToArray();

            foreach (var setting in settings) {
                if (setting == null) continue;
                var config = setting.GetBaseConfiguration(machine);
                if (config != null) { baseConfigurations.Add(config); }
            }

            return baseConfigurations.ToArray();
        }
    }
}

