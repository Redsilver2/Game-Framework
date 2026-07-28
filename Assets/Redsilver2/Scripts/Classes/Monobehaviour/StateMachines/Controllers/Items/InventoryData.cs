using RedSilver2.Framework.Items;
using System.Collections.Generic;
using UnityEngine;

namespace RedSilver2.Framework.Inventories
{
    public class InventoryData : ScriptableObject {

        [SerializeField] private List<Item> defaultItems;
        private static readonly Dictionary<InventoryData, Inventory> inventoryInstances = new Dictionary<InventoryData, Inventory>();


        public void Register(Inventory inventory) {
            if (inventoryInstances == null || inventory == null) return;
            else if(!inventoryInstances.ContainsKey(this)) inventoryInstances?.Add(this, null);

            if (inventoryInstances[this] == null) 
                inventoryInstances.Add(this, inventory);
        }

        public void Unregister() {
            if (inventoryInstances == null) return;
            else if (inventoryInstances.ContainsKey(this)) inventoryInstances?.Remove(this);
        }

        public Inventory GetInventory() {
            if (inventoryInstances == null) return null;
            return inventoryInstances.ContainsKey(this) ? inventoryInstances[this] : null;
        }
    }
}