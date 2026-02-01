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


        protected sealed override PlayerMovementHandler GetMovementHandler() {
             return new RigidbodyMovementHandler(this);
        }

        protected override PlayerStateMachine GetPlayerStateMachine(PlayerMovementHandler movementHandler)  {
            return new PlayerStateMachine(this, movementHandler);
        }
    }
}
