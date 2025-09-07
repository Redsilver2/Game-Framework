using UnityEngine;
namespace RedSilver2.Framework.Player
{
    public sealed class KeyboardTiltMotionModule : TiltMotionModule
    {
        [Space]
        [SerializeField] private float rotationSpeed;
        public float RotationSpeed => rotationSpeed;

        private PlayerStateMachine.KeyboardTiltMotionExtension extension;
        public PlayerStateMachine.KeyboardTiltMotionExtension Extension => extension;

        protected sealed override PlayerStateMachine.PlayerExtension GetExtension(PlayerStateMachine owner)
        {
            PlayerStateMachine.KeyboardTiltMotionExtension extension;
            if (owner == null) return null;

            extension = owner.GetExtension(PlayerStateMachine.KeyboardTiltMotionExtension.EXTENSION_NAME) as PlayerStateMachine.KeyboardTiltMotionExtension;
            if (extension != null) return extension;

            return new PlayerStateMachine.KeyboardTiltMotionExtension(owner, transform, this);
        }

        public sealed override PlayerStateMachine.PlayerExtension GetExtension() { return extension; }

        protected override void SetExtension(PlayerStateMachine.PlayerExtension extension)
        {
            this.extension = extension as PlayerStateMachine.KeyboardTiltMotionExtension;
        }
    }
}
