using UnityEngine;
namespace RedSilver2.Framework.Player
{
    public partial class PlayerStateMachine 
    {
        public class IsNotMoving : MoveTransitionCondition
        {
            public const string TRANSITION_CONDITION_NAME = "Is Not Moving";

            protected IsNotMoving(PlayerStateMachine controller) : base(controller)
            {
            }

            protected override string GetTransitionName()
            {
                return TRANSITION_CONDITION_NAME;
            }

            public sealed override bool WasConditionMeet()
            {
                return GetInputMagnitude() == 0f;
            }

            protected sealed override string[] GetCompatibleStateNames()
            {
                return new string[] { IdolState.STATE_NAME };
            }

            public static void Intialize(PlayerStateMachine controller)
            {
                if (controller != null && Get(controller) == null)
                {
                    controller.AddTransitionCondition(TRANSITION_CONDITION_NAME, new IsNotMoving(controller));
                }
            }

            public static IsNotMoving Get(PlayerStateMachine controller)
            {
                if (controller == null) return null;
                return controller.GetTransitionCondition(TRANSITION_CONDITION_NAME) as IsNotMoving;
            }
        }
    }
}
