using RedSilver2.Framework.StateMachines.Controllers;
using RedSilver2.Framework.StateMachines.States;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;


namespace RedSilver2.Framework.StateMachines
{
    [System.Serializable]
    public abstract partial class StateMachine : IUpdateable, ILateUpdateable
    {
        private bool isEnabled;
        private State currentState;

        private readonly List<StateModule> stateModules;

        private readonly UnityEvent onUpdate;
        private readonly UnityEvent onLateUpdate;

        private readonly UnityEvent onDisabled;
        private readonly UnityEvent onEnabled;

        private readonly UnityEvent<State> onStateAdded;
        private readonly UnityEvent<State> onStateRemoved;

        private readonly UnityEvent<State> onStateEntered;
        private readonly UnityEvent<State> onStateExited;

        private readonly UnityEvent<StateModule> onStateModuleAdded;
        private readonly UnityEvent<StateModule> onStateModuleRemoved;

        private readonly Dictionary<string, State> states;
        public  readonly StateMachineController Controller;

        public bool IsEnabled => isEnabled;

        protected StateMachine(StateMachineController controller) {
            onStateAdded   = new UnityEvent<State>();
            onStateRemoved = new UnityEvent<State>();

            onStateModuleAdded   = new UnityEvent<StateModule>();
            onStateModuleRemoved = new UnityEvent<StateModule>();

            onEnabled            = new UnityEvent();
            onDisabled           = new UnityEvent();

            onUpdate             = new UnityEvent();
            onLateUpdate         = new UnityEvent();

            onStateEntered       = new UnityEvent<State>();
            onStateExited        = new UnityEvent<State>();

            states               = new Dictionary<string, State>();  
            stateModules         = new List<StateModule>();

            Controller = controller;

            AddOnEnabledListener(()  => { isEnabled = true; });
            AddOnDisabledListener(() => { isEnabled = false; });

            AddOnStateEnteredListener(state => { currentState = state; });
            AddOnStateExitedListener (state => { currentState = null;  });

            AddOnStateAddedListener(DefaultStateAddedListener);
            AddOnStateRemovedListener(DefaultStateRemovedListener);

            isEnabled = false;
        }

        public void Update() { 
           onUpdate?.Invoke();
        }

        public void LateUpdate() {
           onLateUpdate?.Invoke();
        }

        public void Enable(){
            onEnabled?.Invoke(); 
        }

        public void Disable() {
            onDisabled?.Invoke();
        }

        public void ChangeState(string stateName) {
            if (string.IsNullOrEmpty(stateName) || states == null || !states.ContainsKey(stateName.ToLower())) return;

            State state = states[stateName.ToLower()];
            if (states == null || state == null || !states.ContainsValue(state) || !state.IsValidTransition()) return;

            onStateExited?.Invoke(currentState);
            onStateEntered?.Invoke(state);
        }

        protected void ChangeState(State state) {


            Debug.Log("New State: " + state + " " + state.IsValidTransition()); 

            if(state != null) ChangeState(state.GetStateName());
        }

        public virtual void AddStateModule(StateModule module)
        {
            if (stateModules == null || module == null) return;

            if (!stateModules.Contains(module)) {
                stateModules?.Add(module);
                onStateModuleAdded?.Invoke(module);
            }
        }

        public void RemoveStateModule(StateModule module)
        {
            if (stateModules == null || module == null) return;

            if (stateModules.Contains(module)) {
                stateModules?.Remove(module);
                onStateModuleRemoved?.Invoke(module);
            }
        }


        public virtual void AddState(State state)
        {
            AddState(GetStateName(state), state);
        }


        public virtual void AddState(string stateName, State state) {
            if (states == null || state == null || string.IsNullOrEmpty(stateName)) return;
            stateName = stateName.ToLower();

            if (states.ContainsKey(stateName)) return;

            states.Add(stateName, state);
            onStateAdded?.Invoke(states[stateName]);
        }

        public  void RemoveState(State state)
        {
            RemoveState(GetStateName(state));
        }

        public virtual void RemoveState(string stateName)
        {
            if (states == null || string.IsNullOrEmpty(stateName)) return;
            stateName = stateName.ToLower();

            if (!states.ContainsKey(stateName)) return;
            onStateRemoved?.Invoke(states[stateName]);
            states.Remove(stateName);
        }



        protected virtual void DefaultStateAddedListener(State state)
        {
            if (state == null) return;
            if (currentState == null) { ChangeState(state); }
        }

        protected virtual void DefaultStateRemovedListener(State state)
        {
            if (state == null) return;
            // if (currentState == state) { ChangeState(null); }
        }

