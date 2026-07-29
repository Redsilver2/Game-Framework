using System.Collections;
using UnityEngine.Events;

namespace RedSilver2.Framework.StateMachines.States
{
    public abstract class UpdatableState : State
    {
        private UnityEvent onUpdate;
        private IEnumerator stateUpdate;

        protected override void Awake() {
            base.Awake();
            onUpdate = new UnityEvent();

            AddOnEnteredListener(OnEntered);
            AddOnExitedListener(OnExited);
        }

        protected virtual void OnEntered() {
            StartStateUpdate();
        }

        protected virtual void OnExited() {
            StopStateUpdate();
        }

        public void AddOnUpdatedListener(UnityAction action)
        {
            if (action != null) onUpdate?.AddListener(action);
        }
        public void RemoevOnUpdatedListener(UnityAction action)
        {
            if (action != null) onUpdate?.RemoveListener(action);
        }

        private void StartStateUpdate()
        {
            StopStateUpdate();
            stateUpdate = StateUpdate();
            StartCoroutine(stateUpdate);
        }

        private void StopStateUpdate()
        {
            if(stateUpdate != null) StopCoroutine(stateUpdate);
            stateUpdate = null; 
        }

        private IEnumerator StateUpdate() {
            while (true) {
                onUpdate?.Invoke();
                yield return null;
            }
        }
    }
}
