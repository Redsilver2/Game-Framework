
using RedSilver2.Framework.StateMachines.States;
using System.Collections.Generic;
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

        private float moveSpeed;
        private float fallSpeed;

        private bool isGrounded;
        private bool isMoving;

        private string groundTag;
        private float airbornTime;

        private UnityEvent<Vector3> onMoved;
        private UnityEvent<string> onGroundTagChanged;

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
            groundCheckRange = Mathf.Clamp(groundCheckRange, 0f, float.MaxValue);
        }
#endif


        protected override void Awake()
        {
            base.Awake();

            onMoved            = new UnityEvent<Vector3>();
            onGroundTagChanged = new UnityEvent<string>();

            groundTag = string.Empty;
            isGrounded = false;

            isMoving  = false;
            moveSpeed = 0f;

            AddOnMovedListener(OnMoved);
            AddOnGroundTagChangedListener(OnGroundTagChanged);
        }

        protected virtual void Start()
        {

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

        public void SetGroundCheckRange(float groundCheckRange) { this.groundCheckRange = groundCheckRange; }
        public void SetIs2DMovement(bool is2DMovement) { this.is2DMovement = is2DMovement;  }

        public void ResetAirbornTime()
        {
            airbornTime = 0f;
        }

        protected sealed override bool CanAddState(UpdatableState state)
        {
            return CanAddState(state as MovementState);
        }

        protected virtual bool CanAddState(MovementState state)
        {
            return state != null ? true : false;
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

        public void AddState(MovementState state)
        {
            AddState(state as State);
        }

        public void RemoveState(MovementStateType type)
        {
            RemoveState(GetState(type));
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

        public MovementState GetState(MovementStateType type) {
            State[] states = States;

            if(states != null) {
                foreach(State state in states) {
                    MovementState movementState = state as MovementState;
                    if (movementState == null || movementState.Type != type) continue;
                    return movementState;
                }
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
