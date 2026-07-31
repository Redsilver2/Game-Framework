
using RedSilver2.Framework.StateMachines.States;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace RedSilver2.Framework.StateMachines
{
    public abstract class MovementStateMachine : UpdatableStateMachine
    {
        [Space]
        [SerializeField] private float groundCheckRange = 0f;

        [Space]
        [SerializeField] private bool is2DMovement;

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

        private float airbornTime;

        private MovementState currentState;
        private Dictionary<MovementStateType, MovementState> states;

        private UnityEvent<Vector3> onMoved;
        private UnityEvent<string> onGroundTagChanged;

        private UnityEvent<MovementState> onMovementStateAdded, onMovementStateRemoved;
        private UnityEvent<MovementState> onMovementStateEntered, onMovementStateExited;

        public float DefaultHeight    => defaultHeight;
        public float DefaultFallSpeed => defaultFallSpeed;

        public float MoveSpeed => moveSpeed;
        public float FallSpeed => fallSpeed;

        public string GroundTag => groundTag;
        public bool IsMoving    => isMoving;
        public bool IsGrounded  => isGrounded;
        public float AirbornTime => airbornTime;

        public float GroundCheckRange => groundCheckRange;
        public bool  Is2DMovement     => is2DMovement;


        public MovementState[] States => states != null ? states.Values.ToArray() : new MovementState[0];

#if UNITY_EDITOR
        protected virtual void OnValidate() {
            groundCheckRange = Mathf.Clamp(groundCheckRange, 0f, float.MaxValue);
        }
#endif


        protected override void Awake()
        {
            states = new Dictionary<MovementStateType, MovementState>();
            base.Awake();

            onMoved = new UnityEvent<Vector3>();
            onGroundTagChanged = new UnityEvent<string>();

            onMovementStateAdded = new UnityEvent<MovementState>();
            onMovementStateRemoved = new UnityEvent<MovementState>();

            onMovementStateEntered = new UnityEvent<MovementState>();
            onMovementStateExited = new UnityEvent<MovementState>();

            groundTag = string.Empty;
            isGrounded = false;

            isMoving  = false;
            fallSpeed = defaultFallSpeed;

            moveSpeed = 0f;
            AddOnMovedListener(OnMoved);
            
            AddOnGroundTagChangedListener(OnGroundTagChanged);
        }

        protected async virtual void Start() {
            while (!states.ContainsKey(defaultState)) await Awaitable.NextFrameAsync();
            ChangeState(defaultState);
        }

        protected sealed override bool CanAddState(State state)
        {
            if (states != null && base.CanAddState(state)) {
                MovementState _state = state as MovementState;

                if (_state == null || states.ContainsKey(_state.Type)) return false;
                return true;
            }

            return false;
        }

        protected override void OnUpdatableStateAdded(UpdatableState state) {
            base.OnUpdatableStateAdded(state);
            OnMovementStateAdded(state as MovementState);
        }
        protected virtual void OnMovementStateAdded(MovementState state) {
            if(states == null || state == null || states.ContainsKey(state.Type)) return;
            states?.Add(state.Type, state);
            onMovementStateAdded?.Invoke(state);
        }

        protected override void OnUpdatableStateRemoved(UpdatableState state)
        {
            base.OnUpdatableStateAdded(state);
            OnMovementStateRemoved(state as MovementState);
        }
        protected virtual void OnMovementStateRemoved(MovementState state)
        {
            if (currentState == state) ChangeState(null);

            if (states == null || state == null || !states.ContainsKey(state.Type)) return;
            else if (states[state.Type] == state) {
                states.Remove(state.Type);
                onMovementStateRemoved?.Invoke(state);
            }
        }

        protected override void OnUpdatableStateEntered(UpdatableState state)
        {
            base.OnUpdatableStateAdded(state);
            OnMovementStateEntered(state as MovementState);
        }
        protected virtual void OnMovementStateEntered(MovementState state)
        {
            currentState = state;
            onMovementStateEntered?.Invoke(state);
        }

        protected override void OnUpdatableStateExited(UpdatableState state)
        {
            base.OnUpdatableStateAdded(state);
            OnMovementStateExited(state as MovementState);
        }
        protected virtual void OnMovementStateExited(MovementState state)
        {
            currentState = null;
            onMovementStateExited?.Invoke(state);
        }

        protected override void OnDisabled() {
            base.OnDisabled();
            this.isGrounded = true;
            this.isMoving = false;
        }

        public void DisableState(MovementStateType type) {
            GetState(type)?.SetIsActif(false);
        }
        public void EnableState(MovementStateType type)
        {
            GetState(type)?.SetIsActif(true);
        }


        public bool IsCurrentState(MovementStateType type)
        {
            if (currentState == null) return false;

            return type == currentState.Type;
        }
        public void ChangeState(MovementStateType type) {
            ChangeState(GetState(type));
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

            onMovementStateRemoved?.Invoke(states[type]);
            states?.Remove(type);
        }

        public bool ContainsState(MovementStateType type) {
            return states != null ? states.ContainsKey(type) : false;
        }
        public MovementState GetState(MovementStateType type) {
            if (states == null || !states.ContainsKey(type)) return null;
            return states[type];
        }

        protected void SetIsMoving(bool isMoving) {
            this.isMoving = isMoving;
        }

        protected override void OnUpdate()
        {
            string currentGroundTag = string.Empty;

            isGrounded = IsCurrentState(MovementStateType.Jump) ? false :  GetGroundCheckResult(out currentGroundTag);
            currentGroundTag = currentGroundTag.ToLower();

            if (!groundTag.ToLower().Equals(currentGroundTag)) onGroundTagChanged?.Invoke(currentGroundTag);

            if (isGrounded) { airbornTime = 0f; }
            else { airbornTime += Time.deltaTime;  }

            airbornTime = Mathf.Clamp(airbornTime, 0f, float.MaxValue);

            if (!IsCurrentState(MovementStateType.Fall))   SetFallSpeed(defaultFallSpeed, fallTransitionSpeed);
            if (!IsCurrentState(MovementStateType.Crouch)) {
                SetHeight(defaultHeight, heightTransitionSpeed);
            }
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

        public void AddOnMovementStateAddedListener(UnityAction<MovementState> action)
        {
            if (action != null) onMovementStateAdded?.AddListener(action);
        }
        public void RemoveOnMovementStateAddedListener(UnityAction<MovementState> action)
        {
            if (action != null) onMovementStateAdded?.RemoveListener(action);
        }

        public void AddOnMovementStateRemovedListener(UnityAction<MovementState> action)
        {
            if (action != null) onMovementStateRemoved?.AddListener(action);
        }
        public void RemoveOnMovementStateRemovedListener(UnityAction<MovementState> action)
        {
            if (action != null) onMovementStateRemoved?.RemoveListener(action);
        }

        public void AddOnMovementStateEnteredListener(UnityAction<MovementState> action)
        {
            if (action != null) onMovementStateEntered?.AddListener(action);
        }
        public void RemoveOnMovementStateEnteredListener(UnityAction<MovementState> action)
        {
            if (action != null) onMovementStateEntered?.RemoveListener(action);
        }

        public void AddOnMovementStateExitedListener(UnityAction<MovementState> action)
        {
            if (action != null) onMovementStateExited?.AddListener(action);
        }
        public void RemoveOnMovementStateExitedListener(UnityAction<MovementState> action)
        {
            if (action != null) onMovementStateExited?.RemoveListener(action);
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

        protected virtual bool GetGroundCheckResult(out string groundTag) {
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

        public void Move(Vector3 nextPosition) {
            nextPosition = transform.right   * nextPosition.x +
                           transform.up      * nextPosition.y +
                           transform.forward * nextPosition.z;

            onMoved?.Invoke(nextPosition);
        }

        protected abstract void OnMoved(Vector3 nextPosition);

        public virtual void SetHeight(float height) {
            transform.localScale = Vector3.right * transform.localScale.x +
                                   Vector3.up * height +
                                   Vector3.forward * transform.localScale.z;
        }

        public virtual void SetHeight(float height, float transitionSpeed)
        {
            SetHeight(Mathf.Clamp(transform.localScale.y, height, Time.deltaTime * transitionSpeed));
        }
    }
}
