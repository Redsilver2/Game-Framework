using RedSilver2.Framework.Inputs.Settings;
using UnityEngine;

namespace RedSilver2.Framework.StateMachines
{
    public abstract class ConsumableItemState : EquippableItemState {
        [Space]
        [SerializeField][Range(0f, 1f)] private float consumption;

        [Space]
        [SerializeField] private float actionExecutionTime;

        [Space]
        [SerializeField] private PressInputSettings settings;

        protected sealed override void OnEnabled(EquippableItemStateMachine stateMachine) {
            base.OnEnabled(stateMachine);
            OnEnabled(stateMachine as ConsumableItemStateMachine);
        }

        protected sealed override void OnDisabled(EquippableItemStateMachine stateMachine) {
            base.OnDisabled(stateMachine);
            OnDisabled(stateMachine as ConsumableItemStateMachine);
        }

        protected sealed override void OnEntered(EquippableItemStateMachine stateMachine)
        {
            base.OnEntered(stateMachine);
            OnEntered(stateMachine as ConsumableItemStateMachine);
        }

        protected sealed override void OnExited(EquippableItemStateMachine stateMachine)
        {
            base.OnExited(stateMachine);
            OnExited(stateMachine as ConsumableItemStateMachine);
        }

        protected virtual void OnEnabled(ConsumableItemStateMachine stateMachine) { }
        protected virtual void OnDisabled(ConsumableItemStateMachine stateMachine) { }

        protected virtual void OnEntered(ConsumableItemStateMachine stateMachine) {
            stateMachine?.StartConsuming(actionExecutionTime, consumption);
        }

        protected virtual void OnExited(ConsumableItemStateMachine stateMachine) { }

        public sealed override bool CanTransition(EquippableItemStateMachine stateMachine) {
            return base.CanTransition(stateMachine) && CanTransition(stateMachine as ConsumableItemStateMachine);
        }

        public virtual bool CanTransition(ConsumableItemStateMachine stateMachine) {
            if (stateMachine == null || settings == null || stateMachine.ConsumptionValue <= 0f || !stateMachine.IsCooldownOver()) return false;
            settings?.Enable();
            return settings.GetValue();
        }

        protected sealed override bool CanAddTransitionState(EquippableItemState state) {
            return base.CanAddTransitionState(state) && CanAddTransitionState(state as ConsumableItemState);
        }

        protected virtual bool CanAddTransitionState(ConsumableItemState state)
        {
            return state != null ? true : false;
        }

        public void SetInputSettings(PressInputSettings settings) {
            this.settings = settings;
        }
    }
}
