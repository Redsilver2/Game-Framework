
using RedSilver2.Framework.Interactions.Items;
using RedSilver2.Framework.Items;
using RedSilver2.Framework.StateMachines.Controllers;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace RedSilver2.Framework.Player.Inventories
{
    public abstract class Inventory : MonoBehaviour
    {
        [SerializeField] private string inventoryName;
        [SerializeField] private Item[] defaultItems;

        [Space]
        [SerializeField] private bool allowDuplicateItems;

        [Space]
        [SerializeField] private ItemType[] allowedItemTypes;

        private UnityEvent onOpenUI, onCloseUI;
        private UnityEvent<Item> onItemAdded, onItemRemoved;

        private bool isUIOpened;
        private List<Item> items;


        public static Inventory Current { get; private set; }
        private readonly static List<Inventory> Instances = new List<Inventory>();
       
        public bool IsUIOpened => isUIOpened;

        protected virtual void Awake()
        {
            Instances.Add(this);
            items         = new List<Item>();

            onCloseUI     = new UnityEvent();
            onOpenUI      = new UnityEvent();

            onItemAdded   = new UnityEvent<Item>();
            onItemRemoved = new UnityEvent<Item>();

            isUIOpened    = false;

            AddOnOpenUIListener(OnOpenUI);
            AddOnCloseUIListener(OnCloseUI);

            AddOnItemAddedListener(OnItemAdded);
            AddOnItemRemovedListener(OnItemRemoved);
        }


        public void Open() {
            if (onOpenUI != null) onOpenUI.Invoke();
        }

        public void Close() {
            if (onCloseUI != null) onCloseUI.Invoke();
        }

        public void AddOnOpenUIListener(UnityAction action)
        {
            Debug.Log("On Open UI: " + onOpenUI + " | Action: " + action);

            if (onOpenUI != null && action != null)
                onOpenUI.AddListener(action);
        }
        public void RemoveOnOpenUIListener(UnityAction action)
        {
            if (action != null && onOpenUI != null)
                onOpenUI.RemoveListener(action);
        }

        public void AddOnCloseUIListener(UnityAction action)
        {
            if (action != null)
                onCloseUI?.AddListener(action);
        }
        public void RemoveOnCloseUIListener(UnityAction action)
        {
            if (action != null)
                onCloseUI?.RemoveListener(action);
        }

        public void AddOnItemAddedListener(UnityAction<Item> action)
        {
            if (action != null) 
                onItemAdded?.AddListener(action);
        }
        public void RemoveOnItemAddedListener(UnityAction<Item> action)
        {
            if (action != null)
                onItemAdded?.RemoveListener(action);
        }

        public void AddOnItemRemovedListener(UnityAction<Item> action)
        {
            if (action != null) 
                onItemRemoved?.AddListener(action);
           
        }

        public void RemoveOnItemRemovedListener(UnityAction<Item> action)
        {
            if (action != null) 
                onItemRemoved?.RemoveListener(action);
        }

        public virtual void AddItem(Item item) {
            AddItem(item, out bool isItemAdded);
        }

        public virtual void AddItem(Item item, out bool isItemAdded)
        {
            isItemAdded = false;

            if (items == null || item == null || allowedItemTypes == null || !allowedItemTypes.Contains(item.Type))
                return;

            if (allowDuplicateItems) {
                if (items.Count == 0 || ContainsDuplicate(item)) items.Add(item);
            }
            else if (!Contains(item) && !ContainsDuplicate(item)) items.Add(item);
            else {
                isItemAdded = false; return;
            }

            isItemAdded = Contains(item);
            if (isItemAdded == true) onItemAdded?.Invoke(item);
        }

        public void RemoveItem(Item item)
        {
            RemoveItem(item, out bool isItemRemoved);
        }


        protected virtual void RemoveItem(Item item, out bool isItemRemoved)
        {
            isItemRemoved = false;
            if (items == null || item == null) return;

            if (items.Contains(item)) {
                items.Remove(item);
                isItemRemoved = !Contains(item);
                if (isItemRemoved == true) onItemRemoved?.Invoke(item);
            }
        }

        public void EnableUI()
        {
            PlayerController.Disable();
            CameraController.Disable();

            gameObject.SetActive(true);
            isUIOpened = true;
        }

        public void DisableUI()
        {
            gameObject.SetActive(false);
            isUIOpened = false;

            PlayerController.Enable();
            CameraController.Enable();
        }

        protected virtual void OnOpenUI()
        {
            Debug.Log("Open Inventory UI");
            EnableUI();
        }

        protected virtual void OnCloseUI() {
            Debug.Log("Close Inventory UI");
        }

        public int GetMaxHorizontalIndex()
        {
            if (items == null) return 0;
            return items.Count;
        }

        public bool ContainsDuplicate(Item item)
        {
            if (items == null || item == null) return false;
            return GetDuplicateCount(item) >= 1;
        }

        public bool ContainsDuplicate(string itemName)
        {
            return ContainsDuplicate(GetItem(itemName));
        }


        public int GetDuplicateCount(Item item)
        {
            if (items == null || item == null) return 0;
            return items.Where(x => x.GetType() == item.GetType()).Count();
        }

        public int GetDuplicateCount(string itemName)
        {
            return GetDuplicateCount(GetItem(itemName));
        }

        public bool Contains(Item item)
        {
            if (items == null || item == null) return false;
            return items.Contains(item);
        }

        public bool Contains(string itemName)
        {
            return Contains(GetItem(itemName));
        }

        public int GetHorizontalIndex(Item item)
        {
            if (items == null || item == null) return -1;

            for (int i = 0; i < items.Count; i++)
                if (items[i] == item) return i;

            return -1;
        }

        public int GetHorizontalIndex(string itemName)
        {
            return GetHorizontalIndex(GetItem(itemName));
        }

        public Item[] GetItems()
        {
            if (items == null) return new Item[0];
            return items.ToArray();
        }

        public Item GetItem(int index)
        {
            if (items == null || index < 0 || index >= items.Count) return null;
            return items[index];
        }

        public Item GetItem(string itemName)
        {
            if (items == null) return null;

            var results = items.Where(x => x != null).Where(x => x.name.ToLower() == itemName.ToLower());
            if (results.Count() > 0) return results.First();

            return null;
        }


        protected abstract void OnItemAdded(Item item);
        protected abstract void OnItemRemoved(Item item);

        public bool IsCurrent(Inventory inventory)
        {
            if (Current == null) return true;
            return Current.Equals(inventory);
        }

        public static void SetCurrent(int index) {
            SetCurrent(Get(index));
        }

        public static void SetCurrent(string name) {
            SetCurrent(Get(name));
        }
        public static void SetCurrent(Transform transform) {
            SetCurrent(Get(transform));
        }


        public static void SetCurrent(Inventory inventory)
        {
            Disable();
            Current = inventory;
            Enable();
        }

        public static void Enable()
        {
            SetEnabledState(true);
        }
        public static void Disable()
        {
            SetEnabledState(false);
        }

        private static void SetEnabledState(bool isEnabled)
        {
            if (Current != null) Current.enabled = isEnabled;
        }

        public static Inventory Get(string inventoryName)
        {
            if (Instances == null || string.IsNullOrEmpty(inventoryName)) return null;

            var results = Instances.Where(x => x != null)
                                   .Where(x => !string.IsNullOrEmpty(x.inventoryName))
                                   .Where(x => x.inventoryName.ToLower().Equals(inventoryName.ToLower()));

            return results.Count() > 0 ? results.First() : null;
        }

        public static Inventory Get(int index)
        {
            if (Instances == null || Instances.Count == 0) return null;
            return Instances[index];
        }

        public static Inventory Get(Transform transform) {
            if (Instances == null || Instances.Count == 0) return null;
            return Instances.Where(x => x != null).Where(x => x.transform.Equals(transform)).FirstOrDefault();
        }

        public static Inventory[] GetInventoriesWithItem(Item item)
        {
            if (Instances == null) return null;
            return Instances.Where(x => x != null)
                            .Where(x => x.Contains(item))
                            .ToArray();
        }
    }
}
