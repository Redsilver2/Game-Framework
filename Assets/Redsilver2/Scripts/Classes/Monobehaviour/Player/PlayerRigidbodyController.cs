using Unity.VisualScripting;
using UnityEngine;

namespace RedSilver2.Framework.StateMachines.Controllers
{
    [RequireComponent(typeof(PlayerRigidbodyStateMachine))]
    public class PlayerRigidbodyController : PlayerMovementController
    {
        protected sealed override void Awake() {
            gameObject.GetOrAddComponent<PlayerRigidbodyStateMachine>();
            base.Awake();

        }
    }
}
