using System.Collections;
using UnityEngine;

namespace RedSilver2.Framework.Player.Inventories.UI
{
    public class ItemModelPositionUpdater : ItemModelUpdater
    {
        [Space]
        [SerializeField] protected float modelOffsetX;
        [SerializeField] protected float modelOffsetY;

        [Space]
        [SerializeField] protected float modelPositionLerpSpeed;

        protected override void UpdateModels(SimpleInventoryUINavigator navigator) 
        {
            if (navigator != null) {
                UpdateModelsPosition(navigator.HorizontalIndex, navigator.Models);
            }
        }

        protected override void UpdateModels(VerticalInventoryUINavigator navigator)
        {
            if (navigator != null) {
                UpdateModelsPosition(navigator.VerticalIndex, navigator.HorizontalIndex, navigator.Models);
            }
        }

        protected virtual void UpdateModelsPosition(int horizontalIndex, GameObject[] models)
        {
            if(models == null || models.Length == 0) return;

            for (int i = 0; i < models.Length; i++)
                UpdateModelPosition(i, models[i]);
        }

        protected virtual void UpdateModelsPosition(int vertical, int horizontalIndex, GameObject[,] models)
        {
            if (models == null || models.GetLength(0) == 0 || models.GetLength(1) == 0) return;

            for (int i = 0; i < models.GetLength(0); i++) 
                for (int j = 0; j < models.GetLength(1); j++)
                    UpdateModelPosition(i, j, models[i, j]);
        }

        protected virtual void UpdateModelPosition(int horizontalIndex, GameObject model)
        {
            UpdateModelPosition(0, horizontalIndex, model);
        }

        protected virtual void UpdateModelPosition(int verticalIndex, int horizontalIndex, GameObject model) 
        {
            Transform transform;
            if (model == null) return;

            transform = model.transform;
            transform.localPosition = Vector3.Lerp(transform.localPosition, -Vector3.up      * (verticalIndex   * modelOffsetY) + 
                                                                             Vector3.right   * (horizontalIndex * modelOffsetX), 
                                                                             Time.deltaTime  * modelPositionLerpSpeed);
        }
    }

}
