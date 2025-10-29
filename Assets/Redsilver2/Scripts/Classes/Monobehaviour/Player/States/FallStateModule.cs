using UnityEngine;

namespace RedSilver2.Framework.Player
{
    public sealed class FallStateModule : UpdateablePlayerStateModule
    {
        [Space]
        [SerializeField] private bool canDisableMovementInput;

#if UNITY_EDITOR
        private void OnValidate()
        {
            GradualGravityModule.CreateInstance(transform);
            GroundCheckModule.CreateInstance(transform);
        }
#endif

        protected sealed override void Setup(PlayerStateMachine.PlayerState state)
        {
            base.Setup(state);
        }

        protected override PlayerStateMachine.PlayerState GetState(PlayerStateMachine owner)
        {
            PlayerStateMachine.FallState state;
            if (owner == null) return null;

            state = owner.GetState(PlayerStateMachine.FallState.STATE_NAME) as PlayerStateMachine.FallState;
            if (state != null) return state;

            return new PlayerStateMachine.FallState(owner, canDisableMovementInput);
        }
    }
}