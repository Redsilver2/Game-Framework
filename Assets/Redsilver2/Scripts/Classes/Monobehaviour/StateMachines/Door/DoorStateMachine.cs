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

        private UnityEvent<DoorState> onStateEntered, onStateExited;
        private UnityEvent<DoorState> onStateAdded  , onStateRemoved;

        protected sealed override void Awake()
        {
            base.Awake();
            states = new List<DoorState>();

            onStateAdded   = new UnityEvent<DoorState>();
            onStateRemoved = new UnityEvent<DoorState>();

            onStateEntered = new UnityEvent<DoorState>();
            onStateExited  = new UnityEvent<DoorState>();

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
            onStateEntered?.Invoke(state);
        }
        private void OnDoorStateExited(DoorState state) {
            onStateExited?.Invoke(state);
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

            onStateAdded?.Invoke(state);
        }
        private void OnStateRemoved(DoorState state) {
            if (states == null || state == null || !states.Contains(state)) return;
            states?.Remove(state);

            onStateRemoved?.Invoke(state);
        }

        public void AddOnStateAddedListener(UnityAction<DoorState> action){
            if (action != null) onStateAdded?.AddListener(action);
        }
        public void RemoveOnStateAddedListener(UnityAction<DoorState> action) {
            if (action != null) onStateAdded?.RemoveListener(action);
        }

        public void AddOnStateExitedListener(UnityAction<DoorState> action)
        {
            if (action != null) onStateExited?.AddListener(action);
        }
        public void RemoveOnStateExitedListener(UnityAction<DoorState> action)
        {
            if (action != null) onStateExited?.RemoveListener(action);
        }

        public void AddOnStateEnteredListener(UnityAction<DoorState> action)
        {
            if (action != null) onStateEntered?.AddListener(action);
        }
        public void RemoveOnStateEnteredListener(UnityAction<DoorState> action)
        {
            if (action != null) onStateEntered?.RemoveListener(action);
        }

        public void AddOnStateRemovedListener(UnityAction<DoorState> action) {
            if (action != null) onStateRemoved?.AddListener(action);
        }
        public void RemoveOnStateRemovedListener(UnityAction<DoorState> action) {
            if (action != null) onStateRemoved?.RemoveListener(action);
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
