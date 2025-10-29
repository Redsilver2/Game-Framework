namespace RedSilver2.Framework.Player
{
    public sealed class WalkStateModule : UpdateablePlayerStateModule
    {
        protected override PlayerStateMachine.PlayerState GetState(PlayerStateMachine owner)
        {
            PlayerStateMachine.WalkState state;
            if (owner == null) return null;

            state = owner.GetState(PlayerStateMachine.WalkState.STATE_NAME) as PlayerStateMachine.WalkState;
            if (state != null) return state;

            return new PlayerStateMachine.WalkState(owner);
        }
    }
}
