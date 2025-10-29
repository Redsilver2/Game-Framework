using UnityEngine;

namespace RedSilver2.Framework.Player
{
    public partial class PlayerStateMachine
    {
        [System.Serializable]
        public sealed class GradualGravityExtension : GravityExtension
        {
            public const string EXTENSION_NAME = "Gradual Gravity Extension";

            public GradualGravityExtension(PlayerStateMachine owner, GradualGravityModule module) : base(owner, module)
            {
         
            }

            private void OnUpdate()
            {
               if(owner != null && gravityModule != null) SetGravity(Mathf.Lerp(gravityModule.Gravity, GetDesiredGravity(), Time.deltaTime * 5f));
            }

            protected sealed override void OnEnable()
            {
                base.OnEnable();
                if(owner != null) owner.AddOnUpdateListener(OnUpdate);
            }

            protected sealed override void OnDisable()
            {
                base.OnDisable();
                if (owner != null) owner.RemoveOnUpdateListener(OnUpdate);
            }

            public override bool Compare(PlayerExtension extension)
            {
                if(extension == null) return false;
                return extension is GradualGravityExtension;
            }


            protected override string GetExtensionName() => EXTENSION_NAME;


            public static GradualGravityExtension Get(PlayerStateMachine controller)
            {
                if (controller == null) return null;
                return controller.GetExtension(EXTENSION_NAME) as GradualGravityExtension;
            }
        }
    }
}
