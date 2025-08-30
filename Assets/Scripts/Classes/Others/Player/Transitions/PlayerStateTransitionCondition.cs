using System.Linq;
using UnityEngine;

namespace RedSilver2.Framework.Player
{
    public partial class PlayerStateMachine 
    {
        public abstract class PlayerStateTransitionCondition
        {
            private readonly string[] compatibleStateNames;

            private PlayerStateTransitionCondition() { }

            protected PlayerStateTransitionCondition(PlayerStateMachine controller)
            {
                compatibleStateNames = GetCompatibleStateNames();
            }

            public virtual bool IsCompatible(PlayerState state)
            {
                if (compatibleStateNames != null && state != null)
                    return compatibleStateNames.Where(x => x.ToLower() == state.GetStateName().ToLower())
                                               .Count() > 0;

                return false;
            }

            public abstract bool WasConditionMeet();
            protected abstract string GetTransitionName();

            public bool Compare(string transitionName) => transitionName.ToLower() == GetTransitionName().ToLower();
            protected abstract string[] GetCompatibleStateNames();
        }

        private void SetAllTransitionsForState()
        {
            if (states != null && states != null)
            {
                foreach(PlayerState state in states.Values)
                {
                    state.AddObligatoryTransition();
                }
            }
        }

        private void AddTransitionCondition(string name, PlayerStateTransitionCondition transitionCondition)
        {
            name = name.ToLower();

            if (transitionCondition != null && transitionConditions != null)
            {
                if (!string.IsNullOrEmpty(name) && !transitionConditions.ContainsKey(name))
                {
                    transitionConditions.Add(name, transitionCondition);
                    SetStateTransitionCondition(transitionCondition);
                }
            }
        }
        private void SetStateTransitionCondition(PlayerStateTransitionCondition condition)
        {
            if (transitionConditions != null && states != null)
            {
                foreach (PlayerState state in states.Values)
                {
                   state.AddTransitionCondition(condition);
                }
            }
        }
        private PlayerStateTransitionCondition GetTransitionCondition(string name)
        {
            if (transitionConditions == null || string.IsNullOrEmpty(name)) return null;
            name = name.ToLower();

            if (transitionConditions.ContainsKey(name)) return transitionConditions[name];
            return null;
        }
    }


}
