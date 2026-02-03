using RedSilver2.Framework.Inputs;
using RedSilver2.Framework.StateMachines.Controllers;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem.XR;

namespace RedSilver2.Framework.StateMachines.States.Movement
{
    [System.Serializable]
    public abstract class PlayerMovementHandler : MovementHandler
    {
        private bool isJumpInputUpdateEnabled;
        private bool isCrouchInputUpdateEnabled;
        private bool isRunInputUpdateEnabled;

        private OverrideableKeyboardVector2Input moveInput;
        private OverrideablePressInput           pressJumpInput;

        private OverrideableHoldInput  holdRunInput;
        private OverrideablePressInput pressRunInput;

        private OverrideableHoldInput  holdCrouchInput;
        private OverrideablePressInput pressCrouchInput;

        private readonly UnityEvent onUpdate;


        public const string MOVE_INPUT           = "Move Input";
        public const string HOLD_RUN_INPUT       = "Hold Run Input";
       
        public const string PRESS_RUN_INPUT      = "Press Run Input";
        public const string JUMP_INPUT           = "Jump Input";

        public const string HOLD_CROUCH_INPUT    = "Hold Crouch Input";
        public const string PRESS_CROUCH_INPUT   = "Press Crouch Input";

        public const string RUN_INPUT_SETTING    = "Run Input Setting";
        public const string CROUCH_INPUT_SETTING = "Crouch Input Setting";


        protected PlayerMovementHandler(PlayerController controller) : base(controller) {
            onUpdate = new UnityEvent();
            onUpdate.AddListener(() => {
                isGrounded = IsOnGround(out groundTag, controller ==  null ? null : controller.transform);
                UpdateMoveInput();
            });

            moveInput        = GetMoveInput();
            pressJumpInput   = GetPressJumpInput();

            holdRunInput     = GetHoldRunInput();
            pressRunInput    = GetPressRunInput();

            holdCrouchInput  = GetHoldCrouchInput();
            pressCrouchInput = GetPressCrouchInput();

            pressJumpInput.AddOnUpdateListener(OnPressJumpUpdate);
            pressRunInput.AddOnUpdateListener(OnPressRunUpdate);
            pressCrouchInput.AddOnUpdateListener(OnPressCrouchUpdate);

            if (!PlayerPrefs.HasKey(RUN_INPUT_SETTING))    PlayerPrefs.SetString(RUN_INPUT_SETTING,    "hold");
            if (!PlayerPrefs.HasKey(CROUCH_INPUT_SETTING)) PlayerPrefs.SetString(CROUCH_INPUT_SETTING, "hold");
            PlayerPrefs.Save();
        }

        protected PlayerMovementHandler(PlayerController controller, bool use2DMovement) : base(controller, use2DMovement) {
            onUpdate  = new UnityEvent();
            onUpdate.AddListener(() => {
                isGrounded = IsOnGround(out groundTag, controller == null ? null : controller.transform);
                UpdateMoveInput();
            });

            moveInput = GetMoveInput();
            pressJumpInput = GetPressJumpInput();

            holdRunInput  = GetHoldRunInput();
            pressRunInput = GetPressRunInput();

            holdCrouchInput  = GetHoldCrouchInput();
            pressCrouchInput = GetPressCrouchInput();

            pressJumpInput.AddOnUpdateListener(OnPressJumpUpdate);
           
            holdRunInput.AddOnUpdateListener(OnHoldRunUpdate);  
            pressRunInput.AddOnUpdateListener(OnPressRunUpdate);

            holdCrouchInput.AddOnUpdateListener(OnHoldCrouchUpdate);
            pressCrouchInput.AddOnUpdateListener(OnPressCrouchUpdate);

            if (!PlayerPrefs.HasKey(RUN_INPUT_SETTING)) PlayerPrefs.SetString(RUN_INPUT_SETTING, "hold");
            if (!PlayerPrefs.HasKey(CROUCH_INPUT_SETTING)) PlayerPrefs.SetString(CROUCH_INPUT_SETTING, "hold");
            PlayerPrefs.Save();
        }

        public void AddOnMoveInputUpdateListener(UnityAction<Vector2> action) {
            if (action != null) moveInput?.AddOnUpdateListener(action);
        }

        public void RemoveOnMoveInputUpdateListener(UnityAction<Vector2> action) {
            if (action != null) moveInput?.AddOnUpdateListener(action);
        }

        public bool IsMoveInputEnabled() {
            if (moveInput == null) return false;
            return moveInput.IsEnabled;
        }

        protected override void Update() {
            onUpdate?.Invoke();
            base.Update();
        }


        protected override void Disable() {
            holdRunInput?.Disable();
            pressRunInput?.Disable();

            holdCrouchInput?.Disable();
            pressCrouchInput?.Disable();

            pressJumpInput?.Disable();
            moveInput?.Disable();
        }

        protected override void Enable() {
            holdRunInput?.Enable();
            pressRunInput?.Enable();

            holdCrouchInput?.Enable();
            pressCrouchInput?.Enable();

            pressJumpInput?.Enable();
            moveInput?.Enable();
        }

        public sealed override float GetMoveMagnitude() {
            if(moveInput == null) return 0f;
            return moveInput.Value.magnitude;
        }

        public void EnableJumpInputUpdate()
        {
            if (!isJumpInputUpdateEnabled)
            {
                onUpdate.AddListener(UpdateJumpInput);
                isJumpInputUpdateEnabled = true;
            }
        }

        public void DisableJumpInputUpdate()
        {
            if (isJumpInputUpdateEnabled) {
                onUpdate.RemoveListener(UpdateJumpInput);
                isJumpInputUpdateEnabled = false;
                isJumping = false;
            }
        }


