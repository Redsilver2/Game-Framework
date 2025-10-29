using RedSilver2.Framework.Inputs;
using UnityEngine;


namespace RedSilver2.Framework.Player
{
    public partial class PlayerStateMachine 
    {
        public abstract class JumpConditionTransition : PlayerStateTransitionCondition
        {
            protected readonly PressInput input;

            protected JumpConditionTransition(PlayerStateMachine controller) : base(controller)
            {
                input = JumpState.GetJumpInput();
            }

            public sealed override bool IsCompatible(PlayerState state)
            {
                if (state == null) return false;
                return JumpState.Contains(state.Owner) && base.IsCompatible(state);
            }
        }
    }
}
