using RedSilver2.Framework.StateMachines.Controllers;
using UnityEngine;

namespace RedSilver2.Framework.StateMachines.States {
    public sealed class PlayerLandState : LandState
    {
        public PlayerLandState()
        {
        }



#if UNITY_EDITOR
        public void Validate(PlayerMovementStateMachine stateMachine)
        {
            SetStateMachine(stateMachine);
            Validate();
        }

        protected override void Validate()
        {
            base.Validate();

        }
#endif

        private void SetStateMachine(PlayerMovementStateMachine stateMachine)
        {
            this.MovementStateMachine = stateMachine;
            SetStateMachine(stateMachine as UpdatableStateMachine);
        }

        protected override void OnEnabled()
        {
            base.OnEnabled();
        }

        protected override void OnDisabled()
        {
            base.OnDisabled();
        }

    }
}
