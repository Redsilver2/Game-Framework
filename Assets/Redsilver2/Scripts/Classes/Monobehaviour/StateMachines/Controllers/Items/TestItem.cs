using UnityEngine;

namespace RedSilver2.Framework.Items {
    public class TestItem : EquippableItem
    {
        protected sealed override ItemType GetItemType() {
            return ItemType.LightSource;
        }
    }
}
