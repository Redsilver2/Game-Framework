using RedSilver2.Framework.StateMachines.Controllers;
using UnityEngine;
using UnityEngine.Windows;

namespace RedSilver2.Framework.StateMachines.States.Movement
{
    [System.Serializable]
    public abstract class MovementHandler {
        protected bool  use2DMovement;
        private   float moveSpeed;

        private Vector3 nextPosition;
        private FallStateInitializer fallStateInitializer;
        private MovementStateMachine stateMachine;

        public float   MoveSpeed    => moveSpeed;    
        public Vector3 NextPosition => nextPosition;

        protected MovementHandler(MovementStateMachineController controller) {
            this.use2DMovement  = false;  
        }

        protected MovementHandler(MovementStateMachineController controller, bool use2DMovement) {
            this.use2DMovement  = use2DMovement;
        }

        public void SetStateMachine(MovementStateMachine stateMachine) {
            if(this.stateMachine == null) {
                this.stateMachine = stateMachine;
                stateMachine?.AddOnUpdateListener(Update);
                stateMachine?.AddOnLateUpdateListener(LateUpdate);

                stateMachine?.AddOnEnabledListener(Enable);
                stateMachine?.AddOnDisabledListener(Disable);
                stateMachine?.AddOnStateModuleAddedListener(OnStateModuleAdded);
            }
        }


        private void OnStateModuleAdded(StateModule module)
        {
            if(module as FallStateInitializer) fallStateInitializer = module as FallStateInitializer;
        }


        protected virtual void Update() {
            if (stateMachine == null || stateMachine.Controller == null) return;
            nextPosition = GetNextPosition(GetTransform(), moveSpeed, GetFallSpeed(), false);
        }

        protected virtual void LateUpdate() {
            Move(Time.deltaTime * nextPosition);
        }

        protected abstract void Enable();
        protected virtual void Disable() {
            nextPosition = Vector3.zero;
        }

        public void SetMoveSpeed(float moveSpeed) {
            this.moveSpeed = Mathf.Clamp(moveSpeed, Mathf.Epsilon, Mathf.Infinity);
        }

        public void UpdateMoveSpeed(float nextMoveSpeed, float updateSpeed) {
            if(nextMoveSpeed >= Mathf.Epsilon)
               this.moveSpeed = Mathf.Lerp(moveSpeed, nextMoveSpeed, Time.deltaTime * updateSpeed);
        }

        private float GetFallSpeed()
        {
            if (fallStateInitializer == null || use2DMovement) return 0f;
            return fallStateInitializer.CurrentFallSpeed;
        }

        public void Jump(float jumpForce)
        {
            fallStateInitializer?.SetJumpHeight(jumpForce);
        }

        public abstract void Crouch(float height, float speed);
        public abstract void Move(Vector3 position);
        public abstract float GetMagnitude();

        protected abstract Vector3   GetNextPosition(Transform transform, float moveSpeed, float fallSpeed, bool use2DMovement);
        public    abstract Transform GetTransform();
    }
}
