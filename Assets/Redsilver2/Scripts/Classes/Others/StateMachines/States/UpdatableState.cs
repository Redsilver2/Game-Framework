using UnityEngine;
using UnityEngine.Events;

namespace RedSilver2.Framework.StateMachines.States
{
    [System.Serializable]
    public abstract class UpdatableState : State
    {
        [SerializeField, HideInInspector] private UnityEvent onUpdate;
        [SerializeField, HideInInspector] private UnityEvent onLateUpdate;
        [SerializeField, SerializeReference, HideInInspector] private UpdatableStateMachine updatableStateMachine;


        protected UpdatableState(UpdatableStateMachine stateMachine) : base(stateMachine) {
            this.updatableStateMachine = stateMachine;
        }

        protected override void OnEntered() {
            base.OnEntered();

            AddOnUpdateListener(OnUpdate);
            AddOnLateUpdateListener(OnLateUpdate);

            updatableStateMachine?.AddOnUpdateListener(InvokeOnUpdateEvent);
            updatableStateMachine?.AddOnLateUpdateListener(InvokeOnLateUpdateEvent);
        }

        protected override void OnExited() {
            base.OnExited();

           
            RemoveOnUpdateListener(OnUpdate);
            RemoveOnLateUpdateListener(OnLateUpdate);

            updatableStateMachine?.RemoveOnUpdateListener(InvokeOnUpdateEvent);
            updatableStateMachine?.RemoveOnLateUpdateListener(InvokeOnLateUpdateEvent);
        }

        private void InvokeOnUpdateEvent()     {
            onUpdate?.Invoke();    
        }
        private void InvokeOnLateUpdateEvent() { onLateUpdate?.Invoke(); }

        protected virtual void OnUpdate()     { UpdateStateTransitions(); }
        protected virtual void OnLateUpdate() {                           }

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

        public static UpdatableStateMachine GetStateMachine(UpdatableState state)
        {
            return state != null ? state.updatableStateMachine : null;
        }

        public static UpdatableStateMachine GetUpdatableStateMachine(UpdatableState state)
        {
            return state != null ? state.updatableStateMachine : null;
        }
    }
}
