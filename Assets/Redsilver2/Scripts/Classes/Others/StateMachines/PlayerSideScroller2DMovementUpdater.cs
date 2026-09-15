using RedSilver2.Framework.StateMachines.Controllers;
using UnityEngine;

namespace RedSilver2.Framework.StateMachines.Events {
    [System.Serializable]
    public sealed class PlayerSideScroller2DMovementUpdater : PlayerSideScrollerMovementUpdater
    {
        public PlayerSideScroller2DMovementUpdater(PlayerMovementStateMachine stateMachine) : base(stateMachine)
        {
            // Set New Camera Here
        }
    }
}
