using RedSilver2.Framework.Interactions;
using RedSilver2.Framework.StateMachines.States;
using System;
using UnityEngine;
using UnityEngine.Events;

namespace RedSilver2.Framework.StateMachines
{
    public class DoorStateMachine : StateMachine
    {
        [SerializeField] private Transform handle;

        private bool isOpen;
        private UnityEvent<DoorState> onStateEntered, onStateExited;
        private UnityEvent<DoorState> onStateAdded, onStateRemoved;

        public bool      IsOpen => isOpen;
        public Transform Handle => handle;

#if UNITY_EDITOR

        protected override State GetInspectorState(int stateIndex)
        {
            return null;
        }

        protected override Array GetInspectorValues()
        {
            return null;
        }
#endif


        protected override void Awake() {
            base.Awake();

            onStateAdded   = new UnityEvent<DoorState>();
            onStateRemoved = new UnityEvent<DoorState>();

            onStateEntered = new UnityEvent<DoorState>();
            onStateExited  = new UnityEvent<DoorState>();
        }

        public virtual void Open()  {
            if (!isOpen) {
                ChangeState(DoorStateType.Open);
                isOpen = true;
            }
        }
        public void Close()
        {
            if (isOpen) {
                ChangeState(DoorStateType.Close);
                isOpen = false;
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
            onStateAdded?.Invoke(state);
        }
        private void OnStateRemoved(DoorState state) {
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

        protected void ChangeState(DoorStateType type)
        {
            ChangeState(GetState(type));
        }

        public DoorState GetState(DoorStateType type) {
            return GetState(type.ToString()) as DoorState;
        }
    }

}
