using RedSilver2.Framework.Interactions;
using UnityEngine;

namespace RedSilver2.Framework.StateMachines.States
{
    [System.Serializable]
    public sealed class LockDoorState : DoorState
    {
        public const DoorStateType TYPE = DoorStateType.Locked;

        public LockDoorState(LockableDoorStateMachine stateMachine) : base(stateMachine) {

        }

        protected sealed override void SetDoorStateType(ref DoorStateType type) {
            type = TYPE;
        }

        protected sealed override void OnEntered() {

        }

        public sealed override bool CanTransition() {
            LockableDoorStateMachine stateMachine = DoorStateMachine as LockableDoorStateMachine;
            if (stateMachine == null) return false;
            return base.CanTransition() && !stateMachine.IsLocked;
        }

        protected sealed override void SetIncompatibleTransitionStates(ref string[] incompatibleStates) {
            incompatibleStates = new string[] {
                UnlockDoorState.TYPE.ToString(),
                DoorStateType.Open.ToString()
            };

            base.SetIncompatibleTransitionStates(ref incompatibleStates);
        }

        public LockDoorState GetState(LockableDoorStateMachine stateMachine) {
            if (stateMachine == null) return null;
            return stateMachine.GetState(TYPE) as LockDoorState;
        }
    }
}
