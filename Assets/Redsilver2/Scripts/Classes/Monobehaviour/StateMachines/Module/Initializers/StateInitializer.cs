using UnityEngine.Events;

namespace RedSilver2.Framework.StateMachines.States {
    public abstract class StateInitializer : StateModule {

        private State defaultState;

        protected override void Start()
        {
            defaultState = GetDefaultState(stateMachine);
            stateMachine?.AddState(defaultState);
            base.Start();
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            AddAllTransitionStates();
            stateMachine?.AddState(defaultState);
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            RemoveAllTransitionStates();
            stateMachine?.RemoveState(defaultState);
        }


        protected sealed override void OnStateAdded(State state)
        {
            base.OnStateAdded(state);
            AddTransitionState(state);
        }

        protected sealed override void OnStateRemoved(State state)
        {
            base.OnStateRemoved(state); 
            RemoveTransitionState(state);
        }

        private void AddTransitionState(State state)
        {
            if (IsTransitionState(state as MovementState)) {
                defaultState?.AddTransitionState(state);
            }
        }

        private void RemoveTransitionState(State state)
        {
            defaultState?.RemoveTransitionState(state);
        }


        private void AddAllTransitionStates()
        {
            if (stateMachine == null) return;
            foreach (State state in stateMachine.GetStates()) AddTransitionState(state);
        }

        private void RemoveAllTransitionStates()
        {
            if (stateMachine == null) return;
            foreach (State state in stateMachine.GetStates()) RemoveTransitionState(state);
        }

        protected sealed override UnityAction<State> GetOnStateRemovedAction() { return null; }
        protected sealed override UnityAction<State> GetOnStateAddedAction() { return null; }

        public abstract bool IsTransitionState(MovementState state);
        protected abstract State GetDefaultState(StateMachine controller);
    }
}
