using UnityEngine;

public abstract class KeyboardMotion : Vector2KeyboardConfigurationEvent
{
    [Space]
    [SerializeField] private float defaultLerpSpeed;

    [Space]
    [SerializeField] private Vector2 min;
    [SerializeField] private Vector2 max;

    private Vector3 original;
    private Vector3 desired;

    protected float DefaultLerpSpeed => defaultLerpSpeed;
    protected float MinX => original.x - min.x;
    protected float MaxX => original.x + max.x;
    protected float MinY => original.y - min.y;
    protected float MaxY => original.y + max.y;
    protected Vector3 Original => original;

    public void SetOriginal(Vector3 localPosition) {
         this.original = localPosition;
    }

    public void SetMinPosition(Vector2 minPosition)
    {
        this.min = minPosition;
    }

    public void SetMaxPosition(Vector2 maxPosition)
    {
        this.max = maxPosition;
    }

    protected sealed override void OnLateUpdate() {

        OnLateUpdate(desired);
    }

    protected sealed override void OnUpdate(Vector2 vector)
    {
        base.OnUpdate(vector);
        OnUpdate(ref desired);
    }

    protected abstract void OnLateUpdate(Vector3 desired);   
    protected abstract void OnUpdate(ref Vector3 desired);
}
