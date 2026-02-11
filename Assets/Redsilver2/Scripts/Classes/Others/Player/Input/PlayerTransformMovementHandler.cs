using RedSilver2.Framework.StateMachines.Controllers;
using UnityEngine;

namespace RedSilver2.Framework.StateMachines.States.Movement
{
    public sealed class PlayerTransformMovementHandler : PlayerMovementHandler
    {
        public readonly PlayerTransformController Controller;

        public PlayerTransformMovementHandler(PlayerTransformController controller) : base(controller) {
            this.Controller = controller;
        }

        public PlayerTransformMovementHandler(PlayerTransformController controller, bool use2DMovementInputs) : base(controller, use2DMovementInputs) {
            this.Controller = controller;
        }

        public sealed override void Move(Vector3 position) {
            if(Controller != null)  Controller.transform.Translate(Time.deltaTime * position);
        }

        public sealed override void Crouch(float height, float speed)
        {
            if(Controller != null) {
                Transform transform = Controller.transform;
                Vector3 currentSize = transform.localScale;

                transform.localScale = Vector3.Lerp(currentSize, Vector3.right   * currentSize.x +
                                                                 Vector3.up      * height +
                                                                 Vector3.forward * currentSize.z,
                                                                 Time.deltaTime  * speed);
            }
        }

        public sealed override Transform GetTransform() {
            if (Controller == null) return null;
            return Controller.transform;
        }
    }
}
