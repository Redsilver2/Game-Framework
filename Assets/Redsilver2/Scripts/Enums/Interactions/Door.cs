using RedSilver2.Framework.Interactions.Actions;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace RedSilver2.Framework.Interactions {
    public abstract class Door : InteractionModule {
        [Space]
        [SerializeField] private Transform anchorPoint;
        [SerializeField] private float doorSpeed;

        private bool isOpen;
        private bool isLocked;

        private Vector3 originalTarget;

        private IEnumerator doorUpdateCoroutine;
        private UnityEvent  onOpen, onClose;

        private UnityEvent<bool, float, Transform>     onProgressionUpdate;
        private UnityEvent<bool, Transform> onUpdateStarted, onUpdateEnded;


        public bool  IsOpen => isOpen;
        public bool  IsLocked => isLocked;
        public float DoorSpeed => doorSpeed;
  
        public Vector3 OriginalTarget => originalTarget;
        public Transform AnchorPoint => anchorPoint;

        protected override void Awake()
        {
            base.Awake();
            SetInteractionType(InteractionType.Door);

            onOpen  = new UnityEvent();
            onClose = new UnityEvent();

            onUpdateStarted     = new UnityEvent<bool, Transform>();
            onUpdateEnded       = new UnityEvent<bool, Transform>();
            onProgressionUpdate = new UnityEvent<bool, float, Transform>();

            AddOnOpenListener(OnOpen);
            AddOnCloseListener(OnClose);

            AddOnUpdateStartedListener(OnUpdateStarted);
            AddOnUpdateEndedListener(OnUpdateEnded);
            
            AddOnProgressionUpdateListener(OnProgressionUpdate);


            CloseDoorAction action = new CloseDoorAction(this, new PressInteraction("Close Door"));
            action?.Enable();
            AddInteractionAction(action);
        }

        public void SetAnchorPoint(Transform anchorPoint)     { this.anchorPoint = anchorPoint; }
        public void SetOriginalTarget(Vector3 originalTarget) { this.originalTarget = originalTarget; }

        public void SetOpenState(bool isOpen)   { this.isOpen = isOpen; }
        public void SetLockState(bool isLocked) { this.isLocked = isLocked; }

        public void Open() {
            if (isOpen || isLocked) return;
            onOpen?.Invoke();
        }

        public void Close() {
            if (!isOpen) return;
            onClose?.Invoke();  
        }

        private void OnOpen()
        {
            isOpen = true;
            StartDoorUpdate();
        }

        private void OnClose()
        {
            isOpen = false;
            StartDoorUpdate();
        }

        protected abstract void OnUpdateStarted(bool isOpen, Transform anchor);
        protected abstract void OnUpdateEnded(bool isOpen, Transform anchor);
        protected abstract void OnProgressionUpdate(bool isOpen, float progress, Transform anchor);


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

        public void AddOnUpdateStartedListener(UnityAction<bool, Transform> action)
        {
            if(action != null) onUpdateStarted?.AddListener(action);    
        }
        public void RemoveOnUpdateStartedListener(UnityAction<bool, Transform> action){
            if (action != null) onUpdateStarted?.RemoveListener(action);
        }

        public void AddOnUpdateEndedListener(UnityAction<bool, Transform> action)
        {
            if (action != null) onUpdateEnded?.AddListener(action); 
        }
        public void RemoveOnUpdateEndedListener(UnityAction<bool, Transform> action)
        {
            if (action != null) onUpdateEnded?.RemoveListener(action);
        }

        public void AddOnProgressionUpdateListener(UnityAction<bool, float, Transform> action)
        {
            if (action != null) onProgressionUpdate?.AddListener(action);
        }
        public void RemoveOnProgressionUpdateListener(UnityAction<bool, float, Transform> action)
        {
            if (action != null) onProgressionUpdate?.RemoveListener(action);
        }

        private void StopDoorUpdate()
        {
            if (doorUpdateCoroutine != null) StopCoroutine(doorUpdateCoroutine);
            doorUpdateCoroutine = null;
        }

        private void StartDoorUpdate() {
            StopDoorUpdate();
            doorUpdateCoroutine = DoorUpdate();
            StartCoroutine(doorUpdateCoroutine);
        }

        protected IEnumerator DoorUpdate() {
            float t = 0;
            onUpdateStarted?.Invoke(isOpen, anchorPoint);

            while (anchorPoint != null) {
                t += Time.deltaTime * DoorSpeed;
                onProgressionUpdate?.Invoke(isOpen, Mathf.Clamp01(t/1f), anchorPoint);

                if (t >= 1f || isLocked) break;
                yield return null;
            }

            onUpdateEnded?.Invoke(isOpen, anchorPoint);    
        }

    }
}
