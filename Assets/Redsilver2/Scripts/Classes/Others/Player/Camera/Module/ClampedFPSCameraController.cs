using RedSilver2.Framework.Player;
using UnityEngine;

public class ClampedFPSCameraController : FPSCameraController
{
    [Space]
    [SerializeField] private float minBodyRotation = -45f;
    [SerializeField] private float maxBodyRotation = 45f;

    [Space]
    [SerializeField] private bool canLerpBodyRotation;
    [SerializeField] private float bodyRotationReturnSpeed;


    public void SetMinBodyRotation(float minBodyRotation)
    {
        minBodyRotation = Mathf.Clamp(minBodyRotation, float.MaxValue, 0f);
        this.minBodyRotation = minBodyRotation;
    }

    public void SetMaxBodyRotation(float maxBodyRotation)
    {
        maxBodyRotation = Mathf.Clamp(maxBodyRotation, 0f, float.MaxValue);
        this.maxBodyRotation = maxBodyRotation;
    }

    public void SetCanLerpBodyRotation(bool canLerpBodyRotation)
    {
        this.canLerpBodyRotation = canLerpBodyRotation;
    }


    public void SetBodyRotationReturnSpeed(float bodyRotationReturnSpeed) {
        this.bodyRotationReturnSpeed = bodyRotationReturnSpeed;
    }

    protected override void OnUpdate(Vector2 vector)
    {
        minBodyRotation = Mathf.Clamp(minBodyRotation, float.MinValue, 0f);
        maxBodyRotation = Mathf.Clamp(maxBodyRotation, 0f, float.MaxValue);
        base.OnUpdate(vector);
    }

    protected override void UpdateBodyRotation(Transform body)
    {
        base.UpdateBodyRotation(body);

        if (!canLerpBodyRotation) {
            bodyRotation = Mathf.Clamp(bodyRotation, minBodyRotation, maxBodyRotation);
        }
        else
        {
            if (bodyRotation > maxBodyRotation || bodyRotation < minBodyRotation) {
                ReturnRotation(bodyRotation > maxBodyRotation ? maxBodyRotation : minBodyRotation,
                               minBodyRotation, maxBodyRotation, bodyRotationReturnSpeed, ref bodyRotation);
            }
        }
    }

}
