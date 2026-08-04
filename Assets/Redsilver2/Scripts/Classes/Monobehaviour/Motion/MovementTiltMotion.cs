using UnityEngine;
using UnityEngine.Windows;

namespace RedSilver2.Framework.StateMachines.Events
{
    public abstract class MovementTiltMotion : MovementMotion
    {
        [Space]
        [SerializeField] private float defaultLerpSpeed;
       
        [Space]
        [SerializeField] private float directionUpdateSpeed;

        [Space]
        [SerializeField] private float rotationUpdateSpeed;

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

        protected sealed override void OnLateUpdate()
        {
            transform.localRotation = Quaternion.Slerp(transform.localRotation, Quaternion.Euler(desired), Time.deltaTime * rotationUpdateSpeed);
        }

        protected sealed override void OnInputUpdate(Vector2 vector)
        {
            UpdateRotation(vector, ref desired);
        }
        private void UpdateRotation(Vector2 input, ref Vector3 desired)
        {
            input.Normalize();
            desired.y = GetUpdatedRotation(-input.x, desired.y, original.y, MinY, MaxY);
            desired.x = GetUpdatedRotation(input.y, desired.x, original.x, MinX, MaxX);
            desired.z = original.z;
        }

        private float GetUpdatedRotation(float input, float current, float original,  float min, float max)
        {

            if (Mathf.Abs(input) > 0f) {
                current += Time.deltaTime * -Mathf.Sign(input) * directionUpdateSpeed;
                current = Mathf.Clamp(current, min, max);
            }
            else  current = Mathf.Lerp(current, 0f, Time.deltaTime * defaultLerpSpeed); 
            return current;
        }
    }

}
