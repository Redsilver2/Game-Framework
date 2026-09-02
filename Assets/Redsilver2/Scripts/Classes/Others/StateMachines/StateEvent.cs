using RedSilver2.Framework.StateMachines.States;
using UnityEngine;
using UnityEngine.Events;

namespace RedSilver2.Framework.StateMachines.Events
{
    [System.Serializable]
    public abstract class StateEvent
    {
        [HideInInspector] public string name;
        [SerializeReference, HideInInspector] private State state;

        private bool isEnabled = false;

        public bool IsEnabled => isEnabled;

        protected StateEvent(string name, State state)  {
            this.name  = name;
            this.state = state;
            isEnabled  = false;
        }

        public void Enable() {
            if (!isEnabled) {
                Enable(state);
                isEnabled = true;
            }
        }

        public void Disable() {
            if (isEnabled)  {
                Disable(state);
                isEnabled = false;
            }
        }

        protected abstract void Enable(State state);
        protected abstract void Disable(State state);

        public bool IsOwner(State state)
        {
            return this.state == state;
        }

        public bool Compare(string eventName)
        {
            if(string.IsNullOrEmpty(eventName) || string.IsNullOrEmpty(name)) return false;
            return eventName.ToLower() == name.ToLower();
        }

        public virtual bool Compare(StateEvent _event)
        {
            if (_event == null) return false;
            return Compare(_event.name);
        }
    }
}
