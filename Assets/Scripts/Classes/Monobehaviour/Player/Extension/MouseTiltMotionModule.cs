namespace RedSilver2.Framework.Player
{
    public sealed class MouseTiltMotionModule : TiltMotionModule
    {
        private PlayerStateMachine.MouseTiltMotionExtension extension;
        public PlayerStateMachine.MouseTiltMotionExtension Extension => extension;


        protected sealed override PlayerStateMachine.PlayerExtension GetExtension(PlayerStateMachine owner)
        {
            PlayerStateMachine.MouseTiltMotionExtension extension;
            if (owner == null) return null;

            extension = owner.GetExtension(PlayerStateMachine.MouseTiltMotionExtension.EXTENSION_NAME) as PlayerStateMachine.MouseTiltMotionExtension;
            if (extension != null) return extension;

            return new PlayerStateMachine.MouseTiltMotionExtension(owner, transform, this);
        }

        public sealed override PlayerStateMachine.PlayerExtension GetExtension() { return extension; }

        protected sealed override void SetExtension(PlayerStateMachine.PlayerExtension extension)
        {
            this.extension = extension as PlayerStateMachine.MouseTiltMotionExtension;
        }
    }
}