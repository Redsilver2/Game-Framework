
using RedSilver2.Framework.Inputs.Settings;
using UnityEngine;


namespace RedSilver2.Framework.StateMachines.Controllers {
    public abstract class PlayerMovementStateMachine : MovementStateMachine
    {
        [Space]
        [SerializeField] private MovementInputSettings inputSetting;

        protected override void Awake()
        {
            base.Awake();
            inputSetting?.Enable();
        }

        protected sealed override void Move()
        {

            Vector2 inputValue = inputSetting != null ? inputSetting.GetMoveVector() : Vector2.zero;
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