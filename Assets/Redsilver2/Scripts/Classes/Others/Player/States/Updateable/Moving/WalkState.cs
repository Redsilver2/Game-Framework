using System.Linq;

namespace RedSilver2.Framework.Player
{
    public partial class PlayerStateMachine 
    {
        public sealed class WalkState : MoveState
        {
            public const string STATE_NAME = "Walk";

            private WalkState() { }

            public WalkState(PlayerStateMachine owner) : base(owner)
            {
                IsMoving.Intialize(owner);
            }

            public sealed override string GetStateName() => STATE_NAME;

            public static bool Contains(PlayerStateMachine owner)
            {
                return Contains(owner, out int count);
            }

            protected sealed override void SetObligatoryTransition(PlayerState[] states, bool removeTransition)
            {
                AddTransitionCondition(IsMoving.Get(owner));
            }

            public static bool Contains(PlayerStateMachine owner, out int count)
            {
                count = 0;
                if (owner == null) return false;

                count = owner.GetStates().Where(x => x is WalkState).Count();
                return count > 0;
            }
        }
    }
}
