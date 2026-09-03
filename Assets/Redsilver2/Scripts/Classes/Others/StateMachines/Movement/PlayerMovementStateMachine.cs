               
using RedSilver2.Framework.Inputs.Settings;
using RedSilver2.Framework.Player;
using RedSilver2.Framework.StateMachines.States;
using UnityEngine;
using UnityEngine.Events;


namespace RedSilver2.Framework.StateMachines.Controllers {
    public abstract class PlayerMovementStateMachine : MovementStateMachine
    {
        [Space]
        [SerializeField] private KeyboardVector2InputSettings moveInputSetting;
        [SerializeField, HideInInspector] private CameraController cameraController;
        [SerializeField, HideInInspector] private UnityEvent<Vector2> onMoveInputUpdate;

        private Vector3 nextPosition;

        public Vector2 MoveInput => moveInputSetting != null ? moveInputSetting.GetValue() : Vector2.zero;
        public CameraController CameraController => cameraController;
        public KeyboardVector2InputSettings MoveInputSetting => moveInputSetting;

#if UNITY_EDITOR
        protected override void OnValidate()
        {
            base.OnValidate();
        }

        protected override void DisplayDefaultSettings(Color foldoutColor, Color fieldColor)
        {
            base.DisplayDefaultSettings(foldoutColor, fieldColor);
            SetInputSetting(EditorExtension.DisplayCustomField("Move Input Setting", false, moveInputSetting, fieldColor));
        }

        protected override MovementState GetInspectorState(MovementStateType type)
        {
            if (ContainsState(type)) return null;

            switch (type) {
                case MovementStateType.Idol:   return new PlayerIdolState(this);
                case MovementStateType.Walk:   return new PlayerWalkState(this);
                case MovementStateType.Fall:   return new PlayerFallState(this);
                case MovementStateType.Jump:   return new PlayerJumpState(this);
                case MovementStateType.Land:   return new PlayerLandState(this);
                case MovementStateType.Crouch: return new PlayerCrouchState(this);
                case MovementStateType.Run:    return new PlayerRunState(this);
                default: return null;
            }
        }
#endif


        protected override void Awake() {
            base.Awake();
            cameraController = transform.root != null ? transform.root.GetComponentInChildren<CameraController>() :
                                                                       GetComponentInChildren<CameraController>();

            AddOnMoveInputUpdateListener(OnMoveInputUpdate);
            if (enabled) moveInputSetting?.Enable();
        }

        protected override void Start()
        {
            base.Start();
        }

        protected override void OnEnabled() {

            base.OnEnabled();
            if (cameraController != null) cameraController.enabled = true;
            moveInputSetting?.Enable();
        }

        protected override void OnDisabled()
        {
            base.OnDisabled();
            if (cameraController != null) cameraController.enabled = false;
            moveInputSetting?.Disable();
        }

        protected override void OnUpdate() {
            this.moveInputSetting?.Enable();

            base.OnUpdate();
            onMoveInputUpdate?.Invoke(moveInputSetting != null ? moveInputSetting.GetValue() : Vector2.zero);
        }

        protected sealed override void OnLateUpdate() {
            Move(Time.deltaTime * nextPosition);
        }

        public void SetInputSetting(KeyboardVector2InputSettings inputSetting) {
            this.moveInputSetting = inputSetting;
        }

        private void OnMoveInputUpdate(Vector2 input)
        {
            SetIsMoving(input.magnitude > 0f ? true : false);
            input.Normalize();

            nextPosition = Vector3.right * MoveSpeed * input.x +
                           Vector3.up * FallSpeed +
                           Vector3.forward * MoveSpeed * (Is2DMovement ? 0f : input.y);
        }

        public void AddOnMoveInputUpdateListener(UnityAction<Vector2> action) {
            if (action != null) onMoveInputUpdate?.AddListener(action);
        }
        public void RemoveOnMoveInputUpdateListener(UnityAction<Vector2> action) {
            if (action != null) onMoveInputUpdate?.RemoveListener(action);
        }
    }
}