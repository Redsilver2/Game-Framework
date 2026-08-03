using RedSilver2.Framework.StateMachines.Controllers;
using UnityEngine;

namespace RedSilver2.Framework.Items
{
    public class EquippableItemMovementSwayUpdater : PlayerMovementSwayMotion
    {

        private PlayerMovementSwayMotion swayMotion;
        private EquippableItem item;

        protected override void Awake() {
            base.Awake();

            swayMotion = GetComponent<PlayerMovementSwayMotion>();
            item       = transform.root != null ? transform.root.GetComponent<EquippableItem>() : 
                                                                 GetComponent<EquippableItem>();
        }

        protected override void Start()
        {
            base.Start(); 


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
                SetStateMachine(transform.root != null ? transform.root.GetComponent<PlayerMovementStateMachine>() : null);
            }
        }

        private void OnRemoved() {
            SetStateMachine(null);
        }
    }
}
