using RedSilver2.Framework.StateMachines.Handlers;
using UnityEngine;

namespace RedSilver2.Framework.StateMachines.Controllers
{
    [RequireComponent(typeof(MovementStateMachineEventHandler))]
    public abstract class MovementStateMachineController : UpdateableStateMachineController
    {
        [SerializeField] private float groundCheckRange = 0f;
        [SerializeField] private bool  is2DMovement;

        public float GroundCheckRange                    => groundCheckRange;
        public bool Is2DMovement                         => is2DMovement;
        public MovementStateMachine MovementStateMachine => StateMachine as MovementStateMachine;
    }
}
