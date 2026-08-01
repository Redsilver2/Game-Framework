
using RedSilver2.Framework.Inputs.Settings;
using RedSilver2.Framework.StateMachines.States;
using UnityEngine;
using UnityEngine.Events;


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
        private Vector3 nextPosition;


        private UnityEvent<Vector2> onMoveInputUpdate;

        protected override void Awake() {
            base.Awake();
            onMoveInputUpdate = new UnityEvent<Vector2>();

            AddOnMoveInputUpdateListener(OnMoveInputUpdate);
            if (enabled) inputSetting?.Enable();
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
            onMoveInputUpdate?.Invoke(inputSetting != null ? inputSetting.GetValue() : Vector2.zero);
            UpdateCameraCrouchTransform(IsCurrentState(MovementStateType.Crouch) ? crouchCameraPosition : standCameraPosition);
        }

        protected sealed override void OnLateUpdate(){
            Move(Time.deltaTime * nextPosition);
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

        private void OnMoveInputUpdate(Vector2 input)
        {
            SetIsMoving(input.magnitude > 0f ? true : false);
            input.Normalize();

            nextPosition = Vector3.right   * MoveSpeed * input.x +
                           Vector3.up      * FallSpeed +
                           Vector3.forward * MoveSpeed * (Is2DMovement ? 0f : input.y);
        }

        public void AddOnMoveInputUpdateListener(UnityAction<Vector2> action) {
           if(action != null)  onMoveInputUpdate?.AddListener(action);
        }
        public void RemoveOnMoveInputUpdateListener(UnityAction<Vector2> action)
        {
           if(action != null) onMoveInputUpdate?.RemoveListener(action);
        }

    }
}