
using RedSilver2.Framework.Inputs.Settings;
using RedSilver2.Framework.StateMachines.States;
using UnityEngine;


namespace RedSilver2.Framework.StateMachines.Controllers {
    public abstract class PlayerMovementStateMachine : MovementStateMachine
    {
        [Space]
        [SerializeField] private KeyboardVector2InputSettings inputSetting;

        [Space]
        [SerializeField] private Transform cameraCrouchTransform;

        [Space]
        [SerializeField] private Vector3 crouchCameraPosition;
        [SerializeField] private Vector3 standCameraPosition;

        [Space]
        [SerializeField] private float crouchCameraUpdateSpeed;

        protected override void Awake() {
            base.Awake();
            if(enabled) inputSetting?.Enable();
        }


        protected override void OnEnabled() {

            base.OnEnabled();
            inputSetting?.Enable();
        }

        protected override void OnDisabled()
        {
            base.OnDisabled();
            inputSetting?.Disable();
        }

        protected override void OnUpdate() {
            base.OnUpdate();
            UpdateCameraCrouchTransform(IsCurrentState(MovementStateType.Crouch) ? crouchCameraPosition : standCameraPosition);
        }

        private void UpdateCameraCrouchTransform(Vector3 position) {
            if (cameraCrouchTransform != null)
                cameraCrouchTransform.localPosition = Vector3.Lerp(cameraCrouchTransform.localPosition, position, Time.deltaTime * crouchCameraUpdateSpeed);
        }

        public void SetInputSetting(KeyboardVector2InputSettings inputSetting) {
            this.inputSetting?.Disable();
            this.inputSetting = inputSetting;

            if (inputSetting != null){
                if (enabled) inputSetting?.Enable();
                else         inputSetting?.Disable();
            }
        }

        protected sealed override void Move()
        {

            Vector2 inputValue = inputSetting != null ? inputSetting.GetValue() : Vector2.zero;
            SetIsMoving(inputValue.magnitude > 0f ? true : false);

            inputValue.Normalize();

            float moveSpeed = MoveSpeed, fallSpeed = FallSpeed;
            Vector3 nextPosition = Time.deltaTime * ((transform.right   * moveSpeed * inputValue.x) +
                                                     (transform.up      * fallSpeed)
                                                   + (transform.forward * moveSpeed * inputValue.y));

            Move(nextPosition);
        }
    }
}