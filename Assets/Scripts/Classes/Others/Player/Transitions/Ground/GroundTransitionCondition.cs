

namespace RedSilver2.Framework.Player
{
    public partial class PlayerStateMachine 
    {
        public abstract class GroundTransitionCondition : PlayerStateTransitionCondition
        {
            protected readonly GroundCheckModule groundCheck;



            protected GroundTransitionCondition(PlayerStateMachine controller) : base(controller)
            {
               if(controller != null) groundCheck = controller.owner.GetComponentInChildren<GroundCheckModule>();     
            }

            public override bool IsCompatible(PlayerState state)
            {
                if (state == null) return false;
                return FallState.Contains(state.Owner) && base.IsCompatible(state);
            }

            private GroundCheckExtension GetGroundCheckExtension(PlayerStateMachine controller)
            {
                return GroundCheckExtension.Get(controller);
            }
        }
    }
}
