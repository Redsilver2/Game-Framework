using RedSilver2.Framework.StateMachines.States;
using UnityEngine;


namespace RedSilver2.Framework.StateMachines {
    [RequireComponent(typeof(LightSourceItemStateMachine))]
    public abstract class LightSourceItemState : EquippableItemState {
        private LightSourceStateType type;
        public LightSourceStateType Type => type;


        protected override void Awake() {
            base.Awake();

            SetLightSourceStateType(ref type);
            SetStateName(type.ToString());
        }

        protected sealed override bool CanAddTransitionState(EquippableItemState state)
        {
            return base.CanAddTransitionState(state) && CanAddTransitionState(state as LightSourceItemState);
        }

        protected virtual bool CanAddTransitionState(LightSourceItemState state) {
            return state != null ? true : false;
        }

        protected sealed override void OnDisabled(EquippableItemStateMachine stateMachine)
        {
            base.OnDisabled(stateMachine);
            OnDisabled(stateMachine as LightSourceItemStateMachine);
        }

        protected sealed override void OnEnabled(EquippableItemStateMachine stateMachine)
        {
            base.OnEnabled(stateMachine);
            OnEnabled(stateMachine as LightSourceItemStateMachine);
        }

        protected sealed override void OnEntered(EquippableItemStateMachine stateMachine)
        {
            base.OnEntered(stateMachine);
            OnEntered(stateMachine as LightSourceItemStateMachine);
        }

        protected sealed override void OnExited(EquippableItemStateMachine stateMachine)
        {
            base.OnExited(stateMachine);
            OnExited(stateMachine as LightSourceItemStateMachine);
        }

        public sealed override bool CanTransition(EquippableItemStateMachine stateMachine) {
            return base.CanTransition(stateMachine) && CanTransition(stateMachine as LightSourceItemStateMachine);
        }

        public virtual bool CanTransition(LightSourceItemStateMachine stateMachine)
        {
            return stateMachine != null ? true : false;
        }

        protected virtual void OnDisabled(LightSourceItemStateMachine stateMachine) { }
        protected virtual void OnEnabled(LightSourceItemStateMachine stateMachine) { }

        protected virtual void OnEntered(LightSourceItemStateMachine stateMachine) { }
        protected virtual void OnExited(LightSourceItemStateMachine stateMachine) { }

        protected abstract void SetLightSourceStateType(ref LightSourceStateType type);
    }
}
