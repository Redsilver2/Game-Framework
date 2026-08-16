using RedSilver2.Framework.Interactions;
using UnityEngine;

namespace RedSilver2.Framework.StateMachines.States
{
    [System.Serializable]
    public sealed class LockDoorState : DoorState
    {
        private LockableDoorStateMachine LockableDoorStateMachine;

        public const DoorStateType TYPE = DoorStateType.Locked;

        public LockDoorState() : base() {

        }

        public void SetStateMachine(LockableDoorStateMachine stateMachine)
        {
            this.DoorStateMachine         = stateMachine;
            this.LockableDoorStateMachine = stateMachine;
            SetStateMachine(stateMachine as StateMachine);
        }

        protected sealed override void SetDoorStateType(ref DoorStateType type) {
            type = TYPE;
        }

        protected sealed override void OnEntered() {

        }

        public sealed override bool CanTransition()
        {
            if (LockableDoorStateMachine == null) return false;
            return base.CanTransition() && !LockableDoorStateMachine.IsLocked;
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
