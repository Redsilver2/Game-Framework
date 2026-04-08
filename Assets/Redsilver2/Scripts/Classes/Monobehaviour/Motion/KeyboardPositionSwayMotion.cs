using UnityEngine;

public class KeyboardPositionSwayMotion : KeyboardMotion
{
    protected override void Start()
    {
        SetOriginal(transform.localPosition);
        base.Start();
    }


    protected sealed override void OnLateUpdate(Vector3 desired) {
        transform.localPosition = desired;
    }

    protected sealed override void OnUpdate(ref Vector3 desired)
    {
        float x = Original.x, y = Original.y;
        
        if (Input.magnitude > 0f) 
            UpdatePosition(ref x, ref y);

        float nextPositionX = Mathf.Lerp(desired.x, x, Time.deltaTime * DefaultLerpSpeed);
        float nextPositionY = Mathf.Lerp(desired.y, y, Time.deltaTime * DefaultLerpSpeed);

        desired = Vector3.right * nextPositionX + Vector3.up * nextPositionY + Vector3.forward * Original.z;  
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
