using UnityEngine;
using UnityEngine.Events;

namespace RedSilver2.Framework.Interactions {
    public abstract class Door : InteractionModule {

        [Space]
        [SerializeField] private DoorStateType state;

        private bool isOpen;
        private bool isLocked;

        private UnityEvent<DoorStateType> onStateChanged;

        public bool IsOpen => isOpen;
        public bool IsLocked => isLocked;

        public DoorStateType State       => state;

        protected override void Awake()
        {
            base.Awake();
            onStateChanged = new UnityEvent<DoorStateType>();

            SetInteractionType(InteractionType.Door);
            AddOnStateChangedListener(OnStateChanged);
        }

        protected virtual void Start() {
            onStateChanged?.Invoke(state);
        }

        private void SetDoorState(DoorStateType state) {
            if (this.state != state) {
                this.state = state;
                onStateChanged?.Invoke(state);
            }
        }

        public  void Open() {
            if (state == DoorStateType.Close) { OnOpen(); }
        }

        public void Close() {
            if (state == DoorStateType.Open) { OnClose(); }
        }

        public void Lock() {
            if (state != DoorStateType.Locked) { SetDoorState(DoorStateType.Locked); }
        }
        public void Unlock() {
            if (state == DoorStateType.Locked) { SetDoorState(DoorStateType.Unlocked); }
        }

        protected virtual void OnOpen() {
            SetDoorState(DoorStateType.Open);
        }

        protected virtual void OnClose() {
            SetDoorState(DoorStateType.Close);
        }

        public void AddOnStateChangedListener(UnityAction<DoorStateType> action)
        {
            if (action != null) onStateChanged?.AddListener(action);
        }
        public void RemoveOnStateChangedListener(UnityAction<DoorStateType> action)
        {
            if (action != null) onStateChanged?.RemoveListener(action);
        }
        protected virtual void OnStateChanged(DoorStateType state) {
            if (state == DoorStateType.Close) {
                isOpen = false;
                isLocked = false;
            }
            else if (state == DoorStateType.Open) {
                isOpen = true;
                isLocked = false;
            }
            else if (state == DoorStateType.Locked) {
                isLocked = true;
            }
            else if (state == DoorStateType.Unlocked) {
                isLocked = false;
                SetDoorState(isOpen ? DoorStateType.Open : DoorStateType.Close);
            }
        }

    }
}
