using RedSilver2.Framework.StateMachines.States;

namespace RedSilver2.Framework.StateMachines.Events
{
    [System.Serializable]
    public abstract class UpdatableStateEvent : StateEvent
    {
        protected UpdatableStateEvent() : base() { }
    }
}
