using RedSilver2.Framework.Animations;
using RedSilver2.Framework.StateMachines.States;
using UnityEngine;

namespace RedSilver2.Framework.StateMachines
{
    public abstract class EquippableItemState : UpdatableState {

        [Space]
        [SerializeField] private float defaultCooldown;

        [Space]
        [SerializeField] private RuntimeAnimatorController animatorController;

        [Space]
        [SerializeField] private AnimationData animationData;

        private float cooldown;
        public float Cooldown => cooldown;

#if UNITY_EDITOR
        protected override void OnValidate()
        {
            animationData?.Validate(animatorController);
        }
#endif

        protected override void Awake()
        {
            base.Awake();
            cooldown = defaultCooldown;
        }

        protected sealed override void OnDisabled(UpdatableStateMachine stateMachine)
        {
            base.OnDisabled(stateMachine);
            OnDisabled(stateMachine as EquippableItemStateMachine);
        }

        protected sealed override void OnEnabled(UpdatableStateMachine stateMachine)
        {

            base.OnEnabled(stateMachine);
            OnEnabled(stateMachine as EquippableItemStateMachine);
        }

        protected sealed override void OnEntered(UpdatableStateMachine stateMachine) {
            base.OnEntered(stateMachine);
            OnEntered(stateMachine as EquippableItemStateMachine);
        }

        protected sealed override void OnExited(UpdatableStateMachine stateMachine) {
            base.OnExited(stateMachine);
            OnExited(stateMachine as EquippableItemStateMachine);
        }

        public sealed override bool CanTransition(UpdatableStateMachine stateMachine)
        {
            return base.CanTransition(stateMachine) && CanTransition(stateMachine as EquippableItemStateMachine); 
        }

        public virtual bool CanTransition(EquippableItemStateMachine stateMachine) {
            if (stateMachine == null || !stateMachine.IsCooldownOver()) return false;
            return true;
        }


        protected virtual void OnDisabled(EquippableItemStateMachine stateMachine) { }

        protected virtual void OnEnabled(EquippableItemStateMachine stateMachine) { }

        protected virtual void OnEntered(EquippableItemStateMachine stateMachine) {
            if (stateMachine != null) stateMachine.Animator?.PlayAnimation(animationData);
        }

        protected virtual void OnExited(EquippableItemStateMachine stateMachine) {  }


        protected sealed override bool CanAddTransitionState(UpdatableState state)
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
