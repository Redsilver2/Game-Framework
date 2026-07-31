using UnityEngine;
using UnityEngine.Events;

namespace RedSilver2.Framework.StateMachines.Events
{
    public abstract class StateMachineEvent : MonoBehaviour {

        private StateMachine stateMachine;
     
        protected virtual void Awake()
        {
             stateMachine = transform.root == null ? GetComponentInChildren<StateMachine>()
                                                   : transform.root.GetComponentInChildren<StateMachine>();
        }

        protected virtual void Start()
        {
            SetStateMachineEvents(stateMachine, true);
        }

        private void OnEnable()  { SetStateMachineEvents(stateMachine, true); }
        private void OnDisable() { SetStateMachineEvents(stateMachine, false); }
        protected abstract void SetStateMachineEvents(StateMachine stateMachine, bool isAddingEvents);
    }
}
