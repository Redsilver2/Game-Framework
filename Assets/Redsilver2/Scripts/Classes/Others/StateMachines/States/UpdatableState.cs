using UnityEngine;
using UnityEngine.Events;

namespace RedSilver2.Framework.StateMachines.States
{
    [System.Serializable]
    public abstract class UpdatableState : State
    {
        private UnityEvent onUpdate;
        private UnityEvent onLateUpdate;
        private UpdatableStateMachine updatableStateMachine;


        protected UpdatableState() {         
            onUpdate     = new UnityEvent();
            onLateUpdate = new UnityEvent();

            AddOnUpdateListener(OnUpdate);
            AddOnLateUpdateListener(OnLateUpdate);
        }

        protected void SetStateMachine(UpdatableStateMachine stateMachine) {
            this.updatableStateMachine = stateMachine;   
            SetStateMachine(stateMachine as StateMachine);
        }

        protected override void OnEntered() {
            base.OnEntered();
            updatableStateMachine?.AddOnUpdateListener(InvokeOnUpdateEvent);
            updatableStateMachine?.AddOnLateUpdateListener(InvokeOnLateUpdateEvent);
        }

        protected override void OnExited() {
            base.OnExited();
            updatableStateMachine?.RemoveOnUpdateListener(InvokeOnUpdateEvent);
            updatableStateMachine?.RemoveOnLateUpdateListener(InvokeOnLateUpdateEvent);
        }

        private void InvokeOnUpdateEvent()     { onUpdate?.Invoke();     }
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

    }
}
