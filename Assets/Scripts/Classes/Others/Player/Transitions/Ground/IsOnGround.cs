using UnityEngine;

namespace RedSilver2.Framework.Player
{
    public partial class PlayerStateMachine
    {
        public sealed class IsOnGround : GroundTransitionCondition
        {
            public const string TRANSITION_CONDITION_NAME = "Is Grounded";

            private IsOnGround(PlayerStateMachine controller) : base(controller)
            {
               
            }

            public sealed override bool WasConditionMeet()
            {
                if (groundCheck != null) return groundCheck.IsGrounded;
                return false;
            }

            protected sealed override string GetTransitionName()
            {
                return TRANSITION_CONDITION_NAME;
            }

            protected sealed override string[] GetCompatibleStateNames()
            {
                return new string[] { WalkState.STATE_NAME, IdolState.STATE_NAME, JumpState.STATE_NAME };
            }

            public static void Intialize(PlayerStateMachine controller)
            {
                if(controller != null && Get(controller) == null)
                {
                    controller.AddTransitionCondition(TRANSITION_CONDITION_NAME, new IsOnGround(controller));
                }
            }

            public static IsOnGround Get(PlayerStateMachine controller)
            {
                if(controller == null) return null;
                return controller.GetTransitionCondition(TRANSITION_CONDITION_NAME) as IsOnGround;
            }
        }
    }
}
