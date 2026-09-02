using RedSilver2.Framework.Animations;
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

        private readonly ConsumableItemStateMachine stateMachine;

        protected ConsumableItemState(ConsumableItemStateMachine stateMachine) : base(stateMachine) {

        }

        protected override void OnEntered() {
            base.OnEntered();
            stateMachine?.StartConsuming(actionExecutionTime, consumption);
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
