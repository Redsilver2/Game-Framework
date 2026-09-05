using UnityEngine;
using UnityEngine.Events;

namespace RedSilver2.Framework.StateMachines.Events
{
    [System.Serializable]
    public abstract partial class StateMachineEvent {
        [SerializeField, HideInInspector]                     private string name;
        [SerializeField, SerializeReference, HideInInspector] private StateMachine stateMachine;

        private bool isEnabled;
        public string Name => name;

        protected StateMachineEvent(string name, StateMachine stateMachine) { 
            this.name         = name;
            this.stateMachine = stateMachine;
            isEnabled = false;
        }

        public void Enable()  {
            if(!isEnabled && Application.isPlaying) {
                Enable(stateMachine);
                isEnabled = true;
            }
        }

        public void Disable() {
            if(isEnabled && Application.isPlaying) {
                Disable(stateMachine);
                isEnabled = false;
            }
        }

        protected abstract void Enable(StateMachine stateMachine);
        protected abstract void Disable(StateMachine stateMachine);
    }
    public abstract partial class StateMachineEvent
    {
#if UNITY_EDITOR
        public void DrawInspector() { }
#endif
    }
}
