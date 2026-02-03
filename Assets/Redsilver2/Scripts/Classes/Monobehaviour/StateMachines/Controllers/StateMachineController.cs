using RedSilver2.Framework.StateMachines.States;
using UnityEngine;

namespace RedSilver2.Framework.StateMachines.Controllers
{
    public abstract class StateMachineController : MonoBehaviour
    {
        private StateMachine stateMachine;
        public StateMachine StateMachine => stateMachine;

        protected virtual void Awake() {
            InitializeStateMachine(ref stateMachine);
        }

        private void Update() {
            stateMachine?.Update();
        }

        private void LateUpdate() {
            stateMachine?.LateUpdate();
        }

        private void OnDisable() {
            stateMachine?.Disable();
        }

        private void OnEnable() {
            stateMachine?.Enable();
        }

        protected virtual void AddState(State state) {
            if (state != null)
              stateMachine?.AddState(state.GetStateName(), state);
        }

        public void RemoveState(State state) {
            if(state != null)
              stateMachine?.RemoveState(state.GetStateName());
        }

        protected abstract void InitializeStateMachine(ref StateMachine stateMachine);
    }

}
