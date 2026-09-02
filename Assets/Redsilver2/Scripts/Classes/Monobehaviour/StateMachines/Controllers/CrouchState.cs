using RedSilver2.Framework.StateMachines.Extensions;
using UnityEngine;

namespace RedSilver2.Framework.StateMachines.States
{
    [System.Serializable]
    public abstract class CrouchState : MovementState
    {
        [Space]
        [SerializeField] private float moveSpeed;
        [SerializeField] private float moveTransitionSpeed;

        [Space]
        [SerializeField] private float crouchHeight;
        [SerializeField] private float crouchHeightTransitionSpeed;

        [Space]
        [SerializeField] private float standHeight;
        [SerializeField] private float standHeightTransitionSpeed;


        [Space]
        [SerializeField] private float crouchSafetyCheckDistance;

        public float MoveSpeed => moveSpeed;
        public float MoveTransitionSpeed => moveTransitionSpeed;

        private bool isCrouching;
        private bool canChangeState;

        public bool IsCrouching => isCrouching;
        public bool CanChangeState => canChangeState;

        public float CrouchHeight => crouchHeight;
        public float StandHeight => standHeight;

        public float CrouchHeightTransitionSpeed => crouchHeightTransitionSpeed;
        public float StandHeightTransitionSpeed => standHeightTransitionSpeed;

        public float CrouchSafetyCheckDistance => crouchSafetyCheckDistance;

        private const string SOUND_EVENT = "Crouch Sound";
        public const MovementStateType TYPE = MovementStateType.Crouch;

        public CrouchState(MovementStateMachine stateMachine) : base(stateMachine) {
            isCrouching = false;
        }


#if UNITY_EDITOR
        public override void Validate() {
            moveSpeed = Mathf.Clamp(moveSpeed, 0f, float.MaxValue);
          
            standHeight = Mathf.Clamp(standHeight, 0f, float.MaxValue);
            crouchHeight = Mathf.Clamp(crouchHeight, 0f, standHeight);

            crouchHeightTransitionSpeed = Mathf.Clamp(crouchHeightTransitionSpeed, 0f, float.MaxValue);
            standHeightTransitionSpeed = Mathf.Clamp(standHeightTransitionSpeed, 0f, float.MaxValue);
           
            moveTransitionSpeed   = Mathf.Clamp(moveTransitionSpeed, 0f, float.MaxValue);
            crouchSafetyCheckDistance = Mathf.Clamp(crouchSafetyCheckDistance, 0f, float.MaxValue);

            if(!ContainsEvent(SOUND_EVENT)) AddEvent(new MovementWalkSound(SOUND_EVENT, this));
        }
#endif
        public void SetIsCrouching(bool isCrouching)
        {
            this.isCrouching = isCrouching;
        }

        public sealed override bool CanTransition() {
            return base.CanTransition() && isCrouching;
        }

        protected override void OnAdded()
        {
            base.OnAdded();
            GetMovementStateMachine(this)?.AddOnUpdateListener(Update);
        }

        protected override void OnRemoved()
        {
            base.OnRemoved();
            GetMovementStateMachine(this)?.RemoveOnUpdateListener(Update);
            isCrouching = false;
        }

        public void SetMoveSpeed(float moveSpeed) { this.moveSpeed = moveSpeed;  }
        public void SetMoveTransitionSpeed(float moveTransitionSpeed) { this.moveTransitionSpeed = moveTransitionSpeed; }

        public void SetStandHeight(float standHeight) { this.standHeight = standHeight; }
        public void SetCrouchHeight(float crouchHeight) { this.crouchHeight = crouchHeight; }

        public void SetCrouchHeightTransitionSpeed(float crouchHeightTransitionSpeed) { this.crouchHeightTransitionSpeed = crouchHeightTransitionSpeed; }
        public void SetStandHeightTransitionSpeed(float crouchHeightTransitionSpeed) { this.crouchHeightTransitionSpeed = crouchHeightTransitionSpeed; }
        public void SetCrouchSafetyCheckDistance(float crouchSafetyCheckDistance) { this.crouchSafetyCheckDistance = crouchSafetyCheckDistance; }

        protected virtual void Update()
        {
            GetMovementStateMachine(this)?.SetHeight(isCrouching ? crouchHeight : standHeight, isCrouching ? crouchHeightTransitionSpeed : standHeightTransitionSpeed);
        }


        protected sealed override void OnUpdate() {
            base.OnUpdate();
            GetMovementStateMachine(this)?.SetMoveSpeed(moveSpeed, moveTransitionSpeed);
        }

        protected sealed override void SetMovementStateType(ref MovementStateType type) {
            type = TYPE;
        }

        protected override void OnDisabled()
        {
            base.OnDisabled();
            isCrouching = false;
        }

        protected sealed override MovementStateType[] GetDefaultInvalidTypes()
        {
            return new MovementStateType[] { TYPE, LandState.TYPE, FallState.TYPE };
        }

        protected override void OnEntered()
        {
            base.OnEntered();
            isCrouching = true;
        }

        protected sealed override void UpdateStateTransitions()
        {
            MovementStateMachine movementStateMachine = GetMovementStateMachine(this);

            if(movementStateMachine != null) {
                Transform transform = movementStateMachine.transform;
                canChangeState = !Physics.Raycast(transform.position, transform.up, crouchSafetyCheckDistance, ~(1 << GetLayerToIgnore()));
               
                if (canChangeState) { base.UpdateStateTransitions(); }
                else { isCrouching = true; }
            }
        }

        private int GetLayerToIgnore() {
            return GameManager.PlayerLayer;
        }

        public static CrouchState GetState(MovementStateMachine stateMachine)
        {
            if(stateMachine == null) return null;
            return stateMachine?.GetState(TYPE) as CrouchState;
        }

        public static void ForceState(MovementStateMachine stateMachine) {
            CrouchState state = GetState(stateMachine);
            if (state != null) state.isCrouching = true; 
        }

        public static bool GetIsCrouching(MovementStateMachine stateMachine){
            CrouchState state = GetState(stateMachine);
            return state != null ? state.IsCrouching : false;   
        }
    }
}
