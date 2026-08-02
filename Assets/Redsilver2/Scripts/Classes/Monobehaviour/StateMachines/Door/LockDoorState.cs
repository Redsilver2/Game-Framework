using RedSilver2.Framework.Interactions;
using UnityEngine;

namespace RedSilver2.Framework.StateMachines.States
{
    public sealed class LockDoorState : DoorState
    {
        public const DoorStateType TYPE = DoorStateType.Locked;

        protected sealed override void SetDoorStateType(ref DoorStateType type) {
            type = TYPE;
        }

        protected sealed override void OnEntered(DoorStateMachine stateMachine) {
            base.OnEntered(stateMachine);
            stateMachine?.SetLockState(true);
        }

        public sealed override bool CanTransition(DoorStateMachine stateMachine)
        {
            if (stateMachine == null) return false;
            return !stateMachine.IsLocked;
        }

        protected sealed override void SetIncompatibleTransitionStates(ref string[] incompatibleStates) {
            incompatibleStates = new string[] {
                UnlockDoorState.TYPE.ToString(),
                DoorStateType.Open.ToString()
            };

            base.SetIncompatibleTransitionStates(ref incompatibleStates);
        }

        public LockDoorState GetState(DoorStateMachine stateMachine) {
            if (stateMachine == null) return null;
            return stateMachine.GetState(TYPE) as LockDoorState;
        }
    }
}
