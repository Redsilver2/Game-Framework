using RedSilver2.Framework.StateMachines.States.Movement;
using UnityEngine;
using UnityEngine.Events;

namespace RedSilver2.Framework.StateMachines.States
{
    [RequireComponent(typeof(IdolStateInitializer))]
    public sealed class WalkStateInitializer : MovementStateInitializer
    {
        [SerializeField] private float walkSpeed;
        [SerializeField] private float walkTransitionSpeed;
        public float WalkSpeed => walkSpeed;
        public float WalkTransitionSpeed => walkTransitionSpeed;

        protected sealed override void Start()
        {
            base.Start();

            if(stateMachine is MovementStateMachine)
            {
                MovementHandler handler = (stateMachine as MovementStateMachine).MovementHandler;
                if(handler != null)  handler.SetMoveSpeed(walkSpeed);
            }
        }

        protected sealed override MovementState GetDefaultState(MovementStateMachine stateMachine) {
            if(stateMachine == null)  return null;
            
            if (stateMachine.ContainsState(MovementStateType.Walk)) 
                return stateMachine.GetState(MovementStateType.Walk) as WalkState;

            return new WalkState(stateMachine, this);
        }

        protected override string GetModuleName()
        {
            return "Walk State Initializer";
        }
    }
}
