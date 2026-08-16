using RedSilver2.Framework.Interactions;

namespace RedSilver2.Framework.StateMachines.States
{
    [System.Serializable]
    public sealed class UnlockDoorState : DoorState
    {
        private LockableDoorStateMachine LockableDoorStateMachine;
        public const DoorStateType TYPE = DoorStateType.Unlocked;

        public UnlockDoorState() : base()
        {

        }

        protected sealed override void SetDoorStateType(ref DoorStateType type) {
            type = TYPE;
        }

        public void SetStateMachine(LockableDoorStateMachine stateMachine)
        {
            this.DoorStateMachine = stateMachine;
            this.LockableDoorStateMachine = stateMachine;
            SetStateMachine(stateMachine as StateMachine);
        }


        protected sealed override void OnEntered() {

        }

        protected sealed override void SetIncompatibleTransitionStates(ref string[] incompatibleStates)
        {
            incompatibleStates = new string[] { LockDoorState.TYPE.ToString() };
            base.SetIncompatibleTransitionStates(ref incompatibleStates);
        }

        public sealed override bool CanTransition()
        {
            if (LockableDoorStateMachine == null) return false;
            return base.CanTransition() && LockableDoorStateMachine.IsLocked;
        }


        public UnlockDoorState GetState(LockableDoorStateMachine stateMachine)
        {
            if (stateMachine == null) return null;
            return stateMachine.GetState(TYPE) as UnlockDoorState;
        }
    }
}
