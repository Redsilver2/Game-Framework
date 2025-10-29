
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace RedSilver2.Framework.Player
{
    public partial class PlayerStateMachine
    {
        private PlayerController owner;
        public readonly CharacterController character;
        public readonly bool isInitialized = false;

        private PlayerState currentState;

        private UnityEvent onUpdate;
        private UnityEvent onLateUpdate;

        private UnityEvent<PlayerState> onStateAdded;
        private UnityEvent<PlayerState> onStateRemoved;

        private UnityEvent<PlayerState> onStateEnter;
        private UnityEvent<PlayerState> onStateExit;

        private List<PlayerExtension>                               extensions;
        private Dictionary<string, PlayerState>                     states;
        private Dictionary<string, PlayerStateTransitionCondition>  transitionConditions;

        private PlayerStateMachine() { }

        public PlayerStateMachine(PlayerController owner)
        {
            SetOwner(owner, ref this.owner, ref this.character);
            SetArrays();

            SetEvents();
            AddEvents();

            isInitialized = true;
        }

        private void SetOwner(PlayerController current, ref PlayerController owner, ref CharacterController character)
        {
            if (current != null) { 
            
                owner = current;
                character = owner.GetComponent<CharacterController>();
            }
        }

        private void SetEvents()
        {
            onUpdate       = new UnityEvent();
            onLateUpdate   = new UnityEvent();

            onStateEnter   = new UnityEvent<PlayerState>();
            onStateExit    = new UnityEvent<PlayerState>();

            onStateAdded   = new UnityEvent<PlayerState>();
            onStateRemoved = new UnityEvent<PlayerState>();
        }

        protected virtual void AddEvents()
        {
            AddOnStateEnterListener(OnStateEnter);
            AddOnStateExitListener(OnStateExit);

            AddOnStateAddedListener(OnStateAdded);
            AddOnStateRemovedListener(OnRemoveState);

            MoveState.SetDefaultMovementInputEvent(this);
            JumpState.SetDefaultJumpInputEvents(this);
        }

        private void SetArrays()
        {
            extensions           = new List      <PlayerExtension>();
            states               = new Dictionary<string, PlayerState>();
            transitionConditions = new Dictionary<string, PlayerStateTransitionCondition>();
        }

        public void Update()
        {
            if(onUpdate != null) onUpdate.Invoke();
        }
        
        public void LateUpdate()
        {
            if(onLateUpdate != null) onLateUpdate.Invoke();
        }

        public void AddOnUpdateListener(UnityAction action)
        {
            if (onUpdate != null && action != null) onUpdate.AddListener(action);
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
            if (onLateUpdate != null && action != null) onLateUpdate.RemoveListener(action);
        }


        public void ChangeState(string stateName)
        {
            PlayerState nextState = GetState(stateName);
            
            if (nextState != currentState)
            {
                if (onStateExit != null)  onStateExit.Invoke(currentState);
                if (onStateEnter != null) onStateEnter.Invoke(nextState);
            }
        }

        private void OnStateEnter(PlayerState state)
        {
            if (state != null) state.Enter();
            currentState = state;
        }

        private void OnStateExit(PlayerState state)
        {
            if (state != null) state.Exit(); 
            currentState = null;
        }
    }

}