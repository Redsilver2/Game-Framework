using UnityEngine;
namespace RedSilver2.Framework.Player
{
    public partial class PlayerStateMachine
    {
        public abstract class CrouchTransitionCondition : PlayerStateTransitionCondition
        {
            protected readonly  CrouchState crouchState;

            public CrouchTransitionCondition(PlayerStateMachine controller) : base(controller)
            {
               if(controller != null) crouchState = controller.GetState(CrouchState.STATE_NAME) as CrouchState;
            }
        }
    }
}
