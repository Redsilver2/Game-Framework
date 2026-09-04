using RedSilver2.Framework.StateMachines.States;

namespace RedSilver2.Framework.StateMachines.Events {
    [System.Serializable]
    public abstract class MovementStateEvent : UpdatableStateEvent {

        protected MovementStateEvent(MovementState state) : base(state) {
    
        }

        protected sealed override void Enable(UpdatableState state) {
            Enable(state as MovementState);
        }

        protected sealed override void Disable(UpdatableState state) {
            Disable(state as MovementState);
        }

        protected abstract void Enable(MovementState state);
        protected abstract void Disable(MovementState state);
    }
}