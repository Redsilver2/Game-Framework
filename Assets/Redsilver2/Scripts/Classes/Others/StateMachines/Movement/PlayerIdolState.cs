using RedSilver2.Framework.StateMachines.Controllers;
using RedSilver2.Framework.StateMachines.Extensions;
using UnityEngine;


namespace RedSilver2.Framework.StateMachines.States
{
    [System.Serializable]
    public sealed class PlayerIdolState : IdolState
    {

        [Space]
        [SerializeField] private PlayerMovementStateMotion positionSwayMotion;
        [SerializeField] private PlayerMovementStateMotion rotationSwayMotion;

        public PlayerMovementStateMotion PositionSwayMotion => positionSwayMotion;
        public PlayerMovementStateMotion RotationSwayMotion => rotationSwayMotion;


        public PlayerIdolState() {
        }


#if UNITY_EDITOR
        public void Validate(PlayerMovementStateMachine stateMachine)
        {
            SetStateMachine(stateMachine);
            Validate();
        }

        protected override void Validate() {
            base.Validate();

            if (positionSwayMotion == null) positionSwayMotion = new PlayerMovementStateMotion();
            if (rotationSwayMotion == null) rotationSwayMotion = new PlayerMovementStateMotion();

            positionSwayMotion?.Validate(TYPE, MovementMotionUpdateMode.Sin, MovementMotionLateUpdateMode.Position, MovementMotionInputType.None);
            rotationSwayMotion?.Validate(TYPE, MovementMotionUpdateMode.Sin, MovementMotionLateUpdateMode.Rotation, MovementMotionInputType.None);

            positionSwayMotion?.Validate();
            rotationSwayMotion?.Validate();
        }
#endif

        private void SetStateMachine(PlayerMovementStateMachine stateMachine)
        {
            this.MovementStateMachine = stateMachine;
            SetStateMachine(stateMachine as UpdatableStateMachine);
        }

        protected override void OnEnabled()
        {
            positionSwayMotion?.Enable();
            positionSwayMotion?.SetStateMachine(MovementStateMachine as PlayerMovementStateMachine);

            rotationSwayMotion?.Enable();
            rotationSwayMotion?.SetStateMachine(MovementStateMachine as PlayerMovementStateMachine);

            base.OnEnabled();
        }

        protected override void OnDisabled()
        {
            positionSwayMotion?.Disable();
            positionSwayMotion?.SetStateMachine(null);

            rotationSwayMotion?.Disable();
            rotationSwayMotion?.SetStateMachine(null);

            base.OnDisabled();
        }

    }
}
