using RedSilver2.Framework.Inputs;
using UnityEngine;

namespace RedSilver2.Framework.Player
{
    public partial class PlayerStateMachine
    {
        public abstract class MoveTransitionCondition : PlayerStateTransitionCondition
        {
            private readonly Vector2Input input;

            protected MoveTransitionCondition(PlayerStateMachine controller) : base(controller)
            {
                input = MoveState.GetMovementInput();
            }

            protected float GetInputMagnitude()
            {
                if (input != null) return input.Value.magnitude;
                return 0f;
            }
        }
    }
}
