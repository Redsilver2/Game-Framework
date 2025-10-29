using UnityEngine;

namespace RedSilver2.Framework.Player
{
    public class GroundCheckModule : PlayerExtensionModule
    {
        [Space]
        [SerializeField] private float groundCheckRange;
        public float GroundCheckRange => groundCheckRange;

        private PlayerStateMachine.GroundCheckExtension extension;
        public  PlayerStateMachine.GroundCheckExtension Extension => extension;

        protected override void SetExtension(PlayerStateMachine.PlayerExtension extension)
        {
            this.extension = extension as PlayerStateMachine.GroundCheckExtension;
        }

        protected sealed override PlayerStateMachine.PlayerExtension GetExtension(PlayerStateMachine owner)
        {
            PlayerStateMachine.GroundCheckExtension extension;
            if (owner == null) return null;
             
            extension = owner.GetExtension(PlayerStateMachine.GroundCheckExtension.EXTENSION_NAME) as PlayerStateMachine.GroundCheckExtension;
            if (extension != null) return extension;

            return new PlayerStateMachine.GroundCheckExtension(owner, transform, this);
        }

        public sealed override PlayerStateMachine.PlayerExtension GetExtension() { return extension; }

        public static void CreateInstance(Transform transform)
        {
            if (transform == null) { return; }

            if (!transform.GetComponentInChildren<GroundCheckModule>())
            {
                Transform newtransform = new GameObject(PlayerStateMachine.GroundCheckExtension.EXTENSION_NAME)
                     .AddComponent<GroundCheckModule>()
                     .transform;


                newtransform.parent = transform;
                newtransform.localPosition = Vector3.zero;
            }
        }
    }
}
