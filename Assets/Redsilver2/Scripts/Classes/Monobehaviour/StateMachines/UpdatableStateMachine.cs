using RedSilver2.Framework.StateMachines.States;
using UnityEngine;
using UnityEngine.Events;

namespace RedSilver2.Framework.StateMachines
{
    public abstract class UpdatableStateMachine : StateMachine
    {
        private bool doesCurrentStateExist;

        private UnityEvent onUpdate;
        private UnityEvent onLateUpdate;

        protected override void Awake()
        {
            base.Awake();
            onUpdate = new UnityEvent();
            onLateUpdate = new UnityEvent();

            doesCurrentStateExist = false; 

            AddOnUpdateListener(OnUpdate);
            AddOnLateUpdateListener(OnLateUpdate);
        }

        private void Update() { onUpdate?.Invoke(); }
        private void LateUpdate() { onLateUpdate?.Invoke(); }

        protected virtual void OnUpdate() {
            State[] states = ActifStates;

            if (!doesCurrentStateExist && states != null) {
                foreach (State state in ActifStates) {
                    if (state == null || !state.CanTransition()) continue;
                    ChangeState(state);
                    break;
                }
            }
        }
        protected virtual void OnLateUpdate() { }


        protected override void OnStateEntered(State state)
        {
            base.OnStateEntered(state);
            doesCurrentStateExist = state != null ? true : false;
        }

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


        protected sealed override bool CanAddState(State state)
        {
            return base.CanAddState(state) && CanAddState(state as UpdatableState);
        }

        protected virtual bool CanAddState(UpdatableState state)
        {
            return state != null ? true : false;
        }
    }
}
