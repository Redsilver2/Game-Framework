using RedSilver2.Framework.Inputs.Configurations;
using UnityEngine;

public abstract class Vector2InputConfigurationEvent : MonoBehaviour
{
    private Vector2 input;
    public Vector2 Input => input;

    protected virtual void Start()
    {
        GetConfiguration()?.Enable();
        SetEvent(true);
    }

    protected abstract void OnLateUpdate();
    protected virtual void OnUpdate(Vector2 vector) {
        input = vector;
    }

    private  void SetEvent(bool isAddingEvent)
    {
        SetEvent(isAddingEvent, GetConfiguration());
    }

    protected virtual async void SetEvent(bool isAddingEvent, Vector2InputConfiguration configuration)
    {
        if (isAddingEvent) {
            configuration?.AddOnUpdateListener(OnUpdate);
            configuration?.AddOnLateUpdateListener(OnLateUpdate);
        }
        else {
            configuration.RemoveOnUpdateListener(OnUpdate);
            configuration?.RemoveOnLateUpdateListener(OnLateUpdate);
        }
    }

    protected virtual void OnDisable()
    {
        if (!didAwake) return;
       SetEvent(false);
    }

    protected virtual void OnEnable() {
        if (!didAwake) return;
        SetEvent(true);
    }

    protected abstract Vector2InputConfiguration GetConfiguration();
}
