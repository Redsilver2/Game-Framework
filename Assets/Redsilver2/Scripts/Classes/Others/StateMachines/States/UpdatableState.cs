using UnityEngine.Events;

namespace RedSilver2.Framework.StateMachines.States
{
    public abstract class UpdatableState : State
    {
        private UnityEvent onUpdate;
        private UnityEvent onLateUpdate;


        protected override void Awake() {
            base.Awake();
            onUpdate     = new UnityEvent();
            onLateUpdate = new UnityEvent();

            AddOnUpdateListener(OnUpdate);
            AddOnLateUpdateListener(OnLateUpdate);
        }

        protected sealed override bool CanAddTransitionState(State state) {
            return base.CanAddTransitionState(state) && CanAddTransitionState(state as UpdatableState);
        }

        protected virtual bool CanAddTransitionState(UpdatableState state) { 
            return state != null ? true : false;
        }

        protected sealed override void OnEntered(StateMachine stateMachine) {
            base.OnEntered(stateMachine);
            OnEntered(stateMachine as UpdatableStateMachine);
        }

        protected sealed override void OnExited(StateMachine stateMachine)
        {
            base.OnExited(stateMachine);
            OnExited(stateMachine as UpdatableStateMachine);
        }

        protected sealed override void OnDisabled(StateMachine stateMachine) {
            base.OnDisabled(stateMachine);
            OnDisabled(stateMachine as UpdatableStateMachine);
        }

        protected sealed override void OnEnabled(StateMachine stateMachine)  {
            OnEnabled(stateMachine as UpdatableStateMachine);
            base.OnEnabled(stateMachine);
        }

        protected virtual void OnEntered(UpdatableStateMachine stateMachine) {
            stateMachine?.AddOnUpdateListener(InvokeOnUpdateEvent);
            stateMachine?.AddOnLateUpdateListener(InvokeOnLateUpdateEvent);
        }
        protected virtual void OnExited(UpdatableStateMachine stateMachine) {
            stateMachine?.RemoveOnUpdateListener(InvokeOnUpdateEvent);
            stateMachine?.RemoveOnLateUpdateListener(InvokeOnLateUpdateEvent);
        }

        protected virtual void OnEnabled(UpdatableStateMachine stateMachine) { }
        protected virtual void OnDisabled(UpdatableStateMachine stateMachine) { }

        private void InvokeOnUpdateEvent() { onUpdate?.Invoke(); }
        private void InvokeOnLateUpdateEvent() { onLateUpdate?.Invoke(); }

        protected virtual void OnUpdate() { UpdateStateTransitions(); }
        protected virtual void OnLateUpdate() { }

        public sealed override bool CanTransition(StateMachine stateMachine)
        {
            return base.CanTransition(stateMachine) && CanTransition(stateMachine as UpdatableStateMachine);
        }

        public virtual bool CanTransition(UpdatableStateMachine stateMachine) { 
            return stateMachine != null ? true : false;
        }



        public virtual void OnUpdate(UpdatableStateMachine stateMachine) { }

        public void AddOnUpdateListener(UnityAction action) {
            if (action != null) onUpdate?.AddListener(action);
        }

        public void RemoveOnUpdateListener(UnityAction action) {
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

    }
}
