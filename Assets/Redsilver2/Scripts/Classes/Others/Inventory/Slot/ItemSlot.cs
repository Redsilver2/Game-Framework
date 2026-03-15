using RedSilver2.Framework.Inputs;
using RedSilver2.Framework.Interactions.Items;
using UnityEngine;
using UnityEngine.Events;

namespace RedSilver2.Framework.Player.Inventories
{
    [System.Serializable]
    public class ItemSlot
    {
        public  readonly int     index;
        private KeyboardKey      keyboardKey;

        private EquippableItem item;
        private UnityEvent     onSelected, onDeselected, onUpdated;

        public EquippableItem Item => item;

        public ItemSlot(KeyboardKey key, int index)
        {
            this.item         = null;
            this.index        = index;

            this.onSelected   = new UnityEvent();
            this.onDeselected = new UnityEvent();
            this.onUpdated    = new UnityEvent();

            keyboardKey = key;
        }

        public ItemSlot(EquippableItem item, KeyboardKey key, int index)
        {
            this.item         = item;
            this.index        = index;

            this.onSelected   = new UnityEvent();
            this.onDeselected = new UnityEvent();
            this.onUpdated    = new UnityEvent();

            keyboardKey = key;
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

        public void AddOnUpdatedListener(UnityAction action)
        {
            if (action != null) onUpdated?.AddListener(action);
        }
        public void RemoveOnUpdatedListener(UnityAction action)
        {
            if (action != null) onUpdated?.RemoveListener(action);
        }

     
        public virtual bool TryUpdateShortcut(int maxSlots, int selectedSlotIndex, out bool isSelecting) {
            isSelecting = true;

            Debug.LogWarning(InputManager.GetKeyDown(keyboardKey)  + " " + index);

            if (InputManager.GetKeyDown(keyboardKey))
            {
                if (selectedSlotIndex == index)
                    isSelecting = false;

                return true;
            }

            return false;
        }

        public virtual void Update(EquippableItem item)
        {
            this.item = item;
            onUpdated?.Invoke();
        }
        public void Select()
        {
            onSelected?.Invoke();   
        }
        public void Deselect()
        {
            onDeselected?.Invoke();
        }
    }
}
