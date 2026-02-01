using RedSilver2.Framework.StateMachines.States;
using UnityEngine;

namespace RedSilver2.Framework.StateMachines.Controllers
{
    public abstract class StateMachineController : MonoBehaviour
    {
        [Space]
        [SerializeField] private float walkSpeed;
        [SerializeField] private float runSpeed;

        [Space]
        [SerializeField] private float crouchHeight;
        [SerializeField] private float standHeight;

        [Space]
        [SerializeField] private float jumpForce;

        [Space]
        [SerializeField] private float defaultGravitySpeed;
        [SerializeField] private float fallSpeed;

        [Space]
        [SerializeField] private float groundCheckRange;

        [Space]
        [SerializeField] private float crouchCheckDistance;

        [Space]
        [SerializeField] private bool use2DMovement;
        [SerializeField] private bool visualizeUnCrouchCheck;



        public bool  Use2DMovement      => use2DMovement;
        public float WalkSpeed => walkSpeed;    
        public float RunSpeed => runSpeed;
        public float CrouchHeight => crouchHeight;
        public float StandHeight => standHeight;
        public float JumpForce => jumpForce;
        public float DefaultGravitySpeed => defaultGravitySpeed;
        public float FallSpeed => fallSpeed;
        public float GroundCheckRange   => groundCheckRange; 
        public float CrouchCheckDistance  => crouchCheckDistance;


        private StateMachine stateMachine;
        public StateMachine StateMachine => stateMachine;

        #if UNITY_EDITOR
        protected virtual void OnValidate()
        {
            defaultGravitySpeed = Mathf.Clamp(defaultGravitySpeed, float.MinValue, 0f);
            fallSpeed           = Mathf.Clamp(fallSpeed, float.MinValue, 0f);
        }

        #endif


        protected virtual void Awake() {
            InitializeStateMachine(ref stateMachine);
        }

        private void Update() {
            stateMachine?.Update();
        }

        private void LateUpdate() {
            stateMachine?.LateUpdate();
        }

        private void OnDisable() {
            stateMachine?.Disable();
        }

        private void OnEnable() {
            stateMachine?.Enable();
        }

        protected virtual void AddState(State state) {
            if (state != null)
              stateMachine?.AddState(state.GetStateName(), state);
        }

        public void RemoveState(State state) {
            if(state != null)
              stateMachine?.RemoveState(state.GetStateName());
        }

        protected abstract void InitializeStateMachine(ref StateMachine stateMachine);
    }

}
