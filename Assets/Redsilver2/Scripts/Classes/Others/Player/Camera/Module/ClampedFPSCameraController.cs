using RedSilver2.Framework.Player;
using UnityEngine;

[System.Serializable]
public partial class ClampedFPSCameraController : FPSCameraController
{
    [Space]
    [SerializeField] private float minBodyRotation = -45f;
    [SerializeField] private float maxBodyRotation = 45f;

    [Space]
    [SerializeField] private bool canLerpBodyRotation;
    [SerializeField] private float bodyRotationReturnSpeed;

    public ClampedFPSCameraController() : base()
    {

    }

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

    protected override void Update(Camera camera)
    {
        minBodyRotation = Mathf.Clamp(minBodyRotation, float.MinValue, 0f);
        maxBodyRotation = Mathf.Clamp(maxBodyRotation, 0f, float.MaxValue);
        base.Update();
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

public partial class ClampedFPSCameraController : FPSCameraController
{
#if UNITY_EDITOR
    protected override void ShowBaseSettings(Color foldoutColor, Color buttonColor, Color backgroundColor)
    {
        base.ShowBaseSettings(foldoutColor, buttonColor, backgroundColor);

        EditorExtension.Space(10f);
        minBodyRotation = EditorExtension.DisplayFloatSlider("Min Body Rotation", minBodyRotation, 0f, 100f);
        maxBodyRotation = EditorExtension.DisplayFloatSlider("Max Body Rotation", maxBodyRotation, 0f, 100f);

        EditorExtension.Space(10f);
        canLerpBodyRotation     = EditorExtension.DisplayToggle("Can Lerp Body Rotation", canLerpBodyRotation);
        bodyRotationReturnSpeed = EditorExtension.DisplayFloatSlider("Body Rotation Return Speed", bodyRotationReturnSpeed, 0f, 100f);
#endif
    }
}
