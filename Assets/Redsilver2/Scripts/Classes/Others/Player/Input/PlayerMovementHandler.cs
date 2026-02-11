using RedSilver2.Framework.Inputs;
using RedSilver2.Framework.StateMachines.Controllers;
using UnityEngine;
using UnityEngine.Events;

namespace RedSilver2.Framework.StateMachines.States.Movement
{
    [System.Serializable]
    public abstract class PlayerMovementHandler : MovementHandler
    {
        private OverrideableKeyboardVector2Input moveInput;
        private readonly UnityEvent onUpdate;

        private const string MOVE_INPUT = "Move Input";

        protected PlayerMovementHandler(PlayerController controller) : base(controller) {
            onUpdate = new UnityEvent();
            onUpdate.AddListener(() => { UpdateMoveInput(); });
           
            moveInput = GetMoveInput();
            moveInput?.Enable();
        }

        protected PlayerMovementHandler(PlayerController controller, bool use2DMovement) : base(controller, use2DMovement) {
            onUpdate  = new UnityEvent();
            onUpdate.AddListener(() => { UpdateMoveInput();  });

            moveInput = GetMoveInput();
            moveInput?.Enable();
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


        protected sealed override void Disable() {
            base.Disable();
            moveInput?.Disable();
        }

        protected sealed override void Enable() {
            moveInput?.Enable();
        }

        private void UpdateMoveInput() {
            moveInput?.Update();
        }

        protected override Vector3 GetNextPosition(Transform transform, float moveSpeed, float fallSpeed, bool use2DMovement) {
            if(transform == null || moveInput == null) return Vector3.zero;

            Vector2 input = moveInput.Value;
            input.Normalize();

            return  (transform.forward *                       input.y * moveSpeed +
                     transform.up                                      * fallSpeed +
                     transform.right   * (use2DMovement ? 0f : input.x * moveSpeed));
        }

        public sealed override float GetMagnitude()
        {
            if (moveInput == null) return 0f;
            return moveInput.Value.magnitude;
        }

        public static OverrideableKeyboardVector2Input GetMoveInput() {
            return InputManager.GetOrCreateOverrideableKeyboardVector2Input(MOVE_INPUT);
        }
    }
}
