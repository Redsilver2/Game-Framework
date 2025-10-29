using UnityEngine;
namespace RedSilver2.Framework.Player
{
    public partial class PlayerStateMachine 
    {
        public sealed class IsMoving : MoveTransitionCondition
        {
            public const string TRANSITION_CONDITION_NAME = "Is Moving";

            private IsMoving(PlayerStateMachine controller) : base(controller)
            {

            }

            protected sealed override string GetTransitionName()
            {
                return TRANSITION_CONDITION_NAME;
            }

            public sealed override bool WasConditionMeet()
            {
                return GetInputMagnitude() > 0f;
            }

            protected sealed override string[] GetCompatibleStateNames()
            {
                return new string[] { WalkState.STATE_NAME, RunState.STATE_NAME, CrouchState.STATE_NAME };
            }

            public static void Intialize(PlayerStateMachine controller)
            {
                if (controller != null && Get(controller) == null)
                {
                    controller.AddTransitionCondition(TRANSITION_CONDITION_NAME, new IsMoving(controller));
                }
            }

            public static IsMoving Get(PlayerStateMachine controller)
            {
                if (controller == null) return null;
                return controller.GetTransitionCondition(TRANSITION_CONDITION_NAME) as IsMoving;
            }
        }
    }
}
