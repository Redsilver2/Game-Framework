using RedSilver2.Framework.Inputs;
using UnityEngine;
using UnityEngine.InputSystem;

namespace RedSilver2.Framework.Interactions
{
    public class MouseInteractionHandler : FPSInteractionHandler
    {
        protected override Collider GetCollider(float interactionRange, Camera camera)
        {
            if (camera == null || Mouse.current == null) return null;
            Physics.Raycast(camera.ScreenPointToRay(Mouse.current.position.value), out RaycastHit hitInfo, interactionRange, ~GameManager.PlayerLayer);
            return hitInfo.collider;
        }
    }
}
