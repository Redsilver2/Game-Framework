using RedSilver2.Framework.Interactions.Items;
using UnityEngine;

namespace RedSilver2.Framework.Player.Inventories.UI
{
    public sealed class HorizontalInventoryUINavigator : SimpleInventoryUINavigator
    {
        [Space]
        [SerializeField] private int verticalIndex;
        public int VerticalIndex => verticalIndex;

        public sealed override void Select(Item item)
        {
            if (inventory is SimpleInventory) base.Select(item);
            else                              SetIndex(inventory as ComplexInventory, item);
        }


        protected override void OnItemAdded(Item item)
        {
            if (inventory is SimpleInventory) base.OnItemAdded(item);
            else                              OnItemAdded(inventory as ComplexInventory, item);
        }

        protected override void OnItemRemoved(Item item)
        {
            if (inventory is SimpleInventory) base.OnItemAdded(item);
            else OnItemRemoved(inventory as ComplexInventory, item);
        }

        private void OnItemAdded(ComplexInventory inventory, Item item)
        {
            if (inventory == null || item == null)
                return;

            if(inventory.Contains(verticalIndex, item)) {
                base.OnItemAdded(item);
            }
        }

        private void OnItemRemoved(ComplexInventory inventory, Item item)
        {
            if (inventory == null || item == null)
                return;

            if (inventory.Contains(verticalIndex, item))
            {
                base.OnItemRemoved(item);
            }
        }


        private void SetIndex(ComplexInventory inventory, Item item)
        {
            if(inventory == null || item == null) return;
            if (inventory.Contains(verticalIndex, item)) horizontalIndex = inventory.GetHorizontalIndex(item);
        }

        public sealed override int GetMaxHorizontalIndex()
        {
            if (inventory == null) return -1;

            if     (inventory is SimpleInventory) return base.GetMaxHorizontalIndex();   
            return (inventory as ComplexInventory).GetMaxHorizontalIndex(verticalIndex);
        }

        public sealed override Item GetSelectedItem()
        {
            if (inventory == null) return null;

            if (inventory is SimpleInventory) return base.GetSelectedItem();
            return (inventory as ComplexInventory).GetItem(verticalIndex, horizontalIndex);
        }
    }
}
