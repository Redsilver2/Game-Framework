using RedSilver2.Framework.Inputs;
using RedSilver2.Framework.Inventories;
using RedSilver2.Framework.Items;
using UnityEngine.Events;
using UnityEngine;

namespace RedSilver2.Framework.Player.Inventories
{
    [System.Serializable]
    public class ItemSlot
    {
        private bool isSelected;
        public readonly int Index;

        private KeyboardKey      keyboardKey;

        private EquippableItem item;
        private readonly PlayerInventory inventory;

        private readonly UnityEvent     onSelected, onDeselected;
        private readonly UnityEvent<EquippableItem> onItemChanged;

        public EquippableItem Item => item;

        public ItemSlot(PlayerInventory inventory, KeyboardKey key, int index)
        {
            this.item         = null;
            this.Index        = index;

            this.onSelected   = new UnityEvent();
            this.onDeselected = new UnityEvent();

            keyboardKey = key;
            isSelected = false;

            this.inventory = inventory;
        }

        public ItemSlot(PlayerInventory inventory, EquippableItem item, KeyboardKey key, int index)
        {
            this.item         = item;
            this.Index        = index;

            this.onSelected   = new UnityEvent();
            this.onDeselected = new UnityEvent();

            keyboardKey = key;
            isSelected  = false;

            this.inventory = inventory;
            SetItem(item);
        }

        public void AddOnSelectedListener(UnityAction action)
        {
            if (action != null) onSelected.AddListener(action);
        }
        public void RemoveOnSelectedListener(UnityAction action)
        {
            if (action != null) onSelected.RemoveListener(action);
        }
       
        public void AddOnDeselectedListener(UnityAction action)
        {
            if (action != null) onDeselected.AddListener(action);
        }
        public void RemoveOnDeselectedListener(UnityAction action)
        {
            if (action != null) onDeselected.RemoveListener(action);
        }

        public void SetItem(EquippableItem item) {
            if (inventory == null || !inventory.ContainsItem(item)) {
                return; 
            }
            else if (item != this.item) {
                this.item = item;
                onItemChanged?.Invoke(item);
            }
        }

        public void Update(ref int selectedIndex) {
            if (InputManager.GetKeyDown(keyboardKey)) {
                if (isSelected) Deselect();
                else {
                    selectedIndex = Index;
                    Select();
                }         
            }
            else if (selectedIndex == Index && !isSelected) Select();
            else if (selectedIndex != Index && isSelected) Deselect();
        }

        public void Select() {
            if(!isSelected) {
                isSelected = true;
                onSelected?.Invoke();
            }
        }
        public void Deselect()
        {
            if (isSelected) {
                isSelected = false;
                onDeselected?.Invoke();
            }
        }
    }
}
