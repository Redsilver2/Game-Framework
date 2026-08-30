
using RedSilver2.Framework.StateMachines.States;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace RedSilver2.Framework.StateMachines
{
    public abstract class MovementStateMachine : UpdatableStateMachine
    {
        [Space]
        [SerializeField, SerializeReference] private List<Movement> movements;


        [Space]
        [SerializeField] private float groundCheckRange = 0f;

        [Space]
        [SerializeField] private bool is2DMovement;

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

        private Collider _collider;

        private static readonly Dictionary<ulong, MovementStateMachine> instances = new Dictionary<ulong, MovementStateMachine>();

        public float MoveSpeed => moveSpeed;
        public float FallSpeed => fallSpeed;

        public string GroundTag => groundTag;
        public bool IsMoving    => isMoving;

        public bool IsGrounded   => isGrounded;
        public float AirbornTime => airbornTime;

        public float GroundCheckRange => groundCheckRange;
        public bool  Is2DMovement     => is2DMovement;

#if UNITY_EDITOR
        protected override void OnValidate() {
            base.OnValidate();

            if(movements == null) movements = new List<Movement>();
            else {
                foreach (Movement movement in movements) movement?.Validate(this);
            }
          
            groundCheckRange = Mathf.Clamp(groundCheckRange, 0f, float.MaxValue);
            if (GetMovement(MovementStateType.Idol) == null) AddMovement(new Idol());

        }

        protected abstract void SetMovementStaetMachineType(ref MovementStateMachineType type);
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

            groundTag = string.Empty;
            isGrounded = false;

            isMoving  = false;
            moveSpeed = 0f;

            AddOnMovedListener(OnMoved);
            AddOnGroundTagChangedListener(OnGroundTagChanged);
        }

        protected virtual void Start()
        {
            if (movements != null)
            {
                foreach (Movement movement in movements)  {
                    MovementState state = movement != null ? movement.GetState() : null;

                    if (state == null) continue;
                    movement?.InitializeEvents();
                   
                    AddState(state);
                    EnableState(state.Type);
                }
            }
        }
        private void OnDestroy()
        {
            if (instances != null && _collider != null) {
                EntityId id = _collider.GetEntityId();
              
                if (id.IsValid()) {
                    ulong _id = EntityId.ToULong(id);
                    if (instances.ContainsKey(_id)) instances?.Remove(_id);
                }
            }
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
            if (state == null || GetState(state.Type) != state) return false;
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

        protected override void OnEnabled() {
            if (_collider == null) _collider = GetComponent<Collider>();

            if (instances != null && _collider != null) {
                ulong _id = EntityId.ToULong(_collider.GetEntityId());
                if (!instances.ContainsKey(_id)) instances?.Add(_id, this);
            }

            base.OnEnabled();
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

        public void ChangeState(MovementStateType type)
        {
            ChangeState(type, true);
        }

        public void ChangeState(MovementStateType type, bool checkSimilarity)
        {
            MovementState state = GetState(type);
            if(state != null) ChangeState(state, checkSimilarity);
        }

        public bool ContainsState(MovementStateType type) {
            return GetState(type) != null;
        }

        public virtual void AddMovement(Movement movement) {
            if (movement == null || movements == null) return;
            else if (!movements.Contains(movement)){
                MovementState state = movement.GetState();
                if (state == null || GetMovement(state.Type) != null) return;

                AddState(state);
                movements?.Add(movement);
            }
        }

        public virtual void RemoveMovement(MovementStateType type) {
            RemoveMovement(GetMovement(type));
        }

        private void RemoveMovement(Movement movement)
        {
            if (movement == null || movements == null || !movements.Contains(movement)) return;
            else if (!movements.Contains(movement))
            {
                RemoveState(movement.GetState());
                movements?.Remove(movement);
            }
        }

        public void ReplaceMovement(Movement movement)
        {
            if(movement != null) {
                MovementState state = movement.GetState();
                if(state != null) RemoveMovement(state.Type);

                AddMovement(movement);
            }
        }

        public bool ContainsMovement(MovementStateType state)
        {
            return GetMovement(state) != null;
        }

        private Movement GetMovement(MovementStateType type)
        {
            if(movements != null) {
                foreach(Movement movement in movements) {
                    MovementState state = movement != null ? movement.GetState() : null;
                    if (state == null || state.Type != type) continue;
                    return movement;
                }
            }

            return null;
        }


        public MovementState GetState(MovementStateType type) {
            Movement movement = GetMovement(type);
            return movement != null ? movement.GetState() : null;
        }

        protected void SetIsMoving(bool isMoving) {
            this.isMoving = isMoving;
        }

        protected override void OnUpdate()
        {
            base.OnUpdate();
            string currentGroundTag = string.Empty;

            isGrounded = GetGroundCheckResult(out currentGroundTag);
            currentGroundTag = currentGroundTag.ToLower();


            if (!groundTag.ToLower().Equals(currentGroundTag)) onGroundTagChanged?.Invoke(currentGroundTag);

            if (isGrounded) { airbornTime = 0f; }
            else { airbornTime += Time.deltaTime; }

            airbornTime = Mathf.Clamp(airbornTime, 0f, float.MaxValue);
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
            nextPosition = transform.right * nextPosition.x +
                           transform.up * nextPosition.y +
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

        protected float GetDefaultHeight()
        {
            CrouchState state = CrouchState.GetState(this);
            return state != null ? state.StandHeight : 1f;
        }

        public static MovementStateMachine GetInstance(Collider collider)
        {
            if(collider == null) return null;
            return GetInstance(collider.GetEntityId());
        }


        public static MovementStateMachine GetInstance(Collider2D collider) {
            if (collider == null) return null;
            return GetInstance(collider.GetEntityId());
        }

        private static MovementStateMachine GetInstance(EntityId id)
        {
            if (instances == null || !id.IsValid()) return null;
            ulong _id = EntityId.ToULong(id);

          
            return !instances.ContainsKey(_id) ? null : instances[_id];
        }
    }
}
