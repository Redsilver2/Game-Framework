using RedSilver2.Framework.StateMachines.Controllers;
using UnityEngine;

namespace RedSilver2.Framework.StateMachines.States.Movement
{
    [System.Serializable]
    public abstract class MovementHandler {
        private float height;


        protected bool use2DMovement;
        protected bool isRunning;

        protected bool isCrouching;
        protected bool canResetCrouch;

        protected bool isGrounded;
        protected bool isJumping;

        protected string groundTag;


        private   float moveSpeed;
        protected float fallSpeed;
        protected Vector3 nextPosition;

        private MovementStateMachine stateMachine;

        public float MoveSpeed => moveSpeed;
        public float FallSpeed => fallSpeed;

        public bool   IsCrouching => isCrouching;
        public bool   IsRunning => isRunning;
        public bool   IsJumping => isJumping;
        public bool   IsGrounded => isGrounded;
        public string GroundTag => groundTag;

        public const float DEFAULT_CROUCH_SPEED = 5f;



        protected MovementHandler(MovementStateMachineController controller) {
            this.use2DMovement = false;
           
            this.isGrounded     = true;
            this.isJumping      = false;

            this.isCrouching    = false;
            this.isRunning      = false;

            this.canResetCrouch = true;
            this.groundTag      = string.Empty;
            
            if (controller != null) {
                moveSpeed = controller.WalkSpeed;
                fallSpeed = controller.DefaultGravitySpeed;
                height    = controller.StandHeight;
            }
        }

        protected MovementHandler(MovementStateMachineController controller, bool use2DMovement) {
            this.use2DMovement = use2DMovement;
            
            this.isGrounded  = true;
            this.isJumping   = false;

            this.isCrouching = false;
            this.isRunning   = false;

            this.canResetCrouch = true;
            this.groundTag      = string.Empty;

            if(controller != null) { 
                moveSpeed = controller.WalkSpeed;
                fallSpeed = controller.DefaultGravitySpeed;
                height = controller.StandHeight;
            }
        }

        public void SetStateMachine(MovementStateMachine stateMachine) {
            if(this.stateMachine == null) {
                this.stateMachine = stateMachine;
                stateMachine?.AddOnUpdateListener(Update);
                stateMachine?.AddOnLateUpdateListener(LateUpdate);
              
                stateMachine?.AddOnEnabledListener(Enable);
                stateMachine?.AddOnDisabledListener(Disable);
            }
        }



        protected virtual void Update() {

            if(!isGrounded) {
                isJumping   = false;
               if(canResetCrouch) isCrouching = false;
            }

            if (stateMachine == null || stateMachine.Controller == null) return;
            nextPosition = GetNextPosition(GetTransform(), moveSpeed, fallSpeed, stateMachine.Controller != null ?  false : false);
        }

        protected virtual void LateUpdate() {
            Crouch(height);
            Move(nextPosition);
        }

        protected abstract void Enable();
        protected abstract void Disable();

        private float GetMoveSpeed(StateMachineController controller) {
            if (controller == null) return 0f;
            else if (!isGrounded)   return 5f * 0.5f;
            else if (isCrouching)   return 5f * 0.25f;
            else if (isRunning)     return 10f;
            
            return 5f;
        }

        private float GetHeight(StateMachineController controller)
        {
            if(controller == null) return 0f;
            return isCrouching ? 1f : 2f;
        }

        private float GetGravity(StateMachineController controller) {
            if (controller == null) return 0f;
            return isGrounded ? -10f : -20;
        }

        public void SetCanResetCrouch(bool canResetCrouch) {
            this.canResetCrouch = canResetCrouch;
        }

        public void SetMoveSpeed(float moveSpeed) {
            this.moveSpeed = Mathf.Clamp(moveSpeed, Mathf.Epsilon, Mathf.Infinity);
        }


        public void UpdateMoveSpeed(float nextMoveSpeed, float updateSpeed) {
            if(nextMoveSpeed >= Mathf.Epsilon)
               this.moveSpeed = Mathf.Lerp(moveSpeed, nextMoveSpeed, Time.deltaTime * updateSpeed);
        }

        public void SetFallSpeed(float fallSpeed) {
            this.fallSpeed = Mathf.Clamp(fallSpeed, float.MinValue, 0f);
        }
        public void SetJumpHeight(float jumpHeight) {
            this.fallSpeed = Mathf.Clamp(jumpHeight, 0f, float.MaxValue);
        }

        public void UpdateFallSpeed(float nextFallSpeed, float updateSpeed) {
            if (nextFallSpeed <= 0f)
              this.fallSpeed = Mathf.Lerp(fallSpeed, nextFallSpeed, Time.deltaTime * updateSpeed);
        }



        public void UpdateHeight(float nextHeight, float updateSpeed) {
                this.height = Mathf.Lerp(height, nextHeight, Time.deltaTime * updateSpeed);
        }

        protected abstract void Crouch(float height);
        protected abstract void Move(Vector3 position);

        public    abstract float GetMoveMagnitude();
        public    abstract float GetGroundCheckRange();
        protected abstract bool  IsOnGround(out string groundTag, Transform transform);

        protected abstract Vector3 GetNextPosition(Transform transform, float moveSpeed, float fallSpeed, bool use2DMovement);
        public abstract Transform GetTransform();
    }
}
