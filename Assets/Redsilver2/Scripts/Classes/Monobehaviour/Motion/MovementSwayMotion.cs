using RedSilver2.Framework.StateMachines.Controllers;
using RedSilver2.Framework.StateMachines.Events;
using UnityEngine;

public class MovementSwayMotion : MovementMotion
{
    [SerializeField] private float defaultLerpSpeed;
    [SerializeField] private float positionUpdateSpeed;
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

    public void SetOriginal(Vector3 localPosition)
    {
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

    protected sealed override void SetStateMachineEvents(PlayerMovementStateMachine stateMachine, bool isAddingEvents)
    {
        if (isAddingEvents) {
            stateMachine?.AddOnLateUpdateListener(OnLateUpdate);
            stateMachine?.AddOnMoveInputUpdateListener(OnInputUpdate);
        }
        else {
            stateMachine?.RemoveOnLateUpdateListener(OnLateUpdate);
            stateMachine?.RemoveOnMoveInputUpdateListener(OnInputUpdate);
        }
    }

    protected sealed override void OnLateUpdate()  {
        transform.localPosition = Vector3.Lerp(transform.localPosition, desired, Time.deltaTime * positionUpdateSpeed);
    }

    protected sealed override void OnInputUpdate(Vector2 vector) {
        OnUpdate(vector, ref desired);
    }



    protected void OnUpdate(Vector3 movementVector, ref Vector3 desired)
    {
        float x = Original.x, y = Original.y;
        movementVector.Normalize();

        if (movementVector.magnitude > 0f) UpdatePosition(ref x, ref y);

        desired.x = Mathf.Lerp(desired.x, x, Time.deltaTime);
        desired.y = Mathf.Lerp(desired.y, y, Time.deltaTime);
        desired.z = Original.z;
    }

    protected virtual void UpdatePosition(ref float x, ref float y) 
    {
        float sin    = Mathf.Sin(Time.time * DefaultLerpSpeed);
        float absSin = Mathf.Abs(sin);

        if (sin < 0f) {
            y = Mathf.Lerp(Original.y, MinY, absSin);
            x = Mathf.Lerp(Original.x, MinX, absSin);
        }
        else if (sin > 0f)  {
            y = Mathf.Lerp(Original.y, MaxY, absSin);
            x = Mathf.Lerp(Original.x, MaxX, absSin);
        }
    }
}
