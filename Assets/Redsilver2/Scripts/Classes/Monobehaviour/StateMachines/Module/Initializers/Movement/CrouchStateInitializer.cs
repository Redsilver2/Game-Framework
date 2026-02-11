using RedSilver2.Framework.StateMachines.States;
using UnityEngine;

namespace RedSilver2.Framework.StateMachines {
    [RequireComponent(typeof(CrouchMovementStateCondition))]
    public abstract class CrouchStateInitializer : MovementConditionStateInitializer {
        [SerializeField] private float standingHeight;
        [SerializeField] private float crouchHeight;

        [Space]
        [SerializeField] private float moveSpeed;

        [Space]
        [SerializeField] private float crouchTransitionSpeed;
        [SerializeField] private float standTransitionSpeed;

        [Space]
        [SerializeField] private float resetCrouchCheckRange;
       
        private float height;
        private bool  canResetCrouch;

        public  float  StandingHeight          => standingHeight;
        public  float  CrouchHeight            => crouchHeight;
        public  float  Height                  => height;
        public  float  MoveSpeed               => moveSpeed; 
        public  float  CrouchTransitionSpeed   => crouchTransitionSpeed;
        public float   StandTransitionSpeed    => standTransitionSpeed;
        public  bool   CanResetCrouch          => canResetCrouch;

        #if UNITY_EDITOR
        private void OnValidate()
        {
            standingHeight  = Mathf.Clamp(standingHeight , 2f, float.MaxValue);
            crouchHeight    = Mathf.Clamp(crouchHeight   , 1f, standingHeight - 1f);
            moveSpeed = Mathf.Clamp(moveSpeed, Mathf.Epsilon, float.MaxValue);
        }
        #endif



        protected override void Start()
        {
            base.Start();
            canResetCrouch = true;
            height  = standingHeight;
        }

        protected sealed override MovementState GetDefaultState(MovementStateMachine stateMachine) {
            if (stateMachine == null) return null;

            if (stateMachine.ContainsState(MovementStateType.Crouch)) 
                return stateMachine.GetState(MovementStateType.Crouch) as CrouchState;

            return new CrouchState(stateMachine, this);
        }

        private bool GetCanResetCrouch(Transform transform)
        {
            if (transform == null) return false;
            return !Physics.Raycast(transform.position, transform.up, resetCrouchCheckRange, ~transform.gameObject.layer);
        }

        public void SetCanResetCrouch(bool canResetCrouch) {
            this.canResetCrouch = canResetCrouch;
        }

        public void SetCanResetCrouch(Transform transform)
        {
            canResetCrouch = GetCanResetCrouch(transform);
        }

        public void SetHeight(float height)
        {
            this.height = height;
        }

        protected override string GetModuleName()
        {
            return "Crouch Initializer";
        }

        protected sealed override bool CheckConditions()
        {
            if (!canResetCrouch) return true;
            return base.CheckConditions();
        }

        public float GetHeightTransitionSpeed() {
            return transitionState ? crouchTransitionSpeed : standTransitionSpeed;
        }

        protected sealed override bool IsValidCondition(MovementStateCondition condition)
        {
            return condition is RunMovementStateCondition ||  condition is GroundMovementStateCondition;
        }

        protected sealed override bool IsShowOppositeResultCondition(MovementStateCondition condition)
        {
            return condition is GroundMovementStateCondition;
        }
    }
}
