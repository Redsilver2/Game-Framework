using RedSilver2.Framework.Items;
using UnityEngine;

namespace RedSilver2.Framework.StateMachines
{
    public class DrinkableItemState : ConsumableItemState {
        private DrinkableItemStateType type;
        public DrinkableItemStateType Type => type;

        protected override void Awake()
        {
            base.Awake();
            type = DrinkableItemStateType.Drink;
            SetStateName(type.ToString());
        }

        protected sealed override bool CanAddTransitionState(ConsumableItemState state)
        {
            return base.CanAddTransitionState(state) && CanAddTransitionState(state as DrinkableItemState);
        }

        protected virtual bool CanAddTransitionState(DrinkableItemState state) {
            if(state == null || state.Type == type) return false;
            return true;
        }

        protected sealed override void OnDisabled(ConsumableItemStateMachine stateMachine)
        {
            base.OnDisabled(stateMachine);
            OnDisabled(stateMachine as DrinkableItemStateMachine);
        }

        protected sealed override void OnEnabled(ConsumableItemStateMachine stateMachine)
        {
            base.OnEnabled(stateMachine);
            OnEnabled(stateMachine as DrinkableItemStateMachine);
        }

        protected sealed override void OnEntered(ConsumableItemStateMachine stateMachine)
        {
            base.OnEntered(stateMachine);
            OnEntered(stateMachine as DrinkableItemStateMachine);
        }

        protected sealed override void OnExited(ConsumableItemStateMachine stateMachine) {
            base.OnExited(stateMachine);
            OnExited(stateMachine as DrinkableItemStateMachine);
        }

        protected virtual void OnDisabled(DrinkableItemStateMachine stateMachine) { }

        protected virtual void OnEnabled(DrinkableItemStateMachine stateMachine) { }

        protected virtual void OnEntered(DrinkableItemStateMachine stateMachine) { }

        protected virtual void OnExited(DrinkableItemStateMachine stateMachine) { }

    }
}
