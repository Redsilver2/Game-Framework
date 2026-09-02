using RedSilver2.Framework.Items;
using RedSilver2.Framework.StateMachines.States;
using UnityEngine.Events;

namespace RedSilver2.Framework.StateMachines
{

    public class DrinkableItemStateMachine : ConsumableItemStateMachine {
        protected sealed override bool CanAddState(ConsumableItemState state) {
            return base.CanAddState(state) && CanAddState(state as DrinkableItemState);
        }

        protected virtual bool CanAddState(DrinkableItemState state) {
            return state != null ? true : false;
        }

        public void ChangeState(DrinkableItemState state) {
            ChangeState(state as State);
        }
        public void ChangeState(DrinkableItemStateType type) {
            ChangeState(GetState(type));
        }

        public DrinkableItemState GetState(DrinkableItemStateType type)
        {
            foreach(State state in States) {
                DrinkableItemState _state = state as DrinkableItemState;
                if (_state == null || _state.Type != type) continue;
                return _state;
            }

            return null;
        }
    }
}
