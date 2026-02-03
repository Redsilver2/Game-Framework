using RedSilver2.Framework.StateMachines.States.Movement;
using UnityEngine;


namespace RedSilver2.Framework.StateMachines.Controllers
{
    public abstract class MovementStateMachineController : StateMachineController {
        [Space]
        [SerializeField] private float walkSpeed;
        [SerializeField] private float runSpeed;

        [Space]
        [SerializeField] private float standHeight;

        [Space]
        [SerializeField] private float jumpForce;

        [Space]
        [SerializeField] private float defaultGravitySpeed;
        [SerializeField] private float fallSpeed;

        [Space]
        [SerializeField] private float groundCheckRange;

        [Space]
        [SerializeField] private bool use2DMovement;

        public bool Use2DMovement          => use2DMovement;
        public float WalkSpeed             => walkSpeed;
        public float StandHeight           => standHeight;
        public float DefaultGravitySpeed   => defaultGravitySpeed;
        public float GroundCheckRange      => groundCheckRange;


        protected sealed override void InitializeStateMachine(ref StateMachine stateMachine)
        {
            stateMachine = GetStateMachine(GetMovementHandler());
            stateMachine?.Enable();
        }

        protected abstract MovementStateMachine GetStateMachine(MovementHandler movementHandler);
        protected abstract MovementHandler GetMovementHandler();
    }
}
