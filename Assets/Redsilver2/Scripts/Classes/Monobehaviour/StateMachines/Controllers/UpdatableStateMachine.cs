using UnityEngine;
using UnityEngine.Events;

namespace RedSilver2.Framework.StateMachines.Controllers
{
    public abstract class UpdatableStateMachine : StateMachine
    {
        private UnityEvent onUpdate;
        private UnityEvent onLateUpdate;

        protected override void Awake()
        {
            base.Awake();
            onUpdate     = new UnityEvent();
            onLateUpdate = new UnityEvent();
        }

        private void Update() { onUpdate?.Invoke();  }
        private void LateUpdate() { onLateUpdate?.Invoke();  }

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
    }

}
