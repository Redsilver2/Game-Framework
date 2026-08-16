               
using RedSilver2.Framework.Inputs.Settings;
using RedSilver2.Framework.Player;
using RedSilver2.Framework.StateMachines.States;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.Events;


namespace RedSilver2.Framework.StateMachines.Controllers {
    public abstract class PlayerMovementStateMachine : MovementStateMachine
    {
        [Space]
        [SerializeField] private KeyboardVector2InputSettings inputSetting;

        [Space]
        [SerializeField] private PlayerJumpState jumpState;

        [Space]
        [SerializeField] private PlayerCrouchState crouchState;

        [Space]
        [SerializeField] private PlayerRunState runState;

        private Vector3 nextPosition;

        private CameraController cameraController;
        private UnityEvent<Vector2> onMoveInputUpdate;

        public CameraController CameraController => cameraController;
        public PlayerRunState    RunState    => runState;
     
        public PlayerJumpState   JumpState   => jumpState;
        public PlayerCrouchState CrouchState => crouchState;

#if UNITY_EDITOR
        protected override void OnValidate()
        {
            base.OnValidate();
        }

        protected override void ValidateStates()
        {
            base.ValidateStates();

            if(crouchState == null) crouchState = new PlayerCrouchState();
            crouchState?.Validate(this);

            if (jumpState == null) jumpState = new PlayerJumpState();
            jumpState?.Validate(this);

            if(runState  == null) runState = new PlayerRunState();
            runState?.Validate(this);
        }
#endif


        protected override void Awake() {
            base.Awake();
            onMoveInputUpdate = new UnityEvent<Vector2>();

            cameraController = transform.root != null ? transform.root.GetComponentInChildren<CameraController>() :
                                                                       GetComponentInChildren<CameraController>();

            AddOnMoveInputUpdateListener(OnMoveInputUpdate);
            if (enabled) inputSetting?.Enable();
        }

        protected override void Start()
        {
            base.Start();

            jumpState?.Enable();
            crouchState?.Enable();
            runState?.Enable();
        }

        public override bool ChangeState(MovementStateType type, bool checkSimilarity)
        {
            if(!base.ChangeState(type, checkSimilarity)) {
                switch (type) {
                    case MovementStateType.Jump:   ChangeState(jumpState,   checkSimilarity); return true;
                    case MovementStateType.Run:    ChangeState(runState,    checkSimilarity); return true;
                    case MovementStateType.Crouch: ChangeState(crouchState, checkSimilarity); return true;
                }

                return false;
            }

            return true;
        }



        protected override void OnEnabled() {

            base.OnEnabled();
            if (cameraController != null) cameraController.enabled = true;
            inputSetting?.Enable();
        }

        protected override void OnDisabled()
        {
            base.OnDisabled();
            if (cameraController != null) cameraController.enabled = false;
            inputSetting?.Disable();
        }

        protected override void OnUpdate() {
            base.OnUpdate();
            onMoveInputUpdate?.Invoke(inputSetting != null ? inputSetting.GetValue() : Vector2.zero);

        }

        protected sealed override void OnLateUpdate() {
            Move(Time.deltaTime * nextPosition);
        }

        public void SetInputSetting(KeyboardVector2InputSettings inputSetting) {
            this.inputSetting?.Disable();
            this.inputSetting = inputSetting;

            if (inputSetting != null) {
                if (enabled) inputSetting?.Enable();
                else inputSetting?.Disable();
            }
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