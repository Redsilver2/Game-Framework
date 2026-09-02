using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace RedSilver2.Framework.StateMachines.States
{
    [System.Serializable]
    public abstract class UpdatableDoorState : DoorState
    {
        [SerializeField] private float defaultDuration;

        [Space]
        [SerializeField] private Vector3 desiredPosition;
        [SerializeField] private Vector3 desiredRotation;

        private float duration;

        private Vector3 currentPosition;
        private Vector3 currentRotation;
        private IEnumerator doorUpdate;



        private UnityEvent onUpdateStarted, onUpdateCompleted;
        private UnityEvent<float> onProgressionUpdate;

        protected UpdatableDoorState(DoorStateMachine stateMachine) : base(stateMachine)
        {
            onUpdateStarted     = new UnityEvent();
            onUpdateCompleted   = new UnityEvent();
            onProgressionUpdate = new UnityEvent<float>();

            AddOnUpdateStartedListener(OnUpdateStarted);
            AddOnUpdateCompletedListener(OnUpdateCompleted);
            AddOnProgressionUpdateListener(OnProgressionUpdate);
        }

        protected virtual void OnUpdateStarted() {
            Transform handle = DoorStateMachine != null ? DoorStateMachine.Handle : null;
            currentPosition = handle != null ? handle.localPosition : Vector3.zero;
            currentRotation = handle != null ? handle.localEulerAngles : Vector3.zero;
        }

        protected virtual void OnProgressionUpdate(float progress) {
            Transform handle = DoorStateMachine != null ? DoorStateMachine.Handle : null;

            if (handle != null) {
                handle.localPosition = Vector3.Lerp(currentPosition, desiredPosition, progress);
                handle.localRotation = Quaternion.Slerp(Quaternion.Euler(currentRotation), Quaternion.Euler(desiredRotation), progress);
            }
        }

        protected virtual void OnUpdateCompleted() {
            Transform handle = DoorStateMachine != null ? DoorStateMachine.Handle : null;

            if (handle != null) {
                handle.localPosition = desiredPosition;
                handle.localRotation = Quaternion.Euler(desiredRotation);
            }
        }

        protected override void OnEntered() {
            base.OnEntered();

        
        }

        protected override void OnExited()  { base.OnExited(); }

        public void SetDesiredRotation(Vector3 desiredRotation) {
            this.desiredRotation = desiredRotation;
        }

        public void SetDesiredPosition(Vector3 desiredPosition) {
            this.desiredPosition = desiredPosition;
        }


        private IEnumerator UpdateDoor() {
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