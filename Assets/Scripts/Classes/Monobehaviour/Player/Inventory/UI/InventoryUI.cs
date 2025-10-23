using UnityEngine;

namespace RedSilver2.Framework.Player.Inventories.UI
{
    public abstract class InventoryUI : MonoBehaviour
    {
        [SerializeField] protected InventoryUINavigator navigator;
        protected abstract void Awake();
    }
}