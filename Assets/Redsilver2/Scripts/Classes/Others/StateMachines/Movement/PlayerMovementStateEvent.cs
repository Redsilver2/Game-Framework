using RedSilver2.Framework.StateMachines.Controllers;
using RedSilver2.Framework.StateMachines.States;
using UnityEngine;

namespace RedSilver2.Framework.StateMachines.Extensions
{
    [System.Serializable]
    public abstract class PlayerMovementStateEvent
    {
        private bool isEnabled;
        private PlayerMovementStateMachine stateMachine;
        
        public readonly MovementStateType Type;
        public bool IsEnabled => isEnabled;

        protected PlayerMovementStateEvent(MovementStateType Type) {
            this.Type = Type;
            isEnabled = false;
        }

        public void Enable() {
            if (!isEnabled) isEnabled = true;
        }

        public void Disable() {
            if (isEnabled) isEnabled = false;  
        }   

        public void SetStateMachine(PlayerMovementStateMachine stateMachine) {
            if(stateMachine != this.stateMachine) {
                Debug.Log("+++: " + stateMachine);

                this.stateMachine?.RemoveOnStateEnteredListener(OnStateEntered);
                this.stateMachine?.RemoveOnStateExitedListener(OnStateExited);
                OnStateExited(this.stateMachine != null ? this.stateMachine.CurrentState as MovementState : null);

                this.stateMachine = stateMachine;

                this.stateMachine?.AddOnStateEnteredListener(OnStateEntered);
                this.stateMachine?.AddOnStateExitedListener(OnStateExited);
                OnStateEntered(this.stateMachine != null ? this.stateMachine.CurrentState as MovementState : null);
            }
        }



#if UNITY_EDITOR
        public virtual void Validate() { }
#endif


        private void OnStateEntered(MovementState state) {
            if (state == null || state.Type != Type) return;
            Debug.Log(state);
            AddStateEvents(state);
        }

        private void OnStateExited(MovementState state) {
            if (state == null || state.Type != Type) return;
            RemoveStateEvents(state);
        }

        protected virtual void AddStateEvents(MovementState state) {
            state?.AddOnUpdateListener(OnStateUpdate);
            state?.AddOnLateUpdateListener(OnStateLateUpdate);
        }

        protected virtual void RemoveStateEvents(MovementState state) {
            state?.RemoveOnUpdateListener(OnStateUpdate);
            state?.RemoveOnLateUpdateListener(OnStateLateUpdate);
        }

        private void OnStateUpdate(){
            OnStateUpdate(stateMachine);       
        }

        protected abstract void OnStateUpdate(PlayerMovementStateMachine stateMachine);
        protected abstract void OnStateLateUpdate(); 
        protected abstract void SetMovementType(ref MovementStateType type);
    }
}
