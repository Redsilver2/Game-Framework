using RedSilver2.Framework.StateMachines.States;
using UnityEngine;


namespace RedSilver2.Framework.StateMachines {
    [RequireComponent(typeof(LightSourceStateMachine))]
    public abstract class LightSourceState : EquippableItemState {

        private LightSourceStateType type;
        public LightSourceStateType Type => type;


        protected override void Awake() {
            base.Awake();

            SetLightSourceStateType(ref type);
            SetStateName(type.ToString());
        }

        protected sealed override bool CanAddTransitionState(EquippableItemState state)
        {
            return base.CanAddTransitionState(state) && CanAddTransitionState(state as LightSourceState);
        }

        protected virtual bool CanAddTransitionState(LightSourceState state) {
            return state != null ? true : false;
        }

        protected sealed override void OnDisabled(EquippableItemStateMachine stateMachine)
        {
            base.OnDisabled(stateMachine);
            OnDisabled(stateMachine as LightSourceStateMachine);
        }

        protected sealed override void OnEnabled(EquippableItemStateMachine stateMachine)
        {
            base.OnEnabled(stateMachine);
            OnEnabled(stateMachine as LightSourceStateMachine);
        }

        protected sealed override void OnEntered(EquippableItemStateMachine stateMachine)
        {
            base.OnEntered(stateMachine);
            OnEntered(stateMachine as LightSourceStateMachine);
        }

        protected sealed override void OnExited(EquippableItemStateMachine stateMachine)
        {
            base.OnExited(stateMachine);
            OnExited(stateMachine as LightSourceStateMachine);
        }

        public sealed override bool CanTransition(EquippableItemStateMachine stateMachine) {
            return base.CanTransition(stateMachine) && CanTransition(stateMachine as LightSourceStateMachine);
        }

        public virtual bool CanTransition(LightSourceStateMachine stateMachine)
        {
            return stateMachine != null ? true : false;
        }

        protected virtual void OnDisabled(LightSourceStateMachine stateMachine) { }
        protected virtual void OnEnabled(LightSourceStateMachine stateMachine) { }

        protected virtual void OnEntered(LightSourceStateMachine stateMachine) { }
        protected virtual void OnExited(LightSourceStateMachine stateMachine) { }

        protected abstract void SetLightSourceStateType(ref LightSourceStateType type);
    }
}
