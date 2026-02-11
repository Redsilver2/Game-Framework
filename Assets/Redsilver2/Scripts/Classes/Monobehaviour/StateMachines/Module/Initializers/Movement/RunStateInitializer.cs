using UnityEngine;
using UnityEngine.Events;

namespace RedSilver2.Framework.StateMachines.States
{
    [RequireComponent(typeof(RunMovementStateCondition))]
    [RequireComponent(typeof(MoveMovementStateCondition))]
    public abstract class RunStateInitializer : MovementConditionStateInitializer
    {
        [SerializeField] private float runSpeed;
        [SerializeField] private float runTransitionSpeed;
        public float RunSpeed => runSpeed; 
        public float RunTransitionSpeed => runTransitionSpeed;  

        #if UNITY_EDITOR
        private void OnValidate()
        {
            runSpeed = Mathf.Clamp(runSpeed, Mathf.Epsilon, float.MaxValue);
        }
#endif

        protected sealed override MovementState GetDefaultState(MovementStateMachine stateMachine)
        {
            if (stateMachine == null) return null;

            if (stateMachine.ContainsState(MovementStateType.Run)) 
                return stateMachine.GetState(MovementStateType.Run) as RunState;

            return new RunState(stateMachine, this);
        }

        protected override string GetModuleName()
        {
            return "Run State Initializer";
        }

        protected override bool IsValidCondition(MovementStateCondition condition) {
            return condition is GroundMovementStateCondition || condition is CrouchMovementStateCondition ||
                   condition is MoveMovementStateCondition;
        }

        protected sealed override bool IsShowOppositeResultCondition(MovementStateCondition condition) {
            return condition is GroundMovementStateCondition || condition is MoveMovementStateCondition;
        }
    }
}
