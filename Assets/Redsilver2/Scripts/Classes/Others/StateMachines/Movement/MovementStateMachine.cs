using RedSilver2.Framework.StateMachines.Handlers;
using RedSilver2.Framework.StateMachines.States;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace RedSilver2.Framework.StateMachines.Controllers
{
    [RequireComponent(typeof(MovementStateMachineEventHandler))]
    public abstract class MovementStateMachine : UpdatableStateMachine
    {
        [Space]
        [SerializeField] private float groundCheckRange = 0f;

        [Space]
        [SerializeField] private bool  is2DMovement;

        [Space]
        [SerializeField] private float defaultFallSpeed;


        [Space]
        [SerializeField] private MovementStateType defaultState;


        private float moveSpeed;
        private float fallSpeed;

        private bool isGrounded;
        private bool isCrouching;

        private bool isMoving;
        private bool isRunning;

        private string groundTag;

        private MovementState       currentState;
        private List<MovementState> states;

        private UnityEvent<Vector3> onMoved;
        private UnityEvent<string> onGroundTagChanged;

        private UnityEvent<MovementState> onStateAdded, onStateRemoved;
        private UnityEvent<MovementState> onStateEntered, onStateExited;



        public float MoveSpeed => moveSpeed;
        public float FallSpeed => fallSpeed;

        public string GroundTag => groundTag;
        public bool IsMoving => isMoving;

        public bool IsRunning => isRunning;
        public bool IsGrounded => isGrounded;
        public bool IsCrouching => isCrouching;

        public float GroundCheckRange                    => groundCheckRange;
        public bool Is2DMovement                         => is2DMovement;

        public MovementState[] States => states != null ? states.ToArray() : new MovementState[0];  

#if UNITY_EDITOR
        protected virtual void OnValidate() {
            groundCheckRange = Mathf.Clamp(groundCheckRange, 0f, float.MaxValue);
        }
#endif


        protected override void Awake()
        {
            base.Awake();
            states = new List<MovementState>();

            onMoved            = new UnityEvent<Vector3>();
            onGroundTagChanged = new UnityEvent<string>();

            onStateAdded   = new UnityEvent<MovementState>();
            onStateRemoved = new UnityEvent<MovementState>();

            onStateEntered = new UnityEvent<MovementState>();
            onStateExited  = new UnityEvent<MovementState>();

            groundTag = string.Empty;

            isGrounded  = false;
            isCrouching = false;

            isRunning = false;
            isMoving  = false;

            fallSpeed = -10f;
            moveSpeed = 10f;

            AddOnMovedListener(OnMoved);
            AddOnUpdateListener(OnUpdate);

            AddOnLateUpdateListener(OnLateUpdate);
            AddOnGroundTagChangedListener(OnGroundTagChanged);
            
            AddOnStateEnteredListener(OnStateEntered);
            AddOnStateExitedListener(OnStateExited);

            AddOnStateAddedListener(OnStateAdded);
            AddOnStateRemovedListener(OnStateRemoved);
        }

        protected virtual void Start() {
            ChangeState(defaultState);
        }

        protected virtual void OnStateAdded(MovementState state) {
            if (currentState == null) ChangeState(state);
        }

        protected virtual void OnStateRemoved(MovementState state){
            if(currentState == state)  ChangeState(null);
        }

        protected virtual void OnStateEntered(MovementState state)
        {
            currentState = state;
            currentState?.Enter();
        }

        protected virtual void OnStateExited(MovementState state)
        {
            currentState?.Exit();
            currentState = null;
        }


        public bool IsCurrentState(State state)
        {
            return currentState == state;
        }

        public bool IsCurrentState(MovementStateType type)
        {
            if(currentState == null) return false;
            return type == currentState.Type;
        }


        public void ClearCurrentState()
        {
            onStateExited?.Invoke(currentState);
        }

        public void ChangeState(MovementStateType type) {
            ChangeState(GetState(type));
        }

        public void ChangeState(MovementState state)
        {
            if (states == null || this.currentState == state) return;
            else if (states != null && !states.Contains(state)) return;

            onStateExited?.Invoke(currentState);
            onStateEntered?.Invoke(state);
        }

        public void AddState(MovementState state)
        {
            Debug.Log(state + " " + states);


            if (state == null || states == null || states.Contains(state))
                return;



            states?.Add(state);
            onStateAdded?.Invoke(state);
        }

        public void RemoveState(MovementState state) {
            if (state == null || states == null || !states.Contains(state))
                return;

            states?.Remove(state);
            onStateRemoved?.Invoke(state);
        }

        public bool ContainsState(MovementState state)
        {
            return states == null ? false : states.Contains(state);
        }

        public bool ContainsStateType(MovementStateType type) {
            return GetState(type) != null;
        }

        public MovementState GetState(MovementStateType type) {
            if (states == null) return null;

            foreach (MovementState state in states) {
                if (state == null || type != state.Type) continue;
                return state;
            }

            return null;
        }

        protected void SetIsMoving(bool isMoving) {
            this.isMoving = isMoving;
        }


        protected virtual void OnUpdate()
        {
            string currentGroundTag = string.Empty;
            isGrounded = GetGroundCheckResult(out currentGroundTag);
         
            currentGroundTag = currentGroundTag.ToLower();
            if (!groundTag.ToLower().Equals(currentGroundTag)) onGroundTagChanged?.Invoke(currentGroundTag);
        }

        protected virtual void OnLateUpdate()
        {
            Move();
        }

        protected virtual void OnGroundTagChanged(string groundTag)
        {
            this.groundTag = groundTag;
        }

        protected virtual void OnDisabled()
        {
            this.isGrounded = true;
            this.isCrouching = false;

            this.isMoving = false;
            this.isRunning = false;
        }



        public void AddOnMovedListener(UnityAction<Vector3> action)
        {
            if (action != null) onMoved?.AddListener(action);
        }
        public void RemoveOnMoveListener(UnityAction<Vector3> action)
        {
            if (action != null) onMoved?.RemoveListener(action);
        }

        public void AddOnGroundTagChangedListener(UnityAction<string> action)
        {
            if (action != null) onGroundTagChanged?.AddListener(action);
        }
        public void RemoveOnGroundTagChangedListener(UnityAction<string> action)
        {
            if (action != null) onGroundTagChanged?.RemoveListener(action);
        }

        public void AddOnStateAddedListener(UnityAction<MovementState> action)
        {
            if (action != null) onStateAdded?.AddListener(action);
        }
        public void RemoveOnStateAddedListener(UnityAction<MovementState> action)
        {
            if (action != null) onStateAdded?.RemoveListener(action);
        }

        public void AddOnStateRemovedListener(UnityAction<MovementState> action)
        {
            if (action != null) onStateRemoved?.AddListener(action);
        }
        public void RemoveOnStateRemovedListener(UnityAction<MovementState> action)
        {
            if (action != null) onStateRemoved?.RemoveListener(action);
        }

        public void AddOnStateEnteredListener(UnityAction<MovementState> action)
        {
            if (action != null) onStateEntered?.AddListener(action);
        }
        public void RemoveOnStateEnteredListener(UnityAction<MovementState> action)
        {
            if (action != null) onStateEntered?.RemoveListener(action);
        }

        public void AddOnStateExitedListener(UnityAction<MovementState> action)
        {
            if (action != null) onStateExited?.AddListener(action);
        }
        public void RemoveOnStateExitedListener(UnityAction<MovementState> action)
        {
            if (action != null) onStateExited?.RemoveListener(action);
        }

        public virtual void SetMoveSpeed(float moveSpeed)
        {
            this.moveSpeed = moveSpeed;
        }

        public void SetMoveSpeed(float moveSpeed, float transitionSpeed)
        {
            SetMoveSpeed(Mathf.Lerp(this.moveSpeed, moveSpeed, Time.deltaTime * transitionSpeed));
        }

        public void SetFallSpeed(float fallSpeed)
        {
            this.fallSpeed = fallSpeed;
        }

        public void SetFallSpeed(float fallSpeed, float transitionSpeed)
        {
            SetFallSpeed(Mathf.Lerp(this.fallSpeed, fallSpeed, Time.deltaTime * transitionSpeed));
        }


        public void SetIsRunning(bool isRunning)
        {
            this.isRunning = CanRun() ? isRunning : false;
        }

        private bool GetGroundCheckResult(out string groundTag) {     
            groundTag = string.Empty;

            // Do 2D Ground Check Here...
            if (Is2DMovement) return true;
            else return Get3DGroundCheckResult(groundCheckRange, out groundTag);
        }

        private bool Get3DGroundCheckResult(float groundCheckRange, out string groundTag)
        {
            groundTag = string.Empty;

            if (Physics.Raycast(transform.position, -transform.up, out RaycastHit hitInfo, groundCheckRange, ~GameManager.PlayerLayer))
            {
                if (hitInfo.collider == null) return false;
                else if (hitInfo.collider.gameObject.layer == GameManager.GroundLayer) {
                    groundTag = hitInfo.collider.tag;
                    return true;
                }
            }

            return false;
        }


        protected abstract void Move();

        public void Move(Vector3 nextPosition){
            onMoved?.Invoke(nextPosition);
        }

        protected virtual bool CanRun() {
            if (!isMoving || !isGrounded || isCrouching) return false;
            return true;
        }

        protected abstract void OnMoved(Vector3 nextPosition);

    }
}
