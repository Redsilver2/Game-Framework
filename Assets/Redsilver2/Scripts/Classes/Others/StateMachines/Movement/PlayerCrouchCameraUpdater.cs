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

        [SerializeField][HideInInspector] private PlayerMovementStateMachine stateMachine;

        public PlayerCrouchCameraUpdater() { }

#if UNITY_EDITOR
        public void Validate(PlayerMovementStateMachine stateMachine) {
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
            bool isCrouching = CrouchState.GetIsCrouching(stateMachine);
            if(transform != null) transform.localPosition = Vector3.Lerp(transform.localPosition, isCrouching ?  crouchPosition : standPosition, Time.deltaTime * GetCrouchSpeed(isCrouching)); 
        }

        private float GetCrouchSpeed(bool isCrouching)
        {
            CrouchState state = CrouchState.GetState(stateMachine);
            if (state == null) return 1f;

            return state.IsCrouching ? state.CrouchHeightTransitionSpeed : state.StandHeightTransitionSpeed;
        }
        
    }
}
