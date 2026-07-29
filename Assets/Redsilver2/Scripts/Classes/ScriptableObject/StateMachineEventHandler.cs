using UnityEngine;
using UnityEngine.Events;

namespace RedSilver2.Framework.StateMachines.Handlers
{
    public abstract class StateMachineEventHandler : MonoBehaviour {

        private   StateMachine controller;
     
        protected virtual void Awake()
        {
            controller = transform.root == null ? GetComponentInChildren<StateMachine>()
                                      : transform.root.GetComponentInChildren<StateMachine>();
        }

        protected virtual void Start() {
        }

       // private void OnEnable()  { controller?.AddOnStateMachineChangedListener(SetStateMachine); }
     //   private void OnDisable() { controller?.RemoveOnStateMachineChangedListener(SetStateMachine); }

        //protected virtual void SetStateMachine(StateMachine stateMachine) {
        //   // this.stateMachine = stateMachine;   
        //}
    }
}
