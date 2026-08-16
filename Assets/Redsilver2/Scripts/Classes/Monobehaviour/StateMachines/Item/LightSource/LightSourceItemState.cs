using RedSilver2.Framework.Animations;
using RedSilver2.Framework.StateMachines.States;
using UnityEngine;


namespace RedSilver2.Framework.StateMachines {
    [RequireComponent(typeof(LightSourceItemStateMachine))]
    public abstract class LightSourceItemState : EquippableItemState {
        private LightSourceItemStateType type;
        protected readonly LightSourceItemStateMachine StateMachine;

        public LightSourceItemStateType Type => type;


        protected LightSourceItemState() : base()
        {
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

        protected abstract void SetLightSourceStateType(ref LightSourceItemStateType type);
    }
}
