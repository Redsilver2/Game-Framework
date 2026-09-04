using RedSilver2.Framework.Interactions;

namespace RedSilver2.Framework.StateMachines
{
    public class LockableDoorStateMachine : DoorStateMachine
    {
        private bool isLocked;
        public bool IsLocked => isLocked;


#if UNITY_EDITOR
        protected override void OnValidate()
        {
            base.OnValidate();
        }
#endif
        protected override void Awake()
        {
            base.Awake();
        }

        public void Lock()   {
            ChangeState(DoorStateType.Locked);

            if (!isLocked) {
                isLocked = true;
            }   
        }

        public void Unlock() {
            ChangeState(DoorStateType.Unlocked);
        }

        public sealed override void Open()
        {
            if (!isLocked) base.Open();
        }
    }
}
