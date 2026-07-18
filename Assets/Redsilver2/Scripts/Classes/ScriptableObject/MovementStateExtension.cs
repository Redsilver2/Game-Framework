using RedSilver2.Framework.StateMachines.Handlers;
using UnityEngine;

public abstract class MovementStateExtension : MonoBehaviour
{
    protected MovementStateMachineEventHandler eventHandler;

    private void Awake()
    {
        eventHandler = transform.root != null ? transform.root.GetComponentInChildren<MovementStateMachineEventHandler>()
                                                : GetComponentInChildren<MovementStateMachineEventHandler>();
    }

    protected abstract void Start();    
    protected abstract void OnDisable();
    protected abstract void OnEnable();

    public void SetEventHandler(MovementStateMachineEventHandler eventHandler) {
        if(eventHandler != this.eventHandler) {
            this.eventHandler = eventHandler;
        }
    }
}
