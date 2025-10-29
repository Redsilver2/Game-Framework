using UnityEngine;

namespace RedSilver2.Framework.Player
{
    public partial class PlayerStateMachine 
    {
        public class IsNotJumping : JumpConditionTransition
        {
            public const string TRANSITION_CONDITION_NAME = "Is Not Jumping";

            private IsNotJumping(PlayerStateMachine controller) : base(controller)
            {
            }

            public override bool WasConditionMeet()
            {
                return !input.Value;
            }

            protected override string[] GetCompatibleStateNames()
            {
                return new string[] { WalkState.STATE_NAME, RunState.STATE_NAME, CrouchState.STATE_NAME,
                                      JumpState.STATE_NAME, IdolState.STATE_NAME};
            }

            protected override string GetTransitionName()
            {
                return TRANSITION_CONDITION_NAME;
            }

            public static void Intialize(PlayerStateMachine controller)
            {
                if (controller != null && Get(controller) == null)
                {
                    controller.AddTransitionCondition(TRANSITION_CONDITION_NAME, new IsNotJumping(controller));
                }
            }

            public static IsNotJumping Get(PlayerStateMachine controller)
            {
                if (controller == null) return null;
                return controller.GetTransitionCondition(TRANSITION_CONDITION_NAME) as IsNotJumping;
            }
        }
    }
}
