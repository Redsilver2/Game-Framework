using RedSilver2.Framework.StateMachines.Controllers;
using RedSilver2.Framework.StateMachines.Events;
using UnityEngine;

namespace RedSilver2.Framework.Items
{
    public abstract class EquippableItemMovementUpdater : MonoBehaviour
    {
        private MovementMotion movementMotion;
        private EquippableItem item;

        protected virtual void Awake() {
            movementMotion = GetComponent<MovementMotion>();
            item           = transform.root != null ? transform.root.GetComponentInChildren<EquippableItem>() :
                                                                     GetComponentInChildren<EquippableItem>();
        }

        private void Start()
        {
            item?.AddOnAddedListener(OnAdded);
            item?.AddOnRemovedListener(OnRemoved);
        }

        private void OnDestroy()
        {
            item?.RemoveOnAddedListener(OnAdded);
            item?.RemoveOnRemovedListener(OnRemoved);
        }

        private void OnAdded()
        {
            if(item != null) {
                Transform transform = item.transform;
                movementMotion?.SetStateMachine(transform.root != null ? transform.root.GetComponent<PlayerMovementStateMachine>() : null);
            }
        }

        private void OnRemoved() {
            movementMotion?.SetStateMachine(null);
        }
    }
}
