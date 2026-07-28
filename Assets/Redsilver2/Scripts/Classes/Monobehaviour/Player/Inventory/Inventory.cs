using RedSilver2.Framework.Items;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace RedSilver2.Framework.Inventories
{
    public class Inventory : MonoBehaviour
    {
        [Space]
        [SerializeField] private InventoryType type;

        [Space]
        [SerializeField] private ItemType[] itemTypesAllowed;

        [Space]
        [SerializeField] private Transform itemParent;

        [Space]
        [SerializeField] private bool allowDuplicateItem = true;

        [Space]
        [SerializeField] private uint maxRowCount;
        [SerializeField] private uint maxColumnCount;   


        private List<List<Item>>     items;
        private UnityEvent<Item[][]> onItemsUpdated;
        private UnityEvent<Item>     onItemAdded, onItemRemoved;

#if UNITY_EDITOR
        protected virtual void OnValidate() {
            ValidateRowAndColumnCount();
        }

        private void ValidateRowAndColumnCount()
        {
            if (type == InventoryType.Vertical) {
                maxRowCount    = uint.MaxValue;
                maxColumnCount = 1;
            }
            else if (type == InventoryType.Horizontal) {
                maxRowCount    = 1;
                maxColumnCount = uint.MaxValue;
            }
            else {
                maxRowCount    = (uint)Mathf.Clamp(maxRowCount, 1, 10);
                maxColumnCount = (uint)Mathf.Clamp(maxColumnCount, 1, 10);
            }
        }
#endif


        protected virtual void Awake() {
            items = new List<List<Item>>();

            onItemAdded    = new UnityEvent<Item>();
            onItemRemoved  = new UnityEvent<Item>();
            onItemsUpdated = new UnityEvent<Item[][]>();
            
            AddOnItemAddedListner(OnItemAdded);
            AddOnItemRemovedListner(OnItemRemoved);
        }



        protected virtual void OnItemAdded(Item item)
        {
            if (item != null) item.transform.SetParent(itemParent);
        }

        protected virtual void OnItemRemoved(Item item)
        {

        }

        public virtual void AddItem(Item item) {
            if (items == null || item == null) { return; }
            else if (type == InventoryType.Vertical)   { AddVerticalItem(item); }
            else if (type == InventoryType.Horizontal) { AddHorizontalItem(item); }
        }

        public void RemoveItem(Item item) {
            if (items == null || item == null) return;

            int rowIndex    = GetRowIndex(item);
            int columnIndex = GetColumnIndex(item);

            if (TryGetIndexes(item, out int row, out int column)) {
                items[row][column] = null;

                onItemRemoved?.Invoke(item);
                onItemsUpdated?.Invoke(GetItems());
            }
        }



        private void AddVerticalItem(Item item) {

            if (items == null) return;
            else if (!ContainsItem(item)) {

                items?.Add(new List<Item>());
                items[items.Count - 1].Add(item);

                onItemAdded?.Invoke(item);
                onItemsUpdated?.Invoke(GetItems());
            }
        }

        private void AddHorizontalItem(Item item) {
            if (items == null) return;
            else if (!ContainsItem(item)) {
                if (items.Count != 1) items.Add(new List<Item>());
                items[items.Count - 1].Add(item);

                onItemAdded?.Invoke(item);
                onItemsUpdated?.Invoke(GetItems());
            }
        }

        public void AddOnItemAddedListner(UnityAction<Item> action)
        {
            if(action != null) onItemAdded?.AddListener(action);
        }
        public void RemoveOnItemAddedListner(UnityAction<Item> action)
        {
            if (action != null) onItemAdded?.RemoveListener(action);
        }

        public void AddOnItemRemovedListner(UnityAction<Item> action)
        {
            if (action != null) onItemRemoved?.AddListener(action);
        }
        public void RemoveOnItemRemovedListner(UnityAction<Item> action)
        {
            if (action != null) onItemRemoved?.RemoveListener(action);
        }

        public void AddOnItemsUpdatedListner(UnityAction<Item[][]> action)
        {
            if (action != null) onItemsUpdated?.AddListener(action);
        }
        public void RemoveOnItemsUpdatedListner(UnityAction<Item[][]> action)
        {
            if (action != null) onItemsUpdated?.RemoveListener(action);
        }

        public bool IsRowFull(int row) {
            if (items == null || row < 0 || row >= items.Count || items[row] == null) return true;
            Item[] copies = items[row].ToArray();

            for (int i = 0; i < copies.Length; i++)
                if (!IsColumnFull(row, i)) return false;

            return true;
        }

        public bool IsColumnFull(int row, int column) {
            if (items == null || row < 0 || row >= items.Count || items[row] == null) return true;
            Item[] copies = items[row].ToArray();

            if (column < 0 || column >= copies.Length) return false;
            Item item = copies[column];

            return item != null;
        }

        public bool IsFull()
        {
            if(items == null) return true;

            for(int i = 0; i < items.Count; i++) 
                if(!IsRowFull(i)) return false;
            return true;
        }

        public bool ContainsItem(Item item) {
            if (items == null) return false;

            for (int i = 0; i < items.Count; i++) {
                if (items[i] == null) continue;
                else if (items[i].Contains(item)) {
                    return true;
                }
            }

            return false;
        }

        public bool ContainsItem(int row, Item item) {
            bool containsItem = GetItems(row).Contains(item);

            if (containsItem) return false;
            return true;
        }

        public bool ContainsItem(int row, int column, Item item) {
            bool containsItem = GetItem(row, column) == item;

            if (containsItem) return false;
            return true;
        }

        public bool TryGetIndexes(Item item, out int row, out int column) {
            row = GetRowIndex(item);
            column = GetColumnIndex(item);

            if (row < 0 || column < 0) return false;
            return true;
        }

        private int GetRowIndex(Item item) {
            List<List<Item>> copies = items;
            if (copies == null) return -1;

            for (int i = 0; i < copies.Count; i++) {
                if (copies[i] == null) continue;
                else if (copies[i].Contains(item)) return i;
            }

            return -1;
        }

        private int GetColumnIndex(Item item)
        {
            List<List<Item>> copies = items;
            if (copies == null) return -1;

            for (int i = 0; i < copies.Count; i++) {
                if (copies[i] == null) continue;
                else if (copies[i].Contains(item)) return copies[i].IndexOf(item);
            }

            return -1;
        }

        public int GetRowCount() {
            if (items == null) return -1;
            return items.Count;
        }

        public int GetColumnCount(int row)
        {
            if (items == null || row < 0 || row >= items.Count) return -1;
            else if (items[row] == null) return 0;
            else return items[row].Count;
        }

        public Item GetItem(int row, int column) {
            Item[] rowItems = GetItems(row);
            if (rowItems == null || column < 0 || column >= rowItems.Length) return null;
            return rowItems[column];
        }

        public Item[] GetItems(int row)
        {
            List<Item> results = new List<Item>();
            if (items == null || row < 0 || row >= items.Count || items[row] == null) return results.ToArray();

            Item[] copies = items[row].ToArray();
            foreach (Item item in copies) results?.Add(item);

            return results.ToArray();
        }

        public Item[][] GetItems() {
            if(items == null) return new Item[0][];
            Item[][] results = new Item[items.Count][];

            for(int i = 0; i < results.Length; i++) {
                if(items[i] == null) continue;
                results[i] = items[i].ToArray();
            }

            return results;
        }
    }
}
