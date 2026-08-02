using RedSilver2.Framework.Interactions;
using RedSilver2.Framework.StateMachines.States;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace RedSilver2.Framework.StateMachines
{
    public sealed class DoorStateMachine : StateMachine
    {
        [SerializeField] private Transform handle;

        private bool isOpen;
        private bool isLocked;

        private List<DoorState> states;

        private UnityEvent onClose, onOpen;
        private UnityEvent onLock, onUnlock;

        public bool IsOpen => isOpen;
        public bool IsLocked => isLocked;
        public Transform Handle => handle;

        private UnityEvent<DoorState> onDoorStateEntered, onDoorStateExited;
        private UnityEvent<DoorState> onDoorStateAdded  , onDoorStateRemoved;

        protected sealed override void Awake()
        {
            base.Awake();
            states = new List<DoorState>();

            onDoorStateAdded   = new UnityEvent<DoorState>();
            onDoorStateRemoved = new UnityEvent<DoorState>();

            onDoorStateEntered = new UnityEvent<DoorState>();
            onDoorStateExited  = new UnityEvent<DoorState>();

            onClose  = new UnityEvent();
            onOpen   = new UnityEvent();

            onLock   = new UnityEvent();
            onUnlock = new UnityEvent();
        }

        public void Open() {
            OpenDoorState state = OpenDoorState.GetState(this);

            if (state != null) {
                if (state.CanTransition()) {
                    ChangeState(state);
                    isOpen = true;
                }
            }
        }

        public void Close() {
            CloseDoorState state = CloseDoorState.GetState(this);

            if (state != null) {
                if (state.CanTransition()) {
                    ChangeState(state);
                    isOpen = false;
                }
            }
        }

        public void SetLockState(bool isLocked) {
            if(this.isLocked != isLocked) {
                this.isLocked = isLocked;
               
                if(isLocked) onLock?.Invoke();
                else         onUnlock?.Invoke();
            }
        }


        protected override bool CanAddState(State state){
            return base.CanAddState(state) && CanAddState(state as DoorState);
        }
        private bool CanAddState(DoorState state) {
            return state != null ? true : false;
        }

        protected sealed override void OnStateEntered(State state)
        {
            base.OnStateEntered(state);
            OnDoorStateEntered(state as DoorState);
        }
        protected sealed override void OnStateExited(State state)
        {
            base.OnStateExited(state);
            OnDoorStateExited(state as DoorState);
        }

        private void OnDoorStateEntered(DoorState state) {
            onDoorStateEntered?.Invoke(state);
        }
        private void OnDoorStateExited(DoorState state) {
            onDoorStateExited?.Invoke(state);
        }

        protected sealed override void OnStateAdded(State state) {
            OnStateAdded(state as DoorState);
        }
        protected sealed override void OnStateRemoved(State state) {
            OnStateRemoved(state as DoorState);
        }

        private void OnStateAdded(DoorState state) {
            if(states == null || state == null || states.Contains(state)) return;
            states?.Add(state);

            onDoorStateAdded?.Invoke(state);
        }
        private void OnStateRemoved(DoorState state) {
            if (states == null || state == null || !states.Contains(state)) return;
            states?.Remove(state);

            onDoorStateRemoved?.Invoke(state);
        }

        public void AddOnDoorStateAddedListener(UnityAction<DoorState> action){
            if (action != null) onDoorStateAdded?.AddListener(action);
        }
        public void RemoveOnDoorStateAddedListener(UnityAction<DoorState> action) {
            if (action != null) onDoorStateAdded?.RemoveListener(action);
        }

        public void AddOnDoorStateExitedListener(UnityAction<DoorState> action)
        {
            if (action != null) onDoorStateExited?.AddListener(action);
        }
        public void RemoveOnDoorStateExitedListener(UnityAction<DoorState> action)
        {
            if (action != null) onDoorStateExited?.RemoveListener(action);
        }

        public void AddOnDoorStateEnteredListener(UnityAction<DoorState> action)
        {
            if (action != null) onDoorStateEntered?.AddListener(action);
        }
        public void RemoveOnDoorStateEnteredListener(UnityAction<DoorState> action)
        {
            if (action != null) onDoorStateEntered?.RemoveListener(action);
        }

        public void AddOnDoorStateRemovedListener(UnityAction<DoorState> action) {
            if (action != null) onDoorStateRemoved?.AddListener(action);
        }
        public void RemoveOnDoorStateRemovedListener(UnityAction<DoorState> action) {
            if (action != null) onDoorStateRemoved?.RemoveListener(action);
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

        public void AddOnLockListener(UnityAction action)
        {
            if (action != null) onLock?.AddListener(action);
        }
        public void RemoveOnLockListener(UnityAction action)
        {
            if (action != null) onLock?.RemoveListener(action);
        }

        public void ChangeState(DoorStateType type)
        {
            ChangeState(GetState(type));
        }
        public DoorState GetState(DoorStateType type) {
            if(states == null) return null;

            for(int i = 0; i < states.Count; i++)
                if (states[i].Type == type) return states[i];

            return null;
        }
    }

}
