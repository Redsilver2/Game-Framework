

namespace RedSilver2.Framework.StateMachines.States
{
    [System.Serializable]
    public struct MovementStateConditionCheck  {
        public MovementStateCondition condition;
        public bool                   showOppositeResult;

        public bool GetResult() {
            if (condition == null || !condition.IsEnabled) return true;
            return showOppositeResult ? !condition.Check() : condition.Check();
        }
    }
}
