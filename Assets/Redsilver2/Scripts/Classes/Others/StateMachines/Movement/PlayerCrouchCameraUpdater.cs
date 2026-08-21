using RedSilver2.Framework.StateMachines.Controllers;
using RedSilver2.Framework.StateMachines.States;
using UnityEngine;

namespace RedSilver2.Framework.StateMachines.Extensions
{
    [System.Serializable]
    public sealed class PlayerCrouchCameraUpdater
    {
        [Space]
        [SerializeField] private Transform transform;

        [Space]
        [SerializeField] private Vector3 crouchPosition;
        [SerializeField] private Vector3 standPosition;

        private PlayerCrouchState state;
        private PlayerMovementStateMachine stateMachine;

        public PlayerCrouchCameraUpdater() { }

#if UNITY_EDITOR
        public void Validate(PlayerCrouchState state, PlayerMovementStateMachine stateMachine) {
           this.state = state;
           this.stateMachine = stateMachine;
        }
#endif
        public void SetCrouchPosition(Vector3 crouchPosition) {
            this.crouchPosition = crouchPosition;
        }

        public void SetStandPosition(Vector3 standPosition)
        {
            this.standPosition = standPosition;
        }


        public void LateUpdate() {
            bool isCrouching = state != null ? state.IsCrouching : false;
            if(transform != null) transform.localPosition = Vector3.Lerp(transform.localPosition, isCrouching ?  crouchPosition : standPosition, Time.deltaTime * GetCrouchSpeed(isCrouching)); 
        }

        private float GetCrouchSpeed(bool isCrouching)
        {
            if (state != null && isCrouching) return state.CrouchHeightTransitionSpeed;
            else if (stateMachine != null && !isCrouching) return stateMachine.HeightTransitionSpeed;
            else return 0f;
        }
        
    }
}
