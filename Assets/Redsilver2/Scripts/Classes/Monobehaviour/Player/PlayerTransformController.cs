using Unity.VisualScripting;
using UnityEngine;

namespace RedSilver2.Framework.StateMachines.Controllers {

    [RequireComponent(typeof(PlayerTransformStateMachine))]
    public sealed class PlayerTransformController : PlayerMovementController {
        protected sealed override void Awake() {
            gameObject.GetOrAddComponent<PlayerTransformStateMachine>();
            base.Awake();
        }
    }
}
