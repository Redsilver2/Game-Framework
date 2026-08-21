using RedSilver2.Framework.StateMachines.Controllers;
using RedSilver2.Framework.StateMachines.Extensions;
using RedSilver2.Framework.StateMachines.States;
using UnityEngine;

namespace RedSilver2.Framework.StateMachines
{
    [System.Serializable]
    public class PlayerWalkState : WalkState
    {
        [Space]
        [SerializeField] private PlayerMovementStateMotion positionSwayMotion;
        [SerializeField] private PlayerMovementStateMotion rotationSwayMotion;

        public PlayerMovementStateMotion PositionSwayMotion => positionSwayMotion;
        public PlayerMovementStateMotion RotationSwayMotion => rotationSwayMotion;

        public PlayerWalkState() : base() {

        }


#if UNITY_EDITOR
        public virtual void Validate(PlayerMovementStateMachine stateMachine)
        {
            SetStateMachine(stateMachine);
            Validate();
        }

        protected override void Validate()
        {
            base.Validate();

            if (positionSwayMotion == null) positionSwayMotion = new PlayerMovementStateMotion();
            if (rotationSwayMotion == null) rotationSwayMotion = new PlayerMovementStateMotion();

            positionSwayMotion?.Validate(TYPE, MovementMotionUpdateMode.Sin, MovementMotionLateUpdateMode.Position, MovementMotionInputType.Move);
            rotationSwayMotion?.Validate(TYPE, MovementMotionUpdateMode.Sin, MovementMotionLateUpdateMode.Rotation, MovementMotionInputType.Move);

            rotationSwayMotion?.Validate();
            positionSwayMotion?.Validate();
        }
#endif

        protected sealed override void OnEnabled()
        {
            positionSwayMotion?.Enable();
            positionSwayMotion?.SetStateMachine(this.MovementStateMachine as PlayerMovementStateMachine);

            rotationSwayMotion?.Enable();
            rotationSwayMotion?.SetStateMachine(MovementStateMachine as PlayerMovementStateMachine);
            base.OnEnabled();
        }

        protected sealed override void OnDisabled()
        {
            positionSwayMotion?.Disable();
            positionSwayMotion?.SetStateMachine(null);

            rotationSwayMotion?.Disable();
            rotationSwayMotion?.SetStateMachine(null);
            base.OnDisabled();
        }

        protected void SetStateMachine(PlayerMovementStateMachine stateMachine)
        {
            this.MovementStateMachine       = stateMachine;
            SetStateMachine(stateMachine as UpdatableStateMachine);
        }


    }
}
