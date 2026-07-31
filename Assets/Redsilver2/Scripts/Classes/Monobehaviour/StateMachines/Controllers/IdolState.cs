using RedSilver2.Framework.StateMachines.Controllers;
using UnityEngine;

namespace RedSilver2.Framework.StateMachines.States
{
    public sealed class IdolState : MovementState
    {
        [Space]
        [SerializeField] private float moveSpeedTransition;

        public const MovementStateType TYPE = MovementStateType.Idol;

        protected sealed override bool CanTransition(MovementStateMachine stateMachine)
        {
            if (!base.CanTransition(stateMachine)) return false;
           
            return !stateMachine.IsMoving && stateMachine.IsGrounded
                && !RunState.IsStateMachineRunning(stateMachine) && !CrouchState.IsStateMachineCrouching(stateMachine)
                && !JumpState.IsStateMachineJumping(stateMachine);     
        }

        protected sealed override void OnUpdate(MovementStateMachine stateMachine) {
            if(stateMachine == null) return;
            stateMachine?.SetMoveSpeed(0f, moveSpeedTransition);
        }

        protected sealed override void SetMovementStateType(ref MovementStateType type) {
            type = TYPE;
        }

        public static IdolState GetState(MovementStateMachine stateMachine)
        {
            if(stateMachine == null) return null;
            return stateMachine.GetState(TYPE) as IdolState;
        }


#if UNITY_EDITOR
        protected sealed override MovementStateType[] GetDefaultInvalidTypes()
        {
            return new MovementStateType[] { TYPE, LandState.TYPE };
        }
#endif
    }
}