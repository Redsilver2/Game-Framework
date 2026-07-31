using RedSilver2.Framework.StateMachines.Controllers;
using UnityEngine;

namespace RedSilver2.Framework.StateMachines.Events
{
    public class PlayerMovementTiltMotion : PlayerMovementMotion
    {
        [Space]
        [SerializeField] private float defaultLerpSpeed;

        [Space]
        [SerializeField] private Vector2 min;
        [SerializeField] private Vector2 max;

        [Space]
        [SerializeField] private float directionUpdateSpeed;

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

        protected sealed override void OnLateUpdate()
        {
            transform.localRotation = Quaternion.Euler(desired);
        }

        protected sealed override void OnMoveInputUpdate(Vector2 vector)
        {
            UpdateRotation(vector, ref desired);
        }
        private void UpdateRotation(Vector2 input, ref Vector3 desired)
        {
            float x, y;
            input.Normalize();

            if (Mathf.Abs(input.x) > 0f) {
                x = GetAxis(desired.z + (Time.deltaTime * -Mathf.Sign(input.x) * directionUpdateSpeed), MinX, MaxX);
            }
            else {
                x = Mathf.Lerp(desired.z, Original.z, Time.deltaTime * DefaultLerpSpeed);
            }

            if (Mathf.Abs(input.y) > 0f) {
                y = GetAxis(desired.x + (Time.deltaTime * -Mathf.Sign(input.y) * directionUpdateSpeed), MinY, MaxY);
            }
            else {
                y = Mathf.Lerp(desired.x, Original.x, Time.deltaTime * DefaultLerpSpeed);
            }

            desired = Vector3.right * y + Vector3.up * Original.y + Vector3.forward * x;
        }

        private float GetAxis(float value, float min, float max) {
            return Mathf.Clamp(value, min, max);
        }

    }
}
