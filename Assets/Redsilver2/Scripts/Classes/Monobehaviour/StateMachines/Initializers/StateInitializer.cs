using RedSilver2.Framework.StateMachines.Controllers;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace RedSilver2.Framework.StateMachines.States {
    public abstract class StateInitializer : MonoBehaviour {

        protected State                  state;
        protected StateMachineController controller;

        private UnityAction<State> onStateAdded;
        private UnityAction<State> onStateRemoved;

        private List<State> registeredStates;

        protected virtual void Start() {
            controller = transform.root.GetComponent<StateMachineController>();
            registeredStates = new List<State>();

            state = GetInitializerState(controller);

            onStateAdded = GetOnStateAddedAction();
            onStateRemoved = GetOnStateRemovedAction();

            AddDefaultEvent();

            AddState();
        }

        protected virtual void OnEnable() {
           if(didStart) AddState();
        }
        protected virtual void OnDisable() {
           if(didStart) RemoveState();
        }

        protected virtual void OnDestroy() {
            RemoveDefaultEvent();
        }

        protected virtual void AddDefaultEvent() {
            if (controller == null || controller.StateMachine == null) return;
           
            StateMachine stateMachine = controller.StateMachine;  
            foreach(State state  in stateMachine.GetStates()) OnStateAdded(state);

            stateMachine?.AddOnStateAddedListener(OnStateAdded);
            stateMachine?.AddOnStateRemovedListener(OnStateRemoved);
            stateMachine?.AddStateInitializer(this);
        }

        protected virtual void RemoveDefaultEvent() {
            if (controller == null || controller.StateMachine == null) return;
            StateMachine stateMachine = controller.StateMachine;
            foreach (State state in stateMachine.GetStates()) OnStateRemoved(state);


            stateMachine?.RemoveOnStateAddedListener(OnStateAdded);
            stateMachine?.RemoveOnStateRemovedListener(OnStateRemoved);
            stateMachine?.RemoveStateInitializer(this);
        }

        protected virtual void OnStateAdded(State state)
        {
            if (!registeredStates.Contains(state))  {
                registeredStates?.Add(state);
                onStateAdded?.Invoke(state);
            }
        }

        protected virtual void OnStateRemoved(State state) {
            if (registeredStates.Contains(state)) {
                registeredStates?.Remove(state);
                onStateRemoved?.Invoke(state);
            }
        }
  
        private void AddState(){
            if (controller == null || controller.StateMachine == null || state == null)
                return;


            if (CanAddOrRemoveState(controller)) controller.StateMachine?.AddState(state.GetStateName(), state);
            OnStateAdded(state);
        }
        private void RemoveState() {
            if (controller == null || controller.StateMachine == null || state == null)
                return;

            OnStateRemoved(state);  
            if (CanAddOrRemoveState(controller)) controller.StateMachine?.RemoveState(state.GetStateName());
        }

        protected abstract State GetInitializerState(StateMachineController controller);
        protected abstract bool CanAddOrRemoveState(StateMachineController controller);

        protected abstract UnityAction<State> GetOnStateAddedAction();
        protected abstract UnityAction<State> GetOnStateRemovedAction();
    }
}
