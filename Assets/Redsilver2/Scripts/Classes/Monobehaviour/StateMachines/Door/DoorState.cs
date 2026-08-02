using RedSilver2.Framework.Interactions;

namespace RedSilver2.Framework.StateMachines.States {
    public abstract class DoorState : State {
        private DoorStateType type;
        public DoorStateType Type => type;


        protected override void Awake() {
            base.Awake();
            SetDoorStateType(ref type);
            SetStateName(type.ToString());
        }

        protected sealed override void OnEntered(StateMachine stateMachine)
        {
            base.OnEntered(stateMachine);
            OnEntered(stateMachine as DoorStateMachine);
        }

        protected sealed override void OnExited(StateMachine stateMachine)
        {
            base.OnExited(stateMachine);
            OnExited(stateMachine as DoorStateMachine);
        }

        protected sealed override void OnDisabled(StateMachine stateMachine)
        {
            base.OnDisabled(stateMachine as DoorStateMachine);
            OnDisabled(stateMachine as DoorStateMachine);
        }

        protected override void OnEnabled(StateMachine stateMachine) {
            OnEnabled(stateMachine as DoorStateMachine);
            base.OnEnabled(stateMachine);
        }

        protected virtual void OnEnabled(DoorStateMachine stateMachine)  { }
        protected virtual void OnDisabled(DoorStateMachine stateMachine) { }


        protected virtual void OnEntered(DoorStateMachine stateMachine) {  }
        protected virtual void OnExited(DoorStateMachine stateMachine)  {  }


        protected sealed override bool CanAddTransitionState(State state) {
            if(base.CanAddTransitionState(state)) return CanAddTransitionState(state as DoorState);
            return false;
        }

        private bool CanAddTransitionState(DoorState state) {
            return state != null ?  true : false; 
        }

        public sealed override bool CanTransition(StateMachine stateMachine)
        {
           if(base.CanTransition(stateMachine)) return CanTransition(stateMachine as DoorStateMachine);
           return false;
        }

        public virtual bool CanTransition(DoorStateMachine stateMachine)  {
            return stateMachine != null ? true : false;
        }

        protected abstract void SetDoorStateType(ref DoorStateType type);
    }

}
