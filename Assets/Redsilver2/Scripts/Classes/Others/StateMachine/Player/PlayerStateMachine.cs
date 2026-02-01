using RedSilver2.Framework.StateMachines.Controllers;
using RedSilver2.Framework.StateMachines.States;
using RedSilver2.Framework.StateMachines.States.Movement;
using UnityEngine;


namespace RedSilver2.Framework.StateMachines {
    [System.Serializable]
    public class PlayerStateMachine : StateMachine
    {
        public readonly PlayerMovementHandler MovementHandler;

        public PlayerStateMachine(PlayerController controller, PlayerMovementHandler movementHandler) : base(controller) {
            MovementHandler = movementHandler;
            movementHandler?.SetStateMachine(this);

            AddOnUpdateListener(() => {
                Debug.Log(" Is Crouching: " + MovementHandler.IsCrouching);
            });

            AddOnStateEnteredListener(state => {
                if(state != null)
                Debug.LogWarning("Current State: " + state.GetStateName());
            });

            AddOnStateExitedListener(state => {
                if (state != null)
                    Debug.LogWarning("Previous State: " + state.GetStateName());
            });
        }

        public void ChangeState(PlayerStateType stateType) {
            ChangeState(stateType.ToString());
        }

        public bool ContainsState(PlayerStateType stateType) {
            return ContainsState(stateType.ToString()); 
        }

        public void AddState(PlayerStateType stateType, State state) {
            AddState(stateType.ToString(), state);  
        }

        public sealed override void AddState(string stateName, State state) {
            if(state is PlayerState)  base.AddState(stateName, state);
        }

        public PlayerState GetState(PlayerStateType stateType) {
            return GetState(stateType.ToString()) as PlayerState; 
        }
    }
}