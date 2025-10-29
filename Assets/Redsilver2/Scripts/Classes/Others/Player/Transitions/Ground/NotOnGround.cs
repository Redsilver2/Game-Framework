using UnityEngine;

namespace RedSilver2.Framework.Player
{
    public partial class PlayerStateMachine 
    {
        public sealed class NotOnGround : GroundTransitionCondition
        {
            public const string TRANSITION_CONDITION_NAME = "Is Falling";

            private NotOnGround(PlayerStateMachine controller) : base(controller)
            {
            }

            protected sealed override string GetTransitionName()
            {
                return TRANSITION_CONDITION_NAME;
            }

            public sealed override bool WasConditionMeet()
            {
                PlayerStateMachine.GroundCheckExtension extension = groundCheck.Extension;
                if (extension != null) return !extension.IsGrounded;
                return false;
            }

            protected sealed override string[] GetCompatibleStateNames()
            {
                return new string[] { FallState.STATE_NAME };
            }

            public static void Intialize(PlayerStateMachine controller)
            {
                if (controller != null && Get(controller) == null)
                {
                    controller.AddTransitionCondition(TRANSITION_CONDITION_NAME, new NotOnGround(controller));
                }
            }

            public static NotOnGround Get(PlayerStateMachine controller)
            {
                if (controller == null) return null;
                return controller.GetTransitionCondition(TRANSITION_CONDITION_NAME) as NotOnGround;
            }
        }
    }
}
