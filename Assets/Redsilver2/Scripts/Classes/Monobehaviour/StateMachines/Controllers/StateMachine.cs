using UnityEngine;
using UnityEngine.Events;

public abstract class StateMachine : MonoBehaviour
{
    private UnityEvent onEnabled, onDisabled;

    protected virtual void Awake() {

        onEnabled  = new UnityEvent();
        onDisabled = new UnityEvent();

    }

    private void OnDisable() { onDisabled?.Invoke(); }
    private void OnEnable()  { onEnabled?.Invoke();  }

    public void AddOnEnabledListener(UnityAction action) {
        if (action != null) onEnabled?.AddListener(action);
    }
    public void RemoveOnEnabledListener(UnityAction action)
    {
        if (action != null) onEnabled?.RemoveListener(action);
    }

    public void AddOnDisabledListener(UnityAction action)
    {
        if (action != null) onDisabled?.AddListener(action);
    }
    public void RemoveOnDisabledListener(UnityAction action)
    {
        if (action != null) onDisabled?.RemoveListener(action);
    }
}
