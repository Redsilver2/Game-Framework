using RedSilver2.Framework.StateMachines.Controllers;
using RedSilver2.Framework.StateMachines.States;
using RedSilver2.Framework.StateMachines.States.Conditions;
using RedSilver2.Framework.StateMachines.States.Configurations;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;


namespace RedSilver2.Framework.StateMachines {
    [System.Serializable]
    public abstract class MovementStateMachine : UpdateableStateMachine
    {
        private float moveSpeed;
        private float fallSpeed;

        private bool isGrounded;
        private bool isCrouching;

        private bool  isMoving;
        private bool  isRunning;

        private string groundTag;

        public readonly  Transform           Transform;
        private readonly UnityEvent<Vector2> onMoved;

        public string GroundTag   => groundTag;
        public bool   IsMoving    => isMoving;

        public bool   IsRunning   => isRunning; 
        public bool   IsGrounded  => isGrounded;
        public bool   IsCrouching => isCrouching; 

        protected MovementStateMachine(UpdateableStateMachineController controller) : base(controller) {
            Transform  = controller == null ? null : controller.transform;
            isGrounded = false;
            isCrouching = false;

            isRunning = false;
            isMoving  = false;

            fallSpeed = -10f;
            moveSpeed = 10f;

            onMoved    = new UnityEvent<Vector2>();

           AddOnStateEnteredListener(state => {
                if (state == null) return;
                Debug.LogWarning("Current State: " + state);
           });

            AddOnStateExitedListener(state => {
                if (state == null) return;
                Debug.LogWarning("Previous State: " + state);
            });


            AddOnMovedListener(input => {
                if (Mathf.Abs(input.x) > 0f || Mathf.Abs(input.y) > 0f)
                    this.isMoving = true;
                else
                    this.isMoving = false;

                Debug.Log("Is Moving: " + isMoving);
            });

            AddOnDisabledListener(()    => { 
                this.isGrounded  = true;     
                this.isCrouching = false;

                this.isMoving   = false;
                this.isRunning  = false;
            });

            AddOnUpdateListener(() => {
                if (controller != null) isGrounded = GetGroundCheckResult(out groundTag);
                else                    isGrounded = false;

                if (!isMoving || !isGrounded || isCrouching) isRunning = false;
            });

            AddOnLateUpdateListener(() => { Move(); });
        }

        public void AddOnMovedListener(UnityAction<Vector2> action) {
            if (action != null) onMoved?.AddListener(action);
        }

        public void RemoveOnMovedListener(UnityAction<Vector2> action) {
             if(action != null) onMoved?.RemoveListener(action);
        }

        public override void AddStateConfiguration(StateConfiguration configuration)
        {
            if (configuration is MovementStateConfiguration)
                base.AddStateConfiguration(configuration);
        }

        public void ChangeState(MovementStateType stateType) {

            MovementStateConfiguration configuration = GetStateConfiguration(stateType);
            if(configuration == null) ChangeState(stateType);
        }

        public bool ContainsState(MovementStateType stateType) {
            return GetStateConfiguration(stateType) != null ? true : false;
        }

        public MovementStateConfiguration GetStateConfiguration(MovementStateType stateType) {
            var results01 = GetStateConfigurations().Where(x => x is MovementStateConfiguration).ToArray() as MovementStateConfiguration[];
            if (results01 == null) return null;

            var results02  = results01.Where(x => x.Type == stateType);
            return results02.Count() > 0 ? results02.First() : null;
        }

        public void SetMovementSpeed(float moveSpeed) {
            this.moveSpeed = moveSpeed;
        }

        public void SetFallSpeed(float fallSpeed) {
            this.fallSpeed = fallSpeed;
        }

        public void SetIsRunning(bool isRunning) {
            this.isRunning = isRunning;
        }

        protected void SetIsMoving(bool isMoving)
        {
            this.isMoving = isMoving;
        }

        public float GetMoveSpeed() {
            return moveSpeed;
        }

        public float GetFallSpeed() {
            return fallSpeed;
        }

        private bool GetGroundCheckResult(out string groundTag)
        {
            MovementStateMachineController controller = Controller as MovementStateMachineController;
            groundTag = string.Empty;

            if (controller == null || Transform == null) return false;

            if (controller.Is2DMovement)
                return true;

            return Get3DGroundCheckResult(controller.GroundCheckRange, out groundTag);
        }

        private bool Get3DGroundCheckResult(float groundCheckRange, out string groundTag)
        {
            groundTag = string.Empty;
            if (Transform == null) return false;

            if (Physics.Raycast(Transform.position, -Transform.up, out RaycastHit hitInfo, groundCheckRange, ~GameManager.PlayerLayer)) {
                if (hitInfo.collider == null) return false;

                if (hitInfo.collider.gameObject.layer == GameManager.GroundLayer) {
                    groundTag = hitInfo.collider.tag;
                    return true;
                }
            }

            return false;
        }


        protected abstract void Move();

        public void Move(Vector3 nextPosition, float fallSpeed, float moveSpeed) {
            if (Transform != null) {
                Move(nextPosition);
            }
        }

        public virtual void Move(Vector3 nextPosition) {
            onMoved?.Invoke(Vector2.right * nextPosition.x + Vector2.up * nextPosition.z);  
        }
    }
}