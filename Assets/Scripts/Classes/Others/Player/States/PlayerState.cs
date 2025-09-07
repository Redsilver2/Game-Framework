using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace RedSilver2.Framework.Player
{
    public partial class PlayerStateMachine 
    {
        public abstract class PlayerState
        {
            private readonly UnityEvent onEnterState;
            private readonly UnityEvent onExitState;

            private   readonly List<PlayerStateTransitionCondition> transitionConditions;
            protected readonly PlayerStateMachine owner;
            public PlayerStateMachine Owner => owner;

            protected PlayerState() { }

            protected PlayerState(PlayerStateMachine owner)
            {
                this.owner           = owner;
                onEnterState         = new UnityEvent();
                
                onExitState          = new UnityEvent();
                transitionConditions = new List<PlayerStateTransitionCondition>();

                AddOnEnterStateListener(OnStateEnter);
                AddOnExitStateListener(OnStateExit);

                Startup(owner);        
            }

            protected virtual void Startup(PlayerStateMachine owner)
            {
                if (owner != null)
                {
                    owner.AddOnUpdateListener(UpdateTransition);
                    owner.AddOnStateRemovedListener(OnStateRemoved);
                }
            }

            private void OnStateRemoved(PlayerState state)
            {
                if(state == this)
                {
                    if (owner != null)
                    {
                        owner.RemoveOnUpdateListener(UpdateTransition);
                        owner.RemoveOnStateRemovedListener(OnStateRemoved);
                        if(transitionConditions != null) transitionConditions.Clear();
                    }
                }
            }

            private void Transition(PlayerStateMachine owner)
            {
                if (owner != null) { owner.ChangeState(GetStateName()); }
            }
            
            public abstract string GetStateName();
            protected abstract void SetObligatoryTransition(PlayerState[] states, bool removeTransition);
           
            public void AddObligatoryTransition()
            {
                if (owner != null) SetObligatoryTransition(owner.GetStates(), false);
            }

            public void RemoveObligatoryTransition()
            {
                if (owner != null) SetObligatoryTransition(owner.GetStates(), true);
            }

            private bool CanTransition()
            {
                if(transitionConditions == null) return false;
                return transitionConditions.Where(x => x.WasConditionMeet()).Count() == transitionConditions.Count;
            }
            
            
            private void UpdateTransition()
            {
                if (CanTransition())
                {
                    Transition(owner);
                }
            }

            protected virtual void OnStateEnter()
            {
                if (owner != null) owner.RemoveOnUpdateListener(UpdateTransition);
            }

            protected virtual void OnStateExit()
            {
                if (owner != null) owner.AddOnUpdateListener(UpdateTransition);
            }

            public void Enter()
            {
                if (onEnterState != null) onEnterState.Invoke();
            }

            public void Exit()
            {
                if (onExitState != null)  onExitState.Invoke(); 
            }

            public void AddOnEnterStateListener(UnityAction action)
            {
                if(onEnterState != null && action != null) onEnterState.AddListener(action);
            }

            public void RemoveOnEnterStateListener(UnityAction action)
            {
                if (onEnterState != null && action != null) onEnterState.RemoveListener(action);
            }

            public void AddOnExitStateListener(UnityAction action)
            {
                if (onExitState != null && action != null) onExitState.AddListener(action);
            }

            public void RemoveOnExitStateListener(UnityAction action)
            {
                if (onExitState != null && action != null) onExitState.RemoveListener(action);
            }


            public void AddTransitionCondition(string name)
            {
                if(owner != null)
                {
                    AddTransitionCondition(owner.GetTransitionCondition(name));
                }
            }

            public void AddTransitionCondition(PlayerStateTransitionCondition transitionCondition)
            {
                if (transitionConditions != null && transitionCondition != null)
                {
                    if (!transitionConditions.Contains(transitionCondition) && transitionCondition.IsCompatible(this)) 
                    {
                        transitionConditions.Add(transitionCondition);
                    }
                }
            }

            public void RemoveTransitionCondition(string name)
            {
                if (owner != null)
                {
                    RemoveTransitionCondition(owner.GetTransitionCondition(name));
                }
            }

            public void RemoveTransitionCondition(PlayerStateTransitionCondition transitionCondition)
            {
                if(transitionConditions != null && transitionCondition != null)
                {
                    if (transitionConditions.Contains(transitionCondition))
                    {
                        transitionConditions.Remove(transitionCondition);
                    }
                }
            }

            public PlayerStateTransitionCondition GetTransitionCondition()
            {
                if (transitionConditions != null)
                {
                    var results = transitionConditions.ToArray();
                    if (results.Length > 0) return results.First();
                }

                return null;
            }

            public PlayerStateTransitionCondition[] GetTransitionConditions()
            {
                if(transitionConditions != null)
                    return transitionConditions.ToArray();
                return null;
            }
        }


        public void AddOnStateEnterListener(UnityAction<PlayerState> action)
        {
            if (onStateEnter != null && action != null) onStateEnter.AddListener(action);
        }
        public void RemoveOnStateEnterListener(UnityAction<PlayerState> action)
        {
            if (onStateEnter != null && action != null) onStateEnter.RemoveListener(action);
        }

        public void AddOnStateExitListener(UnityAction<PlayerState> action)
        {
            if (onStateExit != null && action != null) onStateExit.AddListener(action);
        }
        public void RemoveOnStateExitListener(UnityAction<PlayerState> action)
        {
            if (onStateExit != null && action != null) onStateExit.RemoveListener(action);
        }

        public void AddOnStateAddedListener(UnityAction<PlayerState> action)
        {
            if (onStateAdded != null && action != null) onStateAdded.AddListener(action);
        }
        public void RemoveOnStateAddedListener(UnityAction<PlayerState> action)
        {
            if (onStateAdded != null && action != null) onStateAdded.RemoveListener(action);
        }

        public void AddOnStateRemovedListener(UnityAction<PlayerState> action)
        {
            if (onStateRemoved != null && action != null) onStateRemoved.AddListener(action);
        }
        public void RemoveOnStateRemovedListener(UnityAction<PlayerState> action)
        {
            if (onStateRemoved != null && action != null) onStateRemoved.RemoveListener(action);
        }

        private void SetStates(PlayerStateModule[] modules)
        {
            SetStates(modules, 0);
        }

        private void SetStates(PlayerStateModule[] modules, int defaultState)
        {
            if (modules != null)
            {
                modules = modules.Where(x => x != null).ToArray();
                defaultState = Mathf.Clamp(defaultState, 0, modules.Length - 1);

                AddStates(modules);
                if (modules.Length > 0) ChangeState(modules[defaultState].State.GetStateName());
            }
        }       
        private void SetStates(PlayerStateModule[] modules, string name)
        {
            if (modules != null)
            {
                modules = modules.Where(x => x != null).ToArray();
                AddStates(modules);

                PlayerState state = GetState(name);
                if (state != null) ChangeState(state.GetStateName());
            }
        }

        public void AddState(PlayerStateModule module)
        {
            if (module == null) return;
            AddState(module.State);
        }

        private void AddState(PlayerState state)
        {
            if (states != null && state != null)
            {
                string name = state.GetStateName().ToLower();

                if (!string.IsNullOrEmpty(name) && !states.ContainsKey(name))
                    onStateAdded.Invoke(state);
            }
        }

        public void AddStates(PlayerStateModule[] modules)
        {
            if (modules != null)
            {
                foreach (PlayerStateModule module in modules)
                {
                    AddState(module);
                }
            }
        }

        public void RemoveState(PlayerStateModule module)
        {
            if (module != null && states != null)
            {
                onStateRemoved.Invoke(module.State);
            }
        }


        private void OnRemoveState(PlayerState state)
        {
            string name = state == null ? string.Empty : state.GetStateName().ToLower();
            if (states == null || string.IsNullOrEmpty(name) || !states.ContainsKey(name)) return;

            states.Remove(name);
            state.RemoveObligatoryTransition();

            if (states[name] == currentState) currentState = null;
        }

        private void OnStateAdded(PlayerState state)
        {
            if (states != null)
            {
                states.Add(state.GetStateName().ToLower(), state);

                if (isInitialized)
                {
                    SetAllTransitionsForState();
                    state.AddObligatoryTransition();            
                }
            }
        }

        public PlayerState GetState(string name)
        {
            name = name.ToLower();

            if (states == null || string.IsNullOrEmpty(name) || !states.ContainsKey(name)) return null;
            return states[name];
        }

        public PlayerState[] GetStates(string[] names)
        {
            List<PlayerState> results = new List<PlayerState>();

            if(names != null && states != null)
            {
                foreach(string name in names)
                {
                    PlayerState state = GetState(name);
                    if(state != null) results.Add(state);
                }
            }

            return results.ToArray();
        }

        public PlayerState[] GetStates()
        {
           if(states != null) return states.Values.ToArray();
            return null;
        }


    }
}
