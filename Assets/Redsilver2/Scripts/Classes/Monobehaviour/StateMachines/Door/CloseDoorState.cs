using RedSilver2.Framework.Interactions;
using UnityEngine;

namespace RedSilver2.Framework.StateMachines.States
{
    public class CloseDoorState : UpdatableDoorState
    {
        [Space]
        [SerializeField] private Vector3 desiredPosition;
        [SerializeField] private Vector3 desiredRotation;

        private Vector3 currentPosition;
        private Vector3 currentRotation;

        private Transform handle;

        public const DoorStateType TYPE = DoorStateType.Close;

        protected sealed override void SetDoorStateType(ref DoorStateType type) {
            type = TYPE;
        }

        public sealed override bool CanTransition(DoorStateMachine stateMachine)
        {
            if (stateMachine == null) return false;
            return stateMachine.IsOpen;
        }

        protected override void OnEntered(DoorStateMachine stateMachine)
        {
            if (stateMachine != null)
            {
                handle = stateMachine.Handle;
                currentPosition = handle != null ? handle.localPosition : Vector3.zero;
                currentRotation = handle != null ? handle.localEulerAngles : Vector3.zero;
            }

            base.OnEntered(stateMachine);
        }

        protected override void OnExited(DoorStateMachine stateMachine)
        {
            base.OnExited(stateMachine);

            currentPosition = desiredPosition;
            currentRotation = desiredRotation;
            handle = null;
        }

        protected sealed override void OnProgressionUpdate(float progress)
        {
            if (handle != null)
            {
                handle.localPosition = Vector3.Lerp(currentPosition, desiredPosition, progress);
                handle.localRotation = Quaternion.Slerp(Quaternion.Euler(currentRotation), Quaternion.Euler(desiredRotation), progress);
            }
        }

        protected sealed override void OnUpdateCompleted()
        {
            if (handle != null)
            {
                handle.localPosition = desiredPosition;
                handle.localRotation = Quaternion.Euler(desiredRotation);
            }
        }

        public void SetDesiredRotation(Vector3 desiredRotation)
        {
            this.desiredRotation = desiredRotation;
        }

        public void SetDesiredPosition(Vector3 desiredPosition)
        {
            this.desiredPosition = desiredPosition;
        }

        public static CloseDoorState GetState(DoorStateMachine stateMachine)
        {
            if (stateMachine == null) return null;
            return stateMachine.GetState(TYPE) as CloseDoorState;
        }

    }
}
