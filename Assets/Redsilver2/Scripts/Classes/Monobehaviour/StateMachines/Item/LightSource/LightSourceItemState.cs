using RedSilver2.Framework.Animations;
using RedSilver2.Framework.StateMachines.States;
using UnityEngine;


namespace RedSilver2.Framework.StateMachines {
    [RequireComponent(typeof(LightSourceItemStateMachine))]
    public abstract class LightSourceItemState : EquippableItemState {
        private LightSourceItemStateType type;
       [SerializeReference, HideInInspector] private LightSourceItemStateMachine stateMachine;

        public LightSourceItemStateMachine StateMachine => stateMachine;
        public LightSourceItemStateType Type => type;


        protected LightSourceItemState(LightSourceItemStateMachine stateMachine) : base(stateMachine)
        {
            this.stateMachine = stateMachine;

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
