using RedSilver2.Framework.StateMachines.Controllers;
using UnityEngine;


namespace RedSilver2.Framework.StateMachines.States
{
    [System.Serializable]
    public sealed class PlayerIdolState : IdolState
    {

        [Space]
        [SerializeField] private HeadbobPositionMotion positionMotion;
        public HeadbobPositionMotion PositionMotion => positionMotion;

        public PlayerIdolState() {
            positionMotion = new HeadbobPositionMotion(TYPE, true);
        }


#if UNITY_EDITOR
        public void Validate(PlayerMovementStateMachine stateMachine)
        {
            SetStateMachine(stateMachine);
            Validate();
        }

        protected override void Validate() {
            base.Validate();
            positionMotion?.Validate();
        }
#endif

        private void SetStateMachine(PlayerMovementStateMachine stateMachine)
        {
            this.MovementStateMachine = stateMachine;
            SetStateMachine(stateMachine as UpdatableStateMachine);
        }

        protected override void OnEnabled()
        {
            positionMotion?.Enable();
            positionMotion?.SetStateMachine(MovementStateMachine as PlayerMovementStateMachine);

            base.OnEnabled();
        }

        protected override void OnDisabled()
        {
            positionMotion?.Disable();
            positionMotion?.SetStateMachine(null);
            base.OnDisabled();
        }

    }
}
