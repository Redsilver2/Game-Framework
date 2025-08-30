using UnityEngine;

namespace RedSilver2.Framework.Player
{
    public partial class PlayerStateMachine 
    {
        public abstract class GroundTransitionCondition : PlayerStateTransitionCondition
        {
            protected readonly GroundCheckExtension groundCheck;



            protected GroundTransitionCondition(PlayerStateMachine controller) : base(controller)
            {
               groundCheck = GetGroundCheckExtension(controller);
            }

            public override bool IsCompatible(PlayerState state)
            {
                if (state == null) return false;
                return FallState.Contains(state.Owner) && base.IsCompatible(state);
            }

            private GroundCheckExtension GetGroundCheckExtension(PlayerStateMachine controller)
            {
                GroundCheckExtension.Instantiate(controller);
                return GroundCheckExtension.Get(controller);
            }
        }
    }
}
