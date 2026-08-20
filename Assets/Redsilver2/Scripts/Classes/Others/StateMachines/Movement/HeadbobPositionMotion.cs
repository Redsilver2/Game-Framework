using RedSilver2.Framework.StateMachines.Extensions;
using RedSilver2.Framework.StateMachines.States;
using UnityEngine;


namespace RedSilver2.Framework.StateMachines
{
    [System.Serializable]
    public class HeadbobPositionMotion : PlayerMovementStateMotion
    {


        [Space]
        [SerializeField] private float transformUpdateSpeed;
        [SerializeField] private float updateSpeed;
        [SerializeField] private float resetUpdateSpeed;


        [Space]
        [SerializeField] private Vector2 originalPosition;

        [Space]
        [SerializeField] private Vector2 minPosition;
        [SerializeField] private Vector2 maxPosition;


        private float minX;
        private float minY;

        private float maxX;
        private float maxY;

        public readonly bool CanAutomaticallyUpdate;

        private Vector2 desiredPosition;

        public float MinX => minX;
        public float MaxX => maxX;

        public float MinY => minY;
        public float MaxY => maxY;

        public  Vector3 OriginalPosition => originalPosition;

        public HeadbobPositionMotion(MovementStateType Type, bool canAutomaticallyUpdate) : base(Type) {
            this.CanAutomaticallyUpdate = canAutomaticallyUpdate;
        }

        public void SetOriginalPosition(Vector3 originalPosition)
        {
            this.originalPosition = originalPosition;
        }

        protected sealed override void OnStateLateUpdate(Transform transform) {
            if (transform != null) transform.localPosition = Vector3.Lerp(transform.localPosition, Vector3.right * desiredPosition.x + Vector3.up * desiredPosition.y + Vector3.forward * 0f, Time.deltaTime * transformUpdateSpeed);
        }

        protected sealed override void OnStateUpdate(Transform transform, Vector2 input, bool is2DMovement) {
            minX = originalPosition.x - minPosition.x;
            maxX = originalPosition.x + maxPosition.x;

            minY = originalPosition.y - minPosition.y;
            maxY = originalPosition.y + maxPosition.y;

            if ((input.magnitude > 0f || CanAutomaticallyUpdate) && IsEnabled && !is2DMovement) OnStateUpdate(true);
            else OnStateUpdate(false);
        }

        private void OnStateUpdate(bool canUpdate) {
            if (canUpdate) {
                float sin = Mathf.Sin(Time.time * updateSpeed);

                desiredPosition.x = Mathf.Lerp(originalPosition.x, sin < 0f ? minX : maxX, Mathf.Abs(sin));
                desiredPosition.y = Mathf.Lerp(originalPosition.y, sin < 0f ? minY : maxY, Mathf.Abs(sin));
            }
            else {
                desiredPosition.x = Mathf.Lerp(desiredPosition.x, originalPosition.x, Time.deltaTime * resetUpdateSpeed);
                desiredPosition.y = Mathf.Lerp(desiredPosition.y, originalPosition.y, Time.deltaTime * resetUpdateSpeed);
            }
        }

        protected override void SetMovementType(ref MovementStateType type) {
            type = WalkState.TYPE;
        }
    }
}
