using RedSilver2.Framework.Player;
using RedSilver2.Framework.StateMachines.Controllers;
using RedSilver2.Framework.StateMachines.Events;
using RedSilver2.Framework.StateMachines.States;
using UnityEngine;
using UnityEngine.Events;

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

        [SerializeField, HideInInspector] private MovementMotionUpdateMode updateMode;
        [SerializeField, HideInInspector] private MovementMotionLateUpdateMode lateUpdateMode;
        [SerializeField, HideInInspector] private MovementMotionInputType inputType;

        public float UpdateSpeed => updateSpeed;
        public float LateUpdateSpeed => lateUpdateSpeed;
        public float ResetSpeed => resetSpeed;

        public MovementMotionUpdateMode UpdateMode => updateMode;
        public MovementMotionLateUpdateMode LateUpdateMode => lateUpdateMode;
        public MovementMotionInputType InputType => inputType;

        public Vector3 Origin => origin;

        public PlayerMovementStateMotion() : base() { }

        public void SetUpdateMode(MovementMotionUpdateMode updateMode) { this.updateMode = updateMode; }
        public void SetLateUpdateMode(MovementMotionLateUpdateMode lateUpdateMode) { this.lateUpdateMode = lateUpdateMode; }
        public void SetInputType(MovementMotionInputType inputType) { this.inputType = inputType; }

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

        private UnityAction GetOnUpdateListener(PlayerMovementStateMachine stateMachine) {
            if (stateMachine == null) return null;

            return () => {
                Vector2 min = Vector2.right * (origin.x - this.min.x) + Vector2.up * (origin.y - this.min.y);
                Vector2 max = Vector2.right * (origin.x + this.max.x) + Vector2.up * (origin.y + this.max.y);
                Vector2 input = Vector3.zero;

                if (inputType == MovementMotionInputType.None) input = Vector2.one;
                else if (stateMachine != null) {
                    if (inputType == MovementMotionInputType.Move) { input = stateMachine.MoveInput; }
                    else if (inputType == MovementMotionInputType.Camera)
                    {
                        CameraController controller = stateMachine.CameraController;
                        input = controller != null ? controller.Input : Vector2.zero;
                    }
                }

                OnStateUpdate(input, min, max, ref desired);
            };
        }

        private void OnLateUpdate() {
            if(transform != null) {
               if(lateUpdateMode == MovementMotionLateUpdateMode.Position) {
                  transform.localPosition = Vector3.Lerp(transform.localPosition, desired, Time.deltaTime * lateUpdateSpeed);
               }
               else if(lateUpdateMode == MovementMotionLateUpdateMode.Rotation) {
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
            if(updateMode == MovementMotionUpdateMode.Sin || inputType == MovementMotionInputType.None) { SinUpdate(input, min, max, ref desired); }
            else if(updateMode == MovementMotionUpdateMode.Input)                                       { InputUpdate(input, min, max, ref desired); }
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

        protected sealed override void Add(MovementState state, PlayerMovementStateMachine stateMachine)
        {
            state?.AddOnUpdateListener(GetOnUpdateListener(stateMachine));
            state?.AddOnLateUpdateListener(OnLateUpdate);
        }

        protected sealed override void Remove(MovementState state, PlayerMovementStateMachine stateMachine)
        {
            state?.RemoveOnUpdateListener(GetOnUpdateListener(stateMachine));
            state?.RemoveOnLateUpdateListener(OnLateUpdate);
        }

        protected sealed override void SetCompatibleStateTypes(ref MovementStateType[] types)
        {
            types = new MovementStateType[] { MovementStateType.Idol, MovementStateType.Walk, MovementStateType.Run, MovementStateType.Crouch };
        }
    }
}
