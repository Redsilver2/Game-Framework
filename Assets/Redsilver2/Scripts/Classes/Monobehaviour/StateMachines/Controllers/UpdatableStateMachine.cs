using RedSilver2.Framework.StateMachines.States;
using System.Collections.Generic;
using UnityEngine.Events;

namespace RedSilver2.Framework.StateMachines
{
    public abstract class UpdatableStateMachine : StateMachine
    {
        private UnityEvent onUpdate;
        private UnityEvent onLateUpdate;

        private UnityEvent<UpdatableState> onUpdatableStateAdded,   onUpdatableStateRemoved;
        private UnityEvent<UpdatableState> onUpdatableStateEntered, onUpdatableStateExited;

        protected override void Awake()
        {
            base.Awake();
            onUpdate     = new UnityEvent();
            onLateUpdate = new UnityEvent();

            onUpdatableStateAdded   = new UnityEvent<UpdatableState>();
            onUpdatableStateRemoved = new UnityEvent<UpdatableState>();

            onUpdatableStateEntered = new UnityEvent<UpdatableState>();
            onUpdatableStateExited  = new UnityEvent<UpdatableState>();

            AddOnUpdateListener(OnUpdate);
            AddOnLateUpdateListener(OnLateUpdate);
        }

        private void Update() { onUpdate?.Invoke();  }
        private void LateUpdate() { onLateUpdate?.Invoke();  }

        protected abstract void OnUpdate();
        protected abstract void OnLateUpdate(); 

        protected override void OnStateEntered(State state)
        {
            base.OnStateEntered(state);
            OnUpdatableStateEntered(state as UpdatableState);
        }
        protected virtual void OnUpdatableStateEntered(UpdatableState state)
        {
            onUpdatableStateEntered?.Invoke(state);
        }

        protected override void OnStateExited(State state)
        {
            base.OnStateExited(state);
            OnUpdatableStateExited(state as UpdatableState);
        }
        protected virtual void OnUpdatableStateExited(UpdatableState state)
        {
            onUpdatableStateEntered?.Invoke(state);
        }

        protected sealed override void OnStateAdded(State state) {
            OnUpdatableStateAdded(state as UpdatableState);
        }
        protected virtual void OnUpdatableStateAdded(UpdatableState state) {
            onUpdatableStateRemoved?.Invoke(state);   
        }

        protected sealed override void OnStateRemoved(State state) {
            OnUpdatableStateAdded(state as UpdatableState);
        }
        protected virtual void OnUpdatableStateRemoved(UpdatableState state) {
            onUpdatableStateRemoved?.Invoke(state);
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

        public void AddOnUpdatableStateAddedListener(UnityAction<UpdatableState> action)
        {
            if (action != null) onUpdatableStateAdded?.AddListener(action);
        }
        public void RemoveOnUpdatableStateAddedListener(UnityAction<UpdatableState> action)
        {
            if (action != null) onUpdatableStateAdded?.RemoveListener(action);
        }

        public void AddOnUpdatableStateRemovedListener(UnityAction<UpdatableState> action)
        {
            if (action != null) onUpdatableStateRemoved?.AddListener(action);
        }
        public void RemoveOnUpdatableStateRemovedListener(UnityAction<UpdatableState> action)
        {
            if (action != null) onUpdatableStateRemoved?.RemoveListener(action);
        }

        public void AddOnUpdatableStateEnteredListener(UnityAction<UpdatableState> action)
        {
            if (action != null) onUpdatableStateEntered?.AddListener(action);
        }
        public void RemoveOnUpdatableStateEnteredListener(UnityAction<UpdatableState> action)
        {
            if (action != null) onUpdatableStateEntered?.RemoveListener(action);
        }

        public void AddOnUpdatableStateExitedListener(UnityAction<UpdatableState> action)
        {
            if (action != null) onUpdatableStateExited?.AddListener(action);
        }
        public void RemoveOnUpdatableStateExitedListener(UnityAction<UpdatableState> action)
        {
            if (action != null) onUpdatableStateExited?.RemoveListener(action);
        }

        protected override bool CanAddState(State state) {
            return base.CanAddState(state) && state as UpdatableState != null;
        }
    }

}
