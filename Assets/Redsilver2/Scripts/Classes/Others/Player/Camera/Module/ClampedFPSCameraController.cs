using RedSilver2.Framework.Player;
using UnityEngine;

public class ClampedFPSCameraController : FPSCameraController
{
    [Space]
    [SerializeField] private float minRotationY = -45f;
    [SerializeField] private float maxRotationY = 45f;

    protected override void UpdateBodyRotation(Transform body, ref float rotation)
    {
        base.UpdateBodyRotation(body, ref rotation);
        rotation = Mathf.Clamp(rotation, minRotationY, maxRotationY);
    }
}
