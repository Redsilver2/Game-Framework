using RedSilver2.Framework.StateMachines;
using UnityEngine;

public abstract class StateMachineController : MonoBehaviour
{
    private StateMachine stateMachine;
    public StateMachine StateMachine => stateMachine;

    public virtual void SetStateMachine(StateMachine stateMachine) {
         this.stateMachine = stateMachine;
    }

    private void OnDisable() { stateMachine?.Disable(); }
    private void OnEnable() { stateMachine?.Enable(); }

}
