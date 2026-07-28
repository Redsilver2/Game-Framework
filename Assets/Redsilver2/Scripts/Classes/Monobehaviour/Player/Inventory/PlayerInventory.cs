using RedSilver2.Framework.Inputs;
using RedSilver2.Framework.Items;
using RedSilver2.Framework.Player.Inventories;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace RedSilver2.Framework.Inventories {
    public class PlayerInventory : Inventory {

        [Space]
        [SerializeField][Range(1, 10)] private uint maxItemsSlotCount;

        [Space]
        [SerializeField] private bool canAutomaticallyAddInSlot;
        [SerializeField] private bool canAutomaticallyEquipItem;

        private int selectedSlotIndex;
        private bool canUpdateInputs;

        private EquippableItem itemEquipped;
        private IEnumerator itemEquippedUpdater;

        private List<ItemSlot> slots;
        private UnityEvent<ItemSlot> onSlotAdded, onSlotRemoved;

#if UNITY_EDITOR
        protected sealed override void OnValidate() {
            base.OnValidate();
            maxItemsSlotCount = (uint)Mathf.Clamp(maxItemsSlotCount, 1, uint.MaxValue);
        }
#endif

        protected override void Awake() {
            base.Awake();
            slots = new List<ItemSlot>();
            selectedSlotIndex = 0;
        }

        private void Start()
        {
            AddSlot();
            AddSlot();
        }

        private void Update() {
            if (canUpdateInputs) {
                for (int i = 0; i < slots.Count; i++)
                    slots[i]?.Update(ref selectedSlotIndex);

                if (InputManager.GetKeyDown(KeyboardKey.G)) {
                    itemEquipped?.RemoveFromInventory();
                    canUpdateInputs = false;
                }
            }
            else {
                if (!ContainsItem(itemEquipped) || itemEquipped == null) {
                    itemEquipped    = null;
                    canUpdateInputs = true;
                }
            }

                
        }

        private void AddSlot() {
            if (slots == null || slots.Count >= maxItemsSlotCount) return;
            ItemSlot slot = GetItemSlot(slots.Count, GetSlotKey(slots.Count));
         
            slot?.AddOnSelectedListener(() => { EquipItem(slot.Item); });
            slots?.Add(slot);

            onSlotAdded?.Invoke(slot);
        }

        public void RemoveSlot() {
            if (slots == null) return;
            RemoveSlot(slots.Count - 1);
        }

        public void RemoveSlot(int slotIndex) {
            if(slots == null || slotIndex < 0 || slotIndex >= slots.Count) return;
          
            ItemSlot slot = slots[slotIndex];
            slot?.Deselect();

            slots?.RemoveAt(slotIndex);
            onSlotRemoved?.Invoke(slot);
        }

        protected sealed override void OnItemAdded(Item item) {
            base.OnItemAdded(item);
            if (canAutomaticallyAddInSlot) UpdateItemFromSlot(item as EquippableItem);
        }

        protected sealed override void OnItemRemoved(Item item) {
            base.OnItemRemoved(item);
            RemoveItemFromSlot(item as EquippableItem);
        }

        public void EquipItem(EquippableItem item)
        {
            if (item == itemEquipped || (item != null && !ContainsItem(item))) return;
           
            if (itemEquippedUpdater != null)
            {
                StopCoroutine(itemEquippedUpdater);
                itemEquippedUpdater = null;
            }

            itemEquippedUpdater = UpdateItemEquipped(item);
            StartCoroutine(itemEquippedUpdater);
        }

        public void UpdateItemFromSlot(EquippableItem item) {
            int index = GetSlotIndex(null);

            if(index >= 0) {
                UpdateItemFromSlot(item, index);
                if (canAutomaticallyEquipItem && itemEquipped == null) EquipItem(item);
            }
        }

        public void UpdateItemFromSlot(EquippableItem item, int slotIndex) {
            if (slots == null || slotIndex < 0 || slotIndex >= slots.Count || slots[slotIndex] == null) return;
            else if (slots[slotIndex].Item != item && ContainsItem(item)) {
                if(slotIndex == selectedSlotIndex) EquipItem(item);
                slots[slotIndex]?.SetItem(item);
            }
        }

        public void RemoveItemFromSlot(EquippableItem item) {
            RemoveItemFromSlot(GetSlotIndex(item));
        }

        public void RemoveItemFromSlot(int slotIndex) {
            if (slots == null || slotIndex < 0 || slotIndex >= slots.Count) return;
            else if (slotIndex == selectedSlotIndex) EquipItem(null);

            slots[slotIndex]?.SetItem(null);
           
        }

        public bool IsInSlot(EquippableItem item) {
            return GetSlotIndex(item) >= 0;
        }

        public bool IsSlotFull(int slotIndex) {
            if (slots == null || slotIndex < 0 || slotIndex >= slots.Count) return true;
            return slots[slotIndex] != null;
        }

        public bool IsSlotsFull() {
            if (slots == null) return true;

            for (int i = 0; i < slots.Count; i++)
            {
                if (slots[i] == null) continue;
                else if (slots[i].Item == null) return false;
            }

            return true;
        }

        public int GetSlotIndex(EquippableItem item) {
            if (slots == null) return -1;

            for(int i = 0; i < slots.Count; i++){
                if      (slots[i] == null)      continue;
                else if (slots[i].Item == item) return i;
            }

            return -1;
        }

        public int GetSlotCount()
        {
            if(slots == null) return 0;
            return slots.Count;
        }

        protected virtual ItemSlot GetItemSlot(int index, KeyboardKey key) {
            return new ItemSlot(this, key, index);
        }

        private KeyboardKey GetSlotKey(int index) {
            if (index < 9) return (KeyboardKey.Alpha0 + (index + 1));
            return KeyboardKey.Alpha0;
        }


        private IEnumerator UpdateItemEquipped(EquippableItem item) {
            itemEquipped?.UnEquip();

            while (itemEquipped != null) {
                if (!itemEquipped.IsEquipped) break;
                yield return null;
            }

            item?.Equip();
            itemEquipped = item;
        }
    }
}
