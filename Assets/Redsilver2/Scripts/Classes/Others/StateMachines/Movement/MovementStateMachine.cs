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
        [SerializeField] private float fallTransitionSpeed;

        [Space]
        [SerializeField] private float defaultHeight;
        [SerializeField] private float heightTransitionSpeed;

        [Space]
        [SerializeField] private MovementStateType defaultState;

        private float moveSpeed;
        private float fallSpeed;

        private bool isGrounded;

        private bool isMoving;
        private string groundTag;

        private MovementState       currentState;
        private Dictionary<MovementStateType, MovementState> states;

        private UnityEvent<Vector3> onMoved;
        private UnityEvent<string> onGroundTagChanged;

        private UnityEvent<MovementState> onStateAdded, onStateRemoved;
        private UnityEvent<MovementState> onStateEntered, onStateExited;

        public float DefaultHeight => defaultHeight;
        public float DefaultFallSpeed => defaultFallSpeed;

        public float MoveSpeed => moveSpeed;
        public float FallSpeed => fallSpeed;

        public string GroundTag   => groundTag;
        public bool   IsMoving    => isMoving;
        public bool   IsGrounded  => isGrounded;

        public float GroundCheckRange                    => groundCheckRange;
        public bool Is2DMovement                         => is2DMovement;

        public MovementState[] States => states != null ? states.Values.ToArray() : new MovementState[0];  

#if UNITY_EDITOR
        protected virtual void OnValidate() {
            groundCheckRange = Mathf.Clamp(groundCheckRange, 0f, float.MaxValue);
        }
#endif


        protected override void Awake()
        {
            base.Awake();
            states = new Dictionary<MovementStateType, MovementState>();

            onMoved            = new UnityEvent<Vector3>();
            onGroundTagChanged = new UnityEvent<string>();

            onStateAdded   = new UnityEvent<MovementState>();
            onStateRemoved = new UnityEvent<MovementState>();

            onStateEntered = new UnityEvent<MovementState>();
            onStateExited  = new UnityEvent<MovementState>();

            groundTag = string.Empty;
            isGrounded  = false;

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

            AddOnDisabledListener(OnDisabled);
            AddOnEnabledListener(OnEnabled);
        }

        protected virtual void Start() {
            ChangeState(defaultState);
        }

        protected virtual void OnEnabled() {
            if(states != null) {
                foreach(MovementStateType  type in states.Keys) {  
                    EnableState(type);
                }
            }
        }

        protected virtual void OnDisabled()
        {
            if (states != null)
            {
                foreach (MovementStateType type in states.Keys) {
                    DisableState(type);
                }
            }

            this.isGrounded = true;
            this.isMoving = false;
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

        public void DisableState(MovementStateType type) {
            GetState(type)?.SetIsActif(false);
        }
        public void EnableState(MovementStateType type)
        {
            GetState(type)?.SetIsActif(true);
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

        private void ChangeState(MovementState state) {        
            if (states == null || this.currentState == state) return;
            onStateExited?.Invoke(currentState);
            onStateEntered?.Invoke(state);
        }
        public void AddState(MovementState state) {

            if (state == null || states == null || states.ContainsKey(state.Type))
                return;

            states?.Add(state.Type, state);
            onStateAdded?.Invoke(state);

            if (state != null) state?.SetIsActif(enabled);
        }

        public void RemoveState(MovementState state)
        {
            if (states == null || state == null || !states.ContainsKey(state.Type)) return;
            else if (states[state.Type] == state) {
                RemoveState(state.Type);
            }
        }
        private void RemoveState(MovementStateType type) {
            if (states == null || !states.ContainsKey(type)) return;

            onStateRemoved?.Invoke(states[type]);
            states?.Remove(type);
        }

        public bool ContainsStateType(MovementStateType type) {
            return GetState(type) != null;
        }
        public MovementState GetState(MovementStateType type) {
            if (states == null || !states.ContainsKey(type)) return null;
            return states[type];
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

            if (!IsCurrentState(MovementStateType.Fall))   SetFallSpeed(defaultFallSpeed, fallTransitionSpeed);
            if (!IsCurrentState(MovementStateType.Crouch)) SetHeight(defaultHeight, heightTransitionSpeed);
        }

        protected virtual void OnLateUpdate() {
            Move();
        }

        protected virtual void OnGroundTagChanged(string groundTag)
        {
            this.groundTag = groundTag;
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

        public void Move(Vector3 nextPosition){
            onMoved?.Invoke(nextPosition);
        }

        protected abstract void Move();
        protected abstract void OnMoved(Vector3 nextPosition);

        public virtual void SetHeight(float height) {
            transform.localScale = Vector3.right   * transform.localScale.x + 
                                   Vector3.up      * height + 
                                   Vector3.forward * transform.localScale.z;
        }

        public virtual void SetHeight(float height, float transitionSpeed)
        {
            SetHeight(Mathf.Clamp(transform.localScale.y, height, Time.deltaTime * transitionSpeed));
        }

    }
}
