using UnityEngine;

namespace RedSilver2.Framework.Inventories {
    public abstract class InventoryEvent : MonoBehaviour {
        private Inventory inventory;

        protected virtual void Awake() {
            inventory = GetComponent<Inventory>();
        }

        private void Start() {
            SetEvents(inventory, true);
        }

        private void OnEnable()  { SetEvents(inventory, true); }
        private void OnDisable() { SetEvents(inventory, false); }
        protected abstract void SetEvents(Inventory inventory, bool isAddingEvents);
    }
}