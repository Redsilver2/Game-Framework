using System.Linq;
using UnityEngine;

namespace RedSilver2.Framework.Player.Inventories.UI
{
    public class ItemModelRotationUpdater : ItemModelUpdater
    {
        [Space]
        [SerializeField] private float rotationSpeed;

        protected sealed override void UpdateModels(SimpleInventoryUINavigator navigator)
        {
            if (navigator != null) UpdateModelsRotation(navigator.Models);
        }

        protected sealed override void UpdateModels(VerticalInventoryUINavigator navigator)
        {
            if(navigator != null) UpdateModelsRotation(navigator.Models);
        }

        protected virtual void UpdateModelsRotation(GameObject[] models)
        {
            if(navigator == null || models == null || models.Length == 0) return;

            foreach(GameObject model in models.Where(x => x != null)) {
                 UpdateModelRotation(model);
            }
        }

        protected virtual void UpdateModelsRotation(GameObject[,] models)
        {
            if (navigator == null ||  models == null || models.GetLength(0) == 0 || models.GetLength(1) == 0) return;

            for (int i = 0; i < models.GetLength(0); i++)
                for (int j = 0; j < models.GetLength(1); j++)
                    UpdateModelRotation(models[i, j]);
        }

        protected void UpdateModelRotation(GameObject model)
        {
            if(model == null) return;
            Transform transform = model.transform;
            transform.localEulerAngles += Time.deltaTime * Vector3.up * rotationSpeed;
        }
    }
}
