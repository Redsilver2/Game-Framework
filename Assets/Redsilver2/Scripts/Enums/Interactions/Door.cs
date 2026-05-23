using RedSilver2.Framework.Interactions.Actions.Setups;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace RedSilver2.Framework.Interactions {

    [RequireComponent(typeof(CloseDoorSetup))]
    public abstract class Door : InteractionModule {
        [Space]
        [SerializeField] private Transform anchorPoint;
        [SerializeField] private float doorSpeed;

        private bool isOpen;
        private bool isLocked;
        private IEnumerator doorUpdateCoroutine;

        private UnityEvent onOpen, onClose;

        public bool  IsOpen => isOpen;
        public bool  IsLocked => isLocked;
        public float DoorSpeed => doorSpeed;

        public void SetOpenState(bool isOpen)   { this.isOpen = isOpen;      }
        public void SetLockState(bool isLocked) { this.isLocked = isLocked;  }

        protected override void Awake()
        {
            base.Awake();
            SetInteractionType(InteractionType.Door);


            onOpen  = new UnityEvent();
            onClose = new UnityEvent();

            AddOnOpenListener(() => {
                isOpen = true;
                StartDoorUpdate();
            });

            AddOnCloseListener(() =>
            {
                isOpen = false;
                StartDoorUpdate();
            });

            SetDefaultValue(anchorPoint);
        }

        public void Open() {
            if (isOpen || isLocked) return;
            onOpen?.Invoke();
        }

        public void Close() {
            if (!isOpen) return;
            onClose?.Invoke();  
        }

        public void AddOnOpenListener(UnityAction action)
        {
            if (action != null) onOpen?.AddListener(action);
        }
        public void RemoveOnOpenListener(UnityAction action)
        {
            if (action != null) onOpen?.RemoveListener(action);
        }

        public void AddOnCloseListener(UnityAction action)
        {
            if (action != null) onClose?.AddListener(action);
        }
        public void RemoveOnCloseListener(UnityAction action)
        {
            if (action != null) onClose?.RemoveListener(action);
        }

        private void StopDoorUpdate()
        {
            if (doorUpdateCoroutine != null) StopCoroutine(doorUpdateCoroutine);
            doorUpdateCoroutine = null;
        }

        private void StartDoorUpdate() {
            StopDoorUpdate();
            doorUpdateCoroutine = DoorUpdate(anchorPoint);
            StartCoroutine(doorUpdateCoroutine);
        }

        protected abstract void SetDefaultValue(Transform anchorPoint);
        protected abstract IEnumerator DoorUpdate(Transform anchor);

    }
}
