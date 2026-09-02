using RedSilver2.Framework.Interactions;
using UnityEngine;

namespace RedSilver2.Framework.StateMachines.States
{
    [System.Serializable]
    public class CloseDoorState : UpdatableDoorState
    {

        public const DoorStateType TYPE = DoorStateType.Close;

        public CloseDoorState(DoorStateMachine stateMachine) : base(stateMachine) {

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
