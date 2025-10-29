using RedSilver2.Framework.Interactions.Items;
using System;
using UnityEngine;

namespace RedSilver2.Framework.Player.Inventories.UI
{
    [RequireComponent(typeof(ComplexInventory))]
    public sealed class ComplexInventoryUINavigator : VerticalInventoryUINavigator
    {
        [Space]
        [SerializeField] private int maxHorizontalIndex;

        public sealed override void Select(Item item)
        {
            if (inventory is ComplexInventory && item != null) 
            {
                ComplexInventory inventory = (this.inventory as ComplexInventory);

                if (inventory.Contains(item))
                    inventory.GetItemIndexes(item, out verticalIndex, out horizontalIndex);
            }
        }

        public sealed override int GetMaxHorizontalIndex()
        {
            Item[,] items = GetItems();
            if (items == null || items.GetLength(0) == 0 || items.GetLength(1) == 0) return -1;
            return items.GetLength(1);
        }

        public sealed override int GetMaxVerticalIndex()
        {
            Item[,] items = GetItems();
            if (items == null || items.GetLength(0) == 0 || items.GetLength(1) == 0) return -1;
            return items.GetLength(0);
        }

        protected override void OnItemAdded(Item item)
        {
            Item[,] items = GetItems();
            string results = "";

            for (int i = 0; i < items.GetLength(0); i++)
            {
                results += $"Row {i + 1} | ";

                for (int j = 0; j < items.GetLength(1); j++) {
                    results += $"Item Name: " + (items[i, j] == null ? "Null" : items[i, j].name) + "| ";
                }

                results += "\n";
            }
                    
            Debug.LogWarning(results);
        }

        protected override void OnItemRemoved(Item item)
        {

        }

        public sealed override Item[,] GetItems()
        {
            if (inventory == null) return new Item[0, 0];
            if (inventory is SimpleInventory) return GetItems(inventory as SimpleInventory);
            return GetItems(inventory as ComplexInventory);
        }

        private Item[,] GetItems(SimpleInventory inventory)
        {
            if (inventory == null || maxHorizontalIndex == 0) return null;
            return GetItems(inventory, inventory.GetMaxHorizontalIndex() / maxHorizontalIndex);
        }

        private Item[,] GetItems(SimpleInventory inventory, int maxVerticalIndex)
        {
            Item[,] results;
            if (inventory == null || maxVerticalIndex <= 0) return new Item[0, 0];

            results = new Item[maxVerticalIndex % 2 == 0 ? maxVerticalIndex : maxVerticalIndex + 1, maxHorizontalIndex];

            GetItems(inventory, ref results);
            return results == null ? new Item[0, 0] : results;
        }

        private void GetItems(SimpleInventory inventory, ref Item[,] results)
        {
            if (inventory == null || results == null || results.GetLength(0) == 0 || results.GetLength(1) == 0 ) return;

            int verticalIndex = 0, horizontalIndex = 0;
            GetItems(inventory.GetItems(), ref verticalIndex, ref horizontalIndex, ref results);
        }


        private Item[,] GetItems(ComplexInventory inventory)
        {
            if (inventory == null || maxHorizontalIndex <= 0) return new Item[0, 0];
            return GetItems(inventory, inventory.GetItemsCount() / maxHorizontalIndex);
        }

        private Item[,] GetItems(ComplexInventory inventory, int maxVerticalIndex)
        {
            if (inventory == null || maxVerticalIndex <= 0) return new Item[0, 0];

            Item[,] results = new Item[maxVerticalIndex % 2 == 0 ? maxVerticalIndex : maxVerticalIndex + 1, maxHorizontalIndex];
            GetItems(inventory.GetItems(), ref results);
            return results;
        }

        public sealed override Item GetSelectedItem()
        {
            Item[,] items = GetItems();
            if (items == null || items.GetLength(0) == 0 || items.GetLength(1) == 0)  return null; 
            return items[verticalIndex, horizontalIndex];
        }

        private void GetItems(Item[][] items, ref Item[,] results)
        {
            int verticalIndex, horizontalIndex;
            if (items == null || results == null || results.GetLength(0) == 0 || results.GetLength(1) == 0) return;

            verticalIndex = 0;
            horizontalIndex = 0;

            for (int i = 0; i < items.Length; i++) 
                GetItems(items[i], ref verticalIndex, ref horizontalIndex, ref results);
        }

        private void GetItems(Item[] items, ref int verticalIndex, ref int horizontalIndex, ref Item[,] results) 
        {
            if (items == null || results == null) return;

            for (int i = 0; i < items.Length; i++) 
                GetItem(items[i], ref verticalIndex, ref horizontalIndex, ref results);
        }

        private void GetItem(Item item, ref int verticalIndex, ref int horizontalIndex, ref Item[,] results)
        {
            if(item == null || results == null) return;

            if (horizontalIndex >= results.GetLength(1)) {
                horizontalIndex = 0;
                verticalIndex++;
            }

            results[base.verticalIndex, horizontalIndex] = item;
            horizontalIndex++;
        }
    }
}
