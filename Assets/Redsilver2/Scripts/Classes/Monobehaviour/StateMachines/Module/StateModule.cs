using RedSilver2.Framework.StateMachines.Controllers;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace RedSilver2.Framework.StateMachines.States
{
    public abstract class StateModule : MonoBehaviour
    {
        private string moduleName;
        private List<State> registeredStates;

        private UnityAction<State> onStateAdded;
        private UnityAction<State> onStateRemoved;

        private StateMachineController controller;
      
        protected StateMachine stateMachine 
        {
            get {
                if (controller == null) return null;
                return controller.StateMachine;
            }
        }

        public string ModuleName => moduleName;

        protected virtual async void Awake()
        {
            moduleName       = GetModuleName();
            registeredStates = new List<State>();

            onStateAdded   = GetOnStateAddedAction();
            onStateRemoved = GetOnStateRemovedAction();

            controller = GetStateMachineController();
        }

        protected virtual void Start()
        {
            stateMachine?.AddOnStateAddedListener(OnStateAdded);
            stateMachine?.AddOnStateRemovedListener(OnStateRemoved);
            stateMachine?.AddStateModule(this);

            if (stateMachine != null)
                foreach (State state in stateMachine.GetStates()) OnStateAdded(state);
        }

        protected virtual void OnEnable()
        {
            if (didStart)
            {
                stateMachine?.AddOnStateAddedListener(OnStateAdded);
                stateMachine?.AddOnStateRemovedListener(OnStateRemoved);
                stateMachine?.AddStateModule(this);

                if (stateMachine != null)
                    foreach (State state in stateMachine.GetStates()) OnStateAdded(state); 
            }
        }

        protected virtual void OnDisable()
        {
            if (didStart)
            {
                stateMachine?.RemoveOnStateAddedListener(OnStateAdded);
                stateMachine?.RemoveOnStateRemovedListener(OnStateRemoved);
                stateMachine?.RemoveStateModule(this);

                if (stateMachine != null)
                    foreach (State state in stateMachine.GetStates()) OnStateRemoved(state);
            }
        }

        protected virtual void OnStateAdded(State state)
        {
            if (registeredStates == null || state == null || !CanAddOrRemoveState(state)) return;

            if (!registeredStates.Contains(state))
            {
                registeredStates?.Add(state);
                onStateAdded?.Invoke(state);
            }
        }

        protected virtual void OnStateRemoved(State state)
        {
            if (registeredStates == null || state == null || !CanAddOrRemoveState(state)) return;

            if (registeredStates.Contains(state))
            {
                registeredStates?.Remove(state);
                onStateRemoved?.Invoke(state);
            }
        }

        private StateMachineController GetStateMachineController()
        {
            if (transform.root.TryGetComponent(out StateMachineController controller)) {
                return GetStateMachineStateMachine(controller);
            }

            return null;
        }

        protected virtual StateMachineController GetStateMachineStateMachine(StateMachineController controller)
        {
            if (controller == null) return null;
            return controller;
        }

        protected abstract bool CanAddOrRemoveState(State state);
        protected abstract string GetModuleName();

        protected abstract UnityAction<State> GetOnStateAddedAction();
        protected abstract UnityAction<State> GetOnStateRemovedAction();
    }
}
