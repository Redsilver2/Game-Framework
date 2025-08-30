using System.Linq;

namespace RedSilver2.Framework.Player
{
    public partial class PlayerStateMachine 
    {
        public sealed class IdolState : PlayerState
        {
            public const string STATE_NAME = "Idol";

            private IdolState() { }

            public IdolState(PlayerStateMachine owner) : base(owner)
            {
                IsNotMoving.Intialize(owner);
                AddTransitionCondition(IsNotMoving.Get(owner));
            }

            public sealed override string GetStateName() => STATE_NAME;

            protected override void SetObligatoryTransition(PlayerState[] states, bool removeTransition) { }

            public static bool Contains(PlayerStateMachine owner)
            {
                return Contains(owner, out int count);
            }

            public static bool Contains(PlayerStateMachine owner, out int count)
            {
                count = 0;
                if (owner == null) return false;

                count = owner.GetStates().Where(x => x is IdolState).Count();
                return count > 0;
            }
        }
    }
}
