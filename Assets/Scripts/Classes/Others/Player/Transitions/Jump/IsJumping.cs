using UnityEngine;

namespace RedSilver2.Framework.Player
{
    public partial class PlayerStateMachine
    {
        public sealed class IsJumping : JumpConditionTransition
        {
            public const string TRANSITION_CONDITION_NAME = "Is Jumping";

            private IsJumping(PlayerStateMachine controller) : base(controller)
            {
            }

            public override bool WasConditionMeet()
            {
                return input.Value;
            }

            protected override string[] GetCompatibleStateNames()
            {
                return new string[] { JumpState.STATE_NAME };
            }

            protected override string GetTransitionName()
            {
                return TRANSITION_CONDITION_NAME;
            }


            public static void Intialize(PlayerStateMachine controller)
            {
                if (controller != null && Get(controller) == null)
                {
                    controller.AddTransitionCondition(TRANSITION_CONDITION_NAME, new IsJumping(controller));
                }
            }

            public static IsJumping Get(PlayerStateMachine controller)
            {
                if (controller == null) return null;
                return controller.GetTransitionCondition(TRANSITION_CONDITION_NAME) as IsJumping;
            }
        }
    }
}
