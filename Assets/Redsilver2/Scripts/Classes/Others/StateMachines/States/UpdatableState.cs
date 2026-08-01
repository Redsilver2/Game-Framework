using RedSilver2.Framework.StateMachines.Controllers;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

namespace RedSilver2.Framework.StateMachines.States
{
    public abstract class UpdatableState : State
    {
        private UpdatableStateMachine stateMachine;
       
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

        protected override void OnEntered() {
            base.OnEntered();
            stateMachine?.AddOnUpdateListener(InvokeOnUpdateEvent);
            stateMachine?.AddOnLateUpdateListener(InvokeOnLateUpdateEvent);
        }

        protected override void OnExited()
        {
            base.OnExited();
            stateMachine?.RemoveOnUpdateListener(InvokeOnUpdateEvent);
            stateMachine?.RemoveOnLateUpdateListener(InvokeOnLateUpdateEvent);
        }

        private void InvokeOnUpdateEvent() { onUpdate?.Invoke(); }
        private void InvokeOnLateUpdateEvent() { onLateUpdate?.Invoke(); }

        protected virtual void OnUpdate() { UpdateStateTransitions(); }
        protected virtual void OnLateUpdate() { }


        protected override void SetStateMachine(StateMachine stateMachine)
        {
            base.SetStateMachine(stateMachine);
            this.stateMachine = stateMachine as UpdatableStateMachine;
        }

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
