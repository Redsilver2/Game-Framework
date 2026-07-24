using RedSilver2.Framework.Interactions.Items;
using System.Collections.Generic;
using UnityEngine;

namespace RedSilver2.Framework.Inventories
{
    public abstract class ItemDisplayer : InventoryUIEvent
    {
        [SerializeField] private float verticalSpacing;
        [SerializeField] private float horizontalSpacing;

        [Space]
        [SerializeField] private float updateSpeed;

        private Dictionary<Item, Transform> itemTransforms;

        protected override void Awake()
        {
            base.Awake();
            itemTransforms = new Dictionary<Item, Transform>();
        }

        protected sealed override void SetEvents(InventoryUI inventoryUI, bool isAddingEvents) {
            if (isAddingEvents) inventoryUI?.AddOnUpdatedListener(OnUpdate);
            else inventoryUI?.RemoveOnUpdatedListener(OnUpdate);
        }

        private void OnUpdate() {
            Inventory inventory = GetInventory();
            OnUpdate(inventory != null ? inventory.GetItems() : null);
        }

        private void OnUpdate(Item[][] items){
            if(items == null || items.Length == 0) return;

            for(int i = 0; i < items.Length; i++) {

            }
        }

        private void OnUpdate(Item[] items) {

            for(int i = 0;i < items.Length; i++) {

            }
        }

        protected void ClearInvalidTransforms() {
            Inventory inventory = GetInventory();
            if (itemTransforms == null || inventory == null) return;

            Dictionary<Item, Transform> copy = itemTransforms;
            itemTransforms?.Clear();

            foreach (KeyValuePair<Item, Transform> pair in copy) {
                if(pair.Key == null ||  pair.Value == null || !inventory.ContainsItem(pair.Key))
                    continue;
               
                if (!itemTransforms.ContainsKey(pair.Key)) itemTransforms?.Add(pair.Key, pair.Value);
            }
        }

        protected void AddTransform(Item item) {
            ClearInvalidTransforms();
        }
    }
}
