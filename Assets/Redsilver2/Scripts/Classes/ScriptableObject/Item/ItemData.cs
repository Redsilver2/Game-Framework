
using RedSilver2.Framework.Interactions;
using UnityEngine;

namespace RedSilver2.Framework.Items
{
    public abstract class ItemData : ScriptableObject, IPickableInteractable
    {
        [SerializeField] private string itemName;
        [SerializeField] private string description;

        [Space]
        [SerializeField] private Sprite icon;
        [SerializeField] private GameObject model;

        public string GetName()        => itemName;
        public string GetDescription() => description;

        public GameObject GetModel()   => model;
        public Sprite GetIcon()        => icon;

        public abstract ItemType GetItemType();
    }
}
