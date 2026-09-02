using RedSilver2.Framework.Animations;
using RedSilver2.Framework.Items;
using UnityEngine;

namespace RedSilver2.Framework.StateMachines
{
    public class DrinkableItemState : ConsumableItemState {
        private DrinkableItemStateType type;
        public DrinkableItemStateType Type => type;

        public DrinkableItemState(DrinkableItemStateMachine stateMachine) : base(stateMachine) {
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
    }
}
