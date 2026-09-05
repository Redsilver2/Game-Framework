using RedSilver2.Framework.Player;
using RedSilver2.Framework.StateMachines.Controllers;
using UnityEngine;


namespace RedSilver2.Framework.StateMachines.Events
{
    [System.Serializable]
    public sealed class PlayerTopDown3DMovementUpdater : PlayerTopDownMovementUpdater
    {
        [SerializeField] private TopDown3DMovementUpdateMode updateMode;

        public PlayerTopDown3DMovementUpdater(PlayerMovementStateMachine stateMachine) : base(stateMachine)
        {
            SetCameraController(new TopDown3DCameraController());
        }

        public void SetUpdateMode(TopDown3DMovementUpdateMode updateMode)
        {
            this.updateMode = updateMode;
        }

        protected sealed override Vector3 GetNextPosition(PlayerMovementStateMachine stateMachine)
        {
            if (stateMachine == null) return Vector3.zero;

            Vector3 result = base.GetNextPosition(stateMachine);
            Vector3 input  = stateMachine.MoveInput;

            if (updateMode == TopDown3DMovementUpdateMode.Directional) {
                if (Mathf.Abs(input.x) > 0f && Mathf.Abs(input.y) > 0f) return Vector3.zero;
                else if (Mathf.Abs(input.x) > 0f) return Vector3.right * result.x;
                else return Vector3.forward * result.y;
            }
            else if(updateMode == TopDown3DMovementUpdateMode.Tank) {
                // Add
            }

            return result;
        }

        protected sealed override bool Is2DMovement()
        {
            return false;
        }


        [System.Serializable]
        public enum TopDown3DMovementUpdateMode
        {
            Default,
            Directional,
            Tank
        }
    }
}