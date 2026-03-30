using UnityEngine;
using UnityEngine.Events;

namespace RedSilver2.Framework.StateMachines.States
{
    public abstract class UpdateableState : State
    {
        private readonly UnityEvent onUpdate    ;
        private readonly UnityEvent onLateUpdate;

        protected UpdateableState(StateMachine owner) : base(owner) {

            onUpdate     = new UnityEvent();
            onLateUpdate = new UnityEvent();

            AddOnStateExitedListener(() => {
                owner?.RemoveOnUpdateListener(Update);
                owner?.RemoveOnLateUpdateListener(LateUpdate);
            });

            AddOnStateEnteredListener(() => {
                owner?.AddOnUpdateListener(Update);
                owner?.AddOnLateUpdateListener(LateUpdate);
            });

            AddOnUpdateListener(UpdateStateTransition);
        }

        private void Update() {
            onUpdate?.Invoke();
        }

        private void LateUpdate() {
            onLateUpdate?.Invoke();
        }

        public void AddOnUpdateListener(UnityAction action) {
            if (action != null) onUpdate?.AddListener(action);
        }

        public void RemoveOnUpdateListener(UnityAction action) {
            if (action != null) onUpdate?.RemoveListener(action);
        }

        public void AddOnLateUpdateListener(UnityAction action) {
            if (action != null) onLateUpdate?.AddListener(action);
        }

        public void RemoveOnLateUpdateListener(UnityAction action) {
            if (action != null) onLateUpdate?.RemoveListener(action);
        }

        protected override void RemoveAllListenersFromOwner(StateMachine owner)
        {
            base.RemoveAllListenersFromOwner(owner);
            owner?.RemoveOnUpdateListener(Update);
            owner?.RemoveOnLateUpdateListener(LateUpdate);
        }

    }
}
