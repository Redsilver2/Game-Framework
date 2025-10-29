using UnityEngine;

namespace RedSilver2.Framework.Player
{
    public class HeadbobModule : PlayerExtensionModule
    {
        [SerializeField] private Vector2 starterPosition;

        [Space]
        [SerializeField] private Vector2 minPosition;
        [SerializeField] private Vector2 maxPosition;

        [Space]
        [SerializeField] private float headbobSpeed;

        public float   HeadbobSpeed    => headbobSpeed;
        public Vector2 StarterPosition => starterPosition;
        public Vector2 MinPosition     => minPosition;
        public Vector2 MaxPosition     => maxPosition;

        private PlayerStateMachine.HeadbobExtension extension;
        public PlayerStateMachine.HeadbobExtension Extension => extension;

        protected sealed override void SetExtension(PlayerStateMachine.PlayerExtension extension)
        {
            this.extension = extension as PlayerStateMachine.HeadbobExtension;
        }

        protected override PlayerStateMachine.PlayerExtension GetExtension(PlayerStateMachine owner)
        {
            PlayerStateMachine.HeadbobExtension extension;
            if (owner == null) return null;

            extension = owner.GetExtension(PlayerStateMachine.HeadbobExtension.EXTENSION_NAME) as PlayerStateMachine.HeadbobExtension;
            if (extension != null) return extension;

            return new PlayerStateMachine.HeadbobExtension(owner, transform, this);
        }

        public sealed override PlayerStateMachine.PlayerExtension GetExtension() { return extension; }

    }
}
