using RedSilver2.Framework.StateMachines.Controllers;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace RedSilver2.Framework.StateMachines.States
{
    public abstract class StateConfigurationModule : MonoBehaviour
    {
        private string      moduleName;
        private List<State> registeredStates;

        private UnityAction<State> onStateAdded;
        private UnityAction<State> onStateRemoved;

        private UpdateableStateMachineController controller;
      
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

        }

        protected virtual void OnEnable()
        {
            if (didStart)
            {
           
            }
        }

        protected virtual void OnDisable()
        {
            if (didStart)
            {
             
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

        private UpdateableStateMachineController GetStateMachineController()
        {
            if (transform.root.TryGetComponent(out UpdateableStateMachineController controller)) {
                return GetStateMachineStateMachine(controller);
            }

            return null;
        }

        protected virtual UpdateableStateMachineController GetStateMachineStateMachine(UpdateableStateMachineController controller)
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
