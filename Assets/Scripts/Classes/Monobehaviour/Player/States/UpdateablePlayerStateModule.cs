using UnityEngine;
using UnityEngine.Events;

namespace RedSilver2.Framework.Player
{
    public abstract class UpdateablePlayerStateModule : PlayerStateModule
    {
        [SerializeField] private UnityEvent onUpdate;
        [SerializeField] private UnityEvent onLateUpdate;

        protected override void Setup(PlayerStateMachine.PlayerState state)
        {
            PlayerStateMachine.UpdateablePlayerState updateable = state as PlayerStateMachine.UpdateablePlayerState;
            base.Setup(state);

            if(updateable != null)
            {
                updateable.AddOnLateUpdateListener(OnLateUpdate);
                updateable.AddOnUpdateListener(OnUpdate);
            }
        }

        private void OnUpdate()
        {
            if(onUpdate != null) onUpdate.Invoke();
        }

        private void OnLateUpdate()
        {
            if (onLateUpdate != null) onLateUpdate.Invoke(); 
        }

    }
}
