using System.Linq;

namespace RedSilver2.Framework.Player
{
    public partial class PlayerStateMachine 
    {
        public sealed class FallState : MoveState
        {
            private bool canDisableMovementInput;
            public const string STATE_NAME = "Fall";

            private FallState()
            {

            }

            public FallState(PlayerStateMachine owner, bool canDisableMovementInput) : base(owner)
            {
                this.canDisableMovementInput = canDisableMovementInput;

                NotOnGround.Intialize(owner);
                IsOnGround .Intialize(owner);
            }

            public sealed override string GetStateName() => STATE_NAME;

            protected sealed override void SetObligatoryTransition(PlayerState[] states, bool removeTransition)
            {
                AddTransitionCondition(NotOnGround.Get(owner));

                if (states != null)
                {
                    foreach (PlayerState state in states.Where(x => x != this))
                    {
                        if (removeTransition) { state.RemoveTransitionCondition(IsOnGround.TRANSITION_CONDITION_NAME); }
                        else                  { state.AddTransitionCondition   (IsOnGround.TRANSITION_CONDITION_NAME); }
                    }
                }
            }

            protected sealed override void OnStateEnter()
            {
                if (movementInput != null && canDisableMovementInput) movementInput.Disable();
                base.OnStateEnter();
            }

            protected sealed override void OnStateExit()
            {
                if (movementInput != null) movementInput.Enable();
                base.OnStateExit();
            }

            

            public static bool Contains(PlayerStateMachine owner)
            {
                return Contains(owner, out int count);
            }

            public static bool Contains(PlayerStateMachine owner, out int count)
            {
                count = 0;
                if(owner == null) return false;

                count = owner.GetStates().Where(x => x is FallState).Count();
                return count > 0;
            }
        }
    }
}
