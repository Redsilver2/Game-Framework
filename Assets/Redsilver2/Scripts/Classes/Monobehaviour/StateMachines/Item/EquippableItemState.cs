using RedSilver2.Framework.Animations;
using RedSilver2.Framework.StateMachines.States;
using UnityEngine;

namespace RedSilver2.Framework.StateMachines
{
    public abstract class EquippableItemState : UpdatableState {

        [Space]
        [SerializeField] private float defaultCooldown;

        private AnimationController controller;

        private float cooldown;
        public float Cooldown => cooldown;

        protected EquippableItemState(EquippableItemStateMachine stateMachine) : base(stateMachine) {
            cooldown = defaultCooldown;
        }


#if UNITY_EDITOR
        public override void Validate()  {
            defaultCooldown = Mathf.Clamp(defaultCooldown, 0f, float.MaxValue);
        }
#endif

        protected override void OnEntered()
        {
            base.OnExited();
            controller?.PlayDefaultData();
        }

        protected sealed override bool CanAddTransitionState(State state)
        {
            return base.CanAddTransitionState(state) && CanAddTransitionState(state as EquippableItemState);
        }

        protected virtual bool CanAddTransitionState(EquippableItemState state)
        {
            return state != null ? true : false;
        }

        public void SetCooldown(float cooldown) {
            this.cooldown = Mathf.Clamp(cooldown, 0f, cooldown);
        }
        public void ResetCooldown() { cooldown = defaultCooldown; }
    }
}
