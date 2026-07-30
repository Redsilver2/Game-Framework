using RedSilver2.Framework.StateMachines.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RedSilver2.Framework.StateMachines.States
{
    [RequireComponent(typeof(LandState))]
    public sealed class FallState : MovementState
    {
        [Space]
        [SerializeField] private float walkSpeed;
        [SerializeField] private float moveTransitionSpeed;

        [Space]
        [SerializeField] private float fallSpeed;
        [SerializeField] private float fallTransitionSpeed;


        public const MovementStateType TYPE = MovementStateType.Fall;

        protected sealed override bool CanTransition(MovementStateMachine stateMachine)
        {
            return stateMachine != null ? !stateMachine.IsGrounded && !stateMachine.IsCurrentState(TYPE) : false;
        }

        protected sealed override void OnUpdate(MovementStateMachine stateMachine)
        {
            if (stateMachine == null) return;
            stateMachine?.SetMoveSpeed(walkSpeed, moveTransitionSpeed);
            stateMachine?.SetFallSpeed(fallSpeed, fallTransitionSpeed);
        }

        protected sealed override void SetMovementStateType(ref MovementStateType type) {
            type = MovementStateType.Fall;
        }

        public void SetWalkSpeed(float walkSpeed)
        {
            this.walkSpeed = walkSpeed;
        }

        public void SetMoveTransitionSpeed(float moveTransitionSpeed)
        {
            this.moveTransitionSpeed = moveTransitionSpeed;
        }

        public void SetFallSpeed(float fallSpeed){
            this.fallSpeed = fallSpeed;
        }

        public void SetFallTransitionSpeed(float falltransitionSpeed)
        {
            this.fallTransitionSpeed = falltransitionSpeed;
        }

        public static FallState GetState(MovementStateMachine stateMachine)
        {
            if(stateMachine == null) return null;
            return stateMachine.GetState(TYPE) as FallState;    
        }



#if UNITY_EDITOR
        protected sealed override MovementStateType[] GetDefaultInvalidTypes()
        {
            var results = Enum.GetValues(typeof(MovementStateType)) as MovementStateType[];
            if(results == null) return new MovementStateType[0];  

            return results.Where(x => x != MovementStateType.Land).ToArray();
        }
#endif
    }
}
