using RedSilver2.Framework.Items;
using RedSilver2.Framework.StateMachines.States;
using System;
using UnityEngine.Events;

namespace RedSilver2.Framework.StateMachines
{

    public class DrinkableItemStateMachine : ConsumableItemStateMachine {


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
            return GetState(type.ToString()) as DrinkableItemState;
        }

    }
}
