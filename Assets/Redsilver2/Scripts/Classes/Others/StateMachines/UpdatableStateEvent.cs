using RedSilver2.Framework.StateMachines.States;

namespace RedSilver2.Framework.StateMachines.Events
{
    [System.Serializable]
    public abstract class UpdatableStateEvent : StateEvent
    {
        protected UpdatableStateEvent(UpdatableState state) : base(state) {
                 
        }

        protected sealed override void Disable(State state) {
            Disable(state as UpdatableState);
        }

        protected sealed override void Enable(State state)
        {
            Enable(state as UpdatableState);
        }


        protected abstract void Disable(UpdatableState state);

        protected abstract void Enable(UpdatableState state);
    }
}
