using RedSilver2.Framework.StateMachines;
using UnityEngine;
using UnityEngine.Events;

public abstract class StateMachineController : MonoBehaviour
{
    private StateMachine stateMachine;
    private UnityEvent<StateMachine> onStateMachineChanged;

    public StateMachine StateMachine => stateMachine;

    protected virtual void Awake()
    {
        onStateMachineChanged = new UnityEvent<StateMachine>();
    }

    public virtual void SetStateMachine(StateMachine stateMachine) {
        this.stateMachine = stateMachine;
    }

    private void OnDisable() { stateMachine?.Disable(); }
    private void OnEnable()  { stateMachine?.Enable();  }

    public void AddOnStateMachineChanged(UnityAction<StateMachine> action){
        if (action != null)
            onStateMachineChanged?.AddListener(action);
    }

    public void RemoveOnStateMachineChanged(UnityAction<StateMachine> action)
    {
        if(action != null)
            onStateMachineChanged?.RemoveListener(action); 
    }


    public bool IsStateMachineNull() { return stateMachine == null; }

}
