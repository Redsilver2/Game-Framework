using UnityEngine;
using UnityEngine.Events;


namespace RedSilver2.Framework.Player
{
    public partial class PlayerStateMachine 
    {
        public abstract class UpdateablePlayerState : PlayerState
        {
            private UnityEvent onUpdate;
            private UnityEvent onLateUpdate;

            protected UpdateablePlayerState() { }

            protected UpdateablePlayerState(PlayerStateMachine owner) : base(owner)
            {
                onUpdate     = new UnityEvent();
                onLateUpdate = new UnityEvent();
            }

            private void Update()
            {
                onUpdate.Invoke();
            }

            private void LateUpdate()
            {
                onLateUpdate.Invoke();  
            }

            protected override void OnStateEnter()
            {
                base.OnStateEnter();

                if(owner != null)
                {
                    owner.AddOnUpdateListener(Update);
                    owner.AddOnLateUpdateListener(LateUpdate);
                }
            }

            protected override void OnStateExit()
            {
                base.OnStateExit();

                if (owner != null)
                {
                    owner.RemoveOnUpdateListener(Update);
                    owner.RemoveOnLateUpdateListener(LateUpdate);
                }
            }

            public void AddOnUpdateListener(UnityAction action)
            {
                if(onUpdate != null && action != null) onUpdate.AddListener(action);
            }

            public void RemoveOnUpdateListener(UnityAction action)
            {
                if (onUpdate != null && action != null) onUpdate.RemoveListener(action);
            }


            public void AddOnLateUpdateListener(UnityAction action)
            {
                if (onLateUpdate != null && action != null) onLateUpdate.AddListener(action);
            }

            public void RemoveOnLateUpdateListener(UnityAction action)
            {
                if (onLateUpdate != null && action != null) onLateUpdate.AddListener(action);
            }
        }
    }
}
