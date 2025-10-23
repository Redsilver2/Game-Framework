using RedSilver2.Framework.Interactions.Items;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace RedSilver2.Framework.Player.Inventories.UI
{
    public sealed class ItemModelsDisplayer : InventoryUI
    {
        private List<GameObject> visibleItemModels;
        private static Dictionary<string, Queue<GameObject>> hiddenItemModels = new Dictionary<string, Queue<GameObject>>();

        private const string ITEM_DISPLAYER_LAYER_NAME = "Item Displayer";

        public GameObject[] VisibleItemModels
        {
            get
            {
                if(visibleItemModels == null) return null;
                return visibleItemModels.ToArray();
            }
        }

        protected sealed override void Awake()
        {
            visibleItemModels = new List<GameObject>();

            if(navigator != null) {
                navigator.AddOnEnableListener(ShowVisibleItemModels);
                navigator.AddOnDisableListener(ClearVisibleItemModels);
                SetInventoryEvents(navigator.Inventory);
            }
        }

        private void SetInventoryEvents(Inventory inventory) {
            if(inventory != null) {
                inventory.AddOnItemAddedListener(RefreshVisibleItems);
                inventory.AddOnItemRemovedListener(RefreshVisibleItems);
            }
        }

        private void RefreshVisibleItems(Item item)
        {
            if (item != null && navigator != null)
            {
                if (navigator.enabled) {
                    ClearVisibleItemModels();
                    ShowVisibleItemModels();
                }
            }
        }

        private void ShowVisibleItemModels() 
        {
            if (navigator == null) return;

            ShowItemModel(navigator.Inventory);
        }


        private void ShowItemModel(Inventory inventory)
        {
            if (inventory == null) return;

            if(inventory is SimpleInventory)
                ShowItemModel(inventory as SimpleInventory);
            else
                ShowItemModel(inventory as ComplexInventory);
          
        }

        private void ShowItemModel(SimpleInventory inventory)
        {
            if (inventory == null) return;
                ShowItemModel(inventory.Items);
        }

        private void ShowItemModel(ComplexInventory inventory)
        {
            if (inventory == null) return;

            for (int i = 0; i < inventory.GetMaxVerticalIndex(); i++)
                 ShowItemModel(inventory.GetItems(i));
        }

        private void ShowItemModel(Item[] items)
        {
            if (visibleItemModels == null) visibleItemModels = new List<GameObject>();

            foreach (Item item in items) {
                if(item == null) { visibleItemModels.Add(null); continue; }
                ShowItemModel(item.GetData() as ItemData);
            }               
        }


        private void ShowItemModel(ItemData data)
        {
            if (data == null || hiddenItemModels == null) { visibleItemModels.Add(null); return; }

            GameObject model = data.Model;
            if (model == null) { visibleItemModels.Add(null); return; }

            string name = model.name.ToLower();
            if (!hiddenItemModels.ContainsKey(name)) hiddenItemModels.Add(name, new Queue<GameObject>());

            ShowItemModel(name, model);
        }

        private void ShowItemModel(string name, GameObject model)
        {
            if(string.IsNullOrEmpty(name)) { visibleItemModels.Add(null); return; }

            if (hiddenItemModels[name].Count > 0)
                visibleItemModels.Add(hiddenItemModels[name].Dequeue());
            else
                visibleItemModels.Add(CreateAndGetNewModel(model));

            ShowItemModel();
        }

        private void ShowItemModel()
        {
            if(visibleItemModels == null) return;
            
            if(visibleItemModels.Count > 0)
                visibleItemModels[visibleItemModels.Count - 1].SetActive(true);
        }

        private void ClearVisibleItemModels()
        {
            if(visibleItemModels == null || hiddenItemModels == null || visibleItemModels.Count == 0) return;

            foreach(GameObject model in visibleItemModels.Where(x => x != null)) {
              
                string name = model.name.ToLower();
                model.SetActive(false);

                model.layer = LayerMask.NameToLayer(ITEM_DISPLAYER_LAYER_NAME);

                if (!hiddenItemModels.ContainsKey(name)) hiddenItemModels.Add(name, new Queue<GameObject>());
                    hiddenItemModels[name].Enqueue(model);
            }

            visibleItemModels.Clear();
        }

        private GameObject CreateAndGetNewModel(GameObject model)
        {
            if(model == null) return null;

            GameObject copy = Instantiate(model);
            copy.name = model.name;
            copy.transform.SetLocalPositionAndRotation(Vector3.zero, transform.rotation);

            copy.layer = LayerMask.NameToLayer(ITEM_DISPLAYER_LAYER_NAME);
            copy.SetActive(false);

            Debug.Log(copy.name);
            return copy;
        }

    }
}
