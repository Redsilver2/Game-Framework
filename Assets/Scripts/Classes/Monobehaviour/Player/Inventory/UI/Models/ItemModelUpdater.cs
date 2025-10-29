using RedSilver2.Framework.Interactions.Items;
using System.Collections;
using System.Linq;
using UnityEngine;

namespace RedSilver2.Framework.Player.Inventories.UI
{
    public abstract class ItemModelUpdater : InventoryUI
    {
        private IEnumerator updateModels;

        protected override void Awake()
        {
            if (navigator != null) {
                navigator.AddOnEnableListener(OnNavigatorEnable);
                navigator.AddOnDisableListener(OnNavigatorDisable);
            }
        }

        private void OnDestroy()
        {
            OnNavigatorDisable();
        }

        private void OnDisable()
        {
            OnNavigatorDisable();
        }

        private void OnNavigatorEnable()
        {
            OnNavigatorDisable();

            if (navigator is SimpleInventoryUINavigator)
                updateModels = UpdateNavigatorModels(navigator as SimpleInventoryUINavigator);
            else if (navigator is VerticalInventoryUINavigator)
                updateModels = UpdateNavigatorModels(navigator as VerticalInventoryUINavigator);

            if(updateModels != null) { StartCoroutine(updateModels); }               
        }

        private void OnNavigatorDisable()
        {
            if (updateModels != null) StopCoroutine(updateModels);
            updateModels = null;
        }



        private IEnumerator UpdateNavigatorModels(SimpleInventoryUINavigator navigator)
        {
            while (navigator != null)  {
                SetParent(navigator.ModelParentTransform, navigator.Models);
                UpdateModels(navigator);
                yield return null;
            }
        }

        private IEnumerator UpdateNavigatorModels(VerticalInventoryUINavigator navigator)
        {
            while(navigator != null) {
                SetParent(navigator.ModelParentTransform, navigator.Models);
                UpdateModels(navigator);
                yield return null;
            }
        }

        protected abstract void UpdateModels(SimpleInventoryUINavigator navigator);
        protected abstract void UpdateModels(VerticalInventoryUINavigator navigator);

        private void SetParent(Transform parent, GameObject[] models)
        {
            if (navigator == null || models == null) return;

            for (int i = 0; i < models.Length; i++)
                SetParent(parent, models[i]);
        }

        private void SetParent(Transform parent, GameObject[,] models)
        {
            if (parent == null || models == null) return;

            for (int i = 0; i < models.GetLength(0); i++)
                for (int j = 0; j < models.GetLength(1); j++)
                    SetParent(parent, models[i, j]);
        }

        private void SetParent(Transform parent, GameObject gameObject)
        {
            if(parent == null || gameObject == null) return;
            gameObject.transform.SetParent(parent);
            gameObject.SetActive(true);
        }
    }
}
