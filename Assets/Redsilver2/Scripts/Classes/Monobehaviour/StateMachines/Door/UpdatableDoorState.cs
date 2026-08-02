using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace RedSilver2.Framework.StateMachines.States
{
    public abstract class UpdatableDoorState : DoorState
    {
        [Space]
        [SerializeField] private float defaultDuration;

        private float duration;
        private IEnumerator doorUpdate;

        private UnityEvent onUpdateStarted, onUpdateCompleted;
        private UnityEvent<float> onProgressionUpdate;

        protected override void Awake()
        {
            base.Awake();
            onUpdateStarted = new UnityEvent();

            onUpdateCompleted = new UnityEvent();
            onProgressionUpdate = new UnityEvent<float>();

            AddOnUpdateStartedListener(OnUpdateStarted);
            AddOnUpdateCompletedListener(OnUpdateCompleted);
            AddOnProgressionUpdateListener(OnProgressionUpdate);

   
        }

        protected virtual void OnUpdateStarted() { }
        protected virtual void OnProgressionUpdate(float progress) { }
        protected virtual void OnUpdateCompleted() { }

        protected override void OnEntered(DoorStateMachine stateMachine)
        {
            base.OnEntered(stateMachine);

            if (doorUpdate != null) StopCoroutine(doorUpdate);
            doorUpdate = null;

            doorUpdate = UpdateDoor(stateMachine);
            StartCoroutine(doorUpdate);
        }

        protected override void OnExited(DoorStateMachine stateMachine)
        {
            base.OnExited(stateMachine);

            if(doorUpdate != null) StopCoroutine(doorUpdate);
            doorUpdate = null;
        }

        private IEnumerator UpdateDoor(DoorStateMachine stateMachine) {
            float t = 0f;
            onUpdateStarted?.Invoke();

            while (t < defaultDuration) {
                float progress = Mathf.Clamp01(t / defaultDuration);
                onProgressionUpdate?.Invoke(progress);
                t += Time.deltaTime;
                yield return null;
            }

            onUpdateCompleted?.Invoke();
        }

        public void AddOnUpdateStartedListener(UnityAction action) {
            if (action != null) onUpdateStarted?.AddListener(action);
        }
        public void RemoveOnUpdateStartedListener(UnityAction action)
        {
            if (action != null) onUpdateStarted?.RemoveListener(action);
        }

        public void AddOnUpdateCompletedListener(UnityAction action)
        {
            if (action != null) onUpdateCompleted?.AddListener(action);
        }
        public void RemoveOnUpdateCompletedListener(UnityAction action)
        {
            if (action != null) onUpdateCompleted?.RemoveListener(action);
        }

        public void AddOnProgressionUpdateListener(UnityAction<float> action)
        {
            if (action != null) onProgressionUpdate?.AddListener(action);
        }
        public void RemoveOnProgressionUpdateListener(UnityAction<float> action)
        {
            if (action != null) onProgressionUpdate?.RemoveListener(action);
        }
    }
}