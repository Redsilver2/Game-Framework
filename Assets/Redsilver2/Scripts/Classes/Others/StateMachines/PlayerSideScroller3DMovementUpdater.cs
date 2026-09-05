using RedSilver2.Framework.Player;
using RedSilver2.Framework.StateMachines.Controllers;
using UnityEngine;



namespace RedSilver2.Framework.StateMachines.Events
{
    [System.Serializable]
    public class PlayerSideScroller3DMovementUpdater : PlayerSideScrollerMovementUpdater
    {
        public PlayerSideScroller3DMovementUpdater(PlayerMovementStateMachine stateMachine) : base(stateMachine) {
            SetCameraController(new TargetFollow3DCameraController());
        }
    }
}
