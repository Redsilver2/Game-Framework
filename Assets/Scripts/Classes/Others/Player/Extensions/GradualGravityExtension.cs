using UnityEngine;

namespace RedSilver2.Framework.Player
{
    public partial class PlayerStateMachine
    {
        public sealed class GradualGravityExtension : GravityExtension
        {
            public const string EXTENSION_NAME = "Gradual Gravity Extension";
            private bool wasUpdateEventAdded = false;

            private GradualGravityExtension(PlayerStateMachine owner) : base(owner)
            {

            }

            public GradualGravityExtension(PlayerStateMachine owner, float defaultGravity) : base(owner, defaultGravity)
            {

            }

            private void OnUpdate()
            {
               if(owner != null) SetGravity(Mathf.Lerp(GetGravity(), GetDesiredGravity(), Time.deltaTime * owner.GravityTransitionSpeed));
            }

            protected sealed override void AddListeners()
            {
                base.AddListeners();

                if (owner != null && States.Length > 0)
                {
                    if (!wasUpdateEventAdded)
                    {
                        wasUpdateEventAdded = true;
                        owner.AddOnUpdateListener(OnUpdate);
                    }
                }
            }

            protected sealed override void RemoveListeners()
            {
                base.RemoveListeners();

                if (owner != null && States.Length == 0)
                {
                    if (wasUpdateEventAdded)
                    {
                        wasUpdateEventAdded = false;
                        owner.RemoveOnUpdateListener(OnUpdate);
                    }
                }
            }

            protected override string GetExtensionName() => EXTENSION_NAME;

            public static void Enable(PlayerStateMachine controller)
            {
                GradualGravityExtension extension = Get(controller);
                if (extension != null) extension.Enable();
            }

            public static void Disable(PlayerStateMachine controller)
            {
                GradualGravityExtension extension = Get(controller);
                if (extension != null) extension.Disable();
            }

            public static void Intialize(PlayerStateMachine controller)
            {
                if (controller != null)
                {
                    if (Get(controller) == null)
                    {
                        controller.AddExtension(new GradualGravityExtension(controller));
                    }
                }
            }

            public static GradualGravityExtension Get(PlayerStateMachine controller)
            {
                if (controller == null) return null;
                return controller.GetExtension(EXTENSION_NAME) as GradualGravityExtension;
            }
        }
    }
}
