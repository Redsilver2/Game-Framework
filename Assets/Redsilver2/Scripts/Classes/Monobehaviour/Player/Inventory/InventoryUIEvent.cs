using UnityEngine;


namespace RedSilver2.Framework.Inventories
{
    public abstract class InventoryUIEvent : MonoBehaviour
    {
        private InventoryUI inventoryUI;

        protected virtual void Awake() {
            inventoryUI = GetComponent<InventoryUI>();
        }

        private void Start() {
            SetEvents(inventoryUI, true);
        }

        private void OnEnable() { SetEvents(inventoryUI, true); }
        private void OnDisable() { SetEvents(inventoryUI, false); }
        protected abstract void SetEvents(InventoryUI inventoryUI, bool isAddingEvents);

        protected Inventory GetInventory() { return inventoryUI != null ? inventoryUI.Inventory : null;  }
    }
}