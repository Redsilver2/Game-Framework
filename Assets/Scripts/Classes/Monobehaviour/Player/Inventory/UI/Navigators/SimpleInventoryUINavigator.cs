using RedSilver2.Framework.Interactions.Items;
using System.Collections.Generic;
using UnityEngine;

namespace RedSilver2.Framework.Player.Inventories.UI
{
    public class SimpleInventoryUINavigator : InventoryUINavigator
    {
        private List<Item>   items;
        private GameObject[] models;

        public Item[] Items
        {
            get
            {
                if (items == null) return new Item[0];
                return items.ToArray();
            }
        }

        public GameObject[] Models => models;

        protected override void Awake()
        {
            base.Awake();
            items = new List<Item>();
        }

        public override void Select(Item item)
        {
            if (inventory == null || item == null) return;
            if (inventory.Contains(item)) horizontalIndex = inventory.GetHorizontalIndex(item);
        }

        public override int GetMaxHorizontalIndex()
        {
            if(inventory != null) return inventory.GetMaxHorizontalIndex();
            return -1;
        }

        protected override void OnCloseInventoryUI()
        {
            ItemModel.ReturnBorrowedModels(models);
        }

        protected override void OnItemAdded(Item item)
        {
            if (items != null && item != null){
                items.Add(item);
                UpdateModels();
            }
        }

        protected override void OnItemRemoved(Item item) 
        {
            if (items != null && item != null) {
                items.Remove(item);
                UpdateModels();
            }
        }

        protected override void UpdateItems() {

        }

        protected override void UpdateModels()
        {
            ItemModel.ReturnBorrowedModels(models);
            models = ItemModel.GetModels(items.ToArray());
        }

        public Item[] GetItems()
        {
            if (items == null) return new Item[0];
            return items.ToArray();
        }

        public override Item GetSelectedItem()
        {
            if(items == null || horizontalIndex <= 0 || horizontalIndex >= items.Count) return null;
            return items[horizontalIndex];
        }
    }
}
