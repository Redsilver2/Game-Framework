namespace RedSilver2.Framework.Player
{
    public sealed class IdolStateModule : PlayerStateModule
    {
        protected override PlayerStateMachine.PlayerState GetState(PlayerStateMachine owner)
        {
            PlayerStateMachine.IdolState state;
            if (owner == null) return null;

            state = owner.GetState(PlayerStateMachine.IdolState.STATE_NAME) as PlayerStateMachine.IdolState;
            if (state != null) return state;

            return new PlayerStateMachine.IdolState(owner);
        }
    }
}