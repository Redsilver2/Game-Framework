using RedSilver2.Framework.Inventories;
using RedSilver2.Framework.StateMachines;
using UnityEngine.Events;
using UnityEngine;

namespace RedSilver2.Framework.Items
{
    public sealed class EquippableItem : Item {
        private bool isEquipped;

        private EquippableItemStateMachine stateMachine;
        private UnityEvent onEquipped, onUnEquipped;

        public bool IsEquipped => isEquipped;
        public EquippableItemStateMachine StateMachine => stateMachine;

        protected override void Awake()
        {
            base.Awake();
            isEquipped              = false;
           
            onEquipped              = new UnityEvent();  
            onUnEquipped            = new UnityEvent();

            stateMachine = GetComponent<EquippableItemStateMachine>();

            AddOnEquippedListener(OnEquipped);  
            AddOnUnEquippedListener(OnUnEquipped);
        }

        public void Equip()
        {
            if (!isEquipped && !IsDropping) onEquipped?.Invoke();
        }

        public void UnEquip() {
            if (isEquipped && !IsDropping) onUnEquipped?.Invoke();
        }


        protected override void RemoveFromInventory(Inventory inventory)
        {
            isEquipped = false;
            base.RemoveFromInventory(inventory);
        }

        private void OnEquipped() {
            isEquipped = true;
        }

        private void OnUnEquipped() {
            isEquipped = false;
        }


        public void AddOnEquippedListener(UnityAction action)
        {
            if(action != null) onEquipped?.AddListener(action);
        }

        public void RemoveOnEquippedListener(UnityAction action)
        {
            if(action != null)  onEquipped?.RemoveListener(action);
        }


        public void AddOnUnEquippedListener(UnityAction action)
        {
            if (action != null) onUnEquipped?.AddListener(action);
        }

        public void RemoveOnUnEquippedListener(UnityAction action)
        {
            if (action != null)  onUnEquipped?.RemoveListener(action); 
        }

        protected sealed override ItemType GetItemType()
        {
            var stateMachine = EquippableItemStateMachine.GetStateMachine(this);
            return stateMachine != null ? stateMachine.Type : ItemType.None;
        }
    }
}
