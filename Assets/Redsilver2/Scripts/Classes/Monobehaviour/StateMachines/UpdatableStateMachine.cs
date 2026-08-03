using RedSilver2.Framework.StateMachines.States;
using UnityEngine.Events;

namespace RedSilver2.Framework.StateMachines
{
    public abstract class UpdatableStateMachine : StateMachine
    {
        private bool doesCurrentStateExist;
        private UpdatableState currentState;

        private UnityEvent onUpdate;
        private UnityEvent onLateUpdate;

        private UnityEvent<UpdatableState> onStateAdded, onStateRemoved;
        private UnityEvent<UpdatableState> onStateEntered, onStateExited;

        protected override void Awake()
        {
            base.Awake();
            onUpdate = new UnityEvent();
            onLateUpdate = new UnityEvent();

            onStateAdded = new UnityEvent<UpdatableState>();
            onStateRemoved = new UnityEvent<UpdatableState>();

            onStateEntered = new UnityEvent<UpdatableState>();
            onStateExited = new UnityEvent<UpdatableState>();

            doesCurrentStateExist = false; 

            AddOnUpdateListener(OnUpdate);
            AddOnLateUpdateListener(OnLateUpdate);
        }

        private void Update() { onUpdate?.Invoke(); }
        private void LateUpdate() { onLateUpdate?.Invoke(); }

        protected virtual void OnUpdate() { 
            if(!doesCurrentStateExist) {
                foreach(State state in States) {
                    if (state == null || !state.CanTransition()) continue;
                    ChangeState(state);
                    break;
                }
            }
        }
        protected virtual void OnLateUpdate() { }


        protected sealed override void OnStateEntered(State state)
        {
            base.OnStateEntered(state);
            OnStateEntered(state as UpdatableState);
        }
        protected virtual void OnStateEntered(UpdatableState state)
        {
            currentState = state;
            doesCurrentStateExist = currentState != null ? true : false;
            onStateEntered?.Invoke(state);
        }

        protected sealed override void OnStateExited(State state)
        {
            base.OnStateExited(state);
            OnStateExited(state as UpdatableState);
        }
        protected virtual void OnStateExited(UpdatableState state)
        {
            currentState = null;
            onStateEntered?.Invoke(state);
        }

        protected sealed override void OnStateAdded(State state)
        {
            OnStateAdded(state as UpdatableState);
        }
        protected virtual void OnStateAdded(UpdatableState state)
        {
            onStateRemoved?.Invoke(state);
        }

        protected sealed override void OnStateRemoved(State state)
        {
            OnStateAdded(state as UpdatableState);
        }
        protected virtual void OnStateRemoved(UpdatableState state)
        {
            onStateRemoved?.Invoke(state);
        }

        public void AddOnUpdateListener(UnityAction action)
        {
            if (action != null) onUpdate?.AddListener(action);
        }
        public void RemoveOnUpdateListener(UnityAction action)
        {
            if (action != null) onUpdate?.RemoveListener(action);
        }

        public void AddOnLateUpdateListener(UnityAction action)
        {
            if (action != null) onLateUpdate?.AddListener(action);
        }
        public void RemoveOnLateUpdateListener(UnityAction action)
        {
            if (action != null) onLateUpdate?.RemoveListener(action);
        }

        public void AddOnStateAddedListener(UnityAction<UpdatableState> action)
        {
            if (action != null) onStateAdded?.AddListener(action);
        }
        public void RemoveOnStateAddedListener(UnityAction<UpdatableState> action)
        {
            if (action != null) onStateAdded?.RemoveListener(action);
        }

        public void AddOnStateRemovedListener(UnityAction<UpdatableState> action)
        {
            if (action != null) onStateRemoved?.AddListener(action);
        }
        public void RemoveOnStateRemovedListener(UnityAction<UpdatableState> action)
        {
            if (action != null) onStateRemoved?.RemoveListener(action);
        }

        public void AddOnStateEnteredListener(UnityAction<UpdatableState> action)
        {
            if (action != null) onStateEntered?.AddListener(action);
        }
        public void RemoveOnStateEnteredListener(UnityAction<UpdatableState> action)
        {
            if (action != null) onStateEntered?.RemoveListener(action);
        }

        public void AddOnStateExitedListener(UnityAction<UpdatableState> action)
        {
            if (action != null) onStateExited?.AddListener(action);
        }
        public void RemoveOnStateExitedListener(UnityAction<UpdatableState> action)
        {
            if (action != null) onStateExited?.RemoveListener(action);
        }

        protected sealed override bool CanAddState(State state)
        {
            return base.CanAddState(state) && CanAddState(state as UpdatableState);
        }

        protected virtual bool CanAddState(UpdatableState state)
        {
            return state != null ? true : false;
        }
    }
}
