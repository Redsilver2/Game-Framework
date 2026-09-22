using RedSilver2.Framework.Player;
using RedSilver2.Framework.StateMachines.Controllers;
using UnityEngine;

namespace RedSilver2.Framework.StateMachines.Events
{
    public class MouseMovementTiltMotion : MovementTiltMotion
    {
        public MouseMovementTiltMotion(string name, PlayerMovementStateMachine stateMachine) : base(name, stateMachine)
        {
        }

        protected override void Disable(PlayerMovementStateMachine stateMachine)
        {

        }

        protected override void Enable(PlayerMovementStateMachine stateMachine)
        {

        }

        protected sealed override void UpdateRotation(Vector2 input, ref Vector3 desired)
        {
            base.UpdateRotation(input, ref desired);
            float x = desired.y, y = desired.x;

            desired.x = y;
            desired.y = Original.y;
            desired.z = x;
        }
    }
}
