using UnityEngine;
using UnityEngine.Events;

namespace RedSilver2.Framework.Player
{
    public abstract class PlayerStateModule : MonoBehaviour
    {

        [SerializeField] private UnityEvent onStateEnter;
        [SerializeField] private UnityEvent onStateExit;

        private PlayerStateMachine owner;

        private PlayerStateMachine.PlayerState state;
        public PlayerStateMachine.PlayerState State => state;

        private void Awake()
        {
            Setup(transform.root.GetComponent<PlayerController>());
        }

        private void OnDisable()
        {
            if (owner != null && state != null) owner.RemoveState(this);
        }

        private void OnEnable()
        {
            if (owner != null && state != null) owner.AddState(this);
        }

        private void Setup(PlayerController owner)
        {
            if(owner != null)
            {
                Setup(owner.StateMachine);
            }
        }

        private void Setup(PlayerStateMachine stateMachine)
        {
            if (stateMachine != null)
            {
                state = GetState(stateMachine);
                if(enabled) stateMachine.AddState(this);
                Setup(state);
            }
        }

        protected virtual void Setup(PlayerStateMachine.PlayerState state)
        {
            if (state != null)
            {
                state.AddOnEnterStateListener(OnStateEnter);
                state.AddOnExitStateListener(OnStateExit);
            }
        }

        private void OnStateEnter()
        {
           if(onStateEnter != null) onStateEnter.Invoke();
        }

        private void OnStateExit()
        {
            if(onStateExit != null) onStateExit.Invoke();   
        }

        protected abstract PlayerStateMachine.PlayerState GetState(PlayerStateMachine owner);
    }
}
