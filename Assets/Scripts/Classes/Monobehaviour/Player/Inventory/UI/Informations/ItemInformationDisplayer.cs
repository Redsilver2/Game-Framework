using RedSilver2.Framework.Interactions.Items;
using UnityEngine;

namespace RedSilver2.Framework.Player.Inventories.UI
{
    public abstract class ItemInformationDisplayer : InventoryUI
    {
        [Space]
        [SerializeField] protected string nullErrorMessage;

        protected override void Awake()
        {
            base.Awake();
            SetNavigatorEvents();
        }

        private void SetNavigatorEvents()
        {
            if (navigator == null) return;
             navigator.AddOnHorizontalIndexChangedListener(OnHorizontalIndexChanged);
             navigator.AddOnInventoryCloseUIListener(OnInventoryUIClose);
             navigator.AddOnInventoryOpenUIListener(OnInventoryUIOpen);

            if (navigator is VerticalInventoryUINavigator)
               (navigator as VerticalInventoryUINavigator).AddOnVerticalIndexChangedListener(OnVerticalIndexChanged);
        }

        private void SetInventoryEvents(Inventory inventory)
        {
            if(inventory == null) return;
        }

        private void OnInventoryUIClose()
        {
            DisplayItemInformation(string.Empty);
        }

        private void OnInventoryUIOpen() {
            DisplayItemInformation();
        }


        private void OnHorizontalIndexChanged(int horizontalIndex) {
            Debug.LogWarning(horizontalIndex);
            DisplayItemInformation();
        }

        private void OnVerticalIndexChanged(int verticalIndex) {
            DisplayItemInformation();
        }

        private void DisplayItemInformation() 
        {
            if (navigator == null) return;
            DisplayItemInformation(navigator.GetSelectedItem());
        }

        private void DisplayItemInformation(Item item) 
        {
            if (item != null)
                DisplayItemInformation(item.GetData() as ItemData);
            else
                DisplayNullMessage();
        }

        protected void DisplayNullMessage() {
            DisplayItemInformation(nullErrorMessage);
        }

        protected abstract void DisplayItemInformation(ItemData data);
        protected abstract void DisplayItemInformation(string message);
    }
}
