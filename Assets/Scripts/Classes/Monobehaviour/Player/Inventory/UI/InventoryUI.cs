using UnityEngine;

namespace RedSilver2.Framework.Player.Inventories.UI
{
    public abstract class InventoryUI : MonoBehaviour
    {
        protected InventoryUINavigator navigator;

        protected virtual void Awake()
        {
            navigator = GetComponent<InventoryUINavigator>();

            if(navigator == null)
                navigator = GetInventoryUINavigator(transform.parent);
        }

        private InventoryUINavigator GetInventoryUINavigator(Transform parent)
        {
            InventoryUINavigator navigator;
            if (parent == null) return null;


            navigator = parent.GetComponent<InventoryUINavigator>();
            if(navigator == null) return GetInventoryUINavigator(parent.parent);
            
            return navigator;
        }
    }
}