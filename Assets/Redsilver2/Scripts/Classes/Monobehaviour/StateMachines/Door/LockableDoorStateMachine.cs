using RedSilver2.Framework.Interactions;
using RedSilver2.Framework.StateMachines.States;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.XR;

namespace RedSilver2.Framework.StateMachines
{
    public class LockableDoorStateMachine : DoorStateMachine
    {
        [Space]
        [SerializeField] private LockDoorState lockState;

        [Space]
        [SerializeField] private UnlockDoorState unlockState;

        private bool isLocked;

        public bool IsLocked => isLocked;
        public LockDoorState LockState => lockState;
        public UnlockDoorState UnlockState => unlockState;

#if UNITY_EDITOR
        protected override void OnValidate()
        {
            base.OnValidate();
        }
#endif
        protected override void Awake()
        {
            base.Awake();
            AddState(unlockState);
            AddState(lockState);
        }

        public void Lock()   {
            if (!isLocked) {
                ChangeState(lockState);
                isLocked = true;
            }   
        }

        public void Unlock() {
            if (isLocked) {
                ChangeState(unlockState);
                isLocked = false;
            }
        }

        public sealed override void Open()
        {
            if (!isLocked) base.Open();
        }

        public sealed override void ChangeState(DoorStateType type)
        {
            base.ChangeState(type);

            switch (type) {
                case DoorStateType.Locked:   ChangeState(lockState);   break;
                case DoorStateType.Unlocked: ChangeState(unlockState); break;
            }
        }

        public sealed override void AddState(DoorStateType type)
        {
            base.AddState(type);

            switch (type)  {
                case DoorStateType.Locked:   AddState(lockState); break;
                case DoorStateType.Unlocked: AddState(unlockState); break;
            }
        }

        public sealed override void RemoveState(DoorStateType type)
        {
            base.RemoveState(type);

            switch (type)
            {
                case DoorStateType.Locked:   RemoveState(lockState); break;
                case DoorStateType.Unlocked: RemoveState(unlockState); break;
            }
        }
    }
}
