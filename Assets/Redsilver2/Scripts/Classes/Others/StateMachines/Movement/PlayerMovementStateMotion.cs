using RedSilver2.Framework.Player;
using RedSilver2.Framework.StateMachines.Controllers;
using RedSilver2.Framework.StateMachines.States;
using UnityEngine;
using UnityEngine.Windows;

namespace RedSilver2.Framework.StateMachines.Extensions
{
    [System.Serializable]
    public sealed class PlayerMovementStateMotion : PlayerMovementStateEvent {
        [Space]
        [SerializeField] private Transform transform;

        [Space]
        [SerializeField] private float lateUpdateSpeed;
        [SerializeField] private float updateSpeed;
        [SerializeField] private float resetSpeed;


        [Space]
        [SerializeField] private Vector2 origin;

        [Space]
        [SerializeField] private Vector2 min;
        [SerializeField] private Vector2 max;

        private Vector2 desired;

        private MovementMotionUpdateMode updateMode;
        private MovementMotionLateUpdateMode lateUpdateMode;
        private MovementMotionInputType inputType;

        public MovementMotionUpdateMode UpdateMode => updateMode;
        public MovementMotionLateUpdateMode LateUpdateMode => lateUpdateMode;
        public MovementMotionInputType InputType => inputType;

        public float UpdateSpeed => updateSpeed;
        public float LateUpdateSpeed => lateUpdateSpeed;

        public float ResetSpeed => resetSpeed;
        public Vector3 Origin => origin;

        public PlayerMovementStateMotion() : base() {
    
        }

#if UNITY_EDITOR
        public void Validate(MovementStateType type, MovementMotionUpdateMode updateMode, MovementMotionLateUpdateMode lateUpdateMode, MovementMotionInputType inputType)
        {
            Validate(type, true);

            this.inputType = inputType;
            this.updateMode = updateMode;
            this.lateUpdateMode = lateUpdateMode;
        }
#endif

        public void SetTransform(Transform transform) { this.transform = transform; }
      
        public void SetTransformUpdateSpeed(float transformUpdateSpeed) {
            this.lateUpdateSpeed = transformUpdateSpeed;
        }
        public void SetUpdateSpeed(float updateSpeed)
        {
            this.updateSpeed = updateSpeed;
        }

        public void SetResetSpeed(float resetSpeed) {
            this.resetSpeed = resetSpeed;
        }

        public void SetMinY(float y)
        {
            this.min.y = y;
        }
        public void SetMinX(float x)
        {
            this.min.x = x;
        }

        public void SetMaxY(float y)
        {
            this.max.y = y;
        }
        public void SetMaxX(float x)
        {
            this.max.x = x;
        }

        public void SetMin(Vector2 minPosition)
        {
            this.min = minPosition;
        }
        public void SetMax(Vector2 maxPosition)
        {
            this.max = maxPosition;
        }
        public void SetOrigin(Vector2 origin)
        {
            this.origin = origin;
        }

        protected sealed override void OnStateUpdate(PlayerMovementStateMachine stateMachine) {
            Vector2 min   = Vector2.right * (origin.x - this.min.x) + Vector2.up * (origin.y - this.min.y);
            Vector2 max   = Vector2.right * (origin.x + this.max.x) + Vector2.up * (origin.y + this.max.y);   
            Vector2 input = Vector3.zero;
           
            if (InputType == MovementMotionInputType.None) input = Vector2.one;
            else if(stateMachine != null) {
                if (InputType == MovementMotionInputType.Move) { input = stateMachine.MoveInput; }
                else if (InputType == MovementMotionInputType.Camera) {
                    CameraController controller = stateMachine.CameraController;
                    input = controller != null ? controller.Input : Vector2.zero;
                }
            }

            OnStateUpdate(input, min, max, ref desired);
        }

        protected sealed override void OnStateLateUpdate() {
            if(transform != null) {
               if(LateUpdateMode == MovementMotionLateUpdateMode.Position) {
                  transform.localPosition = Vector3.Lerp(transform.localPosition, desired, Time.deltaTime * lateUpdateSpeed);
               }
               else if(LateUpdateMode == MovementMotionLateUpdateMode.Rotation) {
                   Quaternion rotation = Quaternion.Euler(desired.y, 0f, desired.x);
                   transform.localRotation = Quaternion.Slerp(transform.localRotation, rotation, Time.deltaTime * lateUpdateSpeed);
               }
            }
        }

        private void OnStateUpdate(Vector2 input, Vector2 min, Vector2 max, ref Vector2 desired) {
            if(input.magnitude <= 0f) {
                desired.x = Mathf.Lerp(desired.x, origin.x, Time.deltaTime * resetSpeed);
                desired.y = Mathf.Lerp(desired.y, origin.y, Time.deltaTime * resetSpeed);
            }
            if(UpdateMode == MovementMotionUpdateMode.Sin || InputType == MovementMotionInputType.None) { SinUpdate(input, min, max, ref desired); }
            else if(UpdateMode == MovementMotionUpdateMode.Input)                                       { InputUpdate(input, min, max, ref desired); }
        }

        private void SinUpdate(Vector2 input, Vector2 min, Vector2 max, ref Vector2 desired)
        {
            float sin = Mathf.Sin(Time.time * updateSpeed);
            desired.x = Mathf.Lerp(origin.x, sin < 0f ? min.x : max.x, Mathf.Abs(sin));
            desired.y = Mathf.Lerp(origin.y, sin < 0f ? min.y : max.y, Mathf.Abs(sin));
        }

        private void InputUpdate(Vector2 input, Vector2 min, Vector2 max, ref Vector2 desired)
        {
            input.Normalize();
            desired.x = Mathf.Clamp(desired.x + (Time.deltaTime * Mathf.Sign(input.x) * updateSpeed), min.x, max.x); 
            desired.y = Mathf.Clamp(desired.y + (Time.deltaTime * Mathf.Sign(input.y) * updateSpeed), min.y, max.y);
        }
    }
}
