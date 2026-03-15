using UnityEngine;

namespace RedSilver2.Framework.Interactions
{
    public interface IPickableInteractable  {
        string GetName();
        string GetDescription();

        GameObject GetModel();
        Sprite GetIcon();
    }
}