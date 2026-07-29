using RedSilver2.Framework.StateMachines.Controllers;
using UnityEngine;

namespace RedSilver2.Framework.StateMachines
{
    public sealed class PlayerTransformStateMachine : PlayerMovementStateMachine
    {
        protected sealed override void OnMoved(Vector3 nextPosition) {
            transform.localPosition += nextPosition;
        }
    }
}
