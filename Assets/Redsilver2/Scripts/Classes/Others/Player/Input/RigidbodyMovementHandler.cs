using RedSilver2.Framework.StateMachines.Controllers;
using UnityEngine;

namespace RedSilver2.Framework.StateMachines.States.Movement
{
    public sealed class RigidbodyMovementHandler : PlayerMovementHandler
    {
        public readonly PlayerRigidbodyController Controller;

        public RigidbodyMovementHandler(PlayerRigidbodyController controller) : base(controller)
        {
            Controller = controller;
        }

        public RigidbodyMovementHandler(PlayerRigidbodyController controller, bool use2DMovement) : base(controller, use2DMovement)
        {
            Controller = controller;
        }

        public sealed override void Move(Vector3 position)
        {
            if(Controller != null) {
               Controller.Rigidbody?.AddForce(Time.deltaTime * position, ForceMode.Force);
            }
        }
        public sealed override void Crouch(float height, float speed) {
            if (Controller != null) {
                Transform transform = Controller.transform;
                Vector3 currentSize = transform.localScale;

                transform.localScale = Vector3.Lerp(currentSize, Vector3.right * currentSize.x +
                                                                 Vector3.up * height +
                                                                 Vector3.forward * currentSize.z,
                                                                 Time.deltaTime * speed);
            }
        }

        public sealed override Transform GetTransform() {
            if (Controller == null) return null;
            return Controller.transform;
        }
    }
}
