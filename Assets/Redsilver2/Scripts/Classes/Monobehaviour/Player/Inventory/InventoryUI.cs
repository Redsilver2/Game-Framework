
using RedSilver2.Framework.Items;
using RedSilver2.Framework.UI;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace RedSilver2.Framework.Inventories {
    public class InventoryUI : MonoBehaviour {
        [SerializeField] private bool canWrapIndex;

        [Space]
        [SerializeField] private Inventory inventory;

        private int rowIndex;
        private int columnIndex;

        private UnityEvent<Item> onItemSelected;
        private UnityEvent onUpdateStarted, onUpdated, onFinished;

        private bool isUpdating;
        private IEnumerator updater;

        public int       RowIndex    => rowIndex;
        public int       ColumnIndex => columnIndex;

        public bool      IsUpdating => isUpdating;
        public Inventory Inventory => inventory;


        protected virtual void Awake()
        {
            onItemSelected  = new UnityEvent<Item>();
            onUpdateStarted = new UnityEvent();
          
            onUpdated      = new UnityEvent();
            onFinished     = new UnityEvent();

            isUpdating = false;
            inventory?.AddOnItemsUpdatedListner(OnItemsUpdated);
           
            AddOnUpdateStartedListener(OnStarted);
            AddOnUpdateFinishedListener(OnFinished);
        }

        public void StartUIUpdate(float enableTime)
        {
            if (!isUpdating) {
                FinishUIUpdate();
                updater = UIUpdate(enableTime);
                StartCoroutine(updater);
            }
        }

        public void FinishUIUpdate() {
            if(updater != null) {
                StopCoroutine(updater);
                updater = null;
            }

            if(isUpdating) onFinished?.Invoke();
        }

        private IEnumerator UIUpdate(float enableTime) {
            onUpdateStarted.Invoke();
            yield return new WaitForSeconds(enableTime);

            while (isUpdating) {
                onUpdated?.Invoke();
                yield return null;
            }
        }

        protected virtual void OnItemsUpdated(Item[][] items) {
            ClampIndexes();
        }

        protected void ClampIndexes() {
            if (inventory == null) return;
            int maxIndex = inventory.GetRowCount() - 1;

            if (rowIndex < 0)              rowIndex = canWrapIndex ? maxIndex : 0;
            else if (rowIndex > maxIndex)  rowIndex = canWrapIndex ? 0 : maxIndex;

            ClampIndexes(rowIndex);
            onItemSelected?.Invoke(GetItemSelected());
        }

        private void ClampIndexes(int rowIndex) {
            if (inventory == null) return;
            int maxIndex = inventory.GetColumnCount(rowIndex) - 1;

            if (columnIndex < 0)             columnIndex = canWrapIndex ?     maxIndex : 0;
            else if (columnIndex > maxIndex) columnIndex = canWrapIndex ? 0 : maxIndex;
        }

        protected virtual void OnUpdated()
        {
            if      (GameUIController.GetNavigateDownState(this))  DecrementRowIndex();
            else if (GameUIController.GetNavigateUpState(this))    IncrementRowIndex();
            else if (GameUIController.GetNavigateRightState(this)) IncrementColumnIndex();
            else if (GameUIController.GetNavigateLeftState(this))  DecrementColumnIndex();
        }

        protected virtual void OnStarted() {
            isUpdating = true;
        }

        protected virtual void OnFinished() {
            isUpdating = false;
        }

        private void IncrementRowIndex() {
            rowIndex++;
            ClampIndexes();
        }

        private void DecrementRowIndex() {
            rowIndex--;
            ClampIndexes();
        }


        private void IncrementColumnIndex() {
            columnIndex++;
            ClampIndexes();
        }

        private void DecrementColumnIndex() {
            columnIndex--;
            ClampIndexes();
        }

        public void AddOnUpdateStartedListener(UnityAction action)
        {
            if(action != null) onUpdateStarted?.AddListener(action);  
        }
        public void RemoveOnUpdateStartedListener(UnityAction action)
        {
            if (action != null) onUpdateStarted?.RemoveListener(action);
        }

        public void AddOnUpdatedListener(UnityAction action)
        {
            if (action != null) onUpdated?.AddListener(action);
        }
        public void RemoveOnUpdatedListener(UnityAction action)
        {
            if (action != null) onUpdated?.RemoveListener(action);
        }

        public void AddOnUpdateFinishedListener(UnityAction action)
        {
            if (action != null) onFinished?.AddListener(action);
        }
        public void RemoveOnUpdateFinishedListener(UnityAction action)
        {
            if (action != null) onFinished?.RemoveListener(action);
        }


        private Item GetItemSelected() {
           if (inventory == null) return null;
            return inventory.GetItem(rowIndex, columnIndex);
        }
    }
}
