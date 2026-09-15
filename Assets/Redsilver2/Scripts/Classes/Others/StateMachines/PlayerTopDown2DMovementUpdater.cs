using RedSilver2.Framework.Player;
using RedSilver2.Framework.StateMachines.Controllers;
using UnityEngine;

namespace RedSilver2.Framework.StateMachines.Events
{
    [System.Serializable]
    public sealed class PlayerTopDown2DMovementUpdater : PlayerTopDownMovementUpdater
    {
        [SerializeField] private TopDown2DMovementUpdateMode updateMode;


        public PlayerTopDown2DMovementUpdater(PlayerMovementStateMachine stateMachine) : base(stateMachine) {
            SetCameraController(new TopDown2DCameraController());
        }

        public void SetUpdateMode(TopDown2DMovementUpdateMode updateMode)
        {
            this.updateMode = updateMode;
        }

        protected sealed override Vector3 GetNextPosition(PlayerMovementStateMachine stateMachine)
        {
            if (stateMachine == null) return Vector3.zero; 
          
            Vector3 result = base.GetNextPosition(stateMachine);
            Vector3 input  = stateMachine.MoveInput; 

            if (updateMode == TopDown2DMovementUpdateMode.Directional) {
                if (Mathf.Abs(input.x) > 0f && Mathf.Abs(input.y) > 0f) return Vector3.zero;
                else if (Mathf.Abs(input.x) > 0f) return Vector3.right * result.x;
                else return Vector3.up * result.y;
            }

            return result;
        }


        protected sealed override bool Is2DMovement()
        {
            return true;
        }


        [System.Serializable]
        public enum TopDown2DMovementUpdateMode {
            Default,
            Directional
        }
    }
}