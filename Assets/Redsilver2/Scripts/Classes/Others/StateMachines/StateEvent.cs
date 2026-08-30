using RedSilver2.Framework.StateMachines.States;
using UnityEngine;
using UnityEngine.Events;

namespace RedSilver2.Framework.StateMachines.Events
{
    [System.Serializable]
    public abstract class StateEvent
    {
        [HideInInspector] public string name;

        private bool isEnabled = false;
        public bool IsEnabled => isEnabled;

        protected StateEvent()  { }


        public abstract void Add(State state, StateMachine stateMachine);
        public abstract void Remove(State state, StateMachine stateMachine);


        public virtual bool IsValid(State state, StateMachine stateMachine) {
            return state != null && stateMachine != null;
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
