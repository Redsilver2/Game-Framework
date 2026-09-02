using RedSilver2.Framework.Interactions;

namespace RedSilver2.Framework.StateMachines.States
{
    [System.Serializable]
    public sealed class UnlockDoorState : DoorState
    {
        public const DoorStateType TYPE = DoorStateType.Unlocked;

        public UnlockDoorState(LockableDoorStateMachine stateMachine) : base(stateMachine)
        {

        }

        protected sealed override void SetDoorStateType(ref DoorStateType type) {
            type = TYPE;
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
            LockableDoorStateMachine stateMachine = DoorStateMachine as LockableDoorStateMachine;
          
            if (stateMachine == null) return false;
            return base.CanTransition() && stateMachine.IsLocked;
        }


        public UnlockDoorState GetState(LockableDoorStateMachine stateMachine)
        {
            if (stateMachine == null) return null;
            return stateMachine.GetState(TYPE) as UnlockDoorState;
        }
    }
}
