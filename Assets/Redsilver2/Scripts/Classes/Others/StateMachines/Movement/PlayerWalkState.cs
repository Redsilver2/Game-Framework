using RedSilver2.Framework.StateMachines.Controllers;
using RedSilver2.Framework.StateMachines.States;
using UnityEngine;

namespace RedSilver2.Framework.StateMachines
{
    [System.Serializable]
    public class PlayerWalkState : WalkState
    {
        [Space]
        [SerializeField] private HeadbobPositionMotion positionMotion;
        public HeadbobPositionMotion PositionMotion => positionMotion;

        public PlayerWalkState() : base() {
            positionMotion = new HeadbobPositionMotion(TYPE, false);
        }


#if UNITY_EDITOR
        public virtual void Validate(PlayerMovementStateMachine stateMachine)
        {
            SetStateMachine(stateMachine);
            Validate();
        }

        protected override void Validate()
        {
            base.Validate();
            positionMotion?.Validate();
        }
#endif

        protected sealed override void OnEnabled()
        {
            positionMotion?.Enable();
            positionMotion?.SetStateMachine(this.MovementStateMachine as PlayerMovementStateMachine);
            base.OnEnabled();
        }

        protected sealed override void OnDisabled()
        {
            positionMotion?.Disable();
            positionMotion?.SetStateMachine(null);
            base.OnDisabled();
        }

        protected void SetStateMachine(PlayerMovementStateMachine stateMachine)
        {
            this.MovementStateMachine       = stateMachine;
            SetStateMachine(stateMachine as UpdatableStateMachine);
        }


    }
}
