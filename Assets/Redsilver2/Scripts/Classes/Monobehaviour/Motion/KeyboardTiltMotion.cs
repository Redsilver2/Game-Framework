using UnityEngine;

public class KeyboardTiltMotion : KeyboardMotion
{
    [Space]
    [SerializeField] private float directionUpdateSpeed;

    protected override void Start()
    {
        SetOriginal(transform.localEulerAngles);
        base.Start();
    }

    protected sealed override void OnLateUpdate(Vector3 desired) {
        // Fix Rotation Bug...
        transform.localEulerAngles = new Vector3(desired.x, desired.y, desired.z);
    }

    protected sealed override void OnUpdate(ref Vector3 desired) {
        UpdateRotation(ref desired);
    }

    private void UpdateRotation(ref Vector3 desired)
    {
        float x, y;

        if (Mathf.Abs(Input.x) > 0f) { 
               x = GetAxis(desired.z + (Time.deltaTime * -Mathf.Sign(Input.x) * directionUpdateSpeed), MinX, MaxX); }
        else { x = Mathf.Lerp(desired.z, Original.z, Time.deltaTime * DefaultLerpSpeed); }

        if (Mathf.Abs(Input.y) > 0f) { 
            y = GetAxis(desired.x + (Time.deltaTime * Mathf.Sign(Input.y) * directionUpdateSpeed), MinY, MaxY); 
        }
        else { y = Mathf.Lerp(desired.x, Original.x, Time.deltaTime * DefaultLerpSpeed); }

        desired = Vector3.right * y + Vector3.up * Original.y + Vector3.forward * x;
    }

    private float GetAxis(float value, float min, float max)
    {
        return Mathf.Clamp(value, min, max);
    }
}
