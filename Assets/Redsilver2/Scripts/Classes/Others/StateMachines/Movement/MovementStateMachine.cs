
using RedSilver2.Framework.StateMachines.States;
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
        [SerializeField] private IdolState idolState;

        [Space]
        [SerializeField] private WalkState walkState;

        [Space]
        [SerializeField] private FallState fallState;

        private LandState landState;


        private float moveSpeed;
        private float fallSpeed;

        private bool isGrounded;

        private bool isMoving;
        private string groundTag;

        private float airbornTime;
        private UnityEvent<Vector3> onMoved;
        private UnityEvent<string> onGroundTagChanged;

        private UnityEvent<MovementState> onStateAdded, onStateRemoved;
        private UnityEvent<MovementState> onStateEntered, onStateExited;

        public float DefaultHeight    => defaultHeight;
        public float DefaultFallSpeed => defaultFallSpeed;

        public float MoveSpeed => moveSpeed;
        public float FallSpeed => fallSpeed;

        public string GroundTag        => groundTag;
        public bool   IsMoving         => isMoving;

        public bool   IsGrounded       => isGrounded;
        public float  AirbornTime      => airbornTime;

        public float  GroundCheckRange => groundCheckRange;
        public bool   Is2DMovement     => is2DMovement;

        public IdolState IdolState => idolState;
        public WalkState WalkState => walkState;
        public FallState FallState => fallState;
        public LandState LandState => landState;

#if UNITY_EDITOR
        protected override void OnValidate() {
            base.OnValidate();
            ValidateStates();

            groundCheckRange = Mathf.Clamp(groundCheckRange, 0f, float.MaxValue);
        }

        protected virtual void ValidateStates() {
            if (walkState == null) walkState = new WalkState();
            walkState?.Validate(this);

            if(fallState == null) fallState = new FallState();
            fallState?.Validate(this);

            if(landState == null) landState = new LandState();
            landState?.Validate(this);

            if(idolState == null) idolState = new IdolState();
            idolState?.Validate(this);
        }
#endif


        protected override void Awake()
        {
            base.Awake();

            onMoved            = new UnityEvent<Vector3>();
            onGroundTagChanged = new UnityEvent<string>();

            onStateAdded   = new UnityEvent<MovementState>();
            onStateRemoved = new UnityEvent<MovementState>();

            onStateEntered = new UnityEvent<MovementState>();
            onStateExited  = new UnityEvent<MovementState>();

            groundTag  = string.Empty;
            isGrounded = false;

            isMoving  = false;
            fallSpeed = defaultFallSpeed;
            moveSpeed = 0f;
          
            AddOnMovedListener(OnMoved);        
            AddOnGroundTagChangedListener(OnGroundTagChanged);
        }

        protected virtual void Start()
        {
            idolState?.Enable();
            walkState?.Enable();

            landState?.Enable();
            fallState?.Enable();

            AddState(idolState);
            AddState(walkState);

            AddState(landState);
            AddState(fallState);
        }

        public void ResetAirbornTime()
        {
            airbornTime = 0f;
        }

        protected sealed override bool CanAddState(UpdatableState state)
        {
            return CanAddState(state as MovementState);
        }

        private bool CanAddState(MovementState state)
        {
            if (state == null || GetState(state.Type) != null) return false;
            return true;
        }

        protected sealed override void OnStateAdded(UpdatableState state) {
            base.OnStateAdded(state);
            OnStateAdded(state as MovementState);
        }
        protected virtual void OnStateAdded(MovementState state) {
            onStateAdded?.Invoke(state);
        }

        protected sealed override void OnStateRemoved(UpdatableState state)
        {
            base.OnStateRemoved(state);
            OnStateRemoved(state as MovementState);
        }
        protected sealed override void OnStateEntered(UpdatableState state)
        {
            base.OnStateEntered(state);
            OnStateEntered(state as MovementState);
        }
        protected virtual void OnStateEntered(MovementState state) {
            onStateEntered?.Invoke(state);
        }

        protected sealed override void OnStateExited(UpdatableState state)
        {
            base.OnStateExited(state);
            OnStateExited(state as MovementState);
        }

        protected virtual void OnStateExited(MovementState state)
        {
            onStateExited?.Invoke(state);
        }

        protected override void OnDisabled() {
            base.OnDisabled();
            this.isGrounded = true;
            this.isMoving = false;
        }

        public void DisableState(MovementStateType type) {
            GetState(type)?.Disable();
        }
        public void EnableState(MovementStateType type)
        {
            GetState(type)?.Enable();
        }


        public bool IsCurrentState(MovementStateType type)
        {
           MovementState state = CurrentState as MovementState;
           return state != null ? state.Type == type : false;
        }

        public bool ChangeState(MovementStateType type)
        {
            return ChangeState(type, true);
        }

        public virtual bool ChangeState(MovementStateType type, bool checkSimilarity) {
            switch (type)
            {
                case MovementStateType.Idol: ChangeState(idolState, checkSimilarity); return true;
                case MovementStateType.Walk: ChangeState(walkState, checkSimilarity); return true;
                case MovementStateType.Fall: ChangeState(fallState, checkSimilarity); return true;
                case MovementStateType.Land: ChangeState(landState, checkSimilarity); return true;
            }

            return false;
        }

        public virtual void AddState(MovementStateType type)
        {
            if      (type == MovementStateType.Idol) AddState(idolState);
            else if (type == MovementStateType.Walk) AddState(walkState);
            else if (type == MovementStateType.Fall) AddState(fallState);
        }


        public virtual void RemoveState(MovementStateType type) {
            if      (type == MovementStateType.Idol) RemoveState(idolState);
            else if (type == MovementStateType.Walk) RemoveState(walkState);
            else if (type == MovementStateType.Fall) RemoveState(fallState);
        }

        public bool ContainsState(MovementStateType type) {
            return GetState(type) != null;
        }
        public MovementState GetState(MovementStateType type) {
          
            foreach(State state in States) {
                MovementState _state = state as MovementState;
                if (_state == null || _state.Type != type) continue;

                return _state;
            }

            return null;
        }

        protected void SetIsMoving(bool isMoving) {
            this.isMoving = isMoving;
        }

        protected override void OnUpdate()
        {
            base.OnUpdate();

            string currentGroundTag = string.Empty;

            isGrounded = IsCurrentState(MovementStateType.Jump) ? false :  GetGroundCheckResult(out currentGroundTag);
            currentGroundTag = currentGroundTag.ToLower();

            if (!groundTag.ToLower().Equals(currentGroundTag)) onGroundTagChanged?.Invoke(currentGroundTag);

            if (isGrounded) { airbornTime = 0f; }
            else { airbornTime += Time.deltaTime;  }

            airbornTime = Mathf.Clamp(airbornTime, 0f, float.MaxValue);

            if (!IsCurrentState(MovementStateType.Fall)) SetFallSpeed(defaultFallSpeed, fallTransitionSpeed);
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