        public bool ContainsState(string stateName) {
            if (states == null || string.IsNullOrEmpty(stateName)) return false;
            return states.ContainsKey(stateName.ToLower());
        }
        public bool ContainsState(State state) {
            if(states == null || state == null) return false;
            return states.ContainsValue(state);
        }

        public void AddOnUpdateListener(UnityAction state)
        {
            if (state != null)
                onUpdate?.AddListener(state);
        }
        public void RemoveOnUpdateListener(UnityAction state)
        {
            if (state != null)
                onUpdate?.RemoveListener(state);
        }

        public void AddOnLateUpdateListener(UnityAction state)
        {
            if (state != null)
                onLateUpdate?.AddListener(state);
        }
        public void RemoveOnLateUpdateListener(UnityAction state)
        {
            if (state != null)
                onLateUpdate?.RemoveListener(state);
        }

        public void AddOnEnabledListener(UnityAction state)
        {
            if (state != null)
                onEnabled?.AddListener(state);
        }
        public void RemoveOnEnabledListener(UnityAction state)
        {
            if (state != null)
                onEnabled?.RemoveListener(state);
        }

        public void AddOnDisabledListener(UnityAction state)
        {
            if (state != null)
                onDisabled?.AddListener(state);
        }
        public void RemoveOnDisabledListener(UnityAction state)
        {
            if (state != null)
                onDisabled?.RemoveListener(state);
        }

        public void AddOnStateAddedListener(UnityAction<State> state)
        {
            if(state != null)
                onStateAdded?.AddListener(state);
        }
        public void RemoveOnStateAddedListener(UnityAction<State> state)
        {
            if (state != null)
                onStateAdded?.RemoveListener(state);
        }
      
        public void AddOnStateRemovedListener(UnityAction<State> state)
        {
            if (state != null)
                onStateRemoved?.AddListener(state);
        }
        public void RemoveOnStateRemovedListener(UnityAction<State> state) {
            if (state != null)
                onStateRemoved?.RemoveListener(state);
        }

        public void AddOnStateEnteredListener(UnityAction<State> state)
        {
            if (state != null)
                onStateEntered?.AddListener(state);
        }
        public void RemoveOnStateEnteredListener(UnityAction<State> state)
        {
            if (state != null)
                onStateEntered?.RemoveListener(state);
        }

        public void AddOnStateExitedListener(UnityAction<State> state)
        {
            if (state != null)
                onStateExited?.AddListener(state);
        }
        public void RemoveOnStateExitedListener(UnityAction<State> state)
        {
            if (state != null)
                onStateExited?.RemoveListener(state);
        }

        public void AddOnStateModuleAddedListener(UnityAction<StateModule> action)
        {
            if (action != null) onStateModuleAdded?.AddListener(action);
        }
        public void RemoveOnStateModuleAddedListener(UnityAction<StateModule> action)
        {
            if (action != null) onStateModuleAdded?.RemoveListener(action);
        }

        public void AddOnStateModuleRemovedListener(UnityAction<StateModule> action)
        {
            if (action != null) onStateModuleRemoved?.AddListener(action);
        }
        public void RemoveOnStateModuleRemovedListener(UnityAction<StateModule> action)
        {
            if (action != null) onStateModuleRemoved?.RemoveListener(action);
        }

        public string[] GetStateNames()
        {
            List<string> results = new List<string>();

            if (this.states != null) {
                foreach (var state in this.states) {
                    results.Add(state.Key);
                }
            }

            return results.ToArray();
        }

        public string GetStateName(State state)
        {
            if (state == null || states == null) return string.Empty;
            var reuslts = states.Where(x => x.Value == state);

            if (reuslts.Count() > 0) return reuslts.First().Key;
            return string.Empty;
        }

        public State[] GetStates() {
            List<State> results = new List<State>();
           
            if(this.states != null) {
                foreach(var state in this.states)
                    results.Add(state.Value);
            }

            return results.ToArray();
        }

        public StateModule[] GetModules()
        {
            if (stateModules != null)
                return stateModules.ToArray();

            return new StateModule[0];
        }

        public StateModule GetModule(string name)
        {
            if (string.IsNullOrEmpty(name) || stateModules == null) return null;

            var results = stateModules.Where(x => x != null)
                                      .Where(x => x.ModuleName.ToLower().Equals(name.ToLower()));

            if(results.Count() > 0) return results.First();
            return null;
        }

        public State GetState(string stateName) {
            if (states == null || string.IsNullOrEmpty(stateName)) return null;
            stateName = stateName.ToLower();

            return states.ContainsKey(stateName) ? states[stateName] : null;
        }

    }

}
