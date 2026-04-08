using UnityEngine;

namespace RedSilver2.Framework.StateMachines.Controllers
{
    public class MovementStateMachineController : UpdateableStateMachineController
    {
        [SerializeField] private float groundCheckRange = 0f;
        [SerializeField] private bool  is2DMovement;

        public float GroundCheckRange => groundCheckRange;
        public bool Is2DMovement => is2DMovement;

        public override void SetStateMachine(StateMachine stateMachine)
        {
            if(stateMachine is MovementStateMachine)
                 base.SetStateMachine(stateMachine);
        }
    }
}
