using RedSilver2.Framework.StateMachines.States.Movement;
using Unity.VisualScripting;
using UnityEngine;

namespace RedSilver2.Framework.StateMachines.Controllers
{
    [RequireComponent(typeof(Rigidbody))]
    public class PlayerRigidbodyController : PlayerController
    {
        private Rigidbody rigidbody;
        public Rigidbody Rigidbody => rigidbody;

        protected override void Awake() {
            rigidbody = gameObject.GetOrAddComponent<Rigidbody>();
            base.Awake();
        }

        protected sealed override PlayerMovementHandler GetPlayerMovementHandler() {
             return new RigidbodyMovementHandler(this);
        }
    }
}
