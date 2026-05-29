using UnityEngine;

namespace RedSilver2.Framework.Player
{
    public class FPSCameraController : CameraController
    {
        [Space]
        [SerializeField] private float minHeadRotation = -45f;
        [SerializeField] private float maxHeadRotation = 45f;

        [Space]
        [SerializeField] private bool  canLerpHeadRotation;
        [SerializeField] private float headRotationReturnSpeed;

        public void SetMinHeadRotation(float minHeadRotation)
        {
            maxHeadRotation = Mathf.Clamp(maxHeadRotation, float.MaxValue, 0f);
            this.minHeadRotation = minHeadRotation;
        }

        public void SetMaxHeadRotation(float maxHeadRotation)
        {
            maxHeadRotation = Mathf.Clamp(maxHeadRotation, 0f, float.MaxValue);
            this.maxHeadRotation = maxHeadRotation;

        }

        public void SetCanLerpHeadRotation(bool canLerpHeadRotation)
        {
            this.canLerpHeadRotation = canLerpHeadRotation;
        }


        public void SetHeadRotationReturnSpeed(float headRotationReturnSpeed)
        {
            this.headRotationReturnSpeed = headRotationReturnSpeed;
        }

        protected sealed override void UpdateHeadRotation(Transform head)
        {
            base.UpdateHeadRotation(head);

            if (!canLerpHeadRotation) {
                headRotation = Mathf.Clamp(headRotation, minHeadRotation, maxHeadRotation);
            }
            else  {
                if (headRotation > maxHeadRotation || headRotation < minHeadRotation)      {
                    ReturnRotation(headRotation > maxHeadRotation ? maxHeadRotation: minHeadRotation, 
                                   minHeadRotation, maxHeadRotation, headRotationReturnSpeed, ref headRotation); 
                }
            }
        }

        protected override void OnUpdate(Vector2 vector)
        {
            minHeadRotation = Mathf.Clamp(minHeadRotation, float.MinValue, 0f);
            maxHeadRotation = Mathf.Clamp(maxHeadRotation, 0f, float.MaxValue);
            base.OnUpdate(vector);

        }

        protected void ReturnRotation(float value, float minValue, float maxValue, float rotationSpeed, ref float rotation)
        {
            value = Mathf.Clamp(value, minValue, maxValue);
            rotation = Mathf.Lerp(rotation, value, Time.deltaTime * rotationSpeed);
        }
    }
}
