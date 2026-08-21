               
using RedSilver2.Framework.Inputs.Settings;
using RedSilver2.Framework.Player;
using RedSilver2.Framework.StateMachines.Extensions;
using RedSilver2.Framework.StateMachines.States;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem.LowLevel;


namespace RedSilver2.Framework.StateMachines.Controllers {
    public abstract class PlayerMovementStateMachine : MovementStateMachine
    {
        [Space]
        [SerializeField] private KeyboardVector2InputSettings moveInputSetting;

        [Space]
        [SerializeField] private PlayerIdolState idolState;

        [Space]
        [SerializeField] private PlayerFallState fallState;

        [Space]
        [SerializeField] private PlayerLandState landState;

        [Space]
        [SerializeField] private PlayerWalkState walkState;

        [Space]
        [SerializeField] private PlayerJumpState jumpState;

        [Space]
        [SerializeField] private PlayerCrouchState crouchState;

        [Space]
        [SerializeField] private PlayerRunState runState;

        private Vector3 nextPosition;

        private CameraController cameraController;
        private UnityEvent<Vector2> onMoveInputUpdate;

        public Vector2 MoveInput => moveInputSetting != null ? moveInputSetting.GetValue() : Vector2.zero;
        public CameraController CameraController => cameraController;

        public PlayerIdolState IdolState   => idolState;
        public PlayerFallState FallState   => fallState;
        public PlayerLandState LandState   => landState;

        public PlayerWalkState   WalkState   => walkState;
        public PlayerRunState    RunState    => runState;

        public PlayerJumpState   JumpState   => jumpState;
        public PlayerCrouchState CrouchState => crouchState;

#if UNITY_EDITOR

        protected override void ValidateStates()
        {
            if (idolState ==  null) idolState = new PlayerIdolState();
            idolState?.Validate(this);

            if (fallState == null) fallState = new PlayerFallState();
            fallState?.Validate(this);

            if(landState == null) landState = new PlayerLandState();
            landState?.Validate(this);

            if (walkState == null) walkState = new PlayerWalkState();
            walkState?.Validate(this);

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
            if (enabled) moveInputSetting?.Enable();
        }

        protected override void Start()
        {
            AddState(idolState);
            AddState(fallState);
            AddState(landState);

            AddState(walkState);

            AddState(jumpState);
            AddState(runState);
            AddState(crouchState);

            idolState?.Enable();
            fallState?.Enable();
            landState?.Enable();    

            walkState?.Enable();
            jumpState?.Enable();
           
            crouchState?.Enable();
            runState?.Enable();
        }

        public override bool ChangeState(MovementStateType type, bool checkSimilarity)
        {
            switch (type)
            {
                case MovementStateType.Jump: ChangeState(jumpState, checkSimilarity); return true;
                case MovementStateType.Run: ChangeState(runState, checkSimilarity); return true;
                case MovementStateType.Crouch: ChangeState(crouchState, checkSimilarity); return true;
                case MovementStateType.Idol: ChangeState(idolState, checkSimilarity); return true;
                case MovementStateType.Fall: ChangeState(fallState, checkSimilarity); return true;
                case MovementStateType.Land: ChangeState(landState, checkSimilarity); return true;
            }

            return false;
        }

        public override void AddState(MovementStateType type)
        {
            switch (type)
            {
                case MovementStateType.Jump: AddState(jumpState); return;
                case MovementStateType.Run: AddState(runState); return;
                case MovementStateType.Crouch: AddState(crouchState); return;
                case MovementStateType.Idol: AddState(idolState); return;
                case MovementStateType.Fall: AddState(fallState); return;
                case MovementStateType.Land: AddState(landState); return;
            }
        }

        public override void RemoveState(MovementStateType type)
        {
            switch (type)
            {
                case MovementStateType.Jump: RemoveState(jumpState); return;
                case MovementStateType.Run: RemoveState(runState); return;
                case MovementStateType.Crouch: RemoveState(crouchState); return;
                case MovementStateType.Idol: RemoveState(idolState); return;
                case MovementStateType.Fall: RemoveState(fallState); return;
                case MovementStateType.Land: RemoveState(landState); return;
            }
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
            base.OnUpdate();
            onMoveInputUpdate?.Invoke(moveInputSetting != null ? moveInputSetting.GetValue() : Vector2.zero);

            crouchState?.UpdateInput();
            runState?.UpdateInput();
            jumpState?.Update();
        }

        protected sealed override void OnLateUpdate() {
            PlayerCrouchCameraUpdater cameraUpdater = crouchState != null ? crouchState.CameraUpdater : null;
            cameraUpdater?.LateUpdate();
            Move(Time.deltaTime * nextPosition);
        }

        public void SetInputSetting(KeyboardVector2InputSettings inputSetting) {
            this.moveInputSetting?.Disable();
            this.moveInputSetting = inputSetting;

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