using RedSilver2.Framework.Interactions;

namespace RedSilver2.Framework.StateMachines.States
{
    public class UnlockDoorState : DoorState
    {
        public const DoorStateType TYPE = DoorStateType.Unlocked;

        protected sealed override void SetDoorStateType(ref DoorStateType type) {
            type = TYPE;
        }

        protected sealed override void OnEntered(DoorStateMachine stateMachine)
        {
            base.OnEntered(stateMachine);
            stateMachine?.SetLockState(false);
        }

        protected sealed override void SetIncompatibleTransitionStates(ref string[] incompatibleStates)
        {
            incompatibleStates = new string[] { LockDoorState.TYPE.ToString() };
            base.SetIncompatibleTransitionStates(ref incompatibleStates);
        }

        public UnlockDoorState GetState(DoorStateMachine stateMachine)
        {
            if (stateMachine == null) return null;
            return stateMachine.GetState(TYPE) as UnlockDoorState;
        }
    }
}
