using RedSilver2.Framework.Interactions;
using UnityEngine;

namespace RedSilver2.Framework.StateMachines.States
{
    [System.Serializable]
    public class CloseDoorState : UpdatableDoorState
    {

        public const DoorStateType TYPE = DoorStateType.Close;

        public CloseDoorState() : base() {

        }

#if UNITY_EDITOR
        public void Validate(DoorStateMachine stateMachine)
        {
            SetStateMachine(stateMachine);
            Validate();
        }
#endif

        private void SetStateMachine(DoorStateMachine stateMachine)
        {
            this.DoorStateMachine = stateMachine;
            SetStateMachine(stateMachine as StateMachine);
        }

        protected sealed override void SetDoorStateType(ref DoorStateType type) {
            type = TYPE;
        }

        public sealed override bool CanTransition()  {
            if (DoorStateMachine == null) return false;
            return base.CanTransition() && DoorStateMachine.IsOpen;
        }

        public static CloseDoorState GetState(DoorStateMachine stateMachine)
        {
            if (stateMachine == null) return null;
            return stateMachine.GetState(TYPE) as CloseDoorState;
        }

    }
}
