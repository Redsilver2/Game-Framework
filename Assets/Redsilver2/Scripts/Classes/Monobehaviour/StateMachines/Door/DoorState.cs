using RedSilver2.Framework.Interactions;
using UnityEngine;

namespace RedSilver2.Framework.StateMachines.States {

    [System.Serializable]
    public abstract class DoorState : State {
        private DoorStateType type;
        [SerializeReference, HideInInspector] private DoorStateMachine doorStateMachine;
      
        protected DoorStateMachine DoorStateMachine => doorStateMachine;
        public DoorStateType Type => type;

        protected DoorState(DoorStateMachine stateMachine) : base(stateMachine) {
            this.doorStateMachine = stateMachine;
            SetDoorStateType(ref type);
        }



        protected sealed override bool CanAddTransitionState(State state) {
            if(base.CanAddTransitionState(state)) return CanAddTransitionState(state as DoorState);
            return false;
        }

        private bool CanAddTransitionState(DoorState state) {
            return state != null ?  true : false; 
        }

        public override bool CanTransition()
        {
            if(doorStateMachine == null) return false;
            return base.CanTransition();
        }

        protected abstract void SetDoorStateType(ref DoorStateType type);
    }

}
