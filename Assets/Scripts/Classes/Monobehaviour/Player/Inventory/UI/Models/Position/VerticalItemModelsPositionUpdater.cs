using UnityEngine;
using UnityEngine.Events;

namespace RedSilver2.Framework.Player.Inventories.UI
{

    public sealed class VerticalItemModelsPositionUpdater : SeperatedItemModelsPositionUpdater
    {

        protected sealed override void UpdateModelsPosition(int spacingIndex, bool isIndexBelowTarget, GameObject[] models) {

        }

        protected sealed override Vector3 GetNextModelPosition(int index, bool isLeftArray) {
            return (isLeftArray ? Vector3.left : Vector3.right) * (index + 1);
        }
    }
}