        public void EnableRunInputUpdate()
        {
            if (!isRunInputUpdateEnabled) {
                onUpdate.AddListener(UpdateRunInput);
                isRunInputUpdateEnabled = true;
            }
        }

        public void DisableRunInputUpdate()
        {
            if (isRunInputUpdateEnabled) {
                onUpdate.RemoveListener(UpdateRunInput);
                isRunInputUpdateEnabled = false;
                isRunning = false;
            }
        }

        public void EnableCrouchInputUpdate() {
            if (!isCrouchInputUpdateEnabled)  {
                onUpdate.AddListener(UpdateCrouchInput);
                isCrouchInputUpdateEnabled = true;
            }
        }

        public void DisableCrouchInputUpdate() {
            if (isCrouchInputUpdateEnabled) {
                onUpdate.RemoveListener(UpdateCrouchInput);
                isCrouchInputUpdateEnabled = false;
                isCrouching = false;
            }
        }


        private void UpdateJumpInput() {
            pressJumpInput?.Update();
        }

        private void UpdateMoveInput() {
            moveInput?.Update();
        }

        private void UpdateRunInput() {
            if(PlayerPrefs.GetString(RUN_INPUT_SETTING).Equals("hold")) {
                OnHoldRunUpdate();
            }
            else {
                pressRunInput?.Update();
            }
        }

        private void UpdateCrouchInput() {
            if(PlayerPrefs.GetString(CROUCH_INPUT_SETTING).Equals("hold")) {
                OnHoldCrouchUpdate();
            }
            else {
                pressCrouchInput?.Update();
            }
        }

        private void OnPressJumpUpdate() {
            if     (pressJumpInput == null) isJumping = false;
            else if(pressJumpInput.Value)   isJumping = true;
        }

        private void OnHoldRunUpdate() {
            holdRunInput?.Update();

            if (holdRunInput == null || GetMoveMagnitude() == 0f) isRunning = false;
            else                                                  isRunning = holdRunInput.Value;
        }

        private void OnPressRunUpdate() {
            if (pressRunInput == null || GetMoveMagnitude() == 0f) {
                isRunning = false;
            }
            else {
                if (pressRunInput.Value) isRunning = !isRunning;
            }
        }

        private void OnHoldCrouchUpdate() {
            Debug.Log("Is Crouching: " + holdCrouchInput.Value + " Can Reset Crouch: " + canResetCrouch);
            if (!canResetCrouch) { return; }
            holdCrouchInput?.Update();

            if (holdCrouchInput == null)  isCrouching = false;
            else                          isCrouching = holdCrouchInput.Value;
        }

        private void OnPressCrouchUpdate() {
            if (!canResetCrouch) { return; }

            if (pressCrouchInput == null) isCrouching = false;
            else {
                if(pressCrouchInput.Value) isCrouching = !isCrouching;
            }
        }

        protected override bool IsOnGround(out string groundTag, Transform transform) {
            
            if (transform != null) {
                float checkRange = GetGroundCheckRange();
                int playerLayer = GameManager.GetPlayerLayer();

                if (Physics.Raycast(transform.position, -transform.up, out RaycastHit hitInfo, checkRange, ~playerLayer))
                    return IsOnGround(out groundTag, hitInfo);
            }

            groundTag = string.Empty;
            return false;
        }

        protected bool IsOnGround(out string groundTag, RaycastHit hitInfo) {
            Collider collider = hitInfo.collider;
            groundTag = string.Empty;

            if(collider == null) return false;
            GameObject gameObject = collider.gameObject;

            if(gameObject.layer == GameManager.GetGroundLayer()){
                groundTag = gameObject.tag;
                return true;
            }

            return false;
        }

        protected override Vector3 GetNextPosition(Transform transform, float moveSpeed, float fallSpeed, bool use2DMovement) {
            if(transform == null || moveInput == null) return Vector3.zero;

            Vector2 input = moveInput.Value;
            return Time.deltaTime * (transform.forward *                       input.y * moveSpeed +
                                     transform.up                                      * fallSpeed +
                                     transform.right   * (use2DMovement ? 0f : input.x * moveSpeed));
        }

        public static OverrideableKeyboardVector2Input GetMoveInput() {
            return InputManager.GetOrCreateOverrideableKeyboardVector2Input(MOVE_INPUT);
        }

        public static OverrideablePressInput GetPressJumpInput() {
            return InputManager.GetOrCreateOverrideablePressInput(JUMP_INPUT, KeyboardKey.Space, GamepadButton.ButtonSouth);
        }

        public static OverrideablePressInput GetPressRunInput() {
            return InputManager.GetOrCreateOverrideablePressInput(PRESS_RUN_INPUT, KeyboardKey.LeftShift, GamepadButton.ButtonEast);
        }

        public static OverrideableHoldInput GetHoldRunInput() {
            return InputManager.GetOrCreateOverrideableHoldInput(HOLD_RUN_INPUT, KeyboardKey.LeftShift, GamepadButton.ButtonEast);
        }


        public static OverrideablePressInput GetPressCrouchInput() {
            return InputManager.GetOrCreateOverrideablePressInput(PRESS_CROUCH_INPUT, KeyboardKey.C, GamepadButton.ButtonWest);
        }

        public static OverrideableHoldInput GetHoldCrouchInput() {
            return InputManager.GetOrCreateOverrideableHoldInput(HOLD_CROUCH_INPUT, KeyboardKey.C, GamepadButton.ButtonWest);
        }


    }
}
