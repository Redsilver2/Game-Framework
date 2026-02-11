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

        protected StateMachine stateMachine;
        public string ModuleName => moduleName;

        protected virtual void Awake()
        {
            moduleName       = GetModuleName();
            registeredStates = new List<State>();

            onStateAdded   = GetOnStateAddedAction();
            onStateRemoved = GetOnStateRemovedAction();
        }

        protected virtual void Start()
        {
            SetStateMachine(ref stateMachine);
            stateMachine?.AddOnStateAddedListener(OnStateAdded);
            stateMachine?.AddOnStateRemovedListener(OnStateRemoved);
            stateMachine?.AddStateModule(this);
        }

        protected virtual void OnEnable()
        {
            if (didStart)
            {
                stateMachine?.AddOnStateAddedListener(OnStateAdded);
                stateMachine?.AddOnStateRemovedListener(OnStateRemoved);
                stateMachine?.AddStateModule(this);
            }
        }

        protected virtual void OnDisable()
        {
            if (didStart)
            {

                stateMachine?.RemoveOnStateAddedListener(OnStateAdded);
                stateMachine?.RemoveOnStateRemovedListener(OnStateRemoved);
                stateMachine?.RemoveStateModule(this);
            }
        }

        protected virtual void OnStateAdded(State state)
        {
            if (!registeredStates.Contains(state))
            {
                registeredStates?.Add(state);
                onStateAdded?.Invoke(state);
            }
        }

        protected virtual void OnStateRemoved(State state)
        {
            if (registeredStates.Contains(state))
            {
                registeredStates?.Remove(state);
                onStateRemoved?.Invoke(state);
            }
        }

        protected abstract void SetStateMachine(ref StateMachine stateMachine);
        protected abstract UnityAction<State> GetOnStateAddedAction();
        protected abstract UnityAction<State> GetOnStateRemovedAction();
        protected abstract string GetModuleName();
    }
}
