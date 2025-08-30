using System.Collections.Generic;
using UnityEngine;

namespace RedSilver2.Framework.Player
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerController : MonoBehaviour
    {
        [Header("Jump Settings")]
        [SerializeField] private float jumpForce = 0f;
        [SerializeField] private float jumpDelay = 0f;

        [Space]
        [Header("Gravity Settings")]
        [SerializeField] private float groundRangeCheck;
        [SerializeField] private float gravityTransitionSpeed;

        [Space]
        [Header("Misc Settings")]
        [SerializeField] private bool canUpdateStates = true;
        [SerializeField] private bool canUpdateCamera = true;

        private PlayerStateMachine stateMachine;
        private CameraController   cameraController;

        public float JumpForce => jumpForce;
        public float JumpDelay => jumpDelay;

        public float GravityTransitionSpeed => gravityTransitionSpeed;
        public float GroundCheckRange       => groundRangeCheck;


        public PlayerStateMachine StateMachine => stateMachine;

        public const string PLAYER_LAYER_NAME = "Player";

        #if UNITY_EDITOR
        private void OnValidate()
        {
            gravityTransitionSpeed = Mathf.Clamp(gravityTransitionSpeed, 0f, float.MaxValue);
            groundRangeCheck       = Mathf.Clamp(groundRangeCheck, 0f, float.MaxValue);

            jumpDelay = Mathf.Clamp(jumpDelay, 0f, float.MaxValue);
            jumpForce = Mathf.Clamp(jumpForce, 0f, float.MaxValue);
        }
        #endif

        private void Awake()
        {
            stateMachine = GetStateMachine();
            cameraController = new ClampedPlayerCameraController(transform, Camera.main.transform.parent, -45f, 45f);
        }

        private void Start()
        {

        }

        private void Update()
        {
            if (stateMachine != null && canUpdateStates) stateMachine.Update();
            if (cameraController != null && canUpdateCamera) cameraController.Update();
        }

        private void LateUpdate()
        {
            if (stateMachine != null && canUpdateStates) stateMachine.LateUpdate();
            if (cameraController != null && canUpdateCamera) cameraController.LateUpdate();
        }

        public void SetCanUpdateStates(bool canUpdateStates)
        {
            this.canUpdateStates = canUpdateStates;
        }

        public void SetCanUpdateCamera(bool canUpdateCamera)
        {

        }

        public string GetTransition(PlayerStateMachine.PlayerStateTransitionCondition transitionCondition)
        {
            if (transitionCondition == null) return string.Empty;
            return transitionCondition.ToString() + "\n";
        }


        public string GetTransitions(PlayerStateMachine.PlayerState state)
        {
            if (state == null) return string.Empty;
            return GetTransitions(state.GetTransitionConditions());
        }

        public string GetTransitions(PlayerStateMachine.PlayerStateTransitionCondition[] conditions)
        {
            string result = string.Empty;

            foreach (var condition in conditions)
            {
                result += GetTransition(condition);
            }

            return result;
        }

        protected virtual PlayerStateMachine GetStateMachine()
        {
            PlayerStateMachine stateMachine = new PlayerStateMachine(this);

            stateMachine.AddStates(new PlayerStateMachine.PlayerState[]
            {
                new PlayerStateMachine.FallState(stateMachine, 5f),
                new PlayerStateMachine.WalkState(stateMachine),
                new PlayerStateMachine.IdolState(stateMachine),
            });

            stateMachine.AddOnStateEnterListener(state => { if (state != null) Debug.Log($"Entering {state.GetStateName()} State | {GetTransitions(state.GetTransitionConditions())}"); });
            stateMachine.AddOnStateExitListener(state => { if (state != null) Debug.Log($"Exiting {state.GetStateName()} State | {GetTransitions(state.GetTransitionConditions())}"); });

            Transform t = Camera.main.transform.parent;
            stateMachine.AddExtension(new PlayerStateMachine.HeadBobExtension(stateMachine, t, new Vector2(-0.36f, 0.403f), new Vector2(0.36f, 0.603f)));

            stateMachine.ChangeState(PlayerStateMachine.IdolState.STATE_NAME);
            return stateMachine;
        }
    }
}
