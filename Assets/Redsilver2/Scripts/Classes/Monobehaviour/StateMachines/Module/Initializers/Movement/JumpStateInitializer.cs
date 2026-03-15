using RedSilver2.Framework.Inputs;
using RedSilver2.Framework.StateMachines.States;
using UnityEngine;
using UnityEngine.Events;


namespace RedSilver2.Framework.StateMachines
{
    [RequireComponent(typeof(JumpMovementStateCondition))]
    public abstract class JumpStateInitializer : MovementConditionStateInitializer
    {
        [SerializeField] private float jumpForce;
        public float JumpForce => jumpForce;

#if UNITY_EDITOR
        private void OnValidate()
        {
            jumpForce = Mathf.Clamp(jumpForce, 0f, float.MaxValue);
        }
#endif

        protected sealed override MovementState GetDefaultState(MovementStateMachine stateMachine)
        {
            if (stateMachine == null) return null;

            if (stateMachine.ContainsState(MovementStateType.Jump)) 
                return stateMachine.GetState(MovementStateType.Jump) as JumpState;

            return new JumpState(stateMachine, this);
        }

        protected override string GetModuleName() {
            return "Jump State Initializer";
        }

        protected sealed override bool IsValidCondition(MovementStateCondition condition)
        {
            return condition is GroundMovementStateCondition || condition is CrouchMovementStateCondition;
        }

        protected sealed override bool IsShowOppositeResultCondition(MovementStateCondition condition)
        {
            return condition is GroundMovementStateCondition;
        }
    }
}
