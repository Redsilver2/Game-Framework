using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RedSilver2.Framework.StateMachines.States
{
    public abstract class MovementConditionStateInitializer : MovementStateInitializer {

        private List<MovementStateConditionCheck> conditions;

        protected bool transitionState;
        public    bool TransitionState => transitionState;

        protected override void Awake()
        {
            base.Awake();
            conditions = new List<MovementStateConditionCheck>();
        }


        protected override void Start()
        {
            base.Start();
            stateMachine?.AddOnUpdateListener(OnUpdate);
            stateMachine?.AddOnStateModuleAddedListener(OnStateModuleAdded);
            stateMachine?.AddOnStateModuleRemovedListener(OnStateModuleRemoved);
            AddAllConditionChecks();
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            stateMachine?.AddOnUpdateListener(OnUpdate);
            stateMachine?.AddOnStateModuleAddedListener(OnStateModuleAdded);
            stateMachine?.AddOnStateModuleRemovedListener(OnStateModuleRemoved);
            AddAllConditionChecks();
        }

        protected override void OnDisable()
        {
            base.OnDisable();
          
            stateMachine?.RemoveOnUpdateListener(OnUpdate);
            stateMachine?.RemoveOnUpdateListener(OnUpdate);
            stateMachine?.RemoveOnStateModuleAddedListener(OnStateModuleAdded);
            stateMachine?.RemoveOnStateModuleRemovedListener(OnStateModuleRemoved);

            RemoveAllConditionChecks();
            transitionState = false;
        }

        private void OnStateModuleAdded(StateModule module) {
            OnStateModuleAdded(module as MovementStateCondition);
        }

        private void OnStateModuleRemoved(StateModule module) {
            OnStateModuleRemoved(module as MovementStateCondition);
        }

        private void OnStateModuleAdded(MovementStateCondition condition)
        {
            if (condition == null || conditions == null) return;

            if (IsValidCondition(condition) && !ContainsDuplicateConditions(condition)) {
                AddConditionCheck(condition);
            }
        }

        private void OnStateModuleRemoved(MovementStateCondition condition)
        {
            if (condition == null || conditions == null) return;
            RemoveConditionCheck(condition);
        }

        private void AddConditionCheck(MovementStateCondition condition)
        {
            if(conditions == null || conditions.Where(x => x.condition == condition).Count() > 0) return;

            MovementStateConditionCheck check = new MovementStateConditionCheck();
            check.condition          = condition;
            check.showOppositeResult = !IsShowOppositeResultCondition(condition);
            conditions?.Add(check);
        }

        private void AddAllConditionChecks()
        {
            if (stateMachine == null) return;
            var results = stateMachine.GetModules().Where(x => x != null).Where(x => x.GetType().Equals(typeof(MovementConditionStateInitializer)));
            if(results.Count() == 0) return;

            foreach (MovementStateCondition condition in results)
                if (IsValidCondition(condition)) AddConditionCheck(condition);
        }

        private void RemoveAllConditionChecks() { 
           if(stateMachine == null) return;
            var results = stateMachine.GetModules().Where(x => x != null).Where(x => x.GetType().Equals(typeof(MovementConditionStateInitializer)));
            if (results.Count() == 0) return;

            foreach (MovementStateCondition condition in results) 
                if(IsValidCondition(condition)) RemoveConditionCheck(condition);
        }

        private void RemoveConditionCheck(MovementStateCondition condition)
        {
            if(condition == null || conditions == null) return;

            var results = conditions.Where(x => x.condition == condition);
            foreach (var result in results) conditions?.Remove(result); 
            
        }

        protected virtual void OnUpdate()
        {
            if (!CheckConditions()) transitionState = false;
        }

        protected bool ContainsDuplicateConditions(MovementStateCondition condition) {
            if(conditions == null || condition == null) return true;
            return conditions.Where(x => x.condition == condition).Count() > 0;
        }

        protected virtual bool CheckConditions()
        {
            if (conditions == null || conditions.Count == 0) return true;
            var results = conditions.Where(x => x.condition != null);
            return results.Where(x => x.GetResult()).Count() == results.Count();
        }

        protected abstract bool IsShowOppositeResultCondition(MovementStateCondition condition);
        protected abstract bool IsValidCondition(MovementStateCondition condition);
    }
}
