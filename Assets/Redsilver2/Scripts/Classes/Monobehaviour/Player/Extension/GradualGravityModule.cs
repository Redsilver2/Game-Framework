using UnityEngine;

namespace RedSilver2.Framework.Player
{
    public sealed class GradualGravityModule : GravityModule
    {
        private PlayerStateMachine.GradualGravityExtension extension;
        public PlayerStateMachine.GradualGravityExtension Extension => extension;

        protected sealed override void SetExtension(PlayerStateMachine.PlayerExtension extension)
        {
            this.extension = extension as PlayerStateMachine.GradualGravityExtension;
        }

        protected sealed override PlayerStateMachine.PlayerExtension GetExtension(PlayerStateMachine owner)
        {
            PlayerStateMachine.GradualGravityExtension extension;
            if (owner == null) return null;

            extension = owner.GetExtension(PlayerStateMachine.GradualGravityExtension.EXTENSION_NAME) as PlayerStateMachine.GradualGravityExtension;
            if (extension != null) return extension;

            return new PlayerStateMachine.GradualGravityExtension(owner, this);
        }

        public sealed override PlayerStateMachine.PlayerExtension GetExtension() { return extension; }

        public static void CreateInstance(Transform transform)
        {
            if(transform == null) return;

            if (!transform.GetComponentInChildren<GravityModule>())
            {
                Transform newtransform = new GameObject(PlayerStateMachine.GradualGravityExtension.EXTENSION_NAME)
                     .AddComponent<GradualGravityModule>()
                     .transform;

                newtransform.parent = transform;
                newtransform.localPosition = Vector3.zero;
            }
        }    
    }
}
