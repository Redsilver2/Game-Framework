using UnityEditor.Callbacks;
using UnityEngine;
using UnityEngine.Events;

namespace RedSilver2.Framework.Interactions {
    public abstract class Door : InteractionModule {

        [Space]
        [SerializeField] private DoorState state;

        private bool isOpen;
        private bool isLocked;

        private UnityEvent<DoorState> onStateChanged;

        public bool IsOpen => isOpen;
        public bool IsLocked => isLocked;

        public DoorState State       => state;

        protected override void Awake()
        {
            base.Awake();
            onStateChanged = new UnityEvent<DoorState>();

            SetInteractionType(InteractionType.Door);
            AddOnStateChangedListener(OnStateChanged);
        }

        protected virtual void Start() {
            onStateChanged?.Invoke(state);
        }

        private void SetDoorState(DoorState state) {
            if (this.state != state) {
                this.state = state;
                onStateChanged?.Invoke(state);
            }
        }

        public  void Open() {
            if (state == DoorState.Closed) { OnOpen(); }
        }

        public void Close() {
            if (state == DoorState.Opened) { OnClose(); }
        }

        public void Lock() {
            if (state != DoorState.Locked) { SetDoorState(DoorState.Locked); }
        }
        public void Unlock() {
            if (state == DoorState.Locked) { SetDoorState(DoorState.Unlocked); }
        }

        protected virtual void OnOpen() {
            SetDoorState(DoorState.Opened);
        }

        protected virtual void OnClose() {
            SetDoorState(DoorState.Closed);
        }

        public void AddOnStateChangedListener(UnityAction<DoorState> action)
        {
            if (action != null) onStateChanged?.AddListener(action);
        }
        public void RemoveOnStateChangedListener(UnityAction<DoorState> action)
        {
            if (action != null) onStateChanged?.RemoveListener(action);
        }
        protected virtual void OnStateChanged(DoorState state) {
            if (state == DoorState.Closed) {
                isOpen = false;
                isLocked = false;
            }
            else if (state == DoorState.Opened) {
                isOpen = true;
                isLocked = false;
            }
            else if (state == DoorState.Locked) {
                isLocked = true;
            }
            else if (state == DoorState.Unlocked) {
                isLocked = false;
                SetDoorState(isOpen ? DoorState.Opened : DoorState.Closed);
            }
        }

    }
}
